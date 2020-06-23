using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrexMyVersion1.Physics;

namespace TrexMyVersion1.Objects
{
    class Cloud : MovingObj
    {
        #region ctor
        /// <summary>
        /// constructor for a cloud
        /// </summary>
        /// <param name="pase"></param>
        /// <param name="drc"></param>
        /// <param name="texture"></param>
        /// <param name="position"></param>
        /// <param name="sourceRectangle"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        /// <param name="pm"></param>
        public Cloud(int pase, DRC drc, Texture2D texture, Vector2 position,
         Rectangle? sourceRectangle, Color color,
         float rotation, Vector2 origin, Vector2 scale,
         SpriteEffects effects, float layerDepth, PhysicsManager pm) : base(pase, drc, texture, position,
          sourceRectangle, color, rotation, origin, scale, effects, layerDepth, pm)
        {

        }

        #endregion

        #region
        public static List<Cloud> generateClouds(Texture2D[] tex, int pase, DRC drc, Vector2 scale)
        {
            List<Cloud> ret = new List<Cloud>(0);
            float length = 100 + (Global.bg.currentLevel.amount * Global.bg.currentLevel.CurrentFrame.Width * Global.scale);
            int i = 0;
            //float posY = Global.me.Position.Y -(Global.me.CurrentFrame.Height * Global.playerScale * 1.5f);
            float posY = (Global.bg.currentLevel.groundPos.Y*Global.scale) - (Global.me.CurrentFrame.Height * Global.playerScale * 2f);
            Vector2 pos = new Vector2(-200, posY);
            while (length>-200)
            {
                if (i == tex.Length)
                    i = 0;
                ret.Add(new Cloud(pase, drc, tex[i], pos, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0.2f, Tools.pm));
                pos = new Vector2(pos.X + (tex[i].Width * scale.X + 50), pos.Y);
                length -= (tex[i].Width * scale.X + 50);
                i++;
            }
            return ret;
        }
        #endregion
    }
}
