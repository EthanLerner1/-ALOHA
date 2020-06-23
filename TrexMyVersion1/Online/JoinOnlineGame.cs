using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
    class JoinOnlineGame : OnlineGame
    {
        string hostip;

        public JoinOnlineGame(string hostip, int port)
        {
            this.port = port;
            this.hostip = hostip;
        }

        protected override void InitChars()
        {
            
        }

        protected override void SocketThread()
        {

            client = new TcpClient();
            client.Connect(hostip, port);

            reader = new BinaryReader(client.GetStream());
            writer = new BinaryWriter(client.GetStream());

            this.SyncBG();

            base.RaiseOnConnectionEvent();

            while (true)
            {
                ReadAndUpdateCharacter(Global.player2);
                WriteCharacterData(Global.me);

                Thread.Sleep(10);
            }
        }

        public override void SyncBG()
        {
            /* level amount
             * amount of obstacles
             * by order obstacles position
             */
            int levelAmount = reader.ReadInt32();
            int obstacleAmount = reader.ReadInt32();
            List<Obstacle> ol = new List<Obstacle>();

            Texture2D frame = Tools.cm.Load<Texture2D>("obstacle/0");
            String mask = "obstacle/obstacleMask";
            Vector2 scale = new Vector2(0.2f);
            Vector2 origin = new Vector2(256, 431);

            for (int i = 0; i < obstacleAmount; i++)
            {
                ol.Add(new Obstacle(frame, mask, Tools.cm.Load<Texture2D>("obstacle/explosion/Explosion")
                        , Tools.cm.Load<Texture2D>("obstacle/explosion/ExplosionM"),
                        null, -999, null, new Vector2(reader.ReadSingle(), reader.ReadSingle()),
                        null, Color.White, 0, origin, scale, SpriteEffects.None, 0, Tools.pm));
            }
            Global.Init(levelAmount, ol);
        }
    }
}
