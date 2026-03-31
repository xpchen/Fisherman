# Fisherman - Unity 6 Project

## Project Overview
休闲搞笑风格第一人称钓鱼手游，目标平台: iOS + 抖音小游戏 + 微信小游戏

## Tech Stack
- Unity 6 (6000.4.0f1) + URP
- C#
- Target: iOS, WebGL (抖音/微信小游戏)

## Project Structure
- `Assets/_Project/Scripts/Core/` - EventBus, GameManager, SaveSystem, FishData, FunnyTextSystem
- `Assets/_Project/Scripts/Fishing/` - FishingController (state machine), CastSystem, BiteSystem, FightSystem
- `Assets/_Project/Scripts/Economy/` - EconomySystem
- `Assets/_Project/Scripts/UI/` - FishingHUD, ResultPanel, ShopPanel
- `Assets/_Project/Scripts/Audio/` - AudioManager
- `Assets/_Project/ScriptableObjects/` - Fish configs, Equipment configs

## Architecture
- Event-driven via `EventBus` (publish/subscribe)
- Fishing state machine: Idle → AimCast → Casting → WaitingBite → BiteWindow → Fighting → CatchSuccess/CatchFail
- Data-driven: Fish/Equipment configs via ScriptableObject
- Local save via JSON

## Style
- 幽默搞笑风格 (Humorous/Funny style)
- Q版卡通 (Chibi cartoon)
- FunnyTextSystem provides random humorous text for all game events
