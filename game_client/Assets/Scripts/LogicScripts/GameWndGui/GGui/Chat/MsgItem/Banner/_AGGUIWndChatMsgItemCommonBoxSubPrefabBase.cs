using System.Collections.Generic;
using ALPackage;
using Common.NpChatObj;
using GS2GC.p004_PlayerOp;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 通用宝箱信息接口
    /// </summary>
    public interface _IChatMsgItemCommonBoxShowInfo
    {
        /// <summary>
        /// 宝箱实例ID
        /// </summary>
        long boxInstanceId { get; }
        /// <summary>
        /// 宝箱配置数据
        /// </summary>
        NPSOCommonBoxRefObj boxRefObj { get; }
        /// <summary>
        /// 发送者信息
        /// </summary>
        NPCommon_ChatPlayerContent sender { get; }

        /// <summary>
        /// 聊天显示窗里显示的内容
        /// </summary>
        /// <returns></returns>
        string getMiniContent();
    }

    /// <summary>
    /// 通用宝箱分享banner附加窗口基类
    /// </summary>
    public abstract class _AGGUIWndChatMsgItemCommonBoxSubPrefabBase<T> : _ATALBasicLoadPrefabSubUIWnd<T> where T: NPGGUIMonoChatMsgItemCommonBoxSubPrefab
    {
        //资源id
        private long _m_uiPathId;
        //宝箱状态
        private ENPBoxChatStatus _m_eBoxState;
        //已领取玩家cid列表
        private List<long> _m_lGainedCidList;
        //宝箱消息详情
        private _IChatMsgItemCommonBoxShowInfo _m_detailInfo;
        //玩家头像
        private NPGGUIWndPlayerIcon _m_playerInfoWnd;
        // banner图标
        private NPGGuiWndTexture _m_wBannerIcon;
        //显示序列号
        private long _m_lShowSerialize;

        public _AGGUIWndChatMsgItemCommonBoxSubPrefabBase(long _uiPathId, Transform _parent) : base(_parent)
	    {
	        _m_uiPathId = _uiPathId;
	    }

	    protected override string _monoAssetPath { get => UIResPathAssistant.getAssetPath(_m_uiPathId); }
	    protected override string _monoObjName { get => UIResPathAssistant.getObjName(_m_uiPathId); }
	    protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }

	    protected sealed override void _onShowWnd()
	    {           
            _m_lShowSerialize = ALSerializeOpMgr.next();
            if (_m_playerInfoWnd != null)
            {
                _refreshPlayerInfo();
            }
            _onShowWndEx();
        }

	    protected sealed override void _onHideWnd()
	    {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_playerInfoWnd?.hideWnd();
            _m_wBannerIcon?.hideWnd();
            _onHideWndEx();
        }

	    protected sealed override void _onReset()
	    {
            _m_playerInfoWnd?.resetWnd();
            _m_wBannerIcon?.discardShowTexture();
            _onResetEx();
        }

	    protected sealed override void _onDiscard()
	    {     
            if (_m_playerInfoWnd != null)
            {
                _m_playerInfoWnd.onClickAction -= _onClickPlayerInfo;
                _m_playerInfoWnd.discard();
            }
            _m_playerInfoWnd = null;
            _m_wBannerIcon?.discard();
            _m_wBannerIcon = null;

            _m_eBoxState = ENPBoxChatStatus.NONE;
            _m_lGainedCidList?.Clear();
            ALUGUICommon.uncombineBtnClick(wnd.btnClick, _onClickBtn);
            _onDiscardEx();
        }

	    protected sealed override void _onWndInitDone()
	    {
            //玩家信息
            if (wnd.monoPlayer != null)
            {
                _m_playerInfoWnd = new NPGGUIWndPlayerIcon(wnd.monoPlayer);
                _m_playerInfoWnd.onClickAction += _onClickPlayerInfo;
            }

            if(wnd.imgBannerIcon !=  null)
                _m_wBannerIcon = new NPGGuiWndTexture(wnd.imgBannerIcon);

            _m_eBoxState = ENPBoxChatStatus.NONE;
            ALUGUICommon.combineBtnClick(wnd.btnClick, _onClickBtn);
            _onWndInitDoneEx();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_boxInfo"></param>
        protected void setBaseInfo(_IChatMsgItemCommonBoxShowInfo _boxInfo)
        {
            if (_boxInfo == null)
                return;

            _m_detailInfo = _boxInfo;
            //可领取或者达到上限状态时请求刷新状态
            if (_m_eBoxState == ENPBoxChatStatus.NONE || _m_eBoxState == ENPBoxChatStatus.IS_LIMIT)
            {
                NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_024_ReqRefreshChatBoxStatus(_m_detailInfo.boxInstanceId),
                    new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_024_RetRefreshChatBoxStatus>((_msg) =>
                    {
                        if (_msg != null && _msg.getInstanceId() == _m_detailInfo.boxInstanceId)
                        {
                            _m_eBoxState = _msg.getBoxStatus();
                            _m_lGainedCidList = _msg.getGainedCidList();
                            _refreshWnd();
                        }
                    }));
            }
            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            _refreshPlayerInfo();
            _refreshLeftCount();
            _refreshBoxState();
            _refreshBanner();
            _onRefreshWnd();
        }

        /// <summary>
        /// 刷新发送玩家信息
        /// </summary>
        private void _refreshPlayerInfo()
        {
            if (_m_detailInfo == null) 
                return;

            //获取cid
            long cid = _getSenderCid();
            //如果有发送者的信息并且获取到的cid是发送者的cid，直接展示发送者，否则使用cid去请求展示
            if (cid > 0 && _m_detailInfo.sender != null && _m_detailInfo.sender.getCid() == cid)
            {
                _m_playerInfoWnd?.showWnd();
                _m_playerInfoWnd?.setPlayerInfo(_m_detailInfo.sender);
            }
            else if(cid > 0)
            {
                _m_playerInfoWnd?.showWnd();
                _m_playerInfoWnd?.setPlayer(_getSenderCid());
            }
            else
                _m_playerInfoWnd?.hideWnd();
        }

        /// <summary>
        /// 刷新剩余次数
        /// </summary>
        private void _refreshLeftCount()
        {
            if (wnd == null || _m_detailInfo == null || _m_detailInfo.boxRefObj == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_detailInfo.boxRefObj.name)); 
            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(_m_detailInfo.getMiniContent())); 
            long alreadyGetCount = _m_lGainedCidList == null ? 0 : _m_lGainedCidList.Count;
            //宝箱剩余：{0}/{1}
            ALUGUICommon.setLabelTxt(wnd.txtLeftCount, TextTranslate.instance.getLanguage(TransKeyConst.common_currentTotalNum_num_num, _m_detailInfo.boxRefObj.box_limit - alreadyGetCount, _m_detailInfo.boxRefObj.box_limit));
        }

        /// <summary>
        /// 刷新宝箱状态
        /// </summary>
        private void _refreshBoxState()
        {
            if (wnd == null || wnd.msgStatusShowParams == null)
                return;

            NPGGUIBoxShareMsgShowParam tempState = null;
            for (int i = 0; i < wnd.msgStatusShowParams.Count; i++)
            {
                if (wnd.msgStatusShowParams[i] == null)
                    continue;

                if (_m_eBoxState == wnd.msgStatusShowParams[i].status)
                    tempState = wnd.msgStatusShowParams[i];
                else
                    ALUGUICommon.setGameObjEnable(wnd.msgStatusShowParams[i].goList, false);
            }

            if (tempState != null)
                ALUGUICommon.setGameObjEnable(tempState.goList, true);
        }

        /// <summary>
        /// 刷新Banner图标
        /// </summary>
        private void _refreshBanner()
        {
            if (_m_detailInfo == null)
                return;

            _m_wBannerIcon?.showWnd();
            _m_wBannerIcon?.setTexture(_m_detailInfo.boxRefObj?.icon);
        }
        
        /// <summary>
        /// 展示提示
        /// </summary>
        /// <param name="_showIsGain"></param>
        private void _showTip(bool _showIsGain)
        {
            switch (_m_eBoxState)
            {
                case ENPBoxChatStatus.IS_INVAILD://已失效
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chat_getBoxRewardInvalid_none);
                    break;
                case ENPBoxChatStatus.IS_LIMIT://达到上限
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chat_getBoxRewardLimit_none);
                    break;
                case ENPBoxChatStatus.IS_EMPTY://已领完
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chat_getBoxRewardEmpty_none);
                    break;
                case ENPBoxChatStatus.IS_GAINED://已领取
                    if(_showIsGain)
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chat_getBoxRewardGain_none);
                    break;
            }
        }
        
        #region 点击事件

        /// <summary>
        /// 点击玩家头像
        /// </summary>
        private void _onClickPlayerInfo()
        {
            long serialize = _m_lShowSerialize;
            //打开玩家详情
            if (_m_detailInfo == null) 
                return;

            long cid = _getSenderCid();
            if (cid <= 0)
                return;

            GCommon.reqPlayerInfo(cid, (_info) =>
            {
                if (wnd == null || null == getGameObj() || !getGameObj().activeInHierarchy ||
                    _m_playerInfoWnd == null || _m_playerInfoWnd.wnd == null || _info == null ||
                    _m_lShowSerialize != serialize)
                    return;

                RectTransform targetRectTransform = (null != _m_playerInfoWnd.wnd.locateRectTrans)
                    ? _m_playerInfoWnd.wnd.locateRectTrans
                    : this.rectTransform;

                WinMsg.SendMsg(WinMsgType.ON_TO_SHOW_ALL_ITEM, targetRectTransform);

                RectTransform rangTrans = GGUIWndChat.instance.getChatInfoListViewport();
                GCommon.showPlayerInfoWndTip(_info, targetRectTransform, 0, rangTrans);
            });
        }

        /// <summary>
        /// 点击按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtn(GameObject _go)
        {
            if (_m_detailInfo == null)
                return;

            switch (_m_eBoxState)
            {
                case ENPBoxChatStatus.NONE://可领取
                case ENPBoxChatStatus.IS_LIMIT://达到上限(可能跨天了可以领取，本地状态还没刷新)
                    NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_023_ReqGainChatBox(_m_detailInfo.boxInstanceId),
                        new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_023_RetGainChatBox>((_msg) =>
                        {
                            if (_msg != null && _msg.getInstanceId() == _m_detailInfo.boxInstanceId)
                            {
                                _m_eBoxState = _msg.getBoxStatus();
                                _m_lGainedCidList = _msg.getGainedCidList();
                                _refreshWnd();
                                _showTip(false);
                            }
                        }));
                    break;
                default:
                    _showTip(true);
                    break;
            }
        }

        #endregion

        /// <summary>
        /// 获取发送者cid，默认从detailInfo里获取，如果没有则返回0，子类可重写从其他途径获取
        /// </summary>
        protected virtual long _getSenderCid()
        {
            return _m_detailInfo != null && _m_detailInfo.sender != null ? _m_detailInfo.sender.getCid() : 0;
        }

        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();
        protected virtual void _onRefreshWnd(){}
    }
}