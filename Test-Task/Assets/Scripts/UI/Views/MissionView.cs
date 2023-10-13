using System;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class MissionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Button _selectButton;
        [SerializeField] private GameObject _fadePanel;
        
        public event Action OnSelectButtonClick;

        private void OnEnable()
        {
            _selectButton.Subscribe(() => OnSelectButtonClick?.Invoke());
        }

        private void OnDisable()
        {
            _selectButton.UnsubscribeAll();
        }
        
        public void SetMissionName(string missionName)
        {
            _nameText.SetText(missionName);
        }

        public void DisplayFadePanel(bool display)
        {
            _fadePanel.gameObject.SetActive(display);
        }
    }
}