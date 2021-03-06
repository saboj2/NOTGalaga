﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NOTGalaga
{
    public abstract class Enemy 
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private double animationTime;
        private double hitAnimationTime;
        private double msPerFrame;

        public Color color { get; set; }

        public Vector2 destination; //This is where the entity is travelling to.
        public Vector2 velocity;
        //public float speed;
        public int Health { get; set; }
        //public int points; //This can be shared, each one doesn't need to keep track of this
        public EnemyManager.enemyType Type { get; set; }
        public Rectangle destinationRectangle;  //This is where the sprite currently is
        public Vector2 current_location;
        public enemyState State { get; set; }
        public float Angle { get; set; }

        protected double idleTimeLimit;
        protected double idleTimer;
        protected bool moved; //Is it ready to despawn and leave? This is kind of lame but it'll work

        public enum enemyState
        {
            //enter,
            idle,
            moving,
            //leave,
            dead
        }

        public Enemy ()
        {
            Texture = null;
            Rows = 0;
            Columns = 0;
            totalFrames = 0;
            State = enemyState.dead;
        }


        public Enemy(/*EnemyManager.enemyType type,*/ Texture2D texture, int rows, int columns, Vector2 location, float angle)
        {
            //Type = type;
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = rows * columns;
            msPerFrame = 250;
            color = Color.White;

            Angle = angle;
            this.current_location = location;
            State = enemyState.idle;

            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, Texture.Width / Columns, Texture.Height / Rows);
        }

        public void Update (GameTime gameTime)
        {
            if (Health > 0)
            {

                //Update Animation
                animationTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (animationTime > msPerFrame)
                {
                    //currentFrame = (currentFrame + 1) % totalFrames;
                    currentFrame = 0;
                    animationTime = 0f;

                }

                if(hitAnimationTime > 0)
                {
                    hitAnimationTime -= gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (hitAnimationTime <= 0)
                    {
                        color = Color.White;
                    }
                }

                //Call class specific pathing logic
                UpdateMovement(gameTime);

                //Call class specific attacking logic
                UpdateAttack(gameTime);

                //Movement
                if (State == enemyState.moving)
                {
                    if (Vector2.Distance(current_location, destination) > Vector2.Distance(current_location, current_location + velocity)) //Distance to next point is greater than the distance to the destination
                    {

                        current_location += velocity;

                        //Converting to int is stopping the smooth transition from working. I need to keep track of the float in the background and only convert when moving the rectangle
                        destinationRectangle.X = (int)current_location.X;
                        destinationRectangle.Y = (int)current_location.Y;
                    }
                    else
                    {
                        current_location = destination;
                        State = enemyState.idle;
                        //idleTimer = idleTimeLimit;
                    }
                }

            }
            else
            {
                //Death sequence. Death animation? How does it affect other enemies or the players? Does it drop anything?
                color = Color.White;

                animationTime += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (animationTime > msPerFrame)
                {
                    if(currentFrame == totalFrames)
                    {
                        currentFrame = 1;
                        State = enemyState.dead;
                    }
                    else
                    {
                        currentFrame += 1;
                    }
                    animationTime = 0f;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int sourceX = (currentFrame % Columns) * (int)(Texture.Width / Columns);
            int sourceY = ((int)((float)currentFrame / (float)Columns) * (int)(Texture.Height / Rows));

            Rectangle sourceRectangle = new Rectangle(sourceX, sourceY, Texture.Width / Columns, Texture.Height / Rows);
            Vector2 origin = new Vector2((Texture.Width / Columns) / 2, (Texture.Height / Rows) / 2);
            //Vector2 origin = new Vector2((Texture.Width / Columns) / 2, 0);

            spriteBatch.Begin();

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, color, Angle, origin, SpriteEffects.None, 0f);

            spriteBatch.End();
        }

        public void wasHit(int damage)
        {
            Health -= damage;
            color = Color.OrangeRed;
            hitAnimationTime = 250f;
        }

        //We can override this if we want to add some different movement gic
        public virtual void moveTo(Vector2 destination, /*Vector2 velocity*/ float speed)
        {

            velocity = Vector2.Subtract(destination, current_location);
            velocity.Normalize();
            velocity = Vector2.Multiply(velocity, speed);

            this.destination = destination;
            //State = (moved) ? enemyState.leave : enemyState.moving;
            State = enemyState.moving;
        }

        //Subclasses will have to implement their own movement and attacking logic
        public abstract void UpdateMovement(GameTime gameTime);

        public abstract void UpdateAttack(GameTime gameTime);

    }
}
