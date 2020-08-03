using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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
        int idleDirection;
        ProjectileManager projectileManager { get; set; }
        Dictionary<enemyType, Texture2D> textures;

        public enum enemyType
        {
            shooter,
            invalid
        }

        public EnemyManager(Game1 game, Dictionary<enemyType, Texture2D> textures, /*GraphicsDeviceManager graphics,*/ ProjectileManager projectileManager)
        {
            this.game = game;
            this.textures = textures;
            this.projectileManager = projectileManager;
            enemies = new List<Enemy>();
            idleDirection = 1;
        }

        public void Update(GameTime gameTime)
        {
            //Go in reverse order so I don't interrupt the iteration
            for(var en = enemies.Count - 1; en >= 0; --en)
            {
                Enemy enemy = enemies[en];

                for (var proj = projectileManager.friendlyProjectiles.Count - 1; proj >= 0; --proj)
                {
                    Projectile projectile = projectileManager.friendlyProjectiles[proj];
                    if (enemy.destinationRectangle.Intersects(projectile.destinationRectangle))
                    {
                        enemy.wasHit(projectile.damage);
                        projectileManager.RemoveFriendlyProjectile(projectile);
                    }
                }

                //Know what, lets not do the idle stuff. Lets instead give them a timed pattern to follow
                if (enemy.state == Enemy.enemyState.idle)
                {
                    //The enemy is going to be spawned at some initial location.
                    //Then they will sit there for a few seconds, and then move to a new location.
                    //Then they will sit there for a few seconds, and then will fly off screen.

                    //I think I need some kind of state machine. And maybe the enemies themselves can deal with this
                    //What happens when I want 
                    if (enemy.idleTimer <= 0)
                    {
                        enemy.moveTo(enemy.current_location + new Vector2(0, 600), 2); 

                        //After the enemies do this, we should move them off screen.
                    }
                }



                enemy.Update(gameTime);

                if (enemy.state == Enemy.enemyState.dead)
                {
                    enemies.Remove(enemy);
                    switch (enemy.type)
                    {
                        case EnemyManager.enemyType.shooter:
                            game.UpdateScore(100);
                            break;
                        default:
                            break;
                    }
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

        public Enemy CreateNewEnemy(EnemyManager.enemyType type, Vector2 location)
        {
            Vector2 velocity = new Vector2(0, -5);
            Vector2 spawn_location = new Vector2(100, -100);   //Spawn it off screen?
            Enemy newEnemy;
            switch (type)
            {
                case EnemyManager.enemyType.shooter:
                    //newEnemy = new Enemy(textures[type], 1, 4, location, (float)(Math.PI));
                    newEnemy = new Enemy(textures[type], 1, 4, spawn_location, (float)(Math.PI));
                    enemies.Add(newEnemy);
                    break;
                default:
                    newEnemy = null;
                    break;
            }

            newEnemy.moveTo(location, 2);
            return newEnemy;
        }

        public EnemyManager.enemyType StringToEnemyType(String type)
        {
            if (type.Equals("shooter", StringComparison.CurrentCultureIgnoreCase)) {
                return enemyType.shooter;
            }
            else
            {
                return enemyType.invalid;
            }
        }
    }
}
