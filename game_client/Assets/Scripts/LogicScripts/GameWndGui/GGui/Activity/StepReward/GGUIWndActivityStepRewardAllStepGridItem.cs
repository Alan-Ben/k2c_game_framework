using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndActivityStepRewardAllStepGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoActivityStepRewardAllStepGridItem>
    {
        private GActivityStepRewardRefObj _m_stepRewardRef;//阶段奖励步骤信息
        private ActivityStepRewardInfo _m_stepRewardInfo;//阶段奖励信息
        private NPGGUIWndCommonMaskItemContainer _m_rewardItemContainer;//奖励列表

        public GGUIWndActivityStepRewardAllStepGridItem(GGUIMonoActivityStepRewardAllStepGridItem _wnd) : base(_wnd)
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
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onGotoBtnClick);
                ALUGUICommon.uncombineBtnClick(wnd.btnGet, _onGetBtnClick);
            }
            
            if (null != _m_rewardItemContainer)
                _m_rewardItemContainer.discard();
            _m_rewardItemContainer = null;

        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (null != wnd.rewardItemContainer)
                _m_rewardItemContainer = new NPGGUIWndCommonMaskItemContainer(wnd.rewardItemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onGotoBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnGet, _onGetBtnClick);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        /// <param name="_stepRewardInfo"></param>
        public void setInfo(GActivityStepRewardRefObj _info, ActivityStepRewardInfo _stepRewardInfo)
        {
            _m_stepRewardRef = _info;
            _m_stepRewardInfo = _stepRewardInfo;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_stepRewardRef == null || _m_stepRewardInfo == null || _m_stepRewardInfo.stepRewardSetRef == null)
                return;

            EStepRewardState state = _m_stepRewardInfo.getStepRewardState(_m_stepRewardRef);
            //刷新信息展示
            string nameStr = TextTranslate.instance.getLanguage(_m_stepRewardRef.name, GCommon.getValueFormatStr(_m_stepRewardInfo.stepRewardSetRef.process_num_format, _m_stepRewardRef.complete_count));

            if(wnd.needShowStepOrder)
                ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(TransKeyConst.common_strDotStr, _m_stepRewardRef.step, nameStr));
            else
                ALUGUICommon.setLabelTxt(wnd.txtName, nameStr);
            
            if (_m_rewardItemContainer != null)
            {
                _m_rewardItemContainer.showWnd();
                _m_rewardItemContainer.showItemList(_m_stepRewardRef.reward_item_list, false, state == EStepRewardState.AlreadyGet);
            }

            //状态显隐
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
                GGameCommonInfo.grayImage(curStatInfo.grayList);
                GGameCommonInfo.disgrayImage(curStatInfo.disGrayList);
                string valueStr = _getValueString(curStatInfo.txtColor, curStatInfo.isChgAll);
                ALUGUICommon.setLabelTxt(wnd.txtProcess, valueStr);
            }
        }

        /// <summary>
        /// 获取进度字符串
        /// </summary>
        /// <returns></returns>
        private string _getValueString(Color _txtColor, bool _chgAllStr = false)
        {
            if (_m_stepRewardRef == null || _m_stepRewardInfo == null || _m_stepRewardInfo.stepRewardSetRef == null)
                return null;

            long curCount = _m_stepRewardInfo.totalScore;
            long targetCount = _m_stepRewardRef.complete_count;

            //获取对应格式进度字符串
            string curCountStr = GCommon.getValueFormatStr(_m_stepRewardInfo.stepRewardSetRef.process_num_format, curCount);
            string targetCountStr = GCommon.getValueFormatStr(_m_stepRewardInfo.stepRewardSetRef.process_num_format, targetCount);

            if (_chgAllStr)
                return GCommon.addColorForRichText(TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, curCountStr, targetCountStr), _txtColor);

            return TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, GCommon.addColorForRichText(curCountStr, _txtColor), targetCountStr);
        }

        /// <summary>
        /// 获取步骤状态
        /// </summary>
        /// <param name="_stat"></param>
        /// <returns></returns>
        private EAchieveStepGetStat _getStepStat(EStepRewardState _stat)
        {
            switch (_stat)
            {
                case EStepRewardState.CanGet:
                    return EAchieveStepGetStat.CAN_GET;
                case EStepRewardState.AlreadyGet:
                    return EAchieveStepGetStat.HAS_GET;
                case EStepRewardState.None:
                    if (_m_stepRewardInfo != null)
                    {
                        GActivityStepRewardRefObj curStepRef = _m_stepRewardInfo.getFirstNotGetRewardStep();
                        if(_m_stepRewardRef != null && curStepRef != null && _m_stepRewardRef.step <= curStepRef.step)
                            return EAchieveStepGetStat.DOING_CAN_NOT_GET;
                        else
                            return EAchieveStepGetStat.CAN_NOT_GET;
                    }
                    else
                        return EAchieveStepGetStat.CAN_NOT_GET;
                default:
                    return EAchieveStepGetStat.NONE;
            }
        }
        
        /// <summary>
        /// 点击前往按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onGotoBtnClick(GameObject _go)
        {
            if (_m_stepRewardInfo == null || _m_stepRewardInfo.stepRewardSetRef == null)
                return;

            _m_stepRewardInfo.stepRewardSetRef.go_to?.dealEffect();
        }
        
        /// <summary>
        /// 点击领奖按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onGetBtnClick(GameObject _go)
        {
            if (_m_stepRewardInfo == null || _m_stepRewardRef == null)
                return;
            
            EStepRewardState state = _m_stepRewardInfo.getStepRewardState(_m_stepRewardRef);
            EAchieveStepGetStat stepState = _getStepStat(state);
            if (stepState != EAchieveStepGetStat.CAN_GET)
                return;
            
            NPPlayer.instance.commonActivityComp.reqDrawActivityStepReward(_m_stepRewardInfo.activityInstanceId,
                _m_stepRewardInfo.stepRewardSetId, _m_stepRewardRef.step, (_isSuc, _msg) =>
                {
                });
        }
    }
}