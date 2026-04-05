
using ALPackage;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 奖励预览弹窗
    /// </summary>
    public class GGUIWndCommonItemToolTip_Reward : _ATNPGGUIWndCommonItemToolTip<GGUIMonoCommonToolTip_Reward>
    {
        private GGUIWndCommonRewardContainer _m_wRewardContainer;

        public GGUIWndCommonItemToolTip_Reward(string _assetPath, string _assetName) : base(_assetPath, _assetName)
        {
        }

        protected override void _onShowWnd()
        {
            base._onShowWnd();

            if (_m_wRewardContainer != null)
                _m_wRewardContainer.showWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
            if (_m_wRewardContainer != null)
                _m_wRewardContainer.hideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
            if (_m_wRewardContainer != null)
                _m_wRewardContainer.resetWnd();

        }

        protected override void _onDiscard()
        {
            base._onDiscard();

            if (_m_wRewardContainer != null)
                _m_wRewardContainer.discard();
            _m_wRewardContainer = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();

            if (null == wnd)
                return;

            if (wnd.rewardContainer != null)
                _m_wRewardContainer = new GGUIWndCommonRewardContainer(wnd.rewardContainer);
        }

        protected override void _onClose()
        {
            QueueMgr.instance.forceCloseNodeByType(typeof(GNodeCommonToolTip_Reward));
        }

        public void setData(string _titleStr, string _contentStr, List<NPCommonCostItem> _list, RectTransform _targetTransRoot, Vector2 _interval, ECommonRewardType _type = ECommonRewardType.NONE)
        {
            if (_m_wRewardContainer != null)
            {
                _m_wRewardContainer.setRewardList(_list, _type);
                _m_wRewardContainer.showWnd();
            }

            ALUGUICommon.setLabelTxt(wnd.titleTxt, _titleStr);
            ALUGUICommon.setLabelTxt(wnd.contentTxt, _contentStr);
            
            //设置位置
            setPos(_targetTransRoot, _interval.x,_interval.y);
        }
    }
}