using System;
using Lib.UnityQuickTools;
using Libs.GameFramework;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class SaveLoadSystem : GameSystem
    {
        [Inject] private GameCharacterSystem characters;

        public override void Subscribe()
        {
        }

        public void SpawnCharacters()
        {
            base.Start();
            for (var i = 0; i < 5; i++)
            {
                characters.CreateCharacter(GetRandomPosition(), Geometry.GetRandomForward());
            }
        }

        private Vector3 GetRandomPosition()
        {
            var position = new Vector3(
                Random.Range(-20, 20), 0, Random.Range(-20, 20));
            NavMesh.SamplePosition(position, out var hit, 20, NavMesh.AllAreas);
            return hit.position;
        }

        public override void Unsubscribe()
        {
        }
    }
}