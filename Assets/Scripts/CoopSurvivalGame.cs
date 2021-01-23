using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class CoopSurvivalGame : GameManager
{
    protected override void RegisterDependencies()
    {
        Register(new GameSystemA());
        Register(new GameSystemB());
    }
}
