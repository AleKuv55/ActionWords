﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDisabler : MonoBehaviour
{
    public void Disable()
    {
        this.gameObject.SetActive(false);
    }
}
