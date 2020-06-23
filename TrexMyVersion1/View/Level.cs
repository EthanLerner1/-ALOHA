using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrexMyVersion1.Objects;

namespace TrexMyVersion1.View
{
    public class Level : DrawObj
    {
        #region data
        public Point groundPos;
        #endregion

        #region ctor
        public Level (Texture2D txt,Texture2D mask,int amount, Vector2 position, Rectangle? sourceRectangle, Color color,
          float rotation, Vector2 origin, Vector2 scale,
          SpriteEffects effects, float layerDepth) : base (txt,amount, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth)
        {

            this.groundPos = groundHight(mask);
        }
        #endregion

        #region funcs

        /// <summary>
        /// retrives a random number that represents the amount
        /// of time the ground texture would be drawn
        /// </summary>
        /// <returns></returns>
        public static int generateGroundSize ()
        { 
            int amount = Tools.rnd.Next(50, 100);
            return 5;
        }


        /// <summary>
        /// sets the point of the ground hight
        /// </summary>
        /// <param name="m">Texture2D, the levels mask</param>
        /// <returns></returns>
        public Point groundHight (Texture2D m)
        {
            Color[] cm = new Color [m.Width*m.Height];
            m.GetData<Color>(cm);
            Color GPC = cm[0]; // Ground Point Color
            Point ret = Point.Zero;
            for (int y = 1; y < m.Height; y++)
            {
                for (int x = 0; x < m.Width; x++)
                {
                    if (cm[x + y * m.Width] == GPC)
                        ret = new Point(x, y);
                }

            }
            return ret; 
        }


        /// <summary>
        /// changing the level 
        /// </summary>
        /// <param name="txt"> Texture2D, new ground texture</param>
        /// <param name="mask">Texture2D, new Mask for the ground</param>
        public void changeLevel (Texture2D txt, Texture2D mask)
        {// maybe ill need to make a scale
            this.CurrentFrame = txt;
            this.amount = generateGroundSize();
            this.groundPos = groundHight(mask);
        }

        #endregion
    }
}
