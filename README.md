[README (1).md](https://github.com/user-attachments/files/33059155/README.1.md)
# Indian Cafe Simulator — Project README

Cozy first-person cafe simulation game, Indian theme (chai/samosa/desi aesthetic),
adapted from the "Lo-Fi Cafe Simulator" GDD. Built in **Unity 6.3 (6000.3.8f1)**,
URP, Input System package.

---

## 1. Environment Setup

- **Unity version:** 6000.3.8f1 (Unity 6.3 LTS)
- **Template:** Universal 3D (URP)
- **Active Input Handling:** Input System Package (New)
  `Edit → Project Settings → Player → Other Settings → Active Input Handling`
- **Packages installed:**
  - Input System
  - AI Navigation (2.0.15) — for NavMesh customer pathing

### Folder Structure
```
Assets/
 ├─ Scenes/
 │   └─ CafeMain.unity (or SampleScene)
 ├─ Scripts/
 │   ├─ Player/
 │   ├─ Interaction/
 │   ├─ UI/
 │   ├─ Customer/
 │   ├─ Cooking/
 │   ├─ Economy/        (not built yet)
 │   └─ SaveSystem/      (not built yet)
 ├─ Prefabs/
 └─ Art/
```

### Layers
- **Interactable** (User Layer) — assigned to any object the player can raycast-interact with (pickup items, cooking stations, customer serve points)

### Step-by-Step: Setting up the project from scratch
1. Unity Hub → **New project** → Editor version **6000.3.x** → Template **Universal 3D** → name it, Create
2. `Edit → Project Settings → Player → Other Settings → Active Input Handling` → set to **Input System Package (New)** (Editor will ask to restart — accept)
3. `Window → Package Manager` → dropdown **Unity Registry** → search `AI Navigation` → **Install**
4. Create the folder structure above (Right-click in Project window → Create → Folder)
5. Inspector (any object) → top-right **Layers → Edit Layers** → add `Interactable` as a User Layer
6. `File → Save As` → save scene as `Assets/Scenes/CafeMain.unity`

---

## 2. Systems Built So Far

Build order follows the GDD's "Core Unity Systems & Scripts" section:

| # | System | Status |
|---|--------|--------|
| 1 | Player Controller | ✅ Done |
| 2 | Customer AI (NavMesh) | ✅ Done |
| 3 | Order & Cooking Logic | 🔧 In progress |
| 4 | Economy & Restock | ⬜ Not started |
| 5 | Save / Load | ⬜ Not started |
| 6 | Basic Prototype PC system | ✅
| 7 | Main Menu | ✅

---

## 3. System 1: Player Controller

**Scripts:** `Scripts/Player/`, `Scripts/Interaction/`

- `PlayerInputHandler.cs` — reads Move/Look/Sprint/Jump/Interact/Drop via the new Input System (actions defined in code, no .inputactions asset needed)
- `PlayerController.cs` — first-person movement using `CharacterController` (walk/sprint/jump/gravity), mouse look with pitch clamp, Esc to toggle cursor lock
- `IInteractable.cs` — interface for anything the player can interact with (`Interact`, `OnHoverEnter`, `OnHoverExit`, `InteractionPrompt`)
- `PlayerInteractor.cs` — raycasts from camera, detects `IInteractable` on the `Interactable` layer, shows prompt UI, handles pickup/drop of held items
- `PickupItemBase.cs` — base class for any holdable object (attach to hold point / detach with push force)
- `PickupItem.cs` — simple pickup (inherits `PickupItemBase`)
- `UI/InteractionPromptUI.cs` — shows "[E] ..." prompt text via TextMeshPro

### Scene objects
- **Player** — CharacterController, PlayerInputHandler, PlayerController, PlayerInteractor
  - **Main Camera** (child) — eye height ~1.6, near clip 0.01
    - **HoldPoint** (child) — where picked-up items attach
- **Canvas** — Screen Space, with **PromptText** (TMP) and **Crosshair** (Image)

### Controls
| Key | Action |
|---|---|
| WASD | Move |
| Mouse | Look |
| Shift | Sprint |
| Space | Jump |
| E | Interact / Pick up |
| Q | Drop held item |
| Esc | Toggle cursor lock |

### Step-by-Step: Building the Player Controller
1. Create the 6 scripts listed above in `Scripts/Player/`, `Scripts/Interaction/`, `Scripts/UI/` with their code
2. Basic greybox: `3D Object → Plane` named **Floor** at `(0,0,0)` scale `(2,1,2)`; `3D Object → Cube` named **Counter** at `(0,0.5,2)` scale `(4,1,1)`
3. Hierarchy → `Create Empty` named **Player** at `(0,0,-3)`
4. Player → **Add Component → Character Controller** (Center `(0,0.9,0)`, Height `1.8`, Radius `0.3`)
5. Drag existing **Main Camera** into Player as a child → Position `(0,1.6,0)`, Rotation `(0,0,0)`; Camera component → Near Clip Plane `0.01`
6. Inside Main Camera, `Create Empty` named **HoldPoint** at local `(0.4,-0.3,0.8)`
7. UI: `Create → UI → Canvas` (Scaler: Scale With Screen Size, 1920×1080) → inside it, `UI → Text - TextMeshPro` named **PromptText** (import TMP Essentials when prompted, anchor bottom-center) and `UI → Image` named **Crosshair** (centered, small)
8. Canvas → **Add Component → Interaction Prompt UI** → drag PromptText into its field
9. Select **Player** → add components **PlayerInputHandler**, **PlayerController** (Camera Transform = Main Camera), **PlayerInteractor** (Camera Transform = Main Camera, Hold Point = HoldPoint, Prompt UI = Canvas, Interactable Layer = Interactable)
10. Test pickup object: `3D Object → Cube` named **Chai Cup**, scale `(0.2,0.2,0.2)`, position on the counter → add **Rigidbody** + **PickupItem** (Item Name = "Chai Cup") → set its Layer to **Interactable**
11. Press Play — WASD/mouse/E/Q should all work per the controls table above

---

## 4. System 2: Customer AI

**Scripts:** `Scripts/Customer/`

- `CustomerState.cs` — enum: Entering → WaitingAtCounter → Ordering → WaitingForOrder → MovingToSeat → Eating → Leaving
- `OrderData.cs` — ScriptableObject (`Create → Cafe → Order Data`): item name, icon, patience time, coin reward
- `CustomerAI.cs` — `NavMeshAgent`-driven lifecycle: walks to counter, places a random order, waits (with patience timer), on `ReceiveOrder()`/`TryServe()` match walks to seat, eats, then exits. Fires `OnOrderExpired` / `OnCustomerLeft` events.
- `CustomerSpawner.cs` — spawns customer prefabs at intervals, assigns a free seat, tracks active customers, cleans up on exit (seat-tracking via a `Dictionary<CustomerAI, Transform>`)

### Scene objects
- **NavMeshSurface** — baked NavMesh (Collect Objects = All)
- **EntryPoint**, **CounterPoint**, **ExitPoint**, **SeatPoint_1** (empty GameObjects marking positions)
- **CustomerSpawner** — holds references to the above points + the Customer prefab
- **Prefabs/Customer** — Capsule placeholder, `NavMeshAgent`, `CustomerAI`, child `OrderIconAnchor`

### Order Data assets
Created under `Assets/...` via `Create → Cafe → Order Data`:
- MasalaChai, Coffee, Samosa (name / patience time / coin reward configured per asset)

### Step-by-Step: Building Customer AI
1. Floor and Counter objects → Inspector top-right **Static** dropdown → tick **Navigation Static** on both (needs the AI Navigation package installed first, with an Editor restart if the option is greyed out)
2. Hierarchy → `Create Empty` named **NavMeshSurface** → **Add Component → Nav Mesh Surface** → Collect Objects = **All** → click **Bake** (blue overlay should appear on the Floor)
3. Create 4 empty GameObjects for positions: **EntryPoint** `(0,0,-8)`, **CounterPoint** `(0,0,1)`, **ExitPoint** `(0,0,-8)`, **SeatPoint_1** `(2,0,3)`
4. Create the 4 scripts in `Scripts/Customer/`: `CustomerState.cs`, `OrderData.cs`, `CustomerAI.cs`, `CustomerSpawner.cs`
5. Project window → right-click → `Create → Cafe → Order Data` → make 3 assets (MasalaChai, Coffee, Samosa), fill in Item Name / Patience Time / Coin Reward on each
6. Hierarchy → `3D Object → Capsule` named **Customer** → **Add Component → Nav Mesh Agent** (Radius `0.3`, Height `1.8`, Stopping Distance `0.5`) → **Add Component → Customer AI** → drag the 3 OrderData assets into `Possible Orders`
7. Inside Customer, `Create Empty` child named **OrderIconAnchor** at local `(0,2,0)` → drag into CustomerAI's `Order Icon Anchor` field
8. Drag the **Customer** object into `Assets/Prefabs` to make a prefab, then delete it from the Hierarchy
9. Hierarchy → `Create Empty` named **CustomerSpawner** → **Add Component → Customer Spawner** → wire Entry/Counter/Seat/Exit points, drag the Customer prefab in, set Max Active Customers
10. Press Play — a customer should spawn every 5–12s, walk to the counter, and wait

---

## 5. System 3: Order & Cooking Logic (in progress)

**Scripts:** `Scripts/Cooking/`

- `CookingStage.cs` — enum: Idle → Boiling → Pouring → Ready
- `CookingStation.cs` — `IInteractable`; state machine with timed Boiling stage, held-interact Pouring stage, then spawns a `ServedItem` the player picks up automatically
- `ServedItem.cs` — holdable finished item carrying an `OrderData` reference (inherits `PickupItemBase`)
- `CustomerServePoint.cs` — `IInteractable` trigger near each customer; player interacts while holding a matching `ServedItem` to serve it (calls `CustomerAI.TryServe()`)

### Required refactor
`PickupItem.cs` was split into a `PickupItemBase` base class so `ServedItem` could extend it; `PlayerInteractor`'s `HeldItem` / `TryPickUp` / `ReleaseHeldItem` now use `PickupItemBase` instead of `PickupItem`.

Also added:
- `PlayerInputHandler.InteractHeld` (bool, for the hold-to-pour mechanic)
- `PlayerInteractor` now calls `CookingStation.SetHolding()` each frame while looking at a station

### Scene objects (per station, e.g. Chai)
- **ChaiStation** (Interactable layer) — `CookingStation` component, `SpawnPoint` child
- **Prefabs/ChaiCupServed** — Rigidbody + `ServedItem`
- Customer prefab — **ServePoint** child (trigger collider, Interactable layer) with `CustomerServePoint`

### Status
Scripts written; scene wiring (stations + served-item prefab + serve points on customer prefab) and in-editor testing still to be confirmed.

### Step-by-Step: Building Order & Cooking Logic
1. Create `Scripts/Cooking/CookingStage.cs`
2. Refactor: create `Scripts/Interaction/PickupItemBase.cs` (move the old `PickupItem.cs` logic into it), then shrink `PickupItem.cs` to `public class PickupItem : PickupItemBase { }`
3. Update `PlayerInteractor.cs` — change `HeldItem` / `TryPickUp` / `ReleaseHeldItem` to use `PickupItemBase` instead of `PickupItem`
4. Add `InteractHeld` (bool, `interactAction.IsPressed()`) to `PlayerInputHandler.cs`
5. In `PlayerInteractor.Update()`, after detecting the current target, call `station.SetHolding(input.InteractHeld)` when the target is a `CookingStation`
6. Create `Scripts/Cooking/ServedItem.cs`, `Scripts/Cooking/CookingStation.cs`, `Scripts/Customer/CustomerServePoint.cs`
7. Add `TryServe(ServedItem item)` to `CustomerAI.cs`
8. Confirm the project compiles with no red errors before touching the scene
9. Scene: `3D Object → Cube` named **ChaiStation** on the counter, Layer = Interactable → child `Create Empty` **SpawnPoint** → **Add Component → Cooking Station** (Order Produced = MasalaChai, Boil Duration `4`, Pour Hold Duration `2`)
10. `3D Object → Cube` named **ChaiCupServed**, scale `(0.15,0.15,0.15)` → **Add Component → Rigidbody** + **Served Item** → drag into `Assets/Prefabs`, delete from Hierarchy, then drag the prefab into ChaiStation's `Served Item Prefab` field
11. Open the **Customer** prefab (double-click) → `Create Empty` child **ServePoint** at local `(0,1,0.5)` → add a trigger **Box Collider**, Layer = Interactable → **Add Component → Customer Serve Point** → wire its `Customer` field to the prefab's root (or use `GetComponentInParent<CustomerAI>()` if drag-and-drop isn't possible in prefab mode) → save and exit prefab mode
12. Press Play — interact with ChaiStation to start boiling, hold E through Pouring, take the item, then walk to a waiting customer's ServePoint and press E to serve

---

## 6. Not Yet Built

- **Economy & Restock** — coin tracking, ingredient inventory/depletion, shop purchases
- **Save / Load** — JSON or ScriptableObject-based persistence
- **UI polish** — HUD (coins, active orders), main menu, shop/upgrade menu
- **Cooking progress UI** — visual bar for Boiling/Pouring (currently only tracked in code via `CookingStation.StageProgress`)
- **Indian theming** — art/assets swap (chai kettle, samosa, desi decor) — current objects are all greybox placeholders (cubes/capsules)

---

## 7. Known Gaps / Things to Double-Check When Resuming

- Confirm `PickupItem.cs` now reads `public class PickupItem : PickupItemBase { }`
- Confirm `CustomerServePoint.customer` field is wired to the correct `CustomerAI` on the prefab (may need `GetComponentInParent<CustomerAI>()` if drag-and-drop isn't possible inside prefab mode)
- No cooking-stage progress UI yet — currently silent except for prompt text changes
- Only one seat point (`SeatPoint_1`) exists — add more for multiple simultaneous customers
