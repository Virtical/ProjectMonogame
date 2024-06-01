using System;
using TankMonogame.Model.Interface;
using TankMonogame.View;

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
            gameplayView.PlayerMoved += ViewModelMovePlayer;
            gameplayView.PlayerRotate += ViewModelRotatePlayer;
            gameplayView.PlayerSlowdownSpeed += ViewModelSlowdownSpeedPlayer;
            gameplayView.PlayerSlowdownRotate += ViewModelSlowdownRotatePlayer;
            gameplayModel.Updated += ModelViewUpdate;

            gameplayModel.Initialize();
        }

        private void ViewModelMovePlayer(object sender, int acceleration)
        {
            gameplayModel.ChangePlayerSpeed(acceleration);
        }

        private void ViewModelRotatePlayer(object sender, float rotationAcceleration)
        {
            gameplayModel.ChangePlayerRotate(rotationAcceleration);
        }

        private void ViewModelSlowdownSpeedPlayer(object sender, float slowdown)
        {
            gameplayModel.PlayerSlowdownSpeed(slowdown);
        }

        private void ViewModelSlowdownRotatePlayer(object sender, float slowdown)
        {
            gameplayModel.PlayerSlowdownRotate(slowdown);
        }

        private void ModelViewUpdate(object sender, GameplayEventArgs e)
        {
            gameplayView.LoadGameCycleParameters(e.Objects);
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
