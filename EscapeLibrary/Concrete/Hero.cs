using System;
using System.Drawing;
using EscapeLibrary.Abstract;

namespace EscapeLibrary.Concrete
{
    internal class Hero : Obj
    {
        // İki constructor'un(obj ve aşağıdaki) eşleşmesi için düzeltme yapıldı
        public Hero(int yukseklik, Size moveAreaSize) : base (moveAreaSize)
        {
            
            Left = 25; // Oluşturulan karakterin başlangıç konumu
            Top = 25;          
            // Her seferinde 150 birim hareket etmesini istedim. 
            HMoveDistance = 150;
            

        }
    }
}
