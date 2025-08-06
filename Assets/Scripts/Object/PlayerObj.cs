using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerObj : MonoBehaviour
{
    private Animator _animator;
    private PlayerInputController _playerInputController;

    private float hSpeed;
    private float vSpeed;
    public float smoothSpeed;// 移动动画平滑变化速度

    private float targetWeight=0;// 目标权重
    private float currentWeight=0;// 当前权重
    public float crouchSmoothSpeed;// 下蹲动画平滑变化速度

    private Vector2 mouseDelta;
    public float roteSpeed;

    public bool isFire;
    
    [SerializeField]private int atk;
    public int money;
    
    private GameUI gameUI;

    private LineRenderer line;
    private GameObject lineObj;
    private RoleInfo nowRoleInfo;
    
    [Header("射线检测相关")]
    public Transform firePoint;
    
    [Header("范围检测相关")] 
    public Vector3 detectionDistance;// 检测距离
    public float radius;
    
    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        _playerInputController = new PlayerInputController();
    }

    void Start()
    {
        _playerInputController.Player.Roll.started += Roll;
        _playerInputController.Player.Fire.started += context =>isFire=true ;
        _playerInputController.Player.Crouch.started += StartCrouch;
        _playerInputController.Player.Fire.canceled += context =>isFire=false ;
        _playerInputController.Player.Crouch.canceled += EndCrouch;
        
        _animator.SetLayerWeight(1,currentWeight);

        nowRoleInfo = GameDataManager.Instance.nowSelectCharacterInfo;

        if (nowRoleInfo.type == 2)
        {
            //射线可视化相关
            lineObj = new GameObject();
            line = lineObj.AddComponent<LineRenderer>();
            line.startWidth = 0.01f;
            line.material = new Material(Shader.Find("Unlit/Color")) { color = Color.red }; 
        }
        
    }
    
    private void OnEnable()
    {
        _playerInputController.Enable();
    }

    private void OnDisable()
    {
        _playerInputController.Disable();
       
    }

    public void InitPlayerInfo(int atk,int money)
    {
        this.atk = atk;
        this.money = money;
 
        UpdateMoney();
    }
    
    void Update()
    {
        if(GameLevelManager.Instance.CheckGameOver() || SafetyAreaObj.Instance.isDead)
            return;
        
        hSpeed =Mathf.Lerp(hSpeed,_playerInputController.Player.Move.ReadValue<Vector2>().x,smoothSpeed*Time.deltaTime) ;
        vSpeed =Mathf.Lerp(vSpeed,_playerInputController.Player.Move.ReadValue<Vector2>().y,smoothSpeed*Time.deltaTime);

        mouseDelta = Mouse.current.delta.ReadValue();
        this.transform.Rotate(Vector3.up,mouseDelta.x*roteSpeed*Time.deltaTime);
        
        currentWeight = Mathf.Lerp(currentWeight, targetWeight, crouchSmoothSpeed * Time.deltaTime);
        _animator.SetLayerWeight(1,currentWeight);
        
        SetAnimation();

        if (nowRoleInfo.type == 2)
        {
            //射线设置位置
            line.SetPosition(0, firePoint.position);
            line.SetPosition(1, firePoint.position + firePoint.forward * 20);
        }

    }

    private void SetAnimation()
    {
       _animator.SetFloat("HSpeed",hSpeed);
       _animator.SetFloat("VSpeed",vSpeed);
       _animator.SetBool("Fire",isFire);
    }
    
    private void Roll(InputAction.CallbackContext obj)
    {
        _animator.SetTrigger("Roll");
    }
    
    private void StartCrouch(InputAction.CallbackContext obj)
    {
        targetWeight = 1;
    }
    
    private void EndCrouch(InputAction.CallbackContext obj)
    {
        targetWeight = 0;
    }

    public void ShootEvent()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(firePoint.position,this.transform.forward),1000);
        MusicPoolManager.Instance.PlaySound("Music/Gun");
        for (int i = 0; i < hits.Length; i++)
        {
            ZombieObj zombie = hits[i].collider.gameObject.GetComponent<ZombieObj>();
            GameObject eff = Instantiate(Resources.Load<GameObject>("effect/attack"),hits[i].point,hits[i].transform.rotation);
            Destroy(eff,0.3f);
            if (zombie != null)
            {
                zombie.TakeDamage(atk);
                break;
            }   
        }
    }

    public void KnifeEvent()
    {
        Collider[] colliders= Physics.OverlapSphere(this.transform.position + this.transform.forward * detectionDistance.z+this.transform.up*detectionDistance.y, radius,
            1 << LayerMask.NameToLayer("Zombie"));
        MusicPoolManager.Instance.PlaySound("Music/Knife");
        for (int i = 0; i < colliders.Length; i++)
        {
            ZombieObj zombie = colliders[i].gameObject.GetComponent<ZombieObj>();
            if (zombie != null)
            {
                zombie.TakeDamage(atk);
                break;
            }   
        }
        
    }

    public void UpdateMoney()
    {
        if(gameUI ==null)
            gameUI = GameObject.Find("GameUI").GetComponent<GameUI>();
        gameUI.UpdateMoney(money);
    }

    public void AddMoney(int money)
    {
        this.money += money;
        UpdateMoney();
    }
    
    // 禁用玩家输入
    public void DisablePlayerInput()
    {
        _playerInputController.Disable();
        isFire = false;
        hSpeed = 0;
        vSpeed = 0;
        SetAnimation();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 detectionCenter = transform.position + transform.forward * detectionDistance.z+this.transform.up*detectionDistance.y;
        Gizmos.DrawWireSphere(detectionCenter, radius);
    }
    
    
}
