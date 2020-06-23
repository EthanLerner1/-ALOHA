using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TrexMyVersion1.Objects;
using TrexMyVersion1.View;

namespace TrexMyVersion1.Physics
{
    public enum collisionState
    {
        noCollision, collision
    }
    static class Collision
    {
        #region //////////// help functions ////////////
        public static List<Point> LineCircleCollision(List<float> line, Point cicrcleM, float circleRad)
        {// calcultaes by equlizing circle equlation to line equlation
            //middle of circle
            float a = cicrcleM.X;
            float c = cicrcleM.Y;
            //radius of circle
            float r = circleRad;
            // return
            List<Point> ret = new List<Point>();

            if (line[0] != -999)
            {
                // if line: y=mx+b
                float m = line[0]; //incline
                float b = line[1];

                //calcutaion
                // d = b-c
                float d = b - c;
                List<float> xResult = Tools.quadraticEquation((float)
                    (1 + Math.Pow(m, 2)), (float)(-2 * a) + (2 * m * d), (float)
                    (Math.Pow(d, 2) - Math.Pow(r, 2) + Math.Pow(a, 2)));

                // putting the X results in y = mx + b
                float y1 = xResult[0] * m + b;
                float y2 = xResult[1] * m + b;


                ret.Add(new Point((int)xResult[0], (int)y1));
                ret.Add(new Point((int)xResult[1], (int)y2));
            }
            else
            {// if line: x=const
                float m = line[1];
                List<float> yResult = Tools.quadraticEquation(1, (-2 * c), (float)(Math.Pow(m, 2) + Math.Pow(a, 2) + Math.Pow(c, 2) - Math.Pow(r, 2) - (2 * m * a)));
                ret.Add(new Point((int)m, (int)yResult[0]));
                ret.Add(new Point((int)m, (int)yResult[1]));
            }
            return ret;
        }

        /// <summary>
        /// this function checks the collsion of two circles represent objects area
        /// </summary>
        /// <param name="obstacleC">the obstacle circle</param>
        /// <param name="obstacle">the obstacle</param>
        /// <param name="Obstaclescale">obstacle scale</param>
        /// <param name="player">the second object usuually the player</param>
        /// <param name="playerC">the circle of the second obj</param>
        /// <returns></returns>
        public static Boolean TwoCircles(Circle obstacleC, DrawObj obstacle, DrawObj player, Circle playerC)
        {
            float Obstaclescale = obstacle.scale.X;
            Vector2 div = ((obstacle.Position - (obstacle.Origin * Obstaclescale) + (obstacleC.position * Obstaclescale))
                - (player.Position - (player.Origin * player.scale) + (playerC.position * player.scale)));
            if (div.Length() < ((obstacleC.radius * obstacle.scale.X) + (playerC.radius*player.scale.X)))
                return true;
            return false;
        }

        #endregion

        #region //////////// collision states ////////////

        /// <summary>
        /// this function checks if the biker is on the air or not
        /// </summary>
        /// <param name="currentLevel">Level, the currnet level in the game </param>
        /// <param name="player">Biker, the player checked</param>
        /// <returns></returns>
        public static collisionState roadBikerState(Level currentLevel, Biker player)
        {
            Vector2 offset = player.Position - (player.Origin * player.scale);
            float yOfWheel = (player.bWheel.position.Y + player.bWheel.radius) * player.scale.X + offset.Y;
            if (yOfWheel < (currentLevel.groundPos.Y * Global.scale))
                return collisionState.noCollision;
            return collisionState.collision;
        }

