# Decisions — RetailShopManage (§16)

Locked 2026-09-15 per `PROJECT_PLAN.md:22` + owner confirmation **NO ads library**. Changes require issue + review.

| # | Decision | Choice (Locked) | Alternative | Impact if changed |
|---|----------|-----------------|-------------|-------------------|
| 1 | **Lane** | **Hybrid active→idle** (manual early, staff visibly automates, keep VIP/thief forever) | Idle clicker | Wasted 3D art/perf if idle |
| 2 | **Currency** | **1 spendable coins + stars reputation gate**. Gems single-purpose (cosmetic/speed via IAP) | Multi-currency | Review red flag |
| 3 | **Art Style** | **Low-poly 3D** (<100K tris, <50 draws, 1K tex, occlusion). Isometric 2D only if lane flips | Photorealistic | 3× cost, fails SD660 |
| 4 | **Ads** | **NONE — no ads SDK** (no MAX/LevelPlay/AdMob, no `AD_ID` perms, saves ~30M + 865 yandex) | Rewarded/interstitial | Owner rejected 2026-09-15 |
| 5 | **Engine** | **Unity 2022.3.67f2 URP** (exact `CODE_ANALYSIS:25`, changeset `451350a4a9f0`) | Godot 4 | Rebuild |
| 6 | **Subscription** | **Skip v1**, add Month 5 season pass | Day1 pass | Churn |
| 7 | **Regional Theme** | **Generic MVP**, kirana/konbini as Tier5/event | Kirana Day1 | Double art before fun |

**Flags:**
- `adsEnabled=false` permanent
- `offlineCapHours=8`
- `unityVersion=2022.3.67f2` `changeset=451350a4a9f0`
- `minSdk=24 targetSdk=34` (`CODE_ANALYSIS:10`)
- `package=com.retail.shop.manage` (to be set in PlayerSettings, distinct from `com.manage.retail.store`)

**Record:** All CHUNK 0.6 tasks must read this file.
