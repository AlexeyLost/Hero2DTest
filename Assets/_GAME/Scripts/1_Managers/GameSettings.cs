using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GAME.Level
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSetting")]
    public class GameSettings : ScriptableObject
    {
        public short startLives;
        public int fruitsStartNum;
        public float minFruitsSpawnRadius;
        public float maxFruitsSpawnRadius;
        public int enemiesCount;
        public float minEnemiesSpawnRadius;
        public float maxEnemiesSpawnRadius;
    }
}