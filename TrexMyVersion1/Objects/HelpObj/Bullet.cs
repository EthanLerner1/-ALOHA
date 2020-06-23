using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrexMyVersion1.Physics;

namespace TrexMyVersion1.Objects
{
    public class Bullet : MovingObj
    {
        #region data
        List<Circle> collisionCircles;
        public Circle area;
        private DrawObj gunner;
        #endregion
        public Bullet(DrawObj gunner,Texture2D mask, int pase, DRC drc, Texture2D texture, Vector2 position,
          Rectangle? sourceRectangle, Color color,
          float rotation, Vector2 origin, Vector2 scale,
          SpriteEffects effects, float layerDepth, PhysicsManager pm) : base(pase, drc, texture, position,
           sourceRectangle, color, rotation, origin, scale, effects, layerDepth, pm)
        {
            this.gunner = gunner;
            area = new Circle(300, new Vector2(300, 170));
            this.collisionCircles = new List<Circle>();
            this.processMask(mask);
            Game1.Event_Update += update;
            Background.Event_NextLevel += nxtLevel;
        }

        #region funcs

        private void update()
        {
            if (Collision.TwoCircles(this.area, this, Global.me, Global.me.area))
            {
                for (int i = 0; i < this.collisionCircles.Count; i++)
                {
                    if (Collision.TwoCircles(this.collisionCircles[i], this, Global.me, Global.me.bWheel))
                    {
                        resetBulletPos();
                        Global.me.collideWithObstacle = true;
                        return;
                    }
                    else
                    {
                        if (Collision.TwoCircles(this.collisionCircles[i], this, Global.me, Global.me.fWheel))
                        {
                            resetBulletPos();
                            Global.me.collideWithObstacle = true;
                            return;
                        }
                    }
                    for (int j = 0; j < Global.me.coliisionPoints.Count; j++)
                    {

                        if (Collision.TwoCircles(this.collisionCircles[i], this, Global.me, Global.me.coliisionPoints[j]))
                        {
                            resetBulletPos();
                            Global.me.collideWithObstacle = true;
                            return;
                        }
                    }
                }
            }

            for (int t = 0; t < Global.copList.Count; t++)
            {
                if (Collision.TwoCircles(this.area, this, Global.copList[t], Global.copList[t].area))
                {
                    for (int i = 0; i < this.collisionCircles.Count; i++)
                    {
                        if (Collision.TwoCircles(this.collisionCircles[i], this, Global.copList[t], Global.copList[t].bWheel))
                        {
                            resetBulletPos();
                            Global.copList[t].collideWithObstacle = true;
                            return;
                        }
                        else
                        {
                            if (Collision.TwoCircles(this.collisionCircles[i], this, Global.copList[t], Global.copList[t].fWheel))
                            {
                                resetBulletPos();
                                Global.copList[t].collideWithObstacle = true;
                                return;
                            }
                        }
                        for (int j = 0; j < Global.copList[t].coliisionPoints.Count; j++)
                        {

                            if (Collision.TwoCircles(this.collisionCircles[i], this, Global.copList[t], Global.copList[t].coliisionPoints[j]))
                            {
                                resetBulletPos();
                                Global.copList[t].collideWithObstacle = true;
                                return;
                            }
                        }
                    }
                }
            } // for Ml check copList

            if (bulletPassedPlayer())
                resetBulletPos();
        }
        private void nxtLevel()
        {
            this.pase = (int)(this.pase * 1.5f);
        }
        private bool bulletPassedPlayer()
        {
            float bulletRight = (this.Position - (this.Origin * this.scale)).X + (this.CurrentFrame.Width * this.scale.X);
            if (bulletRight <= Global.me.Position.X - (Global.me.Origin * Global.me.scale).X)
                return true;
            return false;
        }
        private void resetBulletPos()
        {
            Vector2 firePos = new Vector2(0, 910);
            Vector2 pos = gunner.Position - (gunner.Origin * gunner.scale) + (firePos * gunner.scale);
            this.Position = pos;
        }

        private void processMask(Texture2D mask)
        {
            int cntR = 0;
            Color[,] cm = Tools.getColorMap(mask);
            Color compBody = cm[0, 0];
            for (int i = 0; i < cm.GetLength(1); i++)
            {
                if (cm[0, i] == compBody) //// need to check
                {
                    for (int t = 1; t < cm.GetLength(0); t++)
                    {
                        if (cm[t, i] == compBody)
                        {

                            Vector2 pos = new Vector2(t, i);
                            int tmpIndext = i;
                            while (true)
                            {

                                cntR = cntR + 1;
                                if (cm[t, tmpIndext] == compBody)
                                {
                                    tmpIndext = cntR;
                                    cntR = 0;
                                    break;
                                }
                                tmpIndext--;
                            }
                            this.collisionCircles.Add(new Circle((float)tmpIndext, pos));

                        }

                    }
                }
            }
        }
        #endregion
    }
}
