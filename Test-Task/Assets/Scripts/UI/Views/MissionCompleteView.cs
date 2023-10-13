using System;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class MissionCompleteView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _playerSideText;
        [SerializeField] private TextMeshProUGUI _enemySideText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Image _missionImage;
        [SerializeField] private Button _completeButton;

        public event Action OnCompleteButtonClick; 

        private void OnEnable()
        {
            _completeButton.Subscribe(() => OnCompleteButtonClick?.Invoke());
        }

        private void OnDisable()
        {
            _completeButton.UnsubscribeAll();
        }

        public void SetName(string missionName)
        {
            _nameText.SetText(missionName);
        }
        
        public void SetPlayerSide(string playerSide)
        {
            _playerSideText.SetText(playerSide);
        }

        public void SetEnemySide(string enemySide)
        {
            _enemySideText.SetText(enemySide);
        }

        public void SetDescription(string description)
        {
            _descriptionText.SetText(description);
        }

        public void SetMissionImage(Sprite missionImage)
        {
            _missionImage.sprite = missionImage;
        }
    }
}