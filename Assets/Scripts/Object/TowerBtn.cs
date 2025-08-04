using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
   public Image towerImg;
   public Text moneyText;
   public Text buttonText;

   //初始化炮台显示信息
   public void InitBtnInfo(int id,string text)
   {
      TowerInfo info = GameDataManager.Instance.towerInfo[id - 1];
      towerImg.sprite = Resources.Load<Sprite>(info.imgRes);
      moneyText.text = "¥"+info.money;
      buttonText.text = text;
   }
   
}
