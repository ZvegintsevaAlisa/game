using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public abstract class Strategy
    {
        public abstract void Turn(Army left, Army right);

        public abstract List<Unit> InRange(Army thisarmy, int range, Unit unit);
        public abstract List<Unit> InRange_a(Army earmy, Army farmy, int range, Unit unit);

        public void RemoveBuff(Buff unit, Army army)
        {
            Random random = new Random();
            int n = random.Next(0, 100);
                if (n < 25)
                {
                    army.AddUnit(unit.buffable as Unit);
                    army.RemoveUnit(unit);
                }
            

        }
    }

    public class Default : Strategy
    {  
        string logAtk = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "log_s2t.txt");
        public override void Turn(Army left, Army right)
        {  
            left.unitlist[0].TakeDamage(right.unitlist[0], left);
            File.AppendAllText(logAtk, $"{left.unitlist[0]} took from {right.unitlist[0]}");
            if (!left.unitlist[0].IsDead && left.unitlist[0] is Buff b) RemoveBuff(b, left);
            right.unitlist[0].TakeDamage(left.unitlist[0], right);
            File.AppendAllText(logAtk, $"{right.unitlist[0]} took from {left.unitlist[0]}");
            if (!right.unitlist[0].IsDead && right.unitlist[0] is Buff b_) RemoveBuff(b_, right);
            for (int i = 1; i < Math.Max(left.unitlist.Count, right.unitlist.Count); i++)
            {
                if (i < left.unitlist.Count && left.unitlist[i] is ISpecial unit)
                {
                    if (left.unitlist[i].type == Type.Archer) unit.UseSpecialAbility(right, InRange_a(right, left, unit.range, left.unitlist[i]));
                    else unit.UseSpecialAbility(left, InRange(left, unit.range, left.unitlist[i]));
                }
                if (i < right.unitlist.Count && right.unitlist[i] is ISpecial unit_)
                {
                    if (right.unitlist[i].type == Type.Archer) unit_.UseSpecialAbility(left, InRange_a(left, right, unit_.range, right.unitlist[i]));
                    else unit_.UseSpecialAbility(right, InRange(right, unit_.range, right.unitlist[i]));
                }
            }
        }

        public override List<Unit> InRange(Army f_army, int range, Unit unit)
        {
            List<Unit> inrange = new List<Unit>();
            int ind = f_army.unitlist.IndexOf(unit);
            if (ind + range < f_army.unitlist.Count)
                inrange.AddRange(f_army.unitlist.GetRange(ind, range));
            else inrange.AddRange(f_army.unitlist.GetRange(ind, f_army.unitlist.Count - ind));
            if (ind - range >= 0)
                inrange.AddRange(f_army.unitlist.GetRange(ind - range, range));
            else inrange.AddRange(f_army.unitlist.GetRange(0, ind - 1));
            return inrange;
        }

        public override List<Unit> InRange_a(Army e_army, Army f_army, int range, Unit unit)
        {
            List<Unit> inrange = new List<Unit>();
            int ind = f_army.unitlist.IndexOf(unit);
            if (range > ind)
            {
                if (range - ind >= e_army.unitlist.Count)
                    inrange.AddRange(e_army.unitlist.GetRange(0, e_army.unitlist.Count));
                else
                    inrange.AddRange(e_army.unitlist.GetRange(0, range - ind));
            }
            return inrange;
        }

       
    }

    public class WoW : Strategy
    {
       
        public override void Turn(Army left, Army right)
        {
            for (int i = 0; i < Math.Min(left.unitlist.Count(), right.unitlist.Count()); i++)
            {
                left.unitlist[i].TakeDamage(right.unitlist[i], left);
                if (!left.unitlist[i].IsDead && left.unitlist[i] is Buff b) RemoveBuff(b, left);
                right.unitlist[i].TakeDamage(left.unitlist[i], right);
                if (!right.unitlist[i].IsDead && right.unitlist[i] is Buff b_) RemoveBuff(b_, right);
            }
            if (left.unitlist.Count() > right.unitlist.Count())
            {
                for (int i = 1; i < left.unitlist.Count() - right.unitlist.Count(); i++)
                {
                    if (left.unitlist[left.unitlist.Count() - i-1] is ISpecial unit)
                    {
                        if (left.unitlist[i].type == Type.Archer) unit.UseSpecialAbility(right, InRange_a(right, left, unit.range, left.unitlist[i]));
                        else unit.UseSpecialAbility(left, InRange(left, unit.range, left.unitlist[i]));
                    }
                }
            }
            else
            {
                for (int i = 1; i < right.unitlist.Count() - left.unitlist.Count(); i++)
                {
                    if (right.unitlist[right.unitlist.Count() - i-1] is ISpecial unit)
                    {
                        if (right.unitlist[i].type == Type.Archer) unit.UseSpecialAbility(left, InRange_a(left, right, unit.range, right.unitlist[i]));
                        else unit.UseSpecialAbility(left, InRange(right, unit.range, right.unitlist[i]));
                    }
                }
            }
        }
        public override List<Unit> InRange(Army f_army, int range, Unit unit)
        {
         List<Unit> inrange = new List<Unit>();
        int ind = f_army.unitlist.IndexOf(unit);
            if (ind + range<f_army.unitlist.Count)
                inrange.AddRange(f_army.unitlist.GetRange(ind, range));
            else inrange.AddRange(f_army.unitlist.GetRange(ind, f_army.unitlist.Count - ind));
            if (ind - range >= 0)
                inrange.AddRange(f_army.unitlist.GetRange(ind - range, range));
            else inrange.AddRange(f_army.unitlist.GetRange(0, ind - 1));
            return inrange;
        }
        public override List<Unit> InRange_a(Army e_army, Army f_army, int range, Unit unit)
        {
            List<Unit> inrange = new List<Unit>();
            int ind = f_army.unitlist.IndexOf(unit);
            if (ind + range < e_army.unitlist.Count)
                inrange.AddRange(e_army.unitlist.GetRange(ind, range));
            else inrange.AddRange(e_army.unitlist.GetRange(ind, e_army.unitlist.Count - ind));
            if (ind - range >= 0)
                inrange.AddRange(e_army.unitlist.GetRange(ind - range, range));
            else inrange.AddRange(e_army.unitlist.GetRange(0, ind - 1));
            return inrange;
        }

        
    }

    public class ThreeInLine: Strategy
    {
       
        public override void Turn(Army left, Army right)
        { int lc = left.unitlist.Count();
            int rc = right.unitlist.Count();
            left.unitlist[0].TakeDamage(right.unitlist[0], left);
            if (!left.unitlist[0].IsDead && left.unitlist[0] is Buff b) RemoveBuff(b, left);
            right.unitlist[0].TakeDamage(left.unitlist[0], right);
            if (!right.unitlist[0].IsDead && right.unitlist[0] is Buff b_) RemoveBuff(b_, right);
            left.unitlist[lc/3].TakeDamage(right.unitlist[rc/3], left);
            if (!left.unitlist[lc/3].IsDead && left.unitlist[lc/3] is Buff b1) RemoveBuff(b1, left);
            right.unitlist[rc/3].TakeDamage(left.unitlist[lc/3], right);
            if (!right.unitlist[rc/3].IsDead && right.unitlist[rc/3] is Buff b1_) RemoveBuff(b1_, right);
            left.unitlist[2*(lc/3)].TakeDamage(right.unitlist[2*(rc/3)], left);
            if (!left.unitlist[2*(lc/3)].IsDead && left.unitlist[0] is Buff b2) RemoveBuff(b2, left);
            right.unitlist[2*(rc/3)].TakeDamage(left.unitlist[2*(lc/3)], right);
            if (!right.unitlist[2*(rc/3)].IsDead && right.unitlist[2*(rc/3)] is Buff b2_) RemoveBuff(b2_, left);

            for (int i = 1; i < Math.Max(left.unitlist.Count, right.unitlist.Count); i++)
            {
                if (i < left.unitlist.Count && i!=lc/3 && i != 2 * (lc / 3) && left.unitlist[i] is ISpecial unit)
                {
                    if (left.unitlist[i].type == Type.Archer) unit.UseSpecialAbility(right, InRange_a(right, left, unit.range, left.unitlist[i]));
                    else unit.UseSpecialAbility(left, InRange(left, unit.range, left.unitlist[i]));
                }
                if (i < right.unitlist.Count && i!=rc/3 && i!=2*(rc/3)&& right.unitlist[i] is ISpecial unit_)
                {
                    if (right.unitlist[i].type == Type.Archer) unit_.UseSpecialAbility(left, InRange_a(left, right, unit_.range, right.unitlist[i]));
                    else unit_.UseSpecialAbility(right, InRange(right, unit_.range, right.unitlist[i]));
                }
            }

        }
        public override List<Unit> InRange(Army f_army, int range, Unit unit)
        {
            List<Unit> inrange = new List<Unit>();
            int ind = f_army.unitlist.IndexOf(unit);
            int ac = f_army.unitlist.Count()/3;
            foreach (Unit u in f_army.unitlist)
                inrange.Add(u);
            inrange.RemoveAll(u=>f_army.unitlist.IndexOf(u)<ind-range);
            inrange.RemoveAll(u => f_army.unitlist.IndexOf(u) > ind + range);
            inrange.RemoveAll(u => f_army.unitlist.IndexOf(u) < ac+ind - range);
            inrange.RemoveAll(u => f_army.unitlist.IndexOf(u) > ac+ind + range);
            inrange.RemoveAll(u => f_army.unitlist.IndexOf(u) < ind-ac - range);
            inrange.RemoveAll(u => f_army.unitlist.IndexOf(u) > ind-ac + range);



            return inrange;
        }


        public override List<Unit> InRange_a(Army e_army, Army f_army, int range, Unit unit)
        {
            List<Unit> inrange = new List<Unit>();
            int ind = f_army.unitlist.IndexOf(unit);
            int ac = f_army.unitlist.Count() / 3;
            int eac = e_army.unitlist.Count() / 3;
            int r = 0;
            if (ind < ac) r = range - ind;
            if (ind<2*ac) r = range - 2*ind;
            if (ind < f_army.unitlist.Count()) r = range - 3 * ac;
            foreach (Unit u in e_army.unitlist)
                inrange.Add(u);
            inrange.RemoveAll(u => e_army.unitlist.IndexOf(u) > r-1 && e_army.unitlist.IndexOf(u) < eac);
            inrange.RemoveAll(u => e_army.unitlist.IndexOf(u) > eac-1+r && e_army.unitlist.IndexOf(u) < eac*2);
            inrange.RemoveAll(u => e_army.unitlist.IndexOf(u) > eac*2-1+r && e_army.unitlist.IndexOf(u) < e_army.unitlist.Count());



            return inrange;
        }

       
    }

}
