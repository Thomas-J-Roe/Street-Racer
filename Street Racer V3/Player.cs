using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;

namespace Street_Racer_V3
{
    public class Player : Sprite  //inherits classes from the sprite class
    {
        private const float speed = 2f;//initialises the speed of the player
        public int Health  = 100;//initialises the health of the player
        public int TokensLeft = 3;//initialises the tokens left for the player
        private Vector2 direction = Vector2.Zero;//initialises last direction
        private Texture2D PlayerTextureUp;//initialises the texture for the player moving up
        public Texture2D PlayerTextureDown;//initialises the texture for the player moving down
        public Texture2D PlayerTextureLeft;//initialises the texture for the player moving left
        public Texture2D PlayerTextureRight;//initialises the texture for the player moving right
        public bool EscapeInGame = false;//initialises if the player has pressed escape in game
        public Player(Texture2D textureup, Vector2 Origin, Texture2D texturedown, Texture2D textureleft, Texture2D textureright) : base(textureup, Origin)//intialises a public Player
        {
            MidPoint = new Vector2(12, 12);//sets the midpoint of the player
            PlayerTextureUp = textureup;//sets the texture of the player moving up
            PlayerTextureDown = texturedown;//sets the texture of the player moving down
            PlayerTextureLeft = textureleft;//sets the texture of the player moving left
            PlayerTextureRight = textureright;//sets the texture of the player moving right
        }

        public void Hit()//method for when the player is hit
        {
            Health -= 25;
            if (Health == 0)
            {

            }
        }
        public void Healed()//method for when the player is healed
        {
            Health = 100;
        }
        public void GotToken()//method for when the player gets a token
        {
            TokensLeft--;
            if (TokensLeft == 0)
            {

            }
        }
        public override void Update()// overrides the Update routine of sprite
        {
            var state = Keyboard.GetState();//stores state of keyboard
            if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))//changes the player movement for each of the WASD. 
            {
                direction = new Vector2(0, -1);//changes the direction of the player
                Texture = PlayerTextureUp;//changes the texture of the player
            }
            else if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))//same process as if statement
            {
                direction = new Vector2(0, 1);
                Texture = PlayerTextureDown;
            }

            else if (state.IsKeyDown(Keys.A)||state.IsKeyDown(Keys.Left))//same process as if statement
            {
                direction = new Vector2(-1, 0);
                Texture = PlayerTextureLeft;
            }
            else if (state.IsKeyDown(Keys.D)|| state.IsKeyDown(Keys.Right))//same process as if statement
            {
                direction = new Vector2(1, 0);
                Texture = PlayerTextureRight;
            }
            else if (state.IsKeyDown(Keys.Escape))//if the player presses escape then the game will exit
            {
                EscapeInGame = true;
            }
            else
            {

            }
            if(OutOfBounds(direction * speed))//if the player is out of bounds then the player will stop moving
            {
                direction = Vector2.Zero;
                Texture = PlayerTextureUp;
            }

            Position += direction * speed;//moves the player
        }
    }
}
