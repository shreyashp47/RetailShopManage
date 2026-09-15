# Retail Store Tycoon — Project Plan
**Location:** `/Users/shreyash/RetailManage` (new app) | **Reference:** `/Users/shreyash/Documents/manageRetailShop`
**Intent:** GAME (not POS) — 3D low-poly retail tycoon inspired by XGame `Manage Retail Store` 4.4★ 10M+. Single source of truth: `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:1` (611 lines) + `CODE_ANALYSIS_REVERSE_ENGINEERING.md:1` + `AGENT.md:13`
**Mode:** BASIS FIRST — plan all chunks before any implementation. No code yet; validate chunks in order 0→10.

---

## 0. Ground Rules (Must-Enforce)

| Rule | Source | Implication for tasks |
|------|--------|----------------------|
| **7-step daily cycle** is mandatory | `AGENT.md:19` / `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:105` | CHUNK 2 is pivot; all other chunks feed it |
| **Hybrid manual→idle arc IS progression** | `AGENT.md:20` | CHUNK 5 visible automation, keep manual flavor tasks forever |
| **ONE spendable (coins) + reputation gate** | `AGENT.md:21` / `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:272` | CHUNK 4: never add 2nd hard currency without review |
| **Traps to avoid** | `AGENT.md:22` / `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:322` | CHUNK 3: NavMesh polish, never interstitial mid-checkout, carry 1→3 upgrade |
| **Art: low-poly 3D <100K tris <50 draws 1K tex occlusion** | `AGENT.md:23` | CHUNK 7 target, Isometric 2D only if lane flips |
| **Offline-first, 8h cap, sync never blocks** | `AGENT.md:24` / `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:296` | CHUNK 8 local JSON primary, Firebase optional |
| **NFR: 60fps SD660 3GB, AAB <100MB, <5s cold, <10%/h, crash-free >99.5%** | `AGENT.md:25` / `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:172` | Every chunk has perf exit gate |

**Locked stack for MVP** (`AGENT.md:13`): Unity **2022.3.67f2 URP**, C#10, Addressables, UGUI/UI Toolkit, DOTween, Cinemachine, NavMesh, Blender+Mixamo. Backend: Firebase/PlayFab (never blocks). Never Flutter.

