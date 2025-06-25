using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager
{
    private static GameLevelManager instance;

    public static GameLevelManager Instance
    {
        get
        {
            if (instance == null)
                 instance=new GameLevelManager();
            return instance;
        }
    }

    private Transform playerBornPos;
    private PlayerObj playerObj;

    private List<ZombieBornPoint> allZombieBornPoint=new List<ZombieBornPoint>();//用于记录所有出怪点
    private int allWaveNum=0;//所有出怪点的总波数
    private int currentAllWaveNum=0;//当前波数
    public int currentZombieNum=0;//当前场景中所有怪物数量
    private GameUI ui;

    public int killZombieNum=0;
    private GameLevelManager()
    {
        
    }

    //关卡初始化
    //1.创建人物并初始化人物数据
    //2.设置摄像机看向玩家
    //3.初始化防御塔血量
    public void InitGameLevel(SceneInfo sceneInfo)
    {
        UIManager.Instance.ShowPanel("GameUI");
        ui = GameObject.Find("GameUI").GetComponent<GameUI>();
        
        RoleInfo roleInfo = GameDataManager.Instance.nowSelectCharacterInfo;
        playerBornPos = GameObject.Find("PlayerBornPos").transform;
        GameObject obj = GameObject.Instantiate(Resources.Load<GameObject>(roleInfo.resourcePath),
            playerBornPos.position, playerBornPos.rotation);
        
        Camera.main.GetComponent<CameraMove>().SetLookAtTarget(obj.transform);
        
        playerObj = obj.GetComponent<PlayerObj>();
        playerObj.InitPlayerInfo(roleInfo.atk,sceneInfo.money);
        
        SafetyAreaObj.Instance.InitHp(sceneInfo);
    }
    
    //记录所有出怪点
    public void AddZombieBornPoint(ZombieBornPoint point)
    {
        allZombieBornPoint.Add(point);
    }

    //记录所有出怪点的总波数
    public void AddAllWaveNum(int num)
    {
        allWaveNum += num;
        currentAllWaveNum = allWaveNum;
        ui.UpdateWaveNumber(currentAllWaveNum,allWaveNum);
    }
    
    //记录所有出怪点的剩余波数
    public void UpdateCurrentWaveNum(int num)
    {
        currentAllWaveNum += num;
        ui.UpdateWaveNumber(currentAllWaveNum,allWaveNum);
    }
    
    //记录场景中怪物总数
    public void AddCurrentZombieNum(int num)
    {
        currentZombieNum += num;
    }
    
    //检查游戏是否胜利
    public bool CheckGameOver()
    {
        for (int i = 0; i < allZombieBornPoint.Count; i++)
        {
            if (!allZombieBornPoint[i].CheckCreateZombieOver())
            {
                return false;
            }
        }

        if (currentZombieNum > 0)
            return false;
        
        return true;
    }
    
    //通关后清除关卡数据
    public void ClearGameLevelData()
    {
        allZombieBornPoint.Clear();
        playerObj = null;
        allWaveNum = currentAllWaveNum = currentZombieNum = killZombieNum=0;
        
    }
    
}
