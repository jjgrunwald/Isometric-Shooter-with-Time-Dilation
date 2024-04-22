using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PROG_225_ASSIGNMENT_6.baseCharacter;

namespace PROG_225_ASSIGNMENT_6
{
    public class Director
    {
        static public Form1 mainForm;

        private static playerCharacter player;

        static public System.Windows.Forms.Timer directorTimer = new System.Windows.Forms.Timer();
        static public Random rd = new Random();
        public Bitmap title = new Bitmap("../../../assets/titlescreen.png");
        public Bitmap controls = new Bitmap("../../../assets/controls.png");
        static public Font font = new Font(FontFamily.GenericSansSerif, 44, FontStyle.Bold);
        static public SolidBrush redBrush = new SolidBrush(Color.Red);
        static public int level = 1;

        public Director()
        {
            directorTimer = new System.Windows.Forms.Timer();
            directorTimer.Interval = 1000;
            directorTimer.Tick += DirectorTimer_Tick;
            directorTimer.Start();
        }

        private void DirectorTimer_Tick(object? sender, EventArgs e)
        {
            SpawnEnemyBasedOnScore(100, 10);
            SpawnEnemyBasedOnScore(500, 5);
            SpawnEnemyBasedOnScore(1000, 3);
            SpawnUZI();
        }

        private void SpawnEnemyBasedOnScore(int scoreThreshold, int maxProbability)
        {
            if (Score.playerScore >= scoreThreshold)
            {
                int random = new Random().Next(0, maxProbability);

                if (random == 2)
                {
                    mainForm.Spawn_Enemy();
                }
            }
        }

        private void SpawnUZI()
        {
            int random = new Random().Next(0, 100);

            if (random == 50)
            {
                mainForm.Spawn_UZI();
            }
        }

        public void Start_Game()
        {
            mainForm.Spawn_Player();
            mainForm.Spawn_Enemy();
        }
        
        public void Title_Screen(Graphics e)
        {
            e.DrawImage(title, 20, -20, 1120, 900);
            e.DrawImage(controls, 5, 20, 1200, 800);
        }

        public void Game_Over(Graphics e)
        {
            e.DrawImage(title, 20, -20, 1120, 900);
            e.DrawString("GAME OVER", font, redBrush, 400, 400);
        }

    }
}
