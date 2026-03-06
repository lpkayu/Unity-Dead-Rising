using System.Collections.Generic;
using UnityEngine;

public class QuestManager : SingletonAuto<QuestManager>
{
    // 当前激活的任务列表
    private List<QuestRuntime> activeQuests = new List<QuestRuntime>();
    
    // 已完成的任务ID（用于存档）
    private HashSet<int> completedQuestIds = new HashSet<int>();

    // UI事件
    public event System.Action<QuestRuntime> OnQuestAccepted;
    public event System.Action<QuestRuntime> OnQuestProgressUpdated;
    public event System.Action<QuestRuntime> OnQuestCompleted;

    private void Start()
    {
        // 注册事件监听
        QuestEventHub.OnZombieKilled += HandleZombieKilled;
        QuestEventHub.OnTowerBuilt += HandleTowerBuilt;
        QuestEventHub.OnWaveCleared += HandleWaveCleared;
    }

    private void OnDestroy()
    {
        // 记得移除监听，防止内存泄漏
        QuestEventHub.OnZombieKilled -= HandleZombieKilled;
        QuestEventHub.OnTowerBuilt -= HandleTowerBuilt;
        QuestEventHub.OnWaveCleared -= HandleWaveCleared;
    }

    // 接受新任务
    public void AcceptQuest(QuestItemData questData)
    {
        // 检查是否已经接了该任务，如果已经接了就不重复添加
        if (activeQuests.Exists(q => q.config.id == questData.id)) return;
        
        QuestRuntime newQuest = new QuestRuntime(questData);
        activeQuests.Add(newQuest);
        Debug.Log($"接受任务: {questData.title}");
        
        // 通知UI更新
        OnQuestAccepted?.Invoke(newQuest);
    }

    // 处理击杀事件
    private void HandleZombieKilled(int zombieId)
    {
        UpdateProgress(QuestGoalType.KillZombie, zombieId, 1);
    }

    // 处理造塔事件
    private void HandleTowerBuilt(int towerId)
    {
        UpdateProgress(QuestGoalType.BuildTower, towerId, 1);
    }

    // 处理波次完成事件
    private void HandleWaveCleared(int waveIndex)
    {
        //任务要求“存活过第3波”，那么 targetId=3, amount=1
        //任务要求“存活过任意波次”，那么 targetId=0, amount=1
        UpdateProgress(QuestGoalType.SurviveWave, waveIndex, 1);
    }

    // 通用的进度更新逻辑
    private void UpdateProgress(QuestGoalType type, int targetId, int amount)
    {
        for (int i = activeQuests.Count - 1; i >= 0; i--)
        {
            var quest = activeQuests[i];
            if (quest.isCompleted) continue;

            bool questUpdated = false;
            
            if (quest.config.type != type) continue;
            // 判断ID是否匹配 (0代表通配，比如"杀任意僵尸")
            if (quest.config.targetId != 0 && quest.config.targetId != targetId) continue;
            
            if (!quest.IsComplete)
            {
                quest.currentAmount += amount;
                questUpdated = true;
                Debug.Log($"任务 [{quest.config.title}] 进度更新: {quest.currentAmount}/{quest.config.amount}");
            }

            if (questUpdated)
            {
                // 通知UI刷新单个任务
                OnQuestProgressUpdated?.Invoke(quest);
                CheckQuestCompletion(quest);
            }
        }
    }

    private void CheckQuestCompletion(QuestRuntime quest)
    {
        if (quest.CheckCompletion())
        {
            quest.isCompleted = true;
            completedQuestIds.Add(quest.config.id);
            Debug.Log($"任务完成: {quest.config.title}, 获得奖励: {quest.config.rewardMoney}");
            
            // 发放奖励
            if(LevelManager.Instance.playerObj != null)
            {
                LevelManager.Instance.playerObj.AddMoney(quest.config.rewardMoney);
            }

            // 处理循环任务与一次性任务
            if (quest.config.repeatable)
            {
                // 先通知UI任务完成（播放特效）
                OnQuestCompleted?.Invoke(quest);
                
                // 然后重置进度
                quest.currentAmount = 0;
                quest.isCompleted = false; // 重置状态以便再次触发
                
                // 最后通知UI进度重置
                OnQuestProgressUpdated?.Invoke(quest);
            }
            else
            {
                // 一次性任务：先从激活列表中移除
                // 由于 CheckQuestCompletion 是在 UpdateProgress 的反向循环中调用的，直接 Remove 是安全的。
                activeQuests.Remove(quest);
                
                // 然后通知UI任务完成并移除
                // 注意：必须先Remove再Invoke，否则UI在检查ActiveQuests时会误判为循环任务而不销毁
                OnQuestCompleted?.Invoke(quest);
            }
        }
    }

    public void ClearQuestData()
    {
        activeQuests.Clear();
        // completedQuestIds.Clear(); // 完成的任务记录是否清除视需求而定，这里先不清
    }

    public List<QuestRuntime> GetActiveQuests()
    {
        return activeQuests;
    }
}
