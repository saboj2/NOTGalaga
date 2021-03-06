﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace NOTGalaga
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics { get; set; }
        SpriteBatch spriteBatch;
        ProjectileManager projectileManager;
        EnemyManager enemyManager;
        LevelManager levelManager;

        int score;
        private SpriteFont score_font;
        enum state
        {
            Menu,
            Paused,
            Play
        }

        state gameState;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        public static Player player { get; set; }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gameState = state.Play;
            score = 0;

            //graphics.ToggleFullScreen();
            graphics.PreferredBackBufferWidth = 600;
            graphics.PreferredBackBufferHeight = 900;
            graphics.ApplyChanges();
            
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

            // TODO: use this.Content to load your game content here
            score_font = Content.Load<SpriteFont>("Score");

            //Initialize Projectile Manager
            Dictionary<ProjectileManager.projectileType, Texture2D> projectileTextures = new Dictionary<ProjectileManager.projectileType, Texture2D>();
            projectileTextures.Add(ProjectileManager.projectileType.laser, Content.Load<Texture2D>("Projectile Laser"));

            projectileManager = new ProjectileManager(this, projectileTextures, graphics);

            //Initialize Enemy Manager
            Dictionary<EnemyManager.enemyType, Texture2D> enemyTextures = new Dictionary<EnemyManager.enemyType, Texture2D>();
            enemyTextures.Add(EnemyManager.enemyType.shooter, Content.Load<Texture2D>("Enemy Shooter"));
            enemyManager = new EnemyManager(this, enemyTextures, projectileManager);

            //Initialize player
            Vector2 playerLocation = new Vector2(graphics.PreferredBackBufferWidth / 2, (float)(graphics.PreferredBackBufferHeight * 0.75));
            player = new Player(Content.Load<Texture2D>("Spaceship"), 1, 4, playerLocation, projectileManager);

            //Level level = Content.Load<Level>("NOTGalagaLevels");

            /*
            XmlDocument level = new XmlDocument();
            level.Load(".\\Content\\Xml\\NOTGalagaLevels.xml");
            XmlNodeList list = level.GetElementsByTagName("waves");
            Console.WriteLine(list.Count);
            */

            //Example of loading in levels. Ideally, we would want to do this 
            XDocument levels_doc = XDocument.Load(".\\Content\\Xml\\NOTGalagaLevels.xml");

            levelManager = new LevelManager(enemyManager);
            levelManager.LoadLevels(levels_doc);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeyboardState keys = Keyboard.GetState();

            if (gameState == state.Play)
            {
                //Update Projectiles
                projectileManager.Update(gameTime);

                //Update Player
                player.Update(keys, gameTime);

                //Update the state of the level
                levelManager.Update(gameTime);

                //Update Enemies
                enemyManager.Update(gameTime);
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkViolet);
            

            // TODO: Add your drawing code here

            if (gameState == state.Play)
            {
                //Draw Projectiles
                projectileManager.Draw(spriteBatch);

                //Draw Player
                player.Draw(spriteBatch);

                //Draw Enemies
                enemyManager.Draw(spriteBatch);

                spriteBatch.Begin();

                spriteBatch.DrawString(score_font, "Score: " + score.ToString(), new Vector2(15, 15), Color.White);

                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public void UpdateScore(int points)
        {
            score += points;
        }
    }
}
