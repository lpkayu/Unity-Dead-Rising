using System;

public static class QuestEventHub
{
    public static event Action<int> OnZombieKilled;
    public static event Action<int> OnTowerBuilt;
    public static event Action<int> OnWaveCleared;
    
    public static void TriggerZombieKilled(int zombieId) => OnZombieKilled?.Invoke(zombieId);
    public static void TriggerTowerBuilt(int towerId) => OnTowerBuilt?.Invoke(towerId);
    public static void TriggerWaveCleared(int waveIndex) => OnWaveCleared?.Invoke(waveIndex);
}
