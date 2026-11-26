using System;
using Shared.View;
using Tax.Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace Tax.View
{

    public class TaxView : MonoBehaviour
    {
        [Header("UI elements")]
        [SerializeField] private TaxWarningView warningPanel;
        [SerializeField] private TaxCanPayView canPayPanel;
        [SerializeField] private TaxCannotPayView cannotPayPanel;

        [Header("Battle configuration")]
        [SerializeField] private TaxProgression taxProgression;

        private TaxPresenter presenter;

        private void Start()
        {
            presenter = new TaxPresenter(this);
            warningPanel.gameObject.SetActive(false);
            canPayPanel.gameObject.SetActive(false);
            cannotPayPanel.gameObject.SetActive(false);

            presenter.SetTaxProgression(taxProgression.taxConfigurations);
            presenter.SelectBehaviour();
        }

        public void ShowCanPay()
        {
            cannotPayPanel.gameObject.SetActive(true);
        }

        public void ShowCannotPay()
        {
            canPayPanel.gameObject.SetActive(true);
        }

        public void ShowWarning()
        {
            warningPanel.gameObject.SetActive(true);
        }

        public TaxPresenter GetPresenter()
        {
            return presenter;
        }
    }
}