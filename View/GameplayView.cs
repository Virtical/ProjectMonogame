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
    public class GameplayView : Game, IGameplayView
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

        private Map map;
        private TankHull tankHull;
        private Turret barrelAndTower;
        private List<Bullet> bullets;
        private List<Explosion> explosions;

        private Dictionary<TypeCell, Texture2D> textureCell = new Dictionary<TypeCell, Texture2D>();
        private Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();


        public GameplayView()
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
            textures.Add(4, Content.Load<Texture2D>("Explosion"));
            textures.Add(5, Content.Load<Texture2D>("Barrels"));

            textureCell[TypeCell.Level1] = Content.Load<Texture2D>("FloorLevel1");
            textureCell[TypeCell.Level2] = Content.Load<Texture2D>("FloorLevel2");
            textureCell[TypeCell.Level3] = Content.Load<Texture2D>("FloorLevel3");
            textureCell[TypeCell.Level4] = Content.Load<Texture2D>("FloorLevel4");
            textureCell[TypeCell.Level5] = Content.Load<Texture2D>("FloorLevel5");
            textureCell[TypeCell.Level6] = Content.Load<Texture2D>("FloorLevel6");
            textureCell[TypeCell.Level7] = Content.Load<Texture2D>("FloorLevel7");
            textureCell[TypeCell.Level8] = Content.Load<Texture2D>("FloorLevel8");
        }

        public void LoadGameCycleParameters(Map map, TankHull tankHull, Turret turret, List<Bullet> bullets, List<Explosion> explosions)
        {
            this.map = map;
            this.tankHull = tankHull;
            this.barrelAndTower = turret;
            this.bullets = bullets;
            this.explosions = explosions;
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

            CycleFinished.Invoke(this, new EventArgs());

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(map, textureCell);
            spriteBatch.Draw(tankHull, textures);
            spriteBatch.Draw(barrelAndTower, textures);
            spriteBatch.Draw(bullets, textures);
            spriteBatch.Draw(explosions, textures);

            foreach(var burrel in map.burrels) 
            {
                spriteBatch.Draw(textures[burrel.ImageId], burrel.LTPoint.ToVector2(), Color.White);
            }

            spriteBatch.End();
        }
    }
}
