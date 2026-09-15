# RetailShopManage — 3D Low-Poly Retail Tycoon

Inspired by XGame `Manage Retail Store` (4.4★ 10M+). This repo is a **clean inspired rebuild** (not 1:1 clone) based on research in `/Users/shreyash/Documents/manageRetailShop`.

## Stack
- **Engine:** Unity 2022.3.67f2 URP, C#10, Addressables, UGUI/UI Toolkit, DOTween, Cinemachine, NavMesh
- **Art:** Blender low-poly + Mixamo (1K tex, <100K tris, <50 draws, occlusion)
- **Backend (never blocks):** Firebase 13.6.0 / PlayFab — offline-first JSON + PlayerPrefs primary, 8h cap
- **Monetization V1:** IAP only (no ads), Play Billing 8.3.0

## Project Plan
See [`PROJECT_PLAN.md`](./PROJECT_PLAN.md) — 11 chunks (0 Foundation → 10 Launch), basis-first order.

- **7-step daily cycle:** computer order → shelve one-by-one (carry 1→3) → price vs market±15% → flip Open → NavMesh customers → checkout mini-game → EOD tally
- **Hybrid manual→idle:** hire staff to *visibly* automate, keep manual flavor (VIP/thief)
- **Tier 1-5:** Corner → Convenience → Supermarket → Hypermarket → Retail Empire

## Structure (planned)
```
unity/                  # Unity 2022 URP project (CHUNK 0.1)
  Assets/
    Scripts/Game.Core
    Scripts/Game.Economy
    Scripts/Game.Customer
    Scripts/Game.Store
    Scripts/Game.Save
    Scenes/ Bootstrap, Home, StoreGameplay, Expand, Result
  Packages/
```

## Reference (read-only)
- `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md` (611 lines) — single source of truth
- `CODE_ANALYSIS_REVERSE_ENGINEERING.md` — decompiled APK analysis (libil2cpp 59M, 5426 GO, 52 NavMeshAgents)
- `AGENT.md` — must-enforce rules & NFR (60fps SD660, <100MB, <5s cold)

## NFR Targets
60fps SD660 3GB p50, AAB <100MB core, <5s cold start, <10%/h drain, crash-free >99.5%, D1>40% D7>20% D30>8%

## Roadmap
Prototype 4w → MVP 8w → Polish 4w → Soft Launch 4w (India+PH) → Global 2w = 22w

## License
Educational — do not redistribute decompiled assets. Recreate inspired mechanics.
