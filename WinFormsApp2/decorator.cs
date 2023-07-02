using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public abstract class Buff : Unit, IBuffable
    {
        public IBuffable buffable;

        public Buff(IBuffable buffable) { this.buffable = buffable; }
        public virtual IBuffable GetBuff(int strength)
        {
            return buffable.GetBuff(strength);
        }

        public override int def { get => (buffable as Unit).def; set => (buffable as Unit).def = value; }
        public override int hp { get => (buffable as Unit).hp; set => (buffable as Unit).hp = value; }
        public override int atk { get => (buffable as Unit).atk; set => (buffable as Unit).atk = value; }
        public override int price { get => (buffable as Unit).price; set => (buffable as Unit).price = value; }
        public override Type type { get => (buffable as Unit).type; set => (buffable as Unit).type = value; }
        public override PictureBox icon { get => (buffable as Unit).icon; set => (buffable as Unit).icon = value; }
        public override bool IsDead { get { return (buffable as Unit).hp <= 0; } }
        public override void TakeDamage(Unit opnt, Army army)
        {
            (buffable as Unit).TakeDamage(opnt, army);

        }

    }

    public class Coffee :  Buff
    {
        public Coffee(IBuffable buffable): base(buffable) { }
       
        public override IBuffable GetBuff(int strentgh)
        {
         buffable.GetBuff(strentgh);
            BuffDef(buffable, strentgh);
            return buffable;
        }
        public void BuffDef(IBuffable buffable, int strength)
        {
            (buffable as Unit).def += strength;
        }
    }
    public class EnergyDrink : Buff
    {
        public EnergyDrink(IBuffable buffable) : base(buffable) { }

        public override IBuffable GetBuff(int strentgh)
        {
            buffable.GetBuff(strentgh);
            BuffAtk(buffable, strentgh);
            return buffable;
        }
        public void BuffAtk(IBuffable buffable, int strength)
        {
            (buffable as Unit).atk += strength;
        }
    }
    public class Beer: Buff
    {
        public Beer(IBuffable buffable) : base(buffable) { }

        public override IBuffable GetBuff(int strentgh)
        {
            buffable.GetBuff(strentgh);
            BuffDefnAtk(buffable, strentgh);
            return buffable;
        }
        public void BuffDefnAtk(IBuffable buffable, int strength)
        {
            (buffable as Unit).def += strength;
            (buffable as Unit).atk += strength;
        }
    }
    
}
