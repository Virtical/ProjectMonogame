using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using TankMonogame.Model;
using TankMonogame.Shared.Interface;
using TankMonogame.Shared.Enums;
using Microsoft.Xna.Framework.Content;
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
        public event EventHandler<GameTime> UndergroundLauncherShot = delegate { };
        public event EventHandler<EventArgs> StopUndergroundLauncherShot = delegate { };
        public event EventHandler<ContentManager> LoadContentOnModel = delegate { };

        private Map map;
        private TankHull tankHull;
        private Turret turret;
        private UndergroundLauncher undergroundLauncher;
        private List<Lava> burnPoint;
        private List<Bullet> bullets;
        private List<Explosion> explosions;
        private List<Rocket> rockets;


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
            LoadContentOnModel.Invoke(this, Content);
        }

        public void LoadGameCycleParameters(Map map, TankHull tankHull, Turret turret, List<Bullet> bullets, List<Explosion> explosions, UndergroundLauncher undergroundLauncher, List<Lava> burnPoint, List<Rocket> rockets)
        {
            this.map = map;
            this.tankHull = tankHull;
            this.turret = turret;
            this.bullets = bullets;
            this.explosions = explosions;
            this.undergroundLauncher = undergroundLauncher;
            this.burnPoint = burnPoint;
            this.rockets = rockets;
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

            spriteBatch.Draw(map);
            spriteBatch.Draw(bullets);
            spriteBatch.Draw(explosions);
            spriteBatch.Draw(map.burrels);
            spriteBatch.Draw(undergroundLauncher);
            spriteBatch.Draw(burnPoint);
            spriteBatch.Draw(rockets);
            spriteBatch.Draw(tankHull);
            spriteBatch.Draw(turret);

            spriteBatch.End();
        }
    }
}
