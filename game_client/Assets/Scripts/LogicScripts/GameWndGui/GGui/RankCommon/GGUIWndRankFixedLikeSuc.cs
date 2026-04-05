using ALPackage;
using System;
using UnityEngine;
using Common.RankObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 排行榜点赞成功展示
    /// </summary>
    public class GGUIWndRankFixedLikeSuc : _ANPGGUIBasicWnd<GGUIMonoRankFixedLikeSuc>
    {
        private static GGUIWndRankFixedLikeSuc _g_instance = new GGUIWndRankFixedLikeSuc();
        public static GGUIWndRankFixedLikeSuc instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndRankFixedLikeSuc();
                return _g_instance;
            }
        }

        //点赞结果数据
        private RankFixed_LikeResult _m_likeResult;
        //奖励列表
        private NPGGUIWndCommonItemContainer _m_rewardContainerWnd;
        //形象
        private NPGGUIWndCommonShowCase _m_showcaseWnd;
        //请求刷新操作序列号
        private long _m_lRefreshSerialize;
        //关闭回调
        private Action _m_closeAction;

        public GGUIWndRankFixedLikeSuc() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoRankFixedLikeSuc.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoRankFixedLikeSuc.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }



        protected override void _onShowWnd()
        {
            _m_rewardContainerWnd?.showWnd();
            _m_showcaseWnd?.showWnd();
        }

        protected override void _onHideWnd()
        {
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
            _m_rewardContainerWnd?.hideWnd();
            _m_showcaseWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_rewardContainerWnd?.resetWnd();
            _m_showcaseWnd?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_rewardContainerWnd?.discard();
            _m_rewardContainerWnd = null;
            _m_showcaseWnd?.discard();
            _m_showcaseWnd = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _onClickCloseBtn);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (null != wnd.rewardContainerMono)
                _m_rewardContainerWnd = new NPGGUIWndCommonItemContainer(wnd.rewardContainerMono);

            if (null != wnd.monoShowcase)
                _m_showcaseWnd = new NPGGUIWndCommonShowCase(wnd.monoShowcase);

            ALUGUICommon.combineBtnClick(wnd.closeBtn, _onClickCloseBtn);
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="_likeResult"></param>
        /// <param name="_closeAction"></param>
        public void setData(RankFixed_LikeResult _likeResult, Action _closeAction)
        {
            _m_likeResult = _likeResult;
            _m_closeAction = _closeAction;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshReward();
            _refreshRankInfo();
            _refreshPlayerInfo();
        }

        //刷新奖励列表
        private void _refreshReward()
        {
            if (_m_likeResult == null)
                return;

            _m_rewardContainerWnd?.showWnd();
            _m_rewardContainerWnd?.showItemList(_m_likeResult.getRewardItemList().toItemDataList());
        }

        //刷新排行榜信息
        private void _refreshRankInfo()
        {
            if (wnd == null || _m_likeResult == null || _m_likeResult.getInfo() == null)
                return;

            NPRankFixedRefObj rankFixedRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(_m_likeResult.getRankFixedId());
            if (null == rankFixedRef)
                return;

            NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(rankFixedRef.rank_id);
            if (null == rankRef)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtRankName, rankRef.nameStr);
            ALUGUICommon.setLabelTxt(wnd.txtRank, _m_likeResult.getInfo().getRank());
            ALUGUICommon.setLabelTxt(wnd.txtScore, TextTranslate.instance.getLanguage(TransKeyConst.common_str_colon_str, 
                TextTranslate.instance.getLanguage(rankRef.score_name),
                GCommon.getValueFormatStr(rankRef.process_num_format, _m_likeResult.getInfo().getScore())));
        }

        //刷新玩家信息
        private void _refreshPlayerInfo()
        {
            if (wnd == null || _m_likeResult == null || _m_likeResult.getInfo() == null)
                return;

            NPRankFixedRefObj rankFixedRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(_m_likeResult.getRankFixedId());
            if (null == rankFixedRef)
                return;

            NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(rankFixedRef.rank_id);
            if (null == rankRef)
                return;

            ALUGUICommon.setGameObjEnable(wnd.goReqDataHideList, false);
            ALUGUICommon.setLabelTxt(wnd.txtPlayerName, "");

            switch (rankRef.rank_type)
            {
                case ERankType.PLAYER:
                    _reqPlayerInfo(_m_likeResult.getInfo().getKey());
                    break;
                case ERankType.GUILD:
                    _reqGuildInfo(_m_likeResult.getInfo().getKey());
                    break;
            }
        }

        //请求玩家信息
        private void _reqPlayerInfo(long _cid)
        {
            if (wnd == null)
                return;

            _m_lRefreshSerialize = ALSerializeOpMgr.next();
            long opSerialize = _m_lRefreshSerialize;
            //请求玩家详细信息
            GCommon.reqPlayerInfo(_cid, (_info) =>
            {
                //判断操作序列号是否一致
                if (opSerialize != _m_lRefreshSerialize || null == wnd || null == _info)
                    return;

                ALUGUICommon.setGameObjEnable(wnd.goReqDataHideList, true);

                //设置形象展示
                if (_info.skinRef != null)
                    _m_showcaseWnd?.showWnd(new ShowCaseCommonResUnitInfoObj(_info.skinRef.td_show));

                //设置名称
                NPRankFixedRefObj rankFixedRef = GRefdataCoreMgr.instance.rankFixedRefCore.getRef(_m_likeResult.getRankFixedId());
                NPRankRefObj rankRef = GRefdataCoreMgr.instance.rankCommonRefCore.getRef(rankFixedRef != null ? rankFixedRef.rank_id : 0);
                if (null == rankRef)
                    return;
                switch (rankRef.rank_type)
                {
                    case ERankType.PLAYER:
                        ALUGUICommon.setLabelTxt(wnd.txtPlayerName, _info.name);
                        break;
                    case ERankType.GUILD:
                        string guildName = TextTranslate.instance.getLanguage(TransKeyConst.guild_showName_simpleName_name, _info.guildSimpleName, _info.guildName);
                        ALUGUICommon.setLabelTxt(wnd.txtPlayerName, guildName);
                        break;
                }
            });

            //设置随机文本
            ALUGUICommon.setLabelTxt(wnd.txtRandom, TextTranslate.instance.getLanguage(GRefdataCoreMgr.instance.npGeneral.rank_like_suc_random_str_list.GetRandomItem()));
        }

        //请求联盟信息
        private void _reqGuildInfo(long _guildId)
        {
            NPPlayer.instance.guildComp.reqOtherGuildInfo(_guildId, info =>
            {
                if (wnd == null || info == null || info.getShowInfo() == null)
                    return;

                //展示盟主信息
                _reqPlayerInfo(info.getShowInfo().getLeaderId());
            }, null);
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickCloseBtn(GameObject _go)
        {
            if (null != _m_closeAction)
                _m_closeAction();
        }
    }
}