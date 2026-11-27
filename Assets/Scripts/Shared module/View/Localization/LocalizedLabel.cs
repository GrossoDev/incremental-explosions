using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Shared.View
{
    public class LocalizedLabel : MonoBehaviour
    {
        [SerializeField] private LocalizedText text;

        private void Start()
        {
            string localized = text.Get();

            Text textComponent = GetComponent<Text>();
            TMP_Text tmpTextComponent = GetComponent<TMP_Text>(); 

            if (textComponent is not null)
                textComponent.text = localized;
            
            if (tmpTextComponent is not null)
                tmpTextComponent.text = localized;
        }
    }
}