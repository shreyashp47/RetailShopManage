using UnityEngine;

namespace Game.Economy
{
    // CHUNK 4.2: sales = (retail - marketCost) * units, market ±15% daily via Remote Config
    // Costs base*1.6^level, salary doubles/5 per PROJECT_PLAN.md:4.4
    public class EconomyManager : MonoBehaviour
    {
        public int coins = 500;

        public int CalcProfit(float retail, float marketCost, int units, int salary, int stockCost)
        {
            float margin = retail - marketCost;
            return Mathf.RoundToInt(margin * units) - salary - stockCost;
        }

        public float FluctuateMarket(float baseCost)
        {
            // ±15% daily (RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:281)
            return baseCost * Random.Range(0.85f, 1.15f);
        }

        public int CostForLevel(int baseCost, int level)
        {
            return Mathf.RoundToInt(baseCost * Mathf.Pow(1.6f, level));
        }
    }
}
