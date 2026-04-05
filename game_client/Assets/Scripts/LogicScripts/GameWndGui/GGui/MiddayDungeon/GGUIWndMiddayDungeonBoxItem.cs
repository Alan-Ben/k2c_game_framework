using System;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item
    /// </summary>
    public class GGUIWndMiddayDungeonBoxItem : _ATALBasicUISubWnd<GGUIMonoMiddayDungeonBoxItem>
    {
        private NPGGUIWndPlayerIcon _m_playerInfo; //玩家信息
        private MiddayDungeonBoxInfo _m_boxInfo;
        private EMiddayDungeonBoxState _m_boxState = EMiddayDungeonBoxState.CanOpen;
        private long _m_lRefreshSerialize;
        private NPGGuiWndTexture _m_boxIconWnd;

        public GGUIWndMiddayDungeonBoxItem(GGUIMonoMiddayDungeonBoxItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lRefreshSerialize = ALSerializeOpMgr.next();
            _m_playerInfo?.hideWnd();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_playerInfo?.discard();
            _m_playerInfo = null;
            
            _m_boxIconWnd?.discard();
            _m_boxIconWnd = null;
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;          
            if (wnd.playerInfo !=null) _m_playerInfo = new NPGGUIWndPlayerIcon(wnd.playerInfo);
            if(wnd.boxIcon != null) _m_boxIconWnd = new NPGGuiWndTexture(wnd.boxIcon);
            ALUGUICommon.combineBtnClick(wnd.btnOpenBox, _onBtnOpenBoxClick);

        }

        public void setInfo(MiddayDungeonBoxInfo _data)
        {
            _m_boxInfo = _data;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (null == wnd || _m_boxInfo == null)
                return;
            if (null != _m_playerInfo)
            {
                _m_playerInfo.showWnd();
                _m_playerInfo.setPlayer(_m_boxInfo.playerCid);
            }
            MiddayDungeonBoxRefObj boxRef = GRefdataCoreMgr.instance.middayDungeonBoxRefCore.getRef(_m_boxInfo.boxId);

            if (null != _m_boxIconWnd)
            {   
                if (boxRef != null)
                {
                    _m_boxIconWnd?.setTexture(boxRef.box_icon);
                    _m_boxIconWnd?.showWnd();
                }
            }
            long serializeId = _m_lRefreshSerialize = ALSerializeOpMgr.next();
            _m_boxState = EMiddayDungeonBoxState.Invalid;

            // 判断宝箱是否过期
            if (_m_boxInfo.expireTs >= FpsAndPingMgr.instance.serverTimeTag)
            {
                // 宝箱未过期，检查本地缓存是否已经领完
                if (!AccountSettingMgr.instance.middayDungeonSaver.hasInValid(_m_boxInfo.dbId))
                {
                    NPPlayer.instance.middayDungeonComp.reqMiddayDungeonBoxCanDraw(_m_boxInfo.dbId, _m_boxInfo.expireTs,
                        (_suc, _canDraw, _remainDrawCount) =>
                        {
                            if (serializeId != _m_lRefreshSerialize)
                                return;
                            // 检查是否已经打开过
                            if (AccountSettingMgr.instance.middayDungeonSaver.hasOpen(_m_boxInfo.dbId))
                                _m_boxState = EMiddayDungeonBoxState.HasOpen;
                            else
                                _m_boxState = _canDraw ? EMiddayDungeonBoxState.CanOpen : EMiddayDungeonBoxState.NonCanOpen;

                            if (wnd != null)
                                ALUGUICommon.setLabelTxt(wnd.txtCount, TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_box_has_draw_count, _remainDrawCount, boxRef?.can_draw_limit));
                            NPCommonEnumStatInfo<EMiddayDungeonBoxState>.setStat(wnd.statInfos, _m_boxState);
                        });
                }
                else
                {
                    _m_boxState = EMiddayDungeonBoxState.NonCanOpen;
                }
            }

            NPCommonEnumStatInfo<EMiddayDungeonBoxState>.setStat(wnd.statInfos, _m_boxState);
        }

        private void _onBtnOpenBoxClick(GameObject _)
        {
            if (_m_boxInfo == null)
                return;
            if (_m_boxState == EMiddayDungeonBoxState.Invalid || _m_boxInfo.expireTs < FpsAndPingMgr.instance.serverTimeTag)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_box_invalid_tip));
                return;
            }
            if (_m_boxState == EMiddayDungeonBoxState.HasOpen || _m_boxState == EMiddayDungeonBoxState.NonCanOpen)
            {
                // 如果宝箱已领取或者被领取完毕，则打开记录界面
                if (_m_boxState != EMiddayDungeonBoxState.Invalid)
                {
                    GGUIWndMiddayDungeonBoxRecord.instance.setInfo(_m_boxInfo.boxId, _m_boxInfo.dbId);
                    QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndMiddayDungeonBoxRecord.instance, GGUIWndMiddayDungeonBoxRecord.instance.showWnd, UINodeTagConst.C_MIDDAY_DUNGEON_BOX_RECORD);
                }
                return;
            }
            NPPlayerFixedCDInfo fixedCdInfo = NPPlayer.instance.fixedCdComp.getCDInfoByRefId(_m_boxInfo.boxRefObj?.draw_box_fixed_cd_id ?? 0);
            if (fixedCdInfo != null && fixedCdInfo.getCount() <= 0)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.midday_dungeon_box_reach_max_count, fixedCdInfo.getMaxCount()));
                return;
            }
            NPPlayer.instance.middayDungeonComp.reqMiddayDungeonDrawBox(_m_boxInfo.dbId, _m_boxInfo.expireTs,
                _suc =>
                {
                    if (_suc)
                    {
                        _refreshWnd();
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
