using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : BasePanel
{
    public Button returnBtn;
    public Text waveNumberText;
    public Text amountNumberText;
    public Text hpText;
    public Transform towerBtn;
    public List<TowerBtn> towerBtnObj;
    
    public Image hpImg;
    public Image hpEffectImg;
    public float speed;
    
    protected override void Init()
    {
        returnBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("GameUI");
            SceneManager.LoadScene("Scenes/BeginScene");
        });
        
        towerBtn.gameObject.SetActive(false);
    }
    
    //血条缓动效果
    public void UpdateHp(int currentHp,int maxHp)
    {
        hpText.text = currentHp + "/" + maxHp;
        hpImg.fillAmount =(float) currentHp / maxHp;
        hpEffectImg.fillAmount = Mathf.Lerp(hpEffectImg.fillAmount, hpImg.fillAmount, speed*Time.deltaTime);
    }

    public void UpdateWaveNumber(int nowNum,int maxNum)
    {
        waveNumberText.text = nowNum + "/" + maxNum;
    }

    public void UpdateMoney(int num)
    {
        amountNumberText.text = num.ToString();
    }
    
}
