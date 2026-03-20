FishingScene 第一批素材清单 v0.1
一、目标

这一批素材的目标不是“做完整美术包”，而是先把 第一版可玩的钓鱼场景 跑起来。

第一批只解决 4 件事：

进入钓鱼场景后画面成立

玩家角色能站住

浮标 / 水花 / 鱼影能动起来

HUD 能表达“蓄力 + 抛竿 + 咬钩”

二、分层结构对应素材
1. Bg 背景层

这层负责场景氛围，不负责互动。

必做

fishing_bg_lake_day_clean

白天湖景纯背景

不要人物

不要UI

不要固定钓竿

要有远山、天空、湖面、岸边环境

可选补充

fishing_bg_far_mountain

fishing_bg_sky_cloud

fishing_bg_far_forest

如果想快做，第一版可以先只做一张：

fishing_bg_lake_day_clean

2. Foreground 前景层

这层负责“游戏感”和空间层次。

必做

fishing_fg_dock_bottom

底部木码头/岸边木板

透明背景 PNG

放在屏幕下半部

给角色“坐/站”的落点

fishing_fg_rocks_left

左下角石头/草丛装饰

fishing_fg_rocks_right

右下角石头/草丛装饰

可选搞笑装饰

fishing_fg_bucket_funny

夸张鱼桶/塑料桶

fishing_fg_bait_box

搞笑饵料盒

fishing_fg_old_shoe

旧鞋/破靴子，强化幽默感

3. Character 角色层

这层必须独立，第三人称核心。

必做

player_fisher_back_idle

主角背影站姿/坐姿 idle

卡通、稍夸张、亲和

看起来像“钓鱼佬”

可有草帽、渔夫帽、大背包

player_fisher_back_cast_prepare

蓄力准备动作

player_fisher_back_cast_release

放手抛竿动作

player_fisher_back_reel_idle

普通收线动作

第一版最小可行

如果想更快，第一批甚至可以先只做：

player_fisher_back_idle

后面再补动作。

可选搞笑角色变体

player_hat_straw_01

player_hat_funny_02

player_backpack_big_01

这样后面能做轻收集/换装。

4. Rod / Float 钓具层

钓鱼玩法核心层。

必做

rod_01_idle

普通钓竿

rod_01_bent_light

轻微弯曲

rod_01_bent_heavy

明显弯曲

float_01_idle

标准浮标

float_01_bite

咬钩状态浮标

hook_01

钩子

line_segment_01

鱼线素材，或用代码画线也行

第一版最小可行

rod_01_idle

float_01_idle

float_01_bite

5. Water / Fish 鱼与水面层

这层决定有没有“钓鱼感”。

必做

fish_shadow_small_01

fish_shadow_medium_01

fish_shadow_large_01

用途：

水下游动

不同鱼种大小感

咬钩前预兆

fx_ripple_small_01

小波纹

fx_ripple_medium_01

中波纹

fx_splash_small_01

小水花

fx_splash_big_01

大鱼/重物水花

可选

fish_jump_silhouette_01

bubble_trail_01

6. HUD 层

这层不是场景图，而是交互表达。

必做

hud_power_bar_bg

hud_power_bar_fill

hud_aim_ring

hud_cast_hint

hud_bite_alert

hud_pause_button

推荐功能对应

左下：蓄力条

右侧：方向控制

中间：落点圈/抛竿提示

上方：当前鱼点/小地图/倒计时（后补）

可选搞笑提示

hud_text_bite_now

hud_text_pull_pull

hud_text_big_one

让节奏更轻松。

三、第一批“必须先有”的最小组合

如果你想最快进到 第一版能玩的 FishingScene，先只做下面这 12 个：

场景

fishing_bg_lake_day_clean

fishing_fg_dock_bottom

fishing_fg_rocks_left

fishing_fg_rocks_right

角色

player_fisher_back_idle

钓具

rod_01_idle

float_01_idle

float_01_bite

水面/鱼

fish_shadow_small_01

fx_ripple_small_01

fx_splash_small_01

HUD

hud_power_bar_bg

hud_power_bar_fill

hud_aim_ring

严格说 12 个就够起步，HUD 那 3 个我建议直接一起补上。

四、素材风格约束

这部分很重要，后面你拿去继续生成图时要统一。

风格关键词

2D game art

vertical mobile game

bright relaxing lake

casual fishing

humorous and charming

clean readable shapes

light cartoon realism

colorful but not noisy

mobile game UI friendly

画面要求

明亮

治愈

轻松

略搞笑

不能太写实

不能太复杂太满

要给 UI 留空间

要适合小程序 / 手机竖屏

明确避免

不要全写实厚涂

不要电影海报感

不要大量复杂小物件

不要固定大面积 UI 在背景里

不要第一人称手持视角

不要把角色焊死在背景里

五、尺寸和导出建议
场景背景

比例：9:16 竖屏

建议基准：1080 x 1920

如果 AI 先出高分版，可以 1536 x 2732 再下采样

角色 / 前景 / 道具

导出：透明 PNG

角色高度建议占屏幕高的 18% ~ 28%

浮标尽量单独小图

水花 / 波纹也单独透明 PNG

命名规则

统一小写英文，下划线命名：

fishing_bg_lake_day_clean

player_fisher_back_idle

fx_ripple_small_01

后面你在 Cocos 里会很舒服。

六、Cocos 里推荐节点映射

你未来在 FishingScene 里可以按这个挂：

FishingScene
└─ Canvas
   ├─ Bg
   │  └─ fishing_bg_lake_day_clean
   ├─ ForegroundLayer
   │  ├─ fishing_fg_dock_bottom
   │  ├─ fishing_fg_rocks_left
   │  └─ fishing_fg_rocks_right
   ├─ CharacterLayer
   │  ├─ player_fisher_back_idle
   │  └─ rod_01_idle
   ├─ FloatLayer
   │  ├─ float_01_idle
   │  └─ HookLine
   ├─ FishLayer
   │  └─ fish_shadow_small_01
   ├─ EffectLayer
   │  ├─ fx_ripple_small_01
   │  └─ fx_splash_small_01
   └─ HUD
      ├─ hud_power_bar_bg
      ├─ hud_power_bar_fill
      └─ hud_aim_ring
七、第一批生成顺序

别一口气全做，按顺序最稳：

第 1 轮

fishing_bg_lake_day_clean

fishing_fg_dock_bottom

player_fisher_back_idle

第 2 轮

rod_01_idle

float_01_idle

fish_shadow_small_01

第 3 轮

fx_ripple_small_01

fx_splash_small_01

hud_power_bar_bg / fill / aim_ring

这样最快能把玩法跑起来。

八、对你当前那张参考图的处理建议

你刚发的那种图可以继续当：

氛围参考图

构图参考图

色彩参考图

第三人称视角参考图

但不要直接拿它当正式可用游戏场景。
正式素材要按上面这套单独生成。

九、我给你的最实用结论

你现在不要去“拆那张整插画”，而是直接开始补这三个：

纯背景： fishing_bg_lake_day_clean

前景码头： fishing_fg_dock_bottom

第三人称角色背影： player_fisher_back_idle

这 3 个一出来，你的 FishingScene 就能真正从“占位场景”变成“像游戏”。

下一步我建议直接进入：
我帮你写这 3 张素材的可直接生成用提示词。