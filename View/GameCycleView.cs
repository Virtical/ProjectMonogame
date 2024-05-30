using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TankMonogame.Model;
using TankMonogame.Model.Interface;

namespace TankMonogame.View
{
    public class GameCycleView : Game, IGameplayView
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public event EventHandler CycleFinished = delegate { };
        public event EventHandler<ControlsEventArgs> PlayerMoved = delegate { };
        private Dictionary<int, IObject> objects = new Dictionary<int, IObject>();
        private Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();

        private Map map;
        private Dictionary<ICell.TypeCell, Texture2D> textureCell = new Dictionary<ICell.TypeCell, Texture2D>();


        public GameCycleView()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            map = new Map(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, 64);
        }

        protected override void Initialize()
        {
            Window.Position = new Point(0, 0);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textures.Add(1, Content.Load<Texture2D>("White_Placeholder"));

            textureCell[ICell.TypeCell.Level1] = Content.Load<Texture2D>("FloorLevel1"); 
            textureCell[ICell.TypeCell.Level2] = Content.Load<Texture2D>("FloorLevel2");
            textureCell[ICell.TypeCell.Level3] = Content.Load<Texture2D>("FloorLevel3");
            textureCell[ICell.TypeCell.Level4] = Content.Load<Texture2D>("FloorLevel4");

        }

        public void LoadGameCycleParameters(Dictionary<int, IObject> Objects)
        {
            objects = Objects;
        }

        protected override void Update(GameTime gameTime)
        {
            var keys = Keyboard.GetState().GetPressedKeys();
            if (keys.Length > 0)
            {
                var k = keys[0];
                switch (k)
                {
                    case Keys.W:
                        {
                            PlayerMoved.Invoke(this, new ControlsEventArgs
                              {
                                  Direction = IGameplayModel.Direction.forward
                              }
                            );
                            break;
                        }
                    case Keys.S:
                        {
                            PlayerMoved.Invoke(this, new ControlsEventArgs
                                {
                                    Direction = IGameplayModel.Direction.backward
                                }
                              );
                            break;
                        }
                    case Keys.D:
                        {
                            PlayerMoved.Invoke(this, new ControlsEventArgs
                                {
                                    Direction = IGameplayModel.Direction.right
                                }
                              );
                            break;
                        }
                    case Keys.A:
                        {
                            PlayerMoved.Invoke(this, new ControlsEventArgs
                                {
                                    Direction = IGameplayModel.Direction.left
                                }
                              );
                            break;
                        }
                    case Keys.Escape:
                        {
                            Exit();
                            break;
                        }
                }
            }
            base.Update(gameTime);
            CycleFinished.Invoke(this, new EventArgs());
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
            spriteBatch.Begin();

            foreach (Cell cell in map.Cells)
            {
                spriteBatch.Draw(textureCell[cell.Type], cell.LTPoint.ToVector2(), Color.White);
            }

            foreach (var o in objects.Values)
            {
                spriteBatch.Draw(textures[o.ImageId], o.Pos, Color.White);
            }
            spriteBatch.End();
        }
    }
}
