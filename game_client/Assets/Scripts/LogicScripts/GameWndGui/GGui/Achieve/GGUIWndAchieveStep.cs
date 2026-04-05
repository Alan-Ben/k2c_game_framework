using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 成就步骤详情窗口
    /// </summary>
    public class GGUIWndAchieveStep : _ANPGGUIBasicWnd<GGUIMonoAchieveStep>
    {
        private static GGUIWndAchieveStep _g_instance = new GGUIWndAchieveStep();
        public static GGUIWndAchieveStep instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new GGUIWndAchieveStep();
                return _g_instance;
            }
        }

        private GGUIWndAchievePointProgress _m_wAchievePointProgress;//成就进度窗口
        private GGUIWndAchieveStepGrid _m_wAchieveStepGrid;//成就步骤列表
        private AchieveInfo _m_info;//成就信息

        public GGUIWndAchieveStep() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoAchieveStep.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAchieveStep.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_ACHIEVE_INFO_CHG, _onAchieveInfoChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_ACHIEVE_INFO_CHG, _onAchieveInfoChg);

            if (_m_wAchievePointProgress != null)
                _m_wAchievePointProgress.hideWnd();

            if (_m_wAchieveStepGrid != null)
                _m_wAchieveStepGrid.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wAchievePointProgress != null)
                _m_wAchievePointProgress.resetWnd();

            if (_m_wAchieveStepGrid != null)
                _m_wAchieveStepGrid.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_wAchievePointProgress != null)
                _m_wAchievePointProgress.discard();
            _m_wAchievePointProgress = null;

            if (_m_wAchieveStepGrid != null)
                _m_wAchieveStepGrid.discard();
            _m_wAchieveStepGrid = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAchievePointProgress != null)
                _m_wAchievePointProgress = new GGUIWndAchievePointProgress(wnd.monoAchievePointProgress);

            if (wnd.monoStepGrid != null)
                _m_wAchieveStepGrid = new GGUIWndAchieveStepGrid(wnd.monoStepGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(AchieveInfo _info)
        {
            if (_info == null)
                return;

            _m_info = _info;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshTitle();
            _refreshAchievePointProgress();
            _refreshAchieveStepGrid();
        }

        //刷新标题
        private void _refreshTitle()
        {
            if (wnd == null || _m_info == null || _m_info.achieveRefObj == null)
                return;

            AchieveTypeRefObj achieveTypeRef = GRefdataCoreMgr.instance.achieveTypeMap.getRef((long)_m_info.achieveType);
            if (achieveTypeRef != null)
                ALUGUICommon.setLabelTxt(wnd.txtTitle, TextTranslate.instance.getLanguage(achieveTypeRef.title));
        }

        //刷新成就点进度
        private void _refreshAchievePointProgress()
        {
            if (_m_info == null)
                return;

            if (_m_wAchievePointProgress != null)
            {
                _m_wAchievePointProgress.showWnd();
                _m_wAchievePointProgress.setInfo(_m_info.achieveType);
            }
        }

        //属性成就步骤列表
        private void _refreshAchieveStepGrid()
        {
            if (_m_info == null || _m_info.achieveRefObj == null)
                return;

            if (_m_wAchieveStepGrid != null)
            {
                _m_wAchieveStepGrid.showWnd();
                _m_wAchieveStepGrid.setShowData(_m_info);
            }
        }

        //点击关闭按钮
        private void _onClickCloseBtn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_ACHIEVE_STEP);
        }

        //成就信息变更
        private void _onAchieveInfoChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _m_info == null)
                return;

            AchieveInfo info = (AchieveInfo) _objects[0];
            if(info != null && info.achieveId == _m_info.achieveId)
                _refreshAchieveStepGrid();
        }
    }
}
