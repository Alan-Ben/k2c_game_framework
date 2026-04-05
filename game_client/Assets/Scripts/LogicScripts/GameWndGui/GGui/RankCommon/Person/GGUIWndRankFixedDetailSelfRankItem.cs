using ALPackage;
using CommonEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 详情排行榜item
    /// </summary>
    public class GGUIWndRankFixedDetailSelfRankItem : _ATALBasicUISubWnd<GGUIMonoRankFixedDetailSelfRankItem>
    {
        //排行榜信息
        private NPRankCommonShowInfo _m_rankInfo;
        //玩家头像
        private NPGGUIWndPlayerIcon _m_wPlayerIcon;

        public GGUIWndRankFixedDetailSelfRankItem(GGUIMonoRankFixedDetailSelfRankItem  _wnd)
            : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wPlayerIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wPlayerIcon?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wPlayerIcon?.discard();
            _m_wPlayerIcon = null;

            if (null == wnd)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onClickDetail);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if(wnd.monoPlayerIcon != null)
                _m_wPlayerIcon = new NPGGUIWndPlayerIcon(wnd.monoPlayerIcon);

            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onClickDetail);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(NPRankCommonShowInfo _info)
        {
            _m_rankInfo = _info;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            _m_wPlayerIcon?.showWnd();
            _m_wPlayerIcon?.setSelfInfo();

            if (_m_rankInfo == null)
            {
                ALUGUICommon.setGameObjEnable(wnd.goInRankShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.goNotInRankShowGoList, true);
            }
            else
            {
                NPRankRefObj rankRefObj = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(_m_rankInfo.rankId);
                ALUGUICommon.setLabelTxt(wnd.txtRank, _m_rankInfo.rankSortId);
                if (rankRefObj != null)
                    ALUGUICommon.setLabelTxt(wnd.txtScore, GCommon.getValueFormatStr(rankRefObj.process_num_format, _m_rankInfo.rankScore));

                bool isInRank = _m_rankInfo.rankSortId != 0;
                ALUGUICommon.setGameObjEnable(wnd.goInRankShowGoList, isInRank);
                ALUGUICommon.setGameObjEnable(wnd.goNotInRankShowGoList, !isInRank);
            }
        }

        //点击详情
        private void _onClickDetail(GameObject _go)
        {
            if (null == _m_rankInfo)
                return;

            NPRankRefObj refObj = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(_m_rankInfo.rankId);
            if (null == refObj)
                return;

            switch (refObj.show_type)
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
    }
}
