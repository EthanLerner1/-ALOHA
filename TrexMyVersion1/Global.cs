using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrexMyVersion1.View;
using TrexMyVersion1.Physics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TrexMyVersion1.Objects;
using TrexMyVersion1.View;
using TrexMyVersion1.Physics;
using Microsoft.Xna.Framework.Input;
using TrexMyVersion1.ML;

namespace TrexMyVersion1
{
    public class Circle
    {

        public float radius { set; get; }
        public Vector2 position { set; get; }

        /// <summary>
        /// constructor for a circle
        /// </summary>
        /// <param name="rad"> float, radius sizr in pixels</param>
        /// <param name="pos">Vector2, middle of the circle</param>
        public Circle(float rad, Vector2 pos)
        {
            this.radius = rad;
            this.position = pos;
        }
        public Circle(Circle c)
        {
            this.radius = c.radius;
            this.position = c.position;
        }
        public Circle() { }
    }

    public static class Global
    {
        #region data
        public static event DlgKillAll Event_KillAll;
        #region ML
        public static List<Cop> copList;

        public static Camera cam;
        #endregion
        //static Level[] levels;
        //public static Map map;
        public static Biker me;
        public static Biker player2;
        public static Biker cop1;
        public static Cop cop;
        public static float scale;
        public static float playerScale;
        public static float zoom;
        public static float fps;
        public static Background bg;
        public static int currentLevel = 0;

        // scroes
        public static int myScore = 0;
        public static int enemyScore = 0;
        public static int copScore = 0;

        //flage
        public static bool gameEnd = false;
        public static bool gamseStarted = false;

        //
        public static Circle bWheel;
        public static Circle fWheel;
        public static Circle area;
        public static List<Circle> cp;

        //
        public static bool restartBots = false;
        #endregion

        public static void Init()
        {
            scale = 0.2f;
            playerScale = 0.5f;
            zoom = 1.2f;
            fps = 60;

            //ML
            copList = new List<Cop>();
            NeuralNet n5 = new NeuralNet("stages/s3.txt");

            //biker
            Texture2D[] biker = Animation.loadTextures("playerGif", 10);
            me = new Biker("masks/Bikermask", biker, new UserKeys(Keys.Left, Keys.Right, Keys.Space), 0, PhysicsManager.createAndGetNewEngine(50, 0.2f, 25, 1),
                Tools.cm.Load<Texture2D>("playerGif/0"), new Vector2(0, -100), null, Color.White, 0, new Vector2(97, 65),
                       new Vector2(Global.playerScale), 0, 0, Tools.pm, 1);

            player2 = new Biker("masks/Bikermask", biker, new UserKeys(Keys.A, Keys.D, Keys.W), 1, PhysicsManager.createAndGetNewEngine(50, 0.2f, 25, 1),
                Tools.cm.Load<Texture2D>("playerGif/0"), new Vector2(0, -100), null, Color.Red, 0, new Vector2(97, 65),
                       new Vector2(Global.playerScale), 0, 0, Tools.pm, 1);

            //cop
            Texture2D[] copArray = Animation.loadTextures("copGif", 10);
            cop1 = new Biker("masks/policeCarM", copArray, new BotKeys(), 2, PhysicsManager.createAndGetNewEngine(50, 0.2f, 25, 1),
                Tools.cm.Load<Texture2D>("copGif/0"), new Vector2(0, -100), null, Color.White, 0, new Vector2(730, 1440),
                       new Vector2(0.1f), 0, 0.1f, Tools.pm, 1.2f);
            cop1.kill();



            fWheel = cop1.fWheel;
            bWheel = cop1.bWheel;
            area = cop1.area;
            cp = cop1.coliisionPoints;

            cop = new Cop(n5, "masks/policeCarM", copArray, new BotKeys(), 2, PhysicsManager.createAndGetNewEngine(50, 0.2f, 25, 1),
                Tools.cm.Load<Texture2D>("copGif/0"), new Vector2(0, -100), null, Color.White, 0, new Vector2(730, 1440),
                       new Vector2(0.1f), 0, 0.1f, Tools.pm, 1.2f);
            //background
            bg = new Background();
            bg.init();

            //nextLevel
            Background.Event_NextLevel += nxtlevel;
        }
        public static void Init(int levelAmount, List<Obstacle> ol)
        {
            scale = 0.2f;
            playerScale = 0.5f;
            zoom = 1.2f;
            fps = 60;
            currentLevel = 0;


            //biker
            Texture2D[] biker = Animation.loadTextures("playerGif", 10);
            me = new Biker("masks/Bikermask", biker, new UserKeys(Keys.Left, Keys.Right, Keys.Space), 0, PhysicsManager.createAndGetNewEngine(50, 0.2f, 25, 1),
                Tools.cm.Load<Texture2D>("playerGif/0"), new Vector2(0, -100), null, Color.White, 0, new Vector2(97, 65),
                       new Vector2(Global.playerScale), 0, 0, Tools.pm, 1);
            //player2 = new Animation(biker, new UserKeys(Keys.Left, Keys.Right, Keys.Up, Keys.Down, Keys.Space), -1, null,
            //    Tools.cm.Load<Texture2D>("playerGif/0"), new Vector2(0, -100), null, Color.Red, 0, new Vector2(97, 65),
            //           new Vector2(Global.playerScale), 0, 0, Tools.pm);

            //cop
            Texture2D[] copArray = Animation.loadTextures("copGif", 10);
            cop = new Cop("masks/policeCarM", copArray, new UserKeys(Keys.A, Keys.D, Keys.W), 1, PhysicsManager.createAndGetNewEngine(50, 0.2f, 25, 1),
                Tools.cm.Load<Texture2D>("copGif/0"), new Vector2(0, -100), null, Color.White, 0, new Vector2(730, 1440),
                       new Vector2(0.1f), 0, 0, Tools.pm, 1.2f);

            fWheel = cop.fWheel;
            bWheel = cop.bWheel;
            area = cop.area;
            cp = cop.coliisionPoints;

            //background
            bg = new Background(levelAmount);
            bg.init();

            //nextLevel
            Background.Event_NextLevel += nxtlevel;
        }


        public static void nxtlevel()
        {
            cop.nn = new NeuralNet("stages/s" + currentLevel + ".txt");
            if (currentLevel >= 3)
            //game ended
            {
                gameEnd = true;
                Event_KillAll?.Invoke();
                if (myScore > enemyScore && myScore > copScore)
                {
                    Texture2D[] win = Animation.loadTextures("screens/winGif", 8);
                    float s1 = Tools.W;
                    float s2 = win[0].Width;
                    float s3 = s1 / s2;
                    Tools.DrawSingle(new Animation(40,win, null, -999, null, win[0], Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(s3),
                        SpriteEffects.None, 0.2f, Tools.pm));
                }
                else
                {
                    if (enemyScore > myScore && enemyScore > copScore)
                    {
                        Texture2D[] win = Animation.loadTextures("screens/winGifE", 8);
                        float s1 = Tools.W;
                        float s2 = win[0].Width;
                        float s3 = s1 / s2;
                        Tools.DrawSingle(new Animation(win, null, -999, null, win[0], Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(s3),
                            SpriteEffects.None, 0.2f, Tools.pm));
                    }
                    else
                    {
                        Texture2D tex = Tools.cm.Load<Texture2D>("screens/LLL");
                        float s1 = Tools.W;
                        float s2 = tex.Width;
                        float s3 = s1 / s2;
                        DrawObj l = new DrawObj(tex, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(s3), SpriteEffects.None, 0);
                        Tools.DrawSingle(l);
                    }
                }
                

            }
        }




    }

}
