using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrexMyVersion1.View;
using TrexMyVersion1.Physics;
using TrexMyVersion1.Objects;
using System.Collections.Generic;
using TrexMyVersion1.Online;
using System.IO;
using Microsoft.Xna.Framework.Media;
using TrexMyVersion1.ML;

namespace TrexMyVersion1
{
    enum OnlineState
    {
        AskingRole, //host or join
        Connecting,
        Playing,
        FirstRun
    }

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static event DlgDraw Event_Draw; // regestering an event that draws everything in the game
        public static event DlgUpdate Event_Update; //regestering an event that updates everything in the game



        Background bg;
        DrawObj startScreen;

        //online
        OnlineGame onlineGame;
        OnlineState state = OnlineState.AskingRole;

        //ML
        Rank rankMl;

        //vp
        Viewport leftVp, rightVp;
        Camera Cme, Cenemy;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            IsMouseVisible = true;
            graphics.PreferredBackBufferHeight = 700;
            graphics.PreferredBackBufferWidth = 1200;
            graphics.ApplyChanges();
        }


        protected override void Initialize()
        {

            base.Initialize();
        }


        protected override void LoadContent()
        {
            Tools.Init(GraphicsDevice, Content, this);
            Global.Init();


            //online
            startScreen = new DrawObj(Tools.cm.Load<Texture2D>("screens/startScreen"), new Vector2(0), null, Color.White, 0, Vector2.Zero, new Vector2(0.24385f, 0.2154f), SpriteEffects.None, 0);
            //Texture2D[] copArray = Animation.loadTextures("copGif", 1);
            //Texture2D copF = copArray[0];
            //Engine copy = PhysicsManager.createAndGetNewEngine(50, 0.2f, 15, 1);

            //for (int i = 0; i < 200; i++)
            //{
            //    Global.copList.Add(new Cop("masks/policeCarM", copArray, new BotKeys(), Global.copList.Count + 2, copy,
            //    copF, new Vector2(0, -100), null, Color.White, 0, new Vector2(730, 1440),
            //           new Vector2(0.1f), 0, 0.1f, Tools.pm, 1.2f));
            //}

            //rankMl = new Rank(Global.copList);


            //camera
            leftVp = new Viewport(0, 0, Tools.W / 2, Tools.H);
            rightVp = new Viewport(Tools.W / 2, 0, Tools.W / 2, Tools.H);

            Cme = new Camera(Global.me, leftVp, Vector2.Zero, Global.zoom);
            Cenemy = new Camera(Global.player2, leftVp, Vector2.Zero, Global.zoom);

            //Global.cam = new Camera(Global.me, new Viewport(0, 0, Tools.W, Tools.H), Vector2.Zero, Global.zoom);
        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (Global.gamseStarted)
                Event_Update?.Invoke();//update happend
            else
                Tools.update();

            Window.Title = "my score: " + Global.myScore + "  enemy score: " + Global.enemyScore + "   cop score: " + Global.copScore;

            base.Update(gameTime);

            #region online
            //Tools.update();

            //switch (state)
            //{
            //    case OnlineState.AskingRole:
            //        if (Tools.ks.IsKeyDown(Keys.H))
            //        {
            //            onlineGame = new HostOnlineGame(int.Parse(File.ReadAllText("port.txt")));
            //            onlineGame.OnConnection += new OnConnectionHandler(onlineGame_OnConnection);
            //            onlineGame.Init();

            //            state = OnlineState.Connecting;
            //        }
            //        else if (Tools.ks.IsKeyDown(Keys.J))
            //        {
            //            onlineGame = new JoinOnlineGame(File.ReadAllText("ip.txt"), int.Parse(File.ReadAllText("port.txt")));
            //            onlineGame.OnConnection += new OnConnectionHandler(onlineGame_OnConnection);
            //            onlineGame.Init();

            //            state = OnlineState.Connecting;
            //        }
            //        break;

            //    case OnlineState.Connecting:
            //        break;

            //    case OnlineState.Playing:
            //        Event_Update?.Invoke();//update happend
            //        Window.Title = "my score: " + Global.myScore + "  enemy score: " + Global.enemyScore + "   cop score: " + Global.copScore;
            //        break;
            //}

            //this.Window.Title = state.ToString();
            //base.Update(gameTime);
            #endregion
        }


        #region online
        //protected override void Draw(GameTime gameTime)
        //{
        //    //if (OnlineState.AskingRole == state || OnlineState.Connecting == state)
        //    //    Tools.DrawSingle(startScreen);
        //    //else
        //    //{
        //    //    startScreen.kill();
        //    GraphicsDevice.Clear(Color.CornflowerBlue);
        //    Tools.sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
        //        null, null, null, null, Global.cam.Mat);
        //    Event_Draw?.Invoke();//draw hapend
        //    Tools.sb.End();
        //    //}

        //    base.Draw(gameTime);
        //}
        void onlineGame_OnConnection()
        {
            state = OnlineState.Playing;

        }

        //protected override void Draw(GameTime gameTime)
        //{
        //    //if (OnlineState.AskingRole == state || OnlineState.Connecting == state)
        //    //    Tools.DrawSingle(startScreen);
        //    //else
        //    //{
        //        //startScreen.kill();
        //        GraphicsDevice.Clear(Color.CornflowerBlue);

        //        if (state == OnlineState.Playing)
        //        {
        //            Tools.sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
        //            null, null, null, null, Global.me.cam.Mat);
        //            Event_Draw?.Invoke();//draw hapend
        //            Tools.sb.End();
        //            base.Draw(gameTime);
        //        }
        //    //}
        //    base.Draw(gameTime);
        //}
        #endregion

        protected override void Draw(GameTime gameTime)
        {
            if (!Global.gamseStarted)
            {
                Tools.DrawSingle(startScreen);
                if (Tools.ks.IsKeyDown(Keys.S))
                {
                    Global.gamseStarted = true;
                    startScreen.kill();
                }

                return;
            }

            if (Global.gameEnd != true)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                DrawViewport(rightVp, Cme.Mat);
                DrawViewport(leftVp, Cenemy.Mat);
            }
            else
            {
                Global.nxtlevel();
                //GraphicsDevice.Viewport = new Viewport(0, 0, Tools.W, Tools.H);
            }

            base.Draw(gameTime);
        }

        protected void DrawViewport(Viewport vp, Matrix camMatrix)
        {
            GraphicsDevice.Viewport = vp;

            Tools.sb.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null,
       camMatrix);

            Event_Draw?.Invoke();//draw hapend

            Tools.sb.End();
        }


    }
}

