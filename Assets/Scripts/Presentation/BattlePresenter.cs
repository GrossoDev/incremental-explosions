using System;
using IE.Model;
using IE.UnityView;
using UnityEngine;

namespace IE.Presentation
{
    public class BattlePresenter
    {
        private Battle model;

        public BattlePresenter()
        {
            model = new Battle();
            model.Initialize();
        }

        public Arena GetArena()
        {
            return model.GetArena();
        }
    }
}