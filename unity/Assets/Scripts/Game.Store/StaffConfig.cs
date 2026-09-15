using UnityEngine;

namespace Game.Store
{
    // CHUNK 5.1: Staff per RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:442
    // Roles: cashier/stocker/cleaner/manager, level 1-10, salary doubles each 5 lvls
    public enum StaffRole { Cashier, Stocker, Cleaner, Manager }

    [CreateAssetMenu(menuName = "Retail/StaffConfig")]
    public class StaffConfig : ScriptableObject
    {
        public StaffRole role;
        public int level = 1;
        public float speed = 1f; // 1.0 → 1.4 at L3
        public int salary = 20;
        public int carry = 1; // 1 → 3 upgrade
        public int hireCost = 200;
    }
}
