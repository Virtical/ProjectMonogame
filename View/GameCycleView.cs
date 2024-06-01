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
        public event EventHandler<int> PlayerMoved = delegate { };
        public event EventHandler<float> PlayerRotate = delegate { };
        public event EventHandler<float> PlayerSlowdownSpeed = delegate { };
        public event EventHandler<float> PlayerSlowdownRotate = delegate { };
        private Dictionary<int, IObject> objects = new Dictionary<int, IObject>();
        private Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();

        private Map map;
        private Dictionary<ICell.TypeCell, Texture2D> textureCell = new Dictionary<ICell.TypeCell, Texture2D>();


        public GameCycleView()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = false;
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
            textures.Add(1, Content.Load<Texture2D>("Tank"));

            textureCell[ICell.TypeCell.Level1] = Content.Load<Texture2D>("FloorLevel1"); 
            textureCell[ICell.TypeCell.Level2] = Content.Load<Texture2D>("FloorLevel2");
            textureCell[ICell.TypeCell.Level3] = Content.Load<Texture2D>("FloorLevel3");
            textureCell[ICell.TypeCell.Level4] = Content.Load<Texture2D>("FloorLevel4");
            textureCell[ICell.TypeCell.Level5] = Content.Load<Texture2D>("FloorLevel5");
            textureCell[ICell.TypeCell.Level6] = Content.Load<Texture2D>("FloorLevel6");

        }

        public void LoadGameCycleParameters(Dictionary<int, IObject> Objects)
        {
            objects = Objects;
        }

        protected override void Update(GameTime gameTime)
        {
            var keys = Keyboard.GetState().GetPressedKeys();

            bool isMoving = false;
            bool isRotating = false;

            foreach (var k in keys)
            {
                switch (k)
                {
                    case Keys.W:
                        PlayerMoved.Invoke(this, 1);
                        isMoving = true;
                        break;
                    case Keys.S:
                        PlayerMoved.Invoke(this, -1);
                        isMoving = true;
                        break;
                    case Keys.D:
                        PlayerRotate.Invoke(this, 0.005f);
                        isRotating = true;
                        break;
                    case Keys.A:
                        PlayerRotate.Invoke(this, -0.005f);
                        isRotating = true;
                        break;
                    case Keys.Escape:
                        Exit();
                        break;
                }
            }

            if (!isMoving)
            {
                PlayerSlowdownSpeed.Invoke(this, 0.3f);
            }

            if (!isRotating)
            {
                PlayerSlowdownRotate.Invoke(this, 0.003f);
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
                spriteBatch.Draw(textures[o.ImageId], o.Pos, null, Color.White, o.Rotation, new Vector2(textures[o.ImageId].Width / 2, textures[o.ImageId].Height / 2), 1f, SpriteEffects.None, 0f);
            }
            spriteBatch.End();
        }
    }
}
