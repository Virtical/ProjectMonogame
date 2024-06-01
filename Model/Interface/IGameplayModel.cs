using System;
using System.Collections.Generic;

namespace TankMonogame.Model.Interface
{
    public interface IGameplayModel
    {
        int PlayerId { get; set; }
        Dictionary<int, IObject> Objects { get; set; }
        event EventHandler<GameplayEventArgs> Updated;

        void Update();
        void ChangePlayerSpeed(int acceleration);
        void ChangePlayerRotate(float rotationAcceleration);
        void PlayerSlowdownSpeed(float slowdown);
        void PlayerSlowdownRotate(float slowdown);
        void Initialize();

        public enum Direction : byte
        {
            forward,
            backward,
            right,
            left
        }
    }
    public class GameplayEventArgs : EventArgs
    {
        public Dictionary<int, IObject> Objects { get; set; }
    }
}
