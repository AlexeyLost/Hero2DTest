using System.Collections;
using System.Collections.Generic;
using GAME.Enemies;
using GAME.Fruits;
using GAME.Hero;
using UnityEngine;

namespace GAME.Managers
{
    [CreateAssetMenu(fileName = "PrefabsBD", menuName = "Game/PrefabsBD")]
    public class PrefabsBD : ScriptableObject
    {
        public HeroView heroPrefab;
        public GameObject groundPrefab;
        public EnemyView enemyPrefab;
        public FruitView fruitPrefab;
    }
}