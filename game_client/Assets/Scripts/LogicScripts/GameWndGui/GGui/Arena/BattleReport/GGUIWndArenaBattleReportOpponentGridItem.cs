using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场战报对手列表item
    /// </summary>
    public class GGUIWndArenaBattleReportOpponentGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoArenaBattleReportOpponentGridItem>
    {
        //战报信息
        private ArenaFightBackInfo _m_reportInfo;
        //玩家头像
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        //显示序列
        private long _m_lShowSerialize;

        public GGUIWndArenaBattleReportOpponentGridItem(GGUIMonoArenaBattleReportOpponentGridItem  _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wPlayerIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayerIcon?.resetWnd();
        }

        protected override void _resetGridItem()
        {
            _m_wPlayerIcon?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;

            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnFight, _onClickFight);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (wnd.monoPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);

            ALUGUICommon.combineBtnClick(wnd.btnFight, _onClickFight);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(ArenaFightBackInfo _info)
        {
            _m_reportInfo = _info;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshPlayerInfo();
            _refreshReportInfo();
        }

        //刷新玩家信息
        private void _refreshPlayerInfo()
        {
            if (wnd == null || _m_reportInfo == null)
                return;

            long serialize = _m_lShowSerialize;
            _m_reportInfo.getPlayerInfo(_playerInfo =>
            {
                if (_m_lShowSerialize != serialize || null == _playerInfo || wnd == null || !isShow)
                    return;

                _m_wPlayerIcon?.showWnd();
                _m_wPlayerIcon?.setPlayerInfo(_playerInfo);
            });
        }

        //刷新战报信息
        private void _refreshReportInfo()
        {
            if (wnd == null || _m_reportInfo == null)
                return;

            //击败我方伙伴描述
            ALUGUICommon.setLabelTxt(wnd.txtDefeatHeroCount,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_defeatMyHeroCount_num,
                    _m_reportInfo.defeatHeroNum));

            //我方影响力变化描述
            ALUGUICommon.setLabelTxt(wnd.txtInfluenceChg,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_myInfluenceChg_num,
                    TextTranslate.instance.getLanguage(TransKeyConst.common_reduced_num, _m_reportInfo.deductinfluence)));

            //时间
            ALUGUICommon.setLabelTxt(wnd.txtTime,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_reportPassTime_str,
                    TimeUtil.getPassTimeShow(_m_reportInfo.timestamp)));

            //是否已经反击
            ALUGUICommon.setGameObjEnable(wnd.goFightBackShowList, _m_reportInfo.hadFightBack);
            ALUGUICommon.setGameObjEnable(wnd.goFightBackHideList, !_m_reportInfo.hadFightBack);
        }

        //点击反击
        private void _onClickFight(GameObject obj)
        {
            //已经反击不处理
            if (_m_reportInfo == null || _m_reportInfo.hadFightBack)
                return;

            ArenaInfo arenaInfo = NPPlayer.instance.arenaComp.arenaInfo;
            if (arenaInfo == null || arenaInfo.hadSelectAttackNum >= GRefdataCoreMgr.instance.npGeneral.arena_select_attack_daily_limit)
            {
                //指定谈判已达上限
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.arena_selectAttackCountReachLimit_none);
                return;
            }

            //打开谈判准备界面
            QueueMgr.instance.AddNode(new GMainQueueArenaBattlePrepareNode(() =>
            {
                //设置随机谈判数据
                GGUIWndArenaBattlePrepare.instance.setFightBackAttackInfo(_m_reportInfo.opponentCid, _m_reportInfo.dbId);
            }));
        }
    }
}
