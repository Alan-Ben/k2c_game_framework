using System.Collections.Generic;
using ALPackage;
using Common.ChildEnum;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndAdultMainUnmarried : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoAdultMainUnmarried>
    {
        [ItemNotNull, NotNull] private readonly List<UnmarriedInfo> _m_unmarriedInfoList;
        
        private GGUISubWndAdultUnmarriedGrid _m_adultGrid;
        private ALCommonEnableTaskController _m_timeTask;
        
        
        public GGUIPrefabSubWndAdultMainUnmarried(Transform _parent) 
            : base(_parent)
        {
            _m_unmarriedInfoList = new List<UnmarriedInfo>();
        }
        
        
        protected override string _monoAssetPath { get { return GGUIMonoAdultMainUnmarried.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoAdultMainUnmarried.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            _m_adultGrid?.showWnd();

            refreshWnd();

            NPPlayer.instance.childComp.onUnmarriedAdultCountChg += refreshWnd;
            NPPlayer.instance.childComp.onUnmarriedAdultStateChg += _stateChg;
            _m_timeTask = ALCommonEnableDurationActionMonoTask.addMonoTask(_timeTask, 1f);
        }
        protected override void _onHideWnd()
        {
            _m_timeTask.setDisable();
            NPPlayer.instance.childComp.onUnmarriedAdultStateChg -= _stateChg;
            NPPlayer.instance.childComp.onUnmarriedAdultCountChg -= refreshWnd;
            
            _m_adultGrid?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_adultGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_adultGrid?.discard();
            _m_adultGrid = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoAdultGrid != null)
            {
                _m_adultGrid = new GGUISubWndAdultUnmarriedGrid(wnd.monoAdultGrid);
                _m_adultGrid.onClickItemApply += _onClickItemApply;
            }
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            // ALUGUICommon.setLabelTxt(wnd.txtAdultNum, TextTranslate.instance.getLanguage(TransKeyConst.adult_totalUnmarriedAdultNum_num, NPPlayer.instance.childComp.getUnmarriedChildCount()));
            ALUGUICommon.setLabelTxt(wnd.txtLimitDesc, TextTranslate.instance.getLanguage(TransKeyConst.adult_graduate_limited_num, GRefdataCoreMgr.instance.npGeneral.unmarry_adult_limit));
            ALUGUICommon.setLabelTxt(wnd.txtAdultNum, NPPlayer.instance.childComp.getUnmarriedChildCount());
            NPPlayer.instance.childComp.getUnmarriedChildListNonAlloc(_m_unmarriedInfoList);
            _m_unmarriedInfoList.Sort(_sortUnmarriedInfo);
            _m_adultGrid?.refreshWnd(_m_unmarriedInfoList);
        }
        private void _stateChg()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            _m_adultGrid?.refreshAllItem((_item, _index) =>
            {
                _item.refreshState();
                _item.refreshTime();
            });
        }
        private void _timeTask()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            _m_adultGrid?.refreshAllItem((_item, _index) => _item.refreshTime());
        }
        private int _sortUnmarriedInfo(UnmarriedInfo _x, UnmarriedInfo _y)
        {
            if (_x.status == EAdultStatus.NONE && _y.status != EAdultStatus.NONE)
                return 1;
            if (_x.status != EAdultStatus.NONE && _y.status == EAdultStatus.NONE)
                return -1;
            if (_x.status != EAdultStatus.NONE && _y.status != EAdultStatus.NONE)
                return _x.expiredTs.CompareTo(_y.expiredTs);
            return _y.adultInfo.graduateTs.CompareTo(_x.adultInfo.graduateTs);
        }

        /// <summary>
        /// 点击item申请组队按钮
        /// </summary>
        /// <param name="_info"></param>
        private void _onClickItemApply(UnmarriedInfo _info)
        {
            if (_info == null)
                return;

            GGUIWndAdultEngageRequestSend.instance.refreshWnd(_info.adultInfo);
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndAdultEngageRequestSend.instance, GGUIWndAdultEngageRequestSend.instance.showWnd, UINodeTagConst.C_ADULT_ENGAGE_REQUEST_SEND);

        }
    }
}