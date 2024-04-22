using PROG_225_ASSIGNMENT_6.PROG_225_ASSIGNMENT_6;
using PROG_225_ASSIGNMENT_6.Properties;
using System.Drawing.Imaging;

namespace PROG_225_ASSIGNMENT_6
{
    public class baseCharacter
    {
        static public Form1 mainForm;

        public Image[] sprite;

        public Image staticSprite;

        static public System.Windows.Forms.Timer fireTimer = new System.Windows.Forms.Timer();

        //North East
        public Bitmap NE_standing = new Bitmap("../../../assets/NE0.gif");
        public Bitmap NE_running = new Bitmap("../../../assets/NE1.gif");
        public Bitmap NE_firing = new Bitmap("../../../assets/NE2.gif");

        //South East
        public Bitmap SE_standing = new Bitmap("../../../assets/SE0.gif");
        public Bitmap SE_running = new Bitmap("../../../assets/SE1.gif");
        public Bitmap SE_firing = new Bitmap("../../../assets/SE2.gif");

        //South West
        public Bitmap SW_standing = new Bitmap("../../../assets/SW0.gif");
        public Bitmap SW_running = new Bitmap("../../../assets/SW1.gif");
        public Bitmap SW_firing = new Bitmap("../../../assets/SW2.gif");

        //North West
        public Bitmap NW_standing = new Bitmap("../../../assets/NW0.gif");
        public Bitmap NW_running = new Bitmap("../../../assets/NW1.gif");
        public Bitmap NW_firing = new Bitmap("../../../assets/NW2.gif");

        //Fields
        public int x;
        public int y;
        public PictureBox picbx;
        public Point[] aimZone = new Point[3];
        protected Rectangle hitbx;
        protected bool animating = false;
        protected bool aiming = false;
        protected facing currentFacing = facing.SE;
        protected Point gunPoint;
        protected int frameIndex = 0;
        protected int frameBuffer = 0;
        public int coneSpread = -120;
        public float firingAngle;
        static public SolidBrush brush = new SolidBrush(Color.Black);
        static public Font font = new Font(FontFamily.GenericMonospace, 11);


        //Constructors
        public baseCharacter(int _x, int _y)
        {
            x = _x;
            y = _y;
            baseCharacter thisCharacter = this;
            picbx = new PictureBox();
            staticSprite = SE_standing;
            picbx.Image = staticSprite;
            picbx.Size = new Size(30, 50);
            picbx.Location = new Point(x, y);
            mainForm.Controls.Add(picbx);
            fireTimer = new System.Windows.Forms.Timer();
            fireTimer.Interval = 500;
            aimZone = new Point[3];
            gunPoint = new Point();
            fireTimer.Tick += FireTimer_Tick;
            NEarray = GetFramesFromAnimatedGIF(Resources.NE1);
            SEarray = GetFramesFromAnimatedGIF(Resources.SE1);
            SWarray = GetFramesFromAnimatedGIF(Resources.SW1);
            NWarray = GetFramesFromAnimatedGIF(Resources.NW1);

        }

        public enum facing
        {
            NE = 0,
            SE = 1,
            SW = 2,
            NW = 3,
        }

        public bool movingNE = false;
        public bool movingSE = false;
        public bool movingSW = false;
        public bool movingNW = false;


        private void FireTimer_Tick(object? sender, EventArgs e)
        {
            fireTimer.Stop();
        }

        //Properties
        public int X { get { return x; } }
        public int Y { get { return y; } }

        //Methods
        private Image[] NEarray, SEarray, SWarray, NWarray;

        //Borrowed from stack overflow+Thomas
        public static Image[] GetFramesFromAnimatedGIF(Image IMG)
        {
            List<Image> IMGs = new List<Image>();
            int Length = IMG.GetFrameCount(FrameDimension.Time);

            for (int i = 0; i < Length; i++)
            {
                IMG.SelectActiveFrame(FrameDimension.Time, i);
                IMGs.Add(new Bitmap(IMG));
            }

            return IMGs.ToArray();
        }

        public void Character_Aim()
        {
            aiming = true;

            facing aimingDirection = currentFacing;

            switch (aimingDirection)
            {
                case facing.NE:
                    Firing_NE();
                    gunPoint = new Point(x + 25, y);
                    break;
                case facing.SE:
                    Firing_SE();
                    gunPoint = new Point(x + 28, y + 26);
                    break;
                case facing.SW:
                    Firing_SW();
                    gunPoint = new Point(x, y+26);
                    break;
                case facing.NW:
                    Firing_NW();
                    gunPoint = new Point(x + 4, y);
                    break;
            }
        }


