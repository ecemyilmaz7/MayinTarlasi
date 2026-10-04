using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mayin_Tarlasi
{
    public partial class Form1 : Form
    {
        Button[,] kutular;
        bool[,] mayinliMi;
        bool[,] bayrakliMi;
        bool[,] acikKutular;
        
        int satirUzunlugu = 9;
        int sutunUzunlugu = 9;
        int mayinAdedi = 10;
        
        bool ilkBasma = true;
        bool oyunBittiMi = false;

        NumericUpDown nudSatir;
        NumericUpDown nudSutun;
        NumericUpDown nudMayin;
        Button btnYenile;
        Button btnYuz;

        public Form1()
        {
            InitializeComponent();
            this.Load += new EventHandler(Form1_Load);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Mayın Tarlası - GP2";
            this.Width = 600;
            this.Height = 500;
            
            Panel solPanel = new Panel();
            solPanel.Dock = DockStyle.Left;
            solPanel.Width = 120;
            solPanel.BackColor = Color.LightSteelBlue;
            this.Controls.Add(solPanel);

            btnYuz = new Button();
            btnYuz.Text = "😊";
            btnYuz.Font = new Font("Arial", 24);
            btnYuz.Location = new Point(30, 20);
            btnYuz.Size = new Size(60, 60);
            btnYuz.Click += new EventHandler(btnYenile_Click);
            solPanel.Controls.Add(btnYuz);

            Label lblSatir = new Label() { Text = "Satır:", Location = new Point(10, 100), Width = 100 };
            solPanel.Controls.Add(lblSatir);
            nudSatir = new NumericUpDown() { Value = 9, Minimum = 5, Maximum = 30, Location = new Point(10, 120), Width = 80 };
            solPanel.Controls.Add(nudSatir);

            Label lblSutun = new Label() { Text = "Sütun:", Location = new Point(10, 160), Width = 100 };
            solPanel.Controls.Add(lblSutun);
            nudSutun = new NumericUpDown() { Value = 9, Minimum = 5, Maximum = 30, Location = new Point(10, 180), Width = 80 };
            solPanel.Controls.Add(nudSutun);

            Label lblMayin = new Label() { Text = "Mayın:", Location = new Point(10, 220), Width = 100 };
            solPanel.Controls.Add(lblMayin);
            nudMayin = new NumericUpDown() { Value = 10, Minimum = 1, Maximum = 900, Location = new Point(10, 240), Width = 80 };
            solPanel.Controls.Add(nudMayin);

            btnYenile = new Button() { Text = "Yenile", Location = new Point(10, 280), Width = 80, Height = 30 };
            btnYenile.Click += new EventHandler(btnYenile_Click);
            solPanel.Controls.Add(btnYenile);

            OyunuKur();
        }

        private void btnYenile_Click(object sender, EventArgs e)
        {
            satirUzunlugu = Convert.ToInt32(nudSatir.Value);
            sutunUzunlugu = Convert.ToInt32(nudSutun.Value);
            mayinAdedi = Convert.ToInt32(nudMayin.Value);

            int maxMayin = (satirUzunlugu * sutunUzunlugu) - 1;
            
            if (mayinAdedi > maxMayin)
            {
                mayinAdedi = maxMayin;
                nudMayin.Value = maxMayin;
            }

            OyunuKur();
        }

        private void OyunuKur()
        {
            if (kutular != null)
            {
                for (int i = 0; i < kutular.GetLength(0); i++)
                {
                    for (int j = 0; j < kutular.GetLength(1); j++)
                    {
                        this.Controls.Remove(kutular[i, j]);
                    }
                }
            }

            kutular = new Button[satirUzunlugu, sutunUzunlugu];
            mayinliMi = new bool[satirUzunlugu, sutunUzunlugu];
            bayrakliMi = new bool[satirUzunlugu, sutunUzunlugu];
            acikKutular = new bool[satirUzunlugu, sutunUzunlugu];

            ilkBasma = true;
            oyunBittiMi = false;
            btnYuz.Text = "😊";

            int baslangicX = 130;
            int baslangicY = 10;

            for (int satir = 0; satir < satirUzunlugu; satir++)
            {
                for (int sutun = 0; sutun < sutunUzunlugu; sutun++)
                {
                    Button yeniKutu = new Button();
                    yeniKutu.Width = 30;
                    yeniKutu.Height = 30;
                    yeniKutu.Location = new Point(baslangicX + (sutun * 30), baslangicY + (satir * 30));
                    yeniKutu.Font = new Font("Arial", 10, FontStyle.Bold);
                    yeniKutu.BackColor = Color.LightGray;

                    yeniKutu.MouseDown += new MouseEventHandler(Buton_Tiklandi);

                    kutular[satir, sutun] = yeniKutu;
                    this.Controls.Add(yeniKutu);
                }
            }

            this.Width = 130 + (sutunUzunlugu * 30) + 40;
            this.Height = 10 + (satirUzunlugu * 30) + 60;
            
            if (this.Height < 360) this.Height = 360;
        }

        private void Buton_Tiklandi(object sender, MouseEventArgs e)
        {
            if (oyunBittiMi == true) return;

            Button basilan = (Button)sender;
            int satir = 0;
            int sutun = 0;

            for (int i = 0; i < satirUzunlugu; i++)
            {
                for (int j = 0; j < sutunUzunlugu; j++)
                {
                    if (kutular[i, j] == basilan)
                    {
                        satir = i;
                        sutun = j;
                    }
                }
            }

            if (acikKutular[satir, sutun] == true) return;

            if (e.Button == MouseButtons.Right)
            {
                if (bayrakliMi[satir, sutun] == false)
                {
                    bayrakliMi[satir, sutun] = true;
                    basilan.Text = "🚩";
                }
                else
                {
                    bayrakliMi[satir, sutun] = false;
                    basilan.Text = "";
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (bayrakliMi[satir, sutun] == true) return;

                if (ilkBasma == true)
                {
                    ilkBasma = false;
                    MayinlariDiz(satir, sutun);
                }

                KutuyuAc(satir, sutun);

                if (oyunBittiMi == false)
                {
                    KazanmaKontrolu();
                }
            }
        }

        private void KazanmaKontrolu()
        {
            int acilanSayisi = 0;
            
            for (int i = 0; i < satirUzunlugu; i++)
            {
                for (int j = 0; j < sutunUzunlugu; j++)
                {
                    if (acikKutular[i, j] == true)
                    {
                        acilanSayisi++;
                    }
                }
            }

            int hedefKutuSayisi = (satirUzunlugu * sutunUzunlugu) - mayinAdedi;

            if (acilanSayisi == hedefKutuSayisi)
            {
                oyunBittiMi = true;
                btnYuz.Text = "😎";
                MessageBox.Show("Tebrikler, oyunu kazandınız!", "Kazandın", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MayinlariDiz(int ilkSatir, int ilkSutun)
        {
            Random rastgele = new Random();
            int yerlesenSayisi = 0;

            while (yerlesenSayisi < mayinAdedi)
            {
                int rSatir = rastgele.Next(0, satirUzunlugu);
                int rSutun = rastgele.Next(0, sutunUzunlugu);

                if (mayinliMi[rSatir, rSutun] == false)
                {
                    if (rSatir != ilkSatir || rSutun != ilkSutun)
                    {
                        mayinliMi[rSatir, rSutun] = true;
                        yerlesenSayisi++;
                    }
                }
            }
        }

        private void KutuyuAc(int satir, int sutun)
        {
            if (satir < 0 || satir >= satirUzunlugu) return;
            if (sutun < 0 || sutun >= sutunUzunlugu) return;
            if (acikKutular[satir, sutun] == true || bayrakliMi[satir, sutun] == true) return;

            acikKutular[satir, sutun] = true;
            Button b = kutular[satir, sutun];
            b.BackColor = Color.WhiteSmoke;
            
            if (mayinliMi[satir, sutun] == true)
            {
                OyunuKaybettin(b);
                return;
            }

            int sayi = EtraftakiMayinlariBul(satir, sutun);

            if (sayi > 0)
            {
                b.Text = sayi.ToString();
                if (sayi == 1) b.ForeColor = Color.Blue;
                if (sayi == 2) b.ForeColor = Color.Green;
                if (sayi == 3) b.ForeColor = Color.Red;
            }
            else
            {
                KutuyuAc(satir - 1, sutun - 1); 
                KutuyuAc(satir - 1, sutun);     
                KutuyuAc(satir - 1, sutun + 1); 
                
                KutuyuAc(satir, sutun - 1);     
                KutuyuAc(satir, sutun + 1);     
                
                KutuyuAc(satir + 1, sutun - 1); 
                KutuyuAc(satir + 1, sutun);     
                KutuyuAc(satir + 1, sutun + 1); 
            }
        }

        private int EtraftakiMayinlariBul(int satir, int sutun)
        {
            int sayac = 0;
            
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int kSatir = satir + i;
                    int kSutun = sutun + j;

                    if (kSatir >= 0 && kSatir < satirUzunlugu && kSutun >= 0 && kSutun < sutunUzunlugu)
                    {
                        if (mayinliMi[kSatir, kSutun] == true)
                        {
                            sayac++;
                        }
                    }
                }
            }
            return sayac;
        }

        private void OyunuKaybettin(Button basilanBomba)
        {
            oyunBittiMi = true;
            btnYuz.Text = "😠";
            basilanBomba.BackColor = Color.Red;

            for (int i = 0; i < satirUzunlugu; i++)
            {
                for (int j = 0; j < sutunUzunlugu; j++)
                {
                    if (mayinliMi[i, j] == true)
                    {
                        kutular[i, j].Text = "💣";
                    }
                }
            }

            MessageBox.Show("Mayına bastınız! Oyunu kaybettiniz.", "Oyun Bitti", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
