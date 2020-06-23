using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TrexMyVersion1.Objects
{
    #region abstractKeys
    public abstract class abstractKeys
    {
        public abstract bool Left();
        public abstract bool Right();
        public abstract bool Space();
        public abstract void DefLeft(bool b);
        public abstract void DefRight(bool b);
        public abstract void DefSpace(bool b);
        public abstract void WhoAmI(IFocous me);

    }
    #endregion

    #region user
    class UserKeys : abstractKeys
    {
        Keys  left, right, jump;
        public UserKeys(Keys left, Keys right, Keys jump)
        {
            this.left = left;
            this.right = right;
            this.jump = jump;
        }
        public override bool Left()
        {
            return Tools.ks.IsKeyDown(left);
        }
        
        public override bool Right()
        {
            return Tools.ks.IsKeyDown(right);
        }
        public override bool Space()
        {
            return Tools.ks.IsKeyDown(jump);
        }

        public override void DefLeft(bool b) { }
        public override void DefRight(bool b) { }
        public override void DefSpace(bool b) { }
        public override void WhoAmI(IFocous me) // just because user keys inherit base keys
        {
        }
    }
    #endregion
    //class BotKeys : BaseKeys
    //{
    //    #region DATA
    //    bool left, right, up, down;
    //    IFocous Target;
    //    float wheretoaim = 0;
    //    IFocous me;
    //    #endregion
    //    #region Ctors
    //    public BotKeys(IFocous target)
    //    {
    //        this.Target = target;
    //        wheretoaim = (Global.rnd.Next(100) - 50) / 50f;
    //        Game1.Event_Update += update;
    //    }
    //    #endregion
    //    #region OVERIDE FUNCS
    //    public override bool Left()
    //    {
    //        return left;
    //    }
    //    public override bool Up()
    //    {
    //        return up;
    //    }
    //    public override bool Down()
    //    {
    //        return down;
    //    }
    //    public override bool Right()
    //    {
    //        return right;
    //    }
    //    #endregion
    //    public override void WhoAmI(IFocous me)
    //    {
    //        this.me = me;
    //    }
    //    void update()
    //    {
    //        left = true;
    //        right = false;
    //        Vector2 distance = Target.Position - me.Position;
    //        if (distance.Length() < 100)
    //        {
    //            left = false;
    //            right = false;
    //            up = false;
    //            down = true;
    //            return;
    //        }

    //        double angle = Math.Atan2(distance.X, -distance.Y);

    //        left = false;
    //        right = false;

    //        float reallywheretoaim = wheretoaim;
    //        if (distance.Length() < 500)
    //        {
    //            reallywheretoaim = 0;
    //        }

    //        float angleDif = MathHelper.WrapAngle(mycar.Rotation -
    //            MathHelper.Pi / 2 - (float)angle + reallywheretoaim);

    //        if (Math.Abs(angleDif) > G.STEERSPEED)
    //        {
    //            if (angleDif >= 0) left = true;
    //            else right = true;
    //        }
    //    }
    //}
    //}
    #region botKeys
    class BotKeys : abstractKeys
    {
        #region DATA
        bool left = false, right = false, space = false;
        IFocous me;
        #endregion
        #region Ctors
        public BotKeys()
        {   
        }
        #endregion
        #region OVERIDE FUNCS
        public override bool Left()
        {
            return left;
        }
        
        public override bool Right()
        {
            return right;
        }
        public override bool Space()
        {
            return space;
        }
    public override void DefLeft(bool b) {
            this.left = b;
        }
    public override void DefRight(bool b) {
            this.right = b;
        }
    public override void DefSpace(bool b) {
            this.space = b;
        }
    public override void WhoAmI(IFocous me)
        {
            this.me = me;
        }
        #endregion
    }
    #endregion
}

