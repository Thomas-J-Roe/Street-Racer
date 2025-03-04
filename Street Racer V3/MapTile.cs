using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace Street_Racer_V3
{
    public class MapTile : Sprite
    {

        public MapTile(Texture2D texture, Vector2 Origin, bool IsRoad) : base(texture, Origin)//intialises a public MapTile
        {
            this.IsRoad = IsRoad;//stores if tile is road or not
        }

        public bool IsRoad { get; }//initalises if tile is road or not
    }
}
