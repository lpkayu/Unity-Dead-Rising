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
    private CreateTowerPoint nowSelectTowerPoint;
    
    //用于检测造塔是否输入
    private bool checkInput;
    
    //血条缓动
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

    public void UpdateTowerBtnInfo(CreateTowerPoint towerPoint)
    {
        nowSelectTowerPoint = towerPoint;
    
        if(nowSelectTowerPoint == null)
        {
            checkInput = false;
            towerBtn.gameObject.SetActive(false);
        }
        else
        {
            checkInput = true;
            towerBtn.gameObject.SetActive(true);
            //没造过塔
            if (nowSelectTowerPoint.nowTowerInfo== null)
            {
                for (int i = 0; i < towerBtnObj.Count; i++)
                {
                    towerBtnObj[i].gameObject.SetActive(true);
                    towerBtnObj[i].InitBtnInfo(nowSelectTowerPoint.towerIds[i], "数字键" + (i + 1));
                }
            }
            else //造过塔
            {
                for (int i = 0; i < towerBtnObj.Count; i++)
                {
                    towerBtnObj[i].gameObject.SetActive(false);
                }
                towerBtnObj[1].gameObject.SetActive(true);
                towerBtnObj[1].InitBtnInfo(nowSelectTowerPoint.nowTowerInfo.nextLev, "空格键");
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        
        if (!checkInput)
            return;
        
        if( nowSelectTowerPoint.nowTowerInfo == null )
        {
            if( Input.GetKeyDown(KeyCode.Alpha1) )
            {
                nowSelectTowerPoint.CreatTower(nowSelectTowerPoint.towerIds[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                nowSelectTowerPoint.CreatTower(nowSelectTowerPoint.towerIds[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                nowSelectTowerPoint.CreatTower(nowSelectTowerPoint.towerIds[2]);
            }
        }
        //造过塔 就检测空格键 去建造
        else
        {
            if( Input.GetKeyDown(KeyCode.Space) )
            {
                nowSelectTowerPoint.CreatTower(nowSelectTowerPoint.nowTowerInfo.nextLev);
            }
        }
        
    }
}
