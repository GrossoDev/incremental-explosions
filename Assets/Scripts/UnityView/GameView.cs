using System;
using System.Collections;
using IE.Model;
using IE.Presentation;
using UnityEngine;

namespace IE.UnityView
{
    public class GameView : MonoBehaviour
    {
        StatManager stats;
        Arena arena;

        public void Awake()
        {
            stats = StatManager.Instance;
            arena = new Arena();
            arena.Initialize(20, 20);

            stats.GetStat<RadiusStat>().Upgrade();
            stats.GetStat<RadiusStat>().Upgrade();
            stats.GetStat<RadiusStat>().Upgrade();

            Cell playerCell = arena.GetCellAt(new(10, 0));
            Player player = new Player(playerCell, GridDirection.Up);
            playerCell.AddEntity(player);

            StartCoroutine(NewCoin());
            StartCoroutine(NewZombie());
        }

        private IEnumerator NewZombie()
        {
            while (true)
            {
                Cell zombieCell = arena.GetRandomFreeCell();
                Zombie zombie = new Zombie(zombieCell, GridDirection.Up);
                zombieCell.AddEntity(zombie);

                yield return new WaitForSeconds(1);

                Cell skeletonCell = arena.GetRandomFreeCell();
                Skeleton skeleton = new Skeleton(skeletonCell, GridDirection.Up);
                skeletonCell.AddEntity(skeleton);

                yield return new WaitForSeconds(1);
            }
        }

        private IEnumerator NewCoin()
        {
            while (true)
            {
                Cell coinCell = arena.GetRandomFreeCell();
                Coin coin = new Coin(coinCell, GridDirection.Up, 10);
                coinCell.AddEntity(coin);
                
                yield return new WaitForSeconds(3);
            }
        }
    }
}