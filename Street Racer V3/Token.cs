using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Street_Racer_V3
{
    class Token : Sprite
    {
        public Token(Texture2D texture, Vector2 Origin) : base(texture, Origin)//intialises a public Token
        {
            MidPoint = new Vector2(11, 11);//sets the midpoint of the token
        }

        public void Update(Player player)//overrides the Update routine of sprite
        {

            Vector2 displacementfromPlayer = player.Position + player.MidPoint - Position - MidPoint;//calculates the displacement from the player
            if (displacementfromPlayer.Length() < 30 && displacementfromPlayer.Length() >= 0)//method for when the player is within 30 of the token
            {
                player.GotToken();//gets the token in player class

                Vector2 Origin = CheckOrigin();//checks if respawn point is valid
                while (Origin == player.Position || Origin == Position)//if the origin is the same as the player or the token then change the origin
                {
                    Origin = CheckOrigin();
                }


                Position = Origin;//sets the position to the new respawn point
            }
        }

    }
}
