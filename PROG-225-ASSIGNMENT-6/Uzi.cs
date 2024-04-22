using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PROG_225_ASSIGNMENT_6
{
    public class Uzi : iPickup
    {
        public string Name { get; set; }

        public int x { get; set; }

        public int y { get; set; }

        public Image Image { get; set; }

        public bool disposed { get; set; }

        public Uzi(int _x, int _y)
        {
            iPickup.pickups.Add(this);
            Load_Image();
            x = _x;
            y = _y;
            disposed = false;
        }

            public void Load_Image()
        {
            Image = new Bitmap("../../../assets/uzi.gif");
        }

        public void Effect(playerCharacter player_char)
        {
/*            playerCharacter.coneSpread = -280;*/
            playerCharacter.fireTimer.Interval = 100;
        }


        public void Draw(Graphics e)
        {
            e.DrawImage(Image, x, y);
        }
    }
}
