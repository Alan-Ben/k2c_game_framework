using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟排行界面
    /// </summary>
    public class GGUIWndGuildRank : _ANPGGUIBasicWnd<GGUIMonoGuildRank>
    {
        private NPCommonAssetPathInfo _m_commonAssetPathInfo;
        private bool _m_bSearchGuildFuncOn;//搜索联盟功能是否开启
        private bool _m_bJoinGuildFuncOn;//加入联盟功能是否开启
        
        private string _m_strSearchGuildTag;//搜索的联盟标志

        private bool _m_bIsReqRankData;//是否正在请求排行数据
        private long _m_lReqRankDataSerializeId;//请求排行数据的序列化id
        
        private bool _m_bIsReqSelfRankData;//是否正在请求自身联盟排行数据
        private long _m_lReqSelfRankDataSerializeId;//请求自身联盟排行数据的序列化id
        
        private GGUIWndGuildRankGrid _m_wndGuildRankGrid;//联盟排行列表窗口
        private GGUIWndGuildRankGridItem _m_wndSelfGuildRankInfo;//自身联盟排行信息窗口

        private List<GGUIWndGuildRankGridItem> _m_wndTopRankList;//前三名联盟排行窗口列表

        public GGUIWndGuildRank(NPCommonAssetPathInfo _assetPathInfo, bool _searchGuildFuncOn, bool _joinGuildFuncOn) : base(EALUIWndLayer.NORMAL)
        {
            _m_commonAssetPathInfo = _assetPathInfo;
            _m_bSearchGuildFuncOn = _searchGuildFuncOn;
            _m_bJoinGuildFuncOn = _joinGuildFuncOn;
        }
        
        public GGUIWndGuildRank(long _uiPathId, bool _searchGuildFuncOn, bool _joinGuildFuncOn) : base(EALUIWndLayer.NORMAL)
        {
            _m_commonAssetPathInfo = UIResPathAssistant.getAssetInfo(_uiPathId);
            _m_bSearchGuildFuncOn = _searchGuildFuncOn;
            _m_bJoinGuildFuncOn = _joinGuildFuncOn;
        }

        protected override string _monoAssetPath { get { return _m_commonAssetPathInfo?.asset_path; } }
        protected override string _monoObjName { get { return _m_commonAssetPathInfo?.obj_name; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        //这里因为窗口加载路径不是固定的, 是由外部传入，这里为了防止外部有地方不销毁窗口导致资源泄漏，所以这里需要销毁, 若后续因为某些原因这里需要改成不销毁, 记得找下引用外部要销毁窗口
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wndGuildRankGrid?.hideWnd();
            _m_wndSelfGuildRankInfo?.hideWnd();
            if (_m_wndTopRankList != null)
            {
                for (int i = 0; i < _m_wndTopRankList.Count; i++)
                {
                    _m_wndTopRankList[i]?.hideWnd();
                }
            }

            _m_lReqRankDataSerializeId = ALSerializeOpMgr.next();
            _m_bIsReqRankData = false;

            _m_lReqSelfRankDataSerializeId = ALSerializeOpMgr.next();
            _m_bIsReqSelfRankData = false;
        }

        protected override void _onReset()
        {
            _m_wndGuildRankGrid?.resetWnd();
            _m_wndSelfGuildRankInfo?.resetWnd();
            if (_m_wndTopRankList != null)
            {
                for (int i = 0; i < _m_wndTopRankList.Count; i++)
                {
                    _m_wndTopRankList[i]?.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            _m_wndGuildRankGrid?.discard();
            _m_wndGuildRankGrid = null;
            
            _m_wndSelfGuildRankInfo?.discard();
            _m_wndSelfGuildRankInfo = null;

            if (_m_wndTopRankList != null)
            {
                for (int i = 0; i < _m_wndTopRankList.Count; i++)
                {
                    _m_wndTopRankList[i]?.discard();
                }
                _m_wndTopRankList.Clear();
                _m_wndTopRankList = null;
            }

            if (wnd != null)
            {
                if(wnd.inputSearch != null)
                    wnd.inputSearch.onEndEdit.RemoveListener(_onEndEditSearch);       
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoRankGrid != null)
            {
                _m_wndGuildRankGrid = new GGUIWndGuildRankGrid(wnd.monoRankGrid);
                _m_wndGuildRankGrid.setJoinGuildFuncOn(_m_bJoinGuildFuncOn);
            }

            if (wnd.selfGuildRankInfo != null)
                _m_wndSelfGuildRankInfo = new GGUIWndGuildRankGridItem(wnd.selfGuildRankInfo);

            _m_wndTopRankList = new List<GGUIWndGuildRankGridItem>();
            if (wnd.monoTopRankList != null)
            {
                for (int i = 0; i < wnd.monoTopRankList.Count; i++)
                {
                    GGUIWndGuildRankGridItem topItem = new GGUIWndGuildRankGridItem(wnd.monoTopRankList[i]);
                    _m_wndTopRankList.Add(topItem);
                }
            }
            
            if(wnd.inputSearch != null)
                wnd.inputSearch.onEndEdit.AddListener(_onEndEditSearch);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 刷新窗口
        /// </summary>
        private void _refreshWnd()
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.setGameObjEnable(wnd.searchFuncOnShowGoList, _m_bSearchGuildFuncOn);

            _refreshOthersGuild();
            _refreshSelfGuild();
        }

        private void _refreshOthersGuild()
        {
            if(wnd == null || _m_bIsReqRankData)
                return;

            //先隐藏前几名展示
            if (_m_wndTopRankList != null)
            {
                for (int i = 0; i < _m_wndTopRankList.Count; i++)
                {
                    _m_wndTopRankList[i]?.showWnd();
                    _m_wndTopRankList[i]?.setInfo(null, _m_bJoinGuildFuncOn);
                }
            }
            
            _m_bIsReqRankData = true;
            long serializeReqRankDataId = _m_lReqRankDataSerializeId = ALSerializeOpMgr.next();
            if (_m_bSearchGuildFuncOn && !string.IsNullOrEmpty(_m_strSearchGuildTag))//若联盟搜索功能开启 且 搜索的联盟标志不为空
            {
                NPPlayer.instance.guildComp.reqSearchGuild(_m_strSearchGuildTag, (_succ, _msg) =>
                {
                    _m_bIsReqRankData = false;
                    if(serializeReqRankDataId != _m_lReqRankDataSerializeId || wnd == null || !isShow)
                        return;

                    if (_succ && _msg != null)//若数据请求成功
                    {
                        GuildRankInfo guildRankInfo = new GuildRankInfo(_msg.getRankBaseInfo(),0, false);
                        if (_m_wndGuildRankGrid != null)
                        {
                            _m_wndGuildRankGrid.showWnd();
                            _m_wndGuildRankGrid.clearShowData();
                            _m_wndGuildRankGrid.addShowData(guildRankInfo);
                        }
                        
                        ALUGUICommon.setGameObjEnable(wnd.guildNotFindShow, false);
                        ALUGUICommon.setGameObjEnable(wnd.guildFindShow, true);
                    }
                    else
                    {
                        if(_m_wndGuildRankGrid != null)
                            _m_wndGuildRankGrid.hideWnd();
                        
                        ALUGUICommon.setGameObjEnable(wnd.guildFindShow, false);
                        ALUGUICommon.setGameObjEnable(wnd.guildNotFindShow, true);
                    }

                    // 若收到回包后发现搜索的联盟标志为空，再次刷新窗口
                    if (string.IsNullOrEmpty(_m_strSearchGuildTag))
                    {
                        _refreshWnd();
                    }
                });
            }
            else
            {
                long rankFixedId = GRefdataCoreMgr.instance.npGeneral.guild_rank_fixed_id;
                NPPlayer.instance.rankCommonComp.reqRankFixedBaseList(rankFixedId, _msg =>
                {
                    _m_bIsReqRankData = false;
                    if(serializeReqRankDataId != _m_lReqRankDataSerializeId || wnd == null || !isShow)
                        return;
                    
                    ALUGUICommon.setGameObjEnable(wnd.guildFindShow, false);
                    ALUGUICommon.setGameObjEnable(wnd.guildNotFindShow, false);

                    List<GuildRankInfo> rankInfoList = _msg == null ? null : GuildRankInfo.getRankInfoList(_msg.getBaseItemlist(), rankFixedId, false);

                    //设置前几名联盟信息
                    if (_m_wndTopRankList != null && rankInfoList != null)
                    {
                        for (int i = 0; i < _m_wndTopRankList.Count; i++)
                        {
                            if (rankInfoList.Count > 0)
                            {
                                //设置信息
                                _m_wndTopRankList[i]?.showWnd();
                                _m_wndTopRankList[i]?.setInfo(rankInfoList[0], _m_bJoinGuildFuncOn);
                                //移除已设置的数据
                                rankInfoList.RemoveAt(0);
                            }
                            else
                            {
                                _m_wndTopRankList[i]?.showWnd();
                                _m_wndTopRankList[i]?.setInfo(null, _m_bJoinGuildFuncOn);
                            }
                        }
                    }

                    //设置联盟排行列表
                    if (_m_wndGuildRankGrid != null)
                    {
                        _m_wndGuildRankGrid.showWnd();
                        _m_wndGuildRankGrid.setShowData(rankInfoList);
                    }
                    
                    // 若收到回包后发现搜索的联盟标志不空，再次刷新窗口
                    if (_m_bSearchGuildFuncOn && !string.IsNullOrEmpty(_m_strSearchGuildTag))
                    {
                        _refreshWnd();
                    }
                });
            }
        }

        private void _refreshSelfGuild()
        {
            if(wnd == null)
                return;
            
            if (NPPlayer.instance.guildComp.isJoinGuild())//若自身已经加入联盟
            {
                if (_m_wndSelfGuildRankInfo != null && NPPlayer.instance.guildComp.guildInfo != null && !_m_bIsReqSelfRankData)
                {
                    _m_bIsReqSelfRankData = true;
                    long serializeReqSelfRankDataId = _m_lReqSelfRankDataSerializeId = ALSerializeOpMgr.next();
                    
                    NPPlayer.instance.guildComp.reqSearchGuild(NPPlayer.instance.guildComp.guildInfo.guildId.ToString(), (_succ, _msg) =>
                    {
                        _m_bIsReqSelfRankData = false;
                        if(serializeReqSelfRankDataId != _m_lReqSelfRankDataSerializeId || wnd == null || !isShow)
                            return;

                        if (_succ && _msg != null && _msg.getRankBaseInfo() != null)
                        {
                            _m_wndSelfGuildRankInfo.showWnd();
                            _m_wndSelfGuildRankInfo.setInfo(new GuildRankInfo(_msg.getRankBaseInfo(),0, false), false);//显示玩家自身联盟的item不需要开启加入申请功能
                        }
                        else
                        {
                            _m_wndSelfGuildRankInfo?.hideWnd();
                        }
                    });
                }
            }
            else
            {
                _m_wndSelfGuildRankInfo?.hideWnd();
            }
        }
        
        /// <summary>
        /// 搜索输入框结束编辑
        /// </summary>
        /// <param name="_str"></param>
        private void _onEndEditSearch(string _str)
        {
            _m_strSearchGuildTag = _str;
            _refreshWnd();
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_RANK);
        }
    }
}