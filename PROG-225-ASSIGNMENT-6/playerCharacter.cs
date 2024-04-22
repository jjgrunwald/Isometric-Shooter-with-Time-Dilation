using System.Drawing.Drawing2D;

namespace PROG_225_ASSIGNMENT_6
{
    public class playerCharacter : baseCharacter
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern short GetKeyState(int keyCode);
        static public playerCharacter player;
        private bool disposed = false;
        public const int KeyPressed = 0x8000;
        public bool isMoving = false;
        private Point mousePoint;

        //Debug console can be enabled for player information
        private bool debugConsole = false;

        public playerCharacter(int _x, int _y) : base(_x, _y)
        {
            HUD.player = this;
            player = this;
            mainForm.playerMove += MainForm_playerMove;
            mainForm.playerStop += MainForm_playerStop;
        }


        private void MainForm_playerStop(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W && movingNE == true)
            {
                Stop_NE();
                currentFacing = facing.NE;
            }

            if (e.KeyCode == Keys.D && movingSE == true)
            {
                Stop_SE();
                currentFacing = facing.SE;
            }

            if (e.KeyCode == Keys.S && movingSW == true)
            {
                Stop_SW();
                currentFacing = facing.SW;
            }

            if (e.KeyCode == Keys.A && movingNW == true)
            {
                Stop_NW();
                currentFacing = facing.NW;
            }
        }

        private void MainForm_playerMove(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                IsKeyDown(Keys.Space);
                if (aiming == false) { Character_Aim(); }
                else if (aiming == true) 
                {
                    aiming = false;
                    switch (currentFacing)
                    {
                        case facing.NW:
                            Stop_NW();
                            currentFacing = facing.NW;
                            break;
                        case facing.SW:
                            Stop_SW();
                            currentFacing = facing.SW;
                            break;
                        case facing.NE:
                            Stop_NE();
                            currentFacing = facing.NE;
                            break;
                        case facing.SE:
                            Stop_SE();
                            currentFacing = facing.SE;
                            break;
                    }
                }
            }

            var keyStates = new System.Collections.BitArray(new bool[]

        {IsKeyDown(Keys.A), IsKeyDown(Keys.W), IsKeyDown(Keys.D), IsKeyDown(Keys.S)});

            var keyCombination = new byte[1];

            keyStates.CopyTo(keyCombination, 0);

            if (e.KeyCode == Keys.A)
            {
                IsKeyDown(Keys.A);
                mainForm.Resume_Time();
                movingNW = true;
            }

            if (e.KeyCode == Keys.W)
            {
                IsKeyDown(Keys.W);
                mainForm.Resume_Time();
                movingNE = true;
            }

            if (e.KeyCode == Keys.D)
            {
                IsKeyDown(Keys.D);
                mainForm.Resume_Time();
                movingSE = true;
            }

            if (e.KeyCode == Keys.S)
            {
                IsKeyDown(Keys.S);
                mainForm.Resume_Time();
                movingSW = true;
            }

            switch (keyCombination[0])
            {
                case 1: //'A' KEY
                    Move_NW();
                    break;
                case 2: //'W' KEY
                    Move_NE();
                    break;
                case 4: // 'D' KEY
                    Move_SE();
                    break;
                case 8: // 'S' KEY
                    Move_SW();
                    break;
                default:
                    break;
            }
        }

        private static bool IsKeyDown(Keys key)
        {
            return Convert.ToBoolean(GetKeyState((int)key) & KeyPressed);
        }

        public void Draw_Aiming(Graphics e)
        {


            if (aiming)
            {
                mainForm.Slow_Time();

                Aiming_Zone(this);

                GraphicsPath path = new GraphicsPath();

                path.AddPolygon(new Point[] { gunPoint, aimZone[0], aimZone[1], aimZone[2] });

                // Fill triangle
                Brush brush = new SolidBrush(Color.IndianRed);
                e.FillPath(brush, path);

                // Draw triangle outline
                Pen pen = new Pen(Color.Black, 2);
                e.DrawPath(pen, path);


                var endPoint = EndPoint(firingAngle, mousePoint);

                if (path.IsVisible(mousePoint.X, mousePoint.Y))
                {

                    var aimLine = new Pen(Color.Red, 1);

                    e.DrawLine(aimLine, gunPoint, endPoint);

                    aimLine.Dispose();

                }

                brush.Dispose();
                pen.Dispose();
                path.Dispose();

            }

        }



        public void Mouse_Aim(MouseEventArgs e)
        {
            if (aiming)
            {
                mousePoint = e.Location;


                if (e.Button == MouseButtons.Left)
                {

                    if (IsInPolygon(aimZone, mousePoint) == true)
                    {

                        var endPoint = EndPoint(firingAngle, mousePoint);
                        mainForm.Slow_Time();
                        Fire(endPoint);
                    }
                }
            }
        }


        public override void Draw_Character(Graphics e)
        {
            Pen pen = new Pen(Color.Red, 3);

            if (debugConsole == true)
            {
                //Debug console, enable at the top of this class.
                e.DrawString("Player X: " + x.ToString(), font, brush, 180, 30);
                e.DrawString("Player Y: " + y.ToString(), font, brush, 180, 50);
                e.DrawString("Player Aiming:" + aiming.ToString(), font, brush, 360, 30);
                e.DrawString("Player Aiming X:" + mousePoint.X, font, brush, 570, 30);
                e.DrawString("Player Aiming Y:" + mousePoint.Y, font, brush, 570, 50);
                if (aimZone != null)
                {
                    e.DrawString("Aiming Point 1:" + aimZone[0], font, brush, 770, 30);
                    e.DrawString("Aiming Point 2:" + aimZone[1], font, brush, 770, 50);
                    e.DrawString("Aiming Point 3:" + aimZone[2], font, brush, 770, 70);
                    e.DrawString("Mouse in Aim Zone:"+IsInPolygon(aimZone, mousePoint).ToString(), font, brush, 770, 90);
                }
            }

            e.DrawEllipse(pen, x, y + 34, 28, 18);

            frameBuffer++;

            if (frameBuffer == 12)
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



        public void Destroy(playerCharacter player)
        {
            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    picbx.Dispose();
                    x = -100;
                    y = -100;
                    mainForm.Game_Over();
                }
            }

            disposed = true;

        }

    }
}