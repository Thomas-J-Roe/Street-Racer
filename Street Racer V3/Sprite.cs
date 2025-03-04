using Microsoft.Xna.Framework;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Street_Racer_V3
{
    public class Sprite 
    {
        protected Texture2D Texture;//initialised sprite's texture 
        public Vector2 Position; //initialised sprite's position 
        public Vector2 MidPoint; //initialised sprite's midpoint 
        public Random random = new Random();//initialised random function
        public static List<Vector2> ValidSpawns = new List<Vector2>();//initialised static list of valid spawns gained from the NewGame class
        public static bool[,] ValidTiles;//creates a 2D array of tiles that are valid for the sprite to move on
        public Sprite(Texture2D texture, Vector2 Origin)//initialises the sprite
        {
            Texture = texture;
            Position = Origin;

        }

        public virtual void Update()//creates an update function for the individual sprite classes
        {
        }

        public void Draw(SpriteBatch spriteBatch)//draws the sprite
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        public bool ValidPos(Vector2 nextpos)//checks if the next position is valid
        {
            int posX = (int)nextpos.X / 25;
            int posY = (int)nextpos.Y / 25;
            return ValidTiles[posX, posY];
        }
        public bool OutOfBounds(Vector2 Direction)//checks if the sprite is out of bounds at current direction
        {
            Vector2 nextpos = Position + MidPoint;
            nextpos += Direction;
            if (nextpos.X < 140 || nextpos.X > 488 || nextpos.Y < 75 || nextpos.Y > 510)
            {
                return true;
            }
            if (!ValidPos(nextpos))
            {
                return true;
            }
            return false;
        }
        public Vector2 CheckOrigin()//checks if the origin of the sprite is valid
        {
            int choice = random.Next(0, ValidSpawns.Count - 1);
            Vector2 Origin = ValidSpawns[choice];
            return Origin;
        }
        
    }
}
