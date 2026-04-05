using System;
using System.Collections.Generic;
using ALPackage;
using Common.DinnerObj;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIWndTowerChallengeItemGrid : _ATNPGGUIWndShowAnimContainer<GGUIMonoTowerChapterItem,GGUIMonoTowerChallengeItemContainer,GGUIWndTowerChapterItem>
    {
        private List<TowerLevelInfo> _m_itemDataList = new List<TowerLevelInfo>();
        public List<GGUIWndTowerChapterItem> _m_lItemGroupList;//子控件列表

        public GGUIWndTowerChallengeItemGrid(GGUIMonoTowerChallengeItemContainer gridMono) : base(gridMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            
        }

        protected override void _onHideWnd()
        {
            
        }

        protected override void _onReset()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }

        protected override void _onDiscard()
        {
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            _m_lItemGroupList = new List<GGUIWndTowerChapterItem>();
        }

        protected override GGUIWndTowerChapterItem _createItemWnd(GGUIMonoTowerChapterItem _itemMono)
        {
            GGUIWndTowerChapterItem itemWnd = new GGUIWndTowerChapterItem(_itemMono);
            return itemWnd;
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<TowerLevelInfo> _itemDataList)
        {
            if (_itemDataList == null)
                return;
            _m_itemDataList = _itemDataList;
            
            TowerLevelInfo tempData = null;
            GGUIWndTowerChapterItem tempItemWnd = null;
            
 
            // 删除所有现有的item窗口
            for (int i = _m_lItemGroupList.Count; i > 0; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            if (_itemDataList.Count <= 0)
                return;
            long curChapter = _itemDataList[0].chapter;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                tempData = _itemDataList[i];
                if (tempData == null)
                    continue;
                // 如果章节不一致则添加章节item
                if(curChapter != tempData.chapter)
                {
                    curChapter = tempData.chapter;
                    if (wnd.barItemPrefab != null)
                    {
                        GGUIWndTowerChapterItem barWnd = addItemWnd(wnd.barItemPrefab);
                        _m_lItemGroupList.Add(barWnd);
                        barWnd.setInfo(tempData, _challengeTowerClick);
                    }
                }
              
                tempItemWnd = addItemWnd();
                
                if (tempItemWnd == null)
                    continue;
                //放入数据队列
                _m_lItemGroupList.Add(tempItemWnd);
               

                tempItemWnd.setInfo(tempData, _challengeTowerClick);
            }
            
            ALUGUICommon.setGameObjEnable(wnd.emptyListHide,_m_itemDataList.Count != 0);
            ALUGUICommon.setGameObjEnable(wnd.emptyListShow,_m_itemDataList.Count == 0);
            _refreshContentLayout();
        }
        
        /// <summary>
        /// 刷新容器布局
        /// </summary>
        public void _refreshContentLayout()
        {
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                if (wnd == null || wnd.itemContainer == null)
                    return;
        
                LayoutRebuilder.ForceRebuildLayoutImmediate(wnd.itemContainer.GetComponent<RectTransform>());
            });
        }

        //移动到顶部
        public void moveToTopNextFrame()
        {
            ALCommonTaskController.CommonActionAddNextFrameTask(moveToTop);
        }

        //移动到底部
        public void moveToBottomNextFrame()
        {
            ALCommonTaskController.CommonActionAddNextFrameTask(moveToBottom);
        }

        //模拟点击挑战当前列表第几个boss的效果(序列号0为当前界面最底下一层)
        public void setClickChallengeByIndex(int _index)
        {
            if (_m_itemDataList == null || _index < 0 || _index >= _m_itemDataList.Count)
                return;

            int targetIndex = _m_itemDataList.Count - 1 - _index;
            if (targetIndex < 0 && _m_itemDataList[targetIndex] != null)
                return;
            _challengeTowerClick(_m_itemDataList[targetIndex]);
        }
        
        /// <summary>
        /// 挑战爬塔点击事件
        /// </summary>
        /// <param name="_towerLevelInfo"></param>
        private void _challengeTowerClick(TowerLevelInfo _towerLevelInfo)
        {
            if(_towerLevelInfo == null)
                return;
            NPPlayer.instance.towerComp.reqChallengeTower(_towerLevelInfo.chapter, _towerLevelInfo.level, _result =>
            {
                if(_result == null)
                    return;
                if (AccountSettingMgr.instance.accountSetting.towerSkipBattleShow && GCommon.isSimpleUnlock(GRefdataCoreMgr.instance.npGeneral.tower_skip_battle_simple_unlock_id))
                {
                    if (_result.isSuc)
                    {
                        NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_TowerBattleSuc(_result));
                        if(_result.oldChapterId != _result.targetChapterId)
                            NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_TowerBattleCrossChapter(_result.targetChapterId));
                    }
                    else
                    {
                        NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_TowerBattleFail(_result));
                    }
                }
                else
                {
                    if (_result.isSuc)
                    {
                        bool pve = _result.targetPlayerCid == 0;
                        if (pve)
                        {
                            GGUIWndTowerBattlePVE.instance.setInfo(_result);
                            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndTowerBattlePVE.instance, UINodeTagConst.C_TOWER_BATTLE_PVE, null, null , 0);
                        }
                        else
                        {
                            GGUIWndTowerBattlePVP.instance.setInfo(_result);
                            QueueMgr.instance.addNode_InGame_MainUIMainWnd(GGUIWndTowerBattlePVP.instance, UINodeTagConst.C_TOWER_BATTLE_PVP, null, null , 0);
                        }
                    }
                    else
                    {
                        NPUINoticeMgr.instance.addDealer(new NPNoticeDealer_TowerBattleFail(_result));
                    }
                   
                }
            });
        }
    }
}
