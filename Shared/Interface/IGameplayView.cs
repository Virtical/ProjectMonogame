using System;
using System.Collections.Generic;
using TankMonogame.Model;
using TankMonogame.Shared.Enums;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TankMonogame.Shared.Interface
{
    public interface IGameplayView
    {
        event EventHandler CycleFinished;
        event EventHandler<DirectionOfMovement> TankMoved;
        event EventHandler<DirectionOfRotation> TankRotate;
        event EventHandler<MouseState> TurretRotate;
        event EventHandler<EventArgs> PlayerSlowdownSpeed;
        event EventHandler<EventArgs> PlayerSlowdownRotate;
        event EventHandler<GameTime> TankShoot;
        event EventHandler<EventArgs> StopTankShoot;

        void LoadGameCycleParameters(Map map, TankHull tankHull, BarrelAndTower barrelAndTower, List<Bullet> bullets);
        void Run();
    }

    public class ControlsEventArgs : EventArgs
    {
        public DirectionOfMovement Direction { get; set; }
    }
}
