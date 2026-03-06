using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTowerPoint : MonoBehaviour
{
    public TowerInfo nowTowerInfo;
    private GameObject towerObj;
    public List<int> towerIds;

    private GameUI ui;

    private void Awake()
    {
        nowTowerInfo = null;
    }

    private void Start()
    {
        ui = GameObject.Find("GameUI").GetComponent<GameUI>();
    }

    public void CreatTower(int towerId)
    {
        TowerInfo info = TowerData.Instance.towerInfos[towerId-1];
       
        if(LevelManager.Instance.playerObj.money<info.money)
            return;

        LevelManager.Instance.playerObj.AddMoney(-info.money);
        if (towerObj != null)
        {
            Destroy(towerObj);
            towerObj = null;
        }
        towerObj = Instantiate(ResourceManager.Instance.Load<GameObject>("Tower/"+info.res),this.transform.position,Quaternion.identity);
        towerObj.GetComponent<TowerObj>().InitTowerInfo(info);

        nowTowerInfo = info;
        
        // 触发任务事件
        QuestEventHub.TriggerTowerBuilt(towerId);
        
        if(nowTowerInfo.nextLev != 0)
        {
            ui.UpdateTowerBtnInfo(this);
        }
        else
        {
            ui.UpdateTowerBtnInfo(null);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (nowTowerInfo != null && nowTowerInfo.nextLev == 0) 
            return;
        ui.UpdateTowerBtnInfo(this);
    }

    private void OnTriggerExit(Collider other)
    {
        ui.UpdateTowerBtnInfo(null);
    }
}
