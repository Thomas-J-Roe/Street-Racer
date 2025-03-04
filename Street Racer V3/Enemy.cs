using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Street_Racer_V3
{
    public class Enemy : Sprite
    {
        private Vector2 Direction;//initalises direction
        public float speed = 1f;//initalises speed
        private int timewait = 0;//initalises time wait
        private int chanceofchange = 3;//initalises chance of change in direction

        public Enemy(Texture2D texture, Vector2 Origin) : base(texture, Origin)//intialises a public Enemy
        {
            MidPoint = new Vector2(13, 9);//sets the midpoint of the enemy
        }

        public void Update(Player player)//overrides the Update routine of sprite
        {
            
            Vector2 displacementfromPlayer = player.Position + player.MidPoint - Position - MidPoint;//calculates the displacement from the player
            if (displacementfromPlayer.Length() > 20 )//method for random movement
            {
                speed = 1f;
                if (Direction == Vector2.Zero)//if the direction is zero then change direction immediately
                {
                    ChangeDirection();
                }
                else if (timewait == 0)//if the time to wait for next move is zero then change direction
                {
                    int changedirection = random.Next(1, chanceofchange + 1);//randomly chooses whether to change direction
                    if (OutOfBounds(Direction * timewait))//if the direction is out of bounds then change direction
                    {
                        changedirection = 1;
                    }

                    if (changedirection == 1)//changes direction
                    {
                        ChangeDirection();
                    }
                    else
                    {
                        changedirection--;
                    }
                    timewait = 5;//sets the time to wait for next move

                }
                else
                {
                    timewait--;//decreases the time to wait for next move
                }

                Position += Direction * speed;//moves the enemy
            }
            else if (displacementfromPlayer.Length() < 5 && displacementfromPlayer.Length() >= 0)//method if the player is within 5 of the enemy
            {
                player.Hit();//calls the hit method from the player class

                Vector2 Origin = CheckOrigin();//checks if respawn point is valid
                while (Origin == player.Position||Origin == Position)//if the respawn point is the same as the player or is original place then choose a new respawn point
                {
                    Origin = CheckOrigin();
                }

                Position = Origin;//sets the position to the new respawn point

                Direction = Vector2.Zero;//sets the direction to zero
            }
            else if (displacementfromPlayer.Length() <= 20)//method if the player is within 20 of the enemy
            {
                speed = 1.5f;//sets the speed to 1.5
                displacementfromPlayer.Normalize();//makes the displacement from the player a unit vector
                Position.X += displacementfromPlayer.X;//moves the enemy in x direction
                Position.Y += displacementfromPlayer.Y;//moves the enemy in y direction
                Position += displacementfromPlayer * speed;//moves the enemy
            }
            else
            {

            }

        }
        public void ChangeDirection()//method to change direction
        {
            int randomdirection = random.Next(0, 4);//randomly chooses a direction
            switch (randomdirection)
            {
                case 0:
                    if (Direction != new Vector2(0, 1))//if the direction is not the opposite of the current direction then change direction
                    {
                        Direction = new Vector2(0, -1);
                    }
                    else
                    {
                        Direction = new Vector2(0, 1);//if the direction is the opposite of the current direction then don't direction
                    }
                    if (OutOfBounds(Direction*20))//if the direction is out of bounds then set direction to zero
                    {
                        Direction = Vector2.Zero;
                    }
                    break;
                case 1: //does the same as case 0 but for down direction
                    if (Direction != new Vector2(0, -1))
                    {
                        Direction = new Vector2(0, 1);
                    }
                    else
                    {
                        Direction = new Vector2( 0, -1);
                    }
                    if (OutOfBounds(Direction * 20))
                    {
                        Direction = Vector2.Zero;
                    }
                    break;
                case 2://does the same as case 0 but for left direction
                    if (Direction != new Vector2(1, 0))
                    {
                        Direction = new Vector2(-1, 0);
                    }
                    else
                    {
                        Direction = new Vector2(1, 0);
                    }
                    if (OutOfBounds(Direction * 20))
                    {
                        Direction = Vector2.Zero;
                    }
                    break;
                case 3://does the same as case 0 but for right direction
                    if (Direction != new Vector2(-1, 0))
                    {
                        Direction = new Vector2(1, 0);
                    }
                    else
                    {
                        Direction = new Vector2(-1, 0);
                    }
                    if (OutOfBounds(Direction * 20))
                    {
                        Direction = Vector2.Zero;
                    }
                    break;
                default:
                    break;

            }
        }

        
    }
}
