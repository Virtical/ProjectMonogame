using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using TankMonogame.Shared.Interface;
using TankMonogame.Shared.Enums;
using TankMonogame.Model.QuadTreeAlgorithm;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TankMonogame.Model
{
    public class GameCycleModel : IGameplayModel
    {
	    public event EventHandler<GameplayEventArgs> Updated = delegate { };
 
        private Map map;
        private TankHull tankHull;
        private BarrelAndTower barrelAndTower;
        private List<Bullet> bullets;
        private List<Explosion> explosions;
        private double timeLastShoot = 0;

        private bool IsPossibleShoot = true;
        
        public BoxQT GetBox()
        {
            return IObject.GetBox(tankHull);
        }
        public void Initialize()
        {
            map = new Map(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, 64);

            tankHull = new TankHull
            {
                Pos = new Vector2(200, 350),
                ImageId = 1,
                Speed = 0,
                Angle = 0,
                RotationSpeed = 0,
                MaxSpeed = 3,
                MaxRotationSpeed = 0.015f,
                Anchor = new Vector2(62, 41),
                LeftTop = new Vector2(-62, -41),
                RightBottom = new Vector2(62, 41),
                VelocityProjection = new Vector2(0, 0),
                Velocity = new Vector2(0, 0),
                MAcceleration = 0.07f,
                RAcceleration = 0.005f
            };

            barrelAndTower = new BarrelAndTower
            {
                Pos = tankHull.Pos - new Vector2(15, 15) * new Vector2((float)Math.Cos(tankHull.Angle), (float)Math.Sin(tankHull.Angle)),
                ImageId = 2,
                Speed = 0,
                Angle = 0,
                RotationSpeed = 0,
                MaxSpeed = 3,
                Anchor = new Vector2(27, 23),
                Tank = tankHull,
                LeftTop = new Vector2(-27, -23),
                RightBottom = new Vector2(85, 23)
            };

            bullets = new List<Bullet>();
            explosions = new List<Explosion>();
        }
  
        public void Update()
        {
  	        tankHull.Update();
            barrelAndTower.Update();

            foreach (Bullet bullet in bullets)
            {
                bullet.Update();
            }

            Updated.Invoke(this, new GameplayEventArgs 
            { 
                TankHull = tankHull,
                BarrelAndTower = barrelAndTower,
                Map = map,
                Bullets = bullets,
                Explosions = explosions
            });                  
        }

        public void ChangeTurretRotate(MouseState mouseState)
        {
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            Vector2 direction = mousePosition - barrelAndTower.Pos;
            float turretRotation = (float)Math.Atan2(direction.Y, direction.X);

            float difference = turretRotation - barrelAndTower.Angle;
            difference = (float)((difference + Math.PI) % (2 * Math.PI) - Math.PI);

            if (Math.Abs(difference) < 0.03)
            {
                barrelAndTower.RotationSpeed = 0f;
            }
            else
            {
                barrelAndTower.RotationSpeed = Math.Sign(difference) * 0.03f;
            }
        }

        public void ChangeTankSpeed(DirectionOfMovement dir)
        {
            tankHull.Speed = MathHelper.Clamp(tankHull.Speed + (tankHull.MAcceleration * tankHull.MovementDirectionMultipliers[dir]), -tankHull.MaxSpeed, tankHull.MaxSpeed);
        }
        
        public void ChangeTankRotate(DirectionOfRotation dir)
        {
            tankHull.RotationSpeed = MathHelper.Clamp(tankHull.RotationSpeed + (tankHull.RAcceleration * tankHull.RotationDirectionMultipliers[dir]), -tankHull.MaxRotationSpeed, tankHull.MaxRotationSpeed);
        }

        public void TankSlowdownSpeed()
        {
            if (tankHull.Speed > 0)
            {
                tankHull.Speed -= tankHull.MAcceleration;
                if (tankHull.Speed < 0)
                {
                    tankHull.Speed = 0;
                }
            }
            else if (tankHull.Speed < 0)
            {
                tankHull.Speed += tankHull.MAcceleration;
                if (tankHull.Speed > 0)
                {
                    tankHull.Speed = 0;
                }
            }
        }

        public void TankSlowdownRotate()
        {
            if (tankHull.RotationSpeed > 0)
            {
                tankHull.RotationSpeed -= tankHull.MAcceleration;
                if (tankHull.RotationSpeed < 0)
                {
                    tankHull.RotationSpeed = 0;
                }
            }
            else if (tankHull.RotationSpeed < 0)
            {
                tankHull.RotationSpeed += tankHull.MAcceleration;
                if (tankHull.RotationSpeed > 0)
                {
                    tankHull.RotationSpeed = 0;
                }
            }
        }

        public void CheckTankBoundary()
        {
            var staticNodesHit = map.statictree.Query(IObject.GetBox(tankHull));
            foreach (var staticNode in staticNodesHit)
            {
                var normal = Cell.GetBox(staticNode).DetectCollision(IObject.GetBox(tankHull));
                if (normal != null)
                {
                    float dotProduct = Vector2.Dot(tankHull.Velocity, normal.N.ToVector2());
                    tankHull.Pos += normal.N.ToVector2();
                    tankHull.VelocityProjection = dotProduct * normal.N.ToVector2();
                }
            }
        }

        public void CheckBulletsBoundary()
        {
            for (var i = 0;  i < bullets.Count; i++)
            {
                var staticNodesHit = map.statictree.Query(IObject.GetBox(bullets[i]));
                foreach (var staticNode in staticNodesHit)
                {
                    var newExplosion = new Explosion
                    {
                        Pos = bullets[i].Pos,
                        ImageId = 4,
                        Speed = 0,
                        Angle = bullets[i].Angle,
                        MaxSpeed = 0,
                        Anchor = new Vector2(120, 32),
                        LeftTop = new Vector2(-120, -32),
                        RightBottom = new Vector2(0, 32)
                    };
                    explosions.Add(newExplosion);
                    bullets.RemoveAt(i);
                    break;
                }
            }
        }
        public void UpdateExplosion()
        {
            for (int i = 0; i < explosions.Count; i++)
            {
                if (explosions[i].AnimationFrame == 3)
                {
                    explosions.RemoveAt(i);
                }
                else
                {
                    explosions[i].AnimationFrame++;
                }
            }
        }

        public void TankShoot(GameTime time)
        {
            if (IsPossibleShoot && time.TotalGameTime.TotalSeconds - timeLastShoot > 1)
            {
                var newBullet = new Bullet
                {
                    Pos = new Vector2((int)(barrelAndTower.Pos.X + barrelAndTower.RightBottom.X * Math.Cos(barrelAndTower.Angle)), (int)(barrelAndTower.Pos.Y + barrelAndTower.RightBottom.X * Math.Sin(barrelAndTower.Angle))),
                    ImageId = 3,
                    Speed = 15f,
                    Angle = barrelAndTower.Angle,
                    MaxSpeed = 15,
                    Anchor = new Vector2(0, 4),
                    LeftTop = new Vector2(0, -4),
                    RightBottom = new Vector2(21, 5)
                };

                bullets.Add(newBullet);
                timeLastShoot = time.TotalGameTime.TotalSeconds;
                barrelAndTower.AnimationFrame = 0;
            }

            IsPossibleShoot = false;
        }

        public void StopTankShoot()
        {
            IsPossibleShoot = true;
        }
    }
}
