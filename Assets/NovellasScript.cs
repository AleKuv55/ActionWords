using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NovellasScript : MonoBehaviour
{
    public GameObject Aliana;
    public GameObject Mother;
    public GameObject FishMan;

    [SerializeField] Sprite[] Sprites;

    private void Start()
    {
        Image AlianaImage = Aliana.GetComponent<Image>();
        AlianaImage.sprite = Sprites[0];
    }
 
}
