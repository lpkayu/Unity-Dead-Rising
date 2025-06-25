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
    public Text moneyText;

    private int money;//用于记录奖励
    protected override void Init()
    {
        returnBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("GameOverPanel");
            UIManager.Instance.HidePanel("GameUI");
            
            GameLevelManager.Instance.ClearGameLevelData(); //清除关卡数据
            SceneManager.LoadScene("BeginScene");
        });    
    }
    
    //数据初始化
    public void InitInfo(bool isLost)
    {
        if (isLost)
        {
            money = GameLevelManager.Instance.killZombieNum * 100;
            resultText.text = "You Lost !";
            rewardText.text = "Failure Rewards :";
            moneyText.text = "$" + money.ToString();
            GameDataManager.Instance.playerData.money += money;
            GameDataManager.Instance.SavePlayerData();
        }
        else
        {
            money = GameLevelManager.Instance.killZombieNum * 200;
            resultText.text = "You Win !";
            rewardText.text = "Victory Reward :";
            moneyText.text = "$" + money.ToString();
            GameDataManager.Instance.playerData.money += money;
            GameDataManager.Instance.SavePlayerData();
        }
    }
    
}
