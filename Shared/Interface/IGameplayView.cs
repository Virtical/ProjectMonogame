using System;
using System.Collections.Generic;
using TankMonogame.Model;
using TankMonogame.Shared.Enums;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

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
        event EventHandler<ContentManager> LoadContentOnModel;

        void LoadGameCycleParameters(Map map, TankHull tankHull, Turret barrelAndTower, List<Bullet> bullets, List<Explosion> explosions, UndergroundLauncher undergroundLauncher, List<Lava> burnPoint, List<Rocket> rockets);
        void Run();
    }

    public class ControlsEventArgs : EventArgs
    {
        public DirectionOfMovement Direction { get; set; }
    }
}
