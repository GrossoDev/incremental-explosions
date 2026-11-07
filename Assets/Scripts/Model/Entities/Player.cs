using IE.Presentation;

namespace IE.Model
{
    public class Player : Entity, ITickable
    {
        private int ticksSinceLastAttack = 1;
        private SpeedStat speedStat;
        private DamageStat damageStat;
        private RadiusStat radiusStat;
        private Game game;

        public Player(Cell cell, GridDirection direction) : base(cell, direction)
        {
            this.game = Game.Instance;
            this.speedStat  = game.GetStat<SpeedStat>();
            this.damageStat = game.GetStat<DamageStat>();
            this.radiusStat = game.GetStat<RadiusStat>();
        }

        public void Tick()
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
