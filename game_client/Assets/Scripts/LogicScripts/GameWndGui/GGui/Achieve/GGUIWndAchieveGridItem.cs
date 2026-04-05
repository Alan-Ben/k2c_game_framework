using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 成就信息item
    /// </summary>
    public class GGUIWndAchieveGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoAchieveGridItem>
    {
        private AchieveInfo _m_achieveInfo;//成就信息
        private NPGGUIWndCommonMaskItemContainer _m_rewardItemContainer;//成就奖励列表
        private NPGGuiWndTexture _m_wIcon;//图标
        private Action<GGUIWndAchieveGridItem> _m_aClickGetReward;//点击领取奖励
        private List<CommonUISfxObj> _m_lSfxList;//领取特效列表

        /// <summary>
        /// 成就信息
        /// </summary>
        public AchieveInfo achieveInfo
        {
            get { return _m_achieveInfo; }
        }
        /// <summary>
        /// 点击领取奖励
        /// </summary>
        public Action<GGUIWndAchieveGridItem> onClickGetReward
        {
            get { return _m_aClickGetReward; }
            set { _m_aClickGetReward = value; }
        }

        public GGUIWndAchieveGridItem(GGUIMonoAchieveGridItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.SIMULATE_CLICK_ACHIEVE_REWARD, _simulateClickReward);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.SIMULATE_CLICK_ACHIEVE_REWARD, _simulateClickReward);
            if (_m_wIcon != null)
                _m_wIcon.hideWnd();

            if (_m_lSfxList != null)
            {
                for (int i = 0; i < _m_lSfxList.Count; i++)
                {
                    _m_lSfxList[i]?.forceDiscard();
                }
                _m_lSfxList.Clear();
                _m_lSfxList = null;
            }
        }

        protected override void _onReset()
        {
            if (_m_wIcon != null)
                _m_wIcon.discardTexture();
        }

        protected override void _resetGridItem()
        {
            if (_m_wIcon != null)
                _m_wIcon.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (null != _m_rewardItemContainer)
                _m_rewardItemContainer.discard();
            _m_rewardItemContainer = null;

            if (_m_wIcon != null)
                _m_wIcon.discard();
            _m_wIcon = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onClickDetail);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.rewardItemContainer)
                _m_rewardItemContainer = new NPGGUIWndCommonMaskItemContainer(wnd.rewardItemContainer);

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onClickDetail);
        }

        public void setInfo(AchieveInfo _info)
        {
            _m_achieveInfo = _info;
            _refreshWnd();
        }

        /// <summary>
        /// 播放领奖特效
        /// </summary>
        public void playStepGetRewardSfx()
        {
            if (wnd == null)
                return;

            if (wnd.transGetRewardSfxParent != null && wnd.getRewardSfxId > 0)
            {
                if (_m_lSfxList == null)
                    _m_lSfxList = new List<CommonUISfxObj>();

                CommonUISfxObj sfxObj = PlaySfxMgr.instance.playUISfx(wnd.getRewardSfxId, wnd.transGetRewardSfxParent);
                _m_lSfxList.Add(sfxObj);
            }
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_achieveInfo == null || _m_achieveInfo.achieveRefObj == null || _m_achieveInfo.curStepInfo == null || _m_achieveInfo.curStepInfo.stepRefObj == null)
                return;

            //刷新信息展示
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_achieveInfo.achieveRefObj.name));

            //刷新状态
            EAchievePointProgressState state = EAchievePointProgressState.CAN_NOT_GET;
            if (_m_achieveInfo.isAllDone())
                state = EAchievePointProgressState.ALL_DONE;
            else if (_m_achieveInfo.curStepInfo.getRewardState() == ENPCommonGetStat.CAN_GET)
                state = EAchievePointProgressState.CAN_GET;
            else
                state = EAchievePointProgressState.CAN_NOT_GET;
            if (wnd.stateList != null)
            {
                GGUIAchievePointProgressState curState = null;
                for (int i = 0; i < wnd.stateList.Count; i++)
                {
                    if (wnd.stateList[i] != null && wnd.stateList[i].stete == state)
                    {
                        curState = wnd.stateList[i];
                        break;
                    }
                }

                if (curState != null)
                {
                    ALUGUICommon.setGameObjEnable(curState.goShowList, true);
                    ALUGUICommon.setGameObjEnable(curState.goHideList, false);

                    //获取对应格式进度字符串
                    string curCountStr = GCommon.getValueFormatStr(_m_achieveInfo.achieveRefObj.process_num_format, _m_achieveInfo.curStepCount);
                    string targetCountStr = GCommon.getValueFormatStr(_m_achieveInfo.achieveRefObj.process_num_format, _m_achieveInfo.curStepInfo.stepRefObj.process_count);
                    //设置进度文本
                    string valueStr = null;
                    if (state == EAchievePointProgressState.CAN_GET)
                        valueStr = TextTranslate.instance.getLanguage(TransKeyConst.achieve_canGetRewardProcess_num_num, curCountStr, targetCountStr);
                    else
                        valueStr = TextTranslate.instance.getLanguage(TransKeyConst.achieve_canNotGetRewardProcess_num_num, curCountStr, targetCountStr);
                    ALUGUICommon.setLabelTxt(wnd.txtProcess, valueStr);
                    ALUGUICommon.setLabelTxt(wnd.txtCanNotGetProcess, valueStr);
                }
            }

            //刷新图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_m_achieveInfo.achieveRefObj.icon);
            }

            //刷新奖励列表
            if (_m_rewardItemContainer != null)
            {
                _m_rewardItemContainer.showWnd();
                _m_rewardItemContainer.showItemList(_m_achieveInfo.curStepInfo.stepRefObj.done_item_list, state == EAchievePointProgressState.ALL_DONE, false);
                _m_rewardItemContainer.moveToLeft();
            }
        }

        //模拟领取奖励
        private void _simulateClickReward(object[] _objs)
        {
            if (null == _objs || _objs.Length == 0)
                return;

            long index = (long)_objs[0];
            if (index != _m_iItemIdx)
                return;

            _onClickGetReward(null);
        }

        #region 点击事件

        //点击领取奖励
        private void _onClickGetReward(GameObject _go)
        {
            if (wnd == null || _m_achieveInfo == null || _m_achieveInfo.curStepInfo == null)
                return;

            if (_m_aClickGetReward != null)
                _m_aClickGetReward(this);
        }

        //点击查看详情
        private void _onClickDetail(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndAchieveStep.instance, () =>
            {
                GGUIWndAchieveStep.instance.showWnd();
                GGUIWndAchieveStep.instance.setInfo(_m_achieveInfo);
            }, UINodeTagConst.C_ADD_ACHIEVE_STEP);
        }

        #endregion
    }
}
