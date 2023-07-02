using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{

    class UnitCreator
    {
        public static Unit CreateUnit(int id)
        {
            switch (id)
            {
                case (int)Type.LightInfantry: return new LightInfantryProxy(new LightInfantry());
                case (int)Type.HeavyInfantry: return new HeavyInfantryProxy(new HeavyInfantry());
                case (int)Type.Knight: return new KnightProxy(new Knight());
                case (int)Type.Archer: return new ArcherProxy(new Archer());
                case (int)Type.Warlock: return new WarlockProxy(new Warlock());
                case (int)Type.Healer: return new HealerProxy(new Healer());
                case (int)Type.GulyayGorod: return new GGAdapterProxy(new GGAdapter(new GulyayGorod(20, 35, 55)));
            }
            return null;

        }

    }
    public enum ArmyType
    {
        Left,
        Right
    }
    public interface Army
    {
        public List<Unit> unitlist { get; set; }
        public int armyprice { get; set; }
        public ArmyType armytype { get;  }
        public void AddUnit(Unit unit);
        public void RemoveDead();
        public void RemoveUnit(Unit unit);
        public string GetStats();
        public void Reposition(Strategy strategy);

    }
    public abstract class ArmyCreator
    {
        public abstract Army CreateArmy(List<int> idlist, int armyprice);

    }

    public class LeftArmyCreator : ArmyCreator
    {
        public override Army CreateArmy(List<int> idlist, int armyprice)
        {
            Army army = new LeftArmy();
            army.armyprice = armyprice;
            foreach (int id in idlist)
            {
                army.AddUnit(UnitCreator.CreateUnit(id));
            }
            return army;
        }
    }
    public class RightArmyCreator : ArmyCreator
    {
        public override Army CreateArmy(List<int> idlist, int armyprice)
        {
            Army army = new RightArmy();
            army.armyprice = armyprice;
            foreach (int id in idlist)
            {
                army.AddUnit(UnitCreator.CreateUnit(id));
            }
            return army;
        }
      


    }
        public class LeftArmy : Army
        {
            public List<Unit> unitlist { get; set; }
            public int armyprice { get; set; }
            public ArmyType armytype { get { return ArmyType.Left; } }
            public LeftArmy()
            {
                unitlist = new List<Unit>();
            }
            public void AddUnit(Unit unit)
            {
                unitlist.Add(unit);
                unit.icon.Image = Properties.Resources.left;
       
                unit.icon.Width = 30;
                unit.icon.Height = 33;
                unit.icon.BackColor = Color.Transparent;
                unit.icon.Location = new Point(361 - unitlist.IndexOf(unit) * 27, 229);
                menu.form1.Controls.Add(unit.icon);
                unit.icon.BringToFront();
            }
            public void RemoveUnit(Unit unit)
            {  int t = unitlist.IndexOf(unit);

                unitlist.RemoveAt(t);
            }
            public void Reposition(Strategy strategy)

            {  
             if (strategy is Default)
            {
                int i = 0;
                foreach (Unit unit in unitlist)
                {
                    unit.icon.Location = new Point(361 - i * 27, 229);
                    i++;
                }
            }
             else if (strategy is WoW)
            {
                int i = 0;
                foreach (Unit unit in unitlist)
                {
                    unit.icon.Location = new Point(361, 40+i*27);
                    i++;
                }
            }
             else if (strategy is ThreeInLine)
            {
                int del = unitlist.Count / 3;
                int j = 0;
                for (int i = 0; i < del; i++)
                { unitlist[i].icon.Location = new Point(361 - j * 27, 202); j++; }
                j = 0;
                for (int i = del; i < del * 2; i++)
                { unitlist[i].icon.Location = new Point(361 - j * 27, 229); j++; }
                j = 0;
                for (int i = del*2; i < unitlist.Count(); i++)
                { unitlist[i].icon.Location = new Point(361 - j * 27, 256); j++; }
            }

        }
            public void RemoveDead()
            {
            foreach (Unit unit in unitlist)
                if (unit.IsDead) unit.icon.Visible = false;
            unitlist.RemoveAll(x => x.IsDead);
            }
            public string GetStats()
            {
                string s = " ";
                foreach (Unit unit in unitlist)
                    s += $"{unit.type}/{unit.hp}/{unit.atk}/{unit.def}\n";
                return s;
            }

        }
        public class RightArmy : Army
        {
            public List<Unit> unitlist { get; set; }
            public int armyprice { get; set; }
        public ArmyType armytype { get { return ArmyType.Right; } }
        public RightArmy()
            {
                unitlist = new List<Unit>();
            }
            public void AddUnit(Unit unit)
            {
                unitlist.Add(unit);
                unit.icon.Image = Properties.Resources.right2;
                unit.icon.Width = 30;
                unit.icon.Height = 33;
            unit.icon.BackColor = Color.Transparent;
                unit.icon.Location = new Point(398 + unitlist.IndexOf(unit) * 27, 229);
                unit.icon.BringToFront();
                menu.form1.Controls.Add(unit.icon);

            }
            public void RemoveUnit(Unit unit)
            {
                unitlist.RemoveAt(unitlist.IndexOf(unit));
            }
            public void Reposition(Strategy strategy)
            {
                if (strategy is Default)
                {
                    int i = 0;
                    foreach (Unit unit in unitlist)
                    {

                        unit.icon.Location = new Point(398 + i * 27, 229);
                        unit.icon.BringToFront();
                        i++;
                    }
                }
            else if (strategy is WoW)
            {
                int i = 0;
                foreach (Unit unit in unitlist)
                {
                    unit.icon.Location = new Point(398, 40 + i * 27);
                    i++;
                }
            }
            else if (strategy is ThreeInLine)
            {
                int del = unitlist.Count / 3;
                int j = 0;
                for (int i = 0; i < del; i++)
                { unitlist[i].icon.Location = new Point(398 + j * 27, 202); j++; }
                j = 0;
                for (int i = del; i < del * 2; i++)
                { unitlist[i].icon.Location = new Point(398 + j * 27, 229); j++; }
                j = 0;
                for (int i = del * 2; i < unitlist.Count(); i++)
                { unitlist[i].icon.Location = new Point(398 + j * 27, 256); j++; }
            }
        }

            public string GetStats()
            {
                string s = " ";
                foreach (Unit unit in unitlist)
                    s += $"{unit.type}/{unit.hp}/{unit.atk}/{unit.def}\n";
                return s;
            }
            public void RemoveDead()
            {   foreach (Unit unit in unitlist)
                if (unit.IsDead) unit.icon.Visible = false;
                unitlist.RemoveAll(x => x.IsDead);
            }

        }


    }

   

        
    

