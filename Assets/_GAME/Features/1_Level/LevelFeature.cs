using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GAME.Enemies;
using GAME.Fruits;
using GAME.Hero;
using GAME.UI;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GAME.Level
{
    public class LevelFeature : MonoBehaviour
    {
        public Action LevelInitialized;
        public Action GameStarted;
        public Action<HeroView> HeroSpawned;
        public Action<Collider2D> HeroCollidedWith;
        public Action<Color> HeroColorizeWithColor;
        public event Action GameComplete;

        [SerializeField] private GameSettings m_settings;
        [SerializeField] private GameObject environmentParent;
        [SerializeField] private GameObject groundPrefab;
        [SerializeField] private HeroView heroPrefab;
        [SerializeField] private Fruit fruitPrefab;
        [SerializeField] private Enemy enemyPrefab;
        [SerializeField] private CinemachineVirtualCamera cam;

        private UIFeature _uiFeature;

        private GameObject ground;
        private HeroView hero;
        private List<Fruit> fruits;
        private int bestScore;
        private short score;
        public short Lives { get; private set; }
        public List<Enemy> Enemies { get; private set; }

        private void Awake()
        {
            _uiFeature = FindObjectOfType<UIFeature>();
        }

        private void OnEnable()
        {
            _uiFeature.StartScreenClosed += StartGame;
            HeroCollidedWith += OnHeroCollide;
        }

        private void OnDisable()
        {
            _uiFeature.StartScreenClosed -= StartGame;
            HeroCollidedWith -= OnHeroCollide;
        }

        private void Start()
        {
            InitLevel();
        }

        private void InitLevel()
        {
            SpawnGround();
            SpawnHero();
            SpawnFruits();
            SpawnEnemies();
            GetBestScore();
            score = 0;
            Lives = m_settings.startLives;
            _uiFeature.LivesChanged?.Invoke(Lives);
            _uiFeature.ScoreChanged?.Invoke(score);
            
            LevelInitialized?.Invoke();
        }

        private void SpawnGround()
        {
            ground = Instantiate(groundPrefab, Vector3.zero, quaternion.identity);
            ground.transform.SetParent(environmentParent.transform);
        }

        private void SpawnHero()
        {
            hero = Instantiate(heroPrefab, Vector3.zero, Quaternion.identity);
            cam.m_Follow = hero.transform;
            cam.m_LookAt = hero.transform;
            HeroSpawned?.Invoke(hero);
        }

        private void SpawnFruits()
        {
            fruits = new List<Fruit>();
            for (int i = 0; i < m_settings.fruitsStartNum; i++)
            {
                Vector2 spawnVector = Random.insideUnitCircle.normalized;
                Fruit currentFruit = Instantiate(fruitPrefab, spawnVector * Random.Range(m_settings.minFruitsSpawnRadius, m_settings.maxFruitsSpawnRadius), Quaternion.identity);
                currentFruit.transform.SetParent(environmentParent.transform);
                fruits.Add(currentFruit);
            }
        }

        private void SpawnEnemies()
        {
            Enemies = new List<Enemy>();
            for (int i = 0; i < m_settings.enemiesCount; i++)
            {
                Vector2 spawnVector = Random.insideUnitCircle.normalized;
                Enemy currentEnemy = Instantiate(enemyPrefab, spawnVector * Random.Range(m_settings.minEnemiesSpawnRadius, m_settings.maxEnemiesSpawnRadius), Quaternion.identity);
                currentEnemy.transform.SetParent(environmentParent.transform);
                Enemies.Add(currentEnemy);
            }
        }
        
        private void StartGame()
        {
            GameStarted?.Invoke();
        }

        private void GetBestScore()
        {
            if (PlayerPrefs.HasKey(PlayerPrefsConst.BestScore))
                bestScore = PlayerPrefs.GetInt(PlayerPrefsConst.BestScore);
            else
            {
                bestScore = 0;
                PlayerPrefs.SetInt(PlayerPrefsConst.BestScore, bestScore);
            }
        }

        private void OnHeroCollide(Collider2D otherColl)
        {
            if (otherColl.GetComponent<Fruit>())
            {
                score++;
                _uiFeature.ScoreChanged?.Invoke(score);
                fruits.Remove(otherColl.GetComponent<Fruit>());
                otherColl.enabled = false;
                HeroColorizeWithColor?.Invoke(Color.green);
                if (fruits.Count == 0)
                {
                    SaveBestScoreIfItBest();
                    _uiFeature.GameComplete?.Invoke(bestScore);
                    GameComplete?.Invoke();
                }
            } else if (otherColl.GetComponent<Enemy>())
            {
                Lives--;
                _uiFeature.LivesChanged?.Invoke(Lives);
                otherColl.enabled = false;
                HeroColorizeWithColor?.Invoke(Color.red);
                if (Lives == 0)
                {
                    SaveBestScoreIfItBest();
                    _uiFeature.GameComplete?.Invoke(bestScore);
                    GameComplete?.Invoke();
                }
            }
        }

        private void SaveBestScoreIfItBest()
        {
            if (score > bestScore)
            {
                bestScore = score;
                PlayerPrefs.SetInt(PlayerPrefsConst.BestScore, bestScore);
            }
        }
    }
}