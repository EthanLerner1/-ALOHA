using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrexMyVersion1.Physics;
using TrexMyVersion1.Objects;
using TrexMyVersion1.View;

namespace TrexMyVersion1
{
    public delegate void DlgDraw();
    public delegate void DlgUpdate();
    public delegate void DlgNextLevel ();
    public delegate void DlgKillAll();
    static class Tools
    {
        #region data
        //public const float STEERSPEED = 0.005f;
        public static GameTime gt;
        public static Random rnd;
        public static SpriteBatch sb;
        public static KeyboardState ks;
        public static int W, H;
        public static ContentManager cm;
        public static GraphicsDevice grd;
        public static Game1 game;
        public static PhysicsManager pm;
        #endregion

        #region funcs
        public static void Init(GraphicsDevice gd, ContentManager cm, Game1 g)
        {
            rnd = new Random(0);
            grd = gd;
            sb = new SpriteBatch(gd);
            Tools.cm = cm;
            Game1.Event_Update += update;
            gt = new GameTime();
            W = 1200;
            H = 700;
            game = g;
            pm = new PhysicsManager();

        }
        public static void update()
        {
            ks = Keyboard.GetState();
        }

        #region ///////////// image procesing /////////////
        public static Color[,] getColorMap(string maskname)
        {
            Texture2D tx = cm.Load<Texture2D>(maskname);
            Color[] c = new Color[tx.Width * tx.Height];
            tx.GetData<Color>(c);
            Color[,] cMap = new Color[tx.Width, tx.Height];
            for (int y = 0; y < tx.Height; y++)
            {
                for (int x = 0; x < tx.Width; x++)
                {
                    cMap[x, y] = c[x + y * tx.Width];
                }

            }
            return cMap;
        }
        public static Color[,] getColorMap(Texture2D tx)
        {
            Color[] c = new Color[tx.Width * tx.Height];
            tx.GetData<Color>(c);
            Color[,] cMap = new Color[tx.Width, tx.Height];
            for (int y = 0; y < tx.Height; y++)
            {
                for (int x = 0; x < tx.Width; x++)
                {
                    cMap[x, y] = c[x + y * tx.Width];
                }

            }
            return cMap;
        }
        public static void makeTransparent(Texture2D tx)
        {// this function gets a Texture2D tx and removes its backgroud
         // the color of the background must be the color in the first pixel
         // of the img
            Color[] c = new Color[tx.Width * tx.Height];
            tx.GetData<Color>(c);
            Color bg = c[0];
            for (int h = 0; h < tx.Height; h++)
            {
                for (int w = 0; w < tx.Width; w++)
                {
                    if (c[h * tx.Width + w] == bg)
                        c[h * tx.Width + w] = Color.Transparent;
                }
            }
            //c[0] = Color.Transparent;
            tx.SetData<Color>(c);
        }
        public static Circle findArea(String mask, Point refColor)
        {
            Color[,] cm = Tools.getColorMap(mask);
            Circle ret = new Circle();
            int cntR = 0;
            Color cRef = cm[refColor.X,refColor.Y];

            for (int h = 1; h < cm.GetLength(1); h++)
            {
                if (cm[0, h] == cRef)
                {
                    for (int w = 1; w < cm.GetLength(0); w++)
                    {
                        if (cm[w, h] == cRef)
                        {

                            ret.position = new Vector2(w, h);
                            int tmpIndext = h - 1;
                            while (true)
                            {

                                cntR = cntR + 1;
                                if (cm[w, tmpIndext] == cRef)
                                {
                                    ret.radius = (int)(cntR) ;
                                    return ret;
                                }
                                tmpIndext--;
                            }
                        }

                    }
                }
            }
            return ret;
        }
        public static Boolean isInFrame(Camera cm, DrawObj f)
        {
            Matrix mat = cm.Mat;
            Vector2 tmp = new Vector2(f.Position.X + (f.CurrentFrame.Width - f.Origin.X) * f.scale.X,
                f.Position.Y + (f.CurrentFrame.Height - f.Origin.Y) * f.scale.Y);
            tmp = Vector2.Transform(tmp, mat);
            if (tmp.X < cm.vp.Width && tmp.X > 0)
                if (tmp.Y < cm.vp.Height && tmp.Y > 0)
                    return true;
            return false;

        }
       
