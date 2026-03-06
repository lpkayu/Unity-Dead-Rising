using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class WaveManager : MonoBehaviour //波次管理器
{
    public int totalWaves;//生成的总波次
    public int eachWaveZombieNum;//每波生成的怪物总数
    public float createEachZombieTime;//生成每只怪物的时间间隔
    public float createEachWaveTime;//每个波次的时间间隔
    public float firstWaveCreateTime;//生成第一波的等待时间
 
    private List<ZombieBornPoint> _zombieBornPoints = new List<ZombieBornPoint>();
    private bool isRunning;//用于判断波次是否开始
    private int remainingWaves;//表示剩余的波数
    public int CurrentWave => totalWaves - remainingWaves; // 获取当前进行到的波次
    public bool IsWaveCleared => !isRunning; // 当前波次生成完毕（等待清理）

    private void Awake()
    {
        _zombieBornPoints.AddRange(FindObjectsOfType<ZombieBornPoint>());
        remainingWaves = totalWaves;
    }
 
    private void Start()
    {
        LevelManager.Instance.AddAllWaveNum(totalWaves);
        StartCoroutine(RunWaves());
    }
 
    private IEnumerator RunWaves()//开始波次
    {
        isRunning = true;
        yield return new WaitForSeconds(firstWaveCreateTime);
        for (int wave = 0; wave < totalWaves; wave++)
        {
            // 每一波开始前，如果不是第一波，则说明前一波（wave）已经全部清理完毕并度过了等待时间
            // 注意：这里 wave 是即将生成的波次索引（0-4），wave=1时表示第2波即将开始，说明第1波（wave-1）已完成
            if (wave > 0)
            {
                // 触发上一波完成的事件
                QuestEventHub.TriggerWaveCleared(wave);
                Debug.Log($"波次 {wave} 完成（新波次开始判定）");
            }
            
            for (int i = 0; i < eachWaveZombieNum; i++)
            {
                if (_zombieBornPoints.Count > 0)
                {
                    var point = _zombieBornPoints[Random.Range(0, _zombieBornPoints.Count)];
                    point.SpawnZombieOnce();
                }
                yield return new WaitForSeconds(createEachZombieTime);
            }
            LevelManager.Instance.UpdateCurrentWaveNum(1);
            
            remainingWaves = totalWaves - (wave + 1);
            if (wave < totalWaves - 1)
            {
                yield return new WaitForSeconds(createEachWaveTime);
            }
        }
        
        // 最后一波生成完毕后，等待所有僵尸清理完毕
        // 这里的逻辑需要依赖 LevelManager 的 CheckWaveStatus 来触发最终胜利
        // 或者我们可以开启一个协程来监测
        
        isRunning = false;
        
        // 最后一波不会触发 wave > 0 的逻辑，所以需要额外处理
        // 但由于 RunWaves 只负责生成，清理逻辑在 LevelManager，所以这里只负责将 isRunning 置为 false
        // LevelManager 检测到 isRunning=false 且 zombieList=0 时会触发胜利和最后一波完成
    }
}