        /// <summary>
        /// this function checks the collision of the biker with an obstacle and returns the collision state
        /// </summary>
        /// <param name="block">obstace</param>
        /// <param name="player">player checked</param>
        /// <returns></returns>
        public static collisionState BikerObstacle(Obstacle block, Biker player)
        {
            Vector2 offsetplayer = player.Position - (player.Origin * Global.playerScale);
            Vector2 offsetBlock = block.Position - (block.Origin * block.scale);
            ////// circle equlation
            ////// (x-a)^2 + (y-b)^2 = R^2
            ////// (a,b) middle of the circle, R radius of the circle
            //back wheel
            int a1 = (int)(player.bWheel.position.X * Global.playerScale + offsetplayer.X);
            int b1 = (int)(player.bWheel.position.Y * Global.playerScale + offsetplayer.Y);
            float R1 = player.bWheel.radius * Global.playerScale;
            //front wheel
            int a2 = (int)(player.fWheel.position.X * Global.playerScale + offsetplayer.X);
            int b2 = (int)(player.fWheel.position.Y * Global.playerScale + offsetplayer.Y);
            float R2 = player.fWheel.radius * Global.playerScale;

            Vector2 route = offsetBlock;
            for (int i = 1; i < block.frame.Count(); i++)
            {
                route += (block.frame[i - 1]*block.scale);
                Vector2 finalPoint = route + (block.frame[i] * block.scale);
                List<float> linearEq = Tools.findLinerEquationFromPoints(route.ToPoint(), finalPoint.ToPoint());
                List<Point> colisionPointsBack = LineCircleCollision(linearEq, new Point(a1, b1), R1);
                List<Point> colisionPointsFront = LineCircleCollision(linearEq, new Point(a2, b2), R2);
                //(x,y) - collsion point
                // if (start vector point <(x,y) < finish vector point )
                // then there is truelly a collision

                if (((offsetplayer + (player.fWheel.position * Global.playerScale)) - new Vector2(colisionPointsFront[0].X, colisionPointsFront[0].Y))
                    .Length() <= (player.fWheel.radius * Global.playerScale))
                    return collisionState.collision;
                if (((offsetplayer + (player.fWheel.position * Global.playerScale)) - new Vector2(colisionPointsFront[1].X, colisionPointsFront[1].Y))
                    .Length() <= (player.fWheel.radius * Global.playerScale))
                    return collisionState.collision;
                if (((offsetplayer + (player.bWheel.position * Global.playerScale)) - new Vector2(colisionPointsBack[0].X, colisionPointsBack[0].Y))
                    .Length() <= (player.fWheel.radius * Global.playerScale))
                    return collisionState.collision;
                if (((offsetplayer + (player.bWheel.position * Global.playerScale)) - new Vector2(colisionPointsBack[1].X, colisionPointsBack[1].Y))
                    .Length() <= (player.fWheel.radius * Global.playerScale))
                    return collisionState.collision;

            }
            return collisionState.noCollision;
        }

        /// <summary>
        /// checks if biker "p" reached the flag
        /// </summary>
        /// <param name="p">biker</param>
        /// <param name="line">flag</param>
        /// <returns></returns>
        public static collisionState finishLineBiker(Biker p, Flag line)
        {
            float lineLeft = line.Position.X - (line.Origin.X * line.scale.X);
            float bikerRightP = p.Position.X - (p.Origin.X * p.scale.X) + (p.CurrentFrame.Width * p.scale.X);
            if (lineLeft <= bikerRightP)
                return collisionState.collision;
            else
                return collisionState.noCollision;
        }

        #endregion

        #region //////////// streching vector around perimeter ////////////

        /// <summary>
        /// checkes if there is a pixel with the same color to the one given not diagonal!!!
        /// </summary>
        /// <param name="current">location of currnet pixl</param>
        /// <param name="cm">color map</param>
        /// <param name="refC">the current pixel color</param>
        /// <returns></returns>
        private static Point hasWorH(Point current, Color[,] cm, Color refC)
        {
            Boolean XR = (current.X + 1 < cm.GetLength(0));
            Boolean XL = (current.X - 1 >= 0);
            Boolean YD = current.Y + 1 < cm.GetLength(1);
            Boolean YU = current.Y - 1 >= 0;

            if (XR == true && cm[current.X + 1, current.Y] == refC)
                return new Point(current.X + 1, current.Y);
            if (XL == true && cm[current.X - 1, current.Y] == refC)
                return new Point(current.X - 1, current.Y);
            if (YD == true && cm[current.X, current.Y + 1] == refC)
                return new Point(current.X, current.Y + 1);
            if (YU == true && cm[current.X, current.Y - 1] == refC)
                return new Point(current.X, current.Y - 1);
            return new Point(current.X, current.Y);
        }

