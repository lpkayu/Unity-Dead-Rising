using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Object = UnityEngine.Object;


//懒汉模式：先判空在实例化
//饿汉模式：直接实例化
//没继承mono的单例基类
public class Singleton<T> where T:class,new()  
{
    private static T instance;

    protected static readonly Object lockObj = new Object();  
    
    public static T Instance
    {
        get
        {
            lock (lockObj)  //使用lock语句用于解决多线程并发问题
            {
               if (instance == null)
                   instance = new T();
            }
            return instance; 
        }
    }
}
