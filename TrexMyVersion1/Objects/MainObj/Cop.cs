using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrexMyVersion1.ML;
using TrexMyVersion1.Physics;

namespace TrexMyVersion1.Objects
{
    public class Cop : Biker
    {
        #region data
        public NeuralNet nn;
        static int inputNum = 4; // there are 4 inputs for the nueral network
        static int outputNum = 1;
        float[] inputs = new float[inputNum];
        int bumped = 0;
        public int age = 0;
        #endregion

        #region ctor
        public Cop(String mask, Texture2D[] frames, abstractKeys keys, int engI, Engine eng, Texture2D FirstFrame, Vector2 position,
            Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale,
            SpriteEffects effects, float layerDepth, PhysicsManager pm, float jf)
            : base(true, mask, frames, keys, engI, eng, FirstFrame, position, sourceRectangle, color, rotation, origin, scale,
                effects, layerDepth, pm, jf)
        {
            nn = new NeuralNet(inputNum, inputNum + 3, outputNum);
            Game1.Event_Update += shouldJump;
        }

        public Cop(NeuralNet nn, String mask, Texture2D[] frames, abstractKeys keys, int engI, Engine eng, Texture2D FirstFrame, Vector2 position,
            Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale,
            SpriteEffects effects, float layerDepth, PhysicsManager pm, float jf)
            : base(true, mask, frames, keys, engI, eng, FirstFrame, position, sourceRectangle, color, rotation, origin, scale,
                effects, layerDepth, pm, jf)
        {
            this.nn = new NeuralNet(nn);
            this.nn.mutate();
            Game1.Event_Update += shouldJump;
        }

        //public Cop(String mask, Texture2D[] frames, abstractKeys keys, DRC drc,int pase, Texture2D FirstFrame, Vector2 position,
        //    Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale,
        //    SpriteEffects effects, float layerDepth, PhysicsManager pm, float jf)
        //    : base(mask, frames, keys, drc,pase, FirstFrame, position, sourceRectangle, color, rotation, origin, scale,
        //        effects, layerDepth, pm, jf)
        //{
        //    nn = new NeuralNet(inputNum, inputNum + 3, outputNum);
        //    Game1.Event_Update += shouldJump;
        //}

        //public Cop(NeuralNet nn,String mask, Texture2D[] frames, abstractKeys keys, DRC drc, int pase, Texture2D FirstFrame, Vector2 position,
        //    Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale,
        //    SpriteEffects effects, float layerDepth, PhysicsManager pm, float jf)
        //    : base(mask, frames, keys, drc, pase, FirstFrame, position, sourceRectangle, color, rotation, origin, scale,
        //        effects, layerDepth, pm, jf)
        //{
        //    this.nn = new NeuralNet(nn);
        //    this.nn.mutate();
        //    Game1.Event_Update += shouldJump;
        //}

        #region copy
        public Cop(Cop c, int ID, Engine e) : base(c.mask, c.frames, c.keys, ID, e, c.frames[0], new Vector2(c.Position.X, c.Position.Y), c.sourceRectangle, c.color, c.Rotation, c.Origin, c.scale,
                c.effects, c.layerDepth, Tools.pm, c.jumpForce)
        {
            this.nn = new NeuralNet(c.nn);
            nn.mutate();
            Game1.Event_Update += shouldJump;
        }
        public Cop(Cop c, int ID, Engine e, DRC drc) : base(c.mask, c.frames, c.keys, drc, c.pase, c.frames[0], c.Position, c.sourceRectangle, c.color, c.Rotation, c.Origin, c.scale,
                c.effects, c.layerDepth, Tools.pm, c.jumpForce)
        {
            this.nn = new NeuralNet(c.nn);
            nn.mutate();
            Game1.Event_Update += shouldJump;
        }

        #endregion

        #endregion

        #region updates
        public void shouldJump()
        {
            if (touchesGround == collisionState.collision)
            {
                bool[] answers = What_should_I_doJ();
                if (answers[0] == true)
                    this.keys.DefSpace(true);
                else
                    this.keys.DefSpace(false);
                this.keys.DefRight(true);
            }
            else
            {
                this.keys.DefLeft(false);
                this.keys.DefRight(false);
                this.keys.DefSpace(false);
            }

        }

        public void update()
        {
            this.age++;
            bool[] answers = What_should_I_do();
            if (answers[0])
            {
                this.keys.DefLeft(true);
                this.keys.DefRight(false);
            }
            if (answers[1])
            {
                this.keys.DefLeft(false);
                this.keys.DefRight(true);
            }

            this.keys.DefSpace(answers[2]);
        }

        #endregion

