using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using TankMonogame.Model.Interface;
using System.Threading.Tasks;

namespace TankMonogame.Model
{
    public class GameCycle : IGameplayModel
    {
	    public event EventHandler<GameplayEventArgs> Updated = delegate { };
  
        public int PlayerId { get; set; }
        
        public Dictionary<int, IObject> Objects { get; set; }
  
        public void Initialize()
        {
  	        Objects = new Dictionary<int, IObject>();

            Tank tank = new Tank();
            tank.Pos = new Vector2(200, 350);
            tank.ImageId = 1;
            tank.Speed = 0;
            tank.Angle = 0;
            tank.RotationSpeed = 0;
            tank.MaxSpeed = 3;
            tank.MaxRotationSpeed = 0.015f;
            tank.Anchor = new Vector2(62, 41);
            tank.LeftTop = new Vector2(-62, -41);
            tank.RightBottom = new Vector2(62, 41);
            tank.VelocityProjection = new Vector2(0, 0);
            tank.Velocity = new Vector2(0, 0);
            Objects.Add(1, tank);

            BarrelAndTower turret = new BarrelAndTower();
            turret.Pos = tank.Pos - new Vector2(15, 15) * new Vector2((float)Math.Cos(tank.Angle), (float)Math.Sin(tank.Angle));
            turret.ImageId = 2;
            turret.Speed = 0;
            turret.Angle = 0;
            turret.RotationSpeed = 0;
            turret.MaxSpeed = 3;
            turret.Anchor = new Vector2(27, 23);
            turret.tank = tank;
            turret.LeftTop = new Vector2(-27, -23);
            turret.RightBottom = new Vector2(85, 23);
            Objects.Add(2, turret);

            PlayerId = 1;
        }
  
        public void Update()
        {
  	        foreach (var o in Objects.Values)
            {
    	        o.Update();
            }
            Updated.Invoke(this, new GameplayEventArgs { Objects = Objects });                  
        }
      
        public void ChangePlayerSpeed(int acceleration)
        {
            Tank tank = (Tank)Objects[PlayerId];
            tank.Speed = MathHelper.Clamp(tank.Speed + acceleration, -tank.MaxSpeed, tank.MaxSpeed);
        }
        
        public void ChangePlayerRotate(float rotationAcceleration)
        {
            Tank tank = (Tank)Objects[PlayerId];
            tank.RotationSpeed = MathHelper.Clamp(tank.RotationSpeed + rotationAcceleration, -tank.MaxRotationSpeed, tank.MaxRotationSpeed);
        }

        public void PlayerSlowdownSpeed(float slowndown)
        {
            Tank tank = (Tank)Objects[PlayerId];
            if (tank.Speed > 0)
            {
                tank.Speed -= slowndown;
                if (tank.Speed < 0)
                {
                    tank.Speed = 0;
                }
            }
            else if (tank.Speed < 0)
            {
                tank.Speed += slowndown;
                if (tank.Speed > 0)
                {
                    tank.Speed = 0;
                }
            }
        }

        public void PlayerSlowdownRotate(float slowndown)
        {
            Tank tank = (Tank)Objects[PlayerId];
            if (tank.RotationSpeed > 0)
            {
                tank.RotationSpeed -= slowndown;
                if (tank.RotationSpeed < 0)
                {
                    tank.RotationSpeed = 0;
                }
            }
            else if (tank.RotationSpeed < 0)
            {
                tank.RotationSpeed += slowndown;
                if (tank.RotationSpeed > 0)
                {
                    tank.RotationSpeed = 0;
                }
            }
        }
    }
}
