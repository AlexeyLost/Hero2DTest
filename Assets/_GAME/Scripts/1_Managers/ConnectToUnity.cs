using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GAME.Level;
using GAME.UI;
using UnityEngine;

namespace GAME.Managers
{
    public class ConnectToUnity : MonoBehaviour
    {
        [SerializeField] private PrefabsBD prefabs;
        [SerializeField] private GameSettings settings;
        [SerializeField] private GameObject envParent;
        [SerializeField] private CinemachineVirtualCamera cam;
        [SerializeField] private StartScreen startScreenView;
        [SerializeField] private MainScreen mainScreenView;
        [SerializeField] private EndScreen endGameScreenView;

        private Camera mainCam;
        
        private void Start()
        {
            mainCam = Camera.main;
            
            GameManager.Instance.InitGame(new List<object>
            {
                prefabs,
                settings,
                envParent,
                cam,
                startScreenView,
                mainScreenView,
                endGameScreenView,
            });
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                EventsManager.Instance.HeroEvents.PlayerTapped?.Invoke(mainCam.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }
}