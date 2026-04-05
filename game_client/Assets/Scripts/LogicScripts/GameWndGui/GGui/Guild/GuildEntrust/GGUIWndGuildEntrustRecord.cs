using System.Collections.Generic;
using ALPackage;
using Common.GuildObj;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟委托处理记录
    /// </summary>
    public class GGUIWndGuildEntrustRecord : _ANPGGUIBasicWnd<GGUIMonoGuildEntrustRecord>
    {
        private static GGUIWndGuildEntrustRecord _g_instance;
        public static GGUIWndGuildEntrustRecord instance{ get { return _g_instance ??= new GGUIWndGuildEntrustRecord(); } }
        
        private GGUIWndGuildEntrustRecordItemGrid _m_wndGuildEntrustRecordItemGrid;//委托处理记录itemGrid
        
        public GGUIWndGuildEntrustRecord() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoGuildEntrustRecord.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoGuildEntrustRecord.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.monoGuildEntrustRecordItemGrid != null)
                _m_wndGuildEntrustRecordItemGrid = new GGUIWndGuildEntrustRecordItemGrid(wnd.monoGuildEntrustRecordItemGrid);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
            }
            
            _m_wndGuildEntrustRecordItemGrid?.discard();
            _m_wndGuildEntrustRecordItemGrid = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wndGuildEntrustRecordItemGrid?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wndGuildEntrustRecordItemGrid?.resetWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;
            
            NPPlayer.instance.guildComp.reqGuildAllMemberEntrustInfo((_msg) =>
            {
                if(wnd == null || !isShow)
                    return;
                
                List<GuildMemberEntrustInfo> memberEntrustInfoList = new List<GuildMemberEntrustInfo>();
                if (_msg != null && _msg.getInfoList() != null)
                {
                    foreach (var serverEntrustInfo in _msg.getInfoList())
                    {
                        if(serverEntrustInfo != null)
                            memberEntrustInfoList.Add(new GuildMemberEntrustInfo(serverEntrustInfo));
                    }
                }
                
                memberEntrustInfoList.Sort(GuildMemberEntrustInfo.sort);
                if (_m_wndGuildEntrustRecordItemGrid != null)
                {
                    _m_wndGuildEntrustRecordItemGrid.showWnd();
                    _m_wndGuildEntrustRecordItemGrid.setData(memberEntrustInfoList, false);
                }
                
                // 找到自己的委托信息
                int selfEntrustInfoIndex = memberEntrustInfoList.FindIndex((info) => info.cid == NPPlayer.instance.playerInfo.CID);
                GuildMemberEntrustInfo selfEntrustInfo = memberEntrustInfoList.SafeGet(selfEntrustInfoIndex);

                string selfRankKey = string.IsNullOrEmpty(wnd.txtSelfRankKey) ? TransKeyConst.common_value : wnd.txtSelfRankKey;
                ALUGUICommon.setLabelTxt(wnd.txtSelfRank, TextTranslate.instance.getLanguage(selfRankKey, selfEntrustInfoIndex + 1));
                
                string selfEntrustDealCountKey = string.IsNullOrEmpty(wnd.txtSelfEntrustDealCountKey) ? TransKeyConst.common_currentTotalNum_num_num : wnd.txtSelfEntrustDealCountKey;
                ALUGUICommon.setLabelTxt(wnd.txtSelfEntrustDealCount,
                    TextTranslate.instance.getLanguage(selfEntrustDealCountKey, selfEntrustInfo?.getDayDealTimes() ?? 0,
                        selfEntrustInfo?.getTotalDealTimes() ?? 0));
            }, null);
        }
        
        /// <summary>
        /// 关闭按钮被点击
        /// </summary>
        private void _onCloseBtnClick(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_ENTRUST_RECORD);
        }
    }
}