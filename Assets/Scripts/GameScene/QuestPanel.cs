using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : BasePanel
{
    public Transform questContent;
    public GameObject questItemPrefab;
    // 关闭按钮
    public Button closeButton;

    // 存储当前显示的任务项，key是任务ID
    private Dictionary<int, QuestItemUI> questItems = new Dictionary<int, QuestItemUI>();

    protected override void Init()
    {
        closeButton.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("TaskPanel");
        });
        

        // 监听 QuestManager 的事件
        QuestManager.Instance.OnQuestAccepted += AddQuestItem;
        QuestManager.Instance.OnQuestCompleted += RemoveQuestItem;
        QuestManager.Instance.OnQuestProgressUpdated += UpdateQuestItem;
        
        // 初始化时加载已有任务
        RefreshAllQuests();
    }

    private void RefreshAllQuests()
    {
        // 清理旧的
        foreach (var item in questItems.Values)
        {
            Destroy(item.gameObject);
        }
        questItems.Clear();

        // 加载新的
        List<QuestRuntime> quests = QuestManager.Instance.GetActiveQuests();
        foreach (var quest in quests)
        {
            AddQuestItem(quest);
        }
    }

    private void OnDestroy()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.OnQuestAccepted -= AddQuestItem;
            QuestManager.Instance.OnQuestCompleted -= RemoveQuestItem;
            QuestManager.Instance.OnQuestProgressUpdated -= UpdateQuestItem;
        }
    }

    // 更新任务显示
    private void UpdateQuestItem(QuestRuntime quest)
    {
        if (questItems.ContainsKey(quest.config.id))
        {
            questItems[quest.config.id].RefreshUI();
        }
        else
        {
            // 如果任务进度更新了但还没显示，尝试添加（比如加载存档后）
            AddQuestItem(quest);
        }
    }

    // 添加任务显示
    private void AddQuestItem(QuestRuntime quest)
    {
        if (questItems.ContainsKey(quest.config.id)) return;

        GameObject itemObj = Instantiate(questItemPrefab, questContent);
        QuestItemUI itemUI = itemObj.GetComponent<QuestItemUI>();
        
        if (itemUI != null)
        {
            itemUI.InitQuest(quest);
            questItems.Add(quest.config.id, itemUI);
        }
    }
    
    // 移除任务显示（任务完成后）
    private void RemoveQuestItem(QuestRuntime quest)
    {
        // 如果任务还在激活列表中（说明是循环任务），则不移除，仅刷新
        if (QuestManager.Instance.GetActiveQuests().Exists(q => q.config.id == quest.config.id))
        {
            if (questItems.ContainsKey(quest.config.id))
            {
                questItems[quest.config.id].RefreshUI();
            }
            return;
        }

        if (questItems.ContainsKey(quest.config.id))
        {
            Destroy(questItems[quest.config.id].gameObject);
            questItems.Remove(quest.config.id);
        }
    }
}
