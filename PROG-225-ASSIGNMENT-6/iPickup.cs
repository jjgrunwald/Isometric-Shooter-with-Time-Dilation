using PROG_225_ASSIGNMENT_6.PROG_225_ASSIGNMENT_6;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROG_225_ASSIGNMENT_6
{

    public interface iPickup
    {

        public static List<iPickup> pickups = new List<iPickup>();

        public int x { get; set; }

        public int y { get; set; }

        public string Name { get; set; }

        public Image Image { get; set; }

        public bool disposed { get; set; }

        public void Load_Image();

        public void Effect(playerCharacter player_char);

        public void Pickup(playerCharacter player_char)
        {
            foreach (iPickup pickup in pickups.ToList())
            {
                if (Check_Pickup(player_char))
                {
                    Effect(player_char);
                    Dispose();
                }
            }
        }

        public bool Check_Pickup(playerCharacter player_char)
        {
            Rectangle pickupRect = new Rectangle(x, y, 4, 4);
            Rectangle playerRect = new Rectangle(player_char.X, player_char.Y, 20, 50);

            return pickupRect.IntersectsWith(playerRect);
        }

        public void Draw(Graphics e);

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
                    pickups.Remove(this);

                }
            }

            disposed = true;

        }
    }
}
