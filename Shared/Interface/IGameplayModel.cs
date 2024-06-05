using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TankMonogame.Model;
using TankMonogame.Shared.Enums;

namespace TankMonogame.Shared.Interface
{
    public interface IGameplayModel
    {
        event EventHandler<GameplayEventArgs> Updated;

        void Update();
        void ChangeTankSpeed(DirectionOfMovement dir);
        void ChangeTankRotate(DirectionOfRotation dir);
        void ChangeTurretRotate(MouseState mouseState);
        void TankShoot(GameTime time);
        void StopTankShoot();
        void CheckBulletsBoundary();
        void TankSlowdownSpeed();
        void TankSlowdownRotate();
        void CheckTankBoundary();
        void Initialize();
    }
    public class GameplayEventArgs : EventArgs
    {
        public TankHull TankHull { get; set; }
        public BarrelAndTower BarrelAndTower{ get; set; }
        public Map Map { get; set; }
        public List<Bullet> Bullets { get; set; }
    }
}
