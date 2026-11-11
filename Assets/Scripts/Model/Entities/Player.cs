using System.Linq;
using IE.Presentation;

namespace IE.Model
{
    public class Player : Entity
    {
        private int ticksSinceLastAttack = 1;
        private SpeedStat speedStat;
        private DamageStat damageStat;
        private RadiusStat radiusStat;
        private Game game;

        public Player(Cell cell, GridDirection direction) : base(cell, direction, new PlayerCollisionHandler())
        {
            this.game = Game.Instance;
            this.speedStat  = game.GetStat<SpeedStat>();
            this.damageStat = game.GetStat<DamageStat>();
            this.radiusStat = game.GetStat<RadiusStat>();
        }

        public override void Tick()
        {
            AttackTick();
        }

        public void CollectedCoin(int value)
        {
            game.AddCoins(value);
        }

        private void AttackTick()
        {
            int ticksBetweenAttacks = speedStat.GetTicksBetweenAttacks();

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

            int fireDamage = damageStat.GetDamage();
            int fireRadius = radiusStat.GetRadius();

            Fire fire = new Fire(cell, GridDirection.None, fireDamage, fireRadius);
        }

        public void MoveInDirection(GridDirection direction)
        {
            var arena = GetArena();
            var currentCell = GetCell();

            SetDirection(direction);
            
            if (arena.HasNeighbourCell(currentCell, direction))
            {
                var targetCell = arena.GetCellNeighbour(currentCell, direction);

                MoveToCell(targetCell);
            }
        }

        protected override void CreatePresenter()
        {
            presenter = new PlayerPresenter(this);
            presenter.UpdatePosition(occupiedCell.GetPosition());
            presenter.UpdateDirection(direction);
        }
    }
}
