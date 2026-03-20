# 釣魚佬 CODING_RULES.md

## 1. 基本原則

### 技術棧
- Engine: Cocos Creator 3.8
- Language: TypeScript only
- Project Type: 2D
- Orientation: Portrait
- Target Platforms:
  - WeChat Mini Game
  - Douyin Mini Game

### 核心開發理念
- 代碼驅動優先
- 編輯器操作最小化
- 結構清晰優先於炫技
- 小步快跑、可迭代
- AI 可理解、可持續接手

---

## 2. AI 協作總規則

### 2.1 AI 輸出要求
每次新增或修改功能，AI 必須輸出：
1. 修改文件列表
2. 完整代碼
3. 節點樹變更說明
4. Inspector 綁定清單
5. 測試清單

### 2.2 AI 不應直接假設
- 不應默認場景中已經存在某些節點
- 不應默認某個 prefab 已經建立完成
- 不應默認某些資源路徑一定存在
- 不應默認第三方插件已接入

### 2.3 AI 生成代碼原則
- 優先小文件
- 優先可讀性
- 優先明確命名
- 優先低耦合
- 優先本地可跑通

---

## 3. 項目目錄規範

```text
assets/
  scenes/
  scripts/
    core/
    gameplay/
    ui/
    data/
    types/
    utils/
  prefabs/
    ui/
    gameplay/
  resources/
    images/
    audio/
    fonts/
  animations/
  materials/

docs/
  GAME_DESIGN.md
  CODING_RULES.md
  FEATURE_TODO.md
```

### 說明
- `core/`：全局管理器、應用入口、事件總線、存檔等
- `gameplay/`：釣魚流程、判定、獎勵等玩法控制
- `ui/`：面板、彈窗、HUD、按鈕控制
- `data/`：配置表、靜態數據、生成邏輯
- `types/`：接口、枚舉、狀態類型
- `utils/`：純工具函數

---

## 4. 命名規範

### 文件命名
- 類名與文件名一致
- 使用 PascalCase
- 示例：
  - `GameApp.ts`
  - `CastController.ts`
  - `ResultPopup.ts`
  - `FishTable.ts`

### 變量命名
- 成員變量：camelCase
- 常量：UPPER_SNAKE_CASE 或集中到 Config 中
- 布爾值以 `is` / `has` / `can` 開頭

### 節點命名
Hierarchy 中節點必須語義清晰：
- `Canvas`
- `HUD`
- `TopBar`
- `BottomBar`
- `CastButton`
- `PowerBar`
- `ResultPopup`
- `PopupLayer`

禁止出現大量無語義名稱：
- `Node`
- `node1`
- `test`
- `newNode`

---

## 5. 腳本設計規範

### 5.1 一個職責一個腳本
不要把整個玩法塞進一個超大腳本。

推薦拆分：
- `CastController.ts`
- `BiteController.ts`
- `FightController.ts`
- `RewardController.ts`
- `MainHUD.ts`

### 5.2 文件大小控制
- 儘量控制在 300 行內
- 超過 300 行要考慮拆分
- 超過 500 行默認視為需要重構

### 5.3 方法設計
- 方法要短小
- 方法命名要能直接說明用途
- 公開方法加簡短註釋

### 5.4 禁止濫用 any
- 能用 interface / type / enum 就不用 `any`
- 只有暫時過渡時允許使用 `any`

---

## 6. Cocos 組件規範

### 6.1 使用 @ccclass
所有組件腳本必須按 Cocos 3.8 標準寫法。

### 6.2 Inspector 暴露字段
所有需要拖綁的引用都必須：
- 類型明確
- 名稱清晰
- 使用 `@property(...)`

示例原則：
- `@property(Node) castButton: Node | null = null;`
- `@property(Sprite) powerBarFill: Sprite | null = null;`

### 6.3 不要到處寫死路徑
避免在多處使用：
- `find("Canvas/xxx/yyy")`

推薦：
- 在 Inspector 中顯式拖引用
- 或集中在初始化階段查找一次並緩存

---

## 7. 場景與 Prefab 規範

### 7.1 場景保持輕量
- `Main.scene` 只保留核心節點
- 通用 UI 優先 prefab 化

### 7.2 Prefab 適用場景
以下內容優先做成 prefab：
- 結算彈窗
- 魚卡片
- 提示 Toast
- 任務獎勵單元
- 商店商品項