**Open Decisions §16** (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:559`): Lane=Hybrid active→idle (Rec), Currency=1+rep, Style=low-poly, Ads=rewarded-only or no-ads V1, Engine=Unity, Subscription=skip v1, Theme=generic MVP → kirana/konbini Tier5/event. ⏸ Close before spec; CHUNK 0.6 is decision gate.

---

## Roadmap Map → Chunks

| Phase (22w) | Chunks covered | Exit |
|-------------|---------------|------|
| **P1 Prototype 4w** | 0, 1, 2-bare, 3-bare, 6-FTUE | Greybox loop spawn→sell→EOD, 1 SKU milk, internal playtest |
| **P2 MVP 8w** | 2-full, 3-full, 4 (Tier1-2), 5 (cashier/stocker), 8 (save/offline), 7-greybox→low-poly | Full 7-step cycle, 10 SKUs, carry 1→3, 60fps p50 |
| **P3 Polish 4w** | 6, 7, 4 balance, 10 QA | Final art/audio, haptics, 2× speed, localization EN/HI |
| **P4 Soft Launch 4w** | 9, 10, analytics | India+PH, D1>40% |
| **P5 Global 2w** | 4 Tier3-5, 9 season pass, 7 photo mode | Empire, 20 SKUs |

Basis order: **0 → 1 → 2 → 3 → 4 → 5 → 6 → 7 → 8 → 9 → 10** (no skipping).

---

## CHUNK 0 — Project Foundation & Tooling
*Goal: Reproducible empty Unity project that can pass NFR gates from day 1.*
*Deps: None. Blocks: all.*

- [ ] **0.1** Create Unity 2022.3.67f2 URP project `RetailManage` at `/Users/shreyash/RetailManage/unity` — verify `com.google.firebase.crashlytics.unity_version 2022.3.67f2` match from `CODE_ANALYSIS_REVERSE_ENGINEERING.md:25`
- [ ] **0.2** Git + LFS init (`.gitignore` Unity, `*.assets` LFS), `main` + `develop`, GitHub repo
- [ ] **0.3** UPM packages: Addressables, Cinemachine, AI Navigation (NavMesh), TextMeshPro, DOTween/DOTweenPro, MyBox, LeanPool, Input System. Keep `Firebase 13.6.0` (App/Analytics/RemoteConfig/Crashlytics) from decompiled `ScriptingAssemblies.json:7`; **remove** `MaxSdk.Scripts/Adverty5/Odeeo/Gadsme/Audiomob` (saves ~30M native `CODE_ANALYSIS_REVERSE_ENGINEERING.md:104`)
- [ ] **0.4** Assembly Definitions: `Game.Core`, `Game.Economy`, `Game.Customer`, `Game.Store`, `Game.Save`, `Game.UI`, `Game.IAP`, `Game.Analytics` (as per `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:422`)
- [ ] **0.5** Scenes skeleton: `Bootstrap` (init+RemoteConfig fetch) → `Home` (map Tier1-5) → `StoreGameplay` (grid 6×6) → `Expand/Decor` → `Result/LevelUp` (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:422`)
- [ ] **0.6** **Decision gate:** confirm §16 (hybrid, 1+rep, low-poly, Unity, no-ads V1 skip sub, generic MVP). Record in `DECISIONS.md`
- [ ] **0.7** CI: GitHub Actions + Unity Cloud Build, AAB pipeline, Test Lab matrix (SD660/SD665 3GB, Android 7 target 34 per `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:185`)
- [ ] **0.8** NFR harness: FPS counter (`CodeStage.AFPSCounter`), 5s cold start timer, AAB size check <100MB core, battery profiler
- [ ] **0.9** Copy reference docs locally (read-only): symlink `manageRetailShop/RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md`, `CODE_ANALYSIS_REVERSE_ENGINEERING.md`

**Exit:** Empty URP builds to AAB <20MB, boots <3s on editor, NavMesh package builds level0 equivalent (1315 objs baseline `CODE_ANALYSIS_REVERSE_ENGINEERING.md:40`), `Develop` green.

---

## CHUNK 1 — Core World & Store Grid (FR-1.1, FR-1.4)
*Goal: Place/remove shelves/fridges/checkouts on grid, expand tiles, own economy of space. Decompiled proof: `CakeShelf_lv1`/`FruitShelft_01_A`/`11_CoffeeTable`/`14_CoffeeMachine` prefabs (`CODE_ANALYSIS_REVERSE_ENGINEERING.md:58`).*
*Deps: 0. Blocks: 2, 3*

- [ ] **1.1** Grid system `StoreGrid` 6×6→12×12, snap, tile purchase (`FR-1.1`). Tiles = `ScriptableObject TileConfig {type, cost, unlockLevel}`
- [ ] **1.2** Placeable prefabs: `Shelf`, `Fridge`, `Checkout`, `Crate` — BoxCollider, MeshRenderer (placeholder greybox from Synty). Replicate decompiled split: `sharedassets1 552 MeshRenderer 552 MeshFilter` pattern
- [ ] **1.3** Build mode UI: catalog via computer interaction (see 2.1), drag ghost + valid/invalid tint, DOTween punch
- [ ] **1.4** Expansion gate: Tier1 6×6 free → Tier2 8×8 $800 → Tier3 12×12 $2500 (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:263`). Check `tiles[].type` serialization for save (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:431`)
- [ ] **1.5** Upgrade store size/floor, bigger shelves, commercial fridges (spoilage prevention `FR-1.4`) — hook for CHUNK 4 spoilage
- [ ] **1.6** Decor pass-through: `DecorConfig {id, buff}` 0.05 rating buff → reputation (`FR-1.5`/`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:133`). 5 decor for MVP, 10 for global
- [ ] **1.7** Save hook: `store.size {w,h}`, `tiles[]`, `decor[]`, `checkouts`, `fridges` (Data Model `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:431`)

