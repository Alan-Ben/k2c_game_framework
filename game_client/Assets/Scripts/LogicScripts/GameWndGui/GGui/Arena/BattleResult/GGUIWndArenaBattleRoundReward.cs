using System;
using System.Collections.Generic;
using ALPackage;
using Common.ArenaObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 竞技场回合连胜奖励弹窗
    /// </summary>
    public class GGUIWndArenaBattleRoundReward : _ANPGGUIBasicWnd<GGUIMonoArenaBattleRoundReward>
    {
        private static GGUIWndArenaBattleRoundReward _g_instance;
        public static GGUIWndArenaBattleRoundReward instance
        {
            get
            {
                if (_g_instance == null)
                    _g_instance = new GGUIWndArenaBattleRoundReward();
                return _g_instance;
            }
        }

        //奖励宝箱列表
        private List<GGUIWndArenaBattleRoundRewardBox> _m_boxList = new List<GGUIWndArenaBattleRoundRewardBox>();
        //回合连胜奖励数据
        private Arena_RoundReward _m_roundResult;
        //是否已经点击宝箱
        private bool _m_bHadClickBox;
        //关闭回调
        private Action _m_aOnClose;

        public GGUIWndArenaBattleRoundReward() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoArenaBattleRoundReward.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoArenaBattleRoundReward.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if (_m_boxList != null)
            {
                for (int i = 0; i < _m_boxList.Count; i++)
                {
                    _m_boxList[i]?.hideWnd();
                }
            }

            _m_bHadClickBox = false;

            _m_aOnClose?.Invoke();
            _m_aOnClose = null;
        }

        protected override void _onReset()
        {
            if (_m_boxList != null)
            {
                for (int i = 0; i < _m_boxList.Count; i++)
                {
                    _m_boxList[i]?.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            if (_m_boxList != null)
            {
                for (int i = 0; i < _m_boxList.Count; i++)
                {
                    _m_boxList[i]?.discard();
                }
                _m_boxList.Clear();
                _m_boxList = null;
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_boxList = new List<GGUIWndArenaBattleRoundRewardBox>();
            if (wnd.monoBoxList != null)
            {
                for (int i = 0; i < wnd.monoBoxList.Count; i++)
                {
                    GGUIWndArenaBattleRoundRewardBox rewardBox = new GGUIWndArenaBattleRoundRewardBox(wnd.monoBoxList[i]);
                    _m_boxList.Add(rewardBox);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_roundReward"></param>
        /// <param name="_onClose"></param>
        public void setInfo(Arena_RoundReward _roundReward, Action _onClose)
        {
            _m_roundResult = _roundReward;
            _m_aOnClose = _onClose;
            _m_bHadClickBox = false;
            _refreshState();
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_roundResult == null || _m_roundResult.getGainItem() == null || _m_roundResult.getGainItem().Count == 0)
                return;

            if (_m_boxList != null)
            {
                for (int i = 0; i < _m_boxList.Count; i++)
                {
                    _m_boxList[i]?.showWnd();
                    _m_boxList[i]?.setInfo(_m_roundResult.getGainItem()[0], () =>
                    {
                        _m_bHadClickBox = true;
                        _refreshState();
                    }, () => _m_bHadClickBox);
                }

                //如果设置了自动领取奖励，则随机打开一个宝箱
                if (AccountSettingMgr.instance.accountSetting.arenaSetAutoGetRoundReward)
                {
                    GGUIWndArenaBattleRoundRewardBox item = _m_boxList.GetRandomItem();
                    item?.setOpenBox();
                    _m_bHadClickBox = true;
                    _refreshState();
                }
            }
        }

        //刷新显隐状态
        private void _refreshState()
        {
            if (wnd == null)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, _m_bHadClickBox);
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !_m_bHadClickBox);
        }

        //点击关闭
        private void _onClickClose(GameObject _obj)
        {
            //如果没有点击宝箱，则不处理关闭
            if (!_m_bHadClickBox)
                return;

            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ARENA_BATTLE_ROUND_REWARD);
        }
    }
}