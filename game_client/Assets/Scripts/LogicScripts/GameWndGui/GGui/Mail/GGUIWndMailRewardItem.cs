using UnityEngine;
using System.Collections.Generic;
using ALPackage;
using System;
using System.Text;

namespace GOE
{
    public class GGUIWndMailRewardItem : _ANPGGUIBasicSubWnd<GGUIMonoMailRewardItem>
    {
        /// <summary>
        /// 奖励itemWnd
        /// </summary>
        private NPGGUIWndCommonItem _m_itemWnd;

        public GGUIWndMailRewardItem(GGUIMonoMailRewardItem _wnd)
            : base(_wnd)
        {
            if (null != _wnd.item)
            {
                _m_itemWnd = new NPGGUIWndCommonItem(_wnd.item);
            }
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
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

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
        }

        /// <summary>
        /// 设置item数据
        /// </summary>
        /// <param name="_mHasGet">是否已经领取</param>
        /// <param name="_itemData"></param>
        public void showItem(bool _mHasGet, _IItem _itemData)
        {
            if (null == wnd)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goShowOnRewardGet, _mHasGet);
            if (null != _m_itemWnd)
                _m_itemWnd.showWnd(_itemData);
        }
    }
}

