using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RoleInfo //角色数据
{
   public int id;
   public string resourcePath;
   public int atk;
   public string roleName;
   public int unlockAmount;
   public int type; //用于区分攻击方式
}
