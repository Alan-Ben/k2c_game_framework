using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 大阶段解锁界面
    /// </summary>
    public class GGUIWndStageGoalBigStepUnlock : _ANPGGUIBasicWnd<GGUIMonoStageGoalBigStepUnlock>
    {
        [NotNull] public static GGUIWndStageGoalBigStepUnlock instance { get { return _g_instance ??= new GGUIWndStageGoalBigStepUnlock(); } }
        private static GGUIWndStageGoalBigStepUnlock _g_instance;
        
        //大阶段信息
        private StageGoalBigStepRefObj _m_bigStepRefObj;
        //大阶段图标
        private NPGGuiWndTexture _m_stageTexture;
        
        public GGUIWndStageGoalBigStepUnlock() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoStageGoalBigStepUnlock.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStageGoalBigStepUnlock.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }


        protected override void _onShowWnd()
        {
            _m_stageTexture?.showWnd();
            
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            _m_stageTexture?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_stageTexture?.discardTexture();
        }
        protected override void _onDiscard()
        {
            _m_stageTexture?.discard();
            _m_stageTexture = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.stageImg != null)
                _m_stageTexture = new NPGGuiWndTexture(wnd.stageImg);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }
        
        
        public void refreshWnd(StageGoalBigStepRefObj _bigStepRefObj)
        {
            _m_bigStepRefObj = _bigStepRefObj;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_bigStepRefObj == null)
                return;

            //设置当前大阶段
            ALUGUICommon.setLabelTxt(wnd.txtTitle, _m_bigStepRefObj.getTitle);
            _m_stageTexture?.setTexture(_m_bigStepRefObj.icon);
            ALUGUICommon.setLabelTxt(wnd.stageNumTxt, string.Format("{0:D2}", _m_bigStepRefObj.big_step));
        }

        private void _onClickCloseBtn(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_STAGE_GOAL_BIG_STEP_UNLOCK);
        }
    }
}