        #region calculate
        private bool[] What_should_I_doJ()
        {
            #region input order
            /* 1. speed
             * 2.distance
             * 3.width
             * 4.can jump
             */
            #endregion
            inputs[0] = Tools.pm.getEngine(this.ID).velocity.X;
            List<float> ins = GetAsentialData.findDif(this, Global.bg.obstacleList);
            if (ins.Count != 0)
            {
                inputs[1] = ins[0] / 10;//distamce between obstacle and jump
                inputs[2] = ins[1] / 10;//obstacle width
            }
            else
            {
                bool[] answers1 =   new bool[outputNum];
                answers1[0] = false; ;
                return answers1;
            }

            if (GetAsentialData.CanJump(this))
                inputs[3] = 1;
            else inputs[3] = -1;

            //activating neuralNet
            float[] output = nn.calculate(inputs);

            //maping the outputs
            bool[] answers = new bool[outputNum];
            for (int i = 0; i < answers.Length; i++)
            {
                if (output[i] > 0)
                    answers[i] = true;
                else
                    answers[i] = false;
            }

            //returning answers
            return answers;
        }
        private bool[] What_should_I_doJR()
        {
            #region input order
            /* 1. speed
             * 2.distance
             * 3.width
             * 4.can jump
             * 5. distance to second group
             */
            #endregion
            //// setting input
            inputs[0] = Tools.pm.getEngine(this.ID).velocity.X; // speed
            List<float> ins = GetAsentialData.findDif(this, Global.bg.obstacleList); // find data for jump
            inputs[1] = ins[0] / 100;//distamce between obstacle and jump
            inputs[2] = ins[1] / 10;//obstacle width

            //if can jump 1 else -1
            if (GetAsentialData.CanJump(this))
                inputs[3] = 1;
            else inputs[3] = -1;

            //distance to next Obstacle
            inputs[4] = GetAsentialData.findDifNext(this, Global.bg.obstacleList) / 100;

            //activating neuralNet
            float[] output = nn.calculate(inputs);

            //maping the outputs
            bool[] answers = new bool[outputNum];
            for (int i = 0; i < answers.Length; i++)
            {
                if (output[i] > 0)
                    answers[i] = true;
                else
                    answers[i] = false;
            }

            //returning answers
            return answers;
        }
        private bool[] What_should_I_do()
        {
            #region input order
            /* 1. my real poisition.X
             * 2. my width
             * 3. my X speed
             * 4. can I Jump
             * 5. obstacle Height
             * 6. first obstacle posX
             * 7. second obstacle posX
             * 8. third obstacle posX
             * 9. fourth obstacle posX
             * 10. obstacle Width 
             * 11. my scale
             * 12. an obstacle scale
             * 13. bullet Left pos
             */
            #endregion
            //// setting input
            //float[] inputs = new float[inputNum];
            inputs[0] = (this.Position - this.Origin).X;
            inputs[1] = this.CurrentFrame.Width;
            inputs[2] = pm.getEngine(ID).velocity.X;

            //if can jump 1 else 0
            if (GetAsentialData.CanJump(this))
                inputs[3] = 1;
            else
                inputs[3] = 0;
            inputs[4] = Global.bg.obstacleList[0].CurrentFrame.Height;
            inputs[5] = GetAsentialData.GetFirstObstaclePosX(Global.bg.obstacleList, this);
            inputs[6] = GetAsentialData.GetSecondObstaclePosX(Global.bg.obstacleList, this);
            inputs[7] = GetAsentialData.GetThirdObstaclePosX(Global.bg.obstacleList, this);
            inputs[8] = GetAsentialData.GetFourthObstaclePosX(Global.bg.obstacleList, this);
            inputs[9] = Global.bg.obstacleList[0].CurrentFrame.Width;
            inputs[10] = this.scale.X;
            inputs[11] = Global.bg.obstacleList[0].scale.X;
            inputs[12] = GetAsentialData.GetBulletLeftPosX();

            //normalizing all inputs
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i] = GetAsentialData.NoramlizeValue(inputs[i]);
            }

            //activating neuralNet
            float[] output = nn.calculate(inputs);

            //maping the outputs
            bool[] answers = new bool[outputNum];
            for (int i = 0; i < answers.Length; i++)
            {
                if (output[i] > 0)
                    answers[i] = true;
                else
                    answers[i] = false;
            }

            //returning answers
            return answers;
        }
        private bool[] Wait_for_keys()
        {
            bool[] ret = new bool[3];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = false;
            }
            if (Tools.ks.IsKeyDown(Keys.W))
                ret[2] = true;
            if (Tools.ks.IsKeyDown(Keys.A))
                ret[0] = true;
            if (Tools.ks.IsKeyDown(Keys.A))
                ret[1] = true;
            return ret;
        }

        #endregion

        public override void kill()
        {
            Game1.Event_Update -= update;
            base.kill();
        }
    }
}
