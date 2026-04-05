// using System;
// using System.Collections.Generic;
// using ALPackage;
// using UnityEngine;
//
// namespace GOE
// {
//     /// <summary>
//     /// 跑马灯附加窗口
//     /// </summary>
//     public class GGUIWndSubMarquee : _ATALBasicUISubWnd<GGUIMonoSubMarquee>
//     {
//         //正在展示中的item列表
//         private List<GGUIWndSubMarqueeItem> _m_lMovingItemList;
//         //展示位置ID
//         private int _m_lShowPosId;
//         //跑马灯位置配置数据
//         private MarqueePosRefObj _m_posRef;
//         //是否正在处理移动任务
//         private bool _m_bIsDealingTask;
//         //任务序列号
//         private long _m_lSerialize;
//         //连续点击特殊处理按钮
//         private GGUIWndContinuousClickBtn _m_wContinuousClickBtn;
//
//
//         public GGUIWndSubMarquee(int _showPosId, GGUIMonoSubMarquee _wnd) : base(_wnd)
//         {
//             _m_lShowPosId = _showPosId;
//             _m_posRef = GRefdataCoreMgr.instance.marqueePosRefCore.getRef(_showPosId);
//             initWnd();
//         }
//
//         protected override void _onShowWnd()
//         {
//             WinMsg.RegisterMsg(WinMsgType.ON_ADD_MARQUEE, _onAddMarquee);
//             WinMsg.RegisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
//
//             if (wnd == null)
//                 return;
//
//             _m_lSerialize = ALSerializeOpMgr.next();
//             _m_bIsDealingTask = false;
//             _m_wContinuousClickBtn?.hideWnd();
//             ALUGUICommon.setGameObjEnable(wnd.goHaveMarqueeShowList, false);
//             _checkCanShowInThisNode();
//             _tryPopMarquee(null);
//         }
//
//         protected override void _onHideWnd()
//         {
//             WinMsg.UnregisterMsg(WinMsgType.ON_ADD_MARQUEE, _onAddMarquee);
//             WinMsg.UnregisterMsgAct(WinMsgType.ON_NODE_CHG, _onNodeChg);
//
//             _m_lSerialize = ALSerializeOpMgr.next();
//             _m_bIsDealingTask = false;
//             _clearMovingItemList();
//         }
//
//         protected override void _onReset()
//         {
//         }
//
//         protected override void _onDiscard()
//         {
//             _m_lSerialize = ALSerializeOpMgr.next();
//             _m_bIsDealingTask = false;
//
//             _clearMovingItemList();
//             _m_lMovingItemList = null;
//
//             if (_m_wContinuousClickBtn != null)
//             {
//                 _m_wContinuousClickBtn.discard();
//                 _m_wContinuousClickBtn = null;
//             }
//         }
//
//         protected override void _onWndInitDone()
//         {
//             if (wnd == null)
//                 return;
//
//             _m_lSerialize = ALSerializeOpMgr.next();
//             _m_lMovingItemList = new List<GGUIWndSubMarqueeItem>();
//
//             if (wnd.monoContinuousClickBtn != null)
//             {
//                 _m_wContinuousClickBtn = new GGUIWndContinuousClickBtn(wnd.monoContinuousClickBtn);
//                 _m_wContinuousClickBtn.onNormalClick += _onClickClose;
//                 _m_wContinuousClickBtn.onSpecialClick += _onContinuousClickClose;
//             }
//         }
//
//         /// <summary>
//         /// 尝试增加一个跑马灯展示
//         /// </summary>
//         private void _tryPopMarquee(Action _onPopDone)
//         {
//             if (wnd == null)
//             {
//                 _onPopDone?.Invoke();
//                 return;
//             }
//
//             //获取跑马灯数据
//             MarqueeInfo curMarqueeInfo = NPPlayer.instance.marqueeComp.getMarqueeInfo(_m_lShowPosId);
//             if (curMarqueeInfo == null)
//             {
//                 ALUGUICommon.setGameObjEnable(wnd.goHaveMarqueeShowList, false);
//                 _onPopDone?.Invoke();
//                 return;
//             }
//
//             //获取需要展示的ui样式
//             long uiResId = curMarqueeInfo.uiResId;
//             if (uiResId <= 0)
//             {
//                 ALUGUICommon.setGameObjEnable(wnd.goHaveMarqueeShowList, false);
//                 _onPopDone?.Invoke();
//                 return;
//             }
//
//             long serialize = _m_lSerialize;
//             //获取对应跑马灯item
//             GMarqueeItemCacheMgr.instance.popItem(uiResId, wnd.itemParent, _item =>
//             {
//                 if (_item == null)
//                     return;
//
//                 if (!isShow || wnd == null || serialize != _m_lSerialize)
//                 {
//                     _pushBackMarquee(_item);
//                     return;
//                 }
//
//                 if (_m_lMovingItemList == null)
//                     _m_lMovingItemList = new List<GGUIWndSubMarqueeItem>();
//
//                 _item.showWnd();
//                 _item.setInfo(curMarqueeInfo);
//                 _m_lMovingItemList.Add(_item);
//
//                 //设置开始展示位置在最右
//                 if (wnd.transViewportArea != null)
//                     _item.leftPosition = Vector2.zero + wnd.transViewportArea.rect.width * Vector2.right;
//
//                 //打开移动任务
//                 ALUGUICommon.setGameObjEnable(wnd.goHaveMarqueeShowList, true);
//                 if (!_m_bIsDealingTask)
//                 {
//                     _m_bIsDealingTask = true;
//                     ALMonoTaskMgr.instance.addMonoTask(new MarqueeMoveTask(_m_lSerialize, this, () =>
//                     {
//                         _m_bIsDealingTask = false;
//                         _m_wContinuousClickBtn?.hideWnd();
//                     }));
//                 }
//
//                 _onPopDone?.Invoke();
//             });
//         }
//
//         /// <summary>
//         /// 回收跑马灯item
//         /// </summary>
//         /// <param name="_item"></param>
//         private void _pushBackMarquee(GGUIWndSubMarqueeItem _item)
//         {
//             if (_item == null)
//                 return;
//
//             if (_item.marqueeInfo != null)
//                 GMarqueeItemCacheMgr.instance.pushBackCacheItem(_item.marqueeInfo.uiResId,_item);
//
//             _m_lMovingItemList?.Remove(_item);
//         }
//
//         /// <summary>
//         /// 设置跑马灯已读
//         /// </summary>
//         /// <param name="_item"></param>
//         private void _setReadMarque(GGUIWndSubMarqueeItem _item)
//         {
//             if (wnd == null || _item == null || _item.marqueeInfo == null)
//                 return;
//
//             //设置跑马灯已读
//             float intervalTimeSec = wnd.intervalTimeSec > 0 ? wnd.intervalTimeSec : 1f;
//             _item.marqueeInfo.setRead(intervalTimeSec);
//             //检查是否还有效，无效则删除跑马灯
//             if (!_item.marqueeInfo.checkIsValid())
//                 NPPlayer.instance.marqueeComp.checkRemoveMarquee(_m_lShowPosId, _item.marqueeInfo.instanceId);
//         }
//
//         /// <summary>
//         /// 判断文字是否出了显示范围
//         /// </summary>
//         /// <param name="_item"></param>
//         /// <returns></returns>
//         private bool _checkTextIsOutView(GGUIWndSubMarqueeItem _item)
//         {
//             if (_item == null)
//                 return false;
//             // 文字的最右端的X坐标小于0，那么就出范围了
//             return (_item.width + _item.leftPosition.x) < 0;
//         }
//
//         /// <summary>
//         /// 判断文字是否在显示范围内
//         /// </summary>
//         /// <param name="_item"></param>
//         /// <returns></returns>
//         private bool _checkTextIsInView(GGUIWndSubMarqueeItem _item)
//         {
//             if (wnd == null || wnd.transViewportArea == null || _item == null)
//                 return false;
//
//             return (_item.width + _item.leftPosition.x) < wnd.transViewportArea.rect.width;
//         }
//
//         /// <summary>
//         /// 检查在该窗口是否能展示跑马灯
//         /// </summary>
//         /// <returns></returns>
//         private bool _checkCanShowInThisNode()
//         {
//             if (wnd == null)
//                 return false;
//
//             if (_m_posRef == null || _m_posRef.no_display_node_tag_list == null || _m_posRef.no_display_node_tag_list.Count == 0)
//             {
//                 ALUGUICommon.setGameObjEnable(wnd.goCanNotShowHideList, true);
//                 return true;
//             }
//
//             //如果该跑马灯类型不需要在该窗口展示，则暂停展示
//             BaseQueueNode lastNode = QueueMgr.instance._lastNode;
//             if (lastNode != null && _m_posRef.no_display_node_tag_list.Contains(lastNode.nodeTag))
//             {
//                 ALUGUICommon.setGameObjEnable(wnd.goCanNotShowHideList, false);
//                 return false;
//             }
//             else
//             {
//                 ALUGUICommon.setGameObjEnable(wnd.goCanNotShowHideList, true);
//                 return true;
//             }
//         }
//
//         //清空展示列表
//         private void _clearMovingItemList()
//         {
//             if (_m_lMovingItemList != null)
//             {
//                 for (int i = _m_lMovingItemList.Count - 1; i >= 0; i--)
//                 {
//                     _pushBackMarquee(_m_lMovingItemList[i]);
//                 }
//                 _m_lMovingItemList.Clear();
//             }
//         }
//
//         #region 点击事件
//
//         /// <summary>
//         /// 点击关闭按钮
//         /// </summary>
//         private void _onClickClose()
//         {
//             //停止任务，隐藏关闭按钮
//             _m_lSerialize = ALSerializeOpMgr.next();
//             _m_wContinuousClickBtn?.hideWnd();
//
//             //对当前展示中的item清除
//             if (_m_lMovingItemList != null)
//             {
//                 for (int i = _m_lMovingItemList.Count - 1; i >= 0; i--)
//                 {
//                     GGUIWndSubMarqueeItem item = _m_lMovingItemList[i];
//                     if(item == null)
//                         continue;
//
//                     //移除数据
//                     if (item.marqueeInfo != null)
//                         NPPlayer.instance.marqueeComp.forceRemoveMarquee(_m_lShowPosId, item.marqueeInfo.instanceId);
//
//                     //回收item
//                     _pushBackMarquee(item);
//                 }
//                 _m_lMovingItemList.Clear();
//             }
//
//             //尝试加载新的跑马灯
//             ALCommonActionMonoTask.addNextFrameTask(() =>
//             {
//                 _tryPopMarquee(null);
//             });
//         }
//
//         /// <summary>
//         /// 一定时间内点击多次关闭按钮
//         /// </summary>
//         private void _onContinuousClickClose()
//         {
//             //弹出确认弹窗
//             NPMesMgr.instance.showTwoBtnMes(
//                 TextTranslate.instance.getLanguage(TransKeyConst.marquee_deleteAllConfirmDesc_none), 
//                 TextTranslate.instance.getLanguage(TransKeyConst.cancel), 
//                 null,
//                 TextTranslate.instance.getLanguage(TransKeyConst.confirm), 
//                 () =>
//                 {
//                     _m_lSerialize = ALSerializeOpMgr.next();
//                     _m_wContinuousClickBtn?.hideWnd();
//                     _clearMovingItemList();
//                     ALUGUICommon.setGameObjEnable(wnd?.goHaveMarqueeShowList, false);
//                     NPPlayer.instance.marqueeComp.forceRemoveAllMarquee(_m_lShowPosId);
//                 });
//         }
//
//         #endregion
//
//         #region 消息事件
//
//         /// <summary>
//         /// 新增跑马灯事件
//         /// </summary>
//         /// <param name="_objects"></param>
//         private void _onAddMarquee(params object[] _objects)
//         {
//             if (_objects == null || _objects.Length == 0 || _objects[0] == null)
//                 return;
//
//             int showPosId = (int) _objects[0];
//             if (showPosId == _m_lShowPosId && !_m_bIsDealingTask)
//             {
//                 //如果是当前位置的跑马灯，尝试展示
//                 _tryPopMarquee(null);
//             }
//         }
//
//         /// <summary>
//         /// 节点变化
//         /// </summary>
//         private void _onNodeChg()
//         {
//             _checkCanShowInThisNode();
//         }
//
//         #endregion
//
//
//
//
//
//
//
//         #region 跑马灯移动任务
//
//         /// <summary>
//         /// 跑马灯移动任务
//         /// </summary>
//         private class MarqueeMoveTask : _IALBaseMonoTask
//         {
//             //任务序列号
//             private long _m_lSerialize;
//             //当前窗口
//             private GGUIWndSubMarquee _m_wInstance;
//             //下一次创建跑马灯的时间点
//             private float _m_fNextAddMarqueeTime;
//             //全部展示完事件
//             private Action _m_aOnAllShowDone;
//             //是否正在添加新跑马灯
//             private bool _m_isAddingItem;
//
//             public MarqueeMoveTask(long _serialize, GGUIWndSubMarquee _instance, Action _onAllShowDone)
//             {
//                 _m_lSerialize = _serialize;
//                 _m_wInstance = _instance;
//                 _m_aOnAllShowDone = _onAllShowDone;
//                 _m_fNextAddMarqueeTime = 0;
//                 _m_isAddingItem = false;
//             }
//
//             public void deal()
//             {
//                 if (_m_wInstance == null ||
//                     _m_wInstance._m_lSerialize != _m_lSerialize ||
//                     _m_wInstance.wnd == null ||
//                     _m_wInstance._m_lMovingItemList == null ||
//                     (_m_wInstance._m_lMovingItemList.Count == 0 && _m_fNextAddMarqueeTime == 0 && !_m_isAddingItem))
//                 {
//                     _m_aOnAllShowDone?.Invoke();
//                     return;
//                 }
//
//                 //判断最后一个跑马灯是否全部进入视野，计算下一个展示的时间点
//                 GGUIWndSubMarqueeItem lastItem = _m_wInstance._m_lMovingItemList.GetLast();
//                 if (lastItem != null && _m_wInstance._checkTextIsInView(lastItem) && _m_fNextAddMarqueeTime == 0)
//                 {
//                     //全部进入视野，跑马灯设置为已读
//                     _m_wInstance._setReadMarque(lastItem);
//                     //展示间隔时间
//                     float intervalTimeSec = _m_wInstance.wnd.intervalTimeSec > 0 ? _m_wInstance.wnd.intervalTimeSec : 1f;
//                     //下一次创建跑马灯的时间点
//                     _m_fNextAddMarqueeTime = Time.realtimeSinceStartup + intervalTimeSec;
//                 }
//
//                 //判断是否需要展示关闭按钮
//                 if (lastItem != null && lastItem.marqueeInfo != null && _m_wInstance._m_wContinuousClickBtn != null)
//                 {
//                     if(lastItem.marqueeInfo.canDel)
//                         _m_wInstance._m_wContinuousClickBtn.showWnd();
//                     else
//                         _m_wInstance._m_wContinuousClickBtn.hideWnd();
//                 }
//
//                 //下一个创建时间到，创建新的跑马灯，放入移动列表里
//                 if (_m_fNextAddMarqueeTime > 0 && Time.realtimeSinceStartup - _m_fNextAddMarqueeTime > 0)
//                 {
//                     _m_isAddingItem = true;
//                     _m_fNextAddMarqueeTime = 0f;
//                     _m_wInstance._tryPopMarquee(()=>
//                     {
//                         if(_m_wInstance._m_lSerialize == _m_lSerialize)
//                             _m_isAddingItem = false;
//                     });
//                 }
//
//                 //每一帧判断，移动中的跑马灯，如果移出视野，就删除放回缓存池
//                 GGUIWndSubMarqueeItem tmpItemWnd = null;
//                 for (int i = _m_wInstance._m_lMovingItemList.Count - 1; i >= 0 ; i--)
//                 {
//                     tmpItemWnd = _m_wInstance._m_lMovingItemList[i];
//                     Vector2 pos = tmpItemWnd.leftPosition;
//                     Vector2 offSet = Vector2.left * _m_wInstance.wnd.fMoveSpeed * Time.deltaTime;
//                     pos += offSet;
//                     tmpItemWnd.leftPosition = pos;
//
//                     //判断文字是否出了显示范围
//                     if (_m_wInstance._checkTextIsOutView(tmpItemWnd))
//                         _m_wInstance._pushBackMarquee(tmpItemWnd);
//                 }
//
//                 //如果跑马灯移动列表里都跑出了可视视野，并且没有在添加新的跑马灯，跑马灯任务结束
//                 if ((_m_wInstance._m_lMovingItemList == null || _m_wInstance._m_lMovingItemList.Count == 0) && _m_fNextAddMarqueeTime == 0 && !_m_isAddingItem)
//                 {
//                     _m_aOnAllShowDone?.Invoke();
//                     return;
//                 }
//
//                 ALMonoTaskMgr.instance.addNextFrameTask(this);
//             }
//         }
//
//         #endregion
//     }
// }