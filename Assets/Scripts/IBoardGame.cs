using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardGame
{
    // Activates prefab with board game
    void StartBoardGame();
    
    // Clear and hide board
    void EndBoardGame();

    void ExtraAction ();

    void AddWordActivationCallback (Action<string, SpellEffect> callback);
    void AddExtraActionCallback (Action callback);
}
