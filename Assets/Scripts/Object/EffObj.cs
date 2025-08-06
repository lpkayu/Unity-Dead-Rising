using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffObject : MonoBehaviour
{
   public EffData effData;

   private void Update()
   {
      this.transform.Translate(this.transform.forward*effData.speed*Time.deltaTime,Space.World);
      
   }

   private void OnCollisionEnter(Collision other)
   {
      int zombieLayer = LayerMask.NameToLayer("Zombie");
      if (other.gameObject.layer == zombieLayer)
      {
         Debug.Log("检测到僵尸");
         Destroy(this.gameObject);
      }
   }
   
}
