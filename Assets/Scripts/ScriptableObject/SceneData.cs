using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SceneData")]
public class SceneData : SingletonScriptableObject<SceneData>
{
    public List<SceneInfo> sceneDatas;
}
