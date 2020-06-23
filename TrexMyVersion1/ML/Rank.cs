using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrexMyVersion1.Objects;
using System.IO;
using TrexMyVersion1.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrexMyVersion1.ML
{
    public class Rank
    {
        #region data
        private int amount_of_runs;
        private int cntRound = 0;
        private int whichOne = 1;
        private List<Cop> bots;
        public Cop lastBestBot { get; set; }
        Texture2D[] copArray = Animation.loadTextures("copGif", 1);
        Texture2D copF;
        #endregion

        #region ctor
        public Rank(List<Cop> bots)
        {
            amount_of_runs = 100;
            Game1.Event_Update += update;
            this.bots = bots;
            copF = copArray[0];
        }
        #endregion

        private void update()
        {
            //restart
            if (Global.restartBots == true)
            {
                restartBots(bots);
            }

            //find bestC
            Cop bestC;
            int best = FindIndexOfMostAdvancedPos();
            bestC = bots[best];
            if (lastBestBot != null && bestC.Position.X > lastBestBot.Position.X)
            {
                lastBestBot = bestC;
            }
            else
            {
                if (lastBestBot != null)
                {
                    lastBestBot.Position = bestC.Position;
                    bestC = lastBestBot;
                    bestC.nn.mutate();
                };

            }
            if (lastBestBot == null)
                lastBestBot = bestC;


            if (allDead())
            {
                for (int i = 0; i < bots.Count; i++)
                {
                    bots[i].Position = new Vector2(0, -100);
                    bots[i].nn = new NeuralNet(lastBestBot.nn);
                    bots[i].nn.mutate();
                    bots[i].dead = false;
                    Global.copList[i].dead = false;
                    Tools.pm.changeSpeed(bots[i].ID, Vector2.Zero);
                    lastBestBot.Position = new Vector2(-999999);
                }
                return;
            }


            Global.cam = bestC.cam;
            for (int i = 0; i < bots.Count; i++)
            {
                if (bots[i].dead == true)
                {
                    bots[i].Position = new Vector2(bestC.Position.X, bestC.Position.Y);
                    NeuralNet nnT = new NeuralNet(bestC.nn);
                    nnT.mutate();
                    bots[i].nn = nnT;
                    Tools.pm.changeSpeed(bots[i].ID, Tools.pm.getEngine(bestC.ID).velocity);
                }
            }

            if (cntRound == amount_of_runs)
            {
                cntRound = 0;//restart counter

                //List<int> worstI = Find_index_of_worst(5);
                //for (int i = 0; i < worstI.Count; i++)
                //{
                //    bots[worstI[i]].kill();
                //    Tools.pm.createNewEngine(bots[worstI[i]].ID, Tools.pm.getEngine(bestC.ID));
                //    bots[worstI[i]] = new Cop(bestC.nn, "masks/policeCarM", copArray, new BotKeys(), bots[worstI[i]].ID, null,
                //copF, new Vector2(bestC.Position.X, bestC.Position.Y), null, Color.White, 0, new Vector2(730, 1440),
                //       new Vector2(0.1f), 0, 0.1f, Tools.pm, 1.2f);
                //}
                //File.WriteAllText("saveNN.txt", bestC.nn.ToString());
                using (StreamWriter sw = File.AppendText("saveNN.txt"))
                {
                    sw.WriteLine(bestC.nn.ToString());
                }
            }

            else
            {
                cntRound++;
            }
        }

        public static void restartBots (List<Cop> bots)
        {
            for (int i = 0; i < bots.Count; i++)
            {
                bots[i].Position = new Vector2(0, -100);
                Tools.pm.changeSpeed(bots[i].ID, Vector2.Zero);

            }
            Global.restartBots = false;
        }
        bool allDead()
        {
            for (int i = 0; i < bots.Count; i++)
            {
                if (bots[i].dead != true)
                    return false;
            }
            return true;
        }

        #region not used


        /// <summary>
        /// returns most advanced cop
        /// this function destroys the list
        /// </summary>
        /// <param name="bots"></param>
        /// <returns></returns>
        Cop FindMostAdvancedCop(List<Cop> bots)
        {
            float bestPosX = -9999999;
            Cop best = null;
            for (int i = 0; i < bots.Count; i++)
            {
                if (bots[i] != null && bots[i].Position.X > bestPosX)
                {
                    bestPosX = bots[i].Position.X;
                    best = bots[i];
                }
            }
            for (int i = 0; i < bots.Count; i++)
            {
                if (bots[i] != null && best.Position.X == bots[i].Position.X)
                    bots[i] = null;
            }
            return best;
        }
        #endregion
        int FindIndexOfMostAdvancedPos()
        {
            //List<int> ret = new List<int>();
            int ret = 0;
            float bestPosX = -9999999;
            int best = -1;

            for (int t = 0; t < bots.Count; t++)
            {
                if (bots[t].dead == false && (bots[t].Position.X > bestPosX))
                    bestPosX = bots[t].Position.X;

            }
            for (int t = 0; t < bots.Count; t++)
            {
                if (bots[t].Position.X == bestPosX)
                    return t;
            }
            return ret;

        }

        int FindIndexOfOldest()
        {
            int ret = 0;
            float bestAge = -9999999;
            int best = -1;

            for (int t = 0; t < bots.Count; t++)
            {
                if ((bots[t].age > bestAge))
                    bestAge = bots[t].age;

            }
            for (int t = 0; t < bots.Count; t++)
            {
                if (bots[t].age == bestAge)
                    return t;
            }
            return ret;
        }

        List<int> RemoveWorst(List<Cop> bots, int amount)
        {
            List<int> ret = new List<int>();
            for (int i = 0; i < amount; i++)
            {
                ret.Add(RemoveLeastAdvancedCop(bots));
            }
            return ret;
        }

        int RemoveLeastAdvancedCop(List<Cop> bots)
        {
            float worstposX = 9999999999999999;
            Cop worst = null;
            for (int i = 0; i < bots.Count; i++)
            {
                if (bots[i] != null && bots[i].Position.X < worstposX)
                {
                    worstposX = bots[i].Position.X;
                    worst = bots[i];
                }
            }
            for (int i = 0; i < bots.Count; i++)
            {
                if (bots[i] != null && worst.Position.X == bots[i].Position.X)
                {
                    //bots[i].kill();
                    //bots[i] = null;
                    return i;
                }

            }
            return 0;
        }

        List<int> Find_index_of_worst(int amount)
        {
            List<int> ret = new List<int>();
            List<float> posX = new List<float>();
            for (int i = 0; i < bots.Count; i++)
            {
                posX.Add(bots[i].Position.X);
            }
            for (int i = 0; i < amount; i++)
            {
                ret.Add(Get_index_of_min(posX));
            }
            return ret;
        }

        int Get_index_of_min(List<float> l)
        {
            float min = 999999;
            int index = 0;
            for (int i = 0; i < l.Count; i++)
            {
                if (min > l[i])
                {
                    min = l[i];
                    index = i;
                }
            }
            l[index] = 999999;
            return index;
        }
    }
}
