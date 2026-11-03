using System;
using System.Collections.Generic;
using System.Linq;

namespace IE.Model
{
    public class Player : Entity, ITickable, ICoinCollector
    {
        private int ticksBetweenAttacks = 50;
        private int ticksSinceLastAttack = 1;
        private StatManager statManager;

        public Player(Cell cell, GridDirection direction) : base(cell, direction)
        {
            statManager = StatManager.Instance;
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
                CreateFireInDirection(GridDirection.Up);
                CreateFireInDirection(GridDirection.Down);
                CreateFireInDirection(GridDirection.Left);
                CreateFireInDirection(GridDirection.Right);

                ticksSinceLastAttack = 0;
            }
            else
            {
                ticksSinceLastAttack++;
            }
        }

        private void CreateFireInDirection(GridDirection direction)
        {
            Arena arena = GetArena();
            Cell currentCell = GetCell();

            if (arena.DoesCellHaveNeighbor(currentCell, direction))
            {
                Cell targetCell = arena.GetCellNeighbor(currentCell, direction);

                int fireDamage = statManager.GetStat<DamageStat>().GetDamage();
                int fireRadius = statManager.GetStat<RadiusStat>().GetRadius();

                if (targetCell != null)
                {
                    Fire fire = new Fire(targetCell, direction, fireDamage, fireRadius);
                    targetCell.AddEntity(fire);
                }
            }
        }
    }
}
