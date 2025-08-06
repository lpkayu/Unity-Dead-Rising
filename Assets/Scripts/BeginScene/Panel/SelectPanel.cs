using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectPanel : BasePanel
{
    public Button returnBtn;
    public Button startBtn;
    
    public Button leftBtn;
    public Button rightBtn;
    
    public Button unlockBtn;
    public Text unlockText;

    public Text roleName;

    public Text moneyText;

    private Transform characterPos;
    private GameObject characterObj;
    private RoleInfo nowRoleData;//当前角色信息
    private int nowIndex;//当前角色数据索引
    
    protected override void Init()
    {
        CameraAnimation cameraAnimation = Camera.main.GetComponent<CameraAnimation>();
        characterPos = GameObject.Find("CharacterPos").transform;

        moneyText.text = GameDataManager.Instance.playerData.money.ToString();//获取金币数据
        
        ChangeCharacter();//获取初始角色数据
        
        returnBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("SelectPanel");
            
            cameraAnimation.MoveBack(() =>
            {
                UIManager.Instance.ShowPanel("BeginPanel");
            });
            
        });
        
        startBtn.onClick.AddListener(() =>
        {
            GameDataManager.Instance.nowSelectCharacterInfo = nowRoleData;
            UIManager.Instance.HidePanel("SelectPanel");
            UIManager.Instance.ShowPanel("SelectScenePanel");
        });
       
        leftBtn.onClick.AddListener(() =>
        {
            --nowIndex;
            if (nowIndex < 0)
            {
                nowIndex = RoleData.Instance.roleInfos.Count-1;
            }
            ChangeCharacter();
        });
        
        rightBtn.onClick.AddListener(() =>
        {
            ++nowIndex;
            if (nowIndex > RoleData.Instance.roleInfos.Count-1)
            {
                nowIndex = 0;
            }
            
            ChangeCharacter();
        });
        
        unlockBtn.onClick.AddListener(() =>
        {
            if (GameDataManager.Instance.playerData.money >= nowRoleData.unlockAmount)
            {
                GameDataManager.Instance.playerData.money -= nowRoleData.unlockAmount;
                moneyText.text = GameDataManager.Instance.playerData.money.ToString();
                GameDataManager.Instance.playerData.unlockCharacter.Add(nowRoleData.id);
                GameDataManager.Instance.SavePlayerData();
                UpdateUnlockCharacter();
            }
            else
            {
                UIManager.Instance.ShowPanel("TipsPanel");
            }
        });
        
    }
    
    public void ChangeCharacter()
    {
        if (characterObj != null)
        {
            Destroy(characterObj);
        }
        nowRoleData = RoleData.Instance.roleInfos[nowIndex];
        characterObj = Instantiate(Resources.Load<GameObject>(nowRoleData.resourcePath), characterPos.position,characterPos.rotation);
        roleName.text = nowRoleData.roleName;
        if (characterObj.GetComponent<PlayerObj>() != null)
            Destroy(characterObj.GetComponent<PlayerObj>());
        UpdateUnlockCharacter();
    }

    public void UpdateUnlockCharacter()//更新解锁人物
    {
        if (nowRoleData.unlockAmount > 0&&!GameDataManager.Instance.playerData.unlockCharacter.Contains(nowRoleData.id))
        {
            unlockBtn.gameObject.SetActive(true);
            unlockText.text ="¥："+nowRoleData.unlockAmount.ToString();
            startBtn.gameObject.SetActive(false);
        }
        else
        {
            unlockBtn.gameObject.SetActive(false);
            startBtn.gameObject.SetActive(true);
        }
    }
    
    
    public override void HideMe(UnityAction func)
    {
        base.HideMe(func);
        if (characterObj != null)
        {
            Destroy(characterObj);
        }
    }
}
