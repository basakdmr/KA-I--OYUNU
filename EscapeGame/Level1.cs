using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EscapeLibrary.Concrete;
using EscapeLibrary.Enum;

namespace EscapeGame
{
    public partial class Level1 : Form
    {
        
        //Game.cs sınıfı farklı bir projenin içinde olduğu için, öncelikle Level1.cs içine Tanımlanmıştır.
        //Tüm bileşenler oluştuktan sonra formun boyutu tanımlandı.
        // _game nesnesi, Game.cs sınıfının constructorudur. (Yapıcı method) Bu yapıcı method sayesinde, Game.cs içinde tanımladığımız alanları ile (fields) Level1.cs içinde bağlantı kurabildim.
        // Game.cs içinde,TİMER sınıfından türeyen _elapsedTimer isimli bir timer ve TİMESPAN özelliğinde ElapsedTime isimli bir nesne bulunmaktadır. ElapsedTime, her saniye geçen süreyi kendi içinde bir arttırır.
        // Constructor sayesinde ElapsedTime içindeki değeri, EventHandler yardımı ile her saniye tetiklenen bir event(Game_ElapsedTimeChanged) ile bağdaştırdım ve Form içerisindeki LabelTime' ın text özelliğine yazdırdım.
        // Game isimli interface içerisinde Move isimli bir method bulunmaktadır. Bu method, parametre olarak Directory sınıfının içerisindeki enum'un değerlerinden birini alır.
        // Enum' un içerisindeki her bir değere karşılık Game.cs sınıfının içerisinde methodlar vardır.
        // e.Keycode ifadesi ile klavyeden basılan tuşların sayısal karşılıklarını elde ederiz. Bu tuşlardan istediklerimizi (switch-case) ile kontrol edip istediğimizi yapabiliriz. 
        // this.keyPreview ifadesi formun load event'ine yazılmıştır.Yani form yüklenir yüklenmez, true değeri sayesinde klavyeden basılan tuşları dinleyecektir.       

        private readonly Game _game;
        public Level1()
        {
            InitializeComponent();
            this.Size = new Size(1050,450);
            labelTime.BringToFront();
            _game = new Game(panelMain,panelInfo, levelLabel, labelHealt, labelScore, labelPlayer, textPlayer, menuPanel, isCont);
            _game.ElapsedTimeChanged += Game_ElapsedTimeChanged;           
        }

        private void Level1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    _game.Start();
                    break;
                case Keys.Right:
                    _game.Move(Direction.right);                   
                    break;
                case Keys.Left:
                    _game.Move(Direction.left);
                    break;
                case Keys.Up:
                    _game.Move(Direction.up);
                    break;
                case Keys.Down:
                    _game.Move(Direction.down);
                    break;
                case Keys.P:
                    _game.Pause();
                    break;
            }
        }    
        private void Game_ElapsedTimeChanged(object sender, EventArgs e)
        {
            labelTime.Text = _game.ElapsedTime.ToString(@"m\:ss");
        }
        private void Level1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;       
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
