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

namespace TrexMyVersion1.ML
{
    class GetAsentialData
    {
        /// <summary>
        /// is the biker given is able to jump
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool CanJump(Biker b)
        {
            if (b.touchesGround == collisionState.collision)
                return true;
            return false;
        }
        public static float GroupMiddlePosX(List<Obstacle> ol, DrawObj character)
        {
            float characterRightPos = character.Position.X - ((character.Origin.X + character.CurrentFrame.Width) * character.scale.X);
            float obstacleWidth = ol[0].CurrentFrame.Width * ol[0].scale.X;
            float X = 0;

            for (int i = 0; i < ol.Count - 2; i++)
            {
                float obstacleLeftPos = (ol[i].Position - (ol[i].Origin * ol[i].scale)).X;
                if (obstacleLeftPos > characterRightPos)
                {
                    X += obstacleWidth;
                    int tmpI = i+1;
                    int cnt = 1;
                    while (obstacleLeftPos + (obstacleWidth * cnt) + 10 >= (ol[tmpI].Position.X - (ol[tmpI].Origin.X * ol[tmpI].scale.X)))
                    {
                        X += obstacleWidth;
                        cnt++;
                        tmpI++;
                    }
                    return (X/2)+ obstacleLeftPos;
                }
            }


            for (int i = ol.Count - 2; i < ol.Count; i++)
            {
                float obstacleLeftPos = (ol[i].Position - (ol[i].Origin * ol[i].scale)).X;
                if (obstacleLeftPos > characterRightPos)
                {
                    int cnt = 1;
                    X += obstacleWidth;
                    int tmpI = i + 1;
                    while (tmpI<ol.Count && obstacleLeftPos + (obstacleWidth * cnt) + 10 >= (ol[tmpI].Position.X - (ol[tmpI].Origin.X * ol[tmpI].scale.X)))
                    {
                        X += obstacleWidth;
                        cnt++;
                        tmpI++;
                    }
                    return (X / 2) + obstacleLeftPos;
                }
            }
            Global.restartBots = true;
            return 0;
        }

        public static float FindJumpPickPosX(Biker b)
        {
            //const, data
            float g = Tools.pm.g;
            float v0Y = (-40 * g/Global.fps) * b.jumpForce;
            g /= Global.fps;

            //all calculations reagrds to the middle of the biker
            Vector2 middle = b.Position - (b.Origin * b.scale);
            middle = new Vector2(middle.X + (b.CurrentFrame.Width / 2 * b.scale.X), middle.Y + b.CurrentFrame.Height / 2 * b.scale.Y);

            //finding the max height the jumper would arrive
            float h = (float)Math.Pow((v0Y), 2)/(2*g);

            // finding t from the formula y = y0 + v0t + 0.5 at^2
            List<float> t = Tools.quadraticEquation((float)(0.5 * g), v0Y, -h);
            float time = 0;
            for (int i = 0; i < t.Count; i++) // one of the answers is negativ and the other is positive  i need to find the positive one
            {
                if(t[i]>0)
                {
                    time = t[i];
                    break;
                } 
            }

            // finding position X by x = x0+v0t+0.5at^2
            float X =(float) middle.X + Tools.pm.getEngine(b.ID).velocity.X * time + (float)(0.5f*Math.Pow(time,2)*(-0.1f/Global.fps));

            return X;
















        }

        #region not used

        /// <summary>
        /// returns next Obstacle
        /// if there is no closest obstacle returning -999
        /// </summary>
        /// <param name="ol">List<Obstacle></Obstacle></param>
        /// <param name="b">Biker</param>
        /// <returns></returns>
        public static float GetFirstObstaclePosX(List<Obstacle> ol, Biker b)
        {
            float rightPoint = b.Position.X + ((-b.Origin.X + b.CurrentFrame.Width) * b.scale.X);
            float obstacle_leftPoint = -9999;
            for (int i = 0; i < ol.Count(); i++)
            {
                obstacle_leftPoint = ol[i].Position.X + ((-ol[i].Origin.X) * ol[i].scale.X);
                if (obstacle_leftPoint > rightPoint)
                    return obstacle_leftPoint;
            }
            return -999;
        }

