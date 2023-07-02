using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }
        Battle battle;
        private void Form1_Load(object sender, EventArgs e)
        {
            button1.Text = "Move";
            button2.Text = "<-";
            button3.Text = "->";
            button4.Text = "Auto";
            button5.Text = "Restart";
            button6.Text = "default";
            button7.Text = "wall on wall";
            button8.Text = "3 in line";

            Proxy.ClearFiles();

            ArmyCreator ac = new RightArmyCreator();
            Army rightarmy = ac.CreateArmy(menu.rightarmy, menu.rightprice);
            ac = new LeftArmyCreator();
            Army leftarmy = ac.CreateArmy(menu.leftarmy, menu.leftprice);

            
            battle = new Battle(leftarmy, rightarmy);
            battle.leftarmy.Reposition(battle.strategy);
            battle.rightarmy.Reposition(battle.strategy);
            battle.Visualize();
          
        }


        private void button1_Click(object sender, EventArgs e)
        {
          battle.Move();
        

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!battle.IsDone()) battle.UndoMove();
  
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!battle.IsDone()) battle.Auto();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!battle.IsDone()) battle.RedoMove();
      
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!battle.IsDone())
            {
                battle.strategy = new Default();
                battle.StrChanged(battle.strategy);
                battle.Reposition();
            }
    
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!battle.IsDone())
            {
                battle.StrChanged(new WoW());
                battle.Reposition();
            }
  
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!battle.IsDone())
            {
                battle.strategy = new ThreeInLine();
                battle.StrChanged(battle.strategy);
                battle.Reposition();
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {  // battle.
            this.Close();
            menu.priceset = false;
            menu.leftprice = 0;
            menu.rightprice = 0;
            menu.rightarmy.Clear();
            menu.leftarmy.Clear();
            menu.customize = false;
  
            
           //menu m = new menu();
           //m.Show();
           //menu.form1.Hide();


        }
    }
}