using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/TowerData")]
public class TowerData : SingletonScriptableObject<TowerData>
{
    public List<TowerInfo> towerInfos;
}
