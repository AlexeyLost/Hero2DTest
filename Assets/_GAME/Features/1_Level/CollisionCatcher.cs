using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAME.Level
{
    public class CollisionCatcher : MonoBehaviour
    {
        public event Action<Collider2D> OnTriggerEntered;
        private void OnTriggerEnter2D(Collider2D other) => OnTriggerEntered?.Invoke(other);
    }
}