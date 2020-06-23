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
    public class Obstacle : Animation
    {
        #region data
        public bool switchFrames;
        public List<Vector2> frame; // vectors around the character, the first one is the offset
        public Circle region;
        #endregion

        #region ctor

        /// <summary>
        /// the constructor of an obstacle
        /// </summary>
        /// <param name="frames">Texture2D[] that contains all the animation frames</param>
        /// <param name="keys">AbstructKeys, keys that control the character</param>
        /// <param name="engI">the index of the engine </param>
        /// <param name="eng">engine for the biker</param>
        /// <param name="texture">tetxture2D picture/firstframe</param>
        /// <param name="position">Vector2, where to draw</param>
        /// <param name="sourceRectangle">which ractangl to draw</param>
        /// <param name="color">Color, which color</param>
        /// <param name="rotation">float, angle in radians</param>
        /// <param name="origin">Vector2, where to draw the Dmoot from</param>
        /// <param name="scale">Vector2, picture scale</param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        /// <param name="pm">PhysicsManager, the physics manager for the game</param>
        public Obstacle(Texture2D[] frames, String mask, Texture2D spriteSheet, Texture2D spriteMask, abstractKeys keys, int engI, Engine eng, Texture2D firstFrame, Vector2 position,
          Rectangle? sourceRectangle, Color color,
          float rotation, Vector2 origin, Vector2 scale,
          SpriteEffects effects, float layerDepth, PhysicsManager pm) : base(frames, spriteSheet, spriteMask, keys, engI, eng, firstFrame, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth, pm)
        {
            this.frame = Collision.strechVectors(mask);
            this.region = Tools.findArea(mask, new Point(1, 0));
            Game1.Event_Update += update;

        }

        public Obstacle(Texture2D tex, String mask, Texture2D spriteSheet, Texture2D spriteMask, abstractKeys keys, int engI, Engine eng, Vector2 position,
  Rectangle? sourceRectangle, Color color,
  float rotation, Vector2 origin, Vector2 scale,
  SpriteEffects effects, float layerDepth, PhysicsManager pm) : base(tex, spriteSheet, spriteMask, keys, engI, eng, position, sourceRectangle, color, rotation, origin, scale, effects, layerDepth, pm)
        {
            this.frame = Collision.strechVectors(mask);
            this.region = Tools.findArea(mask, new Point(1, 0));
            Game1.Event_Update += update;

        }

        #endregion

        #region funcs

        private void update()
        {
            if (this.switchFrames == false)
            {
                bool tmp1 = Collision.TwoCircles(this.region, this, Global.me, Global.me.area);
                if (tmp1 == true)
                {
                    if (Collision.BikerObstacle(this, Global.me) == collisionState.collision)
                    {
                        this.switchFrames = true;
                        Global.me.collideWithObstacle = true;
                        base.switchToSprite();
                        this.scale = new Vector2(scale.X * 5);
                    }

                }
                bool tmp2 = Collision.TwoCircles(this.region, this, Global.cop, Global.cop.area);
                if (tmp2 == true)
                {
                    if (Collision.BikerObstacle(this, Global.cop) == collisionState.collision)
                    {
                        this.switchFrames = true;
                        Global.cop.collideWithObstacle = true;
                        base.switchToSprite();
                        this.scale = new Vector2(scale.X * 5);
                    }

                }

                tmp2 = Collision.TwoCircles(this.region, this, Global.player2, Global.player2.area);
                if (tmp2 == true)
                {
                    if (Collision.BikerObstacle(this, Global.player2) == collisionState.collision)
                    {
                        this.switchFrames = true;
                        Global.player2.collideWithObstacle = true;
                        base.switchToSprite();
                        this.scale = new Vector2(scale.X * 5);
                    }

                }
                #region ML
                //for (int m = 0; m < Global.copList.Count; m++)
                //{
                //    bool tmp3 = Collision.TwoCircles(this.region, this, Global.copList[m], Global.copList[m].area);
                //    if (tmp3 == true)
                //    {
                //        if (Collision.BikerObstacle(this, Global.copList[m]) == collisionState.collision)
                //        {
                //            //this.switchFrames = true;
                //            Global.copList[m].collideWithObstacle = true;
                //            Global.copList[m].dead = true;
                //            // base.switchToSprite();
                //            //this.scale = new Vector2(scale.X * 5);
                //        }

                //    }
                //}
                #endregion
            }

        }
        /// <summary>
        /// this function creates all the obsticale in a stage ang retrives a list that containes all of the obstacles
        /// </summary>
        /// <param name="frames">frames of the obstacle animation</param>
        /// <param name="origin">origin of the obstacle</param>
        /// <param name="pm">physicsManager og the game</param>
        public static List<Obstacle> randomObstacle1(Texture2D[] frames, Vector2 origin, PhysicsManager pm, Vector2 scale, String mask)
        {

            float levelLength = Global.bg.currentLevel.CurrentFrame.Width * Global.scale * (Global.bg.currentLevel.amount - 1); // the ground length
            levelLength -= 1000; // giving the player 100 pixels before he needs to act
            int obstacleAmount = Tools.rnd.Next((int)levelLength / 2000, (int)levelLength / 1500);
            List<Obstacle> obstaclesList = new List<Obstacle>();

            for (int i = 0; i < obstacleAmount; i++)
            {
                int togather = Tools.rnd.Next(1, 3); // amount of obsticale that can be next to each other
                Vector2 pos = new Vector2(1000 + Tools.rnd.Next(1500, 2000) * i, Global.bg.currentLevel.groundPos.Y * Global.scale);
                for (int t = 0; t < togather; t++)
                {
                    Vector2 tmp = new Vector2(frames[0].Width * scale.X * t, 0);
                    Texture2D tmp2 = Tools.cm.Load<Texture2D>("obstacle/explosion/Explosion");
                    Tools.makeTransparent(tmp2);
                    obstaclesList.Add(new Obstacle(frames, mask, tmp2
                        , Tools.cm.Load<Texture2D>("obstacle/explosion/ExplosionM"), null, -999, null, frames[0], pos + tmp, null, Color.White, 0, origin, scale, SpriteEffects.None, 0, pm));
                }

            }
            return obstaclesList;

        }
        public static List<Obstacle> randomObstacle(Texture2D[] frames, Vector2 origin, PhysicsManager pm, Vector2 scale, String mask)
        {

            float levelLength = Global.bg.currentLevel.CurrentFrame.Width * Global.scale * (Global.bg.currentLevel.amount - 1); // the ground length
            levelLength -= 2000; // giving the player 100 pixels before he needs to act
            //int obstacleAmount = Tools.rnd.Next((int)levelLength / 10000, (int)levelLength / 5000);
            int obstacleAmount = (int)(levelLength / 3000);
            List<Obstacle> obstaclesList = new List<Obstacle>();

            for (int i = 0; i < obstacleAmount; i++)
            {
                //int togather = Tools.rnd.Next(1, 3); // amount of obsticale that can be next to each other
                int togather = 1;
                //Vector2 pos = new Vector2(2000 + Tools.rnd.Next(1500, 2000) * i, Global.bg.currentLevel.groundPos.Y * Global.scale);
                Vector2 pos = new Vector2(2000 + 3000 * i, Global.bg.currentLevel.groundPos.Y * Global.scale);
                for (int t = 0; t < togather; t++)
                {
                    Vector2 tmp = new Vector2(frames[0].Width * scale.X * t, 0);
                    Texture2D tmp2 = Tools.cm.Load<Texture2D>("obstacle/explosion/Explosion");
                    Tools.makeTransparent(tmp2);
                    obstaclesList.Add(new Obstacle(frames, mask, tmp2
                        , Tools.cm.Load<Texture2D>("obstacle/explosion/ExplosionM"), null, -999, null, frames[0], pos + tmp, null, Color.White, 0, origin, scale, SpriteEffects.None, 0, pm));

                }
            }
            return obstaclesList;


        }

        /// <summary>
        /// kills the update func of this instance
        /// </summary>
        public void kill()
        {
            Game1.Event_Update -= update;
            base.kill();
        }
        #endregion
    }
}

