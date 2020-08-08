using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NOTGalaga
{
    public class Projectile
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private double animationTime;
        private double msPerFrame;

        public Vector2 velocity;
        public Rectangle destinationRectangle;
        public float angle;
        public double lifeTime;
        public int damage;


        public Projectile(Texture2D texture, int rows, int columns, Vector2 location, Vector2 velocity, float angle, double lifeTime)
        {
            Texture = texture;
            Columns = columns;
            Rows = rows;
            currentFrame = 0;
            totalFrames = rows * columns;
            animationTime = 0f;
            msPerFrame = 250f;

            this.angle = angle;
            this.velocity = velocity;
            this.lifeTime = lifeTime;
            damage = 10;

            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, Texture.Width / Columns, Texture.Height / Rows);
        }

        public void Update(GameTime gameTime)
        {
            lifeTime -= gameTime.ElapsedGameTime.TotalMilliseconds;

            
            animationTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (animationTime > msPerFrame)
            {
                currentFrame = (currentFrame + 1) % totalFrames;
                animationTime = 0f;
            }
            

            destinationRectangle.X += (int)velocity.X;
            destinationRectangle.Y += (int)velocity.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int sourceX = (currentFrame % Columns) * (int)(Texture.Width / Columns);
            int sourceY = ((int)((float)currentFrame / (float)Columns) * (int)(Texture.Height / Rows));

            Rectangle sourceRectangle = new Rectangle(sourceX, sourceY, Texture.Width / Columns, Texture.Height / Rows);
            Vector2 origin = new Vector2((Texture.Width / Columns) / 2, (Texture.Height / Rows) / 2);

            spriteBatch.Begin();

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, angle, origin, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
    }
}
