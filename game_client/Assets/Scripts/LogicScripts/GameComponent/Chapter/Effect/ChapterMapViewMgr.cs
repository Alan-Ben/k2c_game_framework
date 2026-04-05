using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 关卡表现管理器
    /// </summary>
    public class ChapterMapViewMgr : _IChapterMapEffectInterface
    {
        private bool _m_isInit;
        
        public void init(Action _complete)
        {
            if (_m_isInit)
            {
                _complete?.Invoke();
                return;
            }
            _m_isInit = true;

            NPPlayer.instance.chapterComp.forwardLogicMgr.regMapEffectInterface(this);
            _complete?.Invoke();
        }
        
        public void discard()
        {
            if (!_m_isInit)
                return;
            _m_isInit = false;
            
            NPPlayer.instance.chapterComp.forwardLogicMgr.unregMapEffectInterface(this);
        }


        public void playChapterCompleteEffect(ChapterRefObj _chapterRef, int _targetNodeIndex)
        {
            GGUIWndChapterMap.instance.showNodeUnlock(_chapterRef, _targetNodeIndex);
        }

        public void dealAutoForwardToChapter(int _targetNodeIndex, float _fade, int _coefficient, long _rewardExp, long _rewardPlayerExp, List<NPCommon.NPCommon_ItemInfo> _itemList)
        {
            GGUIWndChapterMap.instance.showAutoForwardEffect(_targetNodeIndex, _fade, _coefficient, _rewardExp, _rewardPlayerExp, _itemList);
        }

        public void dealAutoBossToChapter(Action _playDone)
        {
            GGUIWndChapterMap.instance.showAutoBossEffect(_playDone);
        }

        public void enterAutoState()
        {
            GGUIWndChapterMap.instance.refreshAll();
        }

        public void quitAutoState()
        {
            GGUIWndChapterMap.instance.refreshAll();
        }
    }
}