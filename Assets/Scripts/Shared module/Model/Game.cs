using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Shared.Model
{
    public class Game
    {
        private const int DefaultInitialTax = 150;
        private static readonly Regex JsonIntFieldRegex = new Regex(
            "\"(?<key>[A-Za-z0-9_]+)\"\\s*:\\s*(?<value>-?\\d+)",
            RegexOptions.Compiled
        );

        private int currentLevel;
        private int battleCoins;
        private int totalCoins;
        private int nextTax = DefaultInitialTax;
        private List<Stat> stats = new List<Stat>();
        public static Game Instance { get; private set; } = new Game();

        private Game() { }

        public StatType GetStat<StatType>() where StatType : Stat
        {
            Stat stat = stats.Find(stat => stat is StatType);
            
            return stat as StatType;
        }

        public int GetBattleCoins()
        {
            return battleCoins;
        }

        public void SetBattleCoins(int amount)
        {
            battleCoins = amount;
        }

        public int GetTotalCoins()
        {
            return totalCoins;
        }

        public void SetTotalCoins(int amount)
        {
            totalCoins = amount;
        }

        public int GetCurrentLevel()
        {
            return currentLevel;
        }

        public void SetCurrentLevel(int level)
        {
            currentLevel = level;
        }

        public int GetNextTax()
        {
            return nextTax;
        }

        public void SetNextTax(int amount)
        {
            nextTax = amount;
        }

        public void NewGame()
        {
            totalCoins = 0;
            battleCoins = 0;
            currentLevel = 1;
            nextTax = DefaultInitialTax;
            stats = new List<Stat>()
            {
                new TimeStat(),
                new SpeedStat(),
                new RadiusStat(),
                new LuckStat(),
                new DamageStat(),
                new TorchStat(),
            };
        }

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  \"currentLevel\": {currentLevel},");
            sb.AppendLine($"  \"totalCoins\": {totalCoins},");
            sb.AppendLine($"  \"nextTax\": {nextTax},");
            sb.AppendLine($"  \"radiusStatLevel\": {GetStatLevel<RadiusStat>()},");
            sb.AppendLine($"  \"speedStatLevel\": {GetStatLevel<SpeedStat>()},");
            sb.AppendLine($"  \"damageStatLevel\": {GetStatLevel<DamageStat>()},");
            sb.AppendLine($"  \"luckStatLevel\": {GetStatLevel<LuckStat>()},");
            sb.AppendLine($"  \"timeStatLevel\": {GetStatLevel<TimeStat>()},");
            sb.AppendLine($"  \"torchStatLevel\": {GetStatLevel<TorchStat>()}");
            sb.Append("}");

            return sb.ToString();
        }

        public void Deserialize(string serialized)
        {
            if (string.IsNullOrWhiteSpace(serialized))
                throw new ArgumentException("Saved game data is empty.", nameof(serialized));

            string trimmed = serialized.Trim();
            if (!trimmed.StartsWith("{") || !trimmed.EndsWith("}"))
                throw new FormatException("Saved game data is not a valid JSON object.");

            Dictionary<string, int> fields = ParseJsonIntFields(trimmed);

            currentLevel = Math.Max(1, ReadRequiredInt(fields, "currentLevel"));
            totalCoins = Math.Max(0, ReadRequiredInt(fields, "totalCoins"));
            nextTax = Math.Max(0, ReadOptionalInt(fields, "nextTax", DefaultInitialTax));
            battleCoins = 0;

            int radiusStatLevel = Math.Max(1, ReadRequiredInt(fields, "radiusStatLevel"));
            int speedStatLevel = Math.Max(1, ReadRequiredInt(fields, "speedStatLevel"));
            int damageStatLevel = Math.Max(1, ReadRequiredInt(fields, "damageStatLevel"));
            int luckStatLevel = Math.Max(1, ReadRequiredInt(fields, "luckStatLevel"));
            int timeStatLevel = Math.Max(1, ReadRequiredInt(fields, "timeStatLevel"));
            int torchStatLevel = Math.Max(1, ReadOptionalInt(fields, "torchStatLevel", 1));

            stats = new List<Stat>()
            {
                new RadiusStat(radiusStatLevel),
                new SpeedStat(speedStatLevel),
                new DamageStat(damageStatLevel),
                new LuckStat(luckStatLevel),
                new TimeStat(timeStatLevel),
                new TorchStat(torchStatLevel),
            };
        }

        private int GetStatLevel<StatType>() where StatType : Stat
        {
            return GetStat<StatType>()?.GetLevel() ?? 1;
        }

        private static Dictionary<string, int> ParseJsonIntFields(string json)
        {
            var fields = new Dictionary<string, int>();

            foreach (Match match in JsonIntFieldRegex.Matches(json))
            {
                string key = match.Groups["key"].Value;
                if (int.TryParse(match.Groups["value"].Value, out int value))
                    fields[key] = value;
            }

            return fields;
        }

        private static int ReadRequiredInt(Dictionary<string, int> fields, string key)
        {
            if (!fields.TryGetValue(key, out int value))
                throw new FormatException($"Missing required save field '{key}'.");

            return value;
        }

        private static int ReadOptionalInt(Dictionary<string, int> fields, string key, int defaultValue)
        {
            return fields.TryGetValue(key, out int value) ? value : defaultValue;
        }
    }
}