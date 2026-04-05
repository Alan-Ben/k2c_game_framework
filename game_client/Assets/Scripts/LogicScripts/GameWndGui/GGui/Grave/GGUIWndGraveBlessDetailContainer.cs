using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 祝福详情界面容器
    /// </summary>
    public class GGUIWndGraveBlessDetailContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoGraveBlessDetailContainerItem,GGUIMonoGraveBlessDetailContainer,GGUIWndGraveBlessDetailContainerItem>
    {
        public List<GGUIWndGraveBlessDetailContainerItem> _m_lItemGroupList;//子控件列表
		        
        // <AutoGen:WndDeclaration>
        // </AutoGen:WndDeclaration>
        
        public GGUIWndGraveBlessDetailContainer(GGUIMonoGraveBlessDetailContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            // <AutoGen:_onShowWnd>
            // </AutoGen:_onShowWnd>
        }
    
        protected override void _onHideWnd()
        {
            // <AutoGen:_onHideWnd>
            // </AutoGen:_onHideWnd>
        }
    
        protected override void _onReset()
        {
            // <AutoGen:_onReset>
            // </AutoGen:_onReset>
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
        }
    
        protected override void _onDiscard()
        {
            // <AutoGen:_onDiscard>
            // </AutoGen:_onDiscard>
            if(_m_lItemGroupList != null)
                _m_lItemGroupList.Clear();
            _m_lItemGroupList = null;
        }
    
        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;
            // <AutoGen:_onWndInitDone>
            // </AutoGen:_onWndInitDone>
            _m_lItemGroupList = new List<GGUIWndGraveBlessDetailContainerItem>();
        }

        protected override GGUIWndGraveBlessDetailContainerItem _createItemWnd(GGUIMonoGraveBlessDetailContainerItem _itemMono)
        {
            GGUIWndGraveBlessDetailContainerItem itemWnd = new GGUIWndGraveBlessDetailContainerItem(_itemMono);
            return itemWnd;
        }


        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_itemDataList"></param>
        public void showItemList(List<CommonLongIntInfo> _itemDataList)
        {
            if (_itemDataList == null)
                return;

            
            GGUIWndGraveBlessDetailContainerItem tempItemWnd = null;
            int count = 0;
            for (int i = 0; i < _itemDataList.Count; ++i)
            {
                if (_itemDataList[i] == null)
                    continue;
                long tempData = _itemDataList[i].longValue;
                NPPlayerBuffInfo buffInfo = NPPlayer.instance.playerBuffComp.lookup(tempData);
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

                if(buffInfo != null &&  !buffInfo.hasExpired())
                    tempItemWnd.setInfo(tempData, buffInfo);
                else
                    tempItemWnd.setInfo(tempData);

                
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
