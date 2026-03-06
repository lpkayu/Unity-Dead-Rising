using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerObj : MonoBehaviour
{
    public Transform towerHead;
    public Transform firePoint;
    public float rotaSpeed;
    public string soundName;
    
    private TowerInfo _towerInfo;
    private ZombieObj _targetZombieObj;
    
    private float attackTime;

    private Vector3 zombiePos;//用于临时记录怪物位置 用于调整炮台位置

    private void Awake()
    {
        InitTowerInfo(TowerData.Instance.towerInfos[0]);
        
        attackTime = -_towerInfo.offsetTime;
    }

    public void InitTowerInfo(TowerInfo info)
    {
        this._towerInfo = info;
    }
    
    void Update()
    {
        if (_targetZombieObj == null || _targetZombieObj.isDead ||
            Vector3.Distance(this.transform.position, _targetZombieObj.transform.position) > _towerInfo.atkRange)
        {
            _targetZombieObj = FindTargetZombie(this.transform.position, _towerInfo.atkRange);
        }

        if(_targetZombieObj==null)
            return;
        
        zombiePos = _targetZombieObj.transform.position;
        zombiePos.y = towerHead.position.y; //炮台头部偏移
        towerHead.rotation=Quaternion.Slerp(this.towerHead.rotation,
            Quaternion.LookRotation(zombiePos-towerHead.position),Time.deltaTime*rotaSpeed);

        //攻击逻辑
        if (Vector3.Angle(towerHead.forward, zombiePos - towerHead.position) < 3 &&
            Time.time - attackTime >= _towerInfo.offsetTime)
        {
            _targetZombieObj.TakeDamage(_towerInfo.atk);
            MusicPoolManager.Instance.PlaySound(soundName);
            GameObject eff = Instantiate(ResourceManager.Instance.Load<GameObject>(_towerInfo.eff),firePoint.position,firePoint.rotation);
            Destroy(eff,0.3f);
            attackTime = Time.time;
        }
    }

    //使用范围检测最近的敌人
    private ZombieObj FindTargetZombie(Vector3 towerPos, float range)
    {
        int mask = 1 << LayerMask.NameToLayer("Zombie");
        Collider[] hits = Physics.OverlapSphere(towerPos, range, mask);
        float minDist = float.MaxValue;//用于对比替换最近敌人距离用
        ZombieObj targetZombie = null;
        for (int i = 0; i < hits.Length; i++)
        {
            ZombieObj zombieObj = hits[i].GetComponent<ZombieObj>();
            if (zombieObj == null || zombieObj.isDead) continue;
            float minDistance = Vector3.Distance(towerPos, zombieObj.transform.position);
            if (minDistance < minDist)
            {
                minDist = minDistance;
                targetZombie = zombieObj;
            }
        }
        return targetZombie;
    }
    
}
