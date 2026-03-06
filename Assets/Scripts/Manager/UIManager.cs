using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager:Singleton<UIManager>
{
    private UIManager() //在构造时动态加载Canvas
    {
        GameObject canvas = GameObject.Instantiate(ResourceManager.Instance.Load<GameObject>("UI/Canvas"));
        canvas.name = "Canvas";
        canvasTransform = canvas.transform;
        GameObject.DontDestroyOnLoad(canvas);
    }
    
    private Transform canvasTransform;
    
    private Dictionary<string, BasePanel> uiDic = new Dictionary<string, BasePanel>();
    //控制面板显隐
    public BasePanel ShowPanel(string panelName)
    {
        if (uiDic.ContainsKey(panelName))
        {
            return uiDic[panelName];
        }
        //动态加载面板
        GameObject panelObj;
        if (SceneManager.GetActiveScene().name == "BeginScene")
        {
           panelObj = GameObject.Instantiate(ResourceManager.Instance.Load<GameObject>("UI/BeginScene/"+panelName));
        }
        else
        {
            panelObj = GameObject.Instantiate(ResourceManager.Instance.Load<GameObject>("UI/GameScene/" + panelName));
        }
        
        panelObj.name = panelName;
        panelObj.transform.SetParent(canvasTransform,false);
           
        BasePanel panel = panelObj.GetComponent<BasePanel>();
        uiDic.Add(panelName,panel);
                   
        panel.ShowMe();
        return panel; 
    }
    
    public void HidePanel(string panelName)
    {
        if (uiDic.ContainsKey(panelName))
        {
            uiDic[panelName].HideMe(() =>
            {
                //在淡出后销毁面板
                GameObject.Destroy(uiDic[panelName].gameObject);
                uiDic.Remove(panelName);
            });
        }
    }

}
