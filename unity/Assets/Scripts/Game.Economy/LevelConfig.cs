using UnityEngine;

namespace Game.Economy
{
    // CHUNK 4.5-4.6: Level unlocks per RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:284
    // L2 snack $200, L5 2nd checkout+cashier $800, L10 fridge $2500, L15 thief $6000, L20 prestige 2×
    [CreateAssetMenu(menuName = "Retail/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        public int level;
        public string unlockId;
        public int cost;
        public int customersPerMin;
        public int skuCount;
    }
}
