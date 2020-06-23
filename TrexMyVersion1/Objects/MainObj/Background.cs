using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrexMyVersion1.View;
using TrexMyVersion1.Physics;
using TrexMyVersion1.Objects;

namespace TrexMyVersion1.Objects
{
    public class Background
    {
        #region data
        public static event DlgNextLevel Event_NextLevel; //regestering an event that updates everything in the game


        public Level currentLevel;
        public List<Obstacle> obstacleList;
        public Flag finishLine;
        public Flag trafficLight;
        public PhysicsManager pm;
        private List<Cloud> clouds;
        public Texture2D[] cloudsTex;
        public Gunner gunner;
        #endregion

        #region ctor
        public Background()
        {
            //level
            Texture2D tmp = Tools.cm.Load<Texture2D>("levels/" + Global.currentLevel);
            currentLevel = new Level(tmp, Tools.cm.Load<Texture2D>("levels/masks/0m"), Level.generateGroundSize(),
                new Vector2(-tmp.Width * Global.scale, 0), null, Color.White, 0, new Vector2(0, 0),
                      new Vector2(Global.scale), 0, 0.7f);

            //pm
            this.pm = Tools.pm;

            //cloads textures
            this.cloudsTex = Animation.loadTextures("clouds", 2);
            //register to  events
            Game1.Event_Update += update;
            Background.Event_NextLevel += nextLevel;
        }

        public Background(Background b)
        {
            //copy
            this.currentLevel = b.currentLevel;
            this.obstacleList = b.obstacleList;
            this.finishLine = b.finishLine;
            this.trafficLight = b.trafficLight;
            this.pm = b.pm;

            //register to  events
            Game1.Event_Update += update;
            Background.Event_NextLevel += nextLevel;
        }

        public Background(int levelAmount)
        {
            //level
            Texture2D tmp = Tools.cm.Load<Texture2D>("levels/" + Global.currentLevel);
            currentLevel = new Level(tmp, Tools.cm.Load<Texture2D>("levels/masks/0m"), levelAmount,
                new Vector2(-tmp.Width * Global.scale, 0), null, Color.White, 0, new Vector2(0, 0),
                      new Vector2(Global.scale), 0, 0.9f);

            //pm
            this.pm = Tools.pm;

            //cloads textures
            this.cloudsTex = Animation.loadTextures("clouds", 2);
            //register to  events
            Game1.Event_Update += update;
            Background.Event_NextLevel += nextLevel;
        }
        #endregion

        #region funcs
        public void init()
        {
            //obstacle
            //Texture2D tmp = Tools.cm.Load<Texture2D>("obstacle/coronaHPG");
            //Tools.makeTransparent(tmp);
            //OL = Obstacle.randomObstacle(tmp, new Vector2(500, 980), pm, new Vector2(0.1f), "obstacle /coronaM"); //corona
            obstacleList = Obstacle.randomObstacle(Obstacle.loadTextures("obstacle", 1), new Vector2(256, 431), pm, new Vector2(0.2f), "obstacle/obstacleMask");

            //Trafficlight
            trafficLight = new Flag(Tools.cm.Load<Texture2D>("moreObj/trafficlight"), Flag.findTrafficLightPos(), null,
                Color.White, 0, new Vector2(938, 2670), new Vector2(0.03f), SpriteEffects.None, 0.3f);

            // finish line
            finishLine = new Flag(Tools.cm.Load<Texture2D>("moreObj/finishLine"), Flag.findFinishLinePos(), null,
                Color.White, 0, new Vector2(240, 340), new Vector2(0.5f), SpriteEffects.None, 0.3f);

            //clouds
            this.clouds = Cloud.generateClouds(cloudsTex, 10, DRC.left, new Vector2(0.1f));

            //gunner
        //    Texture2D sprite = Tools.cm.Load<Texture2D>("moreObj/gunner");
        //    Tools.makeTransparent(sprite);
        //    float Y = (currentLevel.groundPos.Y * currentLevel.scale.Y) + currentLevel.Position.Y - (currentLevel.Origin.Y * currentLevel.scale.Y);
        //    this.gunner = new Gunner(DRC.left, Tools.cm.Load<Texture2D>("moreObj/gunnerF1"),sprite, Tools.cm.Load<Texture2D>("masks/gunnerM")
        //        , null, -999, null, new Vector2(finishLine.Position.X+100, Y), null, Color.White, 0, new Vector2(911, 1200), new Vector2(0.1f),
        //        SpriteEffects.None, 0.2f, Tools.pm);
        }

        private void update()
        {
            if (Collision.finishLineBiker(Global.me, finishLine) == collisionState.collision)
            {
                Global.myScore++;
                Event_NextLevel?.Invoke();//nextLevel happend
            }
            if (Collision.finishLineBiker(Global.cop, finishLine) == collisionState.collision)
            {
                Global.copScore++;
                Event_NextLevel?.Invoke();//nextLevel happend
            }
            if (Collision.finishLineBiker(Global.player2, finishLine) == collisionState.collision)
            {
                Global.enemyScore++;
                Event_NextLevel?.Invoke();//nextLevel happend
            }
        }

        /// <summary>
        /// switching level main func
        /// </summary>
        private void nextLevel()
        {
            Global.currentLevel++;
            if (Global.currentLevel ==3)
            {
                return;
            }
            Texture2D tmp = Tools.cm.Load<Texture2D>("levels/" + Global.currentLevel);
            float scale =  (currentLevel.CurrentFrame.Height * currentLevel.scale.X) / tmp.Height;
            if (Global.currentLevel == 2)
            {
                scale *= 2;
                Tools.makeTransparent(tmp);

            }
            Global.scale = scale;
            killBG();
            currentLevel = new Level(tmp, Tools.cm.Load<Texture2D>("levels/masks/" + Global.currentLevel+"m"), Level.generateGroundSize(),
               new Vector2(-tmp.Width * Global.scale, 0), null, Color.White, 0, new Vector2(0, 0),
                     new Vector2(Global.scale), 0, 0.7f);
            this.init();
        }

        /// <summary>
        /// kills all obstacles
        /// </summary>
        private void killBG()
        {
            for (int i = 0; i < obstacleList.Count(); i++)
            {
                obstacleList[i].kill();
            }
            this.currentLevel.kill();
            this.trafficLight.kill();
            this.finishLine.kill();

        }
        #endregion
    }
}
