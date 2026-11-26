using Battle.Presentation;
using UnityEngine;

namespace Battle.View
{
    public class PlayerView : EntityView
    {
        [SerializeField] private TorchLightView torchLightView;

        private void Awake()
        {
            if (torchLightView == null)
                torchLightView = GetComponentInChildren<TorchLightView>();
        }

        private void Start()
        {
            SyncTorchLight();
        }

        protected override void Update()
        {
            base.Update();
            SyncTorchLight();
        }

        public PlayerPresenter GetPresenter()
        {
            return (PlayerPresenter)presenter;
        }

        private void SyncTorchLight()
        {
            if (torchLightView == null || presenter == null)
                return;

            PlayerPresenter playerPresenter = GetPresenter();
            torchLightView.SetLightProfile(playerPresenter.GetTorchRange(), playerPresenter.GetTorchIntensity());
        }
    }
}