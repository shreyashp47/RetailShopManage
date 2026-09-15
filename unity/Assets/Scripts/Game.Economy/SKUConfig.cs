using UnityEngine;

namespace Game.Economy
{
    // CHUNK 4.3: ScriptableObject per RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:441
    // 20 SKUs total (10 MVP): bread/chips L1 → dairy L5 → electronics/meat L10 → premium L20+
    [CreateAssetMenu(menuName = "Retail/SKUConfig")]
    public class SKUConfig : ScriptableObject
    {
        public string id;           // e.g. "milk"
        public float cost = 5f;     // wholesale
        public float marketCost = 5.5f; // fluctuates ±15% daily (CHUNK 4.2)
        public float price = 8f;    // retail set by player
        public int stock = 12;
        public string shelfType;    // e.g. "fridge", "shelf_snack"
        public float demand = 1.2f;
        public int unlockLevel = 1;
    }
}
