using UnityEngine;

namespace Fisherman.Core
{
    /// <summary>
    /// Returns random funny/humorous text for various game events.
    /// The soul of the game's personality.
    /// </summary>
    public static class FunnyTextSystem
    {
        // ===== BITE PROMPTS =====
        private static readonly string[] BiteTexts = {
            "有口了！这鱼是不是近视！",
            "它居然真的咬了！快快快！",
            "鱼：我只是路过...诶？！",
            "上钩了！这条鱼今天出门没看黄历！",
            "叮！您的外卖已接单~",
            "鱼讯来了！准备收网！",
        };

        // ===== HOOK SUCCESS =====
        private static readonly string[] HookSuccessTexts = {
            "中鱼！稳住别慌！",
            "挂住了！别让它跑！",
            "成功刺鱼！它开始挣扎了！",
            "哈！上当了吧小鱼仔！",
        };

        // ===== CATCH SUCCESS =====
        private static readonly string[] CatchCommonTexts = {
            "又是一条小杂鱼...聊胜于无吧",
            "这条鱼看起来比你还不想来",
            "它用无辜的眼神看着你...",
            "一条普通的鱼。但在你手里，它就是MVP！",
            "这鱼的表情仿佛在说：为什么是我？",
        };

        private static readonly string[] CatchUncommonTexts = {
            "哦？这条有点东西啊！",
            "不错不错！值得发个朋友圈！",
            "这条鱼明显不太服气",
            "中等难度的对手！你赢了这一局！",
        };

        private static readonly string[] CatchRareTexts = {
            "天呐！老板亲自下场了！",
            "恭喜！转发这条锦鲤保你今年顺利！",
            "稀有鱼种！建议裱起来挂墙上！",
            "这条鱼的价格足够你吃一顿好的了！",
            "传说中的大佬！你的运气今天爆棚！",
        };

        // ===== CATCH FAIL =====
        private static readonly string[] LineBreakTexts = {
            "断线了...你的线和你的心一起碎了",
            "线断了！这鱼力气也太大了吧！",
            "鱼：切线成功！告辞！",
            "装备不行怪装备（真的）",
            "你需要一根更粗的线...和更强的心脏",
        };

        private static readonly string[] UnhookTexts = {
            "它挥着尾巴跟你说拜拜了~",
            "脱钩了！这鱼嘴上功夫了得！",
            "鱼：下次记得拿稳点哦~",
            "跑了...但你们之间有过美好的回忆",
            "它自由了！而你只得到了寂寞",
        };

        // ===== WAITING =====
        private static readonly string[] WaitingTexts = {
            "等待中...享受宁静吧",
            "鱼呢？鱼都去哪了？",
            "也许它们在开会讨论要不要咬钩",
            "耐心是钓鱼佬最大的美德",
            "这个时候适合思考人生...",
            "听说鱼也会午休？",
        };

        // ===== IDLE / READY =====
        private static readonly string[] IdleTexts = {
            "按住屏幕开始蓄力！",
            "今天的鱼运如何？来一竿试试！",
            "准备好了吗？鱼等不及了！",
            "深呼吸...然后甩竿！",
        };

        // ===== PUBLIC API =====

        public static string GetBiteText() => RandomPick(BiteTexts);
        public static string GetHookSuccessText() => RandomPick(HookSuccessTexts);
        public static string GetWaitingText() => RandomPick(WaitingTexts);
        public static string GetIdleText() => RandomPick(IdleTexts);

        public static string GetCatchText(FishRarity rarity)
        {
            return rarity switch
            {
                FishRarity.Common => RandomPick(CatchCommonTexts),
                FishRarity.Uncommon => RandomPick(CatchUncommonTexts),
                FishRarity.Rare => RandomPick(CatchRareTexts),
                _ => RandomPick(CatchCommonTexts)
            };
        }

        public static string GetFailText(string reason)
        {
            return reason == "line_break"
                ? RandomPick(LineBreakTexts)
                : RandomPick(UnhookTexts);
        }

        private static string RandomPick(string[] pool)
        {
            return pool[Random.Range(0, pool.Length)];
        }
    }
}
