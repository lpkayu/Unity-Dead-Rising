using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyAreaObj : MonoBehaviour
{
    [SerializeField]private int currentHp;
    [SerializeField]private int maxHp;
    
    public bool isDead;
    
    private static SafetyAreaObj instance;
    public static SafetyAreaObj Instance => instance;

    private GameUI ui;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ui = GameObject.Find("GameUI").GetComponent<GameUI>();
    }

    public void InitHp(SceneInfo sceneInfo)
    {
        maxHp = sceneInfo.safetyAreaHp;
        currentHp = maxHp;
    }
    
    public void UpdateHp()
    {
        ui.UpdateHp(currentHp,maxHp);
    }
    
    public void TakeDamage(int hurt)
    {
        if(isDead)
            return;
        currentHp -= hurt;
        if (currentHp <= 0)
        {
            currentHp = 0;
            isDead = true;
            
            // 禁用玩家输入
            if (GameLevelManager.Instance.playerObj != null)
            {
                GameLevelManager.Instance.playerObj.DisablePlayerInput();
            }
            
            UIManager.Instance.ShowPanel("GameOverPanel");
            GameOverPanel panel = GameObject.Find("GameOverPanel").GetComponent<GameOverPanel>();
            panel.InitInfo(true);
        }
        
        UpdateHp();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Update()
    {
        ui.UpdateHp(currentHp,maxHp);
    }
}
