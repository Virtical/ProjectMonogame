﻿using System;
using TankMonogame.Model;
using TankMonogame.Presenter;
using TankMonogame.View;


public static class Program
{
    [STAThread]
    static void Main()
    {      
        GameplayPresenter g = new GameplayPresenter(new GameplayView(), new GameplayModel());
        g.LaunchGame();
    }
}