using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NOTGalaga
{
    public class ProjectileManager
    {
        Game1 game { get; set; }
        public List<Projectile> friendlyProjectiles { get; set; }
        static public List<Projectile> enemyProjectiles { get; set; }
        static Dictionary<projectileType, Texture2D> textures_;
        GraphicsDeviceManager Graphics { get; set; }

        public enum projectileType
        {
            laser
        }

        public ProjectileManager(Game1 game, Dictionary<projectileType, Texture2D> textures, GraphicsDeviceManager graphics)
        {
            this.game = game;
            textures_ = textures;
            friendlyProjectiles = new List<Projectile>();
            enemyProjectiles = new List<Projectile>();
            Graphics = graphics;
        }

        public void Update(GameTime gameTime)
        {

            //Also, maybe we want to use a backward iterative approach like we use with the enemies
            //Update each projectile
            for(int i = 0; i < friendlyProjectiles.Count; ++i)
            {
                friendlyProjectiles[i].Update(gameTime);

                //This works but it is a little questionable
                bool outOfBounds = friendlyProjectiles[i].destinationRectangle.Y < 0 || friendlyProjectiles[i].destinationRectangle.X < 0 || friendlyProjectiles[i].destinationRectangle.Y > Graphics.PreferredBackBufferHeight || friendlyProjectiles[i].destinationRectangle.X > Graphics.PreferredBackBufferWidth;

                if (friendlyProjectiles[i].lifeTime <= 0 || outOfBounds)
                {
                    friendlyProjectiles.RemoveAt(i);
                    i--;
                }
            }

            //Now enemy projectiles
            for (int i = 0; i < enemyProjectiles.Count; ++i)
            {
                enemyProjectiles[i].Update(gameTime);

                //This works but it is a little questionable
                bool outOfBounds = enemyProjectiles[i].destinationRectangle.Y < 0 || enemyProjectiles[i].destinationRectangle.X < 0 || enemyProjectiles[i].destinationRectangle.Y > Graphics.PreferredBackBufferHeight || enemyProjectiles[i].destinationRectangle.X > Graphics.PreferredBackBufferWidth;

                if (enemyProjectiles[i].lifeTime <= 0 || outOfBounds)
                {
                    enemyProjectiles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Projectile projectile in friendlyProjectiles)
            {
                projectile.Draw(spriteBatch);
            }
            foreach(Projectile projectile in enemyProjectiles)
            {
                projectile.Draw(spriteBatch);
            }
        }

        public void CreateFriendlyProjectile(projectileType type, Vector2 location, double lifetime)
        {
            Vector2 velocity = new Vector2(0, -7);
            switch (type)
            {
                case projectileType.laser:
                    Projectile newProjectile = new Projectile(textures_[type], 2, 1, location, velocity, 0f, lifetime);
                    //Projectile newProjectile = new Projectile(textures_[type], 1, 2, location, velocity, 0f, lifetime);
                    friendlyProjectiles.Add(newProjectile);
                    break;
                default:
                    break;
            }
        }

        public void RemoveFriendlyProjectile(Projectile projectile)
        {
            friendlyProjectiles.Remove(projectile);
        }

        static public void CreateEnemyProjectile(projectileType type, Vector2 location, Vector2 velocity, double lifetime)
        {
            float angle = (float)(Math.Atan2(velocity.Y, velocity.X) + (Math.PI / 2)); //The axis is shifted 90 degrees for the sprite batches
            switch (type)
            {
                case projectileType.laser:
                    Projectile newProjectile = new Projectile(textures_[type], 2, 1, location, velocity, angle, lifetime);
                    //Projectile newProjectile = new Projectile(textures_[type], 1, 2, location, velocity, 0f, lifetime);
                    enemyProjectiles.Add(newProjectile);
                    break;
                default:
                    break;
            }
        }

        public void RemoveEnemyProjectile(Projectile projectile)
        {
            enemyProjectiles.Remove(projectile);
        }
    }
}
