using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
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

    private PlayerInputController _playerInputController;
    
    //用于判断是否要进行造塔
    private bool createInput;
    
    //血条缓动
    public Image hpImg;
    public Image hpEffectImg;
    public float speed;

    protected override void Awake()
    {
        base.Awake();
        _playerInputController = new PlayerInputController();
    }
    
     protected override void Start()
     {
         base.Start();
         _playerInputController.UI.CreateTower.performed+=CreateTower;
     }
     
    private void OnEnable()
    {
        _playerInputController.Enable();
    }

    private void OnDisable()
    {
        _playerInputController.UI.CreateTower.performed -= CreateTower;
        _playerInputController.Disable();
    }

    protected override void Init()
    { returnBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("GameUI");
            SceneManager.LoadScene("Scenes/BeginScene");
        });
        
        towerBtn.gameObject.SetActive(false);
    }
    
   

    private void CreateTower(InputAction.CallbackContext obj)
    {
        if(!createInput)
            return;
        
        
        if (nowSelectTowerPoint.nowTowerInfo == null)
        {
            
            if (obj.control is KeyControl control)
            {
                switch (control.keyCode)
                {
                    case Key.Digit1:
                        nowSelectTowerPoint.CreatTower(nowSelectTowerPoint.towerIds[0]);
                        break;
                    case Key.Digit2:
                        nowSelectTowerPoint.CreatTower(nowSelectTowerPoint.towerIds[1]);
                        break;
                    case Key.Digit3:
                        nowSelectTowerPoint.CreatTower(nowSelectTowerPoint.towerIds[2]);
                        break;
                }
            }
        }
        else
        {
            if (obj.control is KeyControl control)
            {
                switch (control.keyCode)
                {
                    case Key.Space:
                        nowSelectTowerPoint.CreatTower(nowSelectTowerPoint.nowTowerInfo.nextLev);
                        break;
                }
            }
        }
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
            createInput = false;
            towerBtn.gameObject.SetActive(false);
        }
        else
        {
            createInput = true;
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
}
