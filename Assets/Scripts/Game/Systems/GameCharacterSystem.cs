using Game.GameManagerTools;
using Game.Models;
using Libs.GameFramework;
using Libs.GameFramework.Systems;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameCharacterSystem : GameSystem
    {
        [Inject] private ObjectSpawnSystem _spawnSystem;
        [Inject] private PrefabLoader _prefabLoader;
        public override void Subscribe()
        {
        }

        public override void Unsubscribe()
        {
        }

        //todo: type, properties
        //hide by interface?
        public GameCharacter CreateCharacter(Vector3 position, Vector3 forward)
        {
            var character = new GameCharacter();
            character.actor = _spawnSystem.Spawn(_prefabLoader.characterPrefab, position, Quaternion.LookRotation(forward, Vector3.up));
            return character;
        }
    }
}