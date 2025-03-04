using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Street_Racer_V3
{
    public class Camera
    {
        
        public Matrix Transformation { get; private set; }//initialises matrix transformation
        public static int ScreenHeight; //stores screenheight from NewGame
        public static int ScreenWidth; //stores screen width from NewGame
        private Vector2 Position;//initialises position
        public float zoom = 3f;//initialises zoom
        private Vector2 Origin;//initialises origin
        public Camera() //initialises camera
        {
            Origin = new Vector2(0, 0);//sets the origin

        } 
        public void Follow(Vector2 targetpos)//follows the target position
        {
            Position = targetpos ;

            Transformation = Matrix.CreateTranslation(-Position.X + 125, -Position.Y + 60, 0) * Matrix.CreateScale(zoom);
        }
    }
}
