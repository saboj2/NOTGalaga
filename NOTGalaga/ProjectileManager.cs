using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NOTGalaga
{
    class ProjectileManager
    {
        Game1 game { get; set; }
        public List<Projectile> friendlyProjectiles { get; set; }
        public List<Projectile> enemyProjectiles { get; set; }
        Dictionary<projectileType, Texture2D> textures_;
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

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Projectile projectile in friendlyProjectiles)
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

        public void CreateEnemyProjectile(projectileType type, Vector2 location, double lifetime)
        {

        }

        public void RemoveEnemyProjectile()
        {

        }
    }
}
