using System.Collections;
using System.Collections.Generic;
using GAME.Enemies;
using GAME.Fruits;
using GAME.Hero;
using Unity.Mathematics;
using UnityEngine;

namespace GAME.Managers
{
    public class InstancesFactory
    {
        private static InstancesFactory instance;
        private static PrefabsBD prefabs;

        
        public InstancesFactory()
        {
            
        }

        public static InstancesFactory Instance
        {
            get
            {
                if (instance == null)
                    instance = new InstancesFactory();
                
                return instance;
            }
        }

        public PrefabsBD Prefabs
        {
            set
            {
                prefabs = value;
            }
        }

        public object GetObjectView(string typeName)
        {
            GameObject currentObject;
            switch (typeName)
            {
                case nameof(HeroView):
                    HeroView _heroVIew = GameObject.Instantiate(prefabs.heroPrefab);
                    return _heroVIew;
                case nameof(EnemyView):
                    EnemyView _enemyView = GameObject.Instantiate(prefabs.enemyPrefab);
                    return _enemyView;
                case "Ground":
                    GameObject _ground = GameObject.Instantiate(prefabs.groundPrefab);
                    return _ground;
                case nameof(FruitView):
                    FruitView _fruitView = GameObject.Instantiate(prefabs.fruitPrefab);
                    return _fruitView;
                default:
                    return null;
            }
        }
    }
}