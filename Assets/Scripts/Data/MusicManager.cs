using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    
    //todo:修改整个音效系统，避免一直创建和销毁音效
    public void PlaySound(string soundName)//音效播放
    {
        AudioSource _audioSource= this.AddComponent<AudioSource>();
        _audioSource.mute = !GameDataManager.Instance.musicData.isOpenSound;
        _audioSource.volume = GameDataManager.Instance.musicData.soundValue;
        _audioSource.clip = Resources.Load<AudioClip>(soundName);
        _audioSource.Play();
        Destroy(_audioSource,1);
    }
    
    
    
}
