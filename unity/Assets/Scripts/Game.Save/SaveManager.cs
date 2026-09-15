using UnityEngine;

namespace Game.Save
{
    // CHUNK 8.1-8.4: Local JSON + PlayerPrefs primary, offlineCapHours=8, no ads
    // Cloud Play Games / PlayFab mirror optional, never blocks
    public class SaveManager : MonoBehaviour
    {
        public const string Key = "RetailSave";
        public SaveData Data { get; private set; } = new SaveData();

        public void Save()
        {
            Data.progress.lastOfflineAt = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string json = JsonUtility.ToJson(Data);
            PlayerPrefs.SetString(Key, json);
            PlayerPrefs.Save();
            Debug.Log($"[SaveManager] Saved day {Data.progress.loginStreak} coins {Data.player.coins}");
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(Key))
            {
                string json = PlayerPrefs.GetString(Key);
                Data = JsonUtility.FromJson<SaveData>(json);
                // Offline earnings calc: min(now-lastOfflineAt, 8h) * tierRate
                long now = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                long delta = System.Math.Min(now - Data.progress.lastOfflineAt, Data.progress.offlineCapHours * 3600);
                Debug.Log($"[SaveManager] Offline {delta/3600f:F1}h cap {Data.progress.offlineCapHours}h");
            }
            else
            {
                Data = new SaveData(); // 500 coins Tier1 6x6 per PROJECT_PLAN.md:31 FTUE
            }
        }
    }
}
