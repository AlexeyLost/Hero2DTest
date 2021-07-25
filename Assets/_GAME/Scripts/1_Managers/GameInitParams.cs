
using Cinemachine;
using GAME.Level;
using GAME.UI;
using UnityEngine;

namespace GAME.Managers
{
    public class GameInitParams
    {
        private PrefabsBD prefabs;
        private GameSettings settings;
        private GameObject envParent;
        private CinemachineVirtualCamera cam;
        private StartScreen startScreenView;
        private MainScreen mainScreenView;
        private EndScreen endGameScreenView;

        public GameInitParams()
        {
            
        }
    }
}