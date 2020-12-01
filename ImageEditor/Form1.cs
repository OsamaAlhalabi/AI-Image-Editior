using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageEditor
{
    public partial class Form1 : Form
    {
        // We do not need this section .. WF 
        public Form1()
        {
            InitializeComponent();
            CustomizeDesign();
        }
        private void CustomizeDesign()
        {
            FiltersPanel.Visible = false;
            EffectsPanel.Visible = false;
            ToolsPanel.Visible = false;
            BrushPanel.Visible = false;
            ImagePanel.Visible = false;
        }
        private void HideSubmenu()
        {
            if (FiltersPanel.Visible == true)
                FiltersPanel.Visible = false;
            if (ToolsPanel.Visible == true)
                ToolsPanel.Visible = false;
            if (EffectsPanel.Visible == true)
                EffectsPanel.Visible = false;
            if (BrushPanel.Visible == true)
                BrushPanel.Visible = false;
            if (ImagePanel.Visible == true)
                ImagePanel.Visible = false;
        }
        private void ShowSubmenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                HideSubmenu();
                submenu.Visible = true;
            }
            else
                submenu.Visible = false;
        }

        // End of the section!

        private Bitmap myBitMap;
        private Image file;
        private Filters filters = new Filters();
        private Layers layers = new Layers();
        private Tools tools = new Tools();
        private Boolean opened = false;
        private Boolean brushandSelect = false;
        private Boolean select = false;
        private Boolean brush = false;
        private int crpX, crpY, rectW, rectH;
        private Point end = new Point();
        private Point current = new Point();
        private Pen brushPen = new Pen(Color.Black, 3);
        private Graphics brushGraph;
        public Pen crpPen = new Pen(Color.White);

        void OpenImage()
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if(dr == DialogResult.OK)
            {
                file = Image.FromFile(openFileDialog1.FileName);
                myBitMap = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = file;
                layers.AddEdition(myBitMap);
                layers.addUserLayer(new Bitmap(pictureBox1.Image));
                opened = true;
            }
        }
        void SaveImage()
        {
            if (opened)
            {
                SaveFileDialog sfd = new SaveFileDialog(); 
                sfd.Filter = "Images|*.png;*.bmp;*.jpg";
                ImageFormat format = ImageFormat.Png;
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string ext = Path.GetExtension(sfd.FileName);
                    switch (ext)
                    {
                        case ".jpg":
                            format = ImageFormat.Jpeg;
                            break;
                        case ".bmp":
                            format = ImageFormat.Bmp;
                            break;
                    }
                    pictureBox1.Image.Save(sfd.FileName, format);
                }



            }
            else { MessageBox.Show("No image loaded, first upload image "); }

        }
        void Reload()
        {
            if (!opened)
            {
                MessageBox.Show("There is no photo to reload it! ");
            }
            else
            {
                file = Image.FromFile(openFileDialog1.FileName);
                myBitMap = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = file;
                opened = true;
            }
        }
        private void DrawInSelection()
        {
            if (select)
            {
                if(rectH != 0 && rectW != 0)
                {
                    brushGraph.DrawEllipse(brushPen, rectW, rectH, rectW, rectH);
                }
            }
        }
         private void Emboss()
        {
            if (myBitMap == null)
            {
                MessageBox.Show("There is no Photo to edit it!");
            }
            else
            {
                pictureBox1.Image = layers.AddEdition(filters.Emboss(myBitMap));
            }
        }
       
        public void AdjustContrast()
        {
            pictureBox1.Image = layers.AddEdition(filters.AdjustContrast(myBitMap, 20));

        }
        public void GammaController(float value)
        {
            pictureBox1.Image = layers.AddEdition(filters.GammaController(myBitMap, value));
        }
     
        public void Blur()
        {
            pictureBox1.Image= filters.Blur(myBitMap);
        }
        public void BrightnessAdjust(float value)
        {
            if (myBitMap == null)
            {
                MessageBox.Show("There is no Photo to edit it!");
            }
            else
            {
                pictureBox1.Image = layers.AddEdition(filters.BrightnessAdjust(myBitMap, value));
            }
            
        }
        public void ConfusionFilter()
        {
            if (!opened)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {
                pictureBox1.Image = layers.AddEdition(filters.ConfusionFilter(pictureBox1.Image));
            }
        }
        void GrayScaleFilter()
        {
            if (!opened)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {
                pictureBox1.Image = layers.AddEdition(filters.GrayFilter(pictureBox1.Image));
            }
        }

        void FlashLightFilter()
        {
            if (!opened)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {
                pictureBox1.Image = layers.AddEdition(filters.FlashLightFilter(pictureBox1.Image));
            }
        }

        void BlueFrozenFilter()
        {

            if (!opened)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {   
                pictureBox1.Image = layers.AddEdition(filters.BlueFrozenFilter(pictureBox1.Image));
            }

        }


        void BlueWinterFilter()
        {
            if (!opened)
            {
                MessageBox.Show("Open an Image then apply changes");
            }
            else
            {
                pictureBox1.Image = layers.AddEdition(filters.BlueWinterFilter(pictureBox1.Image));
            }
        }
        public void Crop()
        {
            if (rectH == 0 || rectW == 0)
                return;
            else
            {
                Cursor = Cursors.Default;
                Bitmap bmp2 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.DrawToBitmap(bmp2, pictureBox1.ClientRectangle);
                Bitmap crpImg = new Bitmap(rectW, rectH);
                for (int i = 0; i < rectW; i++)
                {
                    for (int y = 0; y < rectH; y++)
                    {
                        Color pxlclr = bmp2.GetPixel(crpX + i, crpY + y);
                        crpImg.SetPixel(i, y, pxlclr);
                    }
                }
                pictureBox1.Image = (Image)crpImg;
                myBitMap = bmp2;
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            }
        }
        
        public void DisplayLayers()
        {
            this.pictureBox1.Image = layers.DisplayLayers(myBitMap) ;
    
        }

        void HueController()
        {
            float changered = RedBar.Value * 0.1f;
            float changegreen = GreenBar.Value * 0.1f;
            float changeblue = BlueBar.Value * 0.1f;
            Reload();
            if (!opened)
            {
            }
            else
            {
                pictureBox1.Image = layers.AddEdition(filters.HueController(pictureBox1.Image, changered, changegreen, changeblue));
            }
        }


        /// <summary>
        /// Trash section  ... windows form, we do not need it!
        /// all what we need from this script is crop and select funcs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenImage(); 
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConfusionFilter();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GrayScaleFilter();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BlueWinterFilter();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            BlueFrozenFilter();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FlashLightFilter();
        }

        private void GreenBar_VisibleChanged(object sender, EventArgs e)
        {
   
        }

        private void RedBar_ValueChanged(object sender, EventArgs e)
        {
            HueController();
        }

        private void GreenBar_ValueChanged(object sender, EventArgs e)
        {
            HueController();
        }

        private void BlueBar_Scroll(object sender, EventArgs e)
        {

        }

        private void BlueBar_ValueChanged(object sender, EventArgs e)
        {
            HueController();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            Blur();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label7.Text = trackBar1.Value.ToString();
            Reload();
            BrightnessAdjust(trackBar1.Value );
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Emboss();
        }


  
        private void button12_Click_1(object sender, EventArgs e)
        {
            if (opened)
                AdjustContrast();
            else
                MessageBox.Show("There is no Photo to edit it!");
        }

        private void trackBar2_Scroll_1(object sender, EventArgs e)
        {
            label8.Text = trackBar2.Value.ToString();
            GammaController(trackBar2.Value);


        }
        private void button14_Click(object sender, EventArgs e)
        {
            Crop();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (select)
            {
                select = false;
                Cursor = Cursors.Default;
            }
            else
            {
                select = true;
                Cursor = Cursors.Cross;
            }
        }

        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {

        }

        private void Form1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
        }


        private void button14_MouseDown(object sender, MouseEventArgs e)
        {
            // Determine the initial rectangle coordinates...
        }

        private void button14_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void button14_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {

            if (select )
            {
              base.OnMouseDown(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                crpPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                crpX = e.X;
                crpY = e.Y;
                   
            }
            }
            if (brush)
            {
                end = e.Location;
            }

        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (brushandSelect)
                brushandSelect = false;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button17_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if(cd.ShowDialog() == DialogResult.OK)
            {
                brushPen.Color = cd.Color;
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            brushPen.Width *= 2;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            brushPen.Width *= -2;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            brushPen.Width += 1;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            brushPen.Width += -1;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
            select = false;
            brush = false;

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (select)
            {
                base.OnMouseMove(e);
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    pictureBox1.Refresh();
                    rectW = e.X - crpX;
                    rectH = e.Y - crpY;
                    Graphics g = pictureBox1.CreateGraphics();
                    g.DrawRectangle(crpPen, crpX, crpY, rectW, rectH);
                    g.Dispose();
                }
            }
            if (brush)
            {
                BrushTool(e,false);
            }   
           
        }

        private void button21_Click(object sender, EventArgs e)
        {
            brushandSelect = true;

        }
        private void makeEdit(Bitmap resBitMap)
        {
            layers.addLayer(resBitMap);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            pictureBox2.Image= layers.Traverse();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void button22_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.Retreat();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            layers.addUserLayer(new Bitmap (pictureBox1.Image));
            myBitMap = (Bitmap)pictureBox1.Image;
        }

        private void button25_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = filters.TransparecyController(myBitMap, 40);
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button26_Click(object sender, EventArgs e)
        {
            DisplayLayers();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            pictureBox1.Image = filters.TransparecyController(myBitMap, (Byte)trackBar3.Value);
        }

        private void button27_Click(object sender, EventArgs e)
        {
            ShowSubmenu(FiltersPanel);
        }

        private void ToolsMenu_Click(object sender, EventArgs e)
        {
            ShowSubmenu(ToolsPanel);
        }

        private void BrushMenu_Click(object sender, EventArgs e)
        {
            ShowSubmenu(BrushPanel);
        }

        private void EffectsMenu_Click(object sender, EventArgs e)
        {
            ShowSubmenu(EffectsPanel);
        }

        private void button34_Click(object sender, EventArgs e)
        {
            ShowSubmenu(ImagePanel) ;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(filters.AutoCorrection(myBitMap));
            myBitMap = (Bitmap)pictureBox1.Image;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            layers.addUserLayer(pictureBox1.Image);
        }

        private void button30_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(filters.WaterWaveFilter(myBitMap));
            myBitMap = (Bitmap)pictureBox1.Image;
        }

        private void button33_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(filters.SmothFilter(myBitMap));
            myBitMap = (Bitmap)pictureBox1.Image;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(filters.SepiaFilter(myBitMap));
            myBitMap = (Bitmap)pictureBox1.Image;
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(filters.JitterFilter(myBitMap));
            myBitMap = (Bitmap)pictureBox1.Image;
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(filters.AutoCorrection(myBitMap));
            myBitMap = (Bitmap)pictureBox1.Image;
        }

        private void button21_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(filters.YCbCrFilter(myBitMap));
            myBitMap = (Bitmap)pictureBox1.Image;
        }

        private void button25_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(filters.PosterizationFilter(myBitMap));
            myBitMap = (Bitmap)pictureBox1.Image;
        }

        private void button31_Click(object sender, EventArgs e)
        {
            //pictureBox1.Image = layers.AddEdition(filters.RedMask(myBitMap));
            pictureBox1.Image = layers.AddEdition(filters.ResizeBicubic(myBitMap,400,300));
        }

        private void button32_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(filters.MeanFilter(myBitMap));
        }

        private void button34_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(filters.GaussianSharpenFilter(myBitMap));
        }

        private void button35_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(filters.ExtractChannelFilter(myBitMap));
        }

        private void button9_Click_2(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(tools.Mirror_Y(myBitMap));
        }

        private void button36_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(tools.Mirror_X(myBitMap));
        }

        private void button37_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(filters.YCbCrLinearFilter(myBitMap));
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {

        }

        private void button24_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(tools.Rotate(myBitMap));
     
        } 

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button38_Click(object sender, EventArgs e)
        {
      
        }

        private void button38_Click_1(object sender, EventArgs e)
        {
            pictureBox1.Image = layers.AddEdition(tools.ErasePart(myBitMap, new Rectangle(crpX, crpY, rectW, rectH)));
        }

        private void BrushTool(MouseEventArgs e , Boolean selectionBrush)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (!selectionBrush)
                {
                    current = e.Location;
                    brushGraph.DrawLine(brushPen, end, current);
                    
                    end = current;
                }
                else
                {
                    if (e.Location.X < rectW + crpX  &&
                        e.Location.Y < rectH + crpY && 
                        e.Location.X > crpX && 
                        e.Location.Y > crpY)
                    {
                            current = e.Location;
                            brushGraph.DrawLine(brushPen, end, current);
                            end = current;
                    }

                }
            }
        }
        private void button15_Click(object sender, EventArgs e)
        {
      
            if (brush)
                brush = false;
            else
            {
                brushGraph = pictureBox1.CreateGraphics();
                brushPen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round,
                    System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);
                brush = true;
            }
               
        }

        
        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            base.OnMouseEnter(e);
        }
    }

}
