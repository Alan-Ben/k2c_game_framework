// using System;
// using System.Collections.Generic;
// using ALPackage;
// using NPEnum;
// using UnityEngine;
//
// namespace GOE
// {
//     public class GGUIWndTarvelMapItem: _ANPGGUIBasicSubWnd<GGUIMonoTarvelMapItem>
//     {
//         private TravelPosRefObj _m_travelPosRef;
//         private GGUIWndTravelMessActorContainer _m_itemContainer;
//         private NPGGuiWndTexture _m_iconWnd;
//
//         public GGUIWndTarvelMapItem(GGUIMonoTarvelMapItem _wnd) : base(_wnd)
//         {
//             initWnd();
//         }
//
//         public TravelPosRefObj travelPosRef { get=> _m_travelPosRef;}
//
//         protected override void _onShowWnd()
//         {
//         }
//
//         protected override void _onHideWnd()
//         {
//             
//             _m_iconWnd?.hideWnd();
//         }
//
//         protected override void _onReset()
//         {
//             _m_iconWnd?.discardTexture();
//         }
//
//         protected override void _onDiscard()
//         {
//             _m_itemContainer?.discard();
//             _m_itemContainer = null;
//             _m_iconWnd.discard();
//             _m_iconWnd = null;
//
//         }
//
//         protected override void _onWndInitDone()
//         {
//             if(null == wnd)
//                 return;
//             ALUGUICommon.combineBtnClick(wnd.btnLook, _onClickLookPosInfo);
//             ALUGUICommon.combineBtnClick(wnd.btnGetUnlockReward, _onClickGetUnlockReward);
//             //item 容器
//             if (wnd.itemContainer != null)
//             {
//                 _m_itemContainer = new GGUIWndTravelMessActorContainer(wnd.itemContainer);
//             }
//
//             if (null != wnd.icon)
//             {
//                 _m_iconWnd = new NPGGuiWndTexture(wnd.icon);
//             }
//             
//         }
//
//         /// <summary>
//         /// 设置显示信息
//         /// </summary>
//         /// <param name="_travelPosRef"></param>
//         public void setInfo(TravelPosRefObj _travelPosRef)
//         {
//             _m_travelPosRef = _travelPosRef;
//             _refreshWnd();
//         }
//
//         private void _refreshWnd()
//         {
//             if (null == wnd)
//                 return;
//             EGameCommonUnlockRewardType stat = NPPlayer.instance.travelComp.getPosUnlockRewardStat(_m_travelPosRef);
//             if (null != _m_itemContainer)
//             {
//                 _m_itemContainer.showWnd();
//                 _m_itemContainer.setInfo(_m_travelPosRef,false,false, true, stat != EGameCommonUnlockRewardType.LOCK);
//             }
//             if (null != _m_iconWnd)
//             {
//                 _m_iconWnd.showWnd();
//                 _m_iconWnd.setTexture(_m_travelPosRef.icon);
//             }
//             
//             ALUGUICommon.setLabelTxt(wnd.txtPosName, TextTranslate.instance.getLanguage(_m_travelPosRef.name));
//
//             NPCommonEnumStatInfo<EGameCommonUnlockRewardType>.setStat(wnd.statInfos, stat);
//
//             if (stat == EGameCommonUnlockRewardType.LOCK)
//             {
//                 GGameCommonInfo.grayImage(wnd.lockGrayList);
//             }
//             else
//             {
//                 GGameCommonInfo.disgrayImage(wnd.lockGrayList);
//             }
//                 
//         }
//
//         public void dealShowUnlock(Action _dealDone)
//         {
//             bool needShowUnlock = NPPlayer.instance.travelComp.getNeedShowUnlock(_m_travelPosRef);
//             if (needShowUnlock)
//             {
//                 _playAni(wnd.unlockAniName, () =>
//                 {
//                     NPPlayer.instance.travelComp.setPosUnlockShowed(_m_travelPosRef.id, _refreshWnd);
//                     _dealDone?.Invoke();
//                 });
//             }
//             else
//             {
//                 _dealDone?.Invoke();
//             }
//         }
//
//         private void _playAni(string _aniName, Action _dealDone)
//         {
//             if (null == wnd || null == wnd.wndAnimation)
//             {
//                 _dealDone?.Invoke();
//                 return;
//             }
//
//             wnd.wndAnimation.Play(_aniName, _dealDone);
//         }
//
//         /// <summary>
//         /// 点击领取解锁奖励
//         /// </summary>
//         /// <param name="obj"></param>
//         private void _onClickGetUnlockReward(GameObject obj)
//         {
//             NPPlayer.instance.travelComp.reqGainUnlockedPosReward(_m_travelPosRef, (_info) =>
//             {   
//                 _refreshWnd();
//             });
//         }
//
//         /// <summary>
//         /// 查看pos信息
//         /// </summary>
//         /// <param name="obj"></param>
//         private void _onClickLookPosInfo(GameObject obj)
//         {
//             GGUIWndTravelPosInfo posInfoWnd = new GGUIWndTravelPosInfo(_m_travelPosRef);
//             QueueMgr.instance.addNode_InGame_SingleWnd(posInfoWnd, posInfoWnd.showWnd);
//         }
//     }
// }