using System;
using System.Collections.Generic;
using System.Linq;
using IE.Presentation;

namespace IE.Model
{
    public class Player : Entity, ITickable, ICoinCollector
    {
        private int ticksBetweenAttacks = 50;
        private int ticksSinceLastAttack = 1;
        private StatManager statManager;
        public static Player Instance { get; private set; }

        public Player(Cell cell, GridDirection direction) : base(cell, direction)
        {
            statManager = StatManager.Instance;
            Instance = this;
        }

        public void Tick()
        {
            AttackTick();
        }

        public void CollectedCoin(int value)
        {
            statManager.AddCoins(value);
        }

        private void AttackTick()
        {
            if (ticksSinceLastAttack >= ticksBetweenAttacks)
            {
                CreateFireInCell();
                ticksSinceLastAttack = 0;
            }
            else
            {
                ticksSinceLastAttack++;
            }
        }

        private void CreateFireInCell()
        {
            Cell cell = GetCell();

            int fireDamage = statManager.GetStat<DamageStat>().GetDamage();
            int fireRadius = statManager.GetStat<RadiusStat>().GetRadius();

            Fire fire = new Fire(cell, GridDirection.None, fireDamage, fireRadius);
            cell.AddEntity(fire);
        }

        protected override void CreatePresenter()
        {
            presenter = new PlayerPresenter(this);
            presenter.UpdatePosition(occupiedCell.GetPosition());
            presenter.UpdateDirection(direction);
        }
    }
}
