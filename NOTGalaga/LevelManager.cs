using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NOTGalaga
{
    class LevelManager
    {
        public List<Level> levels;
        //public List<XElement> levels;
        public int current_level;

        public EnemyManager enemyManager;   //This should be static for sure

        public LevelManager(/*XDocument levels,*/ EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
            levels = new List<Level>();
            /*current_level = 0;
            foreach(XElement level in levels.Elements("Level"))
            {
                //How do i want to do this? I think I want to have multiple levels, and each level has a set of waves
                //Each wave will have a set of instructions, like what enemies to spawn, where to spawn them, and instructions to wait after spawning certain enemies
                //So I want to keep track of what level we are on, and what wave we are on, and which instruction we are on.
                Level temp = new Level(level, enemyManager);
                this.levels.Add(temp);
            }*/
        }

        public void Update (GameTime gameTime)
        {
            if(current_level >= levels.Count)
            {
                return;
            }
            levels[current_level].Update(gameTime);
            if(levels[current_level].state == Level.State.finished)
            {
                current_level++;
                /*if(current_level >= levels.Count)
                {
                    //Do something to end the level sequence. Like return to a menu, when we have that implemented
                    current_level = 0;
                }*/
            }
        }

        public void Draw (SpriteBatch spriteBatch)
        {

        }

        public void LoadLevels(XDocument levels)
        {
            current_level = 0;

            foreach (XElement level in levels.Root.Elements("Level"))
            {
                //How do i want to do this? I think I want to have multiple levels, and each level has a set of waves
                //Each wave will have a set of instructions, like what enemies to spawn, where to spawn them, and instructions to wait after spawning certain enemies
                //So I want to keep track of what level we are on, and what wave we are on, and which instruction we are on.
                Level temp = new Level(level, enemyManager);
                this.levels.Add(temp);
            }
        }
    }
}
