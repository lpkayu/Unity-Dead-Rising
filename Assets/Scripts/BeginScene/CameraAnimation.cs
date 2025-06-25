using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraAnimation : MonoBehaviour
{
    private Animator _animator;

    private UnityAction _action;
    
    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
    }

    public void MoveToSelectPanel(UnityAction action)
    {
        _animator.SetTrigger("MoveToSelect");
        _action = action;
    }

    public void MoveBack(UnityAction action)
    {
        _animator.SetTrigger("MoveBack");
        _action = action;
    }
    
    public void AnimationOver()
    {
        _action?.Invoke();
        _action = null;
    }
    
}
