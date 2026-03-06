using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestItemUI : MonoBehaviour
{
    public Text titleText;
    public Text descriptionText;
    public Text rewardText;

    private QuestRuntime _quest;

    public void InitQuest(QuestRuntime quest)
    {
        _quest = quest;
        RefreshUI();
    }

    public void RefreshUI()
    {
        if (_quest == null) return;
        titleText.text = "任务： "+_quest.config.title;
        descriptionText.text = "内容： "+_quest.config.description+$" ({_quest.currentAmount}/{_quest.config.amount})";
        rewardText.text = $"奖励:  {_quest.config.rewardMoney}";
    }
}
