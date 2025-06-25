using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class BasePanel : MonoBehaviour
{
    private CanvasGroup _canvasGroup; //用于控制CanvasGroup下所有组件的透明度
    public float fadeSpeed;
    private bool fadeIn;
    private bool fadeOut;

    private UnityAction callBack;
    
    protected virtual void Awake()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
            _canvasGroup = this.AddComponent<CanvasGroup>();
    }

    protected virtual void Start()
    {
        Init();
    }

    protected abstract void Init();

    protected virtual void Update()
    {
        if (fadeIn && _canvasGroup.alpha != 1)
        {
            _canvasGroup.alpha += fadeSpeed * Time.deltaTime;
            if (_canvasGroup.alpha >= 1)
            {
                _canvasGroup.alpha = 1;
                fadeIn = false;
            }
                
        }
        if (fadeOut && _canvasGroup.alpha != 0)
        {
            _canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            if (_canvasGroup.alpha <= 0)
            {
                _canvasGroup.alpha = 0;
                fadeOut = false;
                callBack?.Invoke();
            }
                
        }
    }
    
    //面板淡入淡出
    public virtual void ShowMe()
    {
        _canvasGroup.alpha = 0;
        fadeIn = true;
    }

    public virtual void HideMe(UnityAction func)
    {
        _canvasGroup.alpha = 1;
        fadeOut = true;
        callBack = func;
    }
    
}
