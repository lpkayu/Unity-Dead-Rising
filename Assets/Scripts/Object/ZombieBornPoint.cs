using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ZombieBornPoint : MonoBehaviour
{
   public List<int> zombieID;//用于创建怪物的id
   private int currentZombieID;
   
   private void Start()
   {
      LevelManager.Instance.AddZombieBornPoint(this);
   }

   public ZombieObj SpawnZombieOnce()//创造单只怪物
   {
      currentZombieID = zombieID[Random.Range(0, zombieID.Count)];
      ZombieInfo info = GameDataManager.Instance.zombieInfo[currentZombieID - 1];
      GameObject obj = Instantiate(ResourceManager.Instance.Load<GameObject>(info.res), this.transform.position,
         this.transform.rotation);
      ZombieObj zombieObj = obj.AddComponent<ZombieObj>();
      zombieObj.InitZombieInfo(info);
      LevelManager.Instance.AddZombieList(zombieObj);
      return zombieObj;
   }
   
}
