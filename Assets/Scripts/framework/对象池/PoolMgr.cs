using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PoolData
{
    private Stack<GameObject> dataStack = new Stack<GameObject>();//用于记录不在使用中的对象
    
    private List<GameObject> usedList = new List<GameObject>(); //用于记录正在使用中的对象

    public int count => dataStack.Count;
    
    public int usedLsitCount => usedList.Count;

    
    public PoolData(GameObject usedObj) //在创建对象时压入list 
    {
        PushUsedList(usedObj);
    }

    public GameObject Pop()
    {
        GameObject obj;

        if (count > 0)
        {
            obj = dataStack.Pop();
            usedList.Add(obj);
        }
        else //若没有不在使用中的对象，就使用使用时长
        {
            obj = usedList[0];
            usedList.RemoveAt(0);
            usedList.Add(obj);
        }
        obj.SetActive(true);
        return obj;
    }

    public void Push(GameObject obj)
    {
        obj.SetActive(false);
        dataStack.Push(obj);
        usedList.Remove(obj);
    }
    
    
    public void PushUsedList(GameObject obj)
    {
        usedList.Add(obj);
    }

}


//对象池模块
public class PoolMgr : Singleton<PoolMgr>
{
    private Dictionary<string, PoolData> poolDic = new Dictionary<string, PoolData>();

    public GameObject GetObj(string name,int number=10)
    {
        GameObject obj;

        #region 没加入数量限制前的逻辑
        /*if (poolDic.ContainsKey(name) && poolDic[name].count>0)
        {
            obj=poolDic[name].Pop();
            obj.SetActive(true);
        }
        else
        {
            obj =GameObject.Instantiate(Resources.Load<GameObject>(name));
            obj.name = name;
        }*/
        
        #endregion

        #region 加入数量上限后逻辑

        if ((poolDic.ContainsKey(name) == false)||(poolDic[name].count==0&&poolDic[name].usedLsitCount < number))
        {
            obj =GameObject.Instantiate(Resources.Load<GameObject>(name));
            obj.name = name;

            if (poolDic.ContainsKey(name) == false)
            {
                poolDic.Add(name,new PoolData(obj));
            }
            else
            {
                poolDic[name].PushUsedList(obj);
            }
        }
        else
        {
            obj = poolDic[name].Pop();
        }
       
        #endregion
        
        return obj;
    }

    public void PushObj(GameObject gameObject)
    {
        gameObject.SetActive(false);
        poolDic[gameObject.name].Push(gameObject);    
        
    }

    public void ClearPool()
    {
        poolDic.Clear();
    }
    
}