**Exit:** Player can place 4 shelves+1 checkout, expand to 8×8, decor adds rating, all persisted via CHUNK 8 JSON.

---

## CHUNK 2 — The 7-Step Daily Cycle (BASIS Loop) (FR-1.2 + GDD §4)
*Goal: The addictive loop that IS the game. Gemini canonical 7 steps (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:105`). Basis first: bare loop with 1 SKU (milk), manual only.*
*Deps: 1, 3-bare. Blocks: 4,5*

> Steps: 1 Computer order → 2 Physically shelve one-by-one → 3 Price vs market → 4 Flip Open → 5 NavMesh customers → 6 Checkout mini-game → 7 EOD tally

- [ ] **2.1** **Step1 Preparation:** `OrderComputer` UI: catalog `SKUConfig {id,cost,marketCost,price,stock,shelf,demand}` (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:441`). Order → delivery van 30s timer (or gems speed for later). Boxes spawn outside. Decompiled ref: `ProductMachineItemMarket/Storage` (`CODE_ANALYSIS_REVERSE_ENGINEERING.md:59`)
- [ ] **2.2** **Step2 Stocking:** Player carry `Box` (`ModelFruitShelft_Box`), walk to shelf, place **one-by-one** (`FR-1.2`). Basis: carry 1. Upgrade 1→3 as `CarryUpgrade $200` (Gemini trap fix `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:130`). Haptic on drop. Empty→fill 0.6 animation
- [ ] **2.3** **Step3 Pricing:** Shelf tag click → slider `retail_price` vs `marketCost ±15% daily` (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:281`). Demand curve 30-50% margin sweet spot. `Product Info Canvas` ref (`CODE_ANALYSIS_REVERSE_ENGINEERING.md:59`)
- [ ] **2.4** **Step4 Open/Close:** Store sign flip `Closed→Open` (bool). Spawning gated by it. Cinemachine orbit + DOTween sign bounce
- [ ] **2.5** **Step5 Service:** Delegated to CHUNK 3, but wiring: `DayManager.Open()` → `CustomerSpawner.Start()` → `CheckoutQueue`
- [ ] **2.6** **Step6 Checkout Mini-Game:** Player-as-cashier `Cash Register`/`Checkout Drawer`/`8_Checkout Coffee` (`CODE_ANALYSIS_REVERSE_ENGINEERING.md:60`). Basis: tap scan + cash change calc `Coin_5_Cents` packs. Speed bonus. Keep **manual flavor forever** even after staff
- [ ] **2.7** **Step7 End-of-Day:** Close store → tally `profit = sales - cost - salary`, XP, `LevelUp` screen. `Gold_Upgrade` ref. Loop to Step1. Triggers LiveOps EOD interstitial gate (CHUNK 9: never mid-checkout)
- [ ] **2.8** `DayManager` FSM: `Preparation → Stocking → Pricing → Open → Checkout → EOD` (mermaid `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:224`)

**Exit:** 90-sec FTUE can run Steps 1-7 with 1 milk SKU, no staff, no crash. Basis loop is fun alone.

---

## CHUNK 3 — Customer AI & NavMesh (FR-2)
*Goal: Believable shoppers; trap #2 is AI glitches (wrong shelf, stuck cleaner `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:324`). Decompiled: 52 NavMeshAgents, 3 NavMeshData, `Agent-Cat/Boy_1/Girl_1/Business Man` (`CODE_ANALYSIS_REVERSE_ENGINEERING.md:62`).*
*Deps: 1, 2. Blocks: 5*

- [ ] **3.1** NavMesh bake: floor layer invisible, obstacles `NavMeshObstacle 10`, rebake from decompiled 3 `NavMeshData` pattern. Test corners/stuck reproducer scene
- [ ] **3.2** `CustomerSpawner` `FR-2.1`: spawn rate = `base 3/min Tier1 × (stars/5 ×1.5) × tierMultiplier` (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:263`). Pool via `LeanPool` (from decompiled)
- [ ] **3.3** `CustomerFSM`: States `Enter → Browse (path to shelf with stock) → Pick (take 1-5 basket `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:295`) → Queue → Pay → Exit`. Uses `Unity.AI.Navigation`
- [ ] **3.4** Patience `FR-2.3`: 30-60s queue tolerance, emoji thought bubble (`Customer Speech Missing Product`), leaves → star drop. Queue `BanThanhToan` pattern
- [ ] **3.5** Satisfaction `FR-2.4`: `stars` 1-5 reputation gate (non-spendable) → unlock departments. Affects spawn. Persist in `player.stars`
- [ ] **3.6** Thief event `FR-5` L15: 1% spawn (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:295`), chase mini-game — manual flavor task
- [ ] **3.7** Archetypes: `Cat_fat / Boy_1..5 / Girl_1..5 / Blue Collar Variant` with speed variance, premium customers attracted by decor rating `FR-1.5`
- [ ] **3.8** Wrong-shelf guard: `Customer` validates `shelf.sku == want`, cleaner path test — Gemini trap polish
- [ ] **3.9** Perf: <52 agents concurrent, LODGroup 69, SkinnedMeshRenderer 217 pattern from `CODE_ANALYSIS_REVERSE_ENGINEERING.md:51`, occlusion culling

**Exit:** 8 customers/min Tier3, zero stuck in 10-min stress (20 devices), patience visible, thief chase works.

---

## CHUNK 4 — Economy, Pricing & Progression (FR-3, FR-5)
*Goal: Long-term grind that sustains months not days. Tier 1-5 is content (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:261`).*
*Deps: 2, 3. Blocks: 5, 6, 9*