        /// <summary>
        /// returns second next Obstacle
        /// if there is no closest obstacle returning -999
        /// </summary>
        /// <param name="ol">List<Obstacle></Obstacle></param>
        /// <param name="b">Biker</param>
        /// <returns></returns>
        public static float GetSecondObstaclePosX(List<Obstacle> ol, Biker b)
        {
            float rightPoint = b.Position.X + ((-b.Origin.X + b.CurrentFrame.Width) * b.scale.X);
            float obstacle_leftPoint = -9999;
            for (int i = 0; i < ol.Count(); i++)
            {
                obstacle_leftPoint = ol[i].Position.X + ((-ol[i].Origin.X) * ol[i].scale.X);
                if (obstacle_leftPoint > rightPoint)
                {
                    if (i + 1 < ol.Count())
                    {
                        obstacle_leftPoint = ol[i + 1].Position.X + ((-ol[i + 1].Origin.X) * ol[i + 1].scale.X);
                        return obstacle_leftPoint;
                    }
                    break;
                }
            }
            return -999;
        }

        /// <summary>
        /// returns third next Obstacle
        /// if there is no closest obstacle returning -999
        /// </summary>
        /// <param name="ol">List<Obstacle></Obstacle></param>
        /// <param name="b">Biker</param>
        /// <returns></returns>
        public static float GetThirdObstaclePosX(List<Obstacle> ol, Biker b)
        {
            float rightPoint = b.Position.X + ((-b.Origin.X + b.CurrentFrame.Width) * b.scale.X);
            float obstacle_leftPoint = -9999;
            for (int i = 0; i < ol.Count(); i++)
            {
                obstacle_leftPoint = ol[i].Position.X + ((-ol[i].Origin.X) * ol[i].scale.X);
                if (obstacle_leftPoint > rightPoint)
                {
                    if (i + 2 < ol.Count())
                    {
                        obstacle_leftPoint = ol[i + 2].Position.X + ((-ol[i + 2].Origin.X) * ol[i + 2].scale.X);
                        return obstacle_leftPoint;
                    }
                    break;
                }
            }
            return -999;
        }

        /// <summary>
        /// returns fourth next Obstacle
        /// if there is no closest obstacle returning -999
        /// </summary>
        /// <param name="ol">List<Obstacle></Obstacle></param>
        /// <param name="b">Biker</param>
        /// <returns></returns>
        public static float GetFourthObstaclePosX(List<Obstacle> ol, Biker b)
        {
            float rightPoint = b.Position.X + ((-b.Origin.X + b.CurrentFrame.Width) * b.scale.X);
            float obstacle_leftPoint = -9999;
            for (int i = 0; i < ol.Count(); i++)
            {
                obstacle_leftPoint = ol[i].Position.X + ((-ol[i].Origin.X) * ol[i].scale.X);
                if (obstacle_leftPoint > rightPoint)
                {
                    if (i + 3 < ol.Count())
                    {
                        obstacle_leftPoint = ol[i + 3].Position.X + ((-ol[i + 3].Origin.X) * ol[i + 3].scale.X);
                        return obstacle_leftPoint;
                    }
                    break;
                }
            }
            return -999;
        }

        public static float GetBulletLeftPosX()
        {
            float n1 = Global.bg.gunner.bullet.Position.X - (Global.bg.gunner.bullet.Origin.X * Global.bg.gunner.bullet.scale.X);
            return n1;
        }

        public static float NoramlizeValue(float v)
        {
            while (Math.Abs(v) > 10)
            {
                v /= 10;
            }
            return v;
        }

        #region notused

        /// <summary>
        /// return time untill arivel to closet obstacle
        /// if there is no closest obstacle returning -999
        /// </summary>
        /// <param name="ol">List<Obstacle></Obstacle></param>
        /// <param name="b">Biker</param>
        /// <returns></returns>
        public static float TimeTillFirstObstacle(List<Obstacle> ol, Biker b)
        {
            float xSpeed = Tools.pm.getEngine(b.ID).velocity.X;
            float rightPoint = b.Position.X + ((-b.Origin.X + b.CurrentFrame.Width) * b.scale.X);
            float obstacle_leftPoint = -999;
            for (int i = 0; i < ol.Count(); i++)
            {
                obstacle_leftPoint = ol[i].Position.X + ((-ol[i].Origin.X) * ol[i].scale.X);
                if (obstacle_leftPoint > rightPoint)
                    return (obstacle_leftPoint - rightPoint) / xSpeed;
            }
            return -999;
        }

        /// <summary>
        /// return time untill arivel to second closest obstacle
        /// if there is no closest obstacle returning -999
        /// </summary>
        /// <param name="ol">List<Obstacle></Obstacle></param>
        /// <param name="b">Biker</param>
        /// <returns></returns>
        public static float TimeTillSecondObstacle(List<Obstacle> ol, Biker b)
        {
            float xSpeed = Tools.pm.getEngine(b.ID).velocity.X;
            float rightPoint = b.Position.X + ((-b.Origin.X + b.CurrentFrame.Width) * b.scale.X);
            float obstacle_leftPoint = -999;
            for (int i = 0; i < ol.Count(); i++)
            {
                obstacle_leftPoint = ol[i].Position.X + ((-ol[i].Origin.X) * ol[i].scale.X);
                if (obstacle_leftPoint > rightPoint)
                {
                    if (i + 1 < ol.Count())
                    {
                        obstacle_leftPoint = ol[i + 1].Position.X + ((-ol[i + 1].Origin.X) * ol[i + 1].scale.X);
                        return (obstacle_leftPoint - rightPoint) / xSpeed;
                    }
                    break;
                }
            }
            return -999;
        }

