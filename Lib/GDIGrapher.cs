using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

namespace Lib
{
    public class GdiGrapher
    {
        private const double PHI = 1.618033988; // see http://en.wikipedia.org/wiki/Golden_ratio

        // Humans can't seem to discriminate more than a dozen or so colors at once.
        // Sequence based on: http://en.wikipedia.org/wiki/Color_term#Basic_color_terms
        private static readonly Brush[] BrushesForAuthors =
        {
            Brushes.LightGray,
            Brushes.Red,
            Brushes.Green,
            Brushes.Yellow,
            Brushes.Blue,
            Brushes.Brown,
            Brushes.Orange,
            Brushes.Pink,
            Brushes.LightBlue,
            Brushes.LightGreen,
        };


        public string Graph(Dictionary<string, List<int>> ownership, string outputFileName)
        {
            const int SQUARE_SIZE = 6;
            const int SPACING_BETWEEN_SQUARES = 2;
            const int SPACING_BETWEEN_RECTS = 10;

            const int WIDTH_OF_IMAGE = 1000;
            const int HEIGHT_OF_IMAGE = 3200; //Need to fix this...
            var bitmap = new Bitmap(WIDTH_OF_IMAGE, HEIGHT_OF_IMAGE);

            Pen blackPen = Pens.Black;
            var font = new Font("Arial", 2, FontStyle.Regular);

            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);

                int xOffset = 10;
                int yOffset = 10;
                int maxHeightOfRectangle = 0;

                foreach (var directory in ownership)
                {
                    var r = CalcSize(directory.Value.Count);
                    int width = r[0] * (SQUARE_SIZE + SPACING_BETWEEN_SQUARES) + SPACING_BETWEEN_SQUARES + 1;
                    int height = r[1] * (SQUARE_SIZE + SPACING_BETWEEN_SQUARES) + SPACING_BETWEEN_SQUARES + 1;

                    if (xOffset + width > WIDTH_OF_IMAGE - 10)
                    {
                        yOffset += maxHeightOfRectangle + SPACING_BETWEEN_RECTS;
                        xOffset = 10;
                        maxHeightOfRectangle = 0;
                    }

                    if (height > maxHeightOfRectangle)
                        maxHeightOfRectangle = height;

                    g.DrawRectangle(blackPen, xOffset, yOffset, width, height);

                    int xOffsetSquare = xOffset + SPACING_BETWEEN_SQUARES + 1;
                    int yOffsetSquare = yOffset + SPACING_BETWEEN_SQUARES + 1;
                    //var ordered = directory.Value;
                    var ordered = directory.Value.OrderBy(i => i == 0 ? Int32.MaxValue : i).ToList();
                    for (int index = 0; index < ordered.Count; index++)
                    {
                        int codeOwnership = ordered[index];
                        g.FillRectangle(SelectBrush(codeOwnership), xOffsetSquare, yOffsetSquare, SQUARE_SIZE, SQUARE_SIZE);

                        if (index != 0 && ((index + 1)%r[0]) == 0)
                        {
                            yOffsetSquare += SQUARE_SIZE + SPACING_BETWEEN_SQUARES;
                            xOffsetSquare -= (SQUARE_SIZE + SPACING_BETWEEN_SQUARES)*(r[0]-1);
                        }
                        else
                        {
                            xOffsetSquare += SQUARE_SIZE + SPACING_BETWEEN_SQUARES;
                        }
                    }
                    xOffset += width + SPACING_BETWEEN_RECTS;
                }
            }

            string filename = outputFileName + ".png";
            bitmap.Save(filename, ImageFormat.Png);
            return filename;
        }

        private static Brush SelectBrush1(Int32 codeOwnership)
        {
            if (codeOwnership == 0)
                return Brushes.Green;
            return Brushes.Red;
        }

        private static Brush SelectBrush(Int32 codeOwnership)
        {
            if (codeOwnership < BrushesForAuthors.Length)
                return BrushesForAuthors[codeOwnership];
            return Brushes.Black;
        }
        
        private static int[] CalcSize1(int num)
        {
            return new int[] {5, (num/5) + (num%5 > 0 ? 1 : 0)};
        }

        private static int[] CalcSize(int num)
        {
            int w = (int)Math.Floor(Math.Sqrt(PHI * num));
            int h = (int)Math.Floor(Math.Sqrt(1/PHI * num));

            // there must be a better way of initialising the list...
            var rects = new List<int[]>();
            rects.Add(new[] {w+1,h});
            rects.Add(new[] {w,h+1});
            rects.Add(new[] {w-1,h+1});
            rects.Add(new[] {w+1,h+1});
            
            var result = rects.Where(r => r[0]*r[1] >= num)
                              .Where(r => ((double)r[0])/r[1] >= 1 && ((double)r[0])/r[1] < 2)
                              .OrderBy(r => r[0]*r[1])
                              .First();
            return result;

        }

//  def rect2(x)
//    w = Math.sqrt(PHI*x).floor.to_f
//    h = Math.sqrt(1/PHI*x).floor.to_f
//  
//    r = [ [w+1, h], [w, h+1], [w-1, h+1], [w+1, h+1] ]  # create some options
//    r = r.select{ |r| r[0]*r[1] >= x }                  # must be big enough
//    r = r.select{ |r| (1..2).include?(r[0]/r[1]) }      # must have reasonable ratio
//    r.sort{ |a, b| a[0]*a[1] - b[0]*b[1] }.first        # pick the smallest one
//  end
    
    }
}