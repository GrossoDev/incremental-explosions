namespace IE.Model
{
    public abstract class Stat
    {
        protected int level = 1;

        public void Upgrade()
        {
            level++;
        }

        public int GetLevel()
        {
            return level;
        }

        public void Reset()
        {
            level = 1;
        }

        public string Serialize() { return ""; }
        
        public Stat Deserialize(string serialized) { return null; }

        public abstract int GetUpgradeCost();
    }
}