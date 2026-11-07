using System;
using System.Collections;
using IE.Model;
using IE.Presentation;
using UnityEngine;
using NaughtyAttributes;

namespace IE.UnityView
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private GameObject devPanel;
        Game game;
        Arena arena;
        RadiusStat radiusStat;
        TimeStat timeStat;
        SpeedStat speedStat;
        DamageStat damageStat;
        LuckStat luckStat;

        public void Awake()
        {
            game = Game.Instance;
            game.Initialize();

            arena = game.GetArena();

            radiusStat = game.GetStat<RadiusStat>();
            timeStat = game.GetStat<TimeStat>();
            speedStat = game.GetStat<SpeedStat>();
            damageStat = game.GetStat<DamageStat>();
            luckStat = game.GetStat<LuckStat>();
        }

        [ShowNativeProperty]
        private string RadiusStat => radiusStat is null ? "" : $"{radiusStat.GetRadius()}\t at level {radiusStat.GetLevel()}\t ${radiusStat.GetUpgradeCost()} to upgrade";
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void UpgradeRadius() => radiusStat.Upgrade();

        [ShowNativeProperty]
        private string TimeStat => timeStat is null ? "" : $"{timeStat.GetRoundTime()}\t at level {timeStat.GetLevel()}\t ${timeStat.GetUpgradeCost()} to upgrade";
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void UpgradeTime() => timeStat.Upgrade();

        [ShowNativeProperty]
        private string SpeedStat => speedStat is null ? "" : $"{speedStat.GetTicksBetweenAttacks()}\t at level {speedStat.GetLevel()}\t ${speedStat.GetUpgradeCost()} to upgrade";
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void UpgradeSpeed() => speedStat.Upgrade();

        [ShowNativeProperty]
        private string DamageStat => damageStat is null ? "" : $"{damageStat.GetDamage()}\t at level {damageStat.GetLevel()}\t ${damageStat.GetUpgradeCost()} to upgrade";
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void UpgradeDamage() => damageStat.Upgrade();

        [ShowNativeProperty]
        private string LuckStat => luckStat is null ? "" : $"{luckStat.GetCoinDropMultiplier()}\t at level {luckStat.GetLevel()}\t ${luckStat.GetUpgradeCost()} to upgrade";
        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void UpgradeLuck() => luckStat.Upgrade();

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void SpawnZombie()
        {
            Cell zombieCell = arena.GetRandomFreeCell();
            Zombie zombie = new Zombie(zombieCell, GridDirection.Up);
            zombieCell.AddEntity(zombie);
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void SpawnSkeleton()
        {
            Cell skeletonCell = arena.GetRandomFreeCell();
            Skeleton skeleton = new Skeleton(skeletonCell, GridDirection.Up);
            skeletonCell.AddEntity(skeleton);
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void SpawnCoin()
        {
            Cell skeletonCell = arena.GetRandomFreeCell();
            Coin skeleton = new Coin(skeletonCell, GridDirection.Up, 10);
            skeletonCell.AddEntity(skeleton);
        }

        public void ToggleDevPanel()
        {
            bool previousState = devPanel.activeSelf;
            devPanel.SetActive(!previousState);
        }
    }
}