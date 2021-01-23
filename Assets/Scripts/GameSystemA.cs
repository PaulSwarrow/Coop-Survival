using UnityEngine;

namespace DefaultNamespace
{
    public class GameSystemA : GameSystem
    {
        [Inject] private GameSystemB systemB;


        public override void Init()
        {
            Debug.Log(systemB);
        }

        public override void Start()
        {
        }

        public override void Stop()
        {
        }
    }
}