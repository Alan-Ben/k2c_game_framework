using ALPackage;
using ChatPackage;
using JetBrains.Annotations;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChatMsgItemGuildRecruit : _ATNPGGUIWndPlayerChatMsgItem_JumpTo<GGUIMonoChatMsgItemGuildRecruit, ChatGuildRecruitMsgInfo>
    {
        private long _m_wndShowSerializeId;
        
        public GGUIWndChatMsgItemGuildRecruit(ChatGuildRecruitMsgInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
        }

        protected override void _loadAdditionTemplate(ALStepCounter _stepCounter)
        {
        }

        protected override void _discardAdditionTemplate()
        {
        }

        protected override long senderPlayerCid { get { return detailInfo == null ? 0 : detailInfo.sender.getCid(); } }

        protected override void _onShowWndEx()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);//加入联盟消息

            _refreshWnd();
        }

        protected override void _onHideWndEx()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);//加入联盟消息

            _m_wndShowSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onResetEx()
        {
        }

        protected override void _onDiscardEx()
        {
        }

        protected override void _onWndInitDoneEx()
        {
        }

        private void _refreshWnd()
        {
            if(wnd == null)
                return;

            ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(TransKeyConst.guild_recruitChatDesc_str1, detailInfo?.content.getGuildName()));
        }
        
        protected override void _refreshPlayerInfo(NPGGUIWndPlayerIcon _playerInfoWnd)
        {
            if (_playerInfoWnd == null || detailInfo == null)
                return;

            _playerInfoWnd.setPlayerInfo(detailInfo.sender);
        }

        protected override ENPFunctionType functionType { get { return ENPFunctionType.GUILD; } }
        protected override void _onClickJump()
        {
            if(detailInfo == null)
                return;

            long serializeId = _m_wndShowSerializeId = ALSerializeOpMgr.next();
            detailInfo.getGuildInfo((_guildInfo) =>
            {
                if(_guildInfo == null || serializeId != _m_wndShowSerializeId || wnd == null || !isShow)
                    return;
                
                // 更新下联盟名
                ALUGUICommon.setLabelTxt(wnd.txtContent, TextTranslate.instance.getLanguage(TransKeyConst.guild_recruitChatDesc_str1, detailInfo?.content.getGuildName()));

                GGUIWndGuildOtherInfo.instance.onRetJoinGuild += _onRetJoinGuild;
                QueueMgr.instance.AddNode(new BaseOnAddContainerSceneUIWndQueueNode(EUIQueueStageType.MAIN, UINodeTagConst_Guild.C_GUILD_OTHER_GUILD_INFO, true, false, false, null, GGUIWndGuildOtherInfo.instance, true, false,
                    () =>
                    {
                        GGUIWndGuildOtherInfo.instance.setData(_guildInfo, true);
                    }, null, null, () =>
                    {
                        GGUIWndGuildOtherInfo.instance.onRetJoinGuild -= _onRetJoinGuild;
                    }));
            });
        }
        
        // 若直接加入成功
        private void _onRetJoinGuild(bool _isSucc)
        {
            
        }

        /// <summary>
        /// 收到加入联盟推送, 只要有tip提示就行
        /// </summary>
        private void _onJoinGuild(params object[] _objects)
        {
            if (_objects == null || _objects[0] == null || detailInfo == null)
                return;

            if (detailInfo.content.getGuildId() == NPPlayer.instance.guildComp.guildInfo.guildId)
            {
                //加入联盟成功
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_reqJoinGuildSuccTip_none);

                //关闭窗口，打开联盟主界面
                QueueMgr.instance.AddNode(new GNodeBuilding());
                QueueMgr.instance.AddNode(new GNodeSpaceStation());
                GCommon.enterUIMainNodeShow(ESysSceneType.GUILD);
            }
        }
    }
}