using System;
using Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class PreOptionMissionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Image _previewImage;
        [SerializeField] private Button _startButton;

        public event Action<string> OnStartButton;

        private string _id;

        private void OnEnable()
        {
            _startButton.Subscribe(() => OnStartButton?.Invoke(_id));
        }

        private void OnDisable()
        {
            _startButton.UnsubscribeAll();
        }

        public void SetId(string id)
        {
            _id = id;
        }

        public void SetName(string missionName)
        {
            _nameText.SetText(missionName);
        }

        public void SetDescription(string description)
        {
            _descriptionText.SetText(description);
        }

        public void SetPreviewSprite(Sprite preview)
        {
            _previewImage.sprite = preview;
        }

        public void UpdateButtonInteractable(bool interactable)
        {
            _startButton.interactable = interactable;
        }
    }
}