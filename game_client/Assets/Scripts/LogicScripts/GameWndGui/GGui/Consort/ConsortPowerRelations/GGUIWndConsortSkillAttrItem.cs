using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 属性item
    /// </summary>
    public class GGUIWndConsortSkillAttrItem : _ATALBasicUISubWnd<GGUIMonoConsortSkillAttrItem>
    {
        private CommonAttrItemInfo _m_attrItem;
        private GGUIWndCommonAttrItem _m_attrWnd;

        public GGUIWndConsortSkillAttrItem(GGUIMonoConsortSkillAttrItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
            
        }

        protected override void _onDiscard()
        {
            _m_attrWnd?.discard();
            _m_attrWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            if (null != wnd.attrItem)
            {
                _m_attrWnd = new GGUIWndCommonAttrItem(wnd.attrItem);
            }
        }

        public void setItemGray(bool _isGray)
        {
            
            if (_isGray)
            {
                GGameCommonInfo.grayImage(wnd.goLockGray);
            }
            else
            {
                GGameCommonInfo.disgrayImage(wnd.goLockGray);
            }
        }

        public void setInfo(CommonAttrItemInfo _attrItem)
        {
            _m_attrItem = _attrItem;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if (null != _m_attrWnd)
            {
                _m_attrWnd.showWnd();
                _m_attrWnd.setInfo(_m_attrItem);
            }
        }
    }
}