using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrexMyVersion1.Objects
{
    public class Flag : DrawObj
    {
        #region data
        public Boolean flag;
        #endregion
        public Flag(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color,
          float rotation, Vector2 origin, Vector2 scale,
          SpriteEffects effects, float layerDepth) : base(texture,position,sourceRectangle,color,rotation,origin,scale,effects,layerDepth)
        {
            flag = false;
        }

        public static Vector2 findTrafficLightPos()
        {
            float Y = (Global.bg.currentLevel.groundPos.Y * Global.bg.currentLevel.scale.X) - (Global.me.CurrentFrame.Height * Global.playerScale) - 10;
            float X = Global.me.Position.X;
            return new Vector2(X, Y);
        }
        public static  Vector2 findFinishLinePos()
        {
            float Y = (Global.bg.currentLevel.groundPos.Y * Global.bg.currentLevel.scale.X) - (Global.me.CurrentFrame.Height * Global.playerScale) - 10;
            float X = (Global.bg.currentLevel.amount-1) * Global.bg.currentLevel.CurrentFrame.Width * Global.scale + Global.bg.currentLevel.Position.X;
            return new Vector2(X, Y);
        }
    }
}
