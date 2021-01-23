using Game.Actors;
using Game.Tools;
using UnityEngine;

namespace Game.Models
{
    public class GameCharacter
    {
        public GameCharacterActor actor;

        public void Move(Vector3 inputVector)
        {
            actor.motor.Move(inputVector);
        }
    }
}