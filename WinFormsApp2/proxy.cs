using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace game
{

    public abstract class Proxy : Unit
    {
        public static string logAtk = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "log_atk.txt");
        public static string logSa = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "log_sa.txt");
        public static string logDeath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "log_death.txt");
        public static void ClearFiles()
        {
            File.WriteAllText(logAtk, string.Empty);
            File.WriteAllText(logSa, string.Empty);
            File.WriteAllText(logDeath, string.Empty);
        }
        public void LogAtk(string log)
        {
            File.AppendAllText(logAtk, log+"\n");
        }
        public void LogSa(string log)
        {
            File.AppendAllText(logSa, log+"\n");
        }
        public void LogDeath(string log)
        {
            File.AppendAllText(logDeath, log + "\n");
        }
    }

    class HealerProxy : Proxy, ISpecial, IHealable
    {
        Healer healer;
        public override int hp { get => healer.hp; set => healer.hp=value; }
        public int maxHP { get => healer.maxHP; set => healer.maxHP=value; }
        public override int atk { get => healer.atk; set=>healer.atk=value; }
        public override int def { get => healer.def; set => healer.def=value; }
        public int strength { get => healer.strength; set=>healer.hp=value; }
        public int range { get=>healer.range; set=>healer.range=value; }
        public override PictureBox icon { get => healer.icon; set=> healer.icon=value; }
        public override Type type { get => healer.type; set => healer.type=value; }
        public override int price { get => healer.price; set=>healer.price=value; }
  
        public HealerProxy(Healer healer)
        {
            this.healer = healer;
            this.icon = healer.icon;
        }
        public override void TakeDamage(Unit opnt, Army thisarmy)
        {
            if (!healer.IsDead && !opnt.IsDead)
            {
                healer.TakeDamage(opnt, thisarmy);
                LogAtk($"({thisarmy.armytype}){this.type} took {(opnt.atk * thisarmy.armyprice / (thisarmy.armyprice + this.def))} damage from {opnt.type}");
                if (healer.IsDead) LogDeath($"({thisarmy.armytype}){this.type} died");
            }
        }
        public override bool IsDead { get { return healer.hp <= 0; } }
        public void UseSpecialAbility(Army f_army, List<Unit> inrange, Unit opnt=null)
        {
                opnt = SetTarget(inrange);
            if (opnt != null)
            {
                healer.UseSpecialAbility(f_army, inrange, opnt);
                LogSa($"{this.type} healed {opnt.type} ");
            }
        }

        public Unit SetTarget(List <Unit> inrange)
        {
            List<Unit> healable = new List<Unit>();
            foreach (Unit u in inrange)
                if (u is IHealable && !u.IsDead)
                    healable.Add(u);
            if (healable.Any())
            {
                Random rnd = new Random();
                int n = rnd.Next(0, healable.Count);
                return healable[n];
            }
            else return null;
        }

        public void GetHealed(int heal)
        {
            healer.GetHealed(heal);
        }
    }


    class LightInfantryProxy : Proxy, IHealable, IClonable, ISpecial
    //ISpecial,
    {
        LightInfantry lightinfantry;
        public override int hp { get => lightinfantry.hp; set => lightinfantry.hp = value; }
        public int maxHP { get => lightinfantry.maxHP; set => lightinfantry.maxHP = value; }
        public override int atk { get => lightinfantry.atk; set => lightinfantry.atk = value; }
        public override int def { get => lightinfantry.def; set => lightinfantry.def = value; }
        public int strength { get => lightinfantry.strength; set => lightinfantry.hp = value; }
        public int range { get => lightinfantry.range; set => lightinfantry.range = value; }
        public override int price { get => lightinfantry.price; set => lightinfantry.price = value; }
        public override PictureBox icon { get => lightinfantry.icon; set => lightinfantry.icon = value; }
        public override Type type { get => lightinfantry.type; set => lightinfantry.type = value; }
        public LightInfantryProxy(LightInfantry lightInfantry)
        {
           this.lightinfantry = lightInfantry;
            this.icon = lightInfantry.icon;
        }

        public override void TakeDamage(Unit opnt, Army thisarmy)
        {
            if (!lightinfantry.IsDead && !opnt.IsDead)
            {
                lightinfantry.TakeDamage(opnt, thisarmy);
                LogAtk($"({thisarmy.armytype}){this.type} took {(opnt.atk * thisarmy.armyprice / (thisarmy.armyprice + this.def))} damage from {opnt.type}");
                if (lightinfantry.hp <= 0) LogDeath($"({thisarmy.armytype}){this.type} died");
            }
        }
        public override bool IsDead { get { return lightinfantry.hp <= 0; } }
        public void UseSpecialAbility(Army f_army, List<Unit> inrange, Unit opnt)
        {
            opnt = SetTarget(inrange);
            if (opnt != null)
            {
                lightinfantry.UseSpecialAbility(f_army, inrange, opnt);
                LogSa($"{this.type} buffed {opnt.type}");
            }
        }

        public  Unit SetTarget(List<Unit> inrange)
        {
            //List<Unit> inrange = new List<Unit>();
            //int ind = f_army.unitlist.IndexOf(this);
            //if (ind + range < f_army.unitlist.Count)
            //    inrange.AddRange(f_army.unitlist.GetRange(ind, range));
            //else inrange.AddRange(f_army.unitlist.GetRange(ind, f_army.unitlist.Count - ind));
            //if (ind - range >= 0)
            //    inrange.AddRange(f_army.unitlist.GetRange(ind - range, range));
            //else inrange.AddRange(f_army.unitlist.GetRange(0, ind - 1));
            List<Unit> buffable = new List<Unit>();
            foreach (Unit u in inrange)
                if (u is IBuffable && !u.IsDead)
                    buffable.Add(u);
            if (buffable.Any())
            {
                Random rnd = new Random();
                int n = rnd.Next(0, buffable.Count);
                return buffable[n];
            }
            else return null;
        }
       
        public void GetHealed(int heal)
        {
            lightinfantry.GetHealed(heal);
        }

        public IClonable Clone()
        {
            return new LightInfantryProxy(lightinfantry.Clone() as LightInfantry);
        }
    }


    class HeavyInfantryProxy : Proxy, IBuffable
    {
        HeavyInfantry heavyinfantry;
        public override int hp { get => heavyinfantry.hp; set =>     heavyinfantry.hp = value; }
        public override int atk { get => heavyinfantry.atk; set => heavyinfantry.atk = value; }
        public override int def { get => heavyinfantry.def; set => heavyinfantry.def = value; }
        public   override PictureBox icon { get => heavyinfantry.icon; set => heavyinfantry.icon = value; }
        public override Type type { get => heavyinfantry.type; set => heavyinfantry.type = value; }
        public override int price { get => heavyinfantry.price; set => heavyinfantry.price = value; }
        public HeavyInfantryProxy(HeavyInfantry heavyInfantry)
        {
            this.heavyinfantry = heavyInfantry;
            this.icon = heavyInfantry.icon;
        }
        public override void TakeDamage(Unit opnt, Army thisarmy)
        {
            if (!heavyinfantry.IsDead && !opnt.IsDead)
            {
                heavyinfantry.TakeDamage(opnt, thisarmy);
           
                LogAtk($"({thisarmy.armytype}){this.type} took {(opnt.atk * thisarmy.armyprice / (thisarmy.armyprice + this.def))} damage from {opnt.type}");
                if (heavyinfantry.hp <= 0) LogDeath($"({thisarmy.armytype}){this.type} died");
            }
        }
        public override bool IsDead { get { return heavyinfantry.hp <= 0; } }
        public IBuffable GetBuff(int strength)
        {
            return this;
        }

    }
    class KnightProxy : Proxy
    {
        Knight knight;

        public override int hp { get => knight.hp; set => knight.hp = value; }
        public override int atk { get => knight.atk; set => knight.atk = value; }
        public override int def { get => knight.def; set => knight.def = value; }
        public override PictureBox icon { get => knight.icon; set => knight.icon = value; }
        public override Type type { get => knight.type; set => knight.type = value; }
        public override int price { get => knight.price; set => knight.price = value; }
        public KnightProxy(Knight knight)
        {
            this.knight = knight;
            this.icon = knight.icon;
        }

        public override void TakeDamage(Unit opnt, Army thisarmy)
        {
            if (!knight.IsDead && !opnt.IsDead)
            {
                knight.TakeDamage(opnt, thisarmy);
                LogAtk($"({thisarmy.armytype}){this.type} took {(opnt.atk * thisarmy.armyprice / (thisarmy.armyprice + this.def))} damage from {opnt.type}");
                if (knight.hp <= 0) LogDeath($"({thisarmy.armytype}){this.type} died");
            }
        }
        public override bool IsDead { get { return knight.hp <= 0; } }

    }
    class ArcherProxy : Proxy, ISpecial, IHealable
    {
        Archer archer;
        public override int hp { get => archer.hp; set => archer.hp = value; }
        public int maxHP { get => archer.maxHP; set => archer.maxHP = value; }
        public override int atk { get => archer.atk; set => archer.atk = value; }
        public override int def { get => archer.def; set => archer.def = value; }
        public int strength { get => archer.strength; set => archer.hp = value; }
        public int range { get => archer.range; set => archer.range = value; }
        public override PictureBox icon { get => archer.icon; set => archer.icon = value; }
        public override Type type { get => archer.type; set => archer.type = value; }
        public override int price { get => archer.price; set => archer.price = value; }
        public ArcherProxy(Archer archer)
        {
            this.archer = archer;
            this.icon = archer.icon;
        }
        public override void TakeDamage(Unit opnt, Army thisarmy)
        {   if (!archer.IsDead && !opnt.IsDead)
            {
                archer.TakeDamage(opnt, thisarmy);
                LogAtk($"({thisarmy.armytype}){this.type} took {(opnt.atk * thisarmy.armyprice / (thisarmy.armyprice + this.def))} damage from {opnt.type}");
                if (archer.hp <= 0) LogDeath($"({thisarmy.armytype}){this.type} died");
            }
        }
        public override bool IsDead { get { return archer.hp <= 0; } }
        public void UseSpecialAbility(Army e_army, List<Unit> inrange, Unit opnt)
        {
            opnt = SetTarget(inrange);
            if (opnt != null)
            {             
                archer.UseSpecialAbility(e_army, inrange, opnt);
                LogSa($"{this.type} shot {opnt.type}");
            }
        }

        public Unit SetTarget(List <Unit> inrange)
        {
           
                //List<Unit> inrange = new List<Unit>();
                //int ind = f_army.unitlist.IndexOf(this);
                //if (range > ind)
                //{
                //    if (range - ind >= e_army.unitlist.Count)
                //        inrange.AddRange(e_army.unitlist.GetRange(0, e_army.unitlist.Count));
                //    else
                //        inrange.AddRange(e_army.unitlist.GetRange(0, range - ind));
                //}
            if (inrange.Any())
            {
                Random rnd = new Random();
                int n = rnd.Next(0, inrange.Count);
                return inrange[n];
            }
            else return null;

            
        }
        
        public void GetHealed(int heal)
        {
            archer.GetHealed(heal);
        }
    }
    class WarlockProxy : Proxy, ISpecial
    {
        Warlock warlock;
        public override    int hp { get => warlock.hp; set => warlock.hp = value; }
        public override int atk { get => warlock.atk; set => warlock.atk = value; }
        public override int def { get => warlock.def; set => warlock.def = value; }
        public int strength { get => warlock.strength; set => warlock.hp = value; }
        public int range { get => warlock.range; set => warlock.range = value; }
       public override PictureBox icon { get => warlock.icon; set => warlock.icon = value; }
        public override Type type { get => warlock.type; set => warlock.type = value; }
        public override int price { get => warlock.price; set => warlock.price = value; }
        public WarlockProxy(Warlock warlock)
        {
            this.warlock = warlock;
            this.icon = warlock.icon;
        }
        public override void TakeDamage(Unit opnt, Army thisarmy)
        { if (!warlock.IsDead && !opnt.IsDead)
            {
                warlock.TakeDamage(opnt, thisarmy);
                LogAtk($"({thisarmy.armytype}){this.type} took {(opnt.atk * thisarmy.armyprice / (thisarmy.armyprice + this.def))} damage from {opnt.type}");
                if (warlock.hp <= 0) LogDeath($"({thisarmy.armytype}){this.type} died");
            }
        }
        public override bool IsDead { get { return warlock.hp <= 0; } }
        public void UseSpecialAbility(Army f_army, List<Unit> inrange, Unit opnt)
        {
            opnt = SetTarget(inrange);
            Random rnd = new Random();
            int chance = rnd.Next(0, 100);
            
            if (opnt != null)
            {
                if (chance <= strength)
                {
                    warlock.UseSpecialAbility(f_army, inrange, opnt);
                    LogSa($"{this.type} cloned {opnt.type}");
                }
            }
        }
        public Unit SetTarget(List <Unit> inrange)
        {
            //List<Unit> inrange = new List<Unit>();
            //int ind = f_army.unitlist.IndexOf(this);
            //if (ind + range < f_army.unitlist.Count)
            //    inrange.AddRange(f_army.unitlist.GetRange(ind, range));
            //else inrange.AddRange(f_army.unitlist.GetRange(ind, f_army.unitlist.Count - ind));
            //if (ind - range >= 0)
            //    inrange.AddRange(f_army.unitlist.GetRange(ind - range, range));
            //else inrange.AddRange(f_army.unitlist.GetRange(0, ind - 1));
            List<Unit> clonable = new List<Unit>();
            foreach (Unit u in inrange)
                if (u is IClonable  && !u.IsDead)
                    clonable.Add(u);
            if (clonable.Any())
            {
                Random rnd = new Random();
                int n = rnd.Next(0, clonable.Count);
                return clonable[n];
            }
            else return null;
        }

    }
    public class GGAdapterProxy : Proxy
    {
        GGAdapter ggadapter;
        public override int hp { get => ggadapter.hp; set => ggadapter.hp = value; }
        public override int atk { get => ggadapter.atk; set => ggadapter.atk = value; }
        public override int def { get => ggadapter.def; set => ggadapter.def = value; }
        public override PictureBox icon { get => ggadapter.icon; set => ggadapter.icon = value; }
        public override Type type { get => ggadapter.type; set => ggadapter.type = value; }
        public override int price { get => ggadapter.price; set => ggadapter.price = value; }
        public GGAdapterProxy(GGAdapter ggadapter)
        {
           this.ggadapter = ggadapter;
            this.icon = ggadapter.icon;
        }

        public override void TakeDamage(Unit opnt, Army thisarmy)
        { if (!ggadapter.IsDead && !opnt.IsDead)
            {
                ggadapter.TakeDamage(opnt, thisarmy);
                LogAtk($"({thisarmy.armytype}){this.type} took {(opnt.atk * thisarmy.armyprice / (thisarmy.armyprice + this.def))} damage from {opnt.type}");
                if (ggadapter.hp <= 0) LogDeath($"({thisarmy.armytype}){this.type} died");
            }
        }
        public override bool IsDead { get { return ggadapter.hp <= 0; } }
    }


}
