namespace IE.Model
{
    public interface ITickable
    {
        public void Tick();
    }

    public interface ITouchable
    {
        public void Touch(Entity toucher);
    }

    public interface IHurtable
    {
        public void Hurted(Entity attacker, int damage);
    }

    public interface ICoinCollector
    {
        public void CollectedCoin(int value);
    }
}