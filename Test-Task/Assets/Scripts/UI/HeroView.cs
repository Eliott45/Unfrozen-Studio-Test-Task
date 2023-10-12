﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Heroes.Data
{
    public class HeroView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _pointsText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _avatarImage;
        [SerializeField] private Image _avatarBackgroundImage;

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
    }
}