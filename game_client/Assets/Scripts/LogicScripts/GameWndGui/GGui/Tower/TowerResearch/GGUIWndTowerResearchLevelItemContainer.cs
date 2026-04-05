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
    public class GGUIWndTowerResearchLevelItemContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoTowerResearchLevelItem,GGUIMonoTowerResearchLevelItemContainer,GGUIWndTowerResearchLevelItem>
    {
        public List<GGUIWndTowerResearchLevelItem> _m_lItemGroupList;//子控件列表
		
        public GGUIWndTowerResearchLevelItemContainer(GGUIMonoTowerResearchLevelItemContainer _containerMono) : base(_containerMono)
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

            _m_lItemGroupList = new List<GGUIWndTowerResearchLevelItem>();
        }

        protected override GGUIWndTowerResearchLevelItem _createItemWnd(GGUIMonoTowerResearchLevelItem _itemMono)
        {
            GGUIWndTowerResearchLevelItem itemWnd = new GGUIWndTowerResearchLevelItem(_itemMono);
            return itemWnd;
        }


        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(TowerChapterRefObj _chapterRef)
        {
            if(_chapterRef == null)
                return;
            List<TowerResearchRefObj> researchList = _chapterRef.research_list;
            if (researchList == null)
                return;

            TowerResearchRefObj tempData = null;
            GGUIWndTowerResearchLevelItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < researchList.Count; ++i)
            {
                tempData = researchList[i];
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

                tempItemWnd.setInfo(_chapterRef, tempData);
                count++;
            }

            for (int i = _m_lItemGroupList.Count; i > count; i--)
            {
                removeItemWnd(_m_lItemGroupList[i - 1]);
                _m_lItemGroupList.RemoveAt(i - 1);
            }

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
    }
}
