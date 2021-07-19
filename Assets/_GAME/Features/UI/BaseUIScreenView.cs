using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;

namespace GAME.UI
{
    public class BaseUIScreenView : MonoBehaviour
    {
        [SerializeField] private UIView view;
        
        public virtual void HideView(Action HideComplete = null)
        {
            view.HideBehavior.OnFinished.Event.AddListener(() => HideComplete?.Invoke());
            view.Hide();
        }

        public virtual void ShowView()
        {
            view.Show();
        }
    }
}