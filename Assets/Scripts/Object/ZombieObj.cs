using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;

public class ZombieObj : MonoBehaviour
{
    private enum State//状态枚举
    {
        Idle, Chase, Attack, Dead
    }
    
    private Animator _animator;
    private NavMeshAgent _agent;

    private ZombieInfo zombieInfo;

    private int atk;
    public int hp;
    public bool isDead;
    private float lastAtkTime;
    
    //状态机相关
    private State state;
    private float attackRange = 4f;
    
    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        _agent = this.GetComponent<NavMeshAgent>();
        state = State.Idle;
    }

    public void InitZombieInfo(ZombieInfo info)
    {
        this.zombieInfo = info;
        _animator.runtimeAnimatorController = ResourceManager.Instance.Load<RuntimeAnimatorController>(zombieInfo.animator);
        this.atk = zombieInfo.atk;
        this.hp = zombieInfo.hp;
        _agent.speed = _agent.acceleration = zombieInfo.moveSpeed;
        _agent.angularSpeed = zombieInfo.roundSpeed;
    }
    
    public void DeadEvent()
    {
        LevelManager.Instance.RemoveZombieList(this);
        Destroy(this.gameObject);

        // 新增：检查波次清理状态和游戏结束状态
        LevelManager.Instance.CheckWaveStatus();
    }

    public void BornOver()
    {
        state = State.Chase;
    }

    public void AtkEvent()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position+this.transform.up+this.transform.forward,0.5f,
            1<<LayerMask.NameToLayer("SafetyArea"));
        MusicPoolManager.Instance.PlaySound("Music/Eat");
        for (int i = 0; i < colliders.Length; i++)
        {
            if (SafetyAreaObj.Instance.gameObject == colliders[i].gameObject)
            {
                SafetyAreaObj.Instance.TakeDamage(atk);
            }
        }
        lastAtkTime = Time.time;
        _agent.isStopped = false;
        if (!isDead) state = State.Chase;
    }
    
    private void Update()
    {
        //状态机实现
        switch (state)
        {
            case State.Idle:
                break;
            case State.Chase:
                _agent.isStopped = false;
                _agent.SetDestination(SafetyAreaObj.Instance.gameObject.transform.position);
                _animator.SetBool("isRun", _agent.velocity != Vector3.zero);
                if (!SafetyAreaObj.Instance.isDead &&
                    Vector3.Distance(this.transform.position, SafetyAreaObj.Instance.transform.position) < attackRange &&
                    Time.time - lastAtkTime >= zombieInfo.atkOffset)
                {
                    state = State.Attack;
                    
                }
                break;
            case State.Attack:
                _agent.isStopped = true;
                _animator.SetTrigger("Attack");
                break;
            case State.Dead:
                _animator.SetBool("isDead",true);
                _agent.isStopped = true;
                break;
        }
    }
    
    public void TakeDamage(int dmg)
    {
        if(isDead)
            return;
        hp -= dmg;
        _animator.SetTrigger("Hurt");
        if (hp <= 0)
        {
            isDead = true;
            state = State.Dead;
            LevelManager.Instance.playerObj.AddMoney(50);
            MusicPoolManager.Instance.PlaySound("Music/ZombieDead");
            
            // 触发任务事件
            if (zombieInfo != null)
            {
                QuestEventHub.TriggerZombieKilled(zombieInfo.id);
            }
        }
        else
        {
            MusicPoolManager.Instance.PlaySound("Music/ZombieHit");
        }
    }
    
}
