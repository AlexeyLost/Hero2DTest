using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GAME.Level;
using GAME.Managers;
using UnityEngine;

namespace GAME.Hero
{
    public class HeroView : MonoBehaviour
    {
        public List<SpriteRenderer> renderers;

        private void OnTriggerEnter2D(Collider2D other)
        {
            EventsManager.Instance.HeroEvents.HeroCollided?.Invoke(other);
        }

        public void Colorize(Color newCol)
        {
            foreach (var curRend in renderers)
            {
                curRend.DOKill(true);
                curRend.DOColor(newCol, 0.2f).SetLoops(4, LoopType.Yoyo);
            }
        }
    }
}