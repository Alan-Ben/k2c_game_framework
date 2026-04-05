using CommonEnum;

namespace GOE
{
    public class EveningDungeonHeroFightInfo : _IEveningDungeonHeroFightInfo
    {
        private HeroInfo _m_heroInfo;
        private int _m_iNowFightCount;//当前出战次数

        private JudgeUnionBonusPart[] _m_FightPowerBonusJudgeParts;//出战实力加成判断部件
        
        public EveningDungeonHeroFightInfo(HeroInfo _heroInfo)
        {
            _m_heroInfo = _heroInfo;

            updateNowFightCount();
        }

        public long heroId { get { return _m_heroInfo?.id ?? 0; } }

        public _IHeroCardShow heroCardShow { get { return _m_heroInfo; } }
        
        public long fightATK 
        {
            get
            {
                if(_m_heroInfo == null)
                    return 0;
                
                long basicATK = _m_heroInfo.power;//基础攻击力为大臣的实力
                // 获取攻击力加成万分比
                long atkAddPro = NPPlayer.instance.playerBonusMgr.getTotalPropertyBonus(EBonusPropertyType.EVENING_DUNGEON_POWER_PER, fightPowerBonusJudgeParts);
                
                // 总伤害=基础伤害*（1+伤害加成）
                return (basicATK * (1 + atkAddPro / 10000f)).ToCeilingLongValue();
            }
        }
        
        // 这边写死大臣最多可出战次数为1次
        public int fightMaxCount { get { return 1; } }
        
        public int nowFightCount { get { return _m_iNowFightCount; } }
        
        public JudgeUnionBonusPart[] fightPowerBonusJudgeParts
        {
            get
            {
                if (_m_heroInfo == null)
                    return null;
                
                return _m_FightPowerBonusJudgeParts ??= new JudgeUnionBonusPart[]
                {
                    new JudgeUnionBonusPart() { filterType = EBonusFilterType.HERO_ATTR, id = (long) _m_heroInfo.specAttrType },
                    new JudgeUnionBonusPart() { filterType = EBonusFilterType.HERO_ID, id = _m_heroInfo.id }
                };
            }
        }
        
        /// <summary>
        /// 更新当前出战次数
        /// </summary>
        /// <param name="_count"></param>
        public void updateNowFightCount(int _count)
        {
            _m_iNowFightCount = _count;
        }

        public void updateNowFightCount()
        {
            if(_m_heroInfo == null)
                return;

            _m_iNowFightCount = NPPlayer.instance.eveningDungeonComp.getHeroUsed(_m_heroInfo.id) ? fightMaxCount : 0;
        }

        /// <summary>
        /// 排序: 可出战＞不可出战 -> 实力低＞实力高 -> 大臣ID
        /// </summary>
        /// <param name="_heroFightInfo1"></param>
        /// <param name="_heroFightInfo2"></param>
        /// <returns></returns>
        public static int sort(EveningDungeonHeroFightInfo _heroFightInfo1, EveningDungeonHeroFightInfo _heroFightInfo2)
        {
            if (_heroFightInfo2 == null)
                return -1;
            if (_heroFightInfo1 == null)
                return 1;
            if (object.ReferenceEquals(_heroFightInfo1, _heroFightInfo2))
                return 0;

            bool hasFightCount1 = _heroFightInfo1.nowFightCount < _heroFightInfo1.fightMaxCount;// _heroFightInfo1是否还有可出战次数
            bool hasFightCount2 = _heroFightInfo2.nowFightCount < _heroFightInfo2.fightMaxCount;// _heroFightInfo2是否还有可出战次数
            if (hasFightCount1 != hasFightCount2)
                return -hasFightCount1.CompareTo(hasFightCount2);//有出战次数在前面, 即true在前

            long heroPower1 = _heroFightInfo1._m_heroInfo?.power ?? 0;
            long heroPower2 = _heroFightInfo2._m_heroInfo?.power ?? 0;
            if (heroPower1 != heroPower2)
                return heroPower1.CompareTo(heroPower2);

            return _heroFightInfo1.heroId.CompareTo(_heroFightInfo2.heroId);
        }
    }
}