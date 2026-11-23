using System;

namespace IE.Model
{
    public abstract class Stat
    {
        protected int level = 1;

        public Stat(int level = 1)
        {
            this.level = level;
        }

        public void Upgrade()
        {
            level++;
        }

        public void Downgrade()
        {
            level = Math.Max(0, level - 1);
        }

        public int GetLevel()
        {
            return level;
        }

        public void Reset()
        {
            level = 1;
        }

        public abstract int GetUpgradeCost();
    }
}