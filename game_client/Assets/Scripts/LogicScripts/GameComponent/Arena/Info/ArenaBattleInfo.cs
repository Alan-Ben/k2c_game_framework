using System.Collections.Generic;
using Common.ArenaObj;
using Common.HeroObj;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗信息
    /// </summary>
    public class ArenaBattleInfo
    {
        //对手CID
        private long _m_lOpponentCid;
        //已击败对手数量
        private int _m_iHadDefeatNum;
        //对手大臣数量
        private int _m_iOpponentHeroNum;
        //对手战力
        private long _m_lOpponentPower;
        //本回合可攻击英雄列表
        private List<Hero_ArenaShowInfo> _m_lCanAttackHeroList;
        //本轮是否已购买buff
        private bool _m_bHadBuyBuff;
        //临时增益列表
        private List<Arena_SingleBuffInfo> _m_lBuffList;
        //我方大臣id
        private long _m_lHeroId;
        //我方基础实力
        private long _m_lBasePower;
        //我方被扣除血量
        private long _m_lDeductedHp;
        //对手是否是机器人
        private bool _m_bIsBot;
        //机器人名称
        private string _m_sBotName;

        /// <summary>
        /// 对手CID
        /// </summary>
        public long opponentCid => _m_lOpponentCid;
        /// <summary>
        /// 已击败对手数量
        /// </summary>
        public int hadDefeatNum => _m_iHadDefeatNum;
        /// <summary>
        /// 对手大臣数量
        /// </summary>
        public int opponentHeroNum => _m_iOpponentHeroNum;
        /// <summary>
        /// 对手剩余伙伴数量
        /// </summary>
        public int opponentLeftHeroNum => _m_iOpponentHeroNum - _m_iHadDefeatNum;
        /// <summary>
        /// 对手战力
        /// </summary>
        public long opponentPower => _m_lOpponentPower;
        /// <summary>
        /// 本回合可攻击英雄列表
        /// </summary>
        public List<Hero_ArenaShowInfo> canAttackHeroList => _m_lCanAttackHeroList;
        /// <summary>
        /// 本轮是否已购买buff
        /// </summary>
        public bool hadBuyBuff => _m_bHadBuyBuff;
        /// <summary>
        /// 临时增益列表
        /// </summary>
        public List<Arena_SingleBuffInfo> buffList => _m_lBuffList;
        /// <summary>
        /// 我方大臣id
        /// </summary>
        public long heroId => _m_lHeroId;
        /// <summary>
        /// 我方基础战力
        /// </summary>
        public long basePower => _m_lBasePower;
        /// <summary>
        /// 我方被扣除血量
        /// </summary>
        public long deductedHp => _m_lDeductedHp;
        /// <summary>
        /// 对手是否是机器人
        /// </summary>
        public bool opponentIsBot => _m_bIsBot;
        /// <summary>
        /// 机器人名称
        /// </summary>
        public string botName => _m_sBotName;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_battleInfo"></param>
        public ArenaBattleInfo(Arena_BattleInfo _battleInfo)
        {
            updateInfo(_battleInfo);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_battleInfo"></param>
        public void updateInfo(Arena_BattleInfo _battleInfo)
        {
            if(_battleInfo == null)
                return;

            _m_lOpponentCid = _battleInfo.getOpponentCid();
            _m_iHadDefeatNum = _battleInfo.getHadDefeatNum();
            _m_iOpponentHeroNum = _battleInfo.getOpponentHeroNum();
            _m_lOpponentPower = _battleInfo.getOpponentPower();
            _m_lCanAttackHeroList = _battleInfo.getCanAttackHeroList();
            _m_bHadBuyBuff = _battleInfo.getHadBuyBuff();
            _m_lBuffList = _battleInfo.getBuffList();
            _m_lHeroId = _battleInfo.getHeroId();
            _m_lBasePower = _battleInfo.getBasePower();
            _m_lDeductedHp = _battleInfo.getDeductedHp();
            _m_bIsBot = _battleInfo.getIsNpc();
            _m_sBotName = _battleInfo.getNpcName();
        }

        /// <summary>
        /// 获取是否是第一回合
        /// </summary>
        /// <returns></returns>
        public bool getIsFirstBattleRound()
        {
            return _m_iHadDefeatNum == 0;
        }

        /// <summary>
        /// 获取总增益万分比
        /// </summary>
        /// <returns></returns>
        public long getTotalBuffAddPer()
        {
            long totalAddPer = 0;
            if (_m_lBuffList == null)
                return totalAddPer;

            for (int i = 0; i < _m_lBuffList.Count; i++)
            {
                if(_m_lBuffList[i] == null)
                    continue;

                ArenaBuffRefObj buffRef = GRefdataCoreMgr.instance.arenaBuffRefCore.getRef(_m_lBuffList[i].getBuffId());
                if(buffRef == null)
                    continue;

                totalAddPer += buffRef.value * _m_lBuffList[i].getNum();
            }

            return totalAddPer;
        }

        /// <summary>
        /// 获取总实力（血量）
        /// </summary>
        /// <returns></returns>
        public long getTotalPower()
        {
            return HeroCommon.calArenaPower(_m_lBasePower, getTotalBuffAddPer());
        }

        /// <summary>
        /// 获取当前剩余实力（血量）
        /// </summary>
        /// <returns></returns>
        public long getCurLeftPower()
        {
            return getTotalPower() - _m_lDeductedHp;
        }
    }
}
