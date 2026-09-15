using UnityEngine;

namespace Game.Store
{
    // CHUNK 1.6: Decor buff 0.05 → rating → stars per RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:133
    [CreateAssetMenu(menuName = "Retail/DecorConfig")]
    public class DecorConfig : ScriptableObject
    {
        public string id; // "plant_01"
        public float buff = 0.05f;
        public int cost = 200;
        public int unlockLevel = 1;
    }
}
