namespace PROG_225_ASSIGNMENT_6
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    namespace PROG_225_ASSIGNMENT_6
    {

        public class Bullet
        {
            private int x;
            private int y;
            private int bulletSpeed;
            private int bulletDirection;
            private int bulletHealth;
            private Point destination;
            private bool disposed = false;
            static public System.Windows.Forms.Timer bulletLife = new System.Windows.Forms.Timer();
            static public Form1 mainForm;
            static public Score scoreDisplay;
            static public List<Bullet> bullets = new List<Bullet>();
            private List<Point> trailPoints = new List<Point>();
            private static playerCharacter player;

            public Bullet(int _startX, int _startY, Point _destinationPoint, int _bulletDirection)
            {
                player = playerCharacter.player;
                x = _startX;
                y = _startY;
                bulletSpeed = 1;
                destination = _destinationPoint;
                bulletDirection = _bulletDirection;
                bulletLife = new System.Windows.Forms.Timer();
                bulletLife.Interval = 10000;
                bullets.Add(this);
                bulletLife.Tick += BulletLife_Tick;
                bulletLife.Start();
            }

            private void BulletLife_Tick(object? sender, EventArgs e)
            {
                Dispose();
            }

            public void Move()
            {
                bulletHealth++;

                foreach (enemyCharacter enemy in enemyCharacter.enemies.ToList())
                {
                    if (CheckCollision(enemy))
                    {
                        Dispose();
                        scoreDisplay.Kill_Score(enemy);
                        enemy.Dispose();
                        return; 
                    }
                }

                if (CheckCollision(player))
                {
                    player.Dispose();

                }

                double deltaX = destination.X - x;
                double deltaY = destination.Y - y;
                double speedX = (deltaX / 100) * bulletSpeed;
                double speedY = (deltaY / 100) * bulletSpeed;

                x += (int)speedX;
                y += (int)speedY;

                if (Math.Sign(speedX) == Math.Sign(deltaX) && Math.Sign(speedY) == Math.Sign(deltaY))
                {
                    if (x < 0 || x > 1200 || y < 0 || y > 900)
                    {
                        Dispose();
                    }
                }

                if (bulletHealth == 1000)
                {
                    Dispose();
                }
            }

            public void Destroy(Bullet bullet)
            {
                GC.SuppressFinalize(this);
            }

            public void Dispose()
            {
                Dispose(true);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (!disposed)
                {
                    if (disposing)
                    {
                        bullets.Remove(this);
                    }
                }

                disposed = true;

            }

            public bool CheckCollision(baseCharacter character)
            {

                Rectangle bulletRect = new Rectangle(x, y, 4, 4);
                Rectangle characterRect = new Rectangle(character.X, character.Y, 20, 50);

                return bulletRect.IntersectsWith(characterRect);
            }


            public void Draw(Graphics e)
            {

                if (mainForm.timeSlowing == true)
                {
                    trailPoints.Add(new Point(x, y));

                    for (int i = 1; i < trailPoints.Count; i++)
                    {
                        Pen trailPen = new Pen(Color.FromArgb(60, Color.Red), 2);
                        e.DrawLine(trailPen, trailPoints[i - 1], trailPoints[i]);
                    }

                    if (trailPoints.Count > 25)
                    {
                        trailPoints.RemoveAt(0);
                    }
                }

                e.FillRectangle(Brushes.Black, x, y, 4, 4);
            }
        }

    }

}
