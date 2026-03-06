using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class LevelManager:Singleton<LevelManager>
{
    private Transform playerBornPos;
    public PlayerObj playerObj;

    private List<ZombieBornPoint> allZombieBornPoint=new List<ZombieBornPoint>();//用于记录所有出怪点
    private int allWaveNum=0;//所有出怪点的总波数
    private int currentAllWaveNum=0;//当前波数

    private List<ZombieObj> zombieList=new List<ZombieObj>();//记录当前场景中所有怪物
    
    private GameUI ui;
    
    private LevelManager()
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
        GameObject obj = GameObject.Instantiate(ResourceManager.Instance.Load<GameObject>(roleInfo.resourcePath),
            playerBornPos.position, playerBornPos.rotation);
        
        Camera.main.GetComponent<ThirdPersonCamera>().SetLookAtTarget(obj.transform);
        
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
        currentAllWaveNum = 0;
        ui.UpdateWaveNumber(currentAllWaveNum,allWaveNum);
    }
    
    //记录所有出怪点的剩余波数
    public void UpdateCurrentWaveNum(int num)
    {
        currentAllWaveNum += num;
        ui.UpdateWaveNumber(currentAllWaveNum,allWaveNum);
    }

    //添加和移除场景中的僵尸数量
    public void AddZombieList(ZombieObj zombieObj)
    {
        zombieList.Add(zombieObj);
    }
    public void RemoveZombieList(ZombieObj zombieObj)
    {
        zombieList.Remove(zombieObj);
    }
    
    //检查游戏是否胜利
    public void CheckWaveStatus()
    {
        WaveManager wm = Object.FindObjectOfType<WaveManager>();
        if (wm != null)
        {
            // 如果所有怪物都清空了
            if (zombieList.Count == 0)
            {
                // 1. 如果当前波次的怪物已经全部生成完毕（即 isRunning=false）
                // 仅处理最后一波的胜利逻辑，中间波次的任务触发交由 WaveManager 在下一波开始时触发
                if (wm.IsWaveCleared)
                {
                    // 触发任务：完成当前波次 (仅针对最后一波)
                    if (wm.CurrentWave == wm.totalWaves)
                    {
                        QuestEventHub.TriggerWaveCleared(wm.CurrentWave);
                        Debug.Log($"最后一波 {wm.CurrentWave} 清理完成！");

                        if (playerObj != null)
                        {
                            playerObj.DisablePlayerInput();
                        }
                        // 游戏胜利逻辑
                        UIManager.Instance.ShowPanel("GameOverPanel");
                        GameOverPanel panel = GameObject.Find("GameOverPanel").GetComponent<GameOverPanel>();
                        panel.InitInfo(false);
                    }
                }
            }
        }
    }
    
    //检查游戏是否胜利
    public bool CheckGameOver()
    {
        // CheckWaveStatus已经包含了通关逻辑
        WaveManager wm = Object.FindObjectOfType<WaveManager>();
        return wm != null && wm.CurrentWave == wm.totalWaves && wm.IsWaveCleared && zombieList.Count == 0;
    }
    
    //通关后清除关卡数据
    public void ClearGameLevelData()
    {
        allZombieBornPoint.Clear();
        zombieList.Clear();
        playerObj = null;
        allWaveNum = currentAllWaveNum=0;
        
        // 清除任务数据（如果任务是随关卡重置的）
        QuestManager.Instance.ClearQuestData();
    }
    
}
