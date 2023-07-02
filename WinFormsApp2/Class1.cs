using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class Battle
    {
        public Army leftarmy;
        public Army rightarmy;
      

        public bool isleftturn = true;
       public  Command command;
       public Strategy strategy;

        TextBox left= new TextBox();
        TextBox right= new TextBox();

        public Battle (Army leftarmy, Army rightarmy)
        {
            this.leftarmy = leftarmy;
            this.rightarmy = rightarmy;
            strategy = new Default();
            command= new Move(leftarmy, rightarmy, strategy, isleftturn);

            left.AutoSize = false;
            right.AutoSize = false;
            left.Location = new Point(40, 250);
            left.Width = 150;
            left.Height = 160;
            left.Multiline = true;
            left.ReadOnly = true;
            right.Location = new Point(590, 250);
            right.Width = 150;
            right.Height = 160;
            right.ReadOnly = true;          
            right.Multiline = true;
            menu.form1.Controls.Add(left);
            menu.form1.Controls.Add(right);
        }

        public bool IsDone()
        {
            if(leftarmy.unitlist.Count == 0 || rightarmy.unitlist.Count == 0)
                return true;
            return false;
        }
        public void Move()
        {   

           
            if (!this.IsDone())
            {
                
                command.Execute();
                leftarmy.RemoveDead();
                rightarmy.RemoveDead();
                leftarmy.Reposition(strategy);
                rightarmy.Reposition(strategy);
                Visualize();

            }
           else if (rightarmy.unitlist.Any()) { VictoryMessage(false); }
            else { VictoryMessage(true); }


        } 
        
        
        public void Auto()
        { 
            while (!IsDone())
            {
                
              
                command.Execute();
                leftarmy.RemoveDead();
                rightarmy.RemoveDead();

                Reposition();
                Visualize();
            }
            if (rightarmy.unitlist.Any()) { VictoryMessage(false); }
            else { VictoryMessage(true); }
   

        }

        public void Reposition()
        {
            leftarmy.Reposition(strategy);
            rightarmy.Reposition(strategy);
        }
        public void StrChanged(Strategy strategy)
        {
            this.strategy = strategy;
            command.StrategyChanged(strategy);
        }

        public void UndoMove()
        {
            if (command.Undoable)
            strategy = command.Undo();
            Reposition();
            Visualize();
 
        }
        public void RedoMove()
        { if (command.Redoable)
           strategy = command.Redo();
            Reposition();
            Visualize();

        }



        public void VictoryMessage(bool leftwon)
        {
            Label label = new Label();
  
            label.ForeColor = Color.White;
            label.BackColor = Color.Transparent;

            if (leftwon)
            {
                label.Text = "Left won";
                label.Location = new Point(50, 50);
            }
            else
            {
                label.Text = "Right won";
                label.Location = new Point(450, 50);
            }
            menu.form1.Controls.Add(label);
        }

        public void Visualize()
        {
            left.Text = leftarmy.GetStats();
            right.Text = rightarmy.GetStats();
           
        }
            // leftarmy.RemoveDead();
            //public void Operation()
            //{
            //    sb.Append($"{leftarmy.unitlist.Count} + {states[move - 1].UnitList.Count}");
            //    leftarmy.unitlist.Clear();
            //    for (int i = 0; i < states[move - 1].UnitList.Count; i++) {
            //        leftarmy.unitlist.Add(states[move - 1].UnitList[i]);
            //        leftarmy.unitlist.Last().hp = states[move - 1].lHP[i];

            //    }
            //    rightarmy.unitlist.Clear();
            //    for (int i = 0; i < states[move - 1].UnitList2.Count; i++)
            //    {
            //        rightarmy.unitlist.Add(states[move - 1].UnitList2[i]);
            //        rightarmy.unitlist.Last().hp = states[move - 1].rHP[i];
            //    }
            //    //  leftarmy.unitlist = states[move - 1].larmy.unitlist;
            //    //  rightarmy = states[move - 1].rarmy;
            //    //foreach (State state in states)
            //    //{
            //    //    state.larmy.GetStats();
            //    //    sb.Append(state.larmy.GetStats() + "\nhhhhhhh");
            //    //}
            //    sb.Append($"");
            //    move--;
            //    leftarmy.Reposition();
            //    rightarmy.Reposition();
            //    File.AppendAllText("log.txt", sb.ToString());
            //    File.AppendAllText(logAtk,"operation:\n" + leftarmy.GetStats() + "-la, \n\n");
            //    File.AppendAllText(logAtk, rightarmy.GetStats() + "-righta, \n\n");
            //}
        }

    }
