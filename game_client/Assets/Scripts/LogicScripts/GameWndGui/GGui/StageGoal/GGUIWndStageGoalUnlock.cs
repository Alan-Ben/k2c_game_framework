using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标小阶段解锁弹窗
    /// </summary>
    public class GGUIWndStageGoalUnlock : _ANPGGUIBasicWnd<GGUIMonoStageGoalUnlock>
    {
        [NotNull] public static GGUIWndStageGoalUnlock instance { get { return _g_instance ??= new GGUIWndStageGoalUnlock(); } }
        private static GGUIWndStageGoalUnlock _g_instance;
        
        private StageGoalRefObj _m_stageRefObj;
        private NPGGuiWndTexture _m_curStageIcon;
        
        public GGUIWndStageGoalUnlock() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoStageGoalUnlock.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStageGoalUnlock.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_curStageIcon?.showWnd();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_curStageIcon?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_curStageIcon?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_curStageIcon?.discard();
            _m_curStageIcon = null;

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
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }

        public void refreshWnd(StageGoalRefObj _stageGoal)
        {
            _m_stageRefObj = _stageGoal;
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
            ALUGUICommon.setLabelTxt(wnd.txtStageTitle, TextTranslate.instance.getLanguage(TransKeyConst.common_twoParamWithSpace_str_str, numString, _m_stageRefObj.getTitle));
            _m_curStageIcon?.setTexture(_m_stageRefObj.icon);
        }

        //点击关闭
        private void _onClickCloseBtn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_STAGE_GOAL_UNLOCK);
        }
    }
}