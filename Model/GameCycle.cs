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
            tank.Pos = new Vector2(250, 250);
            tank.ImageId = 1;
            tank.Speed = 0;
            tank.Rotation = 0;
            tank.RotationSpeed = 0;
            tank.MaxSpeed = 3;
            tank.MaxRotationSpeed = 0.015f;
            Objects.Add(1, tank);

            PlayerId = 1;
        }
  
        public void Update()
        {
  	        foreach (var o in Objects.Values)
            {
    	        o.Update();
            }
            Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects });                  
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
