using System.Collections;
using System.Collections.Generic;
using GAME.Managers;
using UnityEngine;

namespace GAME.UI
{
    public class UIManager
    {
        private static UIManager instance;
        
        public StartScreen startScreenView { get; set; }
        public MainScreen mainScreenView { get; set; }
        public EndScreen endGameScreenView { get; set; }
        public int Lives { get; set; }
        public int BestScore { get; set; }

        public static UIManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new UIManager();
                return instance;
            }
        }

        public void InitUiViews()
        {
            startScreenView.playBtn.onClick.AddListener(PlayButtonClicked);
        }

        private void PlayButtonClicked()
        {
            EventsManager.Instance.UIEvents.PlayButtonClicked?.Invoke();
            startScreenView.HideView();
            mainScreenView.liveLbl.text = "Lives: " + Lives;
            mainScreenView.scoreLbl.text = "Score: 0";
            mainScreenView.ShowView();
        }

        public void UpdateScore(int score)
        {
            mainScreenView.scoreLbl.text = "Score: " + score;
        }

        public void UpdateLives(int lives)
        {
            mainScreenView.liveLbl.text = "Lives: " + lives;
        }

        public void ShowEndScreen(int bestScore)
        {
            mainScreenView.HideView();
            endGameScreenView.bestScoreLbl.text = "Best Score: " + bestScore;
            endGameScreenView.restartButton.onClick.AddListener(() => EventsManager.Instance.GlobalEvents.RestartGame?.Invoke());
            endGameScreenView.ShowView();
            startScreenView.playBtn.onClick.RemoveAllListeners();
        }
    }
}