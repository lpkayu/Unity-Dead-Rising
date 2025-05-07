using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour //用于设置音乐音效
{
    private static MusicManager instance;
    public static MusicManager Instance=>instance;

    private AudioSource audioSource;
    
    private void Awake()
    {
        instance = this;

        audioSource = this.GetComponent<AudioSource>();
        
        SetMusicOn(GameDataManager.Instance.musicData.isOpenMusic);
        SetMusicValue(GameDataManager.Instance.musicData.musicValue);
    }

    public void SetMusicOn(bool isOpen)
    {
        audioSource.mute = !isOpen;
    }

    public void SetMusicValue(float value)
    {
        audioSource.volume = value;
    }
    
}
