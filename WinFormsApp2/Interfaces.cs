using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace game
{
    public enum Type
    {
        LightInfantry = 0,
        HeavyInfantry,
        Knight,
        Healer,
        Archer,
        Warlock,
        GulyayGorod
    }
    public abstract class Unit
    {
        public abstract Type type { get; set; }
        public abstract int hp { get; set; }
        public abstract int atk { get; set; }
        public abstract int def { get; set; }
        public abstract int price { get; set; }

        public abstract PictureBox icon {get; set;}
        public abstract void TakeDamage(Unit opnt, Army thisarmy);

        


        public abstract bool IsDead { get; }
        //{
        //    get { return this.hp <= 0; }
        //}
    }

    interface ISpecial
    {
         int strength { get; set; }
         int range { get; set; }
        // void UseSpecialAbility(Army army, Army army2, Unit opnt = null);
        void UseSpecialAbility(Army army, List<Unit> inrange, Unit opnt = null);
        // List<Unit> unitsinrange(Army f_army, Army e_army);
    }

    interface IHealable
    {
         int maxHP { get; set; }
        void GetHealed(int heal);
    }

    public interface IBuffable
    {   
 
        public IBuffable GetBuff(int strength);
    }

    interface IClonable
    {
        IClonable Clone();
    }
}
