using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrexMyVersion1.Objects;
using TrexMyVersion1.View;

namespace TrexMyVersion1.Online
{
    public static class WriteAndReadClasses
    {
        #region writing and reading actual classes
        #region ////////////    W R I T E   ////////////
        public static void writeDrawObj(BinaryWriter writer, DrawObj d)
        {
            #region order
            /*
             * order:
             * position X
             * position Y
             * Rotation
             * Origin X
             * Origin Y
             * amoint $$int$$
             * Color R
             * Color G
             * Color B
             * Color A
             * scale X = scaleY
             * layer depth;
             */
            #endregion
            //floats
            writer.Write(d.Position.X);
            writer.Write(d.Position.Y);
            writer.Write(d.Rotation);
            writer.Write(d.Origin.X);
            writer.Write(d.Origin.Y);
            //int
            writer.Write(d.amount);
            //byte
            writer.Write(d.color.R);
            writer.Write(d.color.G);
            writer.Write(d.color.B);
            writer.Write(d.color.A);
            //float
            writer.Write(d.scale.X);
            writer.Write(d.layerDepth);
        }

        public static void writeLevel(BinaryWriter writer, Level d)
        {
            #region order
            /*
             * order:
             * all of the DrawObj
             * groundPos X
             * groundPos Y
             */
            #endregion
            writeDrawObj(writer, d);
            writer.Write(d.groundPos.X);
            writer.Write(d.groundPos.Y);
        }

        public static void writeMovingObj(BinaryWriter writer, DrawObj d) { }
        #endregion

        #region ////////////    R E A D   ////////////
        public static void readDrawObj(BinaryReader reader)
        {
            #region order
            /*
             * order:
             * position X
             * position Y
             * Rotation
             * Origin X
             * Origin Y
             * amoint $$int$$
             * Color R
             * Color G
             * Color B
             * Color A
             * scale X = scaleY
             * layer depth;
             */
            #endregion

            #endregion
        }
        #endregion



        #region read and write stages

        #region //// init

        /// <summary>
        /// order: int,level amount. int, amount of obstacles. float, by order posX posY obstacles
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="level"></param>
        /// <param name="obstacles">List of the obstacles</param>
        public static void WriteNecceryInitGlobal(BinaryWriter writer, DrawObj level, List<Obstacle> obstacles)
        {
            #region order
            /* level amount
             * amount of obstacles
             * by order obstacles position
             */
            #endregion
            //int
            writer.Write(level.amount);
            writer.Write(obstacles.Count());

            // every time posX, pos Y (floats)
            for (int i = 0; i < obstacles.Count(); i++)
            {
                writer.Write(obstacles[i].Position.X);
                writer.Write(obstacles[i].Position.Y);
            }
        }

        /// <summary>
        /// returns a list which contains the following data:
        ///  order: int,level amount. int, amount of obstacles. float, by order posX posY obstacles
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<float> ReadNecceryInitGlobal(BinaryReader reader)
        {
            List<float> ret = new List<float>();
            ret.Add(reader.ReadInt32());//level amount
            int i = reader.ReadInt32();
            ret.Add(i);//obstacle amount
            for (int t = 0; t < i; t++)
            { //(x,y)
                ret.Add(reader.ReadSingle());
                ret.Add(reader.ReadSingle());
            }
            return ret;
        }

        #endregion

        #region ////update

        public static void WriteNecceryUpdataData (BinaryWriter writer, Biker me, List<Obstacle> ol)
        {
            #region order
            /* float, my posX
             * float, my posY
             * int, switchframe, 0 non, 1 regular, -1 reverse
             * list<Boolean>, which obstacle exploaded
             * int, my score
             */
            #endregion
            //float
            writer.Write(me.Position.X);
            writer.Write(me.Position.X);

            //int
            order tmp = me.switchF;
            if (tmp == order.regular)
                writer.Write(1);
            else
            {
                if (tmp == order.none)
                    writer.Write(0);
                else
                    writer.Write(-1);
            }

            //bool
            for (int i = 0; i < ol.Count(); i++)
            {
                writer.Write(ol[i].switchFrames);
            }
            writer.Write(Global.myScore);
        }

        #endregion

        #endregion


    }
}
