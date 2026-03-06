using System.Collections.Generic;
using UnityEngine;

public enum QuestGoalType
{
    KillZombie, // 杀敌
    BuildTower, // 建塔
    SurviveWave // 存活波次
}

// 静态配置：单个任务
[System.Serializable]
public class QuestItemData
{
    public int id;
    public string title;
    [TextArea] public string description;
    
    public QuestGoalType type;
    public int targetId;//目标ID,0代表任意
    public int amount;//需要数量
    public bool repeatable = false;//是否可重复完成
    
    public int rewardMoney; 
}

[CreateAssetMenu( menuName = "ScriptableObject/QuestData")]
public class QuestData : ScriptableObject
{
    public List<QuestItemData> quests;
}
