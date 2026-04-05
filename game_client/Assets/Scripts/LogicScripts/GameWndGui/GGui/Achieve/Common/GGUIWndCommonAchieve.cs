using ALPackage;
using CommonEnum;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用成就窗口
    /// </summary>
    public class GGUIWndCommonAchieve : _ANPGGUIBasicWnd<GGUIMonoCommonAchieve>
    {
        //资源id
        private long _m_lUIResId;
        //当前的成就类型
        private EAchieveType _m_eCurType;
        //成就列表
        private GGUIWndAchieveGrid _m_wAchieveGrid;
        //操作序列号
        private long _m_lOpSerialize;

        public GGUIWndCommonAchieve(EAchieveType _CurType, long _uiResId,bool _isMainWnd) : base(_isMainWnd ? EALUIWndLayer.NORMAL : EALUIWndLayer.ADDITION)
        {
            _m_eCurType = _CurType;
            _m_lUIResId = _uiResId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get => true; }
        public override bool needDiscardOnSwitch { get => true; }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_ACHIEVE_INFO_CHG, _onAchieveInfoChg);
            _m_lOpSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_ACHIEVE_INFO_CHG, _onAchieveInfoChg);
            _m_lOpSerialize = ALSerializeOpMgr.next();

            if (_m_wAchieveGrid != null)
                _m_wAchieveGrid.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wAchieveGrid != null)
                _m_wAchieveGrid.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (_m_wAchieveGrid != null)
                _m_wAchieveGrid.discard();
            _m_wAchieveGrid = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAchieveGrid != null)
            {
                _m_wAchieveGrid = new GGUIWndAchieveGrid(wnd.monoAchieveGrid);
                _m_wAchieveGrid.onClickGetReward += _onItemClickGetReward;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        //刷新窗口
        private void _refreshWnd()
        {
            List<AchieveInfo> achieveInfoList = NPPlayer.instance.achieveComp.getInfosByType(_m_eCurType);
            if (_m_wAchieveGrid != null)
            {
                _m_wAchieveGrid.showWnd();
                _m_wAchieveGrid.setShowData(achieveInfoList);
            }
        }

        #region 点击事件

        //item点击领奖
        private void _onItemClickGetReward(GGUIWndAchieveGridItem _item)
        {
            if (_item == null || _item.wnd == null || _item.achieveInfo == null || _item.achieveInfo.curStepInfo == null || _item.achieveInfo.curStepInfo.stepRefObj == null)
                return;

            if (_item.achieveInfo.curStepInfo.getRewardState() != ENPCommonGetStat.CAN_GET)
                return;

            //设置开始获取奖励
            long curSerialize = _m_lOpSerialize;
            NPPlayer.instance.achieveComp.reqDoneAchieveStep(_item.achieveInfo.curStepInfo.achieveId, _item.achieveInfo.curStepInfo.step,
                () =>
                {
                    if (!isShow || _item == null || !_item.isShow || curSerialize != _m_lOpSerialize)
                        return;

                    AchieveInfo info = NPPlayer.instance.achieveComp.getAchimentInfo(_item.achieveInfo.achieveId);
                    //播放领取特效
                    _item.playStepGetRewardSfx();
                    _item.setInfo(info);
                    _refreshWnd();
                });
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_COMMON_ACHIEVE);
        }

        #endregion

        #region 消息事件

        //成就信息变更
        private void _onAchieveInfoChg(params object[] _objects)
        {
            _refreshWnd();
        }

        #endregion
    }
}
