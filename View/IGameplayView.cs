using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankMonogame.Model.Interface;

namespace TankMonogame.View
{
    public interface IGameplayView
    {
        event EventHandler CycleFinished;
        event EventHandler<int> PlayerMoved;
        event EventHandler<float> PlayerRotate;
        event EventHandler<float> PlayerSlowdownSpeed;
        event EventHandler<float> PlayerSlowdownRotate;

        void LoadGameCycleParameters(Dictionary<int, IObject> Objects);
        void Run();
    }

    public class ControlsEventArgs : EventArgs
    {
        public IGameplayModel.Direction Direction { get; set; }
    }
}
