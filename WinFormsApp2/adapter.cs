using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game

{
    public class GGAdapter : Unit
    { 
       private readonly GulyayGorod _gulyayGorod;
        public override Type type { get; set; }
        public override int hp { get => _gulyayGorod.GetCurrentHealth(); set => _gulyayGorod.GetCurrentHealth(); }
        public override int atk { get=>_gulyayGorod.GetStrength(); set=>_gulyayGorod.GetStrength(); }
        public override int def { get=>_gulyayGorod.GetDefence(); set=>_gulyayGorod.GetDefence(); }
        public override int price { get; set; }

        public override PictureBox icon { get; set; }
        public GGAdapter(GulyayGorod gulyayGorod)
        {
            _gulyayGorod = gulyayGorod;
            price = gulyayGorod.GetCost();
            icon = new PictureBox();
            type = Type.GulyayGorod;
      
        }

        public override void TakeDamage(Unit opnt, Army army)
        {
            _gulyayGorod.TakeDamage(opnt.atk);

        }
    public override bool IsDead
        {
            get { return _gulyayGorod.IsDead; }
        }
    }

    public class GulyayGorod
        {
            private readonly int _health;
            private readonly int _defence;
            private readonly int _cost;
            private int _currentHealth;
        
        public GulyayGorod(int health, int defence, int cost)
            {
                _defence = defence;
                _health = _currentHealth = health;
                _cost = cost;
            }

            public int GetDefence()
            {
                return _defence;
            }

            public int GetStrength()
            {
                return 0;
            }

            public int GetHealth()
            {
                return _health;
            }

            public int GetCurrentHealth()
            {
                return _currentHealth;
            }

            public int GetCost()
            {
                return _cost;
            }

            public void TakeDamage(int damage)
            {
                if (_currentHealth == 0)
                    throw new Exception("Unit are death!");
   
                if (damage < 0)
                    throw new ArgumentException("Argument must be greater than zero", "damage");
            
            _currentHealth -= Math.Max(damage - _defence, 1);

                if (_currentHealth < 0)
                    _currentHealth = 0;
            }

            public bool IsDead
            {
                get { return _currentHealth <= 0; }
            }
        }
    }


