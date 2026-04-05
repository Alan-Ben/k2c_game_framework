using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 进度奖励item
    /// </summary>
    public class GGUIWndCommonRewardSliderItemPro : _ATALBasicUISubWnd<GGUIMonoCommonRewardSliderItemPro>
    {
        //奖励信息
        private _ISliderRewardItemInfoPro _m_info;
        //点击事件
        private Action<GGUIWndCommonRewardSliderItemPro> _m_aOnClickItem;
        //领奖状态变化
        private Action<GGUIWndCommonRewardSliderItemPro> _m_aOnRewardStateChg;
        //上个状态
        private ESliderRewardState _m_eLastState;
        //音效实例id
        private long _m_lAudioInstanceId;
        //宝箱图标
        private NPGGuiWndTexture _m_wBoxIcon;
        // 展示奖励item
        private NPGGUIWndCommonItem _m_wShowRewardItem;

        /// <summary>
        /// 点击事件
        /// </summary>
        public Action<GGUIWndCommonRewardSliderItemPro> onClickItem
        {
            get { return _m_aOnClickItem; }
            set { _m_aOnClickItem = value; }
        }
        /// <summary>
        /// 领奖状态变化
        /// </summary>
        public Action<GGUIWndCommonRewardSliderItemPro> onRewardStateChg
        {
            get { return _m_aOnRewardStateChg; }
            set { _m_aOnRewardStateChg = value; }
        }

        /// <summary>
        /// 奖励信息
        /// </summary>
        public _ISliderRewardItemInfoPro itemInfo { get { return _m_info; } }

        /// <summary>
        /// 上个状态
        /// </summary>
        public ESliderRewardState lastState
        {
            get { return _m_eLastState; }
        }

        public GGUIWndCommonRewardSliderItemPro(GGUIMonoCommonRewardSliderItemPro _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            refreshState();
        }

        protected override void _onHideWnd()
        {
            if (_m_lAudioInstanceId > 0)
                PlayAudioMgr.instance.stopClip(_m_lAudioInstanceId);

            _m_wBoxIcon?.hideWnd();
            _m_wShowRewardItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBoxIcon?.discardTexture();
            _m_wShowRewardItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wBoxIcon?.discard();
            _m_wBoxIcon = null;

            _m_wShowRewardItem?.discard();
            _m_wShowRewardItem = null;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickItem);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgBoxIcon != null)
                _m_wBoxIcon = new NPGGuiWndTexture(wnd.imgBoxIcon);

            if(wnd.monoShowRewardItem != null)
                _m_wShowRewardItem = new NPGGUIWndCommonItem(wnd.monoShowRewardItem);
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(_ISliderRewardItemInfoPro _info)
        {
            if (_info == null)
                return;

            _m_info = _info;
            _m_eLastState = _m_info.rewardState;

            //设置初始状态
            if (wnd != null && wnd.stateAniList != null)
                wnd.stateAniList.forcePlay(_m_info.rewardState);

            //设置状态
            refreshState();
        }

        /// <summary>
        /// 刷新状态
        /// </summary>
        public void refreshState()
        {
            if (wnd == null || _m_info == null)
                return;

            string scoreStr = string.IsNullOrEmpty(_m_info.showScore) ? 
                GCommon.getValueFormatStr(_m_info.valueFormatType, _m_info.score) : _m_info.showScore;

            long testlong = 1;
            float testFloat = testlong;
            
            //设置分数
            ALUGUICommon.setLabelTxt(wnd.txtScore, scoreStr);

            //播放可领取音效
            if (_m_eLastState == ESliderRewardState.CAN_NOT_GET && _m_info.rewardState == ESliderRewardState.CAN_GET)
            {
                if(_m_lAudioInstanceId > 0)
                    PlayAudioMgr.instance.stopClip(_m_lAudioInstanceId);

                if(wnd.canGetAudioId > 0)
                    _m_lAudioInstanceId = PlayAudioMgr.instance.playClip(wnd.canGetAudioId);
            }

            //领奖状态变化
            if (_m_eLastState != _m_info.rewardState)
            {
                if (wnd.stateAniList != null)
                    wnd.stateAniList.forcePlay(_m_info.rewardState);
                _m_aOnRewardStateChg?.Invoke(this);
            }
            _m_eLastState = _m_info.rewardState;

            //刷新图标
            if (_m_wBoxIcon != null)
            {
                _m_wBoxIcon.showWnd();
                if (_m_info.icon == null)
                    _m_wBoxIcon.setTexture(_m_info.rewardState == ESliderRewardState.ALREADY_GET ? wnd.alreadyGetBoxIcon : wnd.notGetBoxIcon);
                else
                    _m_wBoxIcon.setTexture(_m_info.icon);
            }

            if (_m_wShowRewardItem != null)
            {
                if (_m_info.showRewardItem == null)
                {
                    _m_wShowRewardItem.hideWnd();
                }
                else
                {
                    _m_wShowRewardItem.showWnd();
                    _m_wShowRewardItem.setItem(_m_info.showRewardItem);
                }
            }
        }

        public void showRewardItemDetail(GameObject _go)
        {
            if(_m_wShowRewardItem != null)
                _m_wShowRewardItem.showItemDetail(_go);
        }
        
        //点击事件
        private void _onClickItem(GameObject _go)
        {
            if (_m_aOnClickItem != null)
                _m_aOnClickItem(this);
        }
    }
}
