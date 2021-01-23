using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DefaultNamespace;
using Game.Actors;
using Game.GameManagerTools;
using Game.Systems;
using UnityEngine;

public class CoopSurvivalGame : GameManager
{
    [SerializeField] private CinemachineFreeLook virtualCamera;
    protected override void RegisterDependencies()
    {
        //register systems:
        Register(new PlayerControllerSystem());
        Register(new GameCharacterSystem());
        Register(new ObjectSpawnSystem());
        Register(new PlayerCameraSystem());
        
        //register objects:
        Register(GetComponent<PrefabLoader>());
        Register(virtualCamera);
    }
}
