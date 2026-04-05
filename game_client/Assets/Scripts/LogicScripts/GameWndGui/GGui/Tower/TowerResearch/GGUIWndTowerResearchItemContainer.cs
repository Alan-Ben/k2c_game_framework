using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// item容器
    /// </summary>
    public class GGUIWndTowerResearchItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoTowerResearchItem,GGUIMonoTowerResearchItemContainer,GGUIWndTowerResearchItem>
    {
        public List<GGUIWndTowerResearchItem> _m_lItemGroupList;//子控件列表
		
        public GGUIWndTowerResearchItemContainer(GGUIMonoTowerResearchItemContainer _containerMono) : base(_containerMono)
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

            _m_lItemGroupList = new List<GGUIWndTowerResearchItem>();
        }

        protected override GGUIWndTowerResearchItem _createItemWnd(GGUIMonoTowerResearchItem _itemMono)
        {
            GGUIWndTowerResearchItem itemWnd = new GGUIWndTowerResearchItem(_itemMono);
            return itemWnd;
        }


        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<TowerChapterRefObj> _itemDataList)
        {
            if (_itemDataList == null)
                return;

            TowerChapterRefObj tempData = null;
            GGUIWndTowerResearchItem tempItemWnd = null;
            int count = 0;
            long curChapter = NPPlayer.instance.towerComp.curChapterId;
            GGUIWndTowerResearchItem curChapterItem = null;
            TowerPosInfo activePosInfo = NPPlayer.instance.towerComp.activatePosInfo;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                tempData = _itemDataList[i];
                if (tempData == null)
                    continue;
                if (count >= _m_lItemGroupList.Count)
                {
                    tempItemWnd = addItemWnd();
                    if (tempItemWnd == null)
                        continue;
                    //放入数据队列
                    _m_lItemGroupList.Add(tempItemWnd);
                }
                else
                {
                    tempItemWnd = _m_lItemGroupList[i];
                }

                bool showLevel = false;
                if (curChapterItem == null)
                {
                    if (NPPlayer.instance.towerComp.canGetResearchReward(tempData.id) && !NPPlayer.instance.towerComp.hasGetResearchReward(tempData.id))
                    {
                        curChapterItem = tempItemWnd;
                        showLevel = true;
                    }

                    if (NPPlayer.instance.towerComp.getChapterActiveState(tempData.id) ==
                        ETowerResearchActiveType.CAN_ACTIVATE)
                    {
                        curChapterItem = tempItemWnd;
                        showLevel = true;
                    }

                    if (curChapter == tempData.id)
                    {
                        curChapterItem = tempItemWnd ;
                        showLevel = true;
                    }
                }
                tempItemWnd.setInfo(tempData, showLevel);

                count++;
            }
            
            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                ALCommonActionMonoTask.addNextFrameTask(() =>
                {
                    moveViewToItem(curChapterItem);
                });
            });
        }
        
        private void moveViewToItem(GGUIWndTowerResearchItem _itemWnd)
        {
            if (wnd == null || wnd.itemContainer == null || _itemWnd == null)
                return;

            if (_itemWnd.wnd == null) return;
            
            RectTransform itemRect = _itemWnd.wnd.GetComponent<RectTransform>();
            if (itemRect == null)
                return;

            Rect rect = itemRect.getRectInParent();
            if (wnd.scrollRect != null && wnd.scrollRect.content != null && wnd.scrollRect.viewport != null)
            {
                float normalizeY = (wnd.scrollRect.content.rect.height + rect.yMax - wnd.scrollRect.viewport.rect.height)/ (wnd.scrollRect.content.rect.height - wnd.scrollRect.viewport.rect.height) ;
                normalizeY = Mathf.Clamp01(normalizeY);
                wnd.scrollRect.verticalNormalizedPosition = normalizeY;
            }
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
    }
}
