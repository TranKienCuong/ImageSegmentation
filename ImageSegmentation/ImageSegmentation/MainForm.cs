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
        public MainForm()
        {
            InitializeComponent();
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            noticeLabel.Visible = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pathLabel.Text = "File path: " + openFileDialog.FileName;
                inputPictureBox.ImageLocation = openFileDialog.FileName;
            }
        }

        private void runButton_Click(object sender, EventArgs e)
        {
            noticeLabel.Visible = true;
            if (openFileDialog.FileName == "")
            {
                noticeLabel.Text = "Please load an image!";
                return;
            }
            noticeLabel.Text = "Please wait...";
            Application.DoEvents();

            int means = (int)meansNumericUpDown.Value;
            int maxInsideLoops = (int)maxLoopsNumericUpDown.Value;
            int minError = (int)minErrorNumericUpDown.Value;

            Segmentation segmentation = new Segmentation(means, maxInsideLoops, minError);
            Bitmap input = (Bitmap)inputPictureBox.Image;
            Bitmap output = segmentation.GetOutputImage(input);
            outputPictureBox.Image = output;

            saveButton.Visible = true;
            noticeLabel.Visible = false;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Bitmap output = (Bitmap)outputPictureBox.Image;
                output.Save(saveFileDialog.FileName);
            }
        }
    }
}
