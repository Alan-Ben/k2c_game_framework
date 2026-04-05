using System;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 每个目标item
    /// </summary>
    public class GGUISubWndCommonTargetRewardItem : _ATNPBasicSimpleUISubWnd<GGUIMonoCommonTargetRewardItem>
    {
        //对应数据
        private CommonTargetRewardInfo _m_info;
        private int _m_iInitPosIndex;//初始位置索引
        
        public GGUISubWndCommonTargetRewardItem(GGUIMonoCommonTargetRewardItem _wnd, int _initIndex) : base(_wnd)
        {
            _m_iInitPosIndex = _initIndex;
            initWnd();
        }
        public event Action<GGUISubWndCommonTargetRewardItem> onClick;
        public CommonTargetRewardInfo info { get { return _m_info; } }
        public int initIndex { get { return _m_iInitPosIndex; } }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            _m_info = NPPlayer.instance.commonTargetRewardComp.getInfoById(wnd.id);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClick);
        }
        
        protected override void _onDiscard()
        {
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
        
        private void _onClick(GameObject _go)
        {
            if (onClick != null) 
                onClick.Invoke(this);
        }

        //是否满足显示条件
        public bool isShowConditionMet()
        {
            if (null == _m_info)
                return true;
            return _m_info.isShowConditionMet();
        }

        //获取数据状态
        public ECommonRewardType getRewardType()
        {
            if (null == _m_info)
                return ECommonRewardType.NONE;

            return _m_info.getRewardType();
        }
        
        //设置选中状态
        public void setIsSelected(bool _isSelected)
        {
            if (null == wnd)
                return;

            ALUGUICommon.setGameObjEnable(wnd.selectShowGoList, _isSelected);
        }

        public void refresh()
        {
            if(null == wnd)
                return;
            
            NPCommonEnumStatInfo<ECommonRewardType>.setStat(wnd.rewardStatList, getRewardType());
        }
    }
}