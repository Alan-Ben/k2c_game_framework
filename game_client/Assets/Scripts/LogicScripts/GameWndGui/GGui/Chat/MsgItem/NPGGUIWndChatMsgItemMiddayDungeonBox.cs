
using ALPackage;
using ChatPackage;
using UnityEngine;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 午间副本宝箱
    /// </summary>
    public class NPGGUIWndChatMsgItemMiddayDungeonBox : _ATNPGGUIWndPlayerChatMsgItem_JumpTo<NPGGUIMonoChatMsgItemMiddayDungeonBox, NPChatMsgMiddayDungeonBoxInfo>
    {
        //刷新操作序列号
        private long _m_lRefreshSerialize;
        private EMiddayDungeonBoxState _m_boxState;

        private NPGGuiWndTexture _m_bannerWnd;
        protected override long senderPlayerCid { get { return detailInfo == null ? 0 : detailInfo.sender.getCid(); } }

        public NPGGUIWndChatMsgItemMiddayDungeonBox(NPChatMsgMiddayDungeonBoxInfo _detailInfo, Transform _parent, [NotNull] GUICacheMgrChatMsgItem _cacheMgr) : base(_detailInfo, _parent, _cacheMgr)
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

        private void _refresh()
        {
            if (detailInfo == null || detailInfo.content == null)
                return;
            long serializeId = _m_lRefreshSerialize = ALSerializeOpMgr.next();


            if (wnd != null)
            {
                ALUGUICommon.setLabelTxt(wnd.txtCount,
                    TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_box_has_draw_count, 0, detailInfo.boxRefObj?.can_draw_limit));

                MiddayDungeonBoxRefObj boxRefObj = GRefdataCoreMgr.instance.middayDungeonBoxRefCore.getRef(detailInfo.content.getBoxId());
                
                _m_bannerWnd?.setTexture(boxRefObj?.box_banner);
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

        private void _refreshWnd()
        {
            if (wnd == null)
                return;

            NPCommonEnumStatInfo<EMiddayDungeonBoxState>.setStat(wnd.statInfos, _m_boxState);    
        }
        

        protected override void _refreshPlayerInfo(NPGGUIWndPlayerIcon _playerInfoWnd)
        {
            if (_playerInfoWnd == null || detailInfo == null)
                return;

            _playerInfoWnd.setPlayerInfo(detailInfo.sender);
        }
        
        protected override ENPFunctionType functionType { get => ENPFunctionType.NONE; }
        protected override void _onClickJump()
        {
            if (detailInfo == null || detailInfo.content == null)
                return;
            if (_m_boxState == EMiddayDungeonBoxState.Invalid || detailInfo.content.getExpiredTimeMs() < FpsAndPingMgr.instance.serverTimeTag)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_box_invalid_tip));
                return;
            }
            if (_m_boxState == EMiddayDungeonBoxState.HasOpen || _m_boxState == EMiddayDungeonBoxState.NonCanOpen)
            {
                // 如果宝箱已领取或者被领取完毕，则打开记录界面
                if (_m_boxState != EMiddayDungeonBoxState.Invalid)
                {
                    GGUIWndMiddayDungeonBoxRecord.instance.setInfo(detailInfo.content.getBoxId(), detailInfo.content.getDbId());
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMiddayDungeonBoxRecord.instance, GGUIWndMiddayDungeonBoxRecord.instance.showWnd, UINodeTagConst.C_MIDDAY_DUNGEON_BOX_RECORD);
                }
                return;
            }
            NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(detailInfo.boxRefObj?.draw_box_fixed_cd_id ?? 0);
            if (fixedCdInfo != null && fixedCdInfo.getCount() <= 0)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_box_reach_max_count, fixedCdInfo.getMaxCount()));
                return;
            }
            NPPlayer.instance.middayDungeonComp.reqMiddayDungeonDrawBox(detailInfo.content.getDbId(), detailInfo.content.getExpiredTimeMs(), _suc =>
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