using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelManager
{
    void AddCrosswordFinishedCallback (Action onLevelFinished);
    void LoadLevel (int levelNumber);

}
