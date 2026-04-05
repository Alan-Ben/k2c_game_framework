using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 排行榜详情玩家信息附加窗口
    /// </summary>
    public abstract class _AGGUIWndBaseSubRankPlayerInfo<T_Mono, T_Info> : _ANPGGUIBasicGridItemWnd<T_Mono> 
        where T_Mono : GGUIMonoBaseSubRankPlayerInfo
        where T_Info : _ARankCommonShowInfo<T_Info>
    {
        //排行榜信息
        private T_Info _m_rankInfo;
        //玩家头像
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;
        //显示序列号
        private long _m_lShowSerialize;

        public _AGGUIWndBaseSubRankPlayerInfo(T_Mono _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected sealed override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _refreshWnd();
            _onShowWndEx();
        }

        protected sealed override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
            _m_wPlayerIcon?.hideWnd();
            _onHideWndEx();
        }

        protected sealed override void _onReset()
        {
            _m_wPlayerIcon?.resetWnd();
            _onResetEx();
        }

        protected sealed override void _resetGridItem()
        {
            _m_wPlayerIcon?.resetWnd();
            _onResetEx();
        }

        protected sealed override void _onDiscard()
        {
            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onClickDetail);
            _onDiscardEx();
        }

        protected sealed override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);

            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onClickDetail);
            _onWndInitDoneEx();
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public virtual void setInfo(T_Info _info)
        {
            if (wnd == null || _info == null)
                return;

            _m_rankInfo = _info;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshRankInfo();
            _refreshPlayerInfo();
        }

        //刷新排行榜信息
        private void _refreshRankInfo()
        {
            if (wnd == null || _m_rankInfo == null)
                return;

            wnd.setRank(_m_rankInfo.rankSortId);
            if(string.IsNullOrEmpty(wnd.scoreTransKey))
            	ALUGUICommon.setLabelTxt(wnd.txtScore, GCommon.getValueFormatStr(_m_rankInfo.rankScoreFormat, _m_rankInfo.rankScore));
            else
            	ALUGUICommon.setLabelTxt(wnd.txtScore, TextTranslate.instance.getLanguage(wnd.scoreTransKey,GCommon.getValueFormatStr(_m_rankInfo.rankScoreFormat, _m_rankInfo.rankScore)));

            //设置跨服活动时的显隐
            ALUGUICommon.setGameObjEnable(wnd.goCrossShowList, _m_rankInfo.isCross);
            ALUGUICommon.setGameObjEnable(wnd.goCrossHideList, !_m_rankInfo.isCross);
        }

        //刷新玩家信息
        private void _refreshPlayerInfo()
        {
            if (wnd == null || _m_rankInfo == null)
                return;

            bool isSelf = _m_rankInfo.cid == NPPlayer.instance.playerInfo.CID;

            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goSelfShowList, isSelf);
            ALUGUICommon.setGameObjEnable(wnd.goSelfHideList, !isSelf);

            //设置文本颜色
            if (wnd.txtSelfNeedChangeColorList != null)
            {
                for (int i = 0; i < wnd.txtSelfNeedChangeColorList.Count; i++)
                {
                    ALUGUICommon.setUIObjColor(wnd.txtSelfNeedChangeColorList[i], isSelf ? wnd.selfTextColor : wnd.otherTextColor);
                }
            }

            //设置正在请求数据时的显隐
            ALUGUICommon.setGameObjEnable(wnd.goReqInfoHideList, false);
            ALUGUICommon.setGameObjEnable(wnd.goReqInfoShowList, true);

            _m_lShowSerialize = ALSerializeOpMgr.next();
            long serialize = _m_lShowSerialize;
            _m_rankInfo.getInfo(false, _info =>
            {
                if (_m_lShowSerialize != serialize || null == _info || wnd == null || !isShow)
                    return;

                //设置正在请求数据时的显隐
                ALUGUICommon.setGameObjEnable(wnd.goReqInfoHideList, true);
                ALUGUICommon.setGameObjEnable(wnd.goReqInfoShowList, false);

                //设置玩家信息
                _m_wPlayerIcon?.showWnd();
                _m_wPlayerIcon?.setPlayerInfo(_info.playerInfo);
            });
        }

        //点击详情
        private void _onClickDetail(GameObject _go)
        {
            if (null == _m_rankInfo)
                return;

            switch (_m_rankInfo.showType)
            {
                case ERankDetailShowType.PLAYER:
                    //列表是简要信息 所以调用这个打开
                    FriendCommon.showPlayerInfo(_m_rankInfo.cid);
                    break;
                case ERankDetailShowType.HERO:
                    //跳转大臣
                    // NPPlayer.instance.rankCommonComp.reqSomeOnePlayerTopHeroInfo(_m_showInfo.cid, (_info) =>
                    // {
                    //     //数据异常 return
                    //     if (null == _info || _info.getHeroId() == 0 || _m_showInfo == null)
                    //         return;
                    //
                    //     QueueMgr.instance.AddNode(new NPGMainQueueChatShareHeroDetailNode(_info, _m_showInfo.playerInfo));
                    //
                    // });
                    break;
            }
        }

        protected abstract void _onShowWndEx();
        protected abstract void _onHideWndEx();
        protected abstract void _onResetEx();
        protected abstract void _onDiscardEx();
        protected abstract void _onWndInitDoneEx();
    }
}
