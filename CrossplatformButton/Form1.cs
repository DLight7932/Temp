using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrossplatformButton
{
    public partial class Form1 : Form
    {
        MyButton button1 = new MyButton();
        MyRoundcornerButton button2 = new MyRoundcornerButton();

        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            button1.position = new VectorInt2(100, 100);
            button1.scale = new VectorInt2(100, 100);
            button1.HorizontalAlignment = HorizontalAlignment.Midle;
            button1.VerticalAlignment = VerticalAlignment.Midle;
            button1.text = "butooon1";
            button1.Icon = new Bitmap("Png.png");
            button1.MyMouseClick += (object sender, MouseEventArgs e) =>
            {
                button1.position = new VectorInt2(
                    random.Next(button1.position.x - button1.Left, pictureBox1.Width - (button1.Right - button1.position.x)),
                    random.Next(button1.position.y - button1.Top, pictureBox1.Height - (button1.Bottom - button1.position.y)));
                //new Thread(() => { MessageBox.Show(".!."); }).Start();
            };
            button2.position = new VectorInt2(pictureBox1.Width / 2, pictureBox1.Height / 2);
            button2.scale = new VectorInt2(100, 100);
            button2.HorizontalAlignment = HorizontalAlignment.Midle;
            button2.VerticalAlignment = VerticalAlignment.Midle;
            button2.text = "butooon2";
            button2.Icon = new Bitmap("Png.png");
            button2.RoundSize = 50;
            button2.MyMouseClick += (object sender, MouseEventArgs e) =>
            {
                button2.RoundSize = random.Next(100);
                button1.Enabled = !button1.Enabled;
                //new Thread(() => { MessageBox.Show(".,."); }).Start();
            };

            pictureBox1.MouseMove += button1.ControlMouseMoveEvent;
            pictureBox1.MouseDown += button1.ControlMouseDownEvent;
            pictureBox1.MouseUp += button1.ControlMouseUpEvent;

            pictureBox1.MouseMove += button2.ControlMouseMoveEvent;
            pictureBox1.MouseDown += button2.ControlMouseDownEvent;
            pictureBox1.MouseUp += button2.ControlMouseUpEvent;

            pictureBox1_Resize(null, null);

            pictureBox1.MouseMove += (object sender, MouseEventArgs e) => { pictureBox1.Invalidate(); };
        }

        Bitmap bufferBitmap;
        Graphics bufferGraphics;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            bufferGraphics = Graphics.FromImage(bufferBitmap);

            bufferGraphics.Clear(BackColor);
            button1.Display(bufferGraphics);
            button2.Display(bufferGraphics);
            pictureBox1.Image = bufferBitmap;
        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {
            bufferBitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
        }
    }
}