        public static bool IsInPolygon(Point[] aimZone, Point checkPoint)
        {
            bool result = false;
            int j = aimZone.Length - 1;
            for (int i = 0; i < aimZone.Length; i++)
            {
                if (aimZone[i].Y < checkPoint.Y && aimZone[j].Y >= checkPoint.Y ||
                    aimZone[j].Y < checkPoint.Y && aimZone[i].Y >= checkPoint.Y)
                {
                    if ((aimZone[i].X + (checkPoint.Y - aimZone[i].Y) /
                       (aimZone[j].Y - aimZone[i].Y) *
                       (aimZone[j].X - aimZone[i].X) < checkPoint.X))
                    {
                        //result = !result;
                        result = true;
                    }
                }
                j = i;
            }
            return result;
        }

        //Movement methods
        public void Move_NE()
        {
            x++;
            y--;
                aiming = false;
                sprite = NEarray;
                picbx.Image = NEarray[frameIndex];
        }

        public void Move_SE()
        {
            x++;
            y++;
                aiming = false;
                sprite = SEarray;
                picbx.Image = SEarray[frameIndex];
        }

        public void Move_SW()
        {
            x--;
            y++;
                aiming = false;
                sprite = SWarray;
                picbx.Image = SWarray[frameIndex];
        }

        public void Move_NW()
        {
            x--;
            y--;
                aiming = false;
                sprite = NWarray;
                picbx.Image = NWarray[frameIndex];
        }

        //Stop animation methods
        public void Stop_NE()
        {
                animating = false;
                staticSprite = NE_standing;
                picbx.Image = staticSprite;
        }

        public void Stop_SE()
        {
                animating = false;
                staticSprite = SE_standing;
                picbx.Image = staticSprite;
        }

        public void Stop_SW()
        {
                animating = false;
                staticSprite = SW_standing;
                picbx.Image = staticSprite;
        }

        public void Stop_NW()
        {
                animating = false;
                staticSprite = NW_standing;
                picbx.Image = staticSprite;
        }


        //Firing animation methods
        public void Firing_NE()
        {
            animating = false;
            staticSprite = NE_firing;
            picbx.Image = staticSprite;
        }

        public void Firing_SE()
        {
            animating = false;
            staticSprite = SE_firing;
            picbx.Image = staticSprite;
        }

        public void Firing_SW()
        {
            animating = false;
            staticSprite = SW_firing;
            picbx.Image = staticSprite;
        }

        public void Firing_NW()
        {
            animating = false;
            staticSprite = NW_firing;
            picbx.Image = staticSprite;
        }



        public virtual void Aiming_Zone(baseCharacter thisCharacter)
        {
            if (aimZone != null) { Array.Clear(aimZone); }

            switch (currentFacing)
            {
                case facing.NE:
                    aimZone[0] = new Point(thisCharacter.x - coneSpread, 1);
                    aimZone[1] = new Point(1200, 1);
                    aimZone[2] = new Point(1200, thisCharacter.y - coneSpread);
                    break;
                case facing.SE:
                    aimZone[0] = new Point(thisCharacter.x - coneSpread, 1195);
                    aimZone[1] = new Point(1200, 900);
                    aimZone[2] = new Point(1200, thisCharacter.y + coneSpread);
                    break;
                case facing.SW:
                    aimZone[0] = new Point(1, thisCharacter.y + coneSpread);
                    aimZone[1] = new Point(1, 900);
                    aimZone[2] = new Point(thisCharacter.x + coneSpread, 1200);
                    break;
                case facing.NW:
                    aimZone[0] = new Point(1, thisCharacter.y - coneSpread);
                    aimZone[1] = new Point(1, 1);
                    aimZone[2] = new Point(thisCharacter.x + coneSpread, 1);
                    break;
            }
        }


        public void Fire(Point aimPoint)
        {
            if (!fireTimer.Enabled)
            {
                if (currentFacing == facing.NE || currentFacing == facing.SE)
                {
                    new Bullet(gunPoint.X, gunPoint.Y, aimPoint, 0);
                }
                if (currentFacing == facing.SW || currentFacing == facing.NW)
                {
                    new Bullet(gunPoint.X-25, gunPoint.Y-25, aimPoint, 1);
                }
                fireTimer.Start();
            }
        }

        public Point EndPoint(float angle, Point endPoint)
        {

            float angleBetweenPoints = (float)Math.Atan2(endPoint.Y - y, endPoint.X - x);

            float newAngle = angle + (angleBetweenPoints - angle);

            endPoint.X = x + (int)(Math.Cos(newAngle) * 4000);
            endPoint.Y = y + (int)(Math.Sin(newAngle) * 4000);

            return new Point(endPoint.X, endPoint.Y);
        }



        public virtual void Draw_Character(Graphics e)
        {
            frameBuffer++;

            if (frameBuffer == 8)
            {
                frameIndex++;
                frameBuffer = 0;
            }

            if (frameIndex == 3) { frameIndex = 0; }

            for (int i = 0; i < mainForm.Controls.Count; i++)
                if (mainForm.Controls[i].GetType() == typeof(PictureBox))
                {
                    var p = mainForm.Controls[i] as PictureBox;
                    p.Visible = false;
                    e.DrawImage(p.Image, p.Left, p.Top, p.Width, p.Height);
                }

            picbx.Location = new Point(x, y);
        }


    }

}
