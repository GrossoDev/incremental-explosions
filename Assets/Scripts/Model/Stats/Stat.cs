namespace IE.Model
{
    public abstract class Stat
    {
        protected int level = 1;
        protected StatManager statManager;

        public Stat(StatManager statManager)
        {
            this.level = 1;
            this.statManager = statManager;
        }

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