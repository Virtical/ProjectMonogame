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
        event EventHandler<EventArgs> CycleFinished;
        event EventHandler<DirectionOfMovement> TankMoved;
        event EventHandler<DirectionOfRotation> TankRotate;
        event EventHandler<MouseState> TurretRotate;
        event EventHandler<EventArgs> PlayerSlowdownSpeed;
        event EventHandler<EventArgs> PlayerSlowdownRotate;
        event EventHandler<GameTime> TankShoot;
        event EventHandler<EventArgs> StopTankShoot;
        event EventHandler<GameTime> UndergroundLauncherShot;
        event EventHandler<EventArgs> StopUndergroundLauncherShot;

        void LoadGameCycleParameters(Map map, TankHull tankHull, Turret barrelAndTower, List<Bullet> bullets, List<Explosion> explosions, UndergroundLauncher undergroundLauncher, Queue<Point> burnPoint);
        void Run();
    }

    public class ControlsEventArgs : EventArgs
    {
        public DirectionOfMovement Direction { get; set; }
    }
}
