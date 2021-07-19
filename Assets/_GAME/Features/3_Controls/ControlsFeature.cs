using System;
using System.Collections;
using System.Collections.Generic;
using GAME.Level;
using UnityEngine;

namespace GAME.Controls
{
    public class ControlsFeature : MonoBehaviour
    {
        public Action<Vector3> PlayerTappedAt;
        
        private LevelFeature _levelFeature;

        private bool started;
        private Vector3 tapPosition;
        private Camera mainCam;

        private void Awake()
        {
            _levelFeature = FindObjectOfType<LevelFeature>();
        }

        private void Start()
        {
            mainCam = Camera.main;
        }

        private void OnEnable()
        {
            _levelFeature.GameStarted += StartControlsLogic;
        }

        private void OnDisable()
        {
            _levelFeature.GameStarted -= StartControlsLogic;
        }

        private void StartControlsLogic()
        {
            started = true;
        }

        private void Update()
        {
            if (started)
            {
                if (Input.GetMouseButtonDown(0)) 
                    PlayerTappedAt?.Invoke(mainCam.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }
}