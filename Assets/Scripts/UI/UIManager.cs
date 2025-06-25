using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager
{
    private static UIManager instance;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
                instance = new UIManager();
            return instance;
        }
    }
    
    private UIManager() //在构造时动态加载Canvas
    {
        GameObject canvas = GameObject.Instantiate(Resources.Load<GameObject>("UI/Canvas"));
        canvas.name = "Canvas";
        canvasTransform = canvas.transform;
        GameObject.DontDestroyOnLoad(canvas);
    }
    
    
    private Dictionary<string, BasePanel> uiDic = new Dictionary<string, BasePanel>();

    private Transform canvasTransform;
    
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
           panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/BeginScene/" + panelName));
        }
        else
        {
            panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/GameScene/" + panelName));
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

    public BasePanel GetPanel(string panelName)
    {
        if (uiDic.ContainsKey(panelName))
        {
            return uiDic[panelName];
        }
        return null;
    }
    
}
