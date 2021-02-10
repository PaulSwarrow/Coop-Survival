using Game.Configs;
using UnityEngine;

namespace App.Modules
{
    public class ResourcesProvider : MonoBehaviour
    {
        [SerializeField] private ParkourConfig _parkourConfig;


        public ParkourConfig GetParkourConfig() => _parkourConfig;

    }
}