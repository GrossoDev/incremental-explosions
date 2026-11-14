namespace IE.Model
{
    public class Coin : Entity
    {
        private int value;

        public Coin(Cell cell, GridDirection direction) : base(cell, direction, new DefaultCollisionHandler())
        {
            this.value = 10;
        }

        public int GetValue()
        {
            return value;
        }
    }
}
