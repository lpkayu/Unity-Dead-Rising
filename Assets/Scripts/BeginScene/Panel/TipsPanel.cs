using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsPanel : BasePanel
{
    public Button returnBtn;
    
    protected override void Init()
    {
        returnBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("TipsPanel");
        });
    }
    
}
