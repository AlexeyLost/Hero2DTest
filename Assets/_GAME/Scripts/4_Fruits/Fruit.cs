using System.Collections;
using System.Collections.Generic;
using GAME.Managers;
using UnityEngine;

namespace GAME.Fruits
{
    public class Fruit
    {
        public FruitView fruitView { get; }

        public Fruit()
        {
            fruitView = InstancesFactory.Instance.GetObjectView(nameof(FruitView)) as FruitView;
        }
    }
}