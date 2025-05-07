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
    
    public RoleInfo nowSelectCharacter;//用于在过场景时保存选择角色的信息
    
    private GameDataManager()
    {
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
        playerData = JsonMgr.Instance.LoadData<PlayerData>("PlayerData");
        
        roleInfo = JsonMgr.Instance.LoadData<List<RoleInfo>>("RoleInfo");
        sceneInfo = JsonMgr.Instance.LoadData<List<SceneInfo>>("SceneInfo");
    }

    //音乐数据相关
    public void SaveMusicData()
    {
       JsonMgr.Instance.SaveData(musicData,"MusicData");
    }

    public void SavePlayerData()
    {
        JsonMgr.Instance.SaveData(playerData,"PlayerData");
    }
    
}
