using System;
using System.Collections;
using IE.Model;
using IE.Presentation;
using UnityEngine;

namespace IE.UnityView
{
    public class GameView : MonoBehaviour
    {
        Game game;
        Arena arena;

        public void Awake()
        {
            game = Game.Instance;
            game.Initialize();

            arena = game.GetArena();
        }
    }
}