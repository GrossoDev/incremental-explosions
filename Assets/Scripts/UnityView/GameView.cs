using System;
using System.Collections;
using IE.Model;
using IE.Presentation;
using UnityEngine;

namespace IE.UnityView
{
    public class GameView : MonoBehaviour
    {
        public void Start()
        {
            StatManager stats = StatManager.Instance;
            Arena arena = new Arena();
            arena.Initialize(20, 20);

            stats.GetStat<RadiusStat>().Upgrade();
            stats.GetStat<RadiusStat>().Upgrade();
            stats.GetStat<RadiusStat>().Upgrade();

            Cell playerCell = arena.GetCellAt(new(10,0));
            Player player = new Player(playerCell, GridDirection.Up);
            playerCell.AddEntity(player);
            Debug.Log($"Player created at cell ({playerCell.GetPosition()})");

            Cell coinCell = arena.GetCellAt(new(10,5));
            Coin coin = new Coin(coinCell, GridDirection.Up, 10);
            coinCell.AddEntity(coin);
            Debug.Log($"Coin created at cell ({coinCell.GetPosition()})");

            Cell zombieCell = arena.GetCellAt(new(12,7));
            Zombie zombie = new Zombie(zombieCell, GridDirection.Up);
            zombieCell.AddEntity(zombie);
            Debug.Log($"Zombie created at cell ({zombieCell.GetPosition()})");

            StartCoroutine(MovePlayer(player, GridDirection.Up, 10, 1.0f));
        }

        IEnumerator MovePlayer(Player player, GridDirection direction, int steps, float delay)
        {
            for (int i = 0; i < steps; i++)
            {
                player.MoveInDirection(direction);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}