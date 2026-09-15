using UnityEngine;

namespace Game.IAP
{
    // CHUNK 9.1 IAP ONLY, NO ADS per DECISIONS.md:4
    // Play Billing 8.3.0 via Unity IAP Codeless replacement, no MaxSdk
    // Catalog: Gems100 $0.99/₹89, Gems500 $4.99, Restock $1.99, Starter $1.99, Premium $2.99
    public class IAPManager : MonoBehaviour
    {
        public void BuyGems100() => Debug.Log("[IAP] Buy Gems100 $0.99");
        public void BuyGems500() => Debug.Log("[IAP] Buy Gems500 $4.99");
        public void BuyRestockBundle() => Debug.Log("[IAP] RestockBundle $1.99 intent when empty");
        public void BuyStarterPack() => Debug.Log("[IAP] Starter $1.99");
        public void BuyPremiumUnlock() => Debug.Log("[IAP] Premium Unlock $2.99 (replaces No Ads)");

        // Verify no ads: no Rewarded/Interstitial code, AD_ID absent
    }
}
