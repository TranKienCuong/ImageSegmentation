using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageSegmentation
{
    public partial class MainForm : Form
    {
        int numberOfMeans = 3;

        public MainForm()
        {
            InitializeComponent();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            noticeLabel.Visible = false;
            openFileDialog.ShowDialog();
            pathLabel.Text = "File path: " + openFileDialog.FileName;
            inputPictureBox.ImageLocation = openFileDialog.FileName;
        }

        private void meansNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            numberOfMeans = (int)meansNumericUpDown.Value;
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.FileName == "")
            {
                noticeLabel.Visible = true;
                return;
            }
            Segmentation segmentation = new Segmentation(numberOfMeans);
            Bitmap input = (Bitmap)inputPictureBox.Image;
            Bitmap output = segmentation.GetOutputImage(input);
            outputPictureBox.Image = output;
        }
    }
}
