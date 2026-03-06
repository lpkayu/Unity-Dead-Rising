using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_GameScene : MonoBehaviour
{
    public QuestData questConfig;

    void Start()
    {
        AcceptAllQuests();
    }

    void AcceptAllQuests()
    {
        foreach (var quest in questConfig.quests)
        {
            QuestManager.Instance.AcceptQuest(quest);
        }
    }
    
}