        /// <summary>
        /// drawing a single object all over the screen
        /// </summary>
        /// <param name="d"></param>
        public static void DrawSingle (DrawObj d)
        {
            grd.Viewport = new Viewport(0, 0, Tools.W, Tools.H);
            grd.Clear(Color.CornflowerBlue);
            Tools.sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                null, null, null, null,null);
            d.draw();
            Tools.sb.End();
        }

        #endregion

        #region ///////////// vector procesing /////////////
        public static Double getDistance(Point p1, Point p2)
        {// calcultaing the value between two point according to the formula:
            // sqrt[(x1-x2)^2 + (y1-y2)^2] = distance
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }

        public static Vector2 direction_from_rot(float rot)
        {
            return Vector2.Transform(Vector2.UnitX, Matrix.CreateRotationZ(rot));
        }

        public static Vector2 findNormalTo(Vector2 v)
        {
            /* finding the normal to a given Vector2
             *based on the fact that two vectures multiplied in each other equals zero
             * Vx*a+Vy*b = 0
             * b = -Vx*a/Vy (a=1, assigmnet)
             * b = -Vx/Vy
             */
            if (v.Y == 0) // incase y=0 so b wont be equal to infiniy
                return new Vector2(0, 1);
            float b = (-v.X / v.Y);
            return new Vector2(1, b);
        }

        public static Vector2 rotateVector(Vector2 v, float radians)
        {
            return Vector2.Transform(v, Matrix.CreateRotationZ(radians));
        }

        public static double vectorAngle(Vector2 v)
        {//returnes the angle of the vector
            return Math.Atan2(v.Y, v.X);
        }

        public static float getXComponentByAngle(Vector2 v, double angle)
        { // returning the x coponant of the vector that would be if the axis would rotate in angle degrees
            return (float)(v.Length() * Math.Cos(angle));
        }

        public static float getYComponentByAngle(Vector2 v, double angle)
        { // returning the x coponant of the vector that would be if the axis would rotate in angle degrees
            return (float)(v.Length() * Math.Sin(angle));
        }
        #endregion

        #region ///////////// math /////////////

        /// <summary>
        /// this function returns the answers for a quadratic Equation
        /// in  a list<float></float>
        /// ax^2 + bx + cx
        /// </summary>
        /// <param name="a">aX^2</param>
        /// <param name="b">bX</param>
        /// <param name="c">const</param>
        /// <returns></returns>
        public static List<float> quadraticEquation (float a, float b, float c)
        {// according to the formula for Root Equations
            float added = -b / (2*a);
            float delta = (float) (Math.Sqrt(Math.Pow(b,2)-(4*a*c))) /(2*a);
            List<float> ret = new List<float>();
            ret.Add(added + delta);
            ret.Add(added - delta);
            return ret;
        }

        /// <summary>
        /// finding the linear equation between two points
        /// return list where first cell containes m/incline
        /// second cell contains b
        /// // by y = mx+b
        /// </summary>
        /// <param name="s">start point of vector</param>
        /// <param name="f">finish point of vector</param>
        /// <returns></returns>
        public static List<float> findLinerEquationFromPoints(Point s, Point f)
        {
            List<float> ret = new List<float>();

            // a priavte case for which there is no incline 
            // x = const
            if ((f.X - s.X) == 0)
            {
                ret.Add(-999);
                ret.Add(s.X);
                return ret;
            }

            // y= mx+b
            float incline = (f.Y - s.Y) / (f.X - s.X); //m
            //b=y-mx
            float b = s.Y - (incline * s.X);
            
            ret.Add(incline);
            ret.Add(b);
            return ret;
        }
        #endregion

        #region ///////////// string  /////////////
        public static string ToString_two_dimention_array<T>(T[,] array)
        {
            string ret="";
            for (int i = 0; i < array.GetLength(1); i++)
            {
                ret += "(";
                for (int t = 0; t < array.GetLength(0); t++)
                {
                    ret += array[t, i].ToString();
                    if (t < array.GetLength(0) - 1)
                        ret += ",";
                }
                ret += ")";
            }
            return ret;
        }

        public static string ToString_array<T>(T[] array)
        {
            string ret = "";
            ret += "(";
            for (int t = 0; t < array.Length; t++)
            {
                ret += array[t].ToString() + ",";
            }
            ret += ")";
            return ret;
        }

        #endregion
        #endregion
    }
}
