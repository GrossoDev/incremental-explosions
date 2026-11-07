namespace IE.Model
{
    public class Coin : Entity
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
    }
}
