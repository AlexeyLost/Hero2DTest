using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GAME.UI
{
    public class UIFeature : MonoBehaviour
    {
        [SerializeField] private StartScreen startScreenView;
        [SerializeField] private MainScreen mainScreenView;
        [SerializeField] private EndScreen endGameScreenView;

        public event Action StartScreenClosed;
        public Action<short> LivesChanged;
        public Action<short> ScoreChanged;
        public Action<int> GameComplete;
        

        private void OnEnable()
        {
            startScreenView.playBtn.onClick.AddListener(PlayButtonPressed);
            endGameScreenView.restartButton.onClick.AddListener(RestartClicked);
            LivesChanged += SetLivesLbl;
            ScoreChanged += SetScoreLbl;
            GameComplete += GameCompleteActions;
        }

        private void OnDisable()
        {
            startScreenView.playBtn.onClick.RemoveListener(PlayButtonPressed);
            endGameScreenView.restartButton.onClick.RemoveListener(RestartClicked);
            LivesChanged -= SetLivesLbl;
            ScoreChanged -= SetScoreLbl;
            GameComplete -= GameCompleteActions;
        }

        private void PlayButtonPressed()
        {
            startScreenView.HideView(() =>
            {
                StartScreenClosed?.Invoke();
                startScreenView.playBtn.onClick.RemoveListener(PlayButtonPressed);
            });
            mainScreenView.ShowView();
        }

        private void SetLivesLbl(short lives)
        {
            mainScreenView.liveLbl.text = "Lives: " + lives;
        }

        private void SetScoreLbl(short score)
        {
            mainScreenView.scoreLbl.text = "Score: " + score;
        }

        private void GameCompleteActions(int bestScore)
        {
            mainScreenView.HideView();
            endGameScreenView.bestScoreLbl.text = "Best Score: " + bestScore;
            endGameScreenView.ShowView();
        }

        private void RestartClicked()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}