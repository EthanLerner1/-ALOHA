using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TrexMyVersion1.Objects;

namespace TrexMyVersion1.Physics
{
    public enum DRC
    {
        left, right
    } 
    public class PhysicsManager
    {
        #region data
        private List<Engine> eng = new List<Engine>();
        public float g = 9.8f;
        private int delay = 0;
        private int collideState;

        #endregion

        #region ctor
        public PhysicsManager()
        {
            Background.Event_NextLevel += nxtLevel;
        }
        #endregion

        #region funcs
        #region ////////////// ENGINE /////////////////
        #region ///// physics engine

        public void createNewEngine(Engine eng1)
        {//creating a new engine in the list with the same values as the given engine
            this.eng.Add(new Engine(eng1));
        }
        public void createNewEngine(int ID,Engine e/*, float speed, Vector2 velocity*/)
        {//creating a new engine in the list with the same values as the given engine
            if (eng.Count-1>=ID)
            {
                this.eng[ID] = (new Engine(e));
                /*this.eng[ID].speed = speed;
                this.eng[ID].velocity = velocity;*/
            }
            
        }

        public void changeSpeed (int ID, Vector2 speed)
        {
            this.eng[ID].velocity = speed;
            this.eng[ID].speed = speed.X;
        }

        public void engineupdate(abstractKeys keys, int index, IFocous f)
        {//activates the update function of an engine by his index
            eng[index].engine_update(keys);
        }
        public Engine getEngine(int index)
        {//gets the index of the wanted engine and returns a copy of this engine
            return new Engine(eng[index]);
        }

        public static Engine createAndGetNewEngine(float maxpower, float maxsteer, float maxspeed, float maxbreak)
        {
            return new Engine(maxpower, maxsteer, maxspeed, maxbreak);
        }
        #endregion
        #region //// static engine
        public void activateStaticEngine (DRC drc , IFocous character, int pase)
        {
            switch (drc)
            {
                case DRC.right:
                    character.Position = new Vector2(character.Position.X + (pase / 50), character.Position.Y);
                    break;
                case DRC.left:
                    character.Position = new Vector2(character.Position.X - (pase / 50), character.Position.Y);
                    break;
            }
        }
        #endregion
        #endregion

        #region ////////////// ROAD MANAGMENT /////////////////

        /// <summary>
        /// activates the gravity effect between the road and the player
        /// activates gravity when he is on the air
        /// </summary>
        /// <param name="player">Biker, the player that have interaction with the road</param>
        public collisionState roadPlayerInteraction(Biker player)
        {
            if (Collision.roadBikerState(Global.bg.currentLevel, player) == collisionState.collision)
            {//stoping hes movment on the Y axis
                this.eng[player.ID].velocity = new Vector2(this.eng[player.ID].speed, 0);


                if(!player.keys.Right())
                {
                    this.eng[player.ID].velocity = new Vector2(this.eng[player.ID].velocity.X - (0.1f / Global.fps), 0);
                    this.eng[player.ID].speed = this.eng[player.ID].velocity.X - (0.1f / Global.fps);
                }
                if (player.keys.Space())
                    jump(player);


                // allowing the biker to paddle
                return collisionState.collision;
            }
            else // activate gravity and air firction
            {
                this.eng[player.ID].velocity = new Vector2(this.eng[player.ID].velocity.X -(0.1f / Global.fps), this.eng[player.ID].velocity.Y + g / Global.fps);
                this.eng[player.ID].speed = this.eng[player.ID].velocity.X - (0.1f / Global.fps);
            }
               
            return collisionState.noCollision;
        }
          
        public void jump (Biker player)
        {//this.eng[player.ID].velocity.Y +
            float newSpeed =  (g / Global.fps * -40)*player.jumpForce;
            this.eng[player.ID].velocity = new Vector2(this.eng[player.ID].speed, newSpeed);
        }

        #endregion

        #region ///////////////// physics effects /////////////////
        public void bumped (Biker p)
        {
            jump(p);
            Vector2 tmp = new Vector2(-this.eng[p.ID].velocity.X * -0.7f, this.eng[p.ID].velocity.Y);
            this.eng[p.ID].speed = this.eng[p.ID].speed*-0.7f;
        }
        #endregion

        #region nextLevel
        private void nxtLevel()
        {
            for (int i = 0; i < this.eng.Count(); i++)
            {
                this.eng[i].speed = 0;
                this.eng[i].velocity = Vector2.Zero;
                this.eng[i].rpm = 0;
            }
        }
        #endregion


        #endregion
    }
}
