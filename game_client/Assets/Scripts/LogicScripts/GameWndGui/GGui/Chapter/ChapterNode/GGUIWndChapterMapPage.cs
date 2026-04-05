using System;
using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndChapterMapPage : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoChapterMapPage>
    {
        private NPCommonAssetPathInfo _m_assetPath;
        
        private ChapterRefObj _m_chapterRefObj;
        [NotNull]private List<GGUIWndChapterMapNodeItem> _m_lChapterItemList = new List<GGUIWndChapterMapNodeItem>();

        public GGUIWndChapterMapPage(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_assetPath = _assetPathInfo;
        }

        protected override string _monoAssetPath { get { return _m_assetPath?.asset_path; } }
        protected override string _monoObjName { get { return _m_assetPath?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        public NPCommonAssetPathInfo assetPath
        {
            get { return _m_assetPath; }
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null || null == wnd.nodeItemList)
                return;
        }
        
        protected override void _onDiscard()
        {
            foreach (var item in _m_lChapterItemList)
            {
                if (item != null) 
                    item.discard();
            }
            _m_lChapterItemList.Clear();
            _m_chapterRefObj = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            
        }

        public void initSetInfo(ChapterRefObj _chapterRef)
        {
            if(null == _chapterRef || null == wnd)
                return;

            if (_m_chapterRefObj == _chapterRef)
            {
                foreach (GGUIWndChapterMapNodeItem gguiWndChapterMapNodeItem in _m_lChapterItemList)
                {
                    if (gguiWndChapterMapNodeItem != null) 
                        gguiWndChapterMapNodeItem.refresh();
                }
            }
            else
            {
                _m_chapterRefObj = _chapterRef;
                if (wnd.nodeItemList?.Count != _m_chapterRefObj.nodeList?.Count + 1)
                {
                    Debug.LogError($"关卡资源配置错误, 关卡节点数量与UI节点数量不匹配, 关卡节点数量:{_m_chapterRefObj.nodeList?.Count}, UI节点数量:{wnd.nodeItemList?.Count}");
                    return;
                }
            
                foreach (var item in _m_lChapterItemList)
                {
                    if (item != null) 
                        item.discard();
                }
                _m_lChapterItemList.Clear();
            
                for (int i = 0; i < wnd.nodeItemList.Count; i++)
                {
                    GGUIWndChapterMapNodeItem nodeItem = new GGUIWndChapterMapNodeItem(wnd.nodeItemList[i]);
                    nodeItem.showWnd();
                    nodeItem.setChapterInfo(_m_chapterRefObj, i);
                    _m_lChapterItemList.Add(nodeItem);
                }   
            }
        }

        /// <summary>
        /// item切换为完成状态表现
        /// </summary>
        /// <param name="_playDone"></param>
        public void showItemChgToCompletedState(int _nodeIndex, Action _playDone = null)
        {
            if (_m_lChapterItemList.Count <= _nodeIndex)
            {
                if (_playDone != null) 
                    _playDone();
                return;
            }

            GGUIWndChapterMapNodeItem item = _m_lChapterItemList[_nodeIndex];
            if (null == item)
            {
                if (_playDone != null) 
                    _playDone();
                return;
            }
            
            item.showItemChgToCompletedState(_playDone);
        }

        /// <summary>
        /// item切换为进行中状态表现
        /// </summary>
        /// <param name="_playDone"></param>
        public void showItemChgToUnderwayState(int _nodeIndex, Action _playDone = null)
        {
            if (_m_lChapterItemList.Count <= _nodeIndex)
            {
                if (_playDone != null) 
                    _playDone();
                return;
            }

            GGUIWndChapterMapNodeItem item = _m_lChapterItemList[_nodeIndex];
            if (null == item)
            {
                if (_playDone != null) 
                    _playDone();
                return;
            }
            
            item.showItemChgToUnderwayState(_playDone);
        }
        
        /// <summary>
        /// 强制刷新item显示状态
        /// </summary>
        public void forceRefreshAllItemState(int _index, EChapterMapNodeState _state)
        {
            if (_m_lChapterItemList.Count <= _index)
            {
                return;
            }

            GGUIWndChapterMapNodeItem item = _m_lChapterItemList[_index];
            if (null == item)
            {
                return;
            }
            
            item.forceRefreshItemShowState(_state);
        }

        public void showBossAutoBattleState(Action _playDone = null)
        {
            if(_m_lChapterItemList.Count == 0)
            {
                if (_playDone != null) 
                    _playDone();
                return;
            }

            GGUIWndChapterMapNodeItem item = _m_lChapterItemList.GetLast();
            if(null == item)
            {
                if (_playDone != null) 
                    _playDone();
                return;
            }
            
            item.showBossAutoBattleState(_playDone);
        }
        
        public RectTransform getCurNodeDoingRectTransform()
        {
            foreach (GGUIWndChapterMapNodeItem gguiWndChapterMapNodeItem in _m_lChapterItemList)
            {
                if(null == gguiWndChapterMapNodeItem)
                    continue;

                if (gguiWndChapterMapNodeItem.eCurChapterState == EChapterMapNodeState.UNDERWAY)
                    return gguiWndChapterMapNodeItem.rectTransform;
            }

            return null;
        }
        
        public RectTransform getNodeRectTransform(int _index)
        {
            if (_m_lChapterItemList.Count <= _index)
            {
                return null;
            }

            return _m_lChapterItemList[_index]?.wnd?.posStart;
        }
        
        public void playNodeAutoSfx(int _index)
        {
            if (_m_lChapterItemList.Count <= _index)
                return;

            GGUIWndChapterMapNodeItem item = _m_lChapterItemList[_index];
            if (null == item)
            {
                return;
            }
            
            item.playAutoForwardSfx();
        }
    }
}