        /// <summary>
        /// checkes if there is a pixel with the same color to the one given  diagonal!!!
        /// </summary>
        /// <param name="current">location of currnet pixl</param>
        /// <param name="cm">color map</param>
        /// <param name="refC">the current pixel color</param>
        /// <returns></returns>
        private static Point hasDiagonal(Point current, Color[,] cm, Color refC)
        {
            Boolean RU = (current.X + 1 < cm.GetLength(0) && current.Y - 1 >= 0); // step right step up
            Boolean RD = (current.X + 1 < cm.GetLength(0) && current.Y + 1 < cm.GetLength(1)); // step right step down
            Boolean LU = (current.X - 1 >= 0 && current.Y - 1 >= 0); // step left step up
            Boolean LD = (current.X - 1 >= 0 && current.Y + 1 < cm.GetLength(1)); // step left step down

            if (RU == true && cm[current.X + 1, current.Y - 1] == refC)
                return new Point(current.X + 1, current.Y - 1);

            if (RD == true && cm[current.X + 1, current.Y + 1] == refC)
                return new Point(current.X + 1, current.Y + 1);

            if (LU == true && cm[current.X - 1, current.Y - 1] == refC)
                return new Point(current.X - 1, current.Y - 1);

            if (LD == true && cm[current.X - 1, current.Y + 1] == refC)
                return new Point(current.X - 1, current.Y + 1);
            return current;

        }

        /// <summary>
        /// streches vectors around the parimeter of a given character or its "mask". the first ine is the offset
        /// the explenation of how thw mask should be like is in the apendix
        /// </summary>
        /// <param name="mask">Texture2D, mask of the object</param>
        /// <returns></returns>
        public static List<Vector2> strechVectors(String mask)
        {
            Color[,] cm = Tools.getColorMap(mask);
            Color c = cm[0, 0];
            List<Vector2> vectorsList = new List<Vector2>();
            // i need to give the fnction the location of the first pixel!!!
            for (int w = 0; w < cm.GetLength(0); w++)
            {
                for (int y = 1; y < cm.GetLength(1); y++)
                {
                    if (cm[w, y] == c)
                    {
                        strechVectorsSon(vectorsList, cm, c, new Point(0, 0), new Point(w, y), 30);
                        return vectorsList;
                    }
                }
            }
            return vectorsList;
        }

        /// <summary>
        /// this is a recursive function that serves strechVectors function it streches all the vectors around
        /// the parimeter and puts it inside of a given list
        /// </summary>
        /// <param name="vectorsList">List<Vector2>, a list to put all the vectors in</param>
        /// <param name="cm">Color[,], map of the mask</param>
        /// <param name="refC">Color, the color of the parimeter</param>
        /// <param name="first">Point, start of the current vector being made</param>
        /// <param name="last">Point, curent posision in the color map (when creating this must be the first pixel of the perimeter)</param>
        /// <param name="grid">the grid of the vectors (if changing you also need to change inside the function row 15</param>
        public static void strechVectorsSon(List<Vector2> vectorsList, Color[,] cm, Color refC, Point first, Point last, int grid)
        {
            Point next = hasWorH(last, cm, refC);
            Point nextDiagonal = hasDiagonal(last, cm, refC);

            if (next == last && nextDiagonal == last && grid != 30 && first != last)
            {
                Vector2 add = new Vector2(last.X - first.X, last.Y - first.Y);
                vectorsList.Add(add);
            }

            if (grid == 30)
            {
                Vector2 add = new Vector2(last.X - first.X, last.Y - first.Y);
                vectorsList.Add(add);
                first = last;
                grid = 0;
            }

            if (next != last)
            {
                cm[last.X, last.Y] = new Color(0, 0, 0, 0);
                last = next;
                strechVectorsSon(vectorsList, cm, refC, first, last, ++grid);
            }
            else
            {
                if (nextDiagonal != last)
                {
                    cm[last.X, last.Y] = new Color(0, 0, 0, 0);
                    last = next;
                    strechVectorsSon(vectorsList, cm, refC, first, last, grid++);
                }
            }

            return;
        }
        #endregion

    }






}

