using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager //读取和保存本地数据
{
    private static GameDataManager instance;

    public static GameDataManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameDataManager();
            return instance;
        }
    }

    public MusicData musicData;
    public PlayerData playerData;
    
    public List<RoleInfo> roleInfo;
    public List<SceneInfo> sceneInfo;
    public List<ZombieInfo> zombieInfo;
    public List<TowerInfo> towerInfo;
    
    public RoleInfo nowSelectCharacterInfo;//用于在过场景时保存选择角色的信息
    
    private GameDataManager()
    {
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        
        roleInfo = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        sceneInfo = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
        zombieInfo = JsonMgr.Instance.LoadData<List<ZombieInfo>>("ZombieInfo");
        towerInfo = JsonMgr.Instance.LoadData<List<TowerInfo>>("TowerInfo");
    }

    //音乐数据相关
    public void SaveMusicData()
    {
       JsonMgr.Instance.SaveData(musicData,"MusicData");
    }

    //玩家数据相关
    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData,"PlayerData");
    }
    
}
