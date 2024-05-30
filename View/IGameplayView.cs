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
        //Включается в конце каждого цикла, чтобы обновить модель
        event EventHandler CycleFinished;
        event EventHandler<ControlsEventArgs> PlayerMoved;

        void LoadGameCycleParameters(Dictionary<int, IObject> Objects);
        void Run();
    }

    public class ControlsEventArgs : EventArgs
    {
        public IGameplayModel.Direction Direction { get; set; }
    }
}
