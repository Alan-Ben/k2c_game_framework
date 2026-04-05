// using System.Collections.Generic;
// using Common.DungeonObj;
// using CommonEnum;
//
// namespace GOE
// {
//     /// <summary>
//     /// 午间副本_出战大臣信息
//     /// </summary>
//     public class MiddayDungeonFightHero
//     {
//         /// <summary>
//         /// 大臣ID
//         /// </summary>
//         private long heroId;
//
//         /// <summary>
//         /// 出战次数
//         /// </summary>
//         private short num;
//         
//         public MiddayDungeonFightHero(MiddayDungeon_FightHero _info)
//         {
//             heroId = _info.getHeroId();
//             num = _info.getNum();
//         }
//     }
//
//     /// <summary>
//     /// 午间副本_借用出战大臣信息
//     /// </summary>
//     public class MiddayDungeonBorrowHero
//     {
//         /// <summary>
//         /// 玩家ID
//         /// </summary>
//         private long cid;
//
//         /// <summary>
//         /// 大臣ID
//         /// </summary>
//         private long heroId;
//         
//         public MiddayDungeonBorrowHero(MiddayDungeon_BorrowHero _info)
//         {
//             cid = _info.getCid();
//             heroId = _info.getHeroId();
//         }
//     }
//
//     public class MiddayDungeonHeroInfo
//     {
//         /// <summary>
//         /// 已战斗过的英雄列表
//         /// </summary>
//         private List<MiddayDungeonFightHero> selfHeroList = new List<MiddayDungeonFightHero>();
//         /// <summary>
//         /// 借用出战大臣列表
//         /// </summary>
//         private List<MiddayDungeonBorrowHero> borrowHeroList = new List<MiddayDungeonBorrowHero>();
//         
//         public void update(MiddayDungeon_HeroUseInfo _bossInfo)
//         {
//             selfHeroList.Clear();
//             foreach (var selfHero in _bossInfo.getSelfHeroList())
//             {
//                 selfHeroList.Add(new MiddayDungeonFightHero(selfHero));
//             }
//             borrowHeroList.Clear();
//             foreach (var borrowHero in _bossInfo.getBorrowHeroList())
//             {
//                 borrowHeroList.Add(new MiddayDungeonBorrowHero(borrowHero));
//             }
//         }
//
//         public long getSelfMatchHero(long _bossPower)
//         {
//             
//
//             List<HeroInfo> heroList = new List<HeroInfo>();
//             NPPlayer.instance.heroComponent.getAllList(heroList);
//             foreach (var heroInfo in heroList)
//             {
//                 long battleExtraTimes = NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.MIDDAY_DUNGEON_HERO_ATTACK_EXTRA_TIMES, toJudgeUnionBonus(heroInfo));
//
//
//             }
//         }
//         /// <summary>
//         /// 获取JudgeUnionBonus
//         /// </summary>
//         /// <returns></returns>
//         public JudgeUnionBonus toJudgeUnionBonus(HeroInfo _heroInfo)
//         {
//             if (_heroInfo == null)
//                 return null;
//             JudgeUnionBonusPart judgePart = new JudgeUnionBonusPart();
//             judgePart.filterType = EBonusFilterType.HERO_ID;
//             judgePart.id = _heroInfo.id;
//             JudgeUnionBonus judgeUnionBonus = new JudgeUnionBonus(judgePart);
//             return judgeUnionBonus;
//         }
//     }
// }