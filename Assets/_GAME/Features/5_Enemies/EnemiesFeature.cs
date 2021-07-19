using System;
using System.Collections;
using System.Collections.Generic;
using GAME.Level;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GAME.Enemies
{
    public class EnemiesFeature : MonoBehaviour
    {
        private LevelFeature _levelFeature;

        private List<Enemy> enemies;
        private float enemySpeed = 3f;
        private bool moving;
        
        private void Awake()
        {
            _levelFeature = FindObjectOfType<LevelFeature>();
        }

        private void OnEnable()
        {
            _levelFeature.GameStarted += StartMoveLogic;
            _levelFeature.GameComplete += StopMoving;
        }

        private void OnDisable()
        {
            _levelFeature.GameStarted -= StartMoveLogic;
            _levelFeature.GameComplete -= StopMoving;
        }

        private void StartMoveLogic()
        {
            enemies = new List<Enemy>();
            enemies.AddRange(_levelFeature.Enemies);
            moving = true;
            foreach (var curEnemy in enemies)
            {
                curEnemy.StartCoroutine(MoveCoroutine(curEnemy));
            }
        }

        private IEnumerator MoveCoroutine(Enemy currentEnemy)
        {
            while (moving)
            {
                Vector3 direction = Random.insideUnitCircle.normalized;
                float t = 0;
                while (t < Random.Range(1f, 4f))
                {
                    t += Time.deltaTime;
                    currentEnemy.transform.position += direction * Time.deltaTime * enemySpeed;
                    yield return null;
                }

                yield return null;
            }
        }

        private void StopMoving()
        {
            moving = false;
            foreach (var curEnemy in enemies)
            {
                curEnemy.StopAllCoroutines();
            }
        }
    }
}