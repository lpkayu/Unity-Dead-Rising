using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ZombieBornPoint : MonoBehaviour
{
   public int zombieMaxWave; //单个出怪点的总波数
   public int eachWaveZombieNum;//每波怪物数量
   public List<int> zombieID;//用于创建怪物的id
   public float createEachZombieTime; //每只怪物创建的时间间隔
   public float createEachWaveTime;//每波之间的间隔
   public float firstWaveCreateTime;//首波的创建时间间隔

   private int currentZombieID;//用于记录当前波数随机到的怪物id
   [SerializeField]private int currentZombieNum;//用于记录当前波数剩余的怪物数量
   
   private void Start()
   {
      Invoke("CreateZombieWave",firstWaveCreateTime);
      GameLevelManager.Instance.AddZombieBornPoint(this);
      GameLevelManager.Instance.AddAllWaveNum(zombieMaxWave);
   }

   public void CreateZombie()
   {
      currentZombieID = zombieID[Random.Range(0, zombieID.Count)];
      ZombieInfo info = GameDataManager.Instance.zombieInfo[currentZombieID - 1];
      GameObject obj = Instantiate(Resources.Load<GameObject>(info.res), this.transform.position,
         this.transform.rotation);
      ZombieObj zombieObj = obj.AddComponent<ZombieObj>();
      zombieObj.InitZombieInfo(info);
      
      --currentZombieNum;
      GameLevelManager.Instance.AddZombieList(zombieObj);
      
      if (currentZombieNum == 0)
      {
         if (zombieMaxWave > 0)
         {
            Invoke("CreateZombieWave",createEachWaveTime);
         }
      }
      else
      {
         Invoke("CreateZombie",createEachZombieTime);  
      }
   }

   public void CreateZombieWave()
   {
      GameLevelManager.Instance.UpdateCurrentWaveNum(-1);
      currentZombieNum = eachWaveZombieNum;
      zombieMaxWave--;
      CreateZombie();
   }

   public bool CheckCreateZombieOver()
   {
      return currentZombieNum == 0 && zombieMaxWave == 0;
   }
   
}
