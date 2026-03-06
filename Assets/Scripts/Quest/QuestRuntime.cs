using System.Collections.Generic;
using UnityEngine;

// 运行时：任务的状态
public class QuestRuntime
{
    public QuestItemData config;
    public int currentAmount;
    public bool isCompleted = false;

    public QuestRuntime(QuestItemData data)
    {
        this.config = data;
        this.currentAmount = 0;
    }

    public bool IsComplete => currentAmount >= config.amount;
    
    public bool CheckCompletion()
    {
        return IsComplete;
    }
}
