using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GAME.Controls;
using GAME.Level;
using UnityEngine;

namespace GAME.Hero
{
    public class HeroFeature : MonoBehaviour
    {
        private LevelFeature _levelFeature;
        private ControlsFeature _controlsFeature;

        [SerializeField] private float heroSpeed;
        
        private HeroView hero;
        private Vector3 moveVector;
        private Vector3 destPoint;
        private bool started;

        private void Awake()
        {
            _levelFeature = FindObjectOfType<LevelFeature>();
            _controlsFeature = FindObjectOfType<ControlsFeature>();
        }

        private void OnEnable()
        {
            _levelFeature.HeroSpawned += CacheHero;
            _controlsFeature.PlayerTappedAt += SetMoveVector;
            _levelFeature.GameStarted += StartMoveAndCollisionLogic;
            _levelFeature.GameComplete += StopMoving;
            _levelFeature.HeroColorizeWithColor += ColorizeHero;
        }

        private void OnDisable()
        {
            _levelFeature.HeroSpawned -= CacheHero;
            _controlsFeature.PlayerTappedAt -= SetMoveVector;
            _levelFeature.GameStarted -= StartMoveAndCollisionLogic;
            hero.trigger.OnTriggerEntered -= HeroCollided;
            _levelFeature.GameComplete += StopMoving;
            _levelFeature.HeroColorizeWithColor -= ColorizeHero;
        }

        private void CacheHero(HeroView _hero)
        {
            hero = _hero;
            hero.trigger.OnTriggerEntered += HeroCollided;
        }

        private void SetMoveVector(Vector3 _destPoint)
        {
            destPoint = _destPoint;
            destPoint.z = 0;
            moveVector = (destPoint - hero.transform.position).normalized;
        }

        private void StartMoveAndCollisionLogic()
        {
            started = true;
        }

        private void Update()
        {
            if (started)
            {
                if (Vector3.Distance(hero.transform.position, destPoint) > 0.1f)
                    hero.transform.position += moveVector * Time.deltaTime * heroSpeed;
            }
        }

        private void HeroCollided(Collider2D otherColl)
        {
            _levelFeature.HeroCollidedWith?.Invoke(otherColl);
            ICollideableWithHero other = otherColl.gameObject.GetComponent(typeof(ICollideableWithHero)) as ICollideableWithHero;
            if (other != null) other.CollidedWithHero();
        }

        private void StopMoving()
        {
            started = false;
        }

        private void ColorizeHero(Color _color)
        {
            foreach (var curRend in hero.renderers)
            {
                curRend.DOColor(_color, 0.15f).SetLoops(4, LoopType.Yoyo)
                    .OnComplete(() => curRend.DOColor(Color.white, 0));
            }
        }
    }
}