- [ ] **4.1** Currencies (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:272`): `Coins` soft spendable, `Gems` hard single-purpose (cosmetic/speed only), `Stars` reputation gate. **Never**add energy. `1/5/10/20/50 Dollar/Cent Pack` UI packs already in decompiled
- [ ] **4.2** `EconomyManager`: `sales = (retail - marketCost) × units`; `spend = stock + salary + expand`; Daily ±15% `marketCost` fluctuation (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:281`) via Remote Config
- [ ] **4.3** `SKUConfig` ScriptableObjects 20 SKUs total (MVP 10). Unlock path: bread/chips L1 → dairy/snacks L5 → electronics/meat/bakery L10 → premium L20+ (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:298`)
- [ ] **4.4** Costs: `base ×1.6^level`, salary doubles each 5 levels (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:295`). Tune via Remote Config `market_cost`, `delivery_time 30→10s`
- [ ] **4.5** XP/Level 1-50 `FR-5.1`, Stars gate `FR-2.4`, Tier table `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:263`: Corner 1/4/0 → Empire multi-store `FR-5.2`
- [ ] **4.6** Level unlocks table `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:284`: L2 snack $200, L5 2nd checkout+cashier $800, L10 fridge+dairy $2500, L15 security+thief $6000, L20 prestige 2×, L50 full 12×12
- [ ] **4.7** Prestige `FR-5.3`: L20 reset → 2× permanent multiplier
- [ ] **4.8** Quests/achievements `FR-5.3`: `sell_100_milk → 500 coins`, collections catalog, loginStreak (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:443`)
- [ ] **4.9** Spoilage: fridge prevents, else `fill` decays daily — DeepSeek supply chain gap (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:89`)
- [ ] **4.10** Offline earnings `FR-3.3`: `cap 8h` (rec vs 8-24h range), calc on resume, `lastOfflineAt` (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:295`)

**Exit:** Economy sim spreadsheet matches playtest 30 min to L8; prestige not pay-to-win.

---

## CHUNK 5 — Staff & Automation (FR-4) — The Hybrid Arc
*Goal: Hiring *visibly* automates Steps 2 & 6. Transition IS progression (Claude synthesis `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:65`). Decompiled gaps: no `Staff` GO names — logic is 1649 MonoBehaviours, need to design clean (`CODE_ANALYSIS_REVERSE_ENGINEERING.md:95`).*
*Deps: 3, 4. Blocks: 10*

- [ ] **5.1** Roles `FR-4.1`: `Cashier`, `Stocker`, `Cleaner`, `Manager` — `StaffConfig {role, level 1-10, salary, speed, carry}` (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:442`)
- [ ] **5.2** Hire UI: `Staff` tab, cost + salary preview (doubles each 5 levels). Cap by Tier: Tier1 0, Tier2 1, Tier3 5, Tier4 10+ (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:263`)
- [ ] **5.3** Visible automation `FR-4.3`: Stocker walks aisles (NavMesh) finds empty shelf (`fill<0.3`) → carries box → shelves one-by-one (same anim as player). Cashier auto-scans queue. Cleaner seeks dirty floors. **Polish wrong-shelf check**
- [ ] **5.4** Stats: `speed 1.0→1.4 at L3`, `carry 1→3` unlocked, salary drain each 5 min (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:272`)
- [ ] **5.5** Fatigue `FR-4.4`: after 5 min → -20% speed → rest/rotate UI
- [ ] **5.6** Manager buff: global +10% speed or auto-price tweak (choose one for MVP)
- [ ] **5.7** Keep manual flavor forever (`AGENT.md:20`): VIP customer, rare request, thief chase — player still has reason to stay active at Empire

