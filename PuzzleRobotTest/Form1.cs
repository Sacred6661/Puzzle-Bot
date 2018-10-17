using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleRobotTest
{
    public partial class Form1 : Form
    {
        List<Piece> picBox = new List<Piece>();
        List<Piece> listOfParts = new List<Piece>();
        List<Piece> maxCountPieces = new List<Piece>();
        List<Piece> finishList = new List<Piece>();
        int similarityIndex = 0;
        int accuracy = 80;

        public Form1()
        {
            InitializeComponent();
            Bitmap[] test = new Bitmap[9];
            makeBoard(9);
            string writtingPath = AppDomain.CurrentDomain.BaseDirectory + "\\RockNRoll\\Pieces\\";

            FolderBrowserDialog DirDialog = new FolderBrowserDialog();
            if (DirDialog.ShowDialog() == DialogResult.OK)
            {
                writtingPath = DirDialog.SelectedPath;
            }

            var images = Directory.GetFiles(writtingPath, "*.jpg");                                    //take all jpg pieces
            var direct = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "\\");

            for(int i = 0; i < 9; i++)
            {
                test[i] = new Bitmap(images[i]);
            }
            

            pictureBox1.Image = new Bitmap(test[0]);
            pictureBox2.Image = new Bitmap(test[1]);
            pictureBox3.Image = new Bitmap(test[2]);
            pictureBox4.Image = new Bitmap(test[3]);
            pictureBox5.Image = new Bitmap(test[4]);
            pictureBox6.Image = new Bitmap(test[5]);
            pictureBox7.Image = new Bitmap(test[6]);
            pictureBox8.Image = new Bitmap(test[7]);
            pictureBox9.Image = new Bitmap(test[8]);

            picBox.Add(new Piece(pictureBox1));
            picBox.Add(new Piece(pictureBox2));
            picBox.Add(new Piece(pictureBox3));
            picBox.Add(new Piece(pictureBox4));
            picBox.Add(new Piece(pictureBox5));
            picBox.Add(new Piece(pictureBox6));
            picBox.Add(new Piece(pictureBox7));
            picBox.Add(new Piece(pictureBox8));
            picBox.Add(new Piece(pictureBox9));

            similarityIndex = 390;
            accuracy = 80;
            makePuzzleTogether();

            debugSimilarityToFile();

        }

        public void debugSimilarityToFile()
        {
            string writePath = @"D:\debug.txt";
            FileStream fcleanFile = File.Open(writePath, FileMode.Create);
            fcleanFile.Close();
            for (int j = 0; j < picBox.Count; j++)
            {
                int count = 0;
                for (int i = 0; i < picBox.Count; i++)
                {
                    if (picBox[i] == picBox[j]) continue;
                    else
                    {
                        count = getLeft(picBox[j].getImageBitmap(), picBox[i].getImageBitmap());
                        using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine((j + 1) + " - " + (i + 1) + " : " + count + " : left");
                        }
                        count = getRight(picBox[j].getImageBitmap(), picBox[i].getImageBitmap());
                        using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine((j + 1) + " - " + (i + 1) + " : " + count + " : right");
                        }
                        count = getUp(picBox[j].getImageBitmap(), picBox[i].getImageBitmap());
                        using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine((j + 1) + " - " + (i + 1) + " : " + count + " : up");
                        }
                        count = getDown(picBox[j].getImageBitmap(), picBox[i].getImageBitmap());
                        using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                        {
                            sw.WriteLine((j + 1) + " - " + (i + 1) + " : " + count + " : down");
                        }
                    }
                }
            }
        }

        private void makePuzzleTogether()
        {
            try
            {
                cleanPieces();
                for (int j = 0; j < picBox.Count; j++)
                {
                    int count = 0;
                    Piece piece = picBox[j];
                    for (int i = 0; i < picBox.Count; i++)
                    {
                        if (picBox[i] == piece) continue;
                        else
                        {
                            count = getLeft(piece.getImageBitmap(), picBox[i].getImageBitmap());
                            if ((count >= similarityIndex) && (count > piece.leftPix))
                            { piece.left = picBox[i]; piece.leftPix = count; piece.similaarPieces++; }

                            count = getRight(piece.getImageBitmap(), picBox[i].getImageBitmap());
                            if ((count >= similarityIndex) && (count > piece.rightPix))
                            { piece.right = picBox[i]; piece.rightPix = count; piece.similaarPieces++; }

                            count = getUp(piece.getImageBitmap(), picBox[i].getImageBitmap());
                            if ((count >= similarityIndex) && (count > piece.upPix))
                            { piece.up = picBox[i]; piece.upPix = count; piece.similaarPieces++; }

                            count = getDown(piece.getImageBitmap(), picBox[i].getImageBitmap());
                            if ((count >= similarityIndex) && (count > piece.downPix))
                            { piece.down = picBox[i]; piece.downPix = count; piece.similaarPieces++; }
                        }
                    }
                }

                

                finishList.Add(findFirstPiece());
                int index = 0;

                while (finishList.Count() < 9)
                {
                    Piece piece = finishList[index];
                    if (piece.up != null)
                    {
                        if (!findPieceInList(finishList, piece.up))
                        {
                            piece.up.location = piece.location - 3;
                            finishList.Add(piece.up);
                        }
                    }
                    if (piece.right != null)
                    {
                        if (!findPieceInList(finishList, piece.right))
                        {
                            piece.right.location = piece.location + 1;
                            finishList.Add(piece.right);
                        }
                    }
                    if (piece.down != null)
                    {
                        if (!findPieceInList(finishList, piece.down))
                        {
                            piece.down.location = piece.location + 3;
                            finishList.Add(piece.down);
                        }
                    }
                    if (piece.left != null)
                    {
                        if (!findPieceInList(finishList, piece.left))
                        {
                            piece.left.location = piece.location - 1;
                            finishList.Add(piece.down);
                        }
                    }

                    index++;

                }

                foreach (Piece piece in finishList)
                {
                    listOfParts[piece.location].getPiecePicBox().Image = piece.getImageBitmap();
                }
            }
            catch (Exception ex)
            {
                if (similarityIndex > 320)
                {
                    similarityIndex -= 5;
                    cleanFinishList();
                    //MessageBox.Show("-10 = " + similarityIndex);
                    makePuzzleTogether();
                }
                else if (accuracy > 40)
                {
                    accuracy -= 5;
                    similarityIndex = 390;
                    cleanFinishList();
                    //MessageBox.Show("-10 = " + similarityIndex);
                    makePuzzleTogether();
                }
                else
                    MessageBox.Show("Error =(");
            }
        }

        //cleaning existed pieces
        public void cleanPieces()
        {
            foreach(Piece p in picBox)
            {
                p.cleanSimilarPieces();
            }
        }

        //cleaning list with results
        public void cleanFinishList()
        {
            int i = 0;
            while (finishList.Count() > 0)
                finishList.Remove(finishList[0]);
        }

        //function to search if piece exist in list
        private bool findPieceInList(List<Piece> list, Piece searchingPiece)
        {
            foreach (Piece piece in list)
            {
                if (piece == searchingPiece)
                    return true;
            }
            return false;
        }

        //firs piece is upper left piece
        private Piece findFirstPiece()
        {
            foreach(Piece piece in picBox)
            {
                if ((piece.left == null) && (piece.up == null))
                    return piece;
            }

            return null;
        }

        //function to find difference in 2 pixels
        private double getDifference(Color x, Color y)
        {
            int dr = Math.Abs(x.R - y.R);
            int dg = Math.Abs(x.G - y.G);
            int db = Math.Abs(x.B - y.B);
            return Math.Sqrt(dr * dr + dg * dg + db * db);
            //return (dr + dg + db) / 3.0;
        }

        //making board for result
        public void makeBoard(int count)
        {
            for (int i = 0; i < count; i++)
            {
                PictureBox pb = new PictureBox();
                pb.SizeMode = PictureBoxSizeMode.StretchImage;              // stretch the image
                pb.Height = 130;
                pb.Width = 130;
                pb.BackColor = Color.Red;

                //adding new element to layout
                flowLayoutPanel1.Controls.Add(pb);
                listOfParts.Add(new Piece(pb));
            }
        }


        //compare if two images could be simular
        public int getRight(Bitmap img1, Bitmap img2)
        {
            int count = 0;
            for (int i = 0; i < img1.Width; i++)
            {
                Color x = img1.GetPixel(img1.Width - 1, i);
                Color y = img2.GetPixel(0, i);

                double res = getDifference(x, y);

                //difference 80, it could be some other integer, but smaller could skip good pieces
                //end bigger - could take too much pieces
                if (res <= 80)
                    count++;
                
            }

            return count;
        }

        public int getLeft(Bitmap img1, Bitmap img2)
        {
            int count = 0;
            for (int i = 0; i < img1.Width; i++)
            {
                Color x = img2.GetPixel(img2.Width - 1, i);
                Color y = img1.GetPixel(0, i);

                double res = getDifference(x, y);

                if (res <= 80)
                    count++;
            }

            return count;
        }

        public int getDown(Bitmap img1, Bitmap img2)
        {
            int count = 0;
            for (int i = 0; i < img1.Width; i++)
            {
                Color x = img1.GetPixel(i, img1.Height - 1);
                Color y = img2.GetPixel(i, 0);

                double res = getDifference(x, y);

                if (res <= 40)
                    count++;
            }

            return count;
        }

        public int getUp(Bitmap img1, Bitmap img2)
        {
            int count = 0;
            for (int i = 0; i < img1.Width; i++)
            {
                Color x = img2.GetPixel(i, img2.Height - 1);
                Color y = img1.GetPixel(i, 0);

                double res = getDifference(x, y);

                 if (res <= 80)
                    count++;
            }

            return count;
        }

    }
}
