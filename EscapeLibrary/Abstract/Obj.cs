using EscapeLibrary.Enum;
using EscapeLibrary.Interface;
using System;
using System.Drawing;
using System.Windows.Forms;


namespace EscapeLibrary.Abstract
{
    // Obj sınıfından türetilen bütün nesneler Picturebox özelliği taşır.
    // B221200305 Ömer Kubilay Çelik
    internal abstract class Obj : PictureBox, IMove 
    {
        public Size MoveAreaSize { get; }

        public int HMoveDistance { get; protected set; }
        public int EMoveDistance { get; protected set; }
        public new int Right 
        { get => base.Right;
           set => Left = value - Width; // Set özelliği olan bir Right oluşturduk.
            // B221200305 Ömer Kubilay Çelik
        }
        public new int Bottom // Set özelliği olan bir Bottom oluşturuldu.
        {
          get => base.Bottom;        
          set => Top = value - Height;
        }

        public int Center // Yatay konumda Orta noktanın yatay hareket kontrolünü oluşturduk
        { 
            get => Left + (Width/2);
            set => Left = value - (Width/2);
        }

        public int Middle // Dikey konumda Orta noktanın yatay hareket kontrolünü oluşturduk
        { 
            get => Top + (Height/2);
            set => Top = value - (Height/2);
        }

        protected Obj(Size moveAreaSize)
        {
            Image = Image.FromFile($@"img\{GetType().Name}.png"); // Hangi isimle sınıf oluşturursak aynı isimli resmi o nesneye ait picture ile nesne oluşturacak(new'lenen sınıf).
            MoveAreaSize = moveAreaSize; // Constructor kullanarak hareket alanını belirledik. B221200305 Ömer Kubilay Çelik
            SizeMode = PictureBoxSizeMode.StretchImage;           
        }
        public bool DoMove(Direction direction)
        {
            switch (direction)
            {
                case Direction.left:
                    return MoveLeft();
                case Direction.right:
                    return MoveRight();
                case Direction.up:
                    return MoveUp();
                case Direction.down:
                    return MoveDown();
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction),direction,null);
            }
        }

        private bool MoveDown()
        {
            if (Bottom == MoveAreaSize.Width -25) return true;

            var newBottom = Bottom + HMoveDistance; // Hero için belirlenmiş mesafe kadar hareket eder !!!!!!
            var isOver = newBottom > MoveAreaSize.Height - 25;

            Bottom = isOver ? MoveAreaSize.Height -25: newBottom; // Set özelliği verdiğimiz Right'ı kullanıyoruz

            return Bottom == MoveAreaSize.Height -25;
        }

        private bool MoveUp() 
        {
            if (Top == 25) return true; // Eğer çarptıysa Bool değişkeni true olacak.
                                        // B221200305 Ömer Kubilay Çelik
            var newTop = Top - HMoveDistance; // Set özelliği eklediğimiz Top property'sini kullandık.
            var isOver = newTop < 25;
            Top = isOver ? 25 : newTop; // Hareket alanı hareket mesafesinden az ise hareket alanı kadar hareket edecek

            return Top == 25;
        }

        private bool MoveRight()
        {
            if (Right == MoveAreaSize.Width-25) return true;

            var newRight = Right + HMoveDistance; // Hero için belirlenmiş mesafe kadar hareket eder !!!!!!
            var isOver = newRight > MoveAreaSize.Width-25;

            Right = isOver ? MoveAreaSize.Width-25 : newRight ; // Set özelliği verdiğimiz Right'ı kullanıyoruz

            return Right == MoveAreaSize.Width-25;
        }

        private bool MoveLeft()
        {
            if (Left == 25) return true; // Eğer çarptıysa Bool değişkeni true olacak.
                                        // B221200305 Ömer Kubilay Çelik
            var newLeft = Left - HMoveDistance; // Hero için belirlenmiş mesafe kadar hareket eder !!!!!!
            var isOver = newLeft < 25;
            Left = isOver ? 25 : newLeft; // Hareket alanı hareket mesafesinden az ise hareket alanı kadar hareket edecek

            return Left == 25;            
        }
    }
}