### 7.3 節點樹規範
主場景節點建議：

```text
Main.scene
- Canvas
  - BgLayer
  - WaterLayer
  - FishLayer
  - EffectLayer
  - HUD
  - PopupLayer
```

### 7.4 不做深層嵌套
- 盡量不要超過 5~6 層深度
- 過深結構不利於 AI 和人工維護

---

## 8. 數據規範

### 8.1 配置表與邏輯分離
- 靜態數據放 `data/`
- 業務邏輯放 `gameplay/` 或 `ui/`

### 8.2 魚類數據表
魚類數據至少包含：
- id
- name
- rarity
- category
- value
- unlockCondition
- resultText
- atlasKey

### 8.3 配置集中管理
全局數值不允許散落各文件，應集中到：
- `Config.ts`
- 或專用配置表中

---

## 9. 狀態機規範

釣魚主流程至少拆成明確狀態：
- Idle
- Charging
- Casting
- WaitingBite
- Fighting
- Result

### 原則
- 使用 enum 定義狀態
- 狀態切換必須集中管理
- 禁止在多個腳本中各自偷偷改遊戲主狀態

---

## 10. UI 規範

### 10.1 UI 腳本單一責任
- `MainHUD.ts` 管 HUD
- `ResultPopup.ts` 管結算彈窗
- `ShopPanel.ts` 管商店
- `FishTankPanel.ts` 管魚缸

### 10.2 不直接在多處互相控制
UI 之間不要亂相互調用。
建議：
- 透過事件
- 或透過 UI 管理器

### 10.3 文案集中管理
搞笑文案、提示文案、評語文案應集中管理，不要散落硬編碼。

---

## 11. 事件系統規範

### 建議做法
建立簡單事件中心，如：
- `EventBus.ts`

事件示例：
- `FISH_BITE`
- `FISH_CAUGHT`
- `ROUND_FINISHED`
- `COIN_CHANGED`
- `TASK_UPDATED`

### 原則
- 事件名稱集中定義
- 事件使用要成對註冊與釋放

---

## 12. 存檔規範

### 首版存檔內容
- 金幣
- 圖鑑解鎖狀態
- 魚缸配置
- 稱號
- 每日任務進度
- 玩家設置

### 原則
- 用 `SaveManager.ts` 統一管理
- 不允許每個腳本自己亂存
- 數據結構明確版本化

---

## 13. Debug 與日誌規範

### 13.1 console 使用
- 開發期允許必要日誌
- 提交前清理無意義日誌
- 不要到處 `console.log("test")`

### 13.2 錯誤處理
- 對可能為空的節點和引用做防護
- 初始化失敗時輸出清晰錯誤信息

---

## 14. 性能規範

### 首版性能原則
- 少用高頻 instantiate / destroy
- 常用彈窗與單元優先考慮池化
- 避免無限制 update
- 避免不必要 Tween 疊加

### 2D 小遊戲優先原則
- 小體積
- 快啟動
- 低記憶體壓力
- 避免過多特效與大圖

---

## 15. Git 與提交規範

### 提交粒度
- 一個功能一個提交
- 不要把無關修改混在一起

### 提交說明建議
- `feat: add cast charging flow`
- `fix: correct bite timing logic`
- `refactor: split result popup and reward logic`
- `docs: update fishinglao feature todo`

---

## 16. AI 任務下發模板

每次讓 AI 做功能時，需求格式建議如下：

```text
請基於 Cocos Creator 3.8 + TypeScript 實現以下功能：
1. 功能目標
2. 涉及文件
3. 需要新增的節點
4. Inspector 綁定要求
5. 不要假設已有 prefab
6. 請輸出完整代碼、節點樹變更、測試清單
```

---

## 17. 禁止事項

- 禁止一個腳本管全部遊戲流程
- 禁止大量無語義節點名稱
- 禁止把配置散落在多個 UI 腳本裡
- 禁止多處寫死節點路徑
- 禁止 UI 腳本彼此硬耦合到不可維護
- 禁止一口氣做過多功能不測試
- 禁止先追求炫技而忽視 MVP 跑通

---

## 18. 當前開發結論

本項目規則的核心不是“做最炫的 Cocos 遊戲”，而是：

> 用最清晰、最可維護、最適合 AI 協作的方式，快速做出一個可發布、可傳播、可持續迭代的 2D 搞笑釣魚小遊戲。
