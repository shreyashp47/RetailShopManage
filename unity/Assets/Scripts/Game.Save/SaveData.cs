using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Save
{
    // CHUNK 8.1: Data model per RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:426-431
    // Local JSON + PlayerPrefs primary, offlineCapHours=8, no ads
    [Serializable]
    public class SaveData
    {
        public PlayerData player = new PlayerData();
        public StoreData store = new StoreData();
        public EmpireData empire = new EmpireData();
        public CatalogData catalog = new CatalogData();
        public List<StaffData> staff = new();
        public ProgressData progress = new ProgressData();
        public SettingsData settings = new SettingsData();
    }

    [Serializable] public class PlayerData { public string id; public int coins = 500; public int gems = 0; public float stars = 4.2f; public int level = 1; public int xp = 0; public int prestigeTier = 0; }
    [Serializable] public class StoreData { public int tier = 1; public string type = "corner"; public Vector2Int size = new Vector2Int(6,6); public List<TileData> tiles = new(); public List<DecorData> decor = new(); public int checkouts = 1; public int fridges = 0; }
    [Serializable] public class TileData { public int x; public int y; public string type; public string sku; public float fill; public float marketCost; }
    [Serializable] public class DecorData { public string id; public int x; public int y; public float buff; }
    [Serializable] public class EmpireData { public List<StoreRef> stores = new(); }
    [Serializable] public class StoreRef { public string id; public string city; public int tier; public int dailyProfit; }
    [Serializable] public class CatalogData { public List<SkuSave> skus = new(); }
    [Serializable] public class SkuSave { public string id; public float cost; public float marketCost; public float price; public int stock; public string shelf; public float demand; }
    [Serializable] public class StaffData { public string id; public string role; public int level = 1; public float speed = 1f; public int salary = 20; public int carry = 1; }
    [Serializable] public class ProgressData { public List<QuestSave> quests = new(); public int loginStreak = 0; public long lastOfflineAt; public int offlineCapHours = 8; }
    [Serializable] public class QuestSave { public string id; public int progress; public int rewardCoins; }
    [Serializable] public class SettingsData { public int speed = 1; public float music = 0.8f; public bool haptics = true; }
}
