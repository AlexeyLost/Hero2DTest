using System;
using System.Collections;
using System.Collections.Generic;
using GAME.Enemies;
using GAME.Fruits;
using UnityEngine;

namespace GAME.Managers
{
    public class EventsManager
    {
        private static EventsManager instance;
        
        public HeroEventsBus HeroEvents { get; }
        public EnemyEventsBus EnemyEvents { get; }
        public FruitEventBus FruitEvents { get; }
        public GlobalEventBus GlobalEvents { get; }

        public UIEventBus UIEvents { get; }
        public class HeroEventsBus
        {
            public Action<Collider2D> HeroCollided;
            public Action<Vector3> PlayerTapped;
        }
        
        public class EnemyEventsBus
        {
        }

        public class FruitEventBus
        {
            
        }
        
        public class GlobalEventBus
        {
            public Action GameStarted;
            public Action<FruitView> FruitCollected;
            public Action<EnemyView> EnemyCollided;
            public Action EndGame;
            public Action RestartGame;
        }
        
        public class UIEventBus
        {
            public Action PlayButtonClicked;
        }
        
        private EventsManager()
        {
            HeroEvents = new HeroEventsBus();
            EnemyEvents = new EnemyEventsBus();
            FruitEvents = new FruitEventBus();
            GlobalEvents = new GlobalEventBus();
            UIEvents = new UIEventBus();
        }

        public static EventsManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new EventsManager();

                return instance;
            }
        }
        
        
    }
}