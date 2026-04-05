
using ALPackage;
using CommonEnum;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public partial class GGUIWndChildMain : _ANPGGUIBasicResBarWnd<GGUIMonoChildMain>
    {
        [NotNull] public static GGUIWndChildMain instance { get { return _g_instance ??= new GGUIWndChildMain(); } }
        private static GGUIWndChildMain _g_instance;


        private NPGGUISubCurrencyHarvestWnd _m_expHarvestWnd;
        private GGUISubWndChildInfo _m_childInfo;
        private GGUIWndChildMainNamingShow _m_namingShow;
        private GGUIWndChildMainEducatingShow _m_educatingShow;
        private GGUIWndChildMainGraduatingShow _m_graduatingShow;
        private GGUIWndChildMainPhaseUp _m_phaseUpShow;
        private GGUIWndChildMainEmpty _m_emptyShow;
        private GGUISubWndChildContainer _m_childContainer;
        private GGUISubWndConsortInviteBuff _m_buffWnd;
        
        private ChildViewMgr _m_viewMgr;
        

        public GGUIWndChildMain() 
            : base(EALUIWndLayer.NORMAL)
        {
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoChildMain.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoChildMain.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_expHarvestWnd?.init();
            _m_childInfo?.showWnd();
            _m_namingShow?.showWnd();
            _m_educatingShow?.showWnd();
            _m_graduatingShow?.showWnd();
            _m_phaseUpShow?.showWnd();
            _m_emptyShow?.showWnd();
            _m_childContainer?.showWnd();
            _m_buffWnd?.showWnd();
            
            refreshWnd();
            
            WinMsg.RegisterMsgAct(WinMsgType.CHILD_EDUCATION_PREDICT_FAIL, refreshWnd);
            WinMsg.RegisterMsgAct(WinMsgType.SET_SELECT_FIRST_NAMED_NO_GRADUATE_CHILD, _onSetSelectFirstNamedNoGraduateChild);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.CHILD_EDUCATION_PREDICT_FAIL, refreshWnd);
            WinMsg.UnregisterMsgAct(WinMsgType.SET_SELECT_FIRST_NAMED_NO_GRADUATE_CHILD, _onSetSelectFirstNamedNoGraduateChild);
            
            _m_expHarvestWnd?.discard();
            _m_childInfo?.hideWnd();
            _m_namingShow?.hideWnd();
            _m_educatingShow?.hideWnd();
            _m_graduatingShow?.hideWnd();
            _m_phaseUpShow?.hideWnd();
            _m_emptyShow?.hideWnd();
            _m_childContainer?.hideWnd();
            _m_buffWnd?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_childInfo?.resetWnd();
            _m_namingShow?.resetWnd();
            _m_educatingShow?.resetWnd();
            _m_graduatingShow?.resetWnd();
            _m_phaseUpShow?.resetWnd();
            _m_emptyShow?.resetWnd();
            _m_childContainer?.resetWnd();
            _m_buffWnd?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_childInfo?.discard();
            _m_namingShow?.discard();
            _m_educatingShow?.discard();
            _m_graduatingShow?.discard();
            _m_phaseUpShow?.discard();
            _m_emptyShow?.discard();
            _m_childContainer?.discard();
            _m_buffWnd?.discard();
            _m_childInfo = null;
            _m_namingShow = null;
            _m_educatingShow = null;
            _m_graduatingShow = null;
            _m_phaseUpShow = null;
            _m_emptyShow = null;
            _m_childContainer = null;
            _m_buffWnd = null;
            
            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onBtnDetailClicked);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.txtExpValue != null)
                _m_expHarvestWnd = new NPGGUISubCurrencyHarvestWnd(ECurrency.HERO_EXP, wnd.txtExpValue, wnd.txtExpValue.transform as RectTransform, string.Empty, EHarvestType.P_EXP);
            if (wnd.monoCurChildInfo != null)
                _m_childInfo = new GGUISubWndChildInfo(wnd.monoCurChildInfo);
            if (wnd.namingShow != null)
                _m_namingShow = new GGUIWndChildMainNamingShow(wnd.namingShow);
            if (wnd.educatingShow != null)
                _m_educatingShow = new GGUIWndChildMainEducatingShow(wnd.educatingShow);
            if (wnd.phaseUpShow != null)
                _m_phaseUpShow = new GGUIWndChildMainPhaseUp(wnd.phaseUpShow);
            if (wnd.graduatingShow != null)
                _m_graduatingShow = new GGUIWndChildMainGraduatingShow(wnd.graduatingShow);
            if (wnd.emptyShow != null)
                _m_emptyShow = new GGUIWndChildMainEmpty(wnd.emptyShow);
            if (wnd.monoChildContainer != null)
                _m_childContainer = new GGUISubWndChildContainer(wnd.monoChildContainer, _onSeatClick);
            if (wnd.monoBuff != null)
                _m_buffWnd = new GGUISubWndConsortInviteBuff(wnd.monoBuff);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onBtnCloseClicked);
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onBtnDetailClicked);
        }


        public void refreshWnd(ChildViewMgr _viewMgr)
        {
            _m_viewMgr = _viewMgr;
            refreshWnd();
        }
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow || _m_viewMgr == null)
                return;
            
            SeatInfo curSelectSeatInfo = _m_viewMgr.curSelectSeatInfo;
            // 理论上不应该出现不存在的情况，这里直接 return 不做异常处理了
            if (curSelectSeatInfo == null)
                return;
            
            // 尝试获取子嗣相关值
            ChildInfo childInfo = curSelectSeatInfo.childInfo;
            int childStep = 0;
            int childLevel = 0;
            int childMaxLevel = 0;
            if (childInfo != null)
            {
                childStep = childInfo.step;
                childLevel = childInfo.level;
                childMaxLevel = childInfo.maxLevel;
                // 设置进度条上的每个阶段的分割点
                wnd.setPhaseSplit(childInfo.qualityRef.getPhaseSplitList());
            }
            
            // 刷新不同阶段显示的内容
            wnd.setPhase(childStep);
            // 设置当前的等级进度
            Slider progress = wnd.sldPhaseProgress;
            if (progress != null)
            {
                // 设置进度条
                progress.maxValue = childMaxLevel;
                progress.value = childLevel;
            }
            // 设置进度文本
            ALUGUICommon.setLabelTxt(wnd.txtPhaseProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, childLevel, childMaxLevel));
            // 设置当前选中的子嗣信息
            _m_childInfo?.refreshWnd(childInfo);
            // 刷新各个子部分
            _m_namingShow?.refreshWnd(_m_viewMgr);
            _m_graduatingShow?.refreshWnd(_m_viewMgr);
            _m_educatingShow?.refreshWnd(_m_viewMgr);
            _m_emptyShow?.refreshWnd(_m_viewMgr);
            // 判断当前界面的显示状态
            GGUIMonoChildMainShowType showType = GGUIMonoChildMainShowType.Empty;
            if (childInfo != null)
            {
                if (string.IsNullOrEmpty(childInfo.name))
                    showType = GGUIMonoChildMainShowType.Naming;
                else if (childInfo.canGraduate())
                    showType = GGUIMonoChildMainShowType.Graduating;
                else if (_m_viewMgr.curState == ChildViewMgrType.StepUp)
                    showType = GGUIMonoChildMainShowType.PhaseUp;
                else
                    showType = GGUIMonoChildMainShowType.Educating;
            }
            // 根据显示状态刷新不用的显示列表
            wnd.setType(showType);
            // 刷新席位列表
            _m_childContainer?.refreshWnd(_m_viewMgr);
        }
        public void refreshProgress()
        {
            if (wnd == null || _m_viewMgr == null)
                return;
            
            SeatInfo curSelectSeatInfo = _m_viewMgr.curSelectSeatInfo;
            // 理论上不应该出现不存在的情况，这里直接 return 不做异常处理了
            if (curSelectSeatInfo == null)
                return;
            
            // 尝试获取子嗣相关值
            ChildInfo childInfo = curSelectSeatInfo.childInfo;
            int childLevel = 0;
            int childMaxLevel = 0;
            if (childInfo != null)
            {
                childLevel = childInfo.level;
                childMaxLevel = childInfo.maxLevel;
            }
            
            // 设置当前的等级进度
            Slider progress = wnd.sldPhaseProgress;
            if (progress != null)
            {
                // 设置进度条
                progress.maxValue = childMaxLevel;
                progress.value = childLevel;
            }
            // 设置进度文本
            ALUGUICommon.setLabelTxt(wnd.txtPhaseProgress, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, childLevel, childMaxLevel)); 
        }
        public void refreshBrainValue()
        {
            _m_educatingShow?.refreshBrainValue();
        }
        public void showExpCollect(Vector2 _clickScreenPos, long _num)
        {
            _m_educatingShow?.showExpCollect(_clickScreenPos, _num);
        }
        public void showContainerExpCollect(ChildInfo _childInfo, long _num)
        {
            _m_childContainer?.showExpCollect(_childInfo, _num);
        }
        public void showEnergyRecover(long _seatId, long _energy)
        {
            _m_childContainer?.showEnergyRecover(_seatId, _energy);
        }
        public void refreshContainerItem(ChildInfo _childInfo)
        {
            _m_childContainer?.refreshItem(_childInfo);
        }
        public void refreshChildInfo()
        {
            _m_childInfo?.refreshWnd();
        }
        public void playEducateSfx(ChildInfo _childInfo)
        {
            _m_childContainer?.playEducateSfx(_childInfo);
        }

        /// <summary>
        /// 获取目标子嗣位置的RectTransform
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public RectTransform getTargetHeroRectTransform(EChildMainTargetChildType _type)
        {
            if (_m_childContainer == null)
                return null;

            switch (_type)
            {
                case EChildMainTargetChildType.NAMED_NO_GRADUATE:
                    return _m_childContainer.getNamedNoGraduateRectTransform();
                default:
                    return null;
            }
        }

        private void _onBtnCloseClicked(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Child.C_MAIN_CHILD_NODE);
        }
        private void _onBtnDetailClicked(GameObject _)
        {
            ChildInfo childInfo = _m_viewMgr?.curSelectSeatInfo?.childInfo;
            if (childInfo == null)
                return;
            
            GGUIWndChildDetail.instance.refreshWnd(childInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndChildDetail.instance, GGUIWndChildDetail.instance.showWnd, EUIQueueStageType.MAIN, string.Empty, true, false);
        }
        private void _onSeatClick(SeatInfo _seatInfo)
        {
            if (_seatInfo == null)
                return;
            
            if (!_seatInfo.isUnlock())
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(_seatInfo.baseRef.getTranslatedUnlockDesc());
                return;
            }
            
            _m_viewMgr?.selectSeat(_seatInfo);
        }
        /// <summary>
        /// 设置选择第一个已命名未毕业的子嗣
        /// </summary>
        private void _onSetSelectFirstNamedNoGraduateChild()
        {
            if (_m_viewMgr == null)
                return;

            for (int i = 0; i < _m_viewMgr.seatList.Count; i++)
            {
                SeatInfo seatInfo = _m_viewMgr.seatList[i];
                if(seatInfo.isUnlock() && seatInfo.childInfo != null && !string.IsNullOrEmpty(seatInfo.childInfo.name) && !seatInfo.childInfo.canGraduate())
                {
                    _m_viewMgr.selectSeat(seatInfo);
                    break;
                }
            }
        }
    }
}