using UnityEngine;

namespace Game.Core
{
    // CHUNK 2.8: 7-step FSM — Preparation → Stocking → Pricing → Open → Checkout → EOD
    // Basis: manual only, no staff. Mirrors RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:224
    public enum DayPhase { Preparation, Stocking, Pricing, Open, Checkout, EOD }

    public class DayManager : MonoBehaviour
    {
        public DayPhase Phase { get; private set; } = DayPhase.Preparation;
        public int Day { get; private set; } = 1;

        public void SetPhase(DayPhase p)
        {
            Phase = p;
            Debug.Log($"[DayManager] Day {Day} -> {p}");
        }

        public void EndDay()
        {
            Day++;
            SetPhase(DayPhase.Preparation);
            // TODO: CHUNK 4 profit tally = sales - cost - salary, XP
        }
    }
}
