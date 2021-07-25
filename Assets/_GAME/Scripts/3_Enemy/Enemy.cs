using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GAME.Managers;
using Random = UnityEngine.Random;

namespace GAME.Enemies
{
    public class Enemy
    {
        public EnemyView enemyView { get; }

        private bool move;
        
        public Enemy()
        {
            enemyView = InstancesFactory.Instance.GetObjectView(nameof(EnemyView)) as EnemyView;
            EventsManager.Instance.GlobalEvents.GameStarted += StartMoveLogic;
            EventsManager.Instance.GlobalEvents.EndGame += StopMovement;
        }

        private void StartMoveLogic()
        {
            enemyView.StartCoroutine(enemyView.MoveCoroutine());
        }

        private void StopMovement()
        {
            enemyView.col.enabled = false;
            enemyView.StopAllCoroutines();
            EventsManager.Instance.GlobalEvents.GameStarted -= StartMoveLogic;
            EventsManager.Instance.GlobalEvents.EndGame -= StopMovement;
        }
    }
}