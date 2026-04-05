using System.Collections.Generic;
using ALPackage;
using CommonEnum;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 周卡委派界面
    /// </summary>
    public class GGUIWndWeekCardAssign : _ANPGGUIBasicWnd<GGUIMonoWeekCardAssign>
    {
        private static GGUIWndWeekCardAssign _g_instance = new GGUIWndWeekCardAssign();
        public static GGUIWndWeekCardAssign instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new  GGUIWndWeekCardAssign();
                return _g_instance;
            }
        }
        
        public GGUIWndWeekCardAssign() : base(EALUIWndLayer.NORMAL)
        {
        }

        private GGUIWndWeekCardAssignSubItemContainer _m_assignItemContainer;
        private NPGGUIWndCommonShowCase _m_assignShowcase;

        protected override string _monoAssetPath { get => GGUIMonoWeekCardAssign.assetPath; }
        protected override string _monoObjName { get => GGUIMonoWeekCardAssign.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

        protected override void _onShowWnd()
        {
            _refreshWnd();
            NPPlayer.instance.weekCardComp.onWeekCardInfoChg += _refreshShowCaseWnd;
        }

        protected override void _onHideWnd()
        {
            _m_assignShowcase?.hideWnd();
            NPPlayer.instance.weekCardComp.onWeekCardInfoChg -= _refreshShowCaseWnd;
        }

        protected override void _onReset()
        {
            _m_assignItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_assignItemContainer?.discard();
            _m_assignItemContainer = null;
            
            _m_assignShowcase?.discard();
            _m_assignShowcase = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            ALUGUICommon.combineBtnClick(wnd.btnAssignConsort, _clickAssignConsort);
            if (null != wnd.assignItemContainer)
            {
                _m_assignItemContainer = new GGUIWndWeekCardAssignSubItemContainer(wnd.assignItemContainer);                
            }

            if (null != wnd.assignShowcase)
            {
                _m_assignShowcase = new NPGGUIWndCommonShowCase(wnd.assignShowcase);
            }
        }

        /// <summary>
        /// 委派执政官
        /// </summary>
        /// <param name="obj"></param>
        private void _clickAssignConsort(GameObject obj)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndWeekCardAssignTDShow.instance,GGUIWndWeekCardAssignTDShow.instance.showWnd);
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;
            List<_AGWeekCardSettingInfoBase> settingInfoList = new List<_AGWeekCardSettingInfoBase>();
            NPPlayer.instance.weekCardComp.getSettingInfoList(settingInfoList);
            settingInfoList.Sort(_sortSettingInfo);
            List<_IWeekCardAssignItemShowInfo> itemDataList = new List<_IWeekCardAssignItemShowInfo>();
            for (int i = 0; i < settingInfoList.Count; i++)
            {
                itemDataList.Add(_getItemShowInfo(settingInfoList[i]));
            }
            
            _m_assignItemContainer?.showWnd();
            _m_assignItemContainer?.setInfo(itemDataList);

            _refreshShowCaseWnd();
        }

        private void _refreshShowCaseWnd()
        {
            NPGGoIndex goIndex = NPPlayer.instance.weekCardComp.getTDShowIndex();
            
            _m_assignShowcase?.showWnd(new ShowCaseCommonResUnitInfoObj(goIndex));
        }

        /// <summary>
        /// 对信息排序
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int _sortSettingInfo(_AGWeekCardSettingInfoBase x, _AGWeekCardSettingInfoBase y)
        {
            int _xIdx = wnd.settleTypeSort.IndexOf(x.settingType);
            int _yIdx = wnd.settleTypeSort.IndexOf(y.settingType);
            if (_xIdx < _yIdx)
                return -1;
            if (_xIdx > _yIdx)
                return 1;
            return 0;
        }

        private _IWeekCardAssignItemShowInfo _getItemShowInfo(_AGWeekCardSettingInfoBase _settingInfo)
        {
            switch (_settingInfo.settingType)
            {
                case EWeekCardSettleType.TRAVEL:
                    return new WeekCardAssignItemShowInfo_Normal(_settingInfo as GWeekCardSettingInfo_DealPolicy, ENPFunctionType.TRAVEL);
                case EWeekCardSettleType.ANECDOTE:
                    return new WeekCardAssignItemShowInfo_Normal(_settingInfo as GWeekCardSettingInfo_DealPolicy, ENPFunctionType.ANECDOTE);
                case EWeekCardSettleType.CHILD_TRAIN:
                    return new WeekCardAssignItemShowInfo_Normal(_settingInfo as GWeekCardSettingInfo_DealPolicy, ENPFunctionType.CHILD);
                // case EWeekCardSettleType.LEVY:
                //     return new WeekCardAssignItemShowInfo_Normal(_settingInfo as GWeekCardSettingInfo_DealPolicy, ENPFunctionType.LEVY);
                case EWeekCardSettleType.CONSORT_RND_CALL:
                    return new WeekCardAssignItemShowInfo_Normal(_settingInfo as GWeekCardSettingInfo_DealPolicy, ENPFunctionType.CONSORT);
                case EWeekCardSettleType.COLLEGE_STUDY:
                    return new WeekCardAssignItemShowInfo_College(_settingInfo as GWeekCardSettingInfo_CollegeStudy);
                
                default:
                    return null;
            }
        }
    }
}