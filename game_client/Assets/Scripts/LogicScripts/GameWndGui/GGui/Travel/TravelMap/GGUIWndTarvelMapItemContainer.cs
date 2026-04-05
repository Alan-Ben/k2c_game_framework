//
// using System;
// using System.Collections.Generic;
// using ALPackage;
// using DG.Tweening;
// using UnityEngine;
// using UnityEngine.UI;
//
//
// namespace GOE
// {
//     //游历地图列表item 容器
//     public class GGUIWndTarvelMapItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoTarvelMapItem, GGUIMonoTarvelMapItemContainer, GGUIWndTarvelMapItem>
//     {
//         public List<GGUIWndTarvelMapItem> _m_lItemGroupList;//子控件列表
//         private int _m_serialize;
//         private RectTransform _m_itemContainer;
//         private Tweener _m_tweener;
//
//         public GGUIWndTarvelMapItemContainer(GGUIMonoTarvelMapItemContainer _containerMono)
//             : base(_containerMono)
//         {
//             initWnd();
//         }
//
//         protected override GGUIWndTarvelMapItem _createItemWnd(GGUIMonoTarvelMapItem _itemMono)
//         {
//             return new GGUIWndTarvelMapItem(_itemMono);
//         }
//
//         /******************
//         * 显示窗口的事件函数
//         **/
//         protected override void _onShowWnd()
//         {
//         }
//         /******************
//          * 隐藏窗口的事件函数
//          **/
//         protected override void _onHideWnd()
//         {
//             MainCameraMono.selfInstance.closeAllInputMask(_m_serialize);
//             _m_serialize = ALSerializeOpMgr.next();
//         }
//         /******************
//          * 重置窗口数据的事件函数
//          **/
//         protected override void _onReset()
//         {
//             if(_m_lItemGroupList != null)
//                 _m_lItemGroupList.Clear();
//         }
//         /******************
//          * 释放资源时触发的事件
//          **/
//         protected override void _onDiscard()
//         {
//             if(_m_lItemGroupList != null)
//                 _m_lItemGroupList.Clear();
//             _m_lItemGroupList = null;
//             _m_tweener?.Kill();
//             _m_tweener = null;
//         }
//         /*************
//          * 窗口初始化完成调用的函数
//          * */
//         protected override void _onWndInitDone()
//         {
//             if(wnd == null)
//                 return;
//
//             _m_lItemGroupList = new List<GGUIWndTarvelMapItem>();
//             _m_itemContainer = wnd.itemContainer.GetComponent<RectTransform>();
//         }
//         
//         /// <summary>
//         /// 显示item列表
//         /// </summary>
//         /// <param name="_itemDataList">物品显示数据列表</param>
//         public void showItemList(List<TravelPosRefObj> _itemDataList)
//         {
//             if(null == _itemDataList)
//                 return;
//             _itemDataList.Sort(_sortTravelMap);
//             TravelPosRefObj tempData = null;
//             GGUIWndTarvelMapItem tempItemWnd = null;
//             int count = 0;
//             for(int i = 0; i < _itemDataList.Count; ++i)
//             {
//                 tempData = _itemDataList[i];
//                 if(tempData == null)
//                     continue;
//
//                 if(i >= _m_lItemGroupList.Count)
//                 {
//                     tempItemWnd = addItemWnd();
//                     if(tempItemWnd == null)
//                         continue;
//                     //放入数据队列
//                     _m_lItemGroupList.Add(tempItemWnd);
//                 }
//                 else
//                 {
//                     tempItemWnd = _m_lItemGroupList[i];
//                 }
//                 tempItemWnd.setInfo(tempData);
//                 count++;
//             }
//             for(int i = _m_lItemGroupList.Count; i > count; i--)
//             {
//                 removeItemWnd(_m_lItemGroupList[i - 1]);
//                 _m_lItemGroupList.RemoveAt(i - 1);
//             }
//
//             _refreshContentLayout();
//         }
//
//         /// <summary>
//         /// 刷新容器布局
//         /// </summary>
//         public void _refreshContentLayout()
//         {
//             ALCommonActionMonoTask.addNextFrameTask(() =>
//             {
//                 if (wnd == null || wnd.itemContainer == null)
//                     return;
//
//                 LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
//                 _m_serialize = MainCameraMono.selfInstance.openAllInputMask();
//                 CommonTaskController.CommonActionAddNextFrameTask(_dealUnlockShow);
//             });
//         }
//
//         private int _sortTravelMap(TravelPosRefObj x, TravelPosRefObj y)
//         {
//             EGameCommonUnlockRewardType xStat = NPPlayer.instance.travelComp.getPosUnlockRewardStat(x);
//             EGameCommonUnlockRewardType yStat = NPPlayer.instance.travelComp.getPosUnlockRewardStat(y);
//             if (xStat > yStat)
//                 return -1;
//             if (xStat < yStat)
//                 return 1;
//             if (xStat == EGameCommonUnlockRewardType.LOCK && yStat == EGameCommonUnlockRewardType.LOCK)
//             {
//                 bool xNeedShowUnlock = NPPlayer.instance.travelComp.getNeedShowUnlock(x);
//                 bool yNeedShowUnlock = NPPlayer.instance.travelComp.getNeedShowUnlock(y);
//                 if (xNeedShowUnlock && !yNeedShowUnlock)
//                     return -1;
//                 if (!xNeedShowUnlock && yNeedShowUnlock)
//                     return 1;
//             }
//             if (xStat < yStat)
//                 return 1;
//             if (x.id < y.id)
//                 return -1;
//             if (x.id > y.id)
//                 return 1;
//             return 0;
//         }
//
//         private void _dealUnlockShow()
//         {
//             GGUIWndTarvelMapItem itemWnd = null;
//             for (int i = 0; i < _m_lItemGroupList.Count; i++)
//             {
//                 itemWnd = _m_lItemGroupList[i];
//                 if(null == itemWnd)
//                     continue;
//                 
//                 bool needShowUnlock = NPPlayer.instance.travelComp.getNeedShowUnlock(itemWnd.travelPosRef);
//                 if (needShowUnlock)
//                 {
//                     _moveToItem(itemWnd, () =>
//                     {
//                         itemWnd.dealShowUnlock(_dealUnlockShow);
//                     });
//                     return;
//                 }
//             }
//             
//             MainCameraMono.selfInstance.closeAllInputMask(_m_serialize);
//         }
//
//         private void _moveToItem(GGUIWndTarvelMapItem _itemWnd, Action _dealDone)
//         {
//             if (null == wnd || null == _itemWnd || null == _m_itemContainer || null == wnd.areaMaskObj)
//             {
//                 _dealDone?.Invoke();
//                 return;
//             }
//             
//             _m_tweener?.Kill();
//             _m_tweener = null;
//             
//             float mergeY = Mathf.Abs(_itemWnd.rectTransform.localPosition.y + _itemWnd.rectTransform.rect.height / 2);
//             float targetHorizontal = 0;
//             if(_m_itemContainer.rect.height > wnd.areaMaskObj.rect.height)
//                 targetHorizontal= mergeY / (_m_itemContainer.rect.height - wnd.areaMaskObj.rect.height);
//             
//             float nowHorizontal = 1 - wnd.scrollRect.verticalNormalizedPosition;
//             float newHorizontal = nowHorizontal;
//             float endY = targetHorizontal;
//             float time = 0.2f;
//             _m_tweener = DOTween.To(_value => newHorizontal = _value, nowHorizontal, endY, time).OnUpdate(() =>
//             {
//                 moveToVerticalRate(newHorizontal);
//             }).SetAutoKill(true).OnKill(() =>
//             {
//                 // CommonTaskController.CommonActionAddMonoTask(_dealDone, 0.2f);
//                 _dealDone?.Invoke();
//             });
//         }
//     }
// }