        /// <summary>
        /// return time untill arivel to third closest obstacle
        /// if there is no closest obstacle returning -999
        /// </summary>
        /// <param name="ol">List<Obstacle></Obstacle></param>
        /// <param name="b">Biker</param>
        /// <returns></returns>
        public static float TimeTillThirdObstacle(List<Obstacle> ol, Biker b)
        {
            float xSpeed = Tools.pm.getEngine(b.ID).velocity.X;
            float rightPoint = b.Position.X + ((-b.Origin.X + b.CurrentFrame.Width) * b.scale.X);
            float obstacle_leftPoint = -999;
            for (int i = 0; i < ol.Count(); i++)
            {
                obstacle_leftPoint = ol[i].Position.X + ((-ol[i].Origin.X) * ol[i].scale.X);
                if (obstacle_leftPoint > rightPoint)
                {
                    if (i + 2 < ol.Count())
                    {
                        obstacle_leftPoint = ol[i + 2].Position.X + ((-ol[i + 2].Origin.X) * ol[i + 2].scale.X);
                        return (obstacle_leftPoint - rightPoint) / xSpeed;
                    }
                    break;
                }
            }
            return -999;
        }

        /// <summary>
        /// finding both distance till obstacke [0]
        /// and both obstacke group width
        /// </summary>
        /// <param name="b">biker</param>
        /// <param name="ol">obstacle list</param>
        /// <returns></returns>
        public static List<float> findDif(Biker b, List<Obstacle> ol) 
        {
            List<float> ret = new List<float>();
            float rightPoint = b.Position.X + ((-b.Origin.X + b.CurrentFrame.Width) * b.scale.X);
            float obstacle_leftPoint = -9999;
            float obstacleWidth = ol[0].CurrentFrame.Width * ol[0].scale.X;
            float X = 0;

            for (int i = 0; i < ol.Count(); i++)
            {
                obstacle_leftPoint = ol[i].Position.X + ((-ol[i].Origin.X) * ol[i].scale.X);
                if (obstacle_leftPoint > rightPoint)
                {
                    ret.Add( obstacle_leftPoint - rightPoint);
                    X += obstacleWidth;
                    int tmpI = i + 1;
                    int cnt = 1;
                    while (tmpI<ol.Count&&obstacle_leftPoint + (obstacleWidth * cnt) + 10 >= (ol[tmpI].Position.X - (ol[tmpI].Origin.X * ol[tmpI].scale.X)))
                    {
                        X += obstacleWidth;
                        cnt++;
                        tmpI++;
                    }
                    ret.Add(X);
                    return ret;
                }
            }
            //if (ret.Count == 0)
            //{
            //    Global.restartBots = true;
            //    Rank.restartBots(Global.copList);
            //    return findDif(b, ol);
            //}
            return ret;
            
        }

        public static float findDifNext(Biker b, List<Obstacle> ol)
        {
            float rightPoint = b.Position.X + ((-b.Origin.X + b.CurrentFrame.Width) * b.scale.X);
            float obstacle_leftPoint = -9999;
            float obstacleWidth = ol[0].CurrentFrame.Width * ol[0].scale.X;
            bool next = false;

            for (int i = 0; i < ol.Count(); i++)
            {
                obstacle_leftPoint = ol[i].Position.X + ((-ol[i].Origin.X) * ol[i].scale.X);
                if (obstacle_leftPoint > rightPoint)
                {
                    int tmpI = i + 1;
                    int cnt = 1;
                    while (tmpI < ol.Count && obstacle_leftPoint + (obstacleWidth * cnt) + 10 >= (ol[tmpI].Position.X - (ol[tmpI].Origin.X * ol[tmpI].scale.X)))
                    {
                        tmpI++;
                        cnt++;
                    }
                    next = true;
                    if (tmpI + 1 < ol.Count)
                        return ol[tmpI + 1].Position.X + ((-ol[tmpI + 1].Origin.X) * ol[tmpI + 1].scale.X) - rightPoint;
                    else return -999;
                }
            }
            return -999;

        }
        #endregion
        #endregion





    }
}
