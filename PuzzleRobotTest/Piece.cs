using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PuzzleRobotTest
{
    class Piece : IComparable
    {
        private int id;
        public PictureBox piecePicBox;
        private Bitmap realImage;
        private int rotatePosition;
        public Piece right = null;
        public int rightPix = 0;
        public Piece left = null;
        public int leftPix = 0;
        public Piece up = null;
        public int upPix = 0;
        public Piece down = null;
        public int downPix = 0;
        public int similaarPieces = 0;
        public int location = 0;

        public Piece( PictureBox piecePicBox)
        {
            this.piecePicBox = piecePicBox;
            rotatePosition = 0;
        }

        public int getId() { return this.id; }
        public void setId(int id) { this.id = id; }

        public PictureBox getPiecePicBox() { return this.piecePicBox; }
        public void setBieceOPicBox(PictureBox piecebm) { this.piecePicBox = piecebm; }

        public Image getRealImage() { return this.realImage; }

        public int getRotatePosition() { return this.rotatePosition; }

        public void rotatePicBox(int rotatePos)
        {
            this.rotatePosition = rotatePos;
            if (rotatePosition == 1)
                piecePicBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            else if (rotatePosition == 2)
                piecePicBox.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
            else if (rotatePosition == 3)
                piecePicBox.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
        }

        public void randomRotatePicBox()
        {
            this.realImage = new Bitmap(piecePicBox.Image);
            int random = new Random().Next(4);
            rotatePicBox(random);
        }

        public void rotate90Degree()
        {
            piecePicBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            rotatePosition++;
            if (rotatePosition >= 4)
                rotatePosition = 0;
        }

        public Bitmap getImageBitmap()
        {
            return (Bitmap)piecePicBox.Image;
        }

        public int CompareTo(object obj)
        {
            Piece p = obj as Piece;
            if (p != null)
                return this.similaarPieces.CompareTo(p.similaarPieces);
            else
                throw new NotImplementedException();
        }

        public void cleanSimilarPieces()
        {
             this.right = null;
             this.rightPix = 0;
             this.left = null;
             this.leftPix = 0;
             this.up = null;
             this.upPix = 0;
             this.down = null;
             this.downPix = 0;
             this.similaarPieces = 0;
             this.location = 0;

    }
}
}
