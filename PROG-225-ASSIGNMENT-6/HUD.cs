namespace PROG_225_ASSIGNMENT_6
{
    public class HUD
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern short GetKeyState(int keyCode);
        public const int KeyPressed = 0x8000;
        static public Form1 mainForm;
        static public playerCharacter player;
        static public SolidBrush blackBrush = new SolidBrush(Color.Black);
        static public SolidBrush redBrush = new SolidBrush(Color.Red);
        static public Font font = new Font(FontFamily.GenericMonospace, 11);
        static public Pen blackPen = new Pen(Color.Black, 2);
        static public Pen redPen = new Pen(Color.Red, 2);
        static public Rectangle NE;
        static public Rectangle SE;
        static public Rectangle SW;
        static public Rectangle NW;
        static public int dirRectSize = 25;
        static public bool movingNE = false;
        static public bool movingSE = false;
        static public bool movingSW = false;
        static public bool movingNW = false;



        public HUD()
        {
            int keys_x = mainForm.Width - 150;
            int keys_y = mainForm.Bottom - 150;

            //S KEY
            SW = new Rectangle();
            SW.X = keys_x - 30;
            SW.Y = keys_y;
            SW.Width = dirRectSize;
            SW.Height = dirRectSize;

            //D KEY
            SE = new Rectangle();
            SE.X = keys_x;
            SE.Y = keys_y;
            SE.Width = dirRectSize;
            SE.Height = dirRectSize;

            //A KEY
            NW = new Rectangle();
            NW.X = keys_x - 60;
            NW.Y = keys_y;
            NW.Width = dirRectSize;
            NW.Height = dirRectSize;

            //W KEY
            NE = new Rectangle();
            NE.X = keys_x - 30; ;
            NE.Y = keys_y - 30;
            NE.Width = dirRectSize;
            NE.Height = dirRectSize;
        }



            public void Draw_HUD(Graphics e)
        {
            int x_ne = 1050;
            int y_ne = 710;

            Point[] NE_arrow = {
                                    new Point(x_ne, y_ne),
                                    new Point(x_ne + 40, y_ne),
                                    new Point(x_ne + 40, y_ne + 40),
                                    new Point(x_ne, y_ne)};


            int x_se = 1050;
            int y_se = 770;

            Point[] SE_arrow ={
                        new Point(x_se+40, y_se),
                        new Point(x_se+40, y_se+40),
                        new Point(x_se, y_se+40),
                        new Point(x_se+40, y_se)};

            int x_sw = 970;
            int y_sw = 770;

            Point[] SW_arrow ={
                        new Point(x_sw, y_sw),
                        new Point(x_sw+40, y_sw+40),
                        new Point(x_sw, y_sw+40),
                        new Point(x_sw, y_sw)};

            int x_nw = 970;
            int y_nw = 710;

            Point[] NW_arrow ={
                        new Point(x_nw, y_nw),
                        new Point(x_nw+40, y_nw),
                        new Point(x_nw, y_nw+40),
                        new Point(x_nw, y_nw)};


            if (IsKeyDown(Keys.S))
            {
                e.FillRectangle(redBrush, SW);
                e.DrawRectangle(blackPen, SW);
                e.FillPolygon(redBrush, SW_arrow);
                e.DrawPolygon(blackPen, SW_arrow);
            }
            else
            {
                e.DrawRectangle(blackPen, SW);
                e.DrawPolygon(blackPen, SW_arrow);
            }

            if (IsKeyDown(Keys.D))
            {
                e.FillRectangle(redBrush, SE);
                e.DrawRectangle(blackPen, SE);
                e.FillPolygon(redBrush, SE_arrow);
                e.DrawPolygon(blackPen, SE_arrow);
            }
            else
            {
                e.DrawRectangle(blackPen, SE);
                e.DrawPolygon(blackPen, SE_arrow);
            }

            if (IsKeyDown(Keys.A))
            {
                e.FillRectangle(redBrush, NW);
                e.DrawRectangle(blackPen, NW);
                e.FillPolygon(redBrush, NW_arrow);
                e.DrawPolygon(blackPen, NW_arrow);
            }
            else
            {
                e.DrawRectangle(blackPen, NW);
                e.DrawPolygon(blackPen, NW_arrow);
            }

            if (IsKeyDown(Keys.W))
            {
                e.FillRectangle(redBrush, NE);
                e.DrawRectangle(blackPen, NE);
                e.FillPolygon(redBrush, NE_arrow);
                e.DrawPolygon(blackPen, NE_arrow);
            }
            else
            {
                e.DrawRectangle(blackPen, NE);
                e.DrawPolygon(blackPen, NE_arrow);
            }
        }

        private bool IsKeyDown(Keys key)
        {
            return (GetKeyState((int)key) & KeyPressed) == KeyPressed;
        }
    }
}
