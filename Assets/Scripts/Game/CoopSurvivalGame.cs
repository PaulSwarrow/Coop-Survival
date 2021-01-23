using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Game.Actors;
using Game.GameManagerTools;
using UnityEngine;

public class CoopSurvivalGame : GameManager
{
    [Serializable]
    public class Prefabs
    {
        public GameCharacterActor characterPrefab;

    }
    protected override void RegisterDependencies()
    {
        Register(new PlayerControllerSystem());
        Register(new GameCharacterSystem());
        Register(GetComponent<PrefabLoader>());
    }
}
