namespace IE.Model
{
    public class Coin : Entity, ITouchable
    {
        private int value;

        public Coin(Cell cell, GridDirection direction, int value) : base(cell, direction)
        {
            this.value = value;
        }

        public int GetValue()
        {
            return value;
        }

        public void Touch(Entity toucher)
        {
            if (toucher is ICoinCollector collector)
            {
                collector.CollectedCoin(value);
                DestroyEntity();
            }
        }
    }
}
