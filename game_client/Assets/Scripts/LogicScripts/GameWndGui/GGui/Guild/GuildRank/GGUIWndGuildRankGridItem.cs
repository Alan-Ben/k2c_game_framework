using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟排行列表item
    /// </summary>
    public class GGUIWndGuildRankGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoGuildRankGridItem>
    {
        private GuildRankInfo _m_guildRankInfo;
        private bool _m_bJoinGuildFuncOn;
        
        private long _m_lShowSerializeId;
        private long _m_lClickItemSerializeId;
        
        private GGUIWndGuildSubBaseInfo _m_wndGuildSubBaseInfo;//联盟基础信息附加窗口
        private GGUIWndJoinGuildBtn _m_wndJoinGuildBtn;//加入联盟按钮窗口

        public GGUIWndGuildRankGridItem(GGUIMonoGuildRankGridItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerializeId = ALSerializeOpMgr.next();
            _m_lClickItemSerializeId = ALSerializeOpMgr.next();
            
            _m_wndGuildSubBaseInfo?.hideWnd();
            _m_wndJoinGuildBtn?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wndGuildSubBaseInfo?.resetWnd();
            _m_wndJoinGuildBtn?.resetWnd();
        }

        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
            _m_wndGuildSubBaseInfo?.discard();
            _m_wndGuildSubBaseInfo = null;

            if (_m_wndJoinGuildBtn != null)
            {
                _m_wndJoinGuildBtn.onRetJoinGuild -= _onRetJoinGuild;
                _m_wndJoinGuildBtn.onRetApplyJoinGuild -= _onRetApplyJoinGuild;
                _m_wndJoinGuildBtn.onRetCancelJoinGuild -= _onRetCancelJoinGuild;
                _m_wndJoinGuildBtn.discard();
            }
            _m_wndJoinGuildBtn = null;

            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClick, _clickItem);
            }
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if(wnd.guildBaseInfoMono != null)
                _m_wndGuildSubBaseInfo = new GGUIWndGuildSubBaseInfo(wnd.guildBaseInfoMono);

            if (wnd.joinGuildBtn != null)
            {
                _m_wndJoinGuildBtn = new GGUIWndJoinGuildBtn(wnd.joinGuildBtn);
                _m_wndJoinGuildBtn.onRetJoinGuild += _onRetJoinGuild;
                _m_wndJoinGuildBtn.onRetApplyJoinGuild += _onRetApplyJoinGuild;
                _m_wndJoinGuildBtn.onRetCancelJoinGuild += _onRetCancelJoinGuild;
            }
            
            ALUGUICommon.combineBtnClick(wnd.btnClick, _clickItem);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_id"></param>
        public void setInfo(GuildRankInfo _guildRankInfo, bool _joinGuildFuncOn)
        {
            if (wnd == null)
                return;

            _m_guildRankInfo = _guildRankInfo;
            _m_bJoinGuildFuncOn = _joinGuildFuncOn;

            ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, _m_guildRankInfo == null);
            ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, _m_guildRankInfo != null);

            if(_guildRankInfo != null)
                _refreshWnd(true);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd(bool _useLatestInfo)
        {
            if (wnd == null)
                return;

            _m_wndJoinGuildBtn?.hideWnd();
            if (_m_guildRankInfo == null)
                return;

            long serializeId = _m_lShowSerializeId;
            _m_guildRankInfo.getGuildInfo((guildInfo) =>
            {
                if(serializeId != _m_lShowSerializeId || guildInfo == null)
                    return;
                
                if (_m_wndGuildSubBaseInfo != null)
                {
                    _m_wndGuildSubBaseInfo.showWnd();
                    _m_wndGuildSubBaseInfo.setBaseInfo(guildInfo);
                }

                if (_m_wndJoinGuildBtn != null && _m_bJoinGuildFuncOn && !NPPlayer.instance.guildComp.isJoinGuild())
                {
                    _m_wndJoinGuildBtn.showWnd();
                    _m_wndJoinGuildBtn.setData(guildInfo);
                }

                if (wnd != null)
                {
                    wnd.setRank(_m_guildRankInfo.rankSortOrder);
                }
            }, _useLatestInfo);
        }
        
        /// <summary>
        /// 收到加入联盟回包
        /// </summary>
        /// <param name="_isSucc"></param>
        private void _onRetJoinGuild(bool _isSucc)
        {
            // 刷新item
            _refreshWnd(true);
        }
        
        /// <summary>
        /// 收到申请加入联盟回包
        /// </summary>
        /// <param name="_isSucc"></param>
        private void _onRetApplyJoinGuild(bool _isSucc)
        {
            // 刷新item
            _refreshWnd(true);
        }
        
        /// <summary>
        /// 收到取消加入联盟回包
        /// </summary>
        /// <param name="_isSucc"></param>
        private void _onRetCancelJoinGuild(bool _isSucc)
        {
            // 刷新item
            _refreshWnd(true);
        }

        /// <summary>
        /// 点击item
        /// </summary>
        /// <param name="_go"></param>
        private void _clickItem(GameObject _go)
        {
            if(_m_guildRankInfo == null)
                return;
            
            long serializeId = _m_lClickItemSerializeId = ALSerializeOpMgr.next();
            _m_guildRankInfo.getGuildInfo((_guildInfo) =>
            {
                if(_guildInfo == null || serializeId != _m_lClickItemSerializeId || wnd == null || !isShow)
                    return;

                GGUIWndGuildOtherInfo.instance.onRetJoinGuild += _onRetJoinGuild;
                GGUIWndGuildOtherInfo.instance.onRetApplyJoinGuild += _onRetApplyJoinGuild;
                GGUIWndGuildOtherInfo.instance.onRetCancelJoinGuild += _onRetCancelJoinGuild;
                QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst_Guild.C_GUILD_OTHER_GUILD_INFO, true, false, false, null, GGUIWndGuildOtherInfo.instance, true, false,
                    () =>
                    {
                        GGUIWndGuildOtherInfo.instance.setData(_guildInfo, true);
                    }, null, null, () =>
                    {
                        GGUIWndGuildOtherInfo.instance.onRetJoinGuild -= _onRetJoinGuild;
                        GGUIWndGuildOtherInfo.instance.onRetApplyJoinGuild -= _onRetApplyJoinGuild;
                        GGUIWndGuildOtherInfo.instance.onRetCancelJoinGuild -= _onRetCancelJoinGuild;
                    }));
            }, true);
        }
    }
}
