using ALPackage;
using Common.PlayerEnum;
using JetBrains.Annotations;
using NPCommon;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标完成大阶段展示界面
    /// </summary>
    public class GGUIWndStageGoalBigStepComplete : _ANPGGUIBasicWnd<GGUIMonoStageGoalBigStepComplete>
    {
        [NotNull] public static GGUIWndStageGoalBigStepComplete instance { get { return _g_instance ??= new GGUIWndStageGoalBigStepComplete(); } }
        private static GGUIWndStageGoalBigStepComplete _g_instance;

        //大阶段信息
        private StageGoalBigStepRefObj _m_bigStepRefObj;
        //奖励物品列表
        private List<NPCommon_ItemInfo> _m_itemInfoList;
        //阶段图标
        private NPGGuiWndTexture _m_stageTexture;
        //奖励列表
        private NPGGUIWndCommonItemContainer _m_wItemContainer;
        
        public GGUIWndStageGoalBigStepComplete() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoStageGoalBigStepComplete.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStageGoalBigStepComplete.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_stageTexture?.showWnd();
            _m_wItemContainer?.showWnd();
            _refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_stageTexture?.hideWnd();
            _m_wItemContainer?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_stageTexture?.discardTexture();
            _m_wItemContainer?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_stageTexture?.discard();
            _m_stageTexture = null;
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.imgStage != null)
                _m_stageTexture = new NPGGuiWndTexture(wnd.imgStage);
            if (wnd.itemContainer != null)
                _m_wItemContainer = new NPGGUIWndCommonItemContainer(wnd.itemContainer);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }
        
        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_bigStepRefObj"></param>
        public void setInfo(StageGoalBigStepRefObj _bigStepRefObj, List<NPCommon_ItemInfo> _itemList)
        {
            _m_bigStepRefObj = _bigStepRefObj;
            _m_itemInfoList = _itemList;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_bigStepRefObj == null)
                return;

            //设置标题
            ALUGUICommon.setLabelTxt(wnd.txtTitle, _m_bigStepRefObj.getTitle);
            //设置阶段图标
            _m_stageTexture?.setTexture(_m_bigStepRefObj.icon);
            //设置阶段编号
            ALUGUICommon.setLabelTxt(wnd.txtStageNum, TextTranslate.instance.getLanguage(TransKeyConst.stage_goal_taskNum_num, _m_bigStepRefObj.big_step));
            //设置奖励列表
            _m_wItemContainer?.showItemList(_m_itemInfoList?.toItemDataList());
            //设置完成时间
            long time = NPPlayer.instance.eventRecordComp.getValue(EPlayerEventRecordType.STAGE_GOAL_FINISH_TIME_MS, _m_bigStepRefObj.begins_from_small_step);
            ALUGUICommon.setLabelTxt(wnd.txtStageCompleteTime, TimeUtil.Milliseconds2StringYMD(time));
        }

        //点击关闭
        private void _onClickCloseBtn(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_STAGE_GOAL_BIG_STEP_COMPLETE);
        }
    }
}