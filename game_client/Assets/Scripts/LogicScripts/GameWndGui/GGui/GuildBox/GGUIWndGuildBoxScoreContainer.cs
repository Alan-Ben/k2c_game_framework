using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟宝箱容器
    /// </summary>
    public class GGUIWndGuildBoxScoreContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoGuildBoxScoreContainerItem,GGUIMonoGuildBoxScoreContainer,GGUIWndGuildBoxScoreContainerItem>
    {
        public List<GGUIWndGuildBoxScoreContainerItem> _m_lItemGroupList;//子控件列表
		        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGuildBoxScoreContainer(GGUIMonoGuildBoxScoreContainer _containerMono) : base(_containerMono)
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
            if(null == wnd)
                return;

            _m_lItemGroupList = new List<GGUIWndGuildBoxScoreContainerItem>();
        }

        protected override GGUIWndGuildBoxScoreContainerItem _createItemWnd(GGUIMonoGuildBoxScoreContainerItem _itemMono)
        {
            GGUIWndGuildBoxScoreContainerItem itemWnd = new GGUIWndGuildBoxScoreContainerItem(_itemMono);
            return itemWnd;
        }


        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<GuildBoxRefObj> _itemDataList)
        {
            if (_itemDataList == null)
                return;

            GuildBoxRefObj tempData = null;
            GGUIWndGuildBoxScoreContainerItem tempItemWnd = null;
            int count = 0;
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

                tempItemWnd.setInfo(tempData, count);
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
        
        // <AutoGen:Method>
        
        // </AutoGen:Method>
    }
}
