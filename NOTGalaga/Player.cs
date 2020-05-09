using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace NOTGalaga
{
    class Player
    {

        ProjectileManager ProjectileManager { get; set; }
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;

        public Vector2 location;
        public float speed;
        private int health;
        private double cooldown;
        private double cooldownLimit;

        public Player(Texture2D texture, int rows, int columns, Vector2 location, ProjectileManager projectileManager)
        {
            ProjectileManager = projectileManager;

            Texture = texture;
            Rows = rows;
            Columns = columns;
            cooldownLimit = 500;

            currentFrame = 0;
            totalFrames = rows * columns;
            health = 100;
            speed = 500;
            //location = new Vector2(0, 0);
            this.location = location;
            cooldown = 0;
           
        }

        public void Update(KeyboardState keys, GameTime gameTime)
        {
            //Detect Collisions
            List<Projectile> projectiles = ProjectileManager.enemyProjectiles;
            foreach (Projectile projectile in projectiles)
            {
                /*
                if(projectile.destinationRectangle.Intersects()
                { 
                
                }
                */
            }

            if (health > 0)
            {
                //Process input

                //Update animation
                currentFrame = (currentFrame + 1) % totalFrames; 

                //Update Location
                if (keys.IsKeyDown(Keys.W)) {
                    location.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (keys.IsKeyDown(Keys.S)) {
                    location.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (keys.IsKeyDown(Keys.A)) {
                    location.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (keys.IsKeyDown(Keys.D)) {
                    location.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if(keys.IsKeyDown(Keys.Space))
                {
                    if (cooldown <= 0)
                    {
                        ProjectileManager.CreateFriendlyProjectile(ProjectileManager.projectileType.laser, new Vector2(location.X + ((Texture.Width / Columns) / 2), location.Y), 2000f);
                        cooldown = cooldownLimit;
                    }
                }

                if (cooldown > 0)
                {
                    cooldown -= gameTime.ElapsedGameTime.TotalMilliseconds;
                }
            }
            else
            {
                //Death sequence. Death animation? How does it affect other enemies or the players? Does it drop anything?
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle( currentFrame * (Texture.Width / Columns), 0, Texture.Width / Columns, Texture.Height / Rows);
            //Rectangle destinationsRectangle = new Rectangle();
            Vector2 origin = new Vector2((Texture.Width / Columns) / 2, (Texture.Height / Rows) / 2);

            spriteBatch.Begin();

            spriteBatch.Draw(Texture, location, sourceRectangle, Color.White);

            spriteBatch.End();
        }

    }
}
