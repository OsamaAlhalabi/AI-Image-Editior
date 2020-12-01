using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageEditor
{
    class Layers
    {
        private List<Bitmap> editionLayers;
        private List<Bitmap> userLayers;
        private int iterator;
        private Filters filters;
        public Layers()
        {
            editionLayers = new List<Bitmap>();
            userLayers = new List<Bitmap>();
            iterator = 0; // starting position .. the main photo
            filters = new Filters();
        }
        public void addLayer(Bitmap newEdition)
        {
            editionLayers.Add(newEdition);
        }
        public void addUserLayer(Image newEdition)
        {
            userLayers.Add((Bitmap)newEdition);
        }
        public List<Bitmap> getAllHistoryLayers()
        {
            return this.editionLayers;
        }
        public List<Bitmap> getAllUserLayers()
        {
            return this.userLayers;
        }
        public int getLayersNum()
        {
            return editionLayers.Count;
        }
        public int getUserLayersNum()
        {
            return userLayers.Count;
        }
        public Bitmap getLastEdition()
        {
            return editionLayers[editionLayers.Count - 1];
        }
        public Bitmap getUserEdit(int index)
        {
            if (index < userLayers.Count)
                return userLayers[index];
            else
                return null ;
        }
        public Bitmap Traverse()
        {
            iterator++;
            if (iterator == userLayers.Count)
                iterator = 0;
            return userLayers[iterator];
        }
        public Bitmap GetEdition()
        {
            return editionLayers[iterator];
        }
        public Bitmap Retreat()
        {
            if (editionLayers.Count > 1)
            {
                editionLayers.RemoveAt(editionLayers.Count - 1);
                return editionLayers[editionLayers.Count - 1];
            }
            else
                return editionLayers[0];
        }

        //apply filter on the layer directly.
        public Bitmap AddEdition(Bitmap filterRes)
        {
            addLayer(filterRes);
            return filterRes;
        }
        public Bitmap DisplayLayers(Bitmap myBitMap)
        {
            List<Bitmap> images = new List<Bitmap>();
            Bitmap finalImage = new Bitmap(myBitMap);
            for (int i = 0; i < getUserLayersNum(); ++i)
            {
                Bitmap bitmap = new Bitmap(getUserEdit(i));
                bitmap.MakeTransparent(Color.Transparent);
                if(i!=0)
                   bitmap = filters.TransparecyController(bitmap, 100);
                images.Add(bitmap);
            }

            using (Graphics g = Graphics.FromImage(finalImage))
            {
                g.Clear(Color.Black);
                int offset = 0;
                foreach (Bitmap image in images)
                {
                    g.DrawImage(image, new Rectangle(offset, 0, image.Width, image.Height));
                }
            }
            return finalImage;
        }
    }
}
