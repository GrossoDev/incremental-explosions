using IE.Model;
using UnityEngine;

namespace IE.UnityView
{
    public class ToggleDevPanelInputCommand : InputCommand
    {
        [SerializeField] private GameObject devPanel;

        protected override void OnKeyDown()
        {
            ToggleDevPanel();
        }

        public void ToggleDevPanel()
        {
            bool previousState = devPanel.activeSelf;
            devPanel.SetActive(!previousState);
        }
    }
}