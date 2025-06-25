using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_GameScene : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Instance.ShowPanel("GameUI");
    }
    
}
