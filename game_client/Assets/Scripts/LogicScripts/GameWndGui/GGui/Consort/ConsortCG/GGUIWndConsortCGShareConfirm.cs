using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 家人CG分享确认弹窗
    /// </summary>
    public class GGUIWndConsortCGShareConfirm : _ANPGGUIBasicWnd<GGUIMonoConsortCGShareConfirm>
    {
        private static GGUIWndConsortCGShareConfirm _g_instance;
        public static GGUIWndConsortCGShareConfirm instance { get { return _g_instance ??= new GGUIWndConsortCGShareConfirm(); } }

        //CG数据
        private ConsortCGRefObj _m_cgRef;
        //当前选择的聊天频道类型
        private ENPChatRoomType _m_eSelectType;
        //CG图标
        private NPGGuiWndTexture _m_wIcon;
        //分享的聊天频道列表
        private List<GGUIWndConsortCGShareChannelItem> _m_lChannelItems;

        public GGUIWndConsortCGShareConfirm() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortCGShareConfirm.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortCGShareConfirm.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wIcon?.hideWnd();
            if (_m_lChannelItems != null)
            {
                for (int i = 0; i < _m_lChannelItems.Count; i++)
                {
                    _m_lChannelItems[i]?.hideWnd();
                }
            }
        }

        protected override void _onReset()
        {
            _m_wIcon?.discardTexture();
            if (_m_lChannelItems != null)
            {
                for (int i = 0; i < _m_lChannelItems.Count; i++)
                {
                    _m_lChannelItems[i]?.resetWnd();
                }
            }
        }

        protected override void _onDiscard()
        {
            _m_wIcon?.discard();
            _m_wIcon = null;
            if (_m_lChannelItems != null)
            {
                for (int i = 0; i < _m_lChannelItems.Count; i++)
                {
                    _m_lChannelItems[i]?.discard();
                }
                _m_lChannelItems.Clear();
                _m_lChannelItems = null;
            }

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickBtnClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnConfirm, _onClickBtnConfirm);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            _m_lChannelItems = new List<GGUIWndConsortCGShareChannelItem>();
            if (wnd.monoChannelItemList != null)
            {
                for (int i = 0; i < wnd.monoChannelItemList.Count; i++)
                {
                    GGUIWndConsortCGShareChannelItem channelItem = new GGUIWndConsortCGShareChannelItem(wnd.monoChannelItemList[i]);
                    channelItem.onClickItem += _onSelectItem;
                    channelItem.setSelect(false);
                    //默认选择第一个频道
                    if (i == 0)
                    {
                        _m_eSelectType = channelItem.channelType;
                        channelItem.setSelect(true);
                    }
                    _m_lChannelItems.Add(channelItem);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickBtnClose);
            ALUGUICommon.combineBtnClick(wnd.btnConfirm, _onClickBtnConfirm);
        }

        /// <summary>
        /// 设置CG数据
        /// </summary>
        /// <param name="_consortCgRef">CG配表数据</param>
        public void setData(ConsortCGRefObj _consortCgRef)
        {
            _m_cgRef = _consortCgRef;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        { 
            if (wnd == null || _m_cgRef == null)
                return;

            //设置名称
            ALUGUICommon.setLabelTxt(wnd.txtCGName, TextTranslate.instance.getLanguage(_m_cgRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtConsortName, GCommon.getItemName(ENPItemType.CONSORT, _m_cgRef.consort_id));

            //图标
            _m_wIcon?.showWnd();
            _m_wIcon?.setTexture(_m_cgRef.cg_icon);

            //描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(TransKeyConst.consort_cgShareConfirmDesc_none, _m_cgRef.name));

            //页签状态
            _refreshTabState();
        }

        /// <summary>
        /// 刷新选中状态
        /// </summary>
        private void _refreshTabState()
        {
            if (_m_lChannelItems == null)
                return;

            for (int i = 0; i < _m_lChannelItems.Count; i++)
            {
                _m_lChannelItems[i]?.setSelect(_m_lChannelItems[i].channelType == _m_eSelectType);
            }
        }

        #region 点击事件

        /// <summary>
        /// 点击选择聊天频道
        /// </summary>
        /// <param name="_type"></param>
        private void _onSelectItem(ENPChatRoomType _type)
        {
            _m_eSelectType = _type;
            _refreshTabState();
        }

        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_CG_SHARE_CONFIRM);
        }

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onClickBtnConfirm(GameObject _go)
        {
            if (_m_cgRef == null) 
                return;

            //如果选择了联盟频道，检查是否加入了联盟
            if (_m_eSelectType == ENPChatRoomType.GUILD && !NPPlayer.instance.guildComp.isJoinGuild())
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chat_shareNotJoinGuild_none);
                return;
            }

            ConsortCgInfo cgInfo = NPPlayer.instance.consortComp.getConsortCgInfo(_m_cgRef.cg_id);
            if (cgInfo == null)
                return;

            GChatUtil.sendChatShareMsg(_m_eSelectType, EChatShareType.CONSORT_CG, NPMsgDetailInfoFactory.createShareConsortCGInfo(cgInfo),
                () =>
                {
                    //分享成功
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.chat_shareSuccess_none);
                });
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_CONSORT_CG_SHARE_CONFIRM);
        }

        #endregion
    }
}