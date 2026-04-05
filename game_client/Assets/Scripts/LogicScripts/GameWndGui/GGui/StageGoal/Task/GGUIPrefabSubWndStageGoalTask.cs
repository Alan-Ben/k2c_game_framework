
using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndStageGoalTask : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoStageGoalTask>
    {
        [ItemNotNull, NotNull] private readonly List<GGUISubWndStageGoalTaskItem> _m_taskItemList;
        [NotNull] private readonly List<StageGoalTaskItem> _m_taskList;
        
        private readonly long _m_uiResId;
        
        
        public GGUIPrefabSubWndStageGoalTask(Transform _parent, long _uiResId) 
            : base(_parent)
        {
            _m_taskList = new List<StageGoalTaskItem>();
            _m_taskItemList = new List<GGUISubWndStageGoalTaskItem>();
            
            _m_uiResId = _uiResId;
        }
        
        
        public long uiResId { get { return _m_uiResId; } }
        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_uiResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_uiResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        

        protected override void _onShowWnd()
        {
            wnd?.allFinishAni?.resetAni();
            refreshWnd();
        }
        protected override void _onHideWnd()
        {
            foreach (GGUISubWndStageGoalTaskItem taskItem in _m_taskItemList)
                taskItem.hideWnd();
        }
        protected override void _onReset()
        {
            foreach (GGUISubWndStageGoalTaskItem taskItem in _m_taskItemList)
                taskItem.resetWnd();
        }
        protected override void _onDiscard()
        {
            foreach (GGUISubWndStageGoalTaskItem taskItem in _m_taskItemList)
                taskItem.discard();
            _m_taskItemList.Clear();
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;
            
            if (wnd.taskItemList != null)
            {
                foreach (GGUIMonoStageGoalTaskItem taskItemMono in wnd.taskItemList)
                {
                    if (taskItemMono == null)
                        continue;
                    
                    GGUISubWndStageGoalTaskItem taskItem = new GGUISubWndStageGoalTaskItem(taskItemMono, hideHandGuide);
                    _m_taskItemList.Add(taskItem);
                }
            }
        }


        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;
            
            //区分显示
            _m_taskList.Clear();
            NPPlayer.instance.stageGoalComp.getSubStageGoalTaskList(_m_taskList);
            for (int i = 0; i < _m_taskItemList.Count; i++)
            {
                if (i < _m_taskList.Count)
                {
                    _m_taskItemList[i].showWnd();
                    _m_taskItemList[i].setItem(_m_taskList[i]);
                }
                else
                {
                    _m_taskItemList[i].hideWnd();
                }
            }
        }
        public bool showGuideHand()
        {
            foreach (GGUISubWndStageGoalTaskItem item in _m_taskItemList)
            {
                if (item.taskItem != null && !item.taskItem.isCompleted() && item.taskItem.isUnlock)
                {
                    item.showHandGuide();
                    return true;
                }
            }
            return false;
        }
        public void hideHandGuide()
        {
            foreach (GGUISubWndStageGoalTaskItem item in _m_taskItemList)
            {
                item.hideHandGuide();       
            }
        }

        /// <summary>
        /// 播放所有完成动画
        /// </summary>
        /// <param name="_onPlayDone"></param>
        public void playAllFinishAni(Action _onPlayDone)
        {
            if (wnd == null || wnd.allFinishAni == null)
            {
                _onPlayDone?.Invoke();
                return;
            }

            wnd.allFinishAni.forcePlay(_onPlayDone);
        }
    }
}