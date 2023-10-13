using UnityEngine;

namespace UI.Views
{
    public class PreMissionView : MonoBehaviour
    {
        [SerializeField] private PreOptionMissionView _primaryOptionView;
        [SerializeField] private PreOptionMissionView _secondaryOptionView;

        public PreOptionMissionView GetPrimaryOptionView()
        {
            return _primaryOptionView;
        }
        
        public PreOptionMissionView GetSecondaryOptionView()
        {
            return _secondaryOptionView;
        }
    }
}