using Game.Configs;
using Libs.GameFramework.Interfaces;
using UnityEngine;

namespace App.Modules
{
    public class ResourcesProvider : MonoBehaviour, IAppModule
    {
        public ParkourConfig parkourConfig  { get; private set; }
        public WeaponConfig[] weapons { get; private set; }

        public void Init()
        {
            parkourConfig = Resources.Load<ParkourConfig>("Configs/ParkourMotions");
            weapons = Resources.LoadAll<WeaponConfig>("Configs/");
            Debug.Log(weapons.Length);
        }
    }
}