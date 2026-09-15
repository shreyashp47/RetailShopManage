using UnityEngine;

namespace Game.Store
{
    // CHUNK 1.1: Grid 6x6→12x12, snap, tile purchase FR-1.1
    // Data model: store.size {w,h}, tiles[] per RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:431
    public class StoreGrid : MonoBehaviour
    {
        public Vector2Int size = new Vector2Int(6, 6);
        public float cellSize = 1f;

        public Vector3 GridToWorld(Vector2Int gridPos)
        {
            return new Vector3(gridPos.x * cellSize, 0, gridPos.y * cellSize);
        }

        public bool IsInside(Vector2Int p) => p.x >= 0 && p.x < size.x && p.y >= 0 && p.y < size.y;

        // TODO: 1.4 expansion gate Tier1 6x6 free → 8x8 $800 → 12x12 $2500
        public bool TryExpand(Vector2Int newSize, int coins)
        {
            // cost = base*1.6^level (CHUNK 4.4)
            return false;
        }
    }
}
