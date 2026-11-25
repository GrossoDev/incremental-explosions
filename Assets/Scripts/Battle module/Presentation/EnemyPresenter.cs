using Battle.Model;
using Battle.View;

namespace Battle.Presentation
{
    public class EnemyPresenter : EntityPresenter
    {
        public EnemyPresenter(Entity model) : base(model)
        {
        }

        public Enemy GetModel()
        {
            return (Enemy)model;
        }

        private EnemyView GetView()
        {
            return (EnemyView)view;
        }

        public void NotifyBeforeMove()
        {
            GetView().PlayBeforeMoveAnimation();
        }

        public void NotifyAfterMove()
        {
            GetView().PlayAfterMoveAnimation();
        }

        public void NotifyDamaged(int damage)
        {
            GetView().PlayDamageAnimation(damage);
        }
    }
}

