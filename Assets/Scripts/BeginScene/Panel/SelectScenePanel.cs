using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectScenePanel : BasePanel
{
    public Button startBtn;
    public Button returnBtn;
    public Button leftBtn;
    public Button rightBtn;
    public Image sceneImage;
    public Text sceneName;
    public Text sceneDescribe;

    private int nowSceneIndex;
    private SceneInfo nowSceneData;
    
    protected override void Init()
    {
        ChangeSceneInfo();
        
        startBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("SelectScenePanel");
            AsyncOperation ao=SceneManager.LoadSceneAsync(nowSceneData.sceneName);
            ao.completed += (obj) =>
            {
                LevelManager.Instance.InitGameLevel(nowSceneData);
            };
        });
        
        returnBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("SelectScenePanel");
            UIManager.Instance.ShowPanel("SelectPanel");
        });
        
        leftBtn.onClick.AddListener(() =>
        { 
            --nowSceneIndex;
            if (nowSceneIndex < 0)
            {
                nowSceneIndex = SceneData.Instance.sceneDatas.Count-1;
            }
            ChangeSceneInfo();
        });
        
        rightBtn.onClick.AddListener(() =>
        {
            ++nowSceneIndex;
            if (nowSceneIndex > SceneData.Instance.sceneDatas.Count-1)
            {
                nowSceneIndex = 0;
            }
            ChangeSceneInfo();
        });
        
    }

    public void ChangeSceneInfo()
    {
        if (sceneImage.sprite != null)
        {
            sceneImage.sprite = null;
        }
        nowSceneData = SceneData.Instance.sceneDatas[nowSceneIndex];
        sceneName.text = "地图：" + nowSceneData.name;
        sceneDescribe.text = "简介：" + nowSceneData.describe;
        sceneImage.sprite = ResourceManager.Instance.Load<Sprite>(nowSceneData.imgPath);
    }
    
}
