using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public class State {
        public List<Unit> UnitList;
        public List<int> lHP;
        public List<int> rHP;
        public List<int> lDef;
        public List<int> rDef;
        public List<int> lAtk;
        public List<int> rAtk;
        public List <Unit> UnitList2;
        public bool isleftturn;
        public Strategy str;


        public State(List<Unit> larmy, List<Unit> rarmy, bool leftturn, Strategy strategy)
        {
            UnitList = new List<Unit>(); UnitList2 = new List<Unit>();
            lHP = new List<int>(); rHP = new List<int>();
            lDef = new List<int>(); lAtk = new List<int>();
            rDef = new List<int>(); rAtk = new List<int>();
            foreach (Unit l in larmy) { UnitList.Add(l); lHP.Add(l.hp); lDef.Add(l.def); lAtk.Add(l.atk); }
            foreach (Unit r in rarmy) { UnitList2.Add(r); rHP.Add(r.hp); rDef.Add(r.def); rAtk.Add(r.atk); }
            str = strategy;
            isleftturn = leftturn;
        }

        
    }

   public interface Command
    {  public bool Undoable { get;  }
        public bool Redoable { get;  }
        public  void Execute();
        public  Strategy Undo();
        public Strategy Redo();
        public void StrategyChanged(Strategy strategy);


    }


    public class Move : Command

    {
       
        public Strategy strategy;
        Army leftarmy;
        Army rightarmy;
        bool isleftturn;
        Stack<State> undo = new Stack<State>();
        Stack<State> redo = new Stack<State>();
       public Stack<State> en {get; set;}
        string logAtk = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "log_st.txt");
        public bool Undoable { get { return undo.Count() > 1; } }
        public bool Redoable { get { return redo.Count() > 0; } }
        public Move(Army leftarmy, Army rightarmy, Strategy strategy, bool ilt)
        {
           this.leftarmy = leftarmy;
            this.rightarmy = rightarmy;
            this.strategy = strategy;
            this.isleftturn = ilt;
            undo.Push(UpdateState(leftarmy, rightarmy, ilt, strategy));
            en = new Stack<State>();
        }

        public void Execute()
        {
            if (isleftturn)
            {
                strategy.Turn(leftarmy, rightarmy);
                isleftturn = false;
            }
            else
            {
                strategy.Turn(rightarmy, leftarmy);
                isleftturn = true;
            }

            undo.Push(UpdateState(leftarmy, rightarmy, isleftturn, strategy));
           
            redo.Clear();
            
        }
        public void StrategyChanged(Strategy strategy)
        {
            this.strategy = strategy;
            undo.Push(UpdateState(leftarmy, rightarmy, isleftturn, strategy));
            en.Push(UpdateState(leftarmy, rightarmy, isleftturn, strategy));
            redo.Clear();
        }
       public Strategy Undo()
        {
           
                redo.Push(undo.Pop());
                leftarmy.unitlist.Clear();
                for (int i = 0; i < undo.Peek().UnitList.Count; i++)
                {
                    leftarmy.unitlist.Add(undo.Peek().UnitList[i]);
                    leftarmy.unitlist[i].hp = undo.Peek().lHP[i];
                leftarmy.unitlist[i].def = undo.Peek().lDef[i];
                leftarmy.unitlist[i].atk = undo.Peek().lAtk[i];
                leftarmy.unitlist[i].icon.Visible=true;
                }
                rightarmy.unitlist.Clear();
                for (int i = 0; i < undo.Peek().UnitList2.Count; i++)
                {
                    rightarmy.unitlist.Add(undo.Peek().UnitList2[i]);
                    rightarmy.unitlist[i].hp = undo.Peek().rHP[i];
                rightarmy.unitlist[i].def = undo.Peek().rDef[i];
                rightarmy.unitlist[i].atk = undo.Peek().rAtk[i];
                rightarmy.unitlist[i].icon.Visible = true;

            }
                isleftturn = undo.Peek().isleftturn;
                if (undo.Peek().str is Default)
                    strategy = new Default();
                else if (undo.Peek().str is WoW)
                    strategy = new WoW();
                else strategy = new ThreeInLine();
                leftarmy.Reposition(strategy);
            leftarmy.RemoveDead();
                rightarmy.Reposition(strategy);
            rightarmy.RemoveDead();
            return strategy;

           
         
        }
        public Strategy Redo()
        {
            
                leftarmy.unitlist.Clear();
                for (int i = 0; i < redo.Peek().UnitList.Count; i++)
                {
                    leftarmy.unitlist.Add(redo.Peek().UnitList[i]);
                    leftarmy.unitlist[i].hp = redo.Peek().lHP[i];
                leftarmy.unitlist[i].def = redo.Peek().lDef[i];
                leftarmy.unitlist[i].atk = redo.Peek().lAtk[i];
         
            }
                rightarmy.unitlist.Clear();
                for (int i = 0; i < redo.Peek().UnitList2.Count; i++)
                {
                    rightarmy.unitlist.Add(redo.Peek().UnitList2[i]);
                    rightarmy.unitlist[i].hp = redo.Peek().rHP[i];
                rightarmy.unitlist[i].def = undo.Peek().rDef[i];
                rightarmy.unitlist[i].atk = undo.Peek().rAtk[i];
                

            }
                isleftturn = redo.Peek().isleftturn;
            if (redo.Peek().str is Default)
                strategy = new Default();
            else if (redo.Peek().str is WoW)
                strategy = new WoW();
            else strategy = new ThreeInLine();
            leftarmy.Reposition(strategy);
            leftarmy.RemoveDead();
                rightarmy.Reposition(strategy);
            rightarmy.RemoveDead();
                undo.Push(redo.Pop());

            return strategy;
       
        }
        public State UpdateState(Army leftarmy, Army rightarmy, bool isleftturn, Strategy strategy)
        {
            State s = new State(leftarmy.unitlist, rightarmy.unitlist, isleftturn, strategy);
            return s;
        }
       
       
    }
}
