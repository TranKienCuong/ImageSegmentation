using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageSegmentation
{
    /// <summary>
    /// The main class of the program, contains methods for segmentation process
    /// </summary>
    public class Segmentation
    {
        int K;
        int MaxLoops = 50;
        int MinE = 0;
        List<Pixel> Pixels = new List<Pixel>();
        List<Pixel> Means = new List<Pixel>();
        List<Pixel>[] Clusters;

        public Segmentation() { }

        public Segmentation(int K, int MaxLoops, int MinE)
        {
            this.K = K;
            this.MaxLoops = MaxLoops;
            this.MinE = MinE;

            Clusters = new List<Pixel>[K];
            for (int i = 0; i < K; i++)
                Clusters[i] = new List<Pixel>();          
        }

        public Bitmap GetOutputImage(Bitmap input)
        {
            LoadImage(input);

            KMeans();

            Bitmap output = new Bitmap(input.Width, input.Height);
            Color[] colors = new Color[K];
            for (int i = 0; i < K; i++)
                colors[i] = GetClusterColor(Clusters[i]);
            foreach (Pixel p in Pixels)
                output.SetPixel(p.X, p.Y, colors[p.Label]);

            return output;
        }

        void LoadImage(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color color = bitmap.GetPixel(i, j);
                    Pixel p = new Pixel(color.R, color.G, color.B, i, j);
                    Pixels.Add(p);
                }
            }
        }

        long Distance(Pixel a, Pixel b)
        {
            return (a.R - b.R) * (a.R - b.R) * 3 + (a.G - b.G) * (a.G - b.G) * 3 + (a.B - b.B) * (a.B - b.B) * 3 + (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }

        void KMeans()
        {
            // initialize by randomizing means
            for (int i = 0; i < K; i++)
            {
                Random rand = new Random();
                Pixel p = Pixels[rand.Next(Pixels.Count)];
                if (!Means.Contains(p))
                    Means.Add(p);
                else // ignore adding mean if it's already existed
                    i--;
            }

            int loop = 0;
            long e = int.MaxValue;

            while (loop < MaxLoops && e > MinE)
            {
                // label each pixel to the closest mean
                foreach (Pixel p in Pixels)
                {
                    long minDistance = long.MaxValue;
                    int label = 0;
                    for (int i = 0; i < K; i++)
                    {
                        Pixel mean = Means[i];
                        long distance = Distance(p, mean);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            label = i;
                        }
                    }
                    p.Label = label;
                    Clusters[label].Add(p);
                }

                // compute means again
                e = 0;
                for (int i = 0; i < K; i++)
                {
                    long sumR = 0, sumG = 0, sumB = 0, sumX = 0, sumY = 0;
                    foreach (Pixel p in Clusters[i])
                    {
                        sumR += p.R;
                        sumG += p.G;
                        sumB += p.B;
                        sumX += p.X;
                        sumY += p.Y;
                    }
                    int n = Clusters[i].Count;
                    Pixel temp = new Pixel((int)(sumR / n), (int)(sumG / n), (int)(sumB / n), (int)(sumX / n), (int)(sumY / n));
                    e += Distance(Means[i], temp);
                    Means[i] = temp;
                }
                loop++;
            }
            Console.WriteLine(loop);
        }

        Color GetClusterColor(List<Pixel> pixels)
        {
            long sumR = 0, sumG = 0, sumB = 0;
            foreach (Pixel p in pixels)
            {
                sumR += p.R;
                sumG += p.G;
                sumB += p.B;
            }
            int n = pixels.Count;
            return Color.FromArgb((int)(sumR / n), (int)(sumG / n), (int)(sumB / n));
        }
    }
}
