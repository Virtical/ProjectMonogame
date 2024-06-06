using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TankMonogame.Model;
using TankMonogame.Shared.Interface;
using TankMonogame.Shared.Enums;
using System.Diagnostics;
namespace TankMonogame.View
{
    public class GameplayView : Game, IGameplayView
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public event EventHandler<EventArgs> CycleFinished = delegate { };
        public event EventHandler<DirectionOfMovement> TankMoved = delegate { };
        public event EventHandler<DirectionOfRotation> TankRotate = delegate { };
        public event EventHandler<EventArgs> PlayerSlowdownSpeed = delegate { };
        public event EventHandler<EventArgs> PlayerSlowdownRotate = delegate { };
        public event EventHandler<GameTime> TankShoot = delegate { };
        public event EventHandler<EventArgs> StopTankShoot = delegate { };
        public event EventHandler<MouseState> TurretRotate = delegate { };
        public event EventHandler<GameTime> UndergroundLauncherShot;
        public event EventHandler<EventArgs> StopUndergroundLauncherShot;

        private Map map;
        private TankHull tankHull;
        private Turret turret;
        private UndergroundLauncher undergroundLauncher;
        private Queue<Point> burnPoint;
        private List<Bullet> bullets;
        private List<Explosion> explosions;

        private Dictionary<TypeCell, Texture2D> textureCell = new Dictionary<TypeCell, Texture2D>();
        private Dictionary<int, Texture2D> textures = new Dictionary<int, Texture2D>();

        Texture2D pointTexture;


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
            textures.Add(6, Content.Load<Texture2D>("UndergroundLauncher"));
            textures.Add(7, Content.Load<Texture2D>("Lava"));

            textureCell[TypeCell.Level1] = Content.Load<Texture2D>("FloorLevel1");
            textureCell[TypeCell.Level2] = Content.Load<Texture2D>("FloorLevel2");
            textureCell[TypeCell.Level3] = Content.Load<Texture2D>("FloorLevel3");
            textureCell[TypeCell.Level4] = Content.Load<Texture2D>("FloorLevel4");
            textureCell[TypeCell.Level5] = Content.Load<Texture2D>("FloorLevel5");
            textureCell[TypeCell.Level6] = Content.Load<Texture2D>("FloorLevel6");
            textureCell[TypeCell.Level7] = Content.Load<Texture2D>("FloorLevel7");
            textureCell[TypeCell.Level8] = Content.Load<Texture2D>("FloorLevel8");

            pointTexture = new Texture2D(GraphicsDevice, 3, 3);
            Color[] colorData = new Color[9];
            for (int i = 0; i < colorData.Length; ++i)
                colorData[i] = Color.Red;

            pointTexture.SetData(colorData);
        }

        public void LoadGameCycleParameters(Map map, TankHull tankHull, Turret turret, List<Bullet> bullets, List<Explosion> explosions, UndergroundLauncher undergroundLauncher, Queue<Point> burnPoint)
        {
            this.map = map;
            this.tankHull = tankHull;
            this.turret = turret;
            this.bullets = bullets;
            this.explosions = explosions;
            this.undergroundLauncher = undergroundLauncher;
            this.burnPoint = burnPoint;
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
            bool isUndergroundLauncherShot = false;

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
                    case Keys.Space:
                        UndergroundLauncherShot.Invoke(this, gameTime);
                        isUndergroundLauncherShot = true;
                        break;
                    case Keys.Escape:
                        Exit();
                        break;
                }
            }

            if (!isUndergroundLauncherShot)
            {
                StopUndergroundLauncherShot.Invoke(this, EventArgs.Empty);
            }

            if (!isMoving)
            {
                PlayerSlowdownSpeed.Invoke(this, EventArgs.Empty);
            }

            if (!isRotating)
            {
                PlayerSlowdownRotate.Invoke(this, EventArgs.Empty);
            }

            CycleFinished.Invoke(this, EventArgs.Empty);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(map, textureCell);
            spriteBatch.Draw(tankHull, textures);
            spriteBatch.Draw(turret, textures);
            spriteBatch.Draw(bullets, textures);
            spriteBatch.Draw(explosions, textures);
            spriteBatch.Draw(map.burrels, textures);

            Rectangle sourceRectangle = new Rectangle(59 * undergroundLauncher.AnimationFrame, 0, 59, 78);
            spriteBatch.Draw(textures[undergroundLauncher.ImageId], undergroundLauncher.Pos, sourceRectangle, Color.White, undergroundLauncher.Angle, undergroundLauncher.Anchor, 1.0f, SpriteEffects.None, 1);

            foreach (var p in burnPoint)
            {
                for (int x = -1; x < 2; x++)
                {
                    for (int y = -1; y < 2; y++)
                    {
                        spriteBatch.Draw(textures[7], (p + new Point(x * 64, y * 64)).ToVector2(), Color.White);
                    }
                }
            }

            spriteBatch.End();
        }
    }
}
