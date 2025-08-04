using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPoolManager : MonoBehaviour
{
    private static MusicPoolManager instance;

    public static MusicPoolManager Instance
    {
        get
        {
            return instance;
        }   
    }

    private AudioSource audioSource;
    
    private List<AudioSource> audioSourcePool;
    private Dictionary<AudioSource,Coroutine> returnToPoolCoroutines; //协程用于控制对象返回池子

    private int initPoolSize=5;//初始化对象池大小
    private int MaxPoolSize=20;//最大对象池大小

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        
        instance = this;
        audioSource = this.GetComponent<AudioSource>();
        
        SetMusicOn(GameDataManager.Instance.musicData.isOpenMusic);
        SetMusicValue(GameDataManager.Instance.musicData.musicValue);
        
        InitAudioSourcePool();
        
        SetSoundOn(GameDataManager.Instance.musicData.isOpenSound);
        SetSoundValue(GameDataManager.Instance.musicData.soundValue);
        
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void SetMusicOn(bool isOn)
    {
        audioSource.mute = !isOn;
    }

    public void SetMusicValue(float value)
    {
        audioSource.volume = value;
    }

    public void SetSoundValue(float value)
    {
        foreach (var audioSource in audioSourcePool)
        {
            audioSource.volume = value;
        }
    }

    public void SetSoundOn(bool isOn)
    {
        foreach (var audioSource in audioSourcePool)
        {
            audioSource.mute=!isOn;
        }
    }
    
    public void InitAudioSourcePool()//初始化音效池
    {
        audioSourcePool = new List<AudioSource>();
        returnToPoolCoroutines = new Dictionary<AudioSource, Coroutine>();
        
        for (int i = 0; i < initPoolSize; i++)
        {
            CreatAudioSource();
        }
    }

    public AudioSource CreatAudioSource()//2.创建audioSource的方法
    {
        GameObject audioSourceObj = new GameObject("audioSource");
        audioSourceObj.transform.SetParent(this.transform);

        AudioSource audioSource = audioSourceObj.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;//初始化audioSource的相关设置
        audioSource.loop = false;
        audioSource.volume = GameDataManager.Instance.musicData.soundValue;
        audioSource.mute = !GameDataManager.Instance.musicData.isOpenSound;
        
        audioSourceObj.SetActive(false);
        audioSourcePool.Add(audioSource);

        return audioSource;
    }
    
    //3.从池中获取对象的方法
    public AudioSource GetAudioSourceFromPool()
    {
        foreach (var audioSource in audioSourcePool) //初始化数量足够的情况下
        {
            if (!audioSource.gameObject.activeInHierarchy)
            {
                audioSource.gameObject.SetActive(true);
                return audioSource;
            }
        }

        if (audioSourcePool.Count < MaxPoolSize) //为超出最大池数量
        {
            AudioSource newAudioSource = CreatAudioSource();
            newAudioSource.gameObject.SetActive(true);
            return newAudioSource;
        }
        
        //满了的话使用 使用时长最久的
        AudioSource oldestAudioSource = audioSourcePool[0];
        if (returnToPoolCoroutines.TryGetValue(oldestAudioSource, out Coroutine coroutine))
        {
            StopCoroutine(coroutine);
            returnToPoolCoroutines.Remove(oldestAudioSource);
        }
        return oldestAudioSource;
        
    }
    
    //4.对象返回池子的方法
    public IEnumerator ReturnToPoolAfterPlaying(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length + 0.1f);
        audioSource.clip = null;
        audioSource.gameObject.SetActive(false);

        returnToPoolCoroutines.Remove(audioSource);
    }
    
    //5.播放音效的方法
    public void PlaySound(string soundName)
    {
        AudioClip clip = Resources.Load<AudioClip>(soundName);
        
        AudioSource audioSource = GetAudioSourceFromPool();
        audioSource.volume = GameDataManager.Instance.musicData.soundValue;
        audioSource.mute = !GameDataManager.Instance.musicData.isOpenSound;
        audioSource.clip = clip;
        audioSource.Play();
        
        
        if (returnToPoolCoroutines.TryGetValue(audioSource, out Coroutine existingCoroutine))
        {
            StopCoroutine(existingCoroutine);
        }
        // 启动协程，在音效播放完成后将AudioSource返回池中
        Coroutine newCoroutine = StartCoroutine(ReturnToPoolAfterPlaying(audioSource));
        returnToPoolCoroutines[audioSource] = newCoroutine;
    }
    
}
