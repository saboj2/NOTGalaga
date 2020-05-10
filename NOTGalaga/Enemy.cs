using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace NOTGalaga
{
    class Enemy
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
        public float speed;
        public int health;
        public int points;
        public Rectangle destinationRectangle;  //This is where the sprite currently is
        public Vector2 current_location;
        public enemyState state;
        public float angle;

        public enum enemyState
        {
            alive,
            dead
        }


        public Enemy (Texture2D texture, int rows, int columns, Vector2 location, float angle)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = rows * columns;
            msPerFrame = 250;
            color = Color.White;

            //this.angle = angle;
            this.current_location = location;
            state = enemyState.alive;
            health = 100;
            points = 100;


            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, Texture.Width / Columns, Texture.Height / Rows);

        }

        public void Update (GameTime gameTime)
        {
            if (health > 0)
            {

                
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


                //destinationRectangle.X += (int)velocity.X;
                //destinationRectangle.Y += (int)velocity.Y;

                //Vector2 current_location = new Vector2(destinationRectangle.X, destinationRectangle.Y);
                //if (!(current_location.Equals(destination)))
                //Distance to next point is greater than the distance to the destination
                if(Vector2.Distance(current_location, destination) > Vector2.Distance(current_location, current_location + velocity))
                {
                    //Vector2 new_location = Vector2.Lerp(current_location, destination, speed / Vector2.Distance(current_location, destination));
                    //Vector2 new_location = Vector2.SmoothStep(current_location, destination, speed / Vector2.Distance(current_location, destination));
                    //Vector2 new_location = Vector2.CatmullRom(current_location, destination, speed / Vector2.Distance(current_location, destination));

                    current_location += velocity;

                    //Converting to int is stopping the smooth transition from working. I need to keep track of the float in the background and only convert when moving the rectangle
                    destinationRectangle.X = (int)current_location.X;
                    destinationRectangle.Y = (int)current_location.Y;
                }
                else
                {
                    current_location = destination;
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
                        state = enemyState.dead;
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

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, color, angle, origin, SpriteEffects.None, 0f);

            spriteBatch.End();
        }

        public void wasHit(int damage)
        {
            health -= damage;
            color = Color.OrangeRed;
            hitAnimationTime = 250f;
        }

        public void moveTo(Vector2 destination, /*Vector2 velocity*/ float speed)
        {
            //this.velocity = velocity;   //Do i want to know this? I feel like this could be a speed value and we calculate the vector based on the destination
            //velocity = 
            velocity = Vector2.Subtract(destination, current_location);
            velocity.Normalize();
            velocity = Vector2.Multiply(velocity, speed);
            this.speed = speed;
            this.destination = destination;
        }

    }
}
