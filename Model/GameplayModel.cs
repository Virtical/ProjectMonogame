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
    public class GameplayModel : IGameplayModel
    {
	    public event EventHandler<GameplayEventArgs> Updated = delegate { };
 
        private Map map;
        private TankHull tankHull;
        private Turret turret;
        private UndergroundLauncher undergroundLauncher;
        private List<Bullet> bullets;
        private List<Explosion> explosions;
        private Queue<Point> burnPoint;
        private double timeLastTankShoot = 0;
        private double timeLastUndergroundLauncherShoot = 0;

        private bool IsPossibleTankShoot = true;
        private bool IsPossibleUndergroundLauncherShoot = true;

        public BoxQT GetBox()
        {
            return IObject.GetBox(tankHull);
        }
        public void Initialize()
        {
            map = new Map(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

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

            turret = new Turret
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

            undergroundLauncher = new UndergroundLauncher
            {
                Pos = new Vector2(1312, 800),
                ImageId = 6,
                Speed = 0,
                Angle = 0,
                MaxSpeed = 0,
                Anchor = new Vector2(29, 48),
                LeftTop = new Vector2(-29, -48),
                RightBottom = new Vector2(30, 30)
            };

            bullets = new List<Bullet>();
            explosions = new List<Explosion>();
            burnPoint = new Queue<Point>();
        }
  
        public void Update()
        {
  	        tankHull.Update();
            turret.Update();

            foreach (Bullet bullet in bullets)
            {
                bullet.Update();
            }

            Updated.Invoke(this, new GameplayEventArgs
            {
                TankHull = tankHull,
                Turret = turret,
                Map = map,
                Bullets = bullets,
                Explosions = explosions,
                UndergroundLauncher = undergroundLauncher,
                BurnPoint = burnPoint
            });                  
        }
        public void UndergroundLauncherShot(GameTime gameTime)
        {
            if (IsPossibleUndergroundLauncherShoot)
            {
                if ((gameTime.TotalGameTime.TotalSeconds - timeLastUndergroundLauncherShoot) > 3)
                {
                    burnPoint.Enqueue(map.GetRandomCleanPlace());
                    timeLastUndergroundLauncherShoot = gameTime.TotalGameTime.TotalSeconds;
                    undergroundLauncher.NextState();
                }
                else
                {
                    undergroundLauncher.AnimationFrame = (int)undergroundLauncher.CurState * 2 + 1;
                }
                IsPossibleUndergroundLauncherShoot = false;
            }
        }

        public void StopUndergroundLauncherShot()
        {
            IsPossibleUndergroundLauncherShoot = true;
        }

        public void ChangeTurretRotate(MouseState mouseState)
        {
            Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
            Vector2 direction = mousePosition - turret.Pos;
            float turretRotation = (float)Math.Atan2(direction.Y, direction.X);

            float difference = turretRotation - turret.Angle;
            difference = (float)((difference + Math.PI) % (2 * Math.PI) - Math.PI);

            if (Math.Abs(difference) < 0.03)
            {
                turret.RotationSpeed = 0f;
            }
            else
            {
                turret.RotationSpeed = Math.Sign(difference) * 0.03f;
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
            var staticNodesHit = map.StaticTreeBorders.Query(IObject.GetBox(tankHull));
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

            var staticBurrelsHit = map.StaticTreeBurrels.Query(IObject.GetBox(tankHull));
            foreach (var staticNode in staticBurrelsHit)
            {
                var normal = Burrel.GetBox(staticNode).DetectCollision(IObject.GetBox(tankHull));
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
            var bulletsToRemove = new List<Bullet>();
            var burrelsToRemove = new List<Burrel>();

            for (var i = 0; i < bullets.Count; i++)
            {
                var bullet = bullets[i];
                var staticCellsHit = map.StaticTreeBorders.Query(IObject.GetBox(bullet));

                bool isHit = false;

                foreach (var staticNode in staticCellsHit)
                {
                    var newExplosion = new Explosion
                    {
                        Pos = bullet.Pos + new Vector2((float)Math.Cos(bullet.Angle) * 17, (float)Math.Sin(bullet.Angle) * 17),
                        ImageId = 4,
                        Speed = 0,
                        Angle = bullet.Angle,
                        MaxSpeed = 0,
                        Anchor = new Vector2(120, 32),
                        LeftTop = new Vector2(-120, -32),
                        RightBottom = new Vector2(0, 32)
                    };

                    explosions.Add(newExplosion);
                    bulletsToRemove.Add(bullet);
                    isHit = true;
                    break;
                }

                if (isHit) continue;

                var staticBurrelsHit = map.StaticTreeBurrels.Query(IObject.GetBox(bullet));
                foreach (var staticNode in staticBurrelsHit)
                {
                    var newExplosion = new Explosion
                    {
                        Pos = bullet.Pos + new Vector2((float)Math.Cos(bullet.Angle) * 17, (float)Math.Sin(bullet.Angle) * 17),
                        ImageId = 4,
                        Speed = 0,
                        Angle = bullet.Angle,
                        MaxSpeed = 0,
                        Anchor = new Vector2(120, 32),
                        LeftTop = new Vector2(-120, -32),
                        RightBottom = new Vector2(0, 32)
                    };
                    explosions.Add(newExplosion);
                    burrelsToRemove.Add(staticNode);
                    bulletsToRemove.Add(bullet);
                    break;
                }
            }

            foreach (var burrel in burrelsToRemove)
            {
                map.burrels.Remove(burrel);
                map.StaticTreeBurrels.Remove(burrel);
            }
            foreach (var bullet in bulletsToRemove)
            {
                bullets.Remove(bullet);
            }
        }
        public void UpdateExplosion()
        {
            for (int i = 0; i < explosions.Count; i++)
            {
                if (explosions[i].AnimationFrame == 11)
                {
                    explosions.RemoveAt(i);
                }
                else
                {
                    explosions[i].AnimationFrame++;
                }
            }
        }

        public void TankShoot(GameTime gameTime)
        {
            if (IsPossibleTankShoot && gameTime.TotalGameTime.TotalSeconds - timeLastTankShoot > 1)
            {
                var newBullet = new Bullet
                {
                    Pos = new Vector2((int)(turret.Pos.X + turret.RightBottom.X * Math.Cos(turret.Angle)), (int)(turret.Pos.Y + turret.RightBottom.X * Math.Sin(turret.Angle))),
                    ImageId = 3,
                    Speed = 15f,
                    Angle = turret.Angle,
                    MaxSpeed = 15,
                    Anchor = new Vector2(0, 4),
                    LeftTop = new Vector2(0, -4),
                    RightBottom = new Vector2(21, 5)
                };

                bullets.Add(newBullet);
                timeLastTankShoot = gameTime.TotalGameTime.TotalSeconds;
                turret.AnimationFrame = 0;
            }

            IsPossibleTankShoot = false;
        }

        public void StopTankShoot()
        {
            IsPossibleTankShoot = true;
        }
    }
}
