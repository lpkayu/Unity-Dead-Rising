using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button startBtn;
    public Button settingBtn;
    public Button exitBtn;

    private CameraAnimation _cameraAnimation;
    
    protected override void Init()
    {
        _cameraAnimation = Camera.main.GetComponent<CameraAnimation>();
        
        startBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("BeginPanel");
            _cameraAnimation.MoveToSelectPanel(() =>
            {
                UIManager.Instance.ShowPanel("SelectPanel");
            });
        });
        
        settingBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel("SettingPanel");
            UIManager.Instance.HidePanel("BeginPanel");
        });
        
        exitBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        
    }
}
