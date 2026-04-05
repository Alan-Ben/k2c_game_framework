using System;
using System.Collections.Generic;
using ALPackage;
using Common.TravelEnum;

namespace GOE
{
    /// <summary>
    /// 游历地点事件item窗口接口
    /// </summary>
    public interface _ITravelPosEventItemWnd : _IBasicDiffItemContainerItemWnd
    {
        /// <summary>
        /// 设置事件数据
        /// </summary>
        void setData(TravelEventRefObj _eventRefObj);
    }

    /// <summary>
    /// 游历地点事件item窗口抽象基类
    /// </summary>
    public abstract class _AGGUIWndTravelPosEventItem<T_MONO> : _ANPGGUIBasicSubWnd<T_MONO>, _ITravelPosEventItemWnd
        where T_MONO : _AGGUIMonoTravelPosEventItem
    {
        protected TravelEventRefObj _m_rTravelEventRefObj; // 事件配表数据
        protected TravelEventTypeRefObj _m_rTravelEventTypeRefObj; // 事件类型配表数据

        private NPGGuiWndTexture _m_wEventBanner; // 事件banner
        private NPGGuiWndTexture _m_wEventRoleMid; // 事件角色半身像

        public _AGGUIWndTravelPosEventItem(T_MONO _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (wnd != null)
            {
                // 构建事件banner
                if (wnd.imgEventBanner != null)
                    _m_wEventBanner = new NPGGuiWndTexture(wnd.imgEventBanner);
                // 构建事件角色半身像
                if (wnd.imgEventRoleMid != null)
                    _m_wEventRoleMid = new NPGGuiWndTexture(wnd.imgEventRoleMid);
            }
            _onWndInitDoneSub();
        }

        protected override void _onShowWnd()
        {
            _onShowWndSub();
        }

        protected override void _onHideWnd()
        {
            _m_wEventBanner?.hideWnd();
            _m_wEventRoleMid?.hideWnd();
            _onHideWndSub();
        }

        protected override void _onReset()
        {
            _m_wEventBanner?.discardTexture();
            _m_wEventRoleMid?.discardTexture();
            _onResetSub();
        }

        protected override void _onDiscard()
        {
            _m_wEventBanner?.discard();
            _m_wEventBanner = null;
            _m_wEventRoleMid?.discard();
            _m_wEventRoleMid = null;
            
            _m_rTravelEventRefObj = null;
            _m_rTravelEventTypeRefObj = null;
            _onDiscardSub();
        }


        /// <summary>
        /// 实现 _IBasicDiffItemContainerItemWnd.getWndMono
        /// </summary>
        public _AALBasicUIWndMono getWndMono()
        {
            return wnd;
        }

        /// <summary>
        /// 设置事件数据并刷新
        /// </summary>
        public void setData(TravelEventRefObj _eventRefObj)
        {
            _m_rTravelEventRefObj = _eventRefObj;
            _m_rTravelEventTypeRefObj = _m_rTravelEventRefObj == null ? null : GRefdataCoreMgr.instance.travelEventTypeRefCore.getRef((long) _m_rTravelEventRefObj.eEventType);
            _onSetData(_eventRefObj);
            _refreshWnd();
        }


        /// <summary>
        /// 刷新基础信息（事件名称、事件描述、banner、角色半身像、角色名）
        /// </summary>
        protected void _refreshWnd()
        {
            if (wnd == null || _m_rTravelEventRefObj == null || _m_rTravelEventTypeRefObj == null)
                return;

            // 事件名称
            ALUGUICommon.setLabelTxt(wnd.txtEventName, TextTranslate.instance.getLanguage(_m_rTravelEventTypeRefObj.event_name));
            // 事件描述
            ALUGUICommon.setLabelTxt(wnd.txtEventDesc, TextTranslate.instance.getLanguage(_m_rTravelEventRefObj.event_info_desc));

            if(_m_wEventBanner != null)
            {
                _m_wEventBanner.showWnd();
                _m_wEventBanner.setTexture(_m_rTravelEventTypeRefObj.event_banner);
            }
            
            // 刷新事件角色信息（半身像、角色名）
            _refreshEventRole();

            _onRefreshWnd();
        }

        /// <summary>
        /// 刷新事件角色信息（半身像、角色名）
        /// </summary>
        private void _refreshEventRole()
        {
            TravelEventRoleConfig target = _m_rTravelEventRefObj?.eventTarget;
            if (target == null)
                return;

            // 角色名
            if (wnd.txtEventRoleName != null)
            {
                string roleName = target.getRoleName();
                ALUGUICommon.setLabelTxt(wnd.txtEventRoleName, TextTranslate.instance.getLanguage(roleName));
            }

            // 角色半身像
            _ITravelEventRole roleInfo = target.roleInfo;
            if (roleInfo != null && _m_wEventRoleMid != null)
            {
                _m_wEventRoleMid.showWnd();
                _m_wEventRoleMid.setTexture(roleInfo.midImage);
            }
        }


        protected abstract void _onWndInitDoneSub();
        protected abstract void _onDiscardSub();
        protected abstract void _onShowWndSub();
        protected abstract void _onHideWndSub();
        protected abstract void _onResetSub();
        protected abstract void _onSetData(TravelEventRefObj _eventRefObj);
        protected abstract void _onRefreshWnd();
    }
}
