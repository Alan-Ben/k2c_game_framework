
using ALPackage;
using ChatPackage;
using UnityEngine;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 晚间副本宝箱聊天消息 Wnd
    /// </summary>
    public class NPGGUIWndChatMsgItemEveningDungeonBox : _ATNPGGUIWndPlayerChatMsgItem_JumpTo<NPGGUIMonoChatMsgItemEveningDungeonBox, NPChatMsgEveningDungeonBoxInfo>
    {
        // 刷新操作序列号
        private long _m_lRefreshSerialize;
        // 宝箱状态
        private EMiddayDungeonBoxState _m_boxState;
        // 宝箱Banner图片窗口
        private NPGGuiWndTexture _m_bannerWnd;

        protected override long senderPlayerCid { get { return detailInfo == null ? 0 : detailInfo.sender.getCid(); } }

        public NPGGUIWndChatMsgItemEveningDungeonBox(NPChatMsgEveningDungeonBoxInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
        {
        }

        protected override void _onShowWndEx()
        {
            _refresh();
        }

        protected override void _onHideWndEx()
        {
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
            _m_bannerWnd?.hideWnd();
        }

        protected override void _onResetEx()
        {
            _m_bannerWnd?.discardTexture();
        }

        protected override void _onDiscardEx()
        {
            _m_bannerWnd?.discard();
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onWndInitDoneEx()
        {
            if (null == wnd)
                return;
            if(wnd.boxBanner != null)
                _m_bannerWnd = new NPGGuiWndTexture(wnd.boxBanner);
        }

        /// <inheritdoc/>
        protected override void _loadAdditionTemplate(ALStepCounter _stepCounter)
        {
        }

        protected override void _discardAdditionTemplate()
        {
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        private void _refresh()
        {
            if (detailInfo == null || detailInfo.content == null)
                return;
            long serializeId = _m_lRefreshSerialize = ALSerializeOpMgr.next();

            if (wnd != null)
            {
                // 设置宝箱领取次数文本(复用午间副本配置的限制领取次数)
                ALUGUICommon.setLabelTxt(wnd.txtCount,
                    TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_box_has_draw_count, 0, detailInfo.boxRefObj?.can_draw_limit ?? 0));

                // 设置宝箱Banner
                _m_bannerWnd?.setTexture(detailInfo.boxRefObj?.box_banner);
                _m_bannerWnd?.showWnd();
            }

            _m_boxState = EMiddayDungeonBoxState.Invalid;
            // 判断宝箱是否过期
            if (detailInfo.content.getExpiredTimeMs() >= FpsAndPingMgr.instance.serverTimeTag )
            {
                // 宝箱未过期，检查本地缓存是否已经领完
                if (!AccountSettingMgr.instance.middayDungeonSaver.hasInValid(detailInfo.content.getDbId()))
                {
                    NPPlayer.instance.middayDungeonComp.reqMiddayDungeonBoxCanDraw(detailInfo.content.getDbId(), detailInfo.content.getExpiredTimeMs(), 
                        (_suc, _canDraw, _remainDrawCount) =>
                        {
                            if (serializeId != _m_lRefreshSerialize)
                                return;
                            // 检查是否已经打开过
                            if (AccountSettingMgr.instance.middayDungeonSaver.hasOpen(detailInfo.content.getDbId()))
                                _m_boxState = EMiddayDungeonBoxState.HasOpen;
                            else
                                _m_boxState = _canDraw ? EMiddayDungeonBoxState.CanOpen : EMiddayDungeonBoxState.NonCanOpen;
                            
                            if (wnd != null)
                                ALUGUICommon.setLabelTxt(wnd.txtCount, TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_box_has_draw_count, _remainDrawCount, detailInfo.boxRefObj?.can_draw_limit));
                            _refreshWnd();
                        });
                }
                else
                {
                    _m_boxState = EMiddayDungeonBoxState.NonCanOpen;
                }
            }              
            _refreshWnd();
        }

        /// <summary>
        /// 刷新宝箱状态显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            NPCommonEnumStatInfo<EMiddayDungeonBoxState>.setStat(wnd.statInfos, _m_boxState);    
        }
        
        /// <summary>
        /// 刷新玩家信息
        /// </summary>
        protected override void _refreshPlayerInfo(NPGGUIWndPlayerIcon _playerInfoWnd)
        {
            if (_playerInfoWnd == null || detailInfo == null)
                return;

            _playerInfoWnd.setPlayerInfo(detailInfo.sender);
        }
        
        protected override ENPFunctionType functionType { get => ENPFunctionType.NONE; }

        /// <summary>
        /// 点击跳转逻辑
        /// </summary>
        protected override void _onClickJump()
        {
            if (detailInfo == null || detailInfo.content == null)
                return;
            
            // 宝箱已失效或已过期
            if (_m_boxState == EMiddayDungeonBoxState.Invalid || detailInfo.content.getExpiredTimeMs() < FpsAndPingMgr.instance.serverTimeTag)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_box_invalid_tip));
                return;
            }
            
            // 如果宝箱已领取或者被领取完毕，则打开记录界面
            if (_m_boxState == EMiddayDungeonBoxState.HasOpen || _m_boxState == EMiddayDungeonBoxState.NonCanOpen)
            {
                if (_m_boxState != EMiddayDungeonBoxState.Invalid)
                {
                    // 复用午间副本宝箱记录界面
                    GGUIWndMiddayDungeonBoxRecord.instance.setInfo(detailInfo.content.getBoxId(), detailInfo.content.getDbId());
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMiddayDungeonBoxRecord.instance, GGUIWndMiddayDungeonBoxRecord.instance.showWnd, UINodeTagConst.C_MIDDAY_DUNGEON_BOX_RECORD);
                }
                return;
            }
            
            // 检查今日领取次数限制(复用午间副本配置的限制领取次数)
            NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(detailInfo.boxRefObj?.draw_box_fixed_cd_id ?? 0);
            if (fixedCdInfo != null && fixedCdInfo.getCount() <= 0)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_box_reach_max_count, fixedCdInfo.getMaxCount()));
                return;
            }
            
            // 请求领取宝箱
            NPPlayer.instance.eveningDungeonComp.reqEveningDungeonDrawBox(detailInfo.content.getDbId(), detailInfo.content.getExpiredTimeMs(), _suc =>
            {
                if (_suc)
                {
                    _refresh();
                }
                else
                {
                    _m_boxState = EMiddayDungeonBoxState.NonCanOpen;
                    _refreshWnd();
                }
            });
        }
    }
}
