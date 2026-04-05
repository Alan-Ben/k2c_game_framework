using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public abstract class _AGGUIWndWeekCardAssignSubItemBase : _AGGUIWndWeekCardAssignSubItemBase<_AGGUIMonoWeekCardAssignSubItemBase>
    {
        public _AGGUIWndWeekCardAssignSubItemBase(Transform _parent) : base(_parent)
        {
        }
    }
    
    /// <summary>
    /// 委派列表item-基类
    /// </summary>
    public abstract class _AGGUIWndWeekCardAssignSubItemBase<T_MONO> : _ATALBasicLoadPrefabSubUIWnd<T_MONO>
    where T_MONO : _AGGUIMonoWeekCardAssignSubItemBase
    {
        private _IWeekCardAssignItemShowInfo _m_info;

        protected _AGGUIWndWeekCardAssignSubItemBase(Transform _parent) : base(_parent)
        {
            
        }

        public _IWeekCardAssignItemShowInfo info { get => _m_info; }

        protected override void _onShowWnd()
        {
            _onShowWndEx();
        }

        protected abstract void _onShowWndEx();

        protected override void _onHideWnd()
        {
            _onHideWndEx();
        }

        protected abstract void _onHideWndEx();
        

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _onDiscardEx();
        }

        protected abstract void _onDiscardEx();

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            _onWndInitDoneEx();
        }

        protected abstract void _onWndInitDoneEx();

        public void setInfo(_IWeekCardAssignItemShowInfo _info)
        {
            _m_info = _info;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(null == wnd)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, _m_info.getAssignName());
            NPCommonEnumStatInfo<EGameCommonUnlockType>.setStat(wnd.statInfos, _m_info.getUnlockType());
            _onRefreshWnd(_m_info);
        }

        protected abstract void _onRefreshWnd(_IWeekCardAssignItemShowInfo _info);
    }
}
