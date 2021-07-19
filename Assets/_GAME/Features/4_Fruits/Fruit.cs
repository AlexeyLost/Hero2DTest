using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GAME.Level;
using UnityEngine;

namespace GAME.Fruits
{
    public class Fruit : MonoBehaviour, ICollideableWithHero
    {
        public void CollidedWithHero()
        {
            transform.DOScale(0, 0.5f).OnComplete(() => Destroy(gameObject));
        }
    }
}