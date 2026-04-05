using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 通用奖励物品item（带锁定状态）
    public class GGUIWndCommonLockRewardContainerItem : _ATALBasicUISubWnd<GGUIMonoCommonLockRewardContainerItem>
    {
        private NPGGUIWndCommonItem _m_itemWnd;
        public GGUIWndCommonLockRewardContainerItem(GGUIMonoCommonLockRewardContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        // 初始化
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (null != _m_monoWnd)
                _m_itemWnd = new NPGGUIWndCommonItem(wnd.itemMono);
        }
        protected override void _onShowWnd()
        {
            if (null != _m_itemWnd)
                _m_itemWnd.showWnd();
        }
        protected override void _onHideWnd()
        {
            if (null != _m_itemWnd)
                _m_itemWnd.hideWnd();
        }

        protected override void _onReset()
        {
            if (null != _m_itemWnd)
                _m_itemWnd.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (null != _m_itemWnd)
                _m_itemWnd.discard();
            _m_itemWnd = null;
        }

        public void setItem(_IItem _item,ECommonLockRewardType _type = ECommonLockRewardType.NONE)
        {
            if (null != _m_itemWnd)
                _m_itemWnd.setItem(_item);

            if(_type != ECommonLockRewardType.NONE)
            {
                NPCommonEnumStatInfo<ECommonLockRewardType>.setStat(wnd.rewardStatList, _type);
            }
        }
    }
}
