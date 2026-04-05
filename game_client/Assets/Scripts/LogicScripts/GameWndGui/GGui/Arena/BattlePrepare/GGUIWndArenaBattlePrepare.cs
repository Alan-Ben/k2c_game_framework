using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场战斗准备主界面
    /// </summary>
    public class GGUIWndArenaBattlePrepare : _ANPGGUIBasicResBarWnd<GGUIMonoArenaBattlePrepare>
    {
        private static GGUIWndArenaBattlePrepare _g_instance;
        public static GGUIWndArenaBattlePrepare instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndArenaBattlePrepare();
                return _g_instance;
            }
        }

        //谈判类型
        private EArenaSelectBattleType _m_eBattleType;
        //对手cid
        private long _m_lOpponentCid;
        //数据库id
        private long _m_lDbId;
        //对手信息附加窗口
        private GGUIWndArenaBattleSubOpponentInfo _m_wSubOpponentInfo;
        //自己信息附加窗口
        private GGUIWndArenaBattleSubSelfInfo _m_wSubSelfInfo;

        public GGUIWndArenaBattlePrepare() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaBattlePrepare.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaBattlePrepare.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        
        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_ARENA_OPEN_SELECT_HERO, _onSimulateClickOpenSelectHero);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_ARENA_OPEN_SELECT_HERO, _onSimulateClickOpenSelectHero);
            _m_wSubOpponentInfo?.hideWnd();
            _m_wSubSelfInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wSubOpponentInfo?.resetWnd();
            _m_wSubSelfInfo?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wSubOpponentInfo?.discard();
            _m_wSubOpponentInfo = null;

            _m_wSubSelfInfo?.discard();
            _m_wSubSelfInfo = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);//点击关闭
            ALUGUICommon.uncombineBtnClick(wnd.btnRank, _onClickRank);//点击排行榜
            ALUGUICommon.uncombineBtnClick(wnd.btnSetting, _onClickSetting);//点击设置
            ALUGUICommon.uncombineBtnClick(wnd.btnFight, _onClickFight);//点击谈判
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoOpponentInfo != null)
                _m_wSubOpponentInfo = new GGUIWndArenaBattleSubOpponentInfo(wnd.monoOpponentInfo);

            if (wnd.monoSelfInfo != null)
                _m_wSubSelfInfo = new GGUIWndArenaBattleSubSelfInfo(wnd.monoSelfInfo);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);//点击关闭
            ALUGUICommon.combineBtnClick(wnd.btnRank, _onClickRank);//点击排行榜
            ALUGUICommon.combineBtnClick(wnd.btnSetting, _onClickSetting);//点击设置
            ALUGUICommon.combineBtnClick(wnd.btnFight, _onClickFight);//点击谈判
        }

        /// <summary>
        /// 设置随机谈判信息
        /// </summary>
        public void setRandomAttackInfo()
        {
            ArenaBattleInfo battleInfo = NPPlayer.instance.arenaComp.arenaBattleInfo;
            if (battleInfo == null)
                return;

            _m_eBattleType = EArenaSelectBattleType.RANDOM;
            _m_lOpponentCid = battleInfo.opponentCid;
            _m_lDbId = 0;
            _refreshWnd();
        }

        /// <summary>
        /// 设置指定谈判信息
        /// </summary>
        /// <param name="_opponentCid"></param>
        public void setSelectAttackInfo(long _opponentCid)
        {
            _m_eBattleType = EArenaSelectBattleType.SELECT;
            _m_lOpponentCid = _opponentCid;
            _m_lDbId = 0;
            _refreshWnd();
        }

        /// <summary>
        /// 设置反击谈判信息
        /// </summary>
        /// <param name="_opponentCid"></param>
        public void setFightBackAttackInfo(long _opponentCid,long _fightBackDbId)
        {
            _m_eBattleType = EArenaSelectBattleType.SELECT;
            _m_lOpponentCid = _opponentCid;
            _m_lDbId = _fightBackDbId;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            //刷新对手信息
            _m_wSubOpponentInfo?.showWnd();
            _m_wSubOpponentInfo?.setCid(_m_lOpponentCid, _info =>
            {
                //如果有固定对方的实力，设置实力值
                ArenaBattleInfo battleInfo = NPPlayer.instance.arenaComp.arenaBattleInfo;
                if(battleInfo != null && battleInfo.opponentPower > 0)
                    _m_wSubOpponentInfo?.setPower(battleInfo.opponentPower);
            });

            //自己信息
            _m_wSubSelfInfo?.showWnd();
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            //如果在战斗中，强制关闭主界面
            if (NPPlayer.instance.arenaComp.isInBattle())
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_MAIN);

            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_PREPARE);
        }

        //点击排行榜
        private void _onClickRank(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaRank.instance, GGUIWndArenaRank.instance.showWnd, UINodeTagConst.C_ARENA_RANK);
        }

        //点击设置
        private void _onClickSetting(GameObject _go)
        {
            if (!GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.arena_convenient_setting_simple_unlock_id, true))
                return;

            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndArenaConvenientSetting.instance, GGUIWndArenaConvenientSetting.instance.showWnd, UINodeTagConst.C_ARENA_CONVENTENT_SETTING);
        }

        //点击谈判
        private void _onClickFight(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndArenaBattleSelectHero.instance, UINodeTagConst.C_ARENA_SELECT_HERO, null, () =>
            {
                GGUIWndArenaBattleSelectHero.instance.showWnd();
                GGUIWndArenaBattleSelectHero.instance.setInfo(_m_eBattleType, _m_lOpponentCid, _m_lDbId);
            }, 0);
        }

        #endregion

        #region 消息事件

        //模拟点击打开选择伙伴
        private void _onSimulateClickOpenSelectHero()
        {
            _onClickFight(null);
        }

        #endregion
    }
}