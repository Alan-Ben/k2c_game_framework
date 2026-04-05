using System;
using System.Collections.Generic;
using Common.NpPlayerInfoObj;
using GC2GS.p004_PlayerOp;
using GS2GC.p004_PlayerOp;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    public static partial class GCommon
    {
        /// <summary>
        /// 请求玩家信息
        /// </summary>
        /// <param name="_cid"></param>
        /// <param name="_onCallback"></param>
        public static void reqPlayerInfo(long _cid , Action<NPCommonSimplePlayerInfo> _onCallback)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_010_ReqSomeOnePlayerInfo(_cid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_010_RetSomeOnePlayerInfo>((info) =>
                {
                    if (null == info)
                        return;

                    NPCommonSimplePlayerInfo someOneShowInfo = new NPCommonSimplePlayerInfo(info.getSomeOneShowInfo());
                    if (null != _onCallback)
                        _onCallback(someOneShowInfo);
                }));
        }
        
        /// <summary>
        /// 请求玩家信息，原始服务端返回数据
        /// </summary>
        /// <param name="_cid"></param>
        /// <param name="_onCallback"></param>
        public static void reqPlayerInfoSer(long _cid , Action<GS2GC_004_010_RetSomeOnePlayerInfo> _onCallback)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_010_ReqSomeOnePlayerInfo(_cid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_010_RetSomeOnePlayerInfo>((info) =>
                {
                    if (null == info)
                        return;
                    if (null != _onCallback)
                        _onCallback(info);
                }));
        }
        
        /// <summary>
        /// 显示跟随tip
        /// </summary>
        /// <param name="_playerInfo"></param>
        /// <param name="_targetTransRoot"></param>
        /// <param name="_interval"></param>
        /// <param name="_rangRectTrans">默认 NPGame.instance.mainCamera.uiRootRectTrans</param>
        public static void showPlayerInfoWndTip(Common.NpPlayerInfoObj.PlayerInfo_CommonShow _playerInfo,RectTransform _targetTransRoot,float _interval,RectTransform _rangRectTrans)
        {
            NPCommonSimplePlayerInfo info = new NPCommonSimplePlayerInfo(_playerInfo);
            showPlayerInfoWndTip(info, _targetTransRoot, _interval, _rangRectTrans);
        }
        
        /// <summary>
        /// 显示跟随tip
        /// </summary>
        /// <param name="_playerInfo"></param>
        /// <param name="_targetTransRoot"></param>
        /// <param name="_interval"></param>
        /// <param name="_rangRectTrans">默认 NPGame.instance.mainCamera.uiRootRectTrans</param>
        public static void showPlayerInfoWndTip(NPCommonSimplePlayerInfo _playerInfo,RectTransform _targetTransRoot,float _interval,RectTransform _rangRectTrans,Action _onClose = null)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(NPGGUIWndCommonPlayerBusinessCardTip.instance, () =>
            {
                NPGGUIWndCommonPlayerBusinessCardTip.instance.showWnd(_playerInfo, _targetTransRoot, _interval, _rangRectTrans,_onClose);

            }, EUIQueueStageType.MAIN, UINodeTagConst.C_ADD_COMMON_PLAYER_BUSINESS_CARD, true, false, false);
        }
        
        /// <summary>
        /// 显示中间弹窗
        /// </summary>
        /// <param name="_playerInfo"></param>
        /// <param name="_onClose"></param>
        public static void showPlayerInfoWnd(Common.NpPlayerInfoObj.PlayerInfo_CommonShow _playerInfo, Action _onClose = null)
        {
            NPCommonSimplePlayerInfo info = new NPCommonSimplePlayerInfo(_playerInfo);
            showPlayerInfoWnd(info, _onClose);
        }

        /// <summary>
        /// 显示玩家简要信息
        /// </summary>
        /// <param name="_simplePlayerInfo"></param>
        public static void showPlayerInfoWnd(NPCommonSimplePlayerInfo _simplePlayerInfo, Action _onClose = null)
        {
            showPlayerInfoWndTip(_simplePlayerInfo, null, 0, null,_onClose);
        }
        
        /// <summary>
        /// 请求玩家称号记录列表
        /// </summary>
        /// <param name="_cid"></param>
        /// <param name="_onCallback"></param>
        public static void reqPlayerTitleRecordList(long _cid , Action<List<long>> _onCallback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_012_ReqTitleRecordList(_cid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_012_RetTitleRecordList>((info) =>
                {
                    if (null == info)
                        return;

                    if (null != _onCallback)
                        _onCallback(info.getTitleIdList());
                }));
        }

    }
}