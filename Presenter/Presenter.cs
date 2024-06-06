using System;
using TankMonogame.Shared.Interface;
using TankMonogame.Shared.Enums;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TankMonogame.Presenter
{
    public class GameplayPresenter
    {
        private IGameplayView gameplayView = null;
        private IGameplayModel gameplayModel = null;

        public GameplayPresenter(IGameplayView gameplayView, IGameplayModel gameplayModel)
        {
            this.gameplayView = gameplayView;
            this.gameplayModel = gameplayModel;

            gameplayView.CycleFinished += ViewModelUpdate;
            gameplayView.TankMoved += ViewModelMoveTank;
            gameplayView.TankRotate += ViewModelRotateTank;
            gameplayView.PlayerSlowdownSpeed += ViewModelSlowdownSpeedTank;
            gameplayView.PlayerSlowdownRotate += ViewModelSlowdownRotateTank;
            gameplayView.TankShoot += ViewModelTankShoot;
            gameplayView.UndergroundLauncherShot += ViewModelUndergroundLauncherShot;
            gameplayView.StopUndergroundLauncherShot += ViewModelStopUndergroundLauncherShot;
            gameplayView.StopTankShoot += ViewModelStopTankShoot;
            gameplayView.TurretRotate += ViewModelRotateTurret;
            gameplayModel.Updated += ModelViewUpdate;

            gameplayModel.Initialize();
        }

        private void ViewModelRotateTurret(object sender, MouseState e)
        {
            gameplayModel.ChangeTurretRotate(e);
        }

        private void ViewModelTankShoot(object sender, GameTime e)
        {
            gameplayModel.TankShoot(e);
        }

        private void ViewModelUndergroundLauncherShot(object sender, GameTime e)
        {
            gameplayModel.UndergroundLauncherShot(e);
        }

        private void ViewModelStopUndergroundLauncherShot(object sender, EventArgs e)
        {
            gameplayModel.StopUndergroundLauncherShot();
        }

        private void ViewModelStopTankShoot(object sender, EventArgs e)
        {
            gameplayModel.StopTankShoot();
        }

        private void ViewModelMoveTank(object sender, DirectionOfMovement dir)
        {
            gameplayModel.ChangeTankSpeed(dir);
        }

        private void ViewModelRotateTank(object sender, DirectionOfRotation dir)
        {
            gameplayModel.ChangeTankRotate(dir);
        }

        private void ViewModelSlowdownSpeedTank(object sender, EventArgs e)
        {
            gameplayModel.TankSlowdownSpeed();
        }

        private void ViewModelSlowdownRotateTank(object sender, EventArgs e)
        {
            gameplayModel.TankSlowdownRotate();
        }

        private void ModelViewUpdate(object sender, GameplayEventArgs e)
        {
            gameplayModel.CheckTankBoundary();
            gameplayModel.CheckBulletsBoundary();
            gameplayModel.UpdateExplosion();
            gameplayView.LoadGameCycleParameters(e.Map, e.TankHull, e.Turret, e.Bullets, e.Explosions, e.UndergroundLauncher, e.BurnPoint);
        }

        private void ViewModelUpdate(object sender, EventArgs e)
        {
            gameplayModel.Update();
        }

        public void LaunchGame()
        {
            gameplayView.Run();
        }
    }
}
