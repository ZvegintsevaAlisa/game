using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    class LightInfantry : Unit,  IHealable, IClonable, ISpecial
    {
        public override Type type { get; set; }
        public override int hp { get; set; }
        public override int atk { get; set; }
        public override int def { get; set; }
        public override int price { get; set; }

        public override PictureBox icon { get; set; }

        public int maxHP { get; set; }
      
        public  int strength { get; set; } 
        public int range { get; set; } 
        public LightInfantry()
        {
            this.hp = 10;
            this.maxHP = 10;
            this.atk = 5;
            this.def = 4;;
            this.icon = new PictureBox();
            this.strength = 3;
            this.range = 1;
            this.price = atk+def+hp+(strength+range)*2;
            this.type = Type.LightInfantry;
        }
        public override bool IsDead { get { return this.hp <= 0; } }
        
        public override void TakeDamage(Unit opnt, Army army)
        {
            this.hp -= (opnt.atk * army.armyprice / (army.armyprice + this.def));
        }
        public void UseSpecialAbility(Army f_army, List<Unit> inrange, Unit opnt)
        {  if (opnt is IBuffable unit)
            {
                if (unit is not Coffee || unit is not EnergyDrink || unit is not Beer)
                {
                    Random rnd = new Random();
                    Buff buff;
                    int b = rnd.Next(0, 2);
                    if (b == 0)
                        buff = new Coffee(unit);
                    else if (b == 1)
                        buff = new EnergyDrink(unit);
                    else buff = new Beer(unit);
                    buff.GetBuff(strength);
                    int n = f_army.unitlist.IndexOf(opnt);
                    f_army.RemoveUnit(opnt);
                    f_army.unitlist.Insert(n, buff);
                }
            }
        }


        public void GetHealed(int heal)
        {
            if (hp + heal < maxHP) this.hp += heal;
            else hp = maxHP;
        }

        public IClonable Clone()

        {
            return new LightInfantry();
        }
    }

    class HeavyInfantry : Unit, IBuffable
    {
        public override Type type { get; set; }
        public override int hp { get; set; }
        public override int atk { get; set; }
        public override int def { get; set; }
        public override int price { get; set; }

        public override PictureBox icon { get; set; }
        public HeavyInfantry()
        {
            this.hp = 20;
            this.atk = 6;
            this.def = 8;
            this.price = hp+atk+def;    
            this.icon = new PictureBox();
            this.type= Type.HeavyInfantry;
        }
        public override bool IsDead { get { return this.hp <= 0; } }

        public override void TakeDamage(Unit opnt, Army army)
        {
            this.hp -= (opnt.atk * army.armyprice / (army.armyprice + this.def));
        }
        public IBuffable GetBuff(int strength)
        {
            return this;
        }



    }


    class Knight : Unit
    {
        public override Type type { get; set; }
        public override int hp { get; set; }
        public override int atk { get; set; }
        public override int def { get; set; }
        public override int price { get; set; }

        public override PictureBox icon { get; set; }
        public Knight()
        {
            this.hp = 30;
            this.atk = 9;
            this.def = 9;
            this.price = atk + def + hp;
            this.icon = new PictureBox();
            this.type = Type.Knight;
        }

        public override bool IsDead { get { return this.hp <= 0; } }

        public override void TakeDamage(Unit opnt, Army army)
        {
            this.hp -= (opnt.atk * army.armyprice / (army.armyprice + this.def));
        }

    }

    class Healer : Unit, ISpecial, IHealable
    {
        public override Type type { get; set; }
        public override int hp { get; set; }
        public override int atk { get; set; }
        public override int def { get; set; }
        public override int price { get; set; }

        public override PictureBox icon { get; set; }
        public int maxHP { get; set; }
        public int strength { get; set; }
        public int range { get; set; }
        public Healer()
        {
            this.hp = 10;
            this.maxHP = 7;
            this.atk = 3;
            this.def = 3;
            this.strength = 3;
            this.range = 2;
            this.price = atk + hp + def + (strength + range) * 2;
            this.icon = new PictureBox();
            this.type = Type.Healer;
        }
        public override bool IsDead { get { return this.hp <= 0; } }

        public override void TakeDamage(Unit opnt, Army army)
        {
            this.hp -= (opnt.atk * army.armyprice / (army.armyprice + this.def));
        }
        public void UseSpecialAbility(Army f_army, List <Unit> inrange, Unit opnt)
        {
            if (opnt is IHealable unit)
                unit.GetHealed(strength);
        }

        public void GetHealed(int heal)
        {
            if (hp + heal < maxHP) this.hp += heal;
            else hp = maxHP;
        }
    }

    class Archer : Unit, ISpecial, IHealable
    {
        public override Type type { get; set; }
        public override int hp { get; set; }
        public override int atk { get; set; }
        public override int def { get; set; }
        public override int price { get; set; }

        public override PictureBox icon { get; set; }
        public int maxHP { get; set; }
        public int strength { get; set; }
        public int range { get; set; }
        
        public Archer()
        {
            this.hp = 8;
            this.maxHP = 8;
            this.atk = 5;
            this.def = 3;
            this.strength = 3;
            this.range = 5;
            this.price = atk + def + hp + (strength + range) * 2;
            this.icon = new PictureBox();
            this.type = Type.Archer;
        }
        public override bool IsDead { get { return this.hp <= 0; } }

        public override void TakeDamage(Unit opnt, Army army)
        {
            this.hp -= (opnt.atk * army.armyprice / (army.armyprice + this.def));
        }
        public void UseSpecialAbility(Army e_army, List <Unit> inrange, Unit opnt)
        {
            
           opnt.TakeDamage(this, e_army);
        }
        

        public void GetHealed(int heal)
        {
            if (hp + heal < maxHP) this.hp += heal;
            else hp = maxHP;
        }
    }

    class Warlock : Unit, ISpecial
    {
        public override Type type { get; set; }
        public override int hp { get; set; }
        public override int atk { get; set; }
        public override int def { get; set; }
        public override int price { get; set; }

        public override PictureBox icon { get; set; }
        public int strength { get; set; }
        public int range { get; set; }
        public Warlock()
        {
            this.hp = 7;
            this.atk = 3;
            this.def = 3;
            this.strength = 7;
            this.range = 2;
            this.price = atk + hp + def + (strength + range) * 2;
            this.icon = new PictureBox();
            this.type = Type.Warlock;
        }
        public override bool IsDead { get { return this.hp <= 0; } }

        public override void TakeDamage(Unit opnt, Army army)
        {
            this.hp -= (opnt.atk * army.armyprice / (army.armyprice + this.def));
        }
        public void UseSpecialAbility(Army f_army, List<Unit> inrange, Unit opnt)
        {
          
          Random rnd = new Random();
            int n = rnd.Next(0, 100);
            if (n < strength)
            {
                IClonable clone;
                if (opnt is IClonable unit)
                {
                    clone = unit.Clone();
                    f_army.AddUnit((Unit)clone);
                }
            }
               
            
        }
        

    }




}
