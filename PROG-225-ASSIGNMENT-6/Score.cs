using PROG_225_ASSIGNMENT_6.PROG_225_ASSIGNMENT_6;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_225_ASSIGNMENT_6
{
    public class Score
    {
        static public int playerScore = 0;
        static public int counter = 0;
        static public Point dead;
        static public Point drawPoint = new Point(80, 20);
        static public SolidBrush redBrush = new SolidBrush(Color.Red);
        static public SolidBrush yellowBrush = new SolidBrush(Color.Goldenrod);
        static public SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(166, Color.Black));
        static public Font font = new Font(FontFamily.GenericSansSerif, 33, FontStyle.Bold);
        static public Font fontSmall = new Font(FontFamily.GenericSansSerif, 18, FontStyle.Bold);
        static public System.Windows.Forms.Timer playerScoreTimer = new System.Windows.Forms.Timer();

        public Score()
        {
            Bullet.scoreDisplay = this;
            playerScoreTimer = new System.Windows.Forms.Timer();
            playerScoreTimer.Interval = 200;
            playerScoreTimer.Tick += PlayerScore_Tick;
        }

        private void PlayerScore_Tick(object? sender, EventArgs e)
        {
            dead.Y-=10;
            counter++;
            if (counter == 10)
            {
                playerScoreTimer.Stop();
                playerScoreTimer.Enabled = false;
                counter = 0;
            }
        }

        public void Kill_Score(enemyCharacter enemy)
        {
            playerScoreTimer.Enabled = true;
            playerScoreTimer.Start();
            playerScore += 100;
            dead.X = enemy.X;
            dead.Y = enemy.Y;

        }

        public void Draw_Score(Graphics e)
        {
    
            e.DrawString(playerScore.ToString(), font, shadowBrush, drawPoint.X + 5, drawPoint.Y + 5);
            e.DrawString(playerScore.ToString(), font, redBrush, drawPoint);

            if (playerScoreTimer.Enabled)
            {
                e.DrawString("+100", fontSmall, redBrush, dead);
                //Shadow effect and flashing, commented out because I didn't like the way it looked.
/*                e.DrawString("+100", fontSmall, shadowBrush, dead.X +5, dead.Y +5);
                if (counter % 2 == 0)
                {
                    e.DrawString("+100", fontSmall, yellowBrush, dead);
*//*                    e.DrawString("+100", fontSmall, shadowBrush, dead.X + 5, dead.Y + 5);*//*
                }*/
            }
        }
    }
}
