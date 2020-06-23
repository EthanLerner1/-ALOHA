using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TrexMyVersion1.Objects;
using TrexMyVersion1.Physics;
using TrexMyVersion1.View;

namespace TrexMyVersion1.Online
{
    class HostOnlineGame : OnlineGame
    {

        public HostOnlineGame(int port)
        {
            this.port = port;
        }

        protected override void InitChars()
        {
            Global.Init();
            
        }

        protected override void SocketThread()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            client = listener.AcceptTcpClient();

            reader = new BinaryReader(client.GetStream());
            writer = new BinaryWriter(client.GetStream());

            this.SyncBG();

            base.RaiseOnConnectionEvent();


            while (true)
            {
                WriteCharacterData(Global.me);
                ReadAndUpdateCharacter(Global.player2);

                Thread.Sleep(10);
            }

        }
        public override void SyncBG()
        {
            WriteAndReadClasses.WriteNecceryInitGlobal(writer, Global.bg.currentLevel, Global.bg.obstacleList);
        }
    }
}
