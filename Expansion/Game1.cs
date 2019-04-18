using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Expansion
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private FrameCounter _frameCounter = new FrameCounter();

        private AnimatedSprite animatedSprite;

        private Texture2D arrow;
        private float angle = 0;

        private Texture2D blue;
        private Texture2D green;
        private Texture2D red;

        private float blueAngle = 0;
        private float greenAngle = 0;
        private float redAngle = 0;

        private float blueSpeed = 0.025f;
        private float greenSpeed = 0.017f;
        private float redSpeed = 0.022f;

        private float distance = 100;

        TimeSpan timer = TimeSpan.FromSeconds(.5);

        Texture2D ballTexture;
        Vector2 ballPosition;
        float ballSpeed;

        private SpriteFont font;
        private string fps = "";

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            //Unlock framerate.
            //this.IsFixedTimeStep = false;
            //graphics.SynchronizeWithVerticalRetrace = false;
            //graphics.ApplyChanges();
            
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            ballPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 100f;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ballTexture = Content.Load<Texture2D>("ball");

            font = Content.Load<SpriteFont>("Fonts/fps");

            Texture2D texture = Content.Load<Texture2D>("SmileyWalk");
            animatedSprite = new AnimatedSprite(texture, 4, 4);

            arrow = Content.Load<Texture2D>("arrow");

            blue = Content.Load<Texture2D>("blue");
            green = Content.Load<Texture2D>("green");
            red = Content.Load<Texture2D>("red");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameCounter.Update(deltaTime);

            if (timer > TimeSpan.Zero)
            {
                timer -= gameTime.ElapsedGameTime;
                if (timer <= TimeSpan.Zero)
                {
                    fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);
                    timer = TimeSpan.FromSeconds(.5);
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            var kstate = Keyboard.GetState();

            if (kstate.IsKeyDown(Keys.Up))
                ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Down))
                ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Left))
                ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Right))
                ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            ballPosition.X = Math.Min(Math.Max(ballTexture.Width / 2, ballPosition.X), graphics.PreferredBackBufferWidth - ballTexture.Width / 2);
            ballPosition.Y = Math.Min(Math.Max(ballTexture.Height / 2, ballPosition.Y), graphics.PreferredBackBufferHeight - ballTexture.Height / 2);

            animatedSprite.Update();

            angle += 0.01f;

            blueAngle += blueSpeed;
            greenAngle += greenSpeed;
            redAngle += redSpeed;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(
                ballTexture,
                ballPosition,
                null,
                Color.White,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
                );

            spriteBatch.DrawString(font, fps, new Vector2(100, 100), Color.Black);

            Vector2 location = new Vector2(400, 240);
            Rectangle sourceRectangle = new Rectangle(0, 0, arrow.Width, arrow.Height);
            Vector2 origin = new Vector2(arrow.Width / 2, arrow.Height / 2);

            spriteBatch.Draw(arrow, location, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1);

            Vector2 bluePosition = new Vector2(
                (float)Math.Cos(blueAngle) * distance,
                (float)Math.Sin(blueAngle) * distance);
            Vector2 greenPosition = new Vector2(
                            (float)Math.Cos(greenAngle) * distance,
                            (float)Math.Sin(greenAngle) * distance);
            Vector2 redPosition = new Vector2(
                            (float)Math.Cos(redAngle) * distance,
                            (float)Math.Sin(redAngle) * distance);

            Vector2 center = new Vector2(300, 140);

            spriteBatch.End();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive);

            spriteBatch.Draw(blue, center + bluePosition, Color.White);
            spriteBatch.Draw(green, center + greenPosition, Color.White);
            spriteBatch.Draw(red, center + redPosition, Color.White);

            spriteBatch.End();

            //This should be inside of the begin/end and not do it itself
            animatedSprite.Draw(spriteBatch, ballPosition);

            base.Draw(gameTime);
        }
    }
}
