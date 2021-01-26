using System;
using Cinemachine;
using DefaultNamespace;
using Game.GameManagerTools;
using Libs.GameFramework;
using Libs.GameFramework.Systems;
using Mirror;
using UnityEngine;

public class GameManager : BaseGameManager
{
    [SerializeField] private CinemachineFreeLook virtualCamera;

    public event Action SessionStartEvent;

    protected override void RegisterDependencies()
    {
        //register systems:
        Register(new PlayerControllerSystem());
        Register(new GameCharacterSystem());
        Register<ObjectSpawnSystem>(new NetObjectSpawnSystem());
        // Register(new PlayerCameraSystem());
        Register(new SaveLoadSystem());
        Register(GetNetworkSystem());

        //register objects:
        Register(GetComponent<PrefabLoader>());
        Register(virtualCamera);
        Register(Camera.main);
    }

    private GameNetworkSystem GetNetworkSystem()
    {
        if (NetworkManager.singleton.mode == NetworkManagerMode.Host)
            return new GameHostSystem();
        return new GameClientSystem();
    }
}