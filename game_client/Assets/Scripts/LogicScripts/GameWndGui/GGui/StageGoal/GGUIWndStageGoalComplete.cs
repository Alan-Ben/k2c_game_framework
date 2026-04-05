using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using NPCommon;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标小阶段完成弹窗
    /// </summary>
    public class GGUIWndStageGoalComplete : _ANPGGUIBasicWnd<GGUIMonoStageGoalComplete>
    {
        [NotNull] public static GGUIWndStageGoalComplete instance { get { return _g_instance ??= new GGUIWndStageGoalComplete(); } }
        private static GGUIWndStageGoalComplete _g_instance;
        
        
        private StageGoalRefObj _m_stageRefObj;
        private List<NPCommon_ItemInfo> _m_rewardItemList;
        
        private NPGGUIWndCommonItemContainer _m_rewardItemContainer;
        private NPGGuiWndTexture _m_curStageIcon;
        
        
        public GGUIWndStageGoalComplete() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoStageGoalComplete.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStageGoalComplete.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_curStageIcon?.showWnd();
            _m_rewardItemContainer?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_curStageIcon?.hideWnd();
            _m_rewardItemContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_curStageIcon?.discardTexture();
            _m_rewardItemContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_curStageIcon?.discard();
            _m_curStageIcon = null;
            _m_rewardItemContainer?.discard();
            _m_rewardItemContainer = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgStage != null)
                _m_curStageIcon = new NPGGuiWndTexture(wnd.imgStage);
            if (wnd.monoRewardItemContainer != null)
                _m_rewardItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoRewardItemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }


        public void refreshWnd(StageGoalRefObj _stageGoal, List<NPCommon_ItemInfo> _itemList)
        {
            _m_stageRefObj = _stageGoal;
            _m_rewardItemList = _itemList;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_stageRefObj == null)
                return;

            StageGoalBigStepRefObj bigStepRefObj = GRefdataCoreMgr.instance.getStageGoalBigStepRefObj((int)_m_stageRefObj.step);
            int bigStepNum = bigStepRefObj != null ? bigStepRefObj.big_step : 0;
            int bigStepFromSmallStep = bigStepRefObj != null ? bigStepRefObj.begins_from_small_step : 0;
            int smallStepNum = (int)_m_stageRefObj.step - bigStepFromSmallStep + 1;
            string numString = TextTranslate.instance.getLanguage(TransKeyConst.common_interval2_num_num, bigStepNum, smallStepNum);
            ALUGUICommon.setLabelTxt(wnd.txtStageNum, numString);
            ALUGUICommon.setLabelTxt(wnd.txtStageTitle, _m_stageRefObj.getTitle);
            _m_curStageIcon?.setTexture(_m_stageRefObj.icon);
            _m_rewardItemContainer?.showItemList(_m_rewardItemList.toItemDataList());

            //下个阶段
            StageGoalRefObj nextStageRefObj = GRefdataCoreMgr.instance.stageGoalRefCore.getRef(_m_stageRefObj.step + 1);
            ALUGUICommon.setGameObjEnable(wnd.noNextStageHideList, nextStageRefObj != null);
            ALUGUICommon.setGameObjEnable(wnd.noNextStageShowList, nextStageRefObj == null);
            if (null != nextStageRefObj)
            {
                StageGoalBigStepRefObj nextBigStepRef = GRefdataCoreMgr.instance.getStageGoalBigStepRefObj((int)nextStageRefObj.step);
                if (nextBigStepRef != null)
                {
                    smallStepNum = (int)nextStageRefObj.step - nextBigStepRef.begins_from_small_step + 1;
                    numString = TextTranslate.instance.getLanguage(TransKeyConst.common_interval2_num_num, nextBigStepRef.big_step, smallStepNum);
                    ALUGUICommon.setLabelTxt(wnd.txtNextStageNum, numString);
                    ALUGUICommon.setLabelTxt(wnd.txtNextStageTitle, nextStageRefObj.getTitle);
                }
            }
        }
        
        //点击关闭按钮
        private void _onClickCloseBtn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_STAGE_GOAL_COMPLETE);
        }
    }
}