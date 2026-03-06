using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : BasePanel
{
    public Button returnBtn;
    public Text resultText;
    public Text rewardText;
    
    protected override void Init()
    {

        returnBtn.onClick.AddListener(() =>
        {
            LevelManager.Instance.ClearGameLevelData(); //清除关卡数据
            ResourceManager.Instance.ClearDic(null);
            UIManager.Instance.HidePanel("GameOverPanel");
            UIManager.Instance.HidePanel("GameUI");
            UIManager.Instance.HidePanel("TaskPanel");
            SceneManager.LoadScene("BeginScene");
        });    
    }
    
    //数据初始化
    public void InitInfo(bool isLost)
    {
        if (isLost)
        {
            resultText.text = "You Lost !";
            rewardText.text ="Reward :0";
            GameDataManager.Instance.playerData.money += 0;
            GameDataManager.Instance.SavePlayerData();
        }
        else
        {
            resultText.text = "You Win !";
            rewardText.text = "Reward :2000";
            GameDataManager.Instance.playerData.money +=2000;
            GameDataManager.Instance.SavePlayerData();
        }
    }
    
}
