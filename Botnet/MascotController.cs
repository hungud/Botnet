using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Botnet
{   
    class MascotController
    {
        public enum states : int
        {
            salutation = 3,
            attacking  = 0 ,
            preparing = 2,
            pause = 1 
        }
        //counters
        int l = 0;
        int r = 0;
        private int state;
        private Timer _Timer= new Timer();
        private Timer Changer = new Timer();
        PictureBox Guy;
        Random Randomizer;
        public MascotController(ref PictureBox image)
        {
            Changer.Interval = 4000;
            Changer.Tick += new EventHandler(ChangePic);
            Guy = image;
            Randomizer = new Random();
            Guy.Image = Properties.Resources._3__3_;
            Changer.Start();
        }
        public void setState(int state)
        {
            // 3 - salutation
            // 2 - preparing
            // 0 - attacking
            // 1 - pause
            this.state = state;
        }
        private void ChangePic(object sender, EventArgs e)
        {
            switch (state)
            {
                case 0: Guy.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("_0__" + Randomizer.Next(1,56).ToString() + "_"); break;
                case 1: Guy.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("_1__" + Randomizer.Next(1,4).ToString() + "_"); break;
                case 2: Guy.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("_2__4_"); break;
                case 3: Guy.Image = (System.Drawing.Image)Properties.Resources.ResourceManager.GetObject("_3__" + Randomizer.Next(1,7).ToString() + "_"); break;
                default: break ;
            }
        }
        public void Counters(bool point)
        {
            if (point)
            {
                l++;
            }
            else
            {
                r++;
            }
            if (l == 3 && r == 4)
            {
                Guy.Image = Properties.Resources.dick;
            } 
            if (l > 40) l = 0;
            if (r > 40) r = 0;
        }
    }
}
