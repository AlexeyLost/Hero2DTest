using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinemachine;
using DG.Tweening;
using GAME.Enemies;
using GAME.Fruits;
using GAME.Hero;
using GAME.Level;
using GAME.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace GAME.Managers
{
    public class GameManager
    {
        private static GameManager instance;

        private GameObject environmentParent;
        private GameSettings settings;

        private Hero.Hero hero;
        private CinemachineVirtualCamera cam;
        private int bestScore;
        private Dictionary<Fruit, FruitView> fruits;
        private Dictionary<Enemy, EnemyView> enemies;
        private int score;

        public GameManager()
        {
            
        }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new GameManager();

                return instance;
            }
        }

        public void InitGame(List<object> _params)
        {
            
            if (PlayerPrefs.HasKey(PlayerPrefsConst.BestScore))
            {
                bestScore = PlayerPrefs.GetInt(PlayerPrefsConst.BestScore);
            }
            else
            {
                bestScore = 0;
                PlayerPrefs.SetInt(PlayerPrefsConst.BestScore, bestScore);
            }
                
            settings = _params.Find(pr => pr.GetType() == typeof(GameSettings)) as GameSettings;
            environmentParent = _params.Find(pr => pr.GetType() == typeof(GameObject)) as GameObject;
            cam = _params.Find(pr => pr.GetType() == typeof(CinemachineVirtualCamera)) as CinemachineVirtualCamera;
            InstancesFactory.Instance.Prefabs = _params.Find(pr => pr.GetType() == typeof(PrefabsBD)) as PrefabsBD;
            UIManager.Instance.mainScreenView = _params.Find(pr => pr.GetType() == typeof(MainScreen)) as MainScreen;
            UIManager.Instance.startScreenView = _params.Find(pr => pr.GetType() == typeof(StartScreen)) as StartScreen;
            UIManager.Instance.endGameScreenView = _params.Find(pr => pr.GetType() == typeof(EndScreen)) as EndScreen;
            UIManager.Instance.Lives = settings.startLives;
            UIManager.Instance.BestScore = bestScore;
            UIManager.Instance.InitUiViews();
            SpawnGround();
            SpawnHero();
            SpawnEnemies();
            SpawnFruits();
            EventsManager.Instance.UIEvents.PlayButtonClicked += StartGame;
        }

        private void StartGame()
        {
            EventsManager.Instance.GlobalEvents.FruitCollected += DestroyFruit;
            EventsManager.Instance.GlobalEvents.EnemyCollided += DecreaseLife;
            EventsManager.Instance.GlobalEvents.EndGame += EndGameActions;
            EventsManager.Instance.GlobalEvents.RestartGame += RestartGame;
            EventsManager.Instance.GlobalEvents.GameStarted?.Invoke();
        }

        private void SpawnGround()
        {
            GameObject _ground = InstancesFactory.Instance.GetObjectView("Ground") as GameObject;
            _ground.transform.SetParent(environmentParent.transform);
        }
        
        private void SpawnHero()
        {
            hero = new Hero.Hero(settings.startLives);
            cam.Follow = hero.heroView.transform;
            cam.LookAt = hero.heroView.transform;
        }

        private void SpawnEnemies()
        {
            enemies = new Dictionary<Enemy, EnemyView>();
            for (int i = 0; i < settings.enemiesCount; i++)
            {
                Enemy curEnemy = new Enemy();
                curEnemy.enemyView.transform.parent = environmentParent.transform;
                Vector2 spawnVector = Random.insideUnitCircle.normalized;
                curEnemy.enemyView.transform.position = spawnVector *
                                                        Random.Range(settings.minEnemiesSpawnRadius,
                                                            settings.maxEnemiesSpawnRadius);
                enemies.Add(curEnemy, curEnemy.enemyView);
            }
        }

        private void SpawnFruits()
        {
            fruits = new Dictionary<Fruit, FruitView>();
            for (int i = 0; i < settings.fruitsStartNum; i++)
            {
                Fruit curFruit = new Fruit();
                curFruit.fruitView.transform.SetParent(environmentParent.transform);
                Vector2 spawnVector = Random.insideUnitCircle.normalized;
                curFruit.fruitView.transform.position = spawnVector *
                                                        Random.Range(settings.minFruitsSpawnRadius,
                                                            settings.maxFruitsSpawnRadius);
                
                fruits.Add(curFruit, curFruit.fruitView);
            }
        }

        private void DestroyFruit(FruitView curFruitView)
        {
            curFruitView.col.enabled = false;
            Fruit curFruit = fruits.FirstOrDefault(fr => fr.Value == curFruitView).Key;
            curFruitView.transform.DOScale(0, 0.3f).OnComplete(() =>
            {
                GameObject.Destroy(curFruitView);
                fruits.Remove(curFruit);
            });
            score++;
            UIManager.Instance.UpdateScore(score);
        }

        private void DecreaseLife(EnemyView curEnemyView)
        {
            curEnemyView.col.enabled = false;
            hero.Lives--;
            curEnemyView.BiteAnimation(0.4f);
            UIManager.Instance.UpdateLives(hero.Lives);
            if (hero.Lives == 0) EventsManager.Instance.GlobalEvents.EndGame?.Invoke();
        }

        private void EndGameActions()
        {
            if (score > bestScore)
            {
                bestScore = score;
                PlayerPrefs.SetInt(PlayerPrefsConst.BestScore, bestScore);
            }
            UIManager.Instance.ShowEndScreen(bestScore);
        }

        private void RestartGame()
        {
            fruits.Clear();
            enemies.Clear();
            score = 0;
            bestScore = 0;
            EventsManager.Instance.GlobalEvents.FruitCollected -= DestroyFruit;
            EventsManager.Instance.GlobalEvents.EnemyCollided -= DecreaseLife;
            EventsManager.Instance.GlobalEvents.EndGame -= EndGameActions;
            EventsManager.Instance.GlobalEvents.RestartGame -= RestartGame;
            EventsManager.Instance.UIEvents.PlayButtonClicked -= StartGame;
            UIManager.Instance.endGameScreenView.restartButton.onClick.RemoveAllListeners();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}