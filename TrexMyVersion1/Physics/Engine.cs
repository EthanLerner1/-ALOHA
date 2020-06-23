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
    public class Engine
    {
        const float MAXRPM = 6000;
        const float RPMINC = 250;
        const float FRICTION = 0.002f;
        const float MINRPM = 1000;

        #region data
        public float wheelrot { get; private set; }
        public float speed { get;  set; }
        public Vector2 velocity { get; set; }
        public float rpm { get; set; }
        float power { get; set; }
        float maxpower;
        float maxsteer;
        float maxspeed;
        float breakpower = 1;

        #endregion
        #region ctor
        public Engine(Engine e)
        {
            maxpower = e.maxpower;
            maxsteer = e.maxsteer;
            maxspeed = e.maxspeed;
            breakpower = e.breakpower;
            wheelrot = 0;
            rpm = e.rpm;
            power = 0;
            speed = e.speed;
            velocity = new Vector2(e.velocity.X,e.velocity.Y);

        }
        public Engine( float maxpower,
            float maxsteer, float maxspeed, float maxbreak)
        {
            this.maxpower = maxpower;
            this.maxsteer = maxsteer;
            this.maxspeed = maxspeed;
            wheelrot = 0;
            rpm = 0;
            power = 0;
            speed = 0;
            this.velocity = Vector2.Zero;
        }

        #endregion
        #region func
        public void engine_update(abstractKeys keys)
        {

            power = 0;

            if (keys.Right())
            {
                if (rpm < MINRPM)
                    rpm = MINRPM; // in case the player is allready in motion
                else 
                if (rpm > MAXRPM) 
                    rpm = MAXRPM; // setting boundris
                rpm += RPMINC;
                power = rpm / MAXRPM * maxpower / 250; // calculating power, (not based on phizics just aproximetly)
                if (speed >= 0)
                    speed = speed - speed * FRICTION + power / (speed + 0.2f);
                else
                {
                    //rpm = MINRPM;
                    speed = speed - speed * FRICTION + Math.Abs(power / (speed + 0.2f)) + 1f;
                    
                }
            }
            if (keys.Left())
            {
                if (rpm > -MINRPM)
                    rpm = -MINRPM; // in case the player is allready in motion
                else if (rpm < -MAXRPM)
                    rpm = -MAXRPM; // setting boundris
                rpm -= RPMINC;// decrising rpm

                power = -Math.Abs(rpm / MAXRPM * maxpower / RPMINC); // calculating power, (not based on phizics)

                // case 1: speed is positie, the player is moving foward
                if (speed <= 0)
                    speed = speed + speed * FRICTION - Math.Abs(power / (speed + 0.2f));
                // case 2: speed is negative, the player is moving backwards
                else
                {
                    //rpm = -MINRPM; // if i wont do this when the speed reaches 0 the rpm would be to big and there wont be any feal of phizics.
                    speed = speed - speed * FRICTION - Math.Abs(power / (speed + 0.2f)) - 1f;
                   
                }
            }


            //setting speed limits
            if (speed > maxspeed) speed = maxspeed;
            if (speed < -maxspeed) speed = -maxspeed;

            this.velocity = new Vector2(speed, velocity.Y);
            #endregion
        }
    }
}
