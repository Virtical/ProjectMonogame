﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using TankMonogame.Model.Interface;

namespace TankMonogame.Model
{
    public class GameCycle : IGameplayModel
{
	public event EventHandler<GameplayEventArgs> Updated = delegate { };
  
  public int PlayerId { get; set; }
        
  public Dictionary<int, IObject> Objects { get; set; }
  
  public void Initialize()
  {
  	Objects = new Dictionary<int, IObject>();
    Car player = new Car();
    player.Pos = new Vector2 (250, 250);
    player.ImageId = 1;
    player.Speed = new Vector2 (0, 0);
    Objects.Add(1, player);
    PlayerId = 1;
  }
  
  public void Update()
  {
  	foreach (var o in Objects.Values)
    {
    	o.Update();
    }
    Updated.Invoke(this, new GameplayEventArgs { Objects = this.Objects });                  
  }
      
  public void ChangePlayerSpeed(IGameplayModel.Direction dir)
  {
  	Car p = (Car)Objects[PlayerId];
  	switch (dir)
    {
    	case IGameplayModel.Direction.forward:
    	{
    		p.Speed += new Vector2(0, -0.5f);
    		break;
   		}
    	case IGameplayModel.Direction.backward:
    	{
    		p.Speed += new Vector2(0, 0.5f);
    		break;
    	}
    	case IGameplayModel.Direction.right:
    	{
    		p.Speed += new Vector2(0.5f, 0);
    		break;
    	}
    	case IGameplayModel.Direction.left:
    	{
   		  p.Speed += new Vector2(-0.5f, 0);
    		break;
    	}
   	}          
  }       
}
}