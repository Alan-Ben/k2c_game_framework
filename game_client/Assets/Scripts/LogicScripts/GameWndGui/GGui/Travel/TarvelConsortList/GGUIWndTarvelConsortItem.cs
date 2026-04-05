using System;
using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTarvelConsortItem: _ANPGGUIBasicSubWnd<GGUIMonoTarvelConsortItem>
    {
        private _IConsortShowInfo _m_consortShowInfo;
        private ETravelConsortUnlockStat _m_eTravelConsortUnlockStat;

        private NPGGUIWndProgress _m_likeProgress;
        private GGUIWndConsortCardItem _m_consortCardItem;

        public GGUIWndTarvelConsortItem(GGUIMonoTarvelConsortItem _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        public _IConsortShowInfo consortShowInfo => _m_consortShowInfo;
        public ETravelConsortUnlockStat travelConsortUnlockStat => _m_eTravelConsortUnlockStat;

        /// <summary>
        /// item被点击回调
        /// </summary>
        public event Action<GGUIWndTarvelConsortItem> onItemClick;

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_likeProgress?.hideWnd();
            _m_consortCardItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_likeProgress?.resetWnd();
            _m_consortCardItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            onItemClick = null;
            
            _m_likeProgress?.discard();
            _m_likeProgress = null;
            
            _m_consortCardItem?.discard();
            _m_consortCardItem = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            
            if (null != wnd.consortItem)
            {
                _m_consortCardItem = new GGUIWndConsortCardItem(wnd.consortItem, _onClickConsortItem);
            }

            if (wnd.monoLikeProgress != null)
            {
                _m_likeProgress = new NPGGUIWndProgress(wnd.monoLikeProgress);
            }
        }

        private void _onClickConsortItem(_IConsortShowInfo obj)
        {
            onItemClick?.Invoke(this);
        }

        /// <summary>
        /// 设置显示信息
        /// </summary>
        /// <param name="_consortShowInfo"></param>
        public void setInfo(_IConsortShowInfo _consortShowInfo, ETravelConsortUnlockStat _travelConsortUnlockStat)
        {
            _m_consortShowInfo = _consortShowInfo;
            _m_eTravelConsortUnlockStat = _travelConsortUnlockStat;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd || _m_consortShowInfo == null)
                return;

            if (null != _m_consortCardItem)
            {
                _m_consortCardItem.showWnd();
                _m_consortCardItem.setInfo(_m_consortShowInfo);
            }

            TravelPosRefObj travelPosRefObj = GRefdataCoreMgr.instance.travelPosCore.getRef(_m_consortShowInfo.consortRefObj?.travel_pos ?? 0);
            if (string.IsNullOrEmpty(wnd.txtTravelPosKey))
            {
                ALUGUICommon.setLabelTxt(wnd.txtTravelPos, TextTranslate.instance.getLanguage(travelPosRefObj?.name));
            }
            else
            {
                ALUGUICommon.setLabelTxt(wnd.txtTravelPos, TextTranslate.instance.getLanguage(wnd.txtTravelPosKey, travelPosRefObj?.name ?? ""));
            }

            TravelConsortInfo travelConsortInfo = NPPlayer.instance.travelComp.getTravelConsortInfo(_m_consortShowInfo.consortId);
            TravelConsortRefObj travelConsortRefObj = travelConsortInfo?.travelConsortRefObj ?? GRefdataCoreMgr.instance.travelConsortRefCore.getRef(_m_consortShowInfo.consortId);

            if (_m_likeProgress != null && travelConsortRefObj != null && travelConsortRefObj.marry_need_like > 0)
            {
                _m_likeProgress.showWnd();
                string likeKey = string.IsNullOrEmpty(wnd.likeProgressKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.likeProgressKey;
                _m_likeProgress.setProgress(travelConsortInfo?.like ?? 0, travelConsortRefObj.marry_need_like, EValueFormatType.NORMAL_NOT_LARGE_STR, likeKey);
            }
            
            // 获取游历妃子状态
            NPCommonEnumStatInfo<ETravelConsortUnlockStat>.setStat(wnd.statInfos, _m_eTravelConsortUnlockStat);
        }
    }
}