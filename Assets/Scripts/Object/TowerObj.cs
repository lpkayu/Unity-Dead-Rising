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
            _targetZombieObj = GameLevelManager.Instance.FindTargetZombie(this.transform.position, _towerInfo.atkRange);
        }

        if(_targetZombieObj==null)
            return;
        
        zombiePos = _targetZombieObj.transform.position;
        zombiePos.y = towerHead.position.y; //炮台头部偏移
        towerHead.rotation=Quaternion.Slerp(this.towerHead.rotation,
            Quaternion.LookRotation(zombiePos-towerHead.position),Time.deltaTime*rotaSpeed);

        if (Vector3.Angle(towerHead.forward, zombiePos - towerHead.position) < 3 &&
            Time.time - attackTime >= _towerInfo.offsetTime)
        {
            _targetZombieObj.TakeDamage(_towerInfo.atk);
            MusicPoolManager.Instance.PlaySound(soundName);
            GameObject eff = Instantiate(Resources.Load<GameObject>(_towerInfo.eff),firePoint.position,firePoint.rotation);
            Destroy(eff,0.3f);
            attackTime = Time.time;
        }
    }
}
