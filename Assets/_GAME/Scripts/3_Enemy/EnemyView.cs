using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace GAME.Enemies
{
    public class EnemyView : MonoBehaviour
    {
        public Collider2D col;
        private Tweener biteTween;

        public void BiteAnimation(float duration)
        {
            col.enabled = false;
            biteTween.Kill(true);
            biteTween = transform.DOScale(Vector3.one * 1.3f, duration).SetLoops(2, LoopType.Yoyo)
                .OnComplete(() => col.enabled = true);
        }

        public IEnumerator MoveCoroutine()
        {
            while (true)
            {
                Vector3 direction = Random.insideUnitCircle.normalized;
                float t = 0;
                while (t < Random.Range(1f, 4f))
                {
                    t += Time.deltaTime;
                    transform.position += direction * Time.deltaTime * 3f;
                    yield return null;
                }

                yield return null;
            }
        }
    }
}