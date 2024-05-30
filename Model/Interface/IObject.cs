﻿using Microsoft.Xna.Framework;

namespace TankMonogame.Model.Interface
{
    public interface IObject
    {
        int ImageId { get; set; }

        Vector2 Pos { get; }

        void Update();
    }
}