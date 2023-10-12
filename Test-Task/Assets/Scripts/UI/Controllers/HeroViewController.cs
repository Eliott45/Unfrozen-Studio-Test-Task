using System;
using Heroes.Data;
using UI.Views;

namespace UI.Controllers
{
    public class HeroViewController : IDisposable
    {
        public event Action<string> OnHeroSelect;
        
        private readonly HeroView _heroView;
        private readonly HeroData _heroData;
        
        public HeroViewController(HeroView heroView, HeroData heroData)
        {
            _heroView = heroView ? heroView : throw new NullReferenceException(nameof(HeroView));
            _heroData = heroData ?? throw new NullReferenceException(nameof(HeroData));
        }

        public void Initialize()
        {
            _heroView.SetAvatar(_heroData.Avatar);
            _heroView.SetAvatarBackground(_heroData.AvatarBackground);
            _heroView.SetHeroName(_heroData.Name);
            _heroView.SetPoints(_heroData.Points.ToString());
            
            _heroView.OnSelectButtonClick += OnSelectButtonClick;
            
            _heroView.DisplaySelectMark(_heroData.Selected);
            _heroView.DisplayLockPanel(_heroData.HeroState == HeroState.Locked);
        }
        
        public void Dispose()
        {
            _heroView.OnSelectButtonClick -= OnSelectButtonClick;
        }

        private void OnSelectButtonClick()
        {
            if (_heroData.HeroState == HeroState.Available)
            {
                OnHeroSelect?.Invoke(_heroData.Id);
            }
        }

        public void Select()
        {
            _heroView.DisplaySelectMark(true);
        }

        public void Unselect()
        {
            _heroView.DisplaySelectMark(false);
        }
    }
}