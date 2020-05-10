using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NOTGalaga
{
    class EnemyManager
    {
        Game1 game;
        List<Enemy> enemies;
        ProjectileManager projectileManager { get; set; }
        Dictionary<enemyType, Texture2D> textures;

        public enum enemyType
        {
            shooter
        }

        public EnemyManager(Game1 game, Dictionary<enemyType, Texture2D> textures, /*GraphicsDeviceManager graphics,*/ ProjectileManager projectileManager)
        {
            this.game = game;
            this.textures = textures;
            this.projectileManager = projectileManager;
            enemies = new List<Enemy>();
        }

        public void Update(GameTime gameTime)
        {
            //So, maybe I want to have a place to run different scripts?
            //Different plans for attacking/enemy formations? 
            //These could be their own objects and files that include instructions and 

            //Go in reverse order so I don't interrupt the iteration
            for(var en = enemies.Count - 1; en >= 0; --en)
            {
                
                Enemy enemy = enemies[en];
                enemy.moveTo(new Vector2(200, 500), 50);
                //foreach (Projectile projectile in projectileManager.friendlyProjectiles)
                for (var proj = projectileManager.friendlyProjectiles.Count - 1; proj >= 0; --proj)
                {
                    Projectile projectile = projectileManager.friendlyProjectiles[proj];
                    if (enemy.destinationRectangle.Intersects(projectile.destinationRectangle))
                    {
                        enemy.wasHit(projectile.damage);
                        projectileManager.RemoveFriendlyProjectile(projectile);
                    }
                }

                enemy.Update(gameTime);

                if (enemy.state == Enemy.enemyState.dead)
                {
                    enemies.Remove(enemy);
                    game.UpdateScore(enemy.points);
                }
            }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        public void CreateNewEnemy(EnemyManager.enemyType type, Vector2 location)
        {
            Vector2 velocity = new Vector2(0, -5);
            switch (type)
            {
                case EnemyManager.enemyType.shooter:
                    Enemy newEnemy = new Enemy(textures[type], 1, 4, location, (float)(Math.PI));
                    enemies.Add(newEnemy);
                    break;
                default:
                    break;
            }
        }

    }
}
