using PROG_225_ASSIGNMENT_6.PROG_225_ASSIGNMENT_6;

namespace PROG_225_ASSIGNMENT_6
{
    public partial class Form1 : Form
    {
        Director director;
        Uzi uzi;
        playerCharacter player_char;
        enemyCharacter enemy;
        Score score;
        HUD hud;

        static public Form1 mainForm;
        public delegate void SendKey(KeyEventArgs e);
        public delegate void SendStop(KeyEventArgs e);
        static public System.Windows.Forms.Timer graphicsTimer = new System.Windows.Forms.Timer();
        static public System.Windows.Forms.Timer temporalTimer = new System.Windows.Forms.Timer();
        static public System.Windows.Forms.Timer colorChangeTimer = new System.Windows.Forms.Timer();
        public event SendKey playerMove;
        public event SendStop playerStop;
        static public Random rand = new Random();
        static public int originalTempo = 8;
        static public int slowCounter = 0;
        Point random = new Point();
        private int colorChangeInterval = 10;
        private int colorChangeStep = 1;
        public bool timeSlowing = false;
        static public bool begin = false;
        static public bool gameOver = false;

        //debug console
        public static bool debug = true;

        public Form1()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            director = new Director();
            Director.mainForm = this;

            Graphics_Timer();
            graphicsTimer.Tick += GraphicsTimer_Tick;
            temporalTimer.Tick += TemporalTimer_Tick;

            Color_Change_Timer();
            colorChangeTimer.Tick += ColorChangeTimer_Tick;

            baseCharacter.mainForm = this;
            Bullet.mainForm = this;
            HUD.mainForm = this;

            score = new Score();
            hud = new HUD();

        }

        public void Spawn_Enemy()
        {
            Random_Location();
            enemy = new enemyCharacter(random.X, random.Y, player_char);
        }

        public void Spawn_Player()
        {
            Random_Location();
            player_char = new playerCharacter(random.X, random.Y);
        }
        
        public void Spawn_UZI()
        {
            Random_Location();
            uzi = new Uzi(random.X, random.Y);
        }

        private void Graphics_Timer()
        {
            graphicsTimer.Interval = 16;
            graphicsTimer.Start();
        }

        public void Game_Over()
        {
            gameOver = true;
        }

        private void GraphicsTimer_Tick(object? sender, EventArgs e)
        {
            Invalidate();
            foreach (Bullet bullet in Bullet.bullets.ToList())
            {
                bullet.Move();
            }
            foreach (enemyCharacter enemy in enemyCharacter.enemies.ToList())
            {
                enemy.Move();
            }
        }

        private void Color_Change_Timer()
        {
            colorChangeTimer = new System.Windows.Forms.Timer();
            colorChangeTimer.Interval = colorChangeInterval;
            colorChangeTimer.Tick += ColorChangeTimer_Tick;
        }

        private void ColorChangeTimer_Tick(object? sender, EventArgs e)
        {
            Color currentColor = this.BackColor;

            int newRed = Math.Min(currentColor.R + colorChangeStep, 255);
            int newGreen = Math.Max(currentColor.G - colorChangeStep, 0);
            int newBlue = Math.Max(currentColor.B - colorChangeStep, 0);

            this.BackColor = Color.FromArgb(newRed, newGreen, newBlue);

            if (newRed == 255)
            {
                colorChangeTimer.Stop();
            }
        }

        private void Slow_Form_Color()
        {
            colorChangeTimer.Start();
        }

        private void Default_Form_Color()
        {
            colorChangeTimer.Stop();
            this.BackColor = SystemColors.Control;
        }

        public void Resume_Time()
        {
            timeSlowing = false;
            slowCounter = 0;
            temporalTimer.Stop();
            graphicsTimer.Interval = originalTempo;
            Default_Form_Color();
        }

        public void Slow_Time()
        {
            timeSlowing = true;
            slowCounter += 8;
            Slow_Form_Color();
            temporalTimer.Start();
        }

        private void TemporalTimer_Tick(object? sender, EventArgs e)
        {
            var slowLimit = 80;

            if (graphicsTimer.Interval <= slowLimit)
            {
                graphicsTimer.Interval += 12;
            }
            else if (graphicsTimer.Interval >= slowLimit) { return; }
        }



        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DoubleBuffered = true;
             
            if (gameOver == true)
            {
                director.Game_Over(e.Graphics);
                Slow_Time();
            }

            if (begin == false && gameOver == false)
            {
                director.Title_Screen(e.Graphics);
            }
            else if (begin == true)
            {
                for (int i = 0; i < Controls.Count; i++)
                    if (Controls[i].GetType() == typeof(PictureBox))
                    {
                        var p = Controls[i] as PictureBox;
                        p.Visible = false;
                        e.Graphics.DrawImage(p.Image, p.Left, p.Top, p.Width, p.Height);
                    }

                player_char.Draw_Aiming(e.Graphics);

                foreach (Bullet bullet in Bullet.bullets.ToList())
                {
                    bullet.Draw(e.Graphics);
                }

                foreach (enemyCharacter enemy in enemyCharacter.enemies.ToList())
                {
                    enemy.Draw_Character(e.Graphics);
                }

                foreach (iPickup pickups in iPickup.pickups.ToList())
                {
                    pickups.Draw(e.Graphics);
                    pickups.Pickup(player_char);
                }

                player_char.Draw_Character(e.Graphics);

                score.Draw_Score(e.Graphics);

                hud.Draw_HUD(e.Graphics);
            }
            }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        { 
            if (begin == false)
            {
                begin = true;
                director.Start_Game();
            }
            if (gameOver == true)
            {
                return;
            }
            playerMove(e);
            Resume_Time();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (begin == false)
            {
                begin = true;
                director.Start_Game();
            }
            if (gameOver == true)
            {
                return;
            }
            playerStop(e);
            Resume_Time();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (begin == true) { player_char.Mouse_Aim(e); }
        }

        private Point Random_Location()
        {
            random.X = rand.Next(0, 1160);
            random.Y = rand.Next(0, 820);

            return random;
        }
    }
}