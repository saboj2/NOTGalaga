using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NOTGalaga
{
    class EnemyShooter : Enemy
    {
        public EnemyS(Texture2D texture, int rows, int columns, Vector2 location, float angle)
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
            state = enemyState.enter;
            health = 100;
            //points = 100;

            idleTimeLimit = 7 * 1000;

            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, Texture.Width / Columns, Texture.Height / Rows);

        }
    }
}
