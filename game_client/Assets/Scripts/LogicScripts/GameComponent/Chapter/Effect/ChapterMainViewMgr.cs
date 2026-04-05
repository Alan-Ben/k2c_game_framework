using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 关卡表现管理器
    /// </summary>
    public class ChapterMainViewMgr : _IChapterEffectInterface
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

            NPPlayer.instance.chapterComp.forwardLogicMgr.regEffectInterface(this);
            _complete?.Invoke();
        }
        
        public void discard()
        {
            if (!_m_isInit)
                return;
            _m_isInit = false;
            
            NPPlayer.instance.chapterComp.forwardLogicMgr.unregEffectInterface(this);
        }

        //切换到idle表现
        public void chgIdleEffect()
        {
            //刷新界面
            GGUIWndChapterMain.instance.refreshWnd();
        }

        //切换到前进表现
        public void forwardToChapter(float _fade, long _sfxId, long _tdSfxId, int _coefficient, long _rewardExp, long _rewardPlayerExp, long _eventId, List<NPCommon.NPCommon_ItemInfo> _itemList)
        {
            //播放前进表现
            GGUIWndChapterMain.instance.playForwardEffect(false, _sfxId, _tdSfxId, _fade, _coefficient, _rewardExp, _rewardPlayerExp, _eventId, _itemList);
        }

        //切换到快速前进表现
        public void quickForwardToChapter(float _fade, long _sfxId, long _tdSfxId, int _coefficient, long _rewardExp, long _rewardPlayerExp, long _eventId, List<NPCommon.NPCommon_ItemInfo> _itemList)
        {
            //播放前进表现
            GGUIWndChapterMain.instance.playForwardEffect(true, _sfxId, _tdSfxId, _fade, _coefficient, _rewardExp, _rewardPlayerExp, _eventId, _itemList);
        }

        /// <summary>
        /// 播放前进视频状态动画
        /// </summary>
        /// <param name="_forwardState"></param>
        public void playForwardVideoAniState(EChapterVideoForwardState _forwardState)
        {
            GGUIWndChapterMain.instance.playForwardVideoAniState(_forwardState);
        }
        
        //切换到boss战前摇表现
        public void forwardToBossPer(Action _doneAction)
        {
            //播放boss战前摇特效动画
            GGUIWndChapterMain.instance.playBossFightPerAnim(_doneAction);
        }

        //切换到boss战前连线表现
        public void forwardToBossWait()
        {
            //刷新界面
            GGUIWndChapterMain.instance.refreshWnd();
        }

        /// <summary>
        /// boss战表现
        /// </summary>
        /// <param name="_isAuto">是否自动</param>
        /// <param name="_doneAction"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void forwardToBoss(ChapterRefObj _chapterRef, Action _doneAction)
        {
            //进入boss战node
            QueueMgr.instance.AddNode(new GNodeChapterBoss(_chapterRef, _doneAction));
        }

        public void showDialogSliderTip(long _pointId, Action _doneAction)
        {
            //展示弹窗
            GGUIWndChapterMain.instance.showDialogSliderTip(_pointId, _doneAction);
        }
        
        public void showDialogTipEffect()
        {
            //展示弹窗
            GGUIWndChapterMain.instance.showDialogTipEffect();
        }
        
        public void showEventTipEffect()
        {
            //展示弹窗
            GGUIWndChapterMain.instance.showEventTipEffect();
        }

        //处理节点移动表现
        public void _dealChapterNodeMoveEffect()
        {
            GGUIWndChapterMain.instance.dealChapterNodeMoveEffect();
        }
        
        //处理Boss节点移动表现
        public void _dealChapterNodeMoveBossEffect()
        {
            GGUIWndChapterMain.instance.dealChapterNodeMoveBossEffect();
        }
    }
}