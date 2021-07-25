using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GAME.Enemies;
using GAME.Fruits;
using GAME.Managers;
using UnityEngine;

namespace GAME.Hero
{
    public class Hero
    {
        public HeroView heroView { get; }
        public int Lives { get; set; }

        private Tweener moveTween;
        
        public Hero(int lives)
        {
            heroView = InstancesFactory.Instance.GetObjectView(nameof(HeroView)) as HeroView;
            Lives = lives;
            EventsManager.Instance.GlobalEvents.GameStarted += StartHeroLogic;
            EventsManager.Instance.GlobalEvents.EndGame += StopHeroLogic;
        }


        private void StartHeroLogic()
        {
            EventsManager.Instance.HeroEvents.HeroCollided += HeroCollisionLogic;
            EventsManager.Instance.HeroEvents.PlayerTapped += MoveHeroTo;
        }

        private void HeroCollisionLogic(Collider2D otherCol)
        {
            if (otherCol.gameObject.GetComponent<FruitView>())
            {
                EventsManager.Instance.GlobalEvents.FruitCollected?.Invoke(otherCol.gameObject.GetComponent<FruitView>());
                heroView.Colorize(Color.green);
            } else if (otherCol.gameObject.GetComponent<EnemyView>())
            {
                EventsManager.Instance.GlobalEvents.EnemyCollided?.Invoke(otherCol.gameObject.GetComponent<EnemyView>());
                heroView.Colorize(Color.magenta);
            }
        }

        private void MoveHeroTo(Vector3 destPos)
        {
            moveTween.Kill();
            moveTween = heroView.transform.DOMove(destPos, 10).SetSpeedBased(true).SetEase(Ease.Linear);
        }

        private void StopHeroLogic()
        {
            moveTween.Kill();
            EventsManager.Instance.HeroEvents.HeroCollided -= HeroCollisionLogic;
            EventsManager.Instance.HeroEvents.PlayerTapped -= MoveHeroTo;
            EventsManager.Instance.GlobalEvents.GameStarted -= StartHeroLogic;
            EventsManager.Instance.GlobalEvents.EndGame -= StopHeroLogic;

        }
    }
}