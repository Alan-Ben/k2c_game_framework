using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场名人榜列表item
    /// </summary>
    public class GGUIWndArenaSubCelebrityGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoArenaSubCelebrityGridItem>
    {
        //名人榜信息
        private ArenaCelebrityInfo _m_celebrityInfo;
        //玩家头像
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        //显示序列
        private long _m_lShowSerialize;

        public GGUIWndArenaSubCelebrityGridItem(GGUIMonoArenaSubCelebrityGridItem  _wnd)
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
            if (null == wnd)
                return;

            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnAttack, _onClickAttack);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (wnd.monoPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);

            ALUGUICommon.combineBtnClick(wnd.btnAttack, _onClickAttack);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(ArenaCelebrityInfo _info)
        {
            _m_celebrityInfo = _info;
            _refreshWnd();
        }

        /// <summary>
        /// 模拟点击攻击
        /// </summary>
        public void simulateClickAttack()
        {
            _onClickAttack(null);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshCelebrityInfo();
            _refreshPlayerInfo();
        }

        //刷新名人榜信息
        private void _refreshCelebrityInfo()
        {
            if (wnd == null || _m_celebrityInfo == null)
                return;

            //是否是自己
            bool isSelf = _m_celebrityInfo.attackerCid == NPPlayer.instance.playerInfo.CID;
            ALUGUICommon.setGameObjEnable(wnd.goSelfShowList, isSelf);
            ALUGUICommon.setGameObjEnable(wnd.goSelfHideList, !isSelf);

            //是否是指定谈判
            bool isSelectAttack = _m_celebrityInfo.isSelectAttack;
            if (isSelectAttack)
                ALUGUICommon.setLabelTxt(wnd.txtFightType, TextTranslate.instance.getLanguage(TransKeyConst.arena_selectTypeBattle_none));
            else
                ALUGUICommon.setLabelTxt(wnd.txtFightType, TextTranslate.instance.getLanguage(TransKeyConst.arena_randomTypeBattle_none));

            //对手名称
            ALUGUICommon.setLabelTxt(wnd.txtFightName, _m_celebrityInfo.defenderName);

            //击败伙伴数
            ALUGUICommon.setLabelTxt(wnd.txtDefeatCount, _m_celebrityInfo.defeatHeroNum);

            //时间
            ALUGUICommon.setLabelTxt(wnd.txtTime,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_reportPassTime_str,
                    TimeUtil.getPassTimeShow(_m_celebrityInfo.timeMs)));
        }

        //刷新玩家信息
        private void _refreshPlayerInfo()
        {
            if (wnd == null || _m_celebrityInfo == null)
                return;

            long serialize = _m_lShowSerialize;
            _m_celebrityInfo.getPlayerInfo(_playerInfo =>
            {
                if (_m_lShowSerialize != serialize || null == _playerInfo || wnd == null || !isShow)
                    return;

                _m_wPlayerIcon?.showWnd();
                _m_wPlayerIcon?.setPlayerInfo(_playerInfo);
            });
        }

        //点击攻击
        private void _onClickAttack(GameObject _go)
        {
            if (_m_celebrityInfo == null)
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
                //设置指定谈判数据
                GGUIWndArenaBattlePrepare.instance.setSelectAttackInfo(_m_celebrityInfo.attackerCid);
            }));

            //记录这次选择的人
            if (!_m_celebrityInfo.isBot)
            {
                NPPlayer.instance.arenaComp.lastSelectCelebrityCid = _m_celebrityInfo.attackerCid;
                _m_celebrityInfo.getPlayerInfo(_info =>
                {
                    NPPlayer.instance.arenaComp.lastSelectCelebrityName = _info?.name;
                });
            }
        }
    }
}
