using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Toggle musicToggle;
    public Toggle soundToggle;
    public Slider musicSlider;
    public Slider soundSlider;

    public Button returnBtn;
    public Button applyBtn;
    
    protected override void Init()
    {
        //进入面板时初始化数据
        MusicData data = GameDataManager.Instance.musicData;
        
        musicToggle.isOn = data.isOpenMusic;
        soundToggle.isOn = data.isOpenSound;
        musicSlider.value = data.musicValue;
        soundSlider.value = data.soundValue;
        
        musicToggle.onValueChanged.AddListener((isOpen) =>
        {
            MusicManager.Instance.SetMusicOn(isOpen);

            GameDataManager.Instance.musicData.isOpenMusic = isOpen;
        });
        
        soundToggle.onValueChanged.AddListener((isOpen) =>
        {
            GameDataManager.Instance.musicData.isOpenSound = isOpen;
        });
        
        musicSlider.onValueChanged.AddListener((value) =>
        {
            MusicManager.Instance.SetMusicValue(value);

            GameDataManager.Instance.musicData.musicValue = value;
        });
        
        soundSlider.onValueChanged.AddListener((value) =>
        {
            GameDataManager.Instance.musicData.soundValue = value;
        });
        
        returnBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel("SettingPanel");
            UIManager.Instance.ShowPanel("BeginPanel");
        });
        
        applyBtn.onClick.AddListener(() =>
        {
            GameDataManager.Instance.SaveMusicData();
        });
        
    }
}
