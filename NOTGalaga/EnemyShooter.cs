using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NOTGalaga
{
    public class EnemyShooter : Enemy
    {
        //Use this for pathing state machine
        private float projectileSpeed = 7;
        private float projectileLifetime = 3000f;
        private double attackCooldown = 1500f;
        private double attackCooldownTimer = 0;
        public moveState MoveState { get; set; }
        public enum moveState
        {
            enter,
            forward,
            straife,
            leaving
        }

        public EnemyShooter(Texture2D texture, int rows, int columns, Vector2 location, float angle) : base(texture, rows, columns, location, angle)
        {
            //Calls the base constructor first, then fills in these class specific fields.
            Type = EnemyManager.enemyType.shooter;
            Health = 100;
            idleTimeLimit = 7 * 1000;
            MoveState = EnemyShooter.moveState.enter;
        }

        public override void UpdateMovement(GameTime gameTime)
        {
            if (State == enemyState.idle)
            {
                //Idle means we finished a state, so we want to move to the next one.
                switch (MoveState)
                {
                    //I need to think about what this means. What happens when we spawn it? We're probably checking this after it's moved to its location
                    //So we need to make sure we are setting the state to enter and moving it before we get to this point
                    case EnemyShooter.moveState.enter:
                        //By this point it has been spawned and has moved to it's initial destination
                        //Now we want to move down the screen towards the player
                        forward();
                        break;
                    case EnemyShooter.moveState.forward:
                        //Now we want to move sideways off screen
                        straife();
                        break;
                    case EnemyShooter.moveState.straife:

                        //This is a little redunandant. But it leaves room to make some changes later on
                        MoveState = EnemyShooter.moveState.leaving;
                        break;
                    case EnemyShooter.moveState.leaving:

                        //Kill it off, it should despawn now
                        //Enemy manager will not update player score if it's health is > 0
                        State = Enemy.enemyState.dead;
                        break;
                    default:
                        break;
                }
            }


        }

        public override void UpdateAttack(GameTime gameTime)
        {
            attackCooldownTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (attackCooldownTimer < 0) {
                //For the shooter, this should be simple. All we need to do is shoot an enemy projectile, sent in the direction of the player, on some kind of cooldown
                //Find the player's position 
                Vector2 projectileVector = new Vector2();
                Vector2 playerLocation = Game1.player.location;
                //Find the vector connecting enemy position to player position
                projectileVector = Vector2.Subtract(playerLocation, current_location);
                projectileVector.Normalize();
                projectileVector = Vector2.Multiply(projectileVector, projectileSpeed);
                //Multiply it by the speed for this enemy

                ProjectileManager.CreateEnemyProjectile(ProjectileManager.projectileType.laser, current_location, projectileVector, projectileLifetime);

                attackCooldownTimer = attackCooldown;
            }
        }

        public void straife()
        {
            //We want to pick a location that is between perpendicular and -30 degress, on the side with the least space
            //First, we determine which side to move to. True means we move right
            bool side = (current_location.X >= (Game1.graphics.PreferredBackBufferWidth / 2));

            Random rnd = new Random();

            //We can use pythagorean theorem to generate some random x and y offsets based on the restrictions we have
            double xDist, yDist;
            xDist = (side) ? (Game1.graphics.PreferredBackBufferWidth - current_location.X + 100) : (current_location.X * -1) - 100;
            yDist = (rnd.NextDouble() * (1.0f - 0.5f) + 0.5f) * (Math.Abs(xDist) / 1.73f); //sqrt(3) approximation. It's a magic number, but it's not a commonly used constant

            Vector2 newLocation = new Vector2(current_location.X + (float)xDist, current_location.Y + (float)yDist);
            moveTo(newLocation, 2.0f);
            MoveState = EnemyShooter.moveState.straife;
        }

        public void forward()
        {
            //Find the distance to move. It's random, and is a percentage of the amount of screen space between the entity and the bottom. 50-80% seemed like an acceptable
            //range of space to make the enemy move
            Random rnd = new Random();
            double dist = (rnd.NextDouble() * (0.80f - 0.5f) + 0.5f) * (Game1.graphics.PreferredBackBufferHeight - current_location.Y);
            //New location is offset by previously calculated distance, and enemy is commanded to move to the new location
            Vector2 nextLocation = new Vector2(current_location.X, current_location.Y + (float)dist);
            moveTo(nextLocation, 2);
            MoveState = EnemyShooter.moveState.forward;
        }
    }
}
