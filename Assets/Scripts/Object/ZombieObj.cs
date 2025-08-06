using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Android;

public class ZombieObj : MonoBehaviour
{
    //出生后进行初始化
    //移动
    //攻击
    //受伤
    //死亡

    private Animator _animator;
    private NavMeshAgent _agent;

    private ZombieInfo zombieInfo;

    [SerializeField]private int atk;
    public int hp;
    public bool isDead;

    private float lastAtkTime;
    
    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        _agent = this.GetComponent<NavMeshAgent>();
    }

    public void InitZombieInfo(ZombieInfo info)
    {
        this.zombieInfo = info;
        _animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(zombieInfo.animator);
        this.atk = zombieInfo.atk;
        this.hp = zombieInfo.hp;
        _agent.speed = _agent.acceleration = zombieInfo.moveSpeed;
        _agent.angularSpeed = zombieInfo.roundSpeed;
    }

    public void TakeDamage(int dmg)
    {
        if(isDead)
            return;
        hp -= dmg;
        _animator.SetTrigger("Hurt");
        if (hp <= 0)
        {
            Dead();
        }
        else
        {
            MusicPoolManager.Instance.PlaySound("Music/ZombieHit");
        }
    }

    public void Dead()
    {
        isDead = true;
        _animator.SetBool("isDead",true);
        _agent.isStopped = true;
        MusicPoolManager.Instance.PlaySound("Music/ZombieDead");
        GameLevelManager.Instance.playerObj.AddMoney(50);
    }

    public void DeadEvent()
    {
        GameLevelManager.Instance.RemoveZombieList(this);
        GameLevelManager.Instance.killZombieNum += 1;
        Destroy(this.gameObject);

        if (GameLevelManager.Instance.CheckGameOver())
        {
            if (GameLevelManager.Instance.playerObj != null)
            {
                GameLevelManager.Instance.playerObj.DisablePlayerInput();
            }
            
            UIManager.Instance.ShowPanel("GameOverPanel");
            GameOverPanel panel = GameObject.Find("GameOverPanel").GetComponent<GameOverPanel>();
            panel.InitInfo(false);
        }
    }

    public void BornOver() //出生后将目标设为安全区
    {
        _agent.SetDestination(SafetyAreaObj.Instance.gameObject.transform.position);
        _animator.SetBool("isRun",true);
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
        
    }
    
    private void Update()
    {
        if (isDead)
            return;
        _animator.SetBool("isRun", _agent.velocity != Vector3.zero);
        if( Vector3.Distance(this.transform.position, SafetyAreaObj.Instance.transform.position ) < 4 &&
            Time.time - lastAtkTime >= zombieInfo.atkOffset && !SafetyAreaObj.Instance.isDead)
        {
            lastAtkTime= Time.time;
            _animator.SetTrigger("Attack");
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Vector3 detectionCenter = this.transform.position+this.transform.up+this.transform.forward;
        
        Gizmos.DrawWireSphere(detectionCenter, 0.5f);
    }
}
