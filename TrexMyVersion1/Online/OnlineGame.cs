using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrexMyVersion1.Objects;
using TrexMyVersion1.Physics;
using TrexMyVersion1.View;


namespace TrexMyVersion1.Online
{
    public delegate void OnConnectionHandler();

    abstract class OnlineGame
    {
        protected BinaryReader reader;
        protected BinaryWriter writer;

        protected Thread thread;

        protected TcpClient client;

        protected int port;

        public event OnConnectionHandler OnConnection;

        protected void RaiseOnConnectionEvent()
        {
            if (OnConnection != null)
            {
                OnConnection();
            }
                
        }

        public void Init()
        {
            InitChars();
            StartCommunication();
        }

        protected abstract void InitChars();

        public void StartCommunication()
        {
            thread = new Thread(new ThreadStart(SocketThread));
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// this need to change thats the data im passing
        /// </summary>
        /// <param name="c"></param>
        protected void ReadAndUpdateCharacter(Animation c)
        {
            /* float, my posX
             * float, my posY
             * int, switchframe, 0 non, 1 regular, -1 reverse
             * Boolean, collidedWithObsatcle
             * list<Boolean>, which obstacle exploaded
             * int, my score
             */

            float X = reader.ReadSingle();
            float Y = reader.ReadSingle();

            c.Position = new Vector2(X, Y);


            int switchFrame = reader.ReadInt32();
            if (switchFrame == 0)
                c.switchF = order.none;
            if (switchFrame == 1)
                c.switchF = order.regular;
            if (switchFrame == -1)
                c.switchF = order.reverse;
            for (int i = 0; i < Global.bg.obstacleList.Count(); i++)
            {
                Global.bg.obstacleList[i].switchFrames = reader.ReadBoolean();
            }
        }

        protected void WriteCharacterData(DrawObj c)
        {
            WriteAndReadClasses.WriteNecceryUpdataData(writer, Global.me, Global.bg.obstacleList);
            //writer.Write(c.pos.X);
            //writer.Write(c.pos.Y);
            //writer.Write(c.rot);
        }

        public abstract void SyncBG();

        protected abstract void SocketThread();

    }
}
