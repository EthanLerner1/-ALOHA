using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrexMyVersion1.Physics;

//namespace TrexMyVersion1.Objects
//{
//    public class Gunner : Animation
//    {
//        #region data
//        MovingObj bullet;
//        Circle bulletC;
//        int bulletSpeed;
//        Vector2 bulletScale;
//        int delay, delaySave;
//        #endregion
//        public Gunner(DRC shootDrc, Texture2D tex, Texture2D spriteSheet, Texture2D spriteMask, abstractKeys keys, int engI, Engine eng, Vector2 position,
//            Rectangle? sourceRectangle, Color color,
//            float rotation, Vector2 origin, Vector2 scale,
//            SpriteEffects effects, float layerDepth, PhysicsManager pm) : base(tex, spriteSheet, spriteMask, keys, engI, eng, position, sourceRectangle, color
//                , rotation, origin, scale, effects, layerDepth, pm)
//        {
//            bulletSpeed = 50;
//            delay = 2;
//            delaySave = delay;
//            Texture2D bulletTex = Tools.cm.Load<Texture2D>("moreObj/bullet");
//            Vector2 firePos = new Vector2(0, 910);
//            Vector2 pos = position - (origin * scale) + (firePos * scale);
//            bulletC = new Circle(300, pos);
//            bulletScale = new Vector2(0.1f);
//            bullet = new MovingObj(bulletSpeed, shootDrc, bulletTex, pos, null, color, 0, new Vector2(280, 140), bulletScale, effects, 0, pm);
//            this.spriteActive = true;
//            Game1.Event_Update += update;
//            Background.Event_NextLevel += nextLevel;
//        }

//        private void update()
//        {

//        }

//        private void nextLevel()
//        {

//        }

//    }
//}
namespace TrexMyVersion1.Objects
{
    public class Gunner : Animation
    {
        #region data
        public Bullet bullet { get; set; }
        Circle bulletC;
        int bulletSpeed;
        Vector2 bulletScale;
        #endregion

        public Gunner(DRC shootDrc, Texture2D tex, Texture2D spriteSheet, Texture2D spriteMask, abstractKeys keys, int engI, Engine eng, Vector2 position,
            Rectangle? sourceRectangle, Color color,
            float rotation, Vector2 origin, Vector2 scale,
            SpriteEffects effects, float layerDepth, PhysicsManager pm) : base(tex, spriteSheet, spriteMask, keys, engI, eng, position, sourceRectangle, color
                , rotation, origin, scale, effects, layerDepth, pm)
        {
            //bullet
            bulletSpeed = 100;
            Texture2D bulletTex = Tools.cm.Load<Texture2D>("moreObj/bullet");
            Tools.makeTransparent(bulletTex);
            Vector2 firePos = new Vector2(0, 910);
            Vector2 pos = position - (origin * scale) + (firePos * scale);
            bulletScale = new Vector2(0.1f);
            bullet = new Bullet(this, Tools.cm.Load<Texture2D>("masks/bulletM"),
                bulletSpeed, shootDrc, bulletTex, pos, null, color, 0, new Vector2(280, 140), bulletScale, effects, 0, pm);
            this.spriteActive = true;
            Game1.Event_Update += update;
            Background.Event_NextLevel += nextLevel;
        }

        private void update()
        {

        }

        private void nextLevel()
        {
            this.Position = new Vector2(Position.X, Global.bg.currentLevel.groundPos.Y * Global.bg.currentLevel.scale.Y);
        }

    }
}

