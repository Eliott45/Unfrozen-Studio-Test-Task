using System;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class HeroView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _pointsText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _avatarImage;
        [SerializeField] private Image _avatarBackgroundImage;
        [SerializeField] private Image _selectedMarkImage;
        [SerializeField] private Button _selectButton;
        [SerializeField] private GameObject _lockPanel;
        
        public event Action OnSelectButtonClick;

        private void OnEnable()
        {
            _selectButton.Subscribe(OnSelectButtonClick);
        }

        private void OnDisable()
        {
            _selectButton.UnsubscribeAll();
        }

        public void SetPoints(string pointsText)
        {
            _pointsText.SetText(pointsText);
        }

        public void SetHeroName(string heroName)
        {
            _nameText.SetText(heroName);
        }

        public void SetAvatar(Sprite avatarSprite)
        {
            _avatarImage.sprite = avatarSprite;
        }

        public void SetAvatarBackground(Sprite avatarBackgroundSprite)
        {
            _avatarBackgroundImage.sprite = avatarBackgroundSprite;
        }

        public void DisplayLockPanel(bool display)
        {
            _lockPanel.SetActive(display);
        }

        public void DisplaySelectMark(bool display)
        {
            _selectedMarkImage.enabled = display;
        }
    }
}