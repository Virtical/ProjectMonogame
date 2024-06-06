using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TankMonogame.Model;
using TankMonogame.Shared.Interface;
using TankMonogame.Shared.Enums;
namespace TankMonogame.View
{
    public class GameCycleView : Game, IGameplayView
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public event EventHandler CycleFinished = delegate { };
        public event EventHandler<DirectionOfMovement> TankMoved = delegate { };
        public event EventHandler<DirectionOfRotation> TankRotate = delegate { };
        public event EventHandler<EventArgs> PlayerSlowdownSpeed = delegate { };
        public event EventHandler<EventArgs> PlayerSlowdownRotate = delegate { };
        public event EventHandler<GameTime> TankShoot = delegate { };
        public event EventHandler<EventArgs> StopTankShoot = delegate { };
        public event EventHandler<MouseState> TurretRotate = delegate { };

        private Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();

        Texture2D pointTexture;

        private Map map;
        private TankHull tankHull;
        private BarrelAndTower barrelAndTower;
        private List<Bullet> bullets;

        private Dictionary<TypeCell, Texture2D> textureCell = new Dictionary<TypeCell, Texture2D>();


        public GameCycleView()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
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
            textures.Add(3, Content.Load<Texture2D>("Bullet"));

            textureCell[TypeCell.Level1] = Content.Load<Texture2D>("FloorLevel1");
            textureCell[TypeCell.Level2] = Content.Load<Texture2D>("FloorLevel2");
            textureCell[TypeCell.Level3] = Content.Load<Texture2D>("FloorLevel3");
            textureCell[TypeCell.Level4] = Content.Load<Texture2D>("FloorLevel4");
            textureCell[TypeCell.Level5] = Content.Load<Texture2D>("FloorLevel5");
            textureCell[TypeCell.Level6] = Content.Load<Texture2D>("FloorLevel6");
            textureCell[TypeCell.Level7] = Content.Load<Texture2D>("FloorLevel7");
            textureCell[TypeCell.Level8] = Content.Load<Texture2D>("FloorLevel8");

            pointTexture = new Texture2D(GraphicsDevice, 1, 1);
            pointTexture.SetData(new Color[] { Color.Red });
        }

        public void LoadGameCycleParameters(Map map, TankHull tankHull, BarrelAndTower barrelAndTower, List<Bullet> bullets)
        {
            this.map = map;
            this.tankHull = tankHull;
            this.barrelAndTower = barrelAndTower;
            this.bullets = bullets;
        }

        protected override void Update(GameTime gameTime)
        {
            var mouseState = Mouse.GetState();
            TurretRotate.Invoke(this, mouseState);

            switch (mouseState.LeftButton)
            {
                case ButtonState.Pressed:
                    TankShoot.Invoke(this, gameTime);
                    break;
                case ButtonState.Released:
                    StopTankShoot.Invoke(this, EventArgs.Empty);
                    break;
            }


            var keys = Keyboard.GetState().GetPressedKeys();

            bool isMoving = false;
            bool isRotating = false;

            foreach (var k in keys)
            {
                switch (k)
                {
                    case Keys.W:
                        TankMoved.Invoke(this, DirectionOfMovement.Forward);
                        isMoving = true;
                        break;
                    case Keys.S:
                        TankMoved.Invoke(this, DirectionOfMovement.Backward);
                        isMoving = true;
                        break;
                    case Keys.D:
                        TankRotate.Invoke(this, DirectionOfRotation.Right);
                        isRotating = true;
                        break;
                    case Keys.A:
                        TankRotate.Invoke(this, DirectionOfRotation.Left);
                        isRotating = true;
                        break;
                    case Keys.Escape:
                        Exit();
                        break;
                }
            }

            if (!isMoving)
            {
                PlayerSlowdownSpeed.Invoke(this, EventArgs.Empty);
            }

            if (!isRotating)
            {
                PlayerSlowdownRotate.Invoke(this, EventArgs.Empty);
            }

            base.Update(gameTime);

            CycleFinished.Invoke(this, new EventArgs());
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            foreach (Cell cell in map.Cells)
            {
                spriteBatch.Draw(textureCell[cell.Type], cell.LTPoint.ToVector2(), Color.White);
            }

            spriteBatch.Draw(textures[tankHull.ImageId], tankHull.Pos, null, Color.White, tankHull.Angle, tankHull.Anchor, 1f, SpriteEffects.None, 0f);

            var boxT = IObject.GetBox(tankHull);
            foreach(var corner in boxT.Corners())
            {
                spriteBatch.Draw(pointTexture, new Rectangle((int)corner.X - 1, (int)corner.Y - 1, 3, 3), Color.Red);
            }

            spriteBatch.Draw(textures[barrelAndTower.ImageId], barrelAndTower.Pos, null, Color.White, barrelAndTower.Angle, barrelAndTower.Anchor, 1f, SpriteEffects.None, 0f);

            foreach (var bullet in bullets)
            {
                spriteBatch.Draw(textures[bullet.ImageId], bullet.Pos, null, Color.White, bullet.Angle, bullet.Anchor, 1f, SpriteEffects.None, 0f);
            }

            spriteBatch.End();
        }
    }
}
