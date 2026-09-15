using UnityEngine;

namespace Game.Store
{
    // CHUNK 1.1, 4.3: Tile + SKU configs per RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:431, 441
    [CreateAssetMenu(menuName = "Retail/TileConfig")]
    public class TileConfig : ScriptableObject
    {
        public string type; // "shelf_snack", "fridge", "checkout"
        public int cost;
        public int unlockLevel;
        public Vector2Int size = Vector2Int.one;
    }
}
