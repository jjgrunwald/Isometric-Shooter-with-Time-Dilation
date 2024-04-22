using PROG_225_ASSIGNMENT_6.PROG_225_ASSIGNMENT_6;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PROG_225_ASSIGNMENT_6
{
    public class enemyCharacter : baseCharacter
    {

        private int random;
        static public Random rd = new Random();
        public System.Windows.Forms.Timer AItimer = new System.Windows.Forms.Timer();
        static public List<enemyCharacter> enemies = new List<enemyCharacter>();
        private bool disposed = false;
        private playerCharacter player;
        private enemyCharacter thisEnemy;
        private Point playerXY;
        private bool playerInSight;
        private bool firingOnPlayer = false;
        private static int AIcounter;
        private DateTime lastShotTime;
        private const double shotCooldown = 128;
        private List<Point> trailPoints = new List<Point>();

        //Debug console for enemy characters.
        private bool debugConsole = false;

        public enemyCharacter(int _x, int _y, playerCharacter _player) : base(_x, _y)
        {
            this.player = _player;
            playerXY = new Point();
            enemies.Add(this);
            coneSpread = -280;
            thisEnemy = this;
            int random = rd.Next(0, 3);
            AItimer = new System.Windows.Forms.Timer();
            AItimer.Interval = 1000;
            AItimer.Tick += AItimer_Tick;
            AItimer.Start();
            AIcounter = 0;
            playerInSight = false;
            Find_Direction(this);
            Enemy_Sight(this);
            lastShotTime = DateTime.MinValue;
        }

        private void AItimer_Tick(object? sender, EventArgs e)
        {
            playerXY.X = player.X;
            playerXY.Y = player.Y;

            if (IsInPolygon(aimZone, (playerXY)))
            {
                playerInSight = true;
            }
            else if (IsInPolygon(aimZone, (playerXY)) == false)
            {
                playerInSight = false;
            }

            if (playerInSight == true || firingOnPlayer == false)
            {
                switch (currentFacing)
                {
                    case facing.NW:
                        gunPoint = new Point(x + 4, y);
                        Firing_NW();
                        break;
                    case facing.SW:
                        gunPoint = new Point(x, y + 26);
                        Stop_SW();
                        break;
                    case facing.NE:
                        gunPoint = new Point(x + 25, y);
                        Stop_NE();
                        break;
                    case facing.SE:
                        gunPoint = new Point(x + 28, y + 26);
                        Stop_SE();
                        break;
                }

            }
            else if (playerInSight == false || firingOnPlayer == true)
            {
                AIcounter++;
            }

            if (firingOnPlayer == false && playerInSight == false) { Find_Player(); }

            if (AIcounter == 60)
            {
                random = rd.Next(0, 3);
                AIcounter = 0; 
            }

            Enemy_Sight(this);
        }
        

        private void Find_Direction(enemyCharacter thisEnemy)
        {
            if (currentFacing == facing.NE)
            {
                random = 2;
            }
            if (currentFacing == facing.SE)
            {
                random = 3;
            }
            if (currentFacing == facing.SW)
            {
                random = 0;
            }
            if (currentFacing == facing.NW)
            {
                random = 1;
            }

            Enemy_Sight(this);
        }
        
        private void Find_Player()
        {
            Enemy_Sight(this);

            switch (currentFacing)
            {
                case facing.NW:
                    Stop_NW();
                    currentFacing = facing.NW;
                    gunPoint = new Point(x + 4, y);
                    break;
                case facing.SW:
                    Stop_SW();
                    currentFacing = facing.SW;
                    gunPoint = new Point(x, y + 26);
                    break;
                case facing.NE:
                    Stop_NE();
                    currentFacing = facing.NE;
                    gunPoint = new Point(x + 25, y);
                    break;
                case facing.SE:
                    Stop_SE();
                    currentFacing = facing.SE;
                    gunPoint = new Point(x + 28, y + 26);
                    break;

            }

        }

        public void Move()
        {
            if (firingOnPlayer == true)
            {

                var endPoint = EndPoint(firingAngle, playerXY);

                switch (currentFacing)
                {
                    case facing.NW:
                        gunPoint = new Point(x + 4, y);
                        Firing_NW();
                        if (playerInSight == true) { Fire(endPoint); };
                        break;
                    case facing.SW:
                        Firing_SW();
                        gunPoint = new Point(x, y + 26);
                        if (playerInSight == true) { Fire(endPoint); };
                        break;
                    case facing.NE:
                        Firing_NE();
                        gunPoint = new Point(x + 25, y);
                        if (playerInSight == true) { Fire(endPoint); };
                        break;
                    case facing.SE:
                        Firing_SE();
                        gunPoint = new Point(x + 28, y + 26);
                        if (playerInSight == true) { Fire(endPoint); };
                        break;
                }

            }
            if (playerInSight == true && firingOnPlayer == false)
            {

                switch (currentFacing)
                {
                    case facing.NW:
                        Stop_NW();
                        currentFacing = facing.NW;
                        gunPoint = new Point(x + 4, y);
                        break;
                    case facing.SW:
                        Stop_SW();
                        currentFacing = facing.SW;
                        gunPoint = new Point(x, y + 26);
                        break;
                    case facing.NE:
                        Stop_NE();
                        currentFacing = facing.NE;
                        gunPoint = new Point(x + 25, y);
                        break;
                    case facing.SE:
                        Stop_SE();
                        currentFacing = facing.SE;
                        gunPoint = new Point(x + 28, y + 26);
                        break;
                }
                Shoot_Player();
            }
            else if (playerInSight == false && firingOnPlayer == false) 
            {
                switch (random)
                {
                    case 0:
                        Move_SW();
                        currentFacing = facing.SW;
                        break;
                    case 1:
                        Move_NW();
                        currentFacing = facing.NW;
                        break;
                    case 2:
                        Move_NE();
                        currentFacing = facing.NE;
                        break;
                    case 3:
                        Move_SE();
                        currentFacing = facing.SE;
                        break;
                    default:
                        break;
                }

            }

            if (AItimer.Enabled == false && playerInSight == false)
            {
                AItimer.Start();
            }

            switch (currentFacing)
            {
                case facing.NW:
                    gunPoint = new Point(x + 4, y);
                    break;
                case facing.SW:
                    gunPoint = new Point(x, y + 26);
                    break;
                case facing.NE:
                    gunPoint = new Point(x + 25, y);
                    break;
                case facing.SE:
                    gunPoint = new Point(x + 28, y + 26);
                    break;
            }


            if (this.x <= 0)
            {
                this.currentFacing = facing.SE;
                random = 3;
                Move_SE();
                x += 20;
                AItimer.Enabled = false;
            }
            if (this.x >= 1160)
            {
                this.currentFacing = facing.SW;
                random = 0;
                Move_SW();
                x -= 20;
                AItimer.Enabled = false;
            }
            if (this.y <= 0)
            {
                this.currentFacing = facing.SW;
                random = 0;
                Move_SW();
                y += 20;
                AItimer.Enabled = false;
            }
            if (this.y >= 820)
            {
                this.currentFacing = facing.NE;
                random = 2;
                Move_NE();
                y -= 20;
                AItimer.Enabled = false;
            }

            Enemy_Sight(this);
        }


        public void Shoot_Player()
        {
            TimeSpan timeSinceLastShot = DateTime.Now - lastShotTime;

            int random = new Random().Next(0, 50);

            if (random == 25 && firingOnPlayer == false)
            {

                if (timeSinceLastShot.TotalSeconds >= shotCooldown)
                {

                    switch (currentFacing)
                    {
                        case facing.NW:
                            gunPoint = new Point(x + 4, y);
                            Firing_NW();
                            break;
                        case facing.SW:
                            gunPoint = new Point(x, y + 26);
                            Firing_SW();
                            break;
                        case facing.NE:
                            gunPoint = new Point(x + 25, y);
                            Firing_NE();
                            break;
                        case facing.SE:
                            gunPoint = new Point(x + 28, y + 26);
                            Firing_SE();
                            break;
                    }
                }
              
                    firingOnPlayer = true;
                    lastShotTime = DateTime.Now;
                
            }
            else if (random != 25) { random = new Random().Next(0, 50); }

        }


        public void Enemy_Sight(enemyCharacter thisEnemy)
        {
            if (aimZone != null) { Array.Clear(aimZone); }

            switch (currentFacing)
            {
                case facing.NE:
                    aimZone[0] = new Point(thisEnemy.x - coneSpread, 1);
                    aimZone[1] = new Point(1200, 1);
                    aimZone[2] = new Point(1200, thisEnemy.y - coneSpread);
                    break;
                case facing.SE:
                    aimZone[0] = new Point(thisEnemy.x - coneSpread, 1195);
                    aimZone[1] = new Point(1200, 900);
                    aimZone[2] = new Point(1200, thisEnemy.y + coneSpread);
                    break;
                case facing.SW:
                    aimZone[0] = new Point(1, thisEnemy.y + coneSpread);
                    aimZone[1] = new Point(1, 900);
                    aimZone[2] = new Point(thisEnemy.x + coneSpread, 1200);
                    break;
                case facing.NW:
                    aimZone[0] = new Point(1, thisEnemy.y - coneSpread);
                    aimZone[1] = new Point(1, 1);
                    aimZone[2] = new Point(thisEnemy.x + coneSpread, 1);
                    break;
            }
        }


        public void Destroy(enemyCharacter enemy)
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
                    AItimer.Tick -= AItimer_Tick;
                    firingOnPlayer = false;
                    playerInSight = false;
                    enemies.Remove(this);
                    picbx.Dispose();
                    AItimer.Stop();
                    AItimer.Dispose();
                }
            }

            disposed = true;

        }


        public override void Draw_Character(Graphics e)
        {
            if (mainForm.timeSlowing == true)
            {
               
                trailPoints.Add(new Point(x, y));

                for (int i = 1; i < trailPoints.Count; i++)
                {
                    Pen trailPen = new Pen(Color.FromArgb(50, Color.Red), 8);
                    e.DrawImage(picbx.Image, trailPoints[i - 1]);
                }

                if (trailPoints.Count > 25)
                {
                    trailPoints.RemoveAt(0);
                }


            }
            if (mainForm.timeSlowing == false)
            {
                trailPoints.Clear();
            }

                frameBuffer++;

                if (frameBuffer == 8)
                {
                    if (mainForm.timeSlowing != true) { frameIndex++; }
                    frameBuffer = 0;
                }

                if (frameIndex == 3) { frameIndex = 0; }


            Pen pen = new Pen(Color.Black, 3);

            if (debugConsole == true)
            {
                //Debug console, enable at the top of this class.
                e.DrawString("X: " + x.ToString(), font, brush, x + 20, y - 38);
                e.DrawString("Y: " + y.ToString(), font, brush, x + 20, y - 25);

                if (aimZone != null)
                {
                    e.DrawString("Player Seen:" + playerInSight.ToString(), font, brush, x + 20, y - 12);
                }

                Aiming_Zone(this);

                GraphicsPath path = new GraphicsPath();

                path.AddPolygon(new Point[] { gunPoint, aimZone[0], aimZone[1], aimZone[2] });
                e.DrawPath(pen, path);

            }


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
