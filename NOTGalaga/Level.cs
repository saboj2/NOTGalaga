using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NOTGalaga
{
    class Level
    {
        //public int waves;
        public int current_wave;
        public XElement current_element;
        public double wave_timer;
        public List<XElement> waves;
        //XElement instructions;
        public State state;
        EnemyManager enemyManager;

        public enum State
        {
            inProgress,
            finished
        }

        public Level(XElement level, EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
            waves = new List<XElement>();
            current_wave = 0;
            foreach (XElement wave in level.Elements("Wave"))
            {
                waves.Add(wave);
            }

            state = State.inProgress;
        }

        public void Update(GameTime gameTime)
        {
            if (wave_timer > 0)
            {
                wave_timer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            }
            else
            {
                /*
                //Here we want to go through the instructions
                if(current_element.Name == "Enemy")
                {
                  
                    EnemyManager.enemyType type = enemyManager.StringToEnemyType(current_element.Element("type").Value.ToString());
                    Vector2 location = new Vector2((float)Convert.ToDouble(current_element.Element("Location").Element("x").Value), (float)Convert.ToDouble(current_element.Element("Location").Element("x").Value));
                    enemyManager.CreateNewEnemy(type, location);
                }
                */

                if(current_wave == waves.Count)
                {
                    state = State.finished;
                    return;
                }

                //Here we unpack the wave and create all the enemies.
                current_element = waves[current_wave];

                foreach(XElement enemy in current_element.Elements("Enemy"))
                {
                    EnemyManager.enemyType type = enemyManager.StringToEnemyType(enemy.Element("type").Value.ToString());
                    Vector2 location = new Vector2((float)Convert.ToDouble(enemy.Element("Location").Element("x").Value), (float)Convert.ToDouble(enemy.Element("Location").Element("y").Value));
                    enemyManager.CreateNewEnemy(type, location); //Returns enemy if we want to do anything with it.
                }

                //Set the wave_timer
                wave_timer = Convert.ToDouble(current_element.Element("wait").Value) * 1000;

                //And set make sure after the timer is up we move on to the next wave
                current_wave++;

            }
            //enemyManager.Update(gameTime); //I might not even need this. This might be able to be handled from the main loop actually
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
