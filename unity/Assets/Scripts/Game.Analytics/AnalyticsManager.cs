using UnityEngine;

namespace Game.Analytics
{
    // CHUNK 8.6: Firebase + GameAnalytics funnel install→tutorial→L1→D1→D7 per RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:494
    // No ads events
    public class AnalyticsManager : MonoBehaviour
    {
        public void LogEvent(string name, string param = null)
        {
            Debug.Log($"[Analytics] {name} {param}");
            // TODO: Firebase Analytics when editor ready (Firebase 13.6.0)
        }

        public void LogFTUE(string step) => LogEvent("ftue_" + step);
        public void LogPurchase(string sku, float price) => LogEvent("iap_purchase", $"{sku}:{price}");
        public void LogEOD(int day, int profit) => LogEvent("eod", $"day{day}:{profit}");
    }
}
