using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OnImangeChoose : MonoBehaviour
{
    public Sprite[] SpriteArray;



    public int NumberLevelFirstLocation;
    public int NumberLevelSecondLocation;


    public void SetBackground(int levelNumber)
    {
        if (levelNumber > NumberLevelFirstLocation)
        {
            this.gameObject.GetComponent<Image>().sprite = SpriteArray[1];
        }
        else
        {
            this.gameObject.GetComponent<Image>().sprite = SpriteArray[0];
        }

    }

}
