using EscapeLibrary.Abstract;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EscapeLibrary.Concrete
{
    internal class Minion : Obj
    {   
        // Minion isminde bir class oluşturdum ve Obj sınıfından kalıtım aldığı için, kendi sınıf ismiyle aynı bir png dosyası olan minion.png yi picturebox' un image değeri olarak atadı.
        // Random sınıfından bir random nesnesi oluşturuldu. Bu nesnenin next özelliğiyle x ve y değişkenlerine rastgele sayılar atandı.
        // Rastgele atanan x ve y değişkenleri, blocklardaki satır ve sütunları temsil eder. 150 ile çarpıldıklarında satır ve sütunun kesiştiği hücre olan block değerinde minion oluşturur.
        // Minion nesnesine ait name, size, sizemode, location özellikleri her nesne için aynı olacağından dolayı kendi sınıfının içinde tanımlanmıştır.
        public Minion(Size moveAreaSize) : base(moveAreaSize)
        {
            Random random = new Random();
            EMoveDistance = 150;
            int x = random.Next(6, 9) * 150 ; // Minionların rastgele oluşacağı konumlar. 
            int y = random.Next(1, 5) * 150 ;
            Name = "Minion";
            Size = new Size(150, 150);
            SizeMode = PictureBoxSizeMode.StretchImage;
            Location = new Point(x, y);       
        }
    }
}
