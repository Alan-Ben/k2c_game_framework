using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 委派列表item
    /// </summary>
    public class GGUIWndWeekCardAssignSubItem_Normal : _AGGUIWndWeekCardAssignSubItemBase
    {
        private GGUIMonoWeekCardAssignSubItem_Normal _m_realWndMono;
        private readonly string _m_assetPath;
        private readonly string _m_objName;
        private NPGGUIWndCommonToggleEx _m_togDoneDeal;
        private bool _m_isDoneDeal = false;
        private WeekCardAssignItemShowInfo_Normal _m_realInfo;

        public GGUIWndWeekCardAssignSubItem_Normal(string _assetPath, string _objName, Transform _parent) : base(_parent)
        {
            _m_assetPath = _assetPath;
            _m_objName = _objName;
        }


        protected override string _monoAssetPath { get => _m_assetPath; }
        protected override string _monoObjName { get => _m_objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        protected override void _onShowWndEx()
        {
            
        }

        protected override void _onHideWndEx()
        {
            
        }

        protected override void _onDiscardEx()
        {
            _m_togDoneDeal?.discard();
            _m_togDoneDeal = null;
        }

        protected override void _onWndInitDoneEx()
        {
            if(null == wnd)
                return;
            _m_realWndMono = wnd as GGUIMonoWeekCardAssignSubItem_Normal;
            if(null == _m_realWndMono)
                return;

            if (null != _m_realWndMono.togDoneDeal)
            {
                _m_togDoneDeal = new NPGGUIWndCommonToggleEx(_m_realWndMono.togDoneDeal);
                _m_togDoneDeal.clickDelegate += _onClickDelegate;
            }
        }

        protected override void _onRefreshWnd(_IWeekCardAssignItemShowInfo _info)
        {
            _m_realInfo = _info as WeekCardAssignItemShowInfo_Normal;
            if(null == _m_realInfo)
                return;
            //获取数据
            _m_isDoneDeal = !_m_realInfo.settingInfo.exInfo.getIsLazy();
            
            setDoneDealTog(_m_isDoneDeal);

            ALUGUICommon.setLabelTxt(_m_realWndMono.txtDesc, info.getAssignDesc());
        }

        private void _onClickDelegate(NPGGUIWndCommonToggleEx obj)
        {
            _m_isDoneDeal = !_m_isDoneDeal;
            setDoneDealTog(_m_isDoneDeal);
            _m_realInfo?.settingInfo?.setExInfo(!_m_isDoneDeal);
        }

        private void setDoneDealTog(bool _isDoneDeal)
        {
            _m_togDoneDeal?.setSelected(_isDoneDeal);
            
        }
    }
}
