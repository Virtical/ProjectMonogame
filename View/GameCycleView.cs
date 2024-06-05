using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TankMonogame.Model;
using TankMonogame.Model.Interface;
using TankMonogame.Model.QuadTree.TankMonogame.Model.QuadTree;
using TankMonogame.Model.QuadTree;

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
        Texture2D pointTexture;

        private Map map;
        private Dictionary<ICell.TypeCell, Texture2D> textureCell = new Dictionary<ICell.TypeCell, Texture2D>();


        public GameCycleView()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            map = new Map(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height, 64);
        }

        protected override void Initialize()
        {
            Window.Position = new Point(0, 0);
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            textures.Add(1, Content.Load<Texture2D>("Tank"));
            textures.Add(2, Content.Load<Texture2D>("BarrelAndTower"));

            textureCell[ICell.TypeCell.Level1] = Content.Load<Texture2D>("FloorLevel1"); 
            textureCell[ICell.TypeCell.Level2] = Content.Load<Texture2D>("FloorLevel2");
            textureCell[ICell.TypeCell.Level3] = Content.Load<Texture2D>("FloorLevel3");
            textureCell[ICell.TypeCell.Level4] = Content.Load<Texture2D>("FloorLevel4");
            textureCell[ICell.TypeCell.Level5] = Content.Load<Texture2D>("FloorLevel5");
            textureCell[ICell.TypeCell.Level6] = Content.Load<Texture2D>("FloorLevel6");
            textureCell[ICell.TypeCell.Level7] = Content.Load<Texture2D>("FloorLevel7");
            textureCell[ICell.TypeCell.Level8] = Content.Load<Texture2D>("FloorLevel8");

            pointTexture = new Texture2D(GraphicsDevice, 1, 1);
            pointTexture.SetData(new Color[] { Color.Red });

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
            spriteBatch.Begin();

            var statictree = new Quadtree<Cell>(new BoxQT(0, 0, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height), Cell.GetBox, Cell.Equals);

            var cnt = 0;
            foreach (Cell cell in map.Cells)
            {
                if (cell.Type == ICell.TypeCell.Level8)
                {
                    statictree.Add(cell);
                    cnt++;
                }
                spriteBatch.Draw(textureCell[cell.Type], cell.LTPoint.ToVector2(), Color.White);
            }

            Tank t = null;
            BarrelAndTower b = null;
            foreach (var o in objects.Values)
            {
                spriteBatch.Draw(textures[o.ImageId], o.Pos, null, Color.White, o.Angle, o.Anchor, 1f, SpriteEffects.None, 0f);
                if (o.GetType() == typeof(Tank)) 
                {
                    t = (Tank)o;
                }

                if (o.GetType() == typeof(BarrelAndTower))
                {
                    b = (BarrelAndTower)o;
                }
            }

            var boxT = Tank.GetBox(t);
            spriteBatch.Draw(pointTexture, new Rectangle((int)boxT.GetLeft() - 2, (int)boxT.GetTop() - 2, 3, 3), Color.Red);
            spriteBatch.Draw(pointTexture, new Rectangle((int)boxT.GetRight() - 2, (int)boxT.GetTop() - 2, 3, 3), Color.Red);
            spriteBatch.Draw(pointTexture, new Rectangle((int)boxT.GetRight() - 2, (int)boxT.GetBottom() - 2, 3, 3), Color.Red);
            spriteBatch.Draw(pointTexture, new Rectangle((int)boxT.GetLeft() - 2, (int)boxT.GetBottom() - 2, 3, 3), Color.Red);

            var staticNodesHit = statictree.Query(boxT);
            foreach (var staticNode in staticNodesHit)
            {

                var normal = Cell.GetBox(staticNode).DetectCollision(boxT);

                float dotProduct = Vector2.Dot(t.Velocity, normal.N.ToVector2());
                t.Pos += normal.N.ToVector2();
                t.VelocityProjection = dotProduct * normal.N.ToVector2();

                spriteBatch.Draw(pointTexture, new Rectangle(staticNode.LTPoint.X, staticNode.LTPoint.Y, staticNode.RBPoint.X - staticNode.LTPoint.X, staticNode.RBPoint.Y - staticNode.LTPoint.Y), new Color(0, 0, 0, 100));
            }

            spriteBatch.End();
        }
    }
}
