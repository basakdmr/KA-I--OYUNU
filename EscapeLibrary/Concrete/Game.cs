using EscapeLibrary.Enum;
using EscapeLibrary.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace EscapeLibrary.Concrete
{
    public class Game : IGame
    {
        // 

        // Oyunun taslağını yazılımsal olarak belirleyebilmek için IGame isimli bir arayüz (interface) oluşturulmuştur. Bu sınıfın içinde çeşitli fonksiyonlar vardır. Bu fonksiyonların Game.cs' ye implemente edilir ve methodlar otomatik olarak sınıfımıza dahil olur.
        // Bu özelliklerden biri de isContinue' dir. Bool veri tipinde olup, oyunda Enter' e basıp oyunu başlatma ve 'P' tuşuna basıp duraklatma (pause) için kullanılacak mantığın temelini oluşturur.

        // Geçen süreyi saydırabilmek için Timer sınıfından _elapsedTimer isimli timer öğesi oluşturulup Interval = 1000 değeri verilerek her 1 saniyede bir çalışacak şekilde ayarlanmıştır.
        // Belirlenen Interval süresinde _elapsedTimer'in Tick Event'i çalışır. Bu event timerin belirlenen Interval süresinde yani her tetiklendiğinde ne olacağını belirlemek için kullanılır.
        // TimeSpan tipinde ElapsedTime isimli bir property ve _elapsedTime isimli bir field tanımlanmıştır. _elapsedTime'ın değeri her değiştiğinde ElapsedTime'in Set özelliği tetiklenir ve değeri değişir.
        // Tetiklenme olayını sürekli yapabilmek için EventHandler tanımlanmıştır.

        // Pause: 'P' tuşuna bastığımızda, isContinue değişkeni true false, false ise true yapar. True değer aldığında timerlerin Start özelliği, false olduğunda ise Timerlerin Stop özelliği çağrılır.
        // StartTimers methodunda params keywordu kullanılarak istediğimiz kadar Timer tipinde değişkeni parametre olarak kullanabiliyoruz.
        // Pause methodunun içinde, bulunduğumuz seviyeye göre istediğimiz timerleri durdurabilmemiz için Switch Case ile level kontrolü yapmaktadır.

        // Game isimli method, program ilk çalıştığında çalışacak olan method yani bir constructor. Bu methoda parametre olarak, oluşturduğumuz field' lara karşılık gelen değişkenler oluşturulmuştur.
        // Böylelikle oluşturduğumuz alanları (field) Form'un kendi sınıfında kullabiliyoruz.

        // Oyun açılıp ENTER ile başlatıldığında Start methodu çalışır. Start methodu sırasıyla CreateBlock, createHero, createTrap, CreateChest methodlarını çalıştırır.
        // CreateBlock: Block isimli, dizi tipinde 50 değer alabilen bir nesne oluşturulmuştur. 0,0 noktasından formun son noktasına kadar, yukarıdan aşağı doğru block oluşturur.
        // Block sınıfı Obj sınıfından inherit edilmiştir(Kalıtım). Obj sınıfı ise pictureBox sınıfından kalıtım almıştır. Yani Block sınıfı da pictureBox özellikleri taşımaktadır.

        // createHero: Hero isimli, pictureBox tipinde bir nesne oluşturulmuştur. Bu öğeye ait özellikler Hero sınıfının içerisinde bulunmaktadır.(Image, Size ...)

        // createTrap: Oyundaki tuzak özelliği için oluşturulmuştur. Oluşturulan blocklardan rastgele 10 tanesinin (Finish ve Start Blockları hariç) name özelliğini "Trap", Image Özelliğini ise rastgele olarak 3 farklı trap resminden birisi yapar.
        // checkTrap: Bu method, Hero isimli nesnemiz her hareket ettiğinde gerçekleşir. "Intersectwith" methodu kullanılarak, hero ile Name özelliğinde Trap içeren(contains) herhangi bir block kesiştiğinde çalışır ve tanımlanan _healt değerini 1 azaltır.

        // createBomb: Oyundaki bomba saldırısı özelliği için oluşturulmuştur. Oluşturulan blocklardan rastgele 10 tanesinin (Finish ve Start Blockları hariç) name özelliğini "Bomb" yapar.
        // _createBombTimer timeri sayesinde, her 3 saniyede bir rastgele alanlarda 10 adet Bomb oluşturur.
        // _restoreBlockTimer timeri sayesinde , her 6 saniyede bir, değişen bomb alanlarını eski haline çevirir ve tekrardan createBomb methodu çalışır böylelikle sonsuz döngü şeklinde bomba oluşur.
        // checkBomb: Bu method, Hero isimli nesnemiz her hareket ettiğinde gerçekleşir. "Intersectwith" methodu kullanılarak, hero ile Name özelliğinde Bomb içeren(contains) herhangi bir block kesiştiğinde çalışır ve tanımlanan _healt değerini 1 azaltır.
        // checkBomb'un checkTrap'tan farkı: Bombalar sürekli yer değiştiriyor. Bu yüzden Hero hareket etmiyorken de  bulunduğu yerde bomba oluşursa _healt değeri 1 azalır.

        // createMinion: Minion isimli, picturebox tipiinde bir nesne oluşturulmuştur. Bu öğeye ait özellikler Minion sınıfının içinde tanımlanmıştır.
        // _createMinionTimer timeri sayesinde her 2 saniyede bir rastgele blockların üzerinde minion oluşmaktadır.
        // moveMinionTimer: Oluşan her minion nesnesi, her saniye bir block sola giderek en son blockta yok olmaktadır(remove).
        // checkMinion: Bu method, Hero isimli nesnemiz her hareket ettiğinde gerçekleşir. "Intersectwith" methodu kullanılarak, hero ile Name özelliğinde Minion içeren(contains) herhangi bir block kesiştiğinde çalışır ve tanımlanan _healt değerini 1 azaltır.


        // createChest: Oluşturulan blocklardan rastgele 1 tanesinin (Finish ve Start hariç) name özelliğini "Chest" yapar ve image özelliğini de dizinde bulunan "Chest.png" ile değiştirir.
        // Intersectwith komutu ile, karakterimiz ile Chest arasında bir kesişme olduğunda CheckChest methodu çalışır
        // CheckChest: Bu method çalıştığında, Random sınıfından 1 ile 2 arasında rastgele bir double sayı oluşturulur. Bu rastgele sayı 1.8' den küçük ise chest'in ismi GChest olarak değiştirilir ve _healt değerini 1 arttırır.
        // Eğer Rastgele oluşturulan değer 0.8' den büyük ise ismi BChest olur ve _healt değerini 1 azaltır. Böylelikle %80 ihtimalle can artırıp %20 ihtimalle can azaltmış olur.

        // Consturctor method: Game (param1, param2.....). Bu yapıcı method, Game.cs içinde oluşturulmuş fieldler ile form arasında ilişkilendirme yapılabilmek amacıyla kurulmuştur.
        // Yapıcı methoda verilen parametreler, Level1.cs içinde kullanılırken formda fiziksel olarak var olan bileşenlerin isimleri yazılarak parametre olurlar.
        // Örneğin Yapıcı methodda _panelMain isimli field, Level1.cs içerisindeki methodda parametre olarak, fiziksel olarak var olan PanelMain öğesi kullanılmıştır.
        
        // _healt, _score, _level, _elapsedTime özelliklerin dinamikleşmesini sağlamak amacıyla prop(property) kullanılmıştır.
        // Örneğin _healt değeri başlangıçta 3 olarak belirlenmiştir. healt isimli bir değişkenin get, set özelliği sayesinde _healt değişkeninde bir değişim olduğunda Set özelliğinin çalışmasıyla güncellenmektedir.
        // Aynı durumlar _score ve _level için de geçerlidir.

        private Random random = new Random();
        #region Timers

        private readonly Timer _elapsedTimer = new Timer { Interval = 1000 };

        private readonly Timer _createBombTimer = new Timer { Interval = 3000 };
        private readonly Timer _restoreBlockTimer = new Timer { Interval = 6000 };

        private readonly Timer _createMinionTimer = new Timer { Interval = 2000 };
        private readonly Timer _moveMinionTimer = new Timer { Interval = 1000 };

        #endregion

        #region Properties

        public bool IsContinue { get; private set; }

     

        public int _level = 1;
        public int _healt = 3;
        public int _score = 0;
        public int healt { // Healt değeri, metodlarda azaltıldığında bu property' nin set özelliği çalışacak ve dinamik bir healt değişimi yapılacak.
            get => _healt;
            set { _healt = value; }
        }

        public int score 
        {   get => _score;
            set
            {
                _score = value;
            }
        }

        #endregion

        #region Fields

        private readonly Panel _panelMain;
        private readonly Panel _panelInfo;
        private readonly Panel _panelMenu;

        private Label _levelLabel;
        private Label _healtLabel;
        private Label _scoreLabel;
        private Label _playerLabel;
        private Label _contLabel;

        private TextBox _playerText;

        private Hero _hero;

        private Block[] _blocks = new Block[50];
        private readonly List<Minion> _minions = new List<Minion>();
        #endregion

        #region EventHandlers
        public event EventHandler BombTimeChanged;
        public event EventHandler MinionTimeChanged;

        public event EventHandler RestoreMinionChanged;
        public event EventHandler RestoreBlockChanged;
 
        public event EventHandler ElapsedTimeChanged;
        #endregion

        #region TimeSpans

        private TimeSpan _elapsedTime;      
        public TimeSpan ElapsedTime 
        {   get => _elapsedTime;
            private set
            {
                _elapsedTime = value;
                ElapsedTimeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion
        public Game(Panel panelmain, Panel panelinfo, Label levelLabel, Label healtLabel, Label scoreLabel, Label playerLabel, TextBox playerText, Panel panelMenu, Label cont) // Panelin içerisine nesne oluşturmak için constructor'a parametre verildi.
        { 
            _panelMain = panelmain;
            _panelInfo = panelinfo;
            _levelLabel = levelLabel;
            _healtLabel = healtLabel;
            _scoreLabel = scoreLabel;
            _playerLabel = playerLabel;
            _playerText = playerText;
            _panelMenu = panelMenu;
            _contLabel = cont;

            _elapsedTimer.Tick += ElapsedTimer_Tick;   

            _createBombTimer.Tick += CreateBombTimer_Tick;
            _createMinionTimer.Tick += CreateMinionTimer_Tick;

            _moveMinionTimer.Tick += MoveMinionTimer_Tick;
            _restoreBlockTimer.Tick += RestoreBlockTimer_Tick; // Bombaların yeri değiştiğinde, bomba düşmeyen blockları eski haline çevirme timeri.      
        }

        private void MoveMinionTimer_Tick(object sender, EventArgs e)
        {
            MoveMinion();
        }

        private void CreateMinionTimer_Tick(object sender, EventArgs e)
        {
            CreateMinion();
        }

        private void RestoreBlockTimer_Tick(object sender, EventArgs e)
        {
            RestartBlock();
        }

        private void ElapsedTimer_Tick(object sender, EventArgs e)
        {
            ElapsedTime += TimeSpan.FromSeconds(1);
            _score = _healt * 500 + (1000 - ElapsedTime.Seconds);
            _scoreLabel.Text = Convert.ToString(_score);
        }

        private void CreateBombTimer_Tick(object sender, EventArgs e)
        {
           CreateBomb();
        }

        public void Move(Direction direction)
        {
            if (!IsContinue) return;
            _hero.DoMove(direction);           
            checkTrap();
            checkBomb();
            checkMinion();
            checkChest();
            checkFinish();
        }
        public void Pause()
        {
            // P tuşuna basıldığında timer çalışıyor ise durdurur, durmuş ise çalıştırır.
            IsContinue = !IsContinue;
            if (IsContinue)
            {
                //_elapsedTimer.Start();
                switch (_level)
                {
                    case 1 :
                        _elapsedTimer.Start();
                        break;
                    case 2:
                        //lvl2TimerStart();
                        startTimers(_createBombTimer, _restoreBlockTimer);
                        break;
                    case 3:
                        //lvl3TimerStart();
                        startTimers(_createMinionTimer, _moveMinionTimer);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                StopTimers();
            }
        }
        public void Start()
        {
            IsContinue = true;
            PlayerMenu();
            _elapsedTimer.Start();
            createHero();
            createBlock();
            createChest();
            createTrap();
        }

        private void PlayerMenu()
        {
            _playerLabel.Text = Convert.ToString(_playerText.Text);
            _panelMenu.Visible = false;
        }

        private void createHero()
        {
            _hero = new Hero(_panelMain.Height, _panelMain.Size)
            {

            };
            _hero.Size = new Size(100, 100);
            _hero.MaximumSize = new Size(100, 100);
            _hero.MinimumSize = new Size(100, 100);
            _hero.SizeMode = PictureBoxSizeMode.StretchImage;
            _hero.BackColor = Color.Transparent;
            _panelMain.Controls.Add(_hero);
            _hero.BringToFront();
        }     
        protected void createBlock()
        {
            int startX = 0;
            int startY = 0;

            int width = 150;
            int height = 150;

            int chest = random.Next(2, 50);
            double randomValue = random.NextDouble();

            // %80 ihtimalle 1, %20 ihtimalle 2 üretme
            int result = (randomValue < 0.8) ? 1 : 2;        

            for (int i = 0; i < 50; i++) // Oluşturulacak block sayısı B221200305 Ömer Kubilay Çelik.
            {                      
                _blocks[i] = new Block(_panelMain.Height, _panelMain.Size)
                {
                    Name = "Block" + (i + 1),
                    Size = new Size(150, 150),
                    Location = new Point(startX, startY)
                };
                _blocks[i].SendToBack();
                _blocks[i].Click += Block_click;              

                if (i == 0)
                {
                    _blocks[i].Name = "StartBlock";
                    _blocks[i].Size = new Size(150, 150);
                    _blocks[i].Location = new Point(startX, startY);
                    _blocks[i].Click += Block_click;
                }

                if (i == 47)
                {
                    _blocks[i].Name = "Finish";
                    _blocks[i].Size = new Size(150, 150);
                    _blocks[i].Location = new Point(startX, startY);
                    _blocks[i].Click += Block_click;
                    _blocks[i].Image = Image.FromFile(@"img\Finish.png");
                }
                _panelMain.Controls.Add(_blocks[i]);

                startY += height; // Sonraki Picturebox'u boşluk bırakmadan oluştur.
                if (startY + height > _panelMain.Height)
                {
                    startY = 0;
                    startX += width;
                }
            }
        }       
        private void createChest()
        {
            while (true)
            {
                int number = random.Next(2, 50);
                if (number != 47 && _blocks[number].Name.StartsWith("Block"))
                {
                    _blocks[number].Name = "Chest";
                    _blocks[number].Image = Image.FromFile(@"img\Chest.png");
                }
                break;
            }          
        }
        Random rnd = new Random();
        public List<int> availableNumbers = Enumerable.Range(1, 50).ToList();
        public void createTrap() // Oluşturulan blocklardan rastgele seçerek isimlerini Trap olarak değiştirir.
        {
            var randomBlocks = _blocks.OrderBy(x => random.Next()).Take(10).ToList();
            foreach (var block in randomBlocks)
            {
                if (block.Name != "Finish" && block.Name != "StartBlock" && block.Name.Contains("Block"))
                {
                    // Bloğun ismini günceller
                    int blockIndex = Array.IndexOf(_blocks, block);
                    block.Name = $"Trap{blockIndex:D2}";

                }
            }
            /*int i = 0;   // Alternatif Trap ekleme yöntemi.
            for (i = 0; i < 10; i++)
            {
                int index = rnd.Next(0, availableNumbers.Count);
                int randomNumber = availableNumbers[index];
                if (randomNumber != 47 && randomNumber != 1)
                {
                    _blocks[randomNumber].Name = "Trap" + randomNumber;
                    checkTrap(); // Karakterin başladığı konumda Trap varsa can kaybeder. 
                }
                availableNumbers.RemoveAt(index);
            }*/
        }
        protected void CreateBomb()
        {         
            RestartBlock();
            var randomBlocks = _blocks.OrderBy(x => random.Next()).Take(10).ToList();
            // Seçilen blockların arka plan resmini "Bomb.png" olarak ayarlar
            foreach (var block in randomBlocks)
            {
                if (block.Name != "Finish" && block.Name != "StartBlock" && block.Name != "Chest")
                {
                    // Bloğun ismini günceller
                    int blockIndex = Array.IndexOf(_blocks, block);
                    block.Name = $"Bomb{blockIndex:D2}";
                    block.Image = Image.FromFile(@"img\Bomb.png");                   
                }                
            }
            checkBomb(); // Karakterin olduğu bölgede bomba oluşursa can kaybeder. 
        }
        private void CreateMinion()
        {
            var minion = new Minion(_panelMain.Size);
            _minions.Add(minion);
            _panelMain.Controls.Add(minion);
            minion.BringToFront();
            minion.Click += Minion_click;           
        }
        
        protected void checkTrap()
        {
            foreach (Control control in _panelMain.Controls)
            {
                if (control is Block && control.Name.Contains("Trap") && _hero.Bounds.IntersectsWith(control.Bounds)) // Hero'nun kesiştiği karenin isminde "Trap" geçiyorsa tuzaktır.
                {                    
                    string pcName = control.Name;
                    string lastTwo = pcName.Substring(4);
                    int a = Convert.ToInt32(lastTwo);
                    Random TrapNumber = new Random();
                    int b = TrapNumber.Next(1, 4);

                    // 1 ile 3 arasında rastgele sayı tutar, üretilen rastgele sayıya göre tuzakların görüntüsü değişir.
                    switch (b)
                    {
                        case 1:
                            _blocks[a].Image = Image.FromFile(@"img\Fire.png");
                            break;
                        case 2:
                            _blocks[a].Image = Image.FromFile(@"img\Trap.png");
                            break;
                        case 3:
                            _blocks[a].Image = Image.FromFile(@"img\Mushroom.png");
                            break;
                        default:
                            break;
                    }
                    LostHealt();
                }
            }
        }
        protected void checkBomb()
        {
            foreach (Control control in _panelMain.Controls)
            {
                if (control is Block && control.Name.Contains("Bomb") && _hero.Bounds.IntersectsWith(control.Bounds)) // Bomba kontrolü
                {
                    LostHealt();
                }
            }
        }
        protected void checkMinion()
        {
            foreach (Control control in _panelMain.Controls)
            {
                if (control is Minion && control.Name.Contains("Minion") && _hero.Bounds.IntersectsWith(control.Bounds)) // Minion kontrolü
                {
                    _hero.BringToFront();
                    LostHealt();
                    break;
                }
            }
        }
        private void checkChest()
        {
            foreach (Control control in _panelMain.Controls)
            {
                if (control is Block && control.Name.Contains("Chest") && _hero.Bounds.IntersectsWith(control.Bounds)) // Minion kontrolü
                {
                    double state = random.NextDouble();
                    int result = (state < 0.8) ? 0 : 1;
                    switch (result)
                    {
                        case 0:
                            control.Name = "GoodChest";
                            UpHealt();
                            control.Name = "GoodC";
                            break;
                        case 1:
                            control.Name = "BadChest";
                            LostHealt();
                            control.Name = "BadC";
                            break;
                    }
                }
            }
        }
        protected void checkFinish()
        {
            foreach (Control control in _panelMain.Controls)
            {
                if (control is Block && control.Name.Contains("Finish") && _hero.Bounds.IntersectsWith(control.Bounds)) // Finish kontrolü
                {
                    if (_level == 3)
                    {
                        Wp();
                    }                    
                    LevelUp();
                }
            }
        }

        private void RestartBlock()
        {
            foreach (var block in _blocks)
            {
                if (block.Name.StartsWith("Bomb"))
                {
                    block.Image = Image.FromFile(@"img\indir(1).png");
                    block.Name = "Block" + (Array.IndexOf(_blocks, block) + 1);
                }
            }
        }     
        private void MoveMinion()
        {          
            for (var i = _minions.Count - 1; i >= 0; i--)
            {
                var minion = _minions[i];
                if (minion.Location.X >= 150 ) // Bu değer 300 yapılırsa ilk sütuna minion ulaşmaz. 
                {
                    minion.Left -= 150;
                }
                else
                {
                    _panelMain.Controls.Remove(minion);
                }
                
            }
            checkMinion();
        }
        private void LostHealt()
        {
            _healt--; // Bu satır kapatılırsa GOD MODE açılır. 
            if (_healt > 0)
            {          
                _healtLabel.Text = Convert.ToString(healt);
            }
            else if (_healt == 0)
            {
                _healtLabel.Text = Convert.ToString(healt);
                //writeScore();
                StopTimers();
                MessageBox.Show("GAME OVER !!!");             
                Application.Exit();
            }
        }
        private void UpHealt()
        {
            _healt++;
            _healtLabel.Text = Convert.ToString(healt);
        }
        protected void LevelUp()
        {
            Destroy();
            _level++;
            writeScore();
            UpHealt();
            switch (_level)
            {
                case 2:
                    createBlock();
                    createHero();
                    createChest();
                    CreateBomb();
                    _createBombTimer.Start();
                    _restoreBlockTimer.Start();
                    _levelLabel.Text = Convert.ToString(_level);
                    _healtLabel.Text = Convert.ToString(healt);
                    break;
                case 3:
                    _createBombTimer.Stop();
                    _restoreBlockTimer.Stop();
                    createBlock();
                    createHero();
                    createChest();
                    CreateMinion();                   
                    _moveMinionTimer.Start();
                    _createMinionTimer.Start();
                    _levelLabel.Text = Convert.ToString(_level);
                    _healtLabel.Text = Convert.ToString(healt);
                    break;
            } 
        }
        private void writeScore()
        {
            _score = _healt * 500 + (1000 - ElapsedTime.Seconds);
            string s = _scoreLabel.Text = Convert.ToString(_score);          
            using (StreamWriter writer = new StreamWriter("Scores.txt",true))
            {
                writer.WriteLine($"Oyuncu -> {_playerLabel.Text} Skor ->{_score}");
            }
        }
        private void Destroy()
        {
            _panelMain.Controls.Remove(_hero);
            BlockRemove();
        }       
        private void BlockRemove()
        {
            for (int i = 0; i < 50; i++)
            {             
                if (_blocks[i].Name.Contains("Trap"))
                {
                    _panelMain.Controls.Remove(_blocks[i]);
                }
                _panelMain.Controls.Remove(_blocks[i]);
            }
        }
        private void Block_click(object sender, EventArgs e)
        {
            Block clicked = (Block)sender;
            MessageBox.Show(clicked.Name + " Clicked!"); // Tıklanan pictureBox'un ismini öğren.
        }
        private void Minion_click(object sender, EventArgs e)
        {
            Minion clicked = (Minion)sender;
            MessageBox.Show(clicked.Name + " Clicked!"); // Tıklanan pictureBox'un ismini öğren.
        }
        private void Wp()
        {
            writeScore();
            StopTimers();
            MessageBox.Show("OYUN BİTTİ !!!", "TEBRİKLER", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);           
            Application.Exit(); // Skor göstergesine gidilecek.
        }
        private void StopTimers()
        {
            _elapsedTimer.Stop();
            _createBombTimer.Stop();
            _restoreBlockTimer.Stop();
            _createMinionTimer.Stop();
            _moveMinionTimer.Stop();
        }
        protected void startTimers(params Timer[] _timer  )
        {
            _elapsedTimer.Start();
            foreach ( Timer timer in _timer )
            {
                timer.Start();
            }
        }
        private void lvl2TimerStart()
        {
            _elapsedTimer.Start();
            _createBombTimer.Start();
            _restoreBlockTimer.Start();          
        }
        private void lvl3TimerStart()
        {
            _createMinionTimer.Start();
            _moveMinionTimer.Start();
        }
    }
}
