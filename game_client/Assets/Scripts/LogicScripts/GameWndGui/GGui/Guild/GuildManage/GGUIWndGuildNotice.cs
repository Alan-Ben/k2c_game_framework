using System.Collections.Generic;
using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟通知编辑界面
    /// </summary>
    public class GGUIWndGuildNotice : _ANPGGUIBasicWnd<GGUIMonoGuildNotice>
    {
        private static GGUIWndGuildNotice _g_instance = new GGUIWndGuildNotice();
        public static GGUIWndGuildNotice instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildNotice();
                return _g_instance;
            }
        }

        //选中的成员id列表
        private List<long> _m_lSelectCidList;
        //消耗道具
        private NPGGUIWndCommonItem _m_wCostItem;

        public GGUIWndGuildNotice() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildNotice.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildNotice.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wCostItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCostItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            _m_wCostItem?.discard();
            _m_wCostItem = null;

            _m_lSelectCidList?.Clear();
            _m_lSelectCidList = null;

            if (wnd.inputMessage != null)
                wnd.inputMessage.onValueChanged?.RemoveAllListeners();

            ALUGUICommon.uncombineBtnClick(wnd.btnSend, _onClickSend);
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnSelectMember, _onClickSelectMember);

        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.inputMessage != null)
                wnd.inputMessage.onValueChanged?.AddListener(_onMessageEdit);

            if (wnd.monoCostItem != null)
                _m_wCostItem = new NPGGUIWndCommonItem(wnd.monoCostItem);

            ALUGUICommon.combineBtnClick(wnd.btnSend, _onClickSend);
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnSelectMember, _onClickSelectMember);
        }

        /// <summary>
        /// 设置选择列表
        /// </summary>
        /// <param name="_selectCidList"></param>
        public void setSelectList(List<long> _selectCidList)
        {
            if(_m_lSelectCidList == null)
                _m_lSelectCidList = new List<long>();
            _m_lSelectCidList.Clear();

            if(_selectCidList != null)
                _m_lSelectCidList.AddRange(_selectCidList);
        }

        //刷新界面
        private void _refreshWnd()
        {
            _refreshMemberCount();
            _refreshTextCount();
            _refreshState();
        }

        //刷新通知的成员数
        private void _refreshMemberCount()
        {
            if (wnd == null)
                return;

            GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
            long selectCount = _m_lSelectCidList != null ? _m_lSelectCidList.Count : 0;
            long totalCount = guildInfo != null ? guildInfo.memberCount : 0;
            ALUGUICommon.setLabelTxt(wnd.txtMemberCount,
                TextTranslate.instance.getLanguage(TransKeyConst.guild_broadcastMessageMemberCount_count_count,
                    selectCount, totalCount));
        }

        //刷新文本数量
        private void _refreshTextCount()
        {
            if (wnd == null)
                return;

            string messageStr = wnd.inputMessage?.text;
            long messageLength = CharacterDetermineMgr.instance.getUnicodeStringLength(messageStr);
            WCGIntRange lengthRange = GRefdataCoreMgr.instance.npGeneral.guild_broadcast_message_length_limit;
            if (lengthRange == null)
                return;

            //设置字数提示
            ALUGUICommon.setLabelTxt(wnd.txtMessageCount, TextTranslate.instance.getLanguage(TransKeyConst.common_words_limit_tip, messageLength, lengthRange.max));
        }

        //刷新次数状态
        private void _refreshState()
        {
            if (wnd == null)
                return;

            long freeCount = NPPlayer.instance.fixedCdComp.getCount(GRefdataCoreMgr.instance.npGeneral.guild_broadcast_message_daily_limit_fixcd_id);
            ALUGUICommon.setGameObjEnable(wnd.goHaveFreeCountShowList,freeCount > 0);
            ALUGUICommon.setGameObjEnable(wnd.goHaveFreeCountHideList,freeCount <=0);
            if (freeCount > 0)
            {
                _m_wCostItem?.hideWnd();
                ALUGUICommon.setLabelTxt(wnd.txtFreeCount, TextTranslate.instance.getLanguage(TransKeyConst.guild_todayNoticeLeftCount_count, freeCount));
            }
            else
            {
                _m_wCostItem?.showWnd();
                _m_wCostItem?.setItem(GRefdataCoreMgr.instance.npGeneral.guild_broadcast_message_cost);
            }
        }

        //消息编辑结束
        private void _onMessageEdit(string _str)
        {
            _refreshTextCount();
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_NOTICE);
        }

        //点击选择成员
        private void _onClickSelectMember(GameObject _go)
        {
            GGUIWndGuildMemberSelect.instance.setInfo(_m_lSelectCidList, _list =>
            {
                _m_lSelectCidList = _list;
            });
            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndGuildMemberSelect.instance, UINodeTagConst_Guild.C_GUILD_MEMBER_SELECT);
        }

        //点击发送
        private void _onClickSend(GameObject _go)
        {
            if (wnd == null || wnd.inputMessage == null)
                return;

            //判断次数或者消耗是否足够
            long freeCount = NPPlayer.instance.fixedCdComp.getCount(GRefdataCoreMgr.instance.npGeneral.guild_broadcast_message_daily_limit_fixcd_id);
            if (freeCount <= 0 && !GCommon.isItemEnough(GRefdataCoreMgr.instance.npGeneral.guild_broadcast_message_cost, true))
                return;

            //判断是否有选择群发的人
            if (_m_lSelectCidList == null || _m_lSelectCidList.Count == 0)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_noticeNoSelectMember_none);
                return;
            }

            //判断文本是否合法
            string messageString = wnd.inputMessage.text;
            if (string.IsNullOrEmpty(messageString))
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_messageIsEmpty_none);
                return;
            }

            WCGIntRange lengthRange = GRefdataCoreMgr.instance.npGeneral.guild_broadcast_message_length_limit;
            int strLength = CharacterDetermineMgr.instance.getUnicodeStringLength(messageString);
            //设置文本是否合法提示
            if (strLength < lengthRange.min)
            {
                //内容过短
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_messageUnderLimit_none);
                return;
            }
            else if (strLength > lengthRange.max)
            {
                //内容过长
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_messageOverLimit_none);
                return;
            }
            else if (CharacterDetermineMgr.instance.isPlayerNameIllegal(messageString))
            {
                //内容包含特殊字符
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_messageIllegal_none);
                return;
            }

            NPPlayer.instance.guildComp.reqGuildBroadcastMessage(_m_lSelectCidList, messageString, null);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_NOTICE);
        }

        #endregion
    }
}