using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObject/RoleData")]
public class RoleData : SingletonScriptableObject<RoleData>
{
    public List<RoleInfo> roleInfos;
}
