using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Imaging.Filters;
namespace ImageEditor
{
    class Tools
    {
        public Bitmap Mirror_X(Bitmap img)
        {
            Mirror mirror = new Mirror(true,true);
            return mirror.Apply(img);
        }
        public Bitmap Mirror_Y(Bitmap img)
        {
            Mirror mirror = new Mirror(false,true);
            return mirror.Apply(img);
        }
        public Bitmap Rotate(Bitmap img)
        {
            RotateBilinear rotate = new RotateBilinear(90, true);
            return rotate.Apply(img);
        }
        public Bitmap DeleteSection(Bitmap img)
        {
            return null;
        }

        public Bitmap ErasePart(Bitmap img, Rectangle rect)
        {
            Bitmap bm = new Bitmap(img);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                GraphicsPath path = new GraphicsPath();
                path.AddRectangle(rect);
                gr.SetClip(path);
                gr.Clear(Color.Transparent);
                gr.ResetClip();
            }
            return bm;
        }
    }
}
