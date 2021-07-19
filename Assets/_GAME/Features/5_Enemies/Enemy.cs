using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using GAME.Level;
using UnityEngine;

namespace GAME.Enemies
{
    public class Enemy : MonoBehaviour, ICollideableWithHero
    {
        [SerializeField] private Collider2D col;
        
        public void CollidedWithHero()
        {
            StartCoroutine(EnableColliderAfterDelay(1.5f));
            transform.DOShakeScale(0.1f, new Vector3(1.2f, 1.2f, 1f)).OnComplete(() =>
            {
                transform.localScale = Vector3.one;
            });
        }

        private IEnumerator EnableColliderAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            col.enabled = true;
        }
    }
}