**Exit:** After hiring cashier $200, player can idle 5 min while staff runs store visibly; no wrong-shelf bugs; fatigue visible.

---

## CHUNK 6 — UI/UX, FTUE & Screens (FR-7, §7)
*Goal: 90-sec FTUE that hits D1>40% (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:499`). Mermaid flows `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:207`.*
*Deps: 2, 4, 5. Blocks: 10*

- [ ] **6.1** FTUE 5 steps `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:205`: Splash → Coachmark1 Place shelf → 2 Order milk $5 → 3 Drag Box→Shelf haptic → 4 Set price $8 → 5 Customer→Queue→Scan → EOD profit $100 → Hire cashier. Skippable after 30s, 70% complete target
- [ ] **6.2** HUD: coins/gems/stars, day, speed 1×/2×, music/sound toggle (Gemini pattern `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:319`), haptics
- [ ] **6.3** Screens table `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:242`: `StoreView` (7-step), `Shop/Upgrade`, `Staff`, `Inventory` (computer), `Map` (Tier1-5), `Settings`, `IAP Store` (stub)
- [ ] **6.4** Interactions: joystick (smooth, dash — Gemini clunky fix), tap shelf tag, drag box, scan swipe. DOTween for all feedback (*oddly satisfying* Claude `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:115`)
- [ ] **6.5** Controls: carry 1→3 upgrade + dash, camera Cinemachine orbit, pinch zoom, portrait one-hand
- [ ] **6.6** Feedback: shelf fill, register `cha-ching`, `Amount` coin pop `Gold_Upgrade` VFX, `Shopping Bag` spawn
- [ ] **6.7** Accessibility: colorblind mode, text scaling, `InputValidator - Digits` pattern from decompiled `CODE_ANALYSIS_REVERSE_ENGINEERING.md:66`
- [ ] **6.8** Localization scaffold: EN (MVP), then ES/PT/HI/ID/JP (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:181`)

**Exit:** FTUE 90s, test with 5-10 users per `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:554`, no stuck coachmark.

---

## CHUNK 7 — Art, Audio & Performance
*Goal: Low-poly 60fps on SD660 (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:172`). Decompiled: 762 Texture2D + 343 Mesh + 226 Material + 69 LODGroup (`CODE_ANALYSIS_REVERSE_ENGINEERING.md:51`).*
*Deps: 1, 3. Blocks: 10*

- [ ] **7.1** Style lock: low-poly 3D default (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:313`). Greybox Phase1 → Synty pack / Blender 1K tex Phase2 → final Phase3. Avoid photorealistic 3× cost
- [ ] **7.2** 3D pipeline: Blender low-poly shelves/food/cash (`CakeShelf_lv1`, `FruitShelft_01`, `CoffeeMachine`, `IceCreamMachine` refs), <100K tris scene, <50 draws, occlusion culling
- [ ] **7.3** Characters: Mixamo rig walk/carry for `Boy/Girl/Business Man/Blue Collar/Agent-Cat` variants, `Spine/Animator` mix, 52 agents pooled
- [ ] **7.4** 2D/UI: `sharedassets0 25 Texture2D 15 Sprite` pattern — Product Icon BG/Image/Canvas, `Coin_50_Cents` packs
- [ ] **7.5** VFX: `ParticleSystem 41`, `AudioClip 18` pattern — box drop dust, van horn, fanfare. LeanPool for bags
- [ ] **7.6** Audio: FMOD/AudioMixer cheerful loop, register cha-ching, chatter ambient, upgrade fanfare, box drop, van horn; explicit music/sound toggle (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:319`)
- [ ] **7.7** Perf pass: URP, LOD, 1K tex max, 18 audio clips, bake lighting `LightmapSettings` (level0 161 MeshRenderer), profile p95 60fps
- [ ] **7.8** Size gate: Addressables for Tier4-5, AAB <100MB core / <150MB with packs (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:178`), <5s cold, <10%/h

**Exit:** Scene <100K tris, 60fps SD665 3GB p50, AAB 95MB.

---

## CHUNK 8 — Save, Offline & Backend (FR-7.3, FR-3.3)
*Goal: Offline-first, local primary, sync never blocks (`AGENT.md:24`). Data model `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:426`.*
*Deps: 1, 4, 5. Blocks: 10*

- [ ] **8.1** Local save: JSON + PlayerPrefs primary (`Game.Save` `SaveMgr`). Schema: `player {coins,gems,stars,level,xp,prestigeTier}`, `store {tier,size,tiles,decor,checkouts,fridges}`, `empire {stores[]}`, `catalog {skus[]}`, `staff[]`, `progress {quests, loginStreak, lastOfflineAt, offlineCapHours:8}`, `settings {speed,music,haptics}` (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:431`)
- [ ] **8.2** ScriptableObjects mirror: `SKUConfig`, `StaffConfig`, `DecorConfig`, `LevelConfig`, `CityConfig` (kirana/konbini future) — Remote Config can override `marketCost`
- [ ] **8.3** Cloud: Play Games Cloud Save + PlayFab PlayerData mirror (optional, fetch on `Bootstrap`). Never block core loop if offline
- [ ] **8.4** Offline earnings: on resume calc `min(now-lastOfflineAt,8h) × tierRate × starsMult` → Welcome Back popup, 2× via Rewarded (CHUNK 9)
- [ ] **8.5** Remote Config: Firebase Remote Config / PlayFab Config for `milk.marketCost`, `delivery_time`, `spawnRate`, `patience` (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:295`). A/B via Firebase A/B Testing
- [ ] **8.6** Analytics: Firebase + GameAnalytics funnel `install→tutorial→L1→D1→D7→IAP→ad watch→churn` (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:494`)
- [ ] **8.7** Crash: Crashlytics, `libFirebaseCppCrashlytics 46K` pattern, <0.5% sessions

**Exit:** Airplane mode fully playable 8h, cloud restores on reinstall, Remote Config tunes economy without update.

---

## CHUNK 9 — Monetization & LiveOps (FR-6)
*Goal: No-ads V1 decision 15 Sep 2026 (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:452`): IAP only. If ads ever added, keep to rewarded/every-3rd-EOD never mid-checkout.*
*Deps: 4, 8. Blocks: 10*

- [ ] **9.1** IAP catalog (Play Billing 8.3.0 `billing.properties:8` + `CodelessIAPStoreListener` pattern `CODE_ANALYSIS_REVERSE_ENGINEERING.md:29` but rebuild via Unity IAP): `Gems100 $0.99/₹89`, `Gems500 $4.99/₹449 +50 bonus`, `RestockBundle $1.99` (intent moment when shelf empty), `Starter Pack $1.99`, `No Ads $2.99` (future-proof), `Season Pass $4.99/mo` skip v1 (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:472`)
- [ ] **9.2** Placements: IAP store + **bundled at intent moments** 30-40% better conversion (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:467`). Cosmetic-first 58% revenue lever → decor/photo mode (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:468`)
- [ ] **9.3** Rewarded (if ads re-enabled post-V1): 3-5/day opt-in — Speed van, double coins 5 min, offline double (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:483`). Stub now, mediation MAX/LevelPlay later
- [ ] **9.4** Interstitial rule: every 3rd EOD only, cap 1 per 3 min, **never** mid-checkout — Gemini 1★ trap. Disabled V1, flag `adsEnabled=false`
- [ ] **9.5** LiveOps: Remote economy + events `Weekend Rush 2× spawn`, `Spoilage Week`, `Festival kirana/konbini decor`, Holidays (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:491`), 30-day seasons (add Month5 `RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:548`)
- [ ] **9.6** Push: FCM tied to game-state (shelf empty, offline ready) not spam (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:494`)
- [ ] **9.7** Compliance: Play Billing no alternative billing, GDPR/COPPA/CCPA, 3+ Families, AD_ID only if ads re-enabled (`CODE_ANALYSIS_REVERSE_ENGINEERING.md:83`)

**Exit:** Test purchase gems→speed delivery works, offline double stub, no ads in build, Remote Config event flips without update.

---

## CHUNK 10 — Polish, QA & Launch (Phase 3-5)
*Goal: D1>40% D7>20% D30>8% 8-12min 3-4/day (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:499`).*
*Deps: 6,7,8,9. Blocks: release.*

- [ ] **10.1** Balance: Remote Config tuning `spawn 3→15/min`, `patience 30→60s`, `basket 1-5`, `thief 1%` (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:295`), A/B price points
- [ ] **10.2** Polish: DOTween all UI, haptics, 2× speed toggle, smooth joystick+dash, colorblind, localization EN/HI→ES/PT/ID/JP, photo mode 13% share installs (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:468`)
- [ ] **10.3** QA NFR: 60fps profile p95 SD660, AAB <100MB, <5s cold, <10%/h, crash-free >99.5% (`CODE_ANALYSIS_REVERSE_ENGINEERING.md:98` gaps closed). Test 20 devices lab
- [ ] **10.4** Traps audit: no wrong-shelf, no stuck cleaner (repro corners), no interstitial mid-checkout, carry 1→3 feels good (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:322`)
- [ ] **10.5** Soft Launch: India+PH 5K users (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:542`), analytics review D1/D7 funnel, monetization A/B 30-40% lift (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:544`)
- [ ] **10.6** Global: release Play Console + App Store, ASO `Supermarket/Store Simulator/Tycoon` (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:72`), TikTok ASMR shelving, Tier3-5 Hypermarket→Empire 20 SKUs, kirana/konbini theme unlock
- [ ] **10.7** Risks check (`RETAIL_STORE_MANAGEMENT_GAME_RESEARCH.md:573`): Unity pricing fallback Godot, clone crowding via kirana moat, CPI <$0.50, grind wall via FTUE

**Exit:** Soft launch KPIs met, global `Tier5 Empire` playable.

---

## Execution Rules (Basis First)

1. **No code before sign-off:** Get approval on this file, then implement CHUNK 0 only. Each chunk exit gate must pass before next.
2. **One chunk in_progress max** (TodoWrite discipline).
3. **Verify each exit:** run build + playtest + NFR check before marking done.
4. **Decompiled is reference only:** Inspired mechanics (`7-step`, `NavMesh 52`, `Coin_50_Cents` packs) — recreate assets, don't copy `sharedassets` directly (educational note `CODE_ANALYSIS_REVERSE_ENGINEERING.md:125`).
5. **Track in this file:** check `- [ ]` → `- [x]` as we go. Create `DECISIONS.md` and `ISSUES.md` alongside.

---

## Next Step

Review this plan → tell me **"approve"** or edit chunk order → I start **CHUNK 0.1** (Unity create). To make decompiled runnable reference instead, say `rip assets` or `dump cs` (see `RetailStoreTycoon_Decompiled/README.md:27`).

*Generated 2026-09-15 basis-first. Sources: Live Play 13 games + Claude hybrid arc + Gemini 7-step + DeepSeek Tier1-5/KPIs/22w roadmap.*
