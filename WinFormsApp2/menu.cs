using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game

{


    public partial class menu : Form
    {
        public static int price;
        public static bool priceset;
        public static bool customize;
        public static int leftprice;
        public static int rightprice;
        public Dictionary<int, int> pricelist;
        public static List<int> leftarmy;
        public static List<int> rightarmy;
        public bool left;
        public static Form form1;
        
        public menu()
        {
            InitializeComponent();
            form1 = new Form1();
            pricelist = new Dictionary<int, int>();
            leftarmy = new List<int>();
            rightarmy = new List<int>();
            Pricelist(pricelist);
            textBox1.Text = "55";
            button1.Text = "Confirm price";
            button2.Text = "Randomize";
            button3.Text = "Customize";
            button4.Text = "Add";
            button5.Text = "Add";
            button6.Text = "Delete";
            button7.Text = "Delete";
            button8.Text = "Confirm";
            textBox2.AutoSize = false;
            textBox3.AutoSize = false;
            textBox2.Width = 50;
            textBox3.Width = 50;

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            textBox1.Text = trackBar1.Value.ToString();
        }

        private void menu_Load(object sender, EventArgs e)
        {

        }




        private void button1_Click(object sender, EventArgs e)
        {
            price = trackBar1.Value;
            priceset = true;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (priceset)
            {
                int price = 0;
                Randomize(price, leftarmy, left = true);
                price = 0;
                Randomize(price, rightarmy, left = false);
                if (rightarmy.Count > 0 && leftarmy.Count > 0)
                {
                    form1 = new Form1();
                    form1.Show();
                }
                //this.Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            customize = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (priceset && customize && comboBox1.SelectedIndex > -1 && leftarmy.Count < 13)
            {
                if (pricelist[comboBox1.SelectedIndex] + leftprice <= price)
                {
                    leftarmy.Add(comboBox1.SelectedIndex);
                    leftprice += pricelist[comboBox1.SelectedIndex];
                    comboBox4.Items.Add((Type)comboBox1.SelectedIndex);
                    textBox3.Text = leftprice.ToString();
                }
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        public void Pricelist(Dictionary<int, int> list)
        {
            LightInfantry li_ex = new LightInfantry();
            HeavyInfantry hi_ex = new HeavyInfantry();
            Knight k_ex = new Knight();
            Healer h_ex = new Healer();
            Archer a_ex = new Archer();
            Warlock w_ex = new Warlock();
            GGAdapter gg_ex = new GGAdapter(new GulyayGorod(25, 30, 55));
            list.Add(0, li_ex.price);
            list.Add(1, hi_ex.price);
            list.Add(2, k_ex.price);
            list.Add(3, h_ex.price);
            list.Add(4, a_ex.price);
            list.Add(5, w_ex.price);
            list.Add(6, gg_ex.price);
        }



        public void Randomize(int prc, List<int> idlist, bool left)
        {
            idlist.Clear();
            Random rnd = new Random();
            int n = 0;

            while (prc < price)
            {
                n = rnd.Next(0, 6);
                idlist.Add(n);
                prc += pricelist[n];

            }
            if (prc > price)
            {

                prc -= pricelist[idlist.Last()];
                idlist.RemoveAt(idlist.Count - 1);
            }
            if (left) leftprice = prc;
            else rightprice = prc;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (leftarmy.Count > 0 && rightarmy.Count > 0)
            {
                form1 = new Form1();
                form1.Show();
                //this.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (priceset && customize && comboBox2.SelectedIndex > -1 && rightarmy.Count < 13)
            {
                if (pricelist[comboBox2.SelectedIndex] + rightprice <= price)
                {
                    rightarmy.Add(comboBox2.SelectedIndex);
                    rightprice += pricelist[comboBox2.SelectedIndex];
                    comboBox3.Items.Add((Type)comboBox2.SelectedIndex);
                    textBox2.Text = rightprice.ToString();
                  
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex > -1)
            {
                
                rightprice -= pricelist[rightarmy[comboBox3.SelectedIndex]];
             
                rightarmy.Remove(rightarmy[comboBox3.SelectedIndex]);
                comboBox3.Text = " ";
                comboBox3.Items.RemoveAt(comboBox3.SelectedIndex);
                textBox2.Text = rightprice.ToString();
               
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            if (comboBox4.SelectedIndex > -1)
            {
                
                leftprice -= pricelist[leftarmy[comboBox4.SelectedIndex]];
                leftarmy.Remove(leftarmy[comboBox4.SelectedIndex]);
                comboBox4.Text = " ";
                comboBox4.Items.RemoveAt(comboBox4.SelectedIndex);
                textBox3.Text = leftprice.ToString();
            }
        }
    }
}
