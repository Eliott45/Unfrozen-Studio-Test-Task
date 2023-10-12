using System;
using TMPro;
using UnityEngine.UI;

namespace Extensions
{
    public static class UIExtensions
    {
        public static void Subscribe(this Button button, Action action)
        {
            button.onClick.AddListener(action.Invoke);
        }

        public static void UnsubscribeAll(this Button button)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}