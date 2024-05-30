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
            gameplayModel.Updated += ModelViewUpdate;

            gameplayModel.Initialize();
        }

        private void ViewModelMovePlayer(object sender, ControlsEventArgs e)
        {
            gameplayModel.ChangePlayerSpeed(e.Direction);
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
