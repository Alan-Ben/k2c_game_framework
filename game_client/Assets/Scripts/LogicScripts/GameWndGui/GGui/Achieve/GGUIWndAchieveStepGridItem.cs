using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 成就步骤信息item
    /// </summary>
    public class GGUIWndAchieveStepGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoAchieveStepGridItem>
    {
        private AchieveStepInfo _m_achieveStepInfo;//成就步骤信息
        private NPGGUIWndCommonMaskItemContainer _m_rewardItemContainer;//成就奖励列表

        public GGUIWndAchieveStepGridItem(GGUIMonoAchieveStepGridItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (null != _m_rewardItemContainer)
                _m_rewardItemContainer.discard();
            _m_rewardItemContainer = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _clickBtnGo);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (null != wnd.rewardItemContainer)
                _m_rewardItemContainer = new NPGGUIWndCommonMaskItemContainer(wnd.rewardItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _clickBtnGo);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(AchieveStepInfo _info)
        {
            _m_achieveStepInfo = _info;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_achieveStepInfo == null || _m_achieveStepInfo.stepRefObj == null || _m_achieveStepInfo.achieveInfo == null)
                return;

            ENPCommonGetStat state = _m_achieveStepInfo.getRewardState();
            //刷新信息展示
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(TransKeyConst.common_strDotStr, _m_achieveStepInfo.step, _m_achieveStepInfo.stepRefObj.getName));
            if (_m_rewardItemContainer != null)
            {
                _m_rewardItemContainer.showWnd();
                _m_rewardItemContainer.showItemList(_m_achieveStepInfo.stepRefObj.done_item_list, false, state == ENPCommonGetStat.HAS_GET);
            }

            //设置置灰状态
            if (state == ENPCommonGetStat.HAS_GET)
                GGameCommonInfo.grayImage(wnd.finishGrayList);
            else
                GGameCommonInfo.disgrayImage(wnd.finishGrayList);

            //状态显影
            EAchieveStepGetStat stepState = _getStepStat(state);
            GGUIMonoAchieveStepGridItemState curStatInfo = null;
            for (int i = 0; i < wnd.stateList.Count; i++)
            {
                curStatInfo = wnd.stateList[i];
                if (null == curStatInfo)
                    continue;

                if (curStatInfo.state == stepState)
                    break;
            }

            if (null != curStatInfo)
            {
                ALUGUICommon.setGameObjEnable(curStatInfo.goListShow, true);
                ALUGUICommon.setGameObjEnable(curStatInfo.goListHide, false);
                string valueStr = _m_achieveStepInfo.achieveInfo.getAchieveProgressStr(_m_achieveStepInfo.step, curStatInfo.txtColor, wnd.curProcessTextSize, curStatInfo.isChgAll);
                ALUGUICommon.setLabelTxt(wnd.txtProcess, valueStr);
            }
        }

        private EAchieveStepGetStat _getStepStat(ENPCommonGetStat _stat)
        {
            switch (_stat)
            {
                case ENPCommonGetStat.CAN_GET:
                    return EAchieveStepGetStat.CAN_GET;
                case ENPCommonGetStat.HAS_GET:
                    return EAchieveStepGetStat.HAS_GET;
                case ENPCommonGetStat.CAN_NOT_GET:
                    {
                        if (_m_achieveStepInfo.step > _m_achieveStepInfo.achieveInfo.step)
                            return EAchieveStepGetStat.CAN_NOT_GET;
                        else
                            return EAchieveStepGetStat.DOING_CAN_NOT_GET;
                    }
                default:
                    return EAchieveStepGetStat.NONE;
            }
        }
        #region 点击事件

        //点击领取奖励
        private void _onClickGetReward(GameObject _go)
        {
            if (wnd == null || _m_achieveStepInfo == null)
                return;

            if (_m_achieveStepInfo.getRewardState() != ENPCommonGetStat.CAN_GET)
                return;

            NPPlayer.instance.achieveComp.reqDoneAchieveStep(_m_achieveStepInfo.achieveId, _m_achieveStepInfo.step, null);
        }
        
        
        private void _clickBtnGo(GameObject obj)
        {
            if (null == _m_achieveStepInfo || _m_achieveStepInfo.achieveInfo == null || _m_achieveStepInfo.achieveInfo.achieveRefObj == null || _m_achieveStepInfo.achieveInfo.achieveRefObj.go_to == null)
                return;
                
            _m_achieveStepInfo.achieveInfo.achieveRefObj.go_to.dealEffect();
        }

        #endregion
    }
}

