using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场名人榜附加窗口
    /// </summary>
    public class GGUIWndArenaSubCelebrity : _ATALBasicUISubWnd<GGUIMonoArenaSubCelebrity>
    {
        //点击关闭事件
        private Action _m_aOnClickClose;
        //名人榜列表
        private GGUIWndArenaSubCelebrityGrid _m_wCelebrityGrid;
        //显示序列号
        private long _m_lShowSerialize;

        public GGUIWndArenaSubCelebrity(GGUIMonoArenaSubCelebrity _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_ARENA_CELEBRITY_ATTACK_BY_INDEX, _simulateClickAttack);
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_ARENA_CELEBRITY_ATTACK_BY_INDEX, _simulateClickAttack);
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wCelebrityGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCelebrityGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wCelebrityGrid?.discard();
            _m_wCelebrityGrid = null;

            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnFightAgain, _onClickFightAgain);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (wnd.monoSubCelebrityGrid != null)
                _m_wCelebrityGrid = new GGUIWndArenaSubCelebrityGrid(wnd.monoSubCelebrityGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnFightAgain, _onClickFightAgain);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_onClickClose"></param>
        public  void setInfo(Action _onClickClose)
        {
            _m_aOnClickClose = _onClickClose;
        }

        //刷新显示
        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            //先隐藏列表
            _m_wCelebrityGrid?.hideWnd();
            //请求列表数据
            long serialize = _m_lShowSerialize;
            NPPlayer.instance.arenaComp.reqCelebrityRank(0, _infoList =>
            {
                if (serialize != _m_lShowSerialize || wnd == null || !isShow)
                    return;

                _m_wCelebrityGrid?.showWnd();
                _m_wCelebrityGrid?.setInfo(_infoList);
            });

            //上榜描述
            ALUGUICommon.setLabelTxt(wnd.txtConditionDesc,
                TextTranslate.instance.getLanguage(TransKeyConst.arena_getOnTheCelebrityListCondition_num,
                    GRefdataCoreMgr.instance.npGeneral.arena_celebrity_rank_up_need_defeat_hero));

            //上次选择描述
            long lastSelectCid = NPPlayer.instance.arenaComp.lastSelectCelebrityCid;
            string lastSelectName = NPPlayer.instance.arenaComp.lastSelectCelebrityName;
            if (lastSelectCid > 0 && !string.IsNullOrEmpty(lastSelectName))
            {
                //上次谈判：{0}
                ALUGUICommon.setLabelTxt(wnd.txtLastSelect,
                    TextTranslate.instance.getLanguage(TransKeyConst.arena_lastSelectCelebrityName_str,
                        lastSelectName));

                ALUGUICommon.setGameObjEnable(wnd.goCanFightAgainHideList, false);
                ALUGUICommon.setGameObjEnable(wnd.goCanFightAgainShowList, true);
            }
            else
            {
                //上次谈判：暂无
                ALUGUICommon.setLabelTxt(wnd.txtLastSelect,
                    TextTranslate.instance.getLanguage(TransKeyConst.arena_lastSelectCelebrityNone_none));

                ALUGUICommon.setGameObjEnable(wnd.goCanFightAgainHideList, true);
                ALUGUICommon.setGameObjEnable(wnd.goCanFightAgainShowList, false);
            }
        }

        //点击关闭
        private void _onClickClose(GameObject _obj)
        {
            _m_aOnClickClose?.Invoke();
        }

        //点击再次挑战
        private void _onClickFightAgain(GameObject _obj)
        {
            ArenaInfo arenaInfo = NPPlayer.instance.arenaComp.arenaInfo;
            if (arenaInfo == null || arenaInfo.hadSelectAttackNum >= GRefdataCoreMgr.instance.npGeneral.arena_select_attack_daily_limit)
            {
                //指定谈判已达上限
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.arena_selectAttackCountReachLimit_none);
                return;
            }

            long cid = NPPlayer.instance.arenaComp.lastSelectCelebrityCid;
            if (cid <= 0)
                return;

            //打开谈判准备界面
            QueueMgr.instance.AddNode(new GMainQueueArenaBattlePrepareNode(() =>
            {
                //设置指定谈判数据
                GGUIWndArenaBattlePrepare.instance.setSelectAttackInfo(cid);
            }));
        }

        //模拟点击攻击
        private void _simulateClickAttack(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] == null)
                return;
            long index = (long) _objects[0];
            if (index < 0)
                return;
            _m_wCelebrityGrid?.simulateClickAttack((int) index);
        }
    }
}
