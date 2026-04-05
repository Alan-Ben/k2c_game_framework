using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 关卡 节-stage 窗口中 章item
    /// </summary>
    public class GGUIWndChapterMapNodeItem : _ANPGGUIBasicSubWnd<GGUIMonoChapterMapNodeItem>
    {
        private long _m_lItemChgStateShowSerialId = 0; // item变化状态表现序列化id

        private long _m_lChapterId; // 章ID
        private int _m_nodeIndex; //第几个node
        private ChapterRefObj _m_rChapterRefObj; // 章数据配表对象
        private _IChapterNodeStyle _m_rNodeStyleRefObj;// 章节点风格数据配表对象
        private NPGGuiWndTexture _m_chapterImg; // 章切图
        private NPGGuiWndTexture _m_chapterImg2; // 章切图2
        private EChapterMapNodeState _m_eCurChapterState = EChapterMapNodeState.UN_REACH; // 当前章状态
        //特效列表
        [NotNull]private List<CommonUISfxObj> _m_lSfxObjList = new List<CommonUISfxObj>();
        
        public GGUIWndChapterMapNodeItem(GGUIMonoChapterMapNodeItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        public long chapterId { get { return _m_lChapterId; } }
        public ChapterRefObj chapterRefObj { get { return _m_rChapterRefObj; } }
        public int nodeIndex { get { return _m_nodeIndex; } }

        public EChapterMapNodeState eCurChapterState
        {
            get { return _m_eCurChapterState; }
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.nodeImg != null)
                _m_chapterImg = new NPGGuiWndTexture(wnd.nodeImg);
            
            if (wnd.nodeImg2 != null)
                _m_chapterImg2 = new NPGGuiWndTexture(wnd.nodeImg2); 
            
            ALUGUICommon.combineBtnClick(wnd.btnGoto, _onGotoBtnClick);
        }
        
        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnGoto, _onGotoBtnClick);
            }
            
            for (int i = 0; i < _m_lSfxObjList.Count; i++)
            {
                _m_lSfxObjList[i]?.forceDiscard();
            }
            _m_lSfxObjList.Clear();
            
            _m_chapterImg?.discard();
            _m_chapterImg = null;
            
            _m_chapterImg2?.discard();
            _m_chapterImg2 = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lItemChgStateShowSerialId = ALSerializeOpMgr.next();

            _m_chapterImg?.hideWnd();
            _m_chapterImg2?.hideWnd();
            
            for (int i = 0; i < _m_lSfxObjList.Count; i++)
            {
                _m_lSfxObjList[i]?.forceDiscard();
            }
            _m_lSfxObjList.Clear();
        }

        protected override void _onReset()
        {
            for (int i = 0; i < _m_lSfxObjList.Count; i++)
            {
                _m_lSfxObjList[i]?.forceDiscard();
            }
            _m_lSfxObjList.Clear();
            
            _m_chapterImg?.discardTexture();
            _m_chapterImg2?.discardTexture();
        }

        /// <summary>
        /// 设置数据
        /// </summary>
        public void setChapterInfo(ChapterRefObj _chapterRefObj, int _nodeIndex)
        {
            if(null == _chapterRefObj)
                return;
            
            _m_rChapterRefObj = _chapterRefObj;
            NPGGoIndex goIndex = null;
            _m_rNodeStyleRefObj = _chapterRefObj.getChapterNodeStyleByIndexId(_nodeIndex, out goIndex);
            _m_lChapterId = _m_rChapterRefObj?.chapter_id ?? 0;
            _m_nodeIndex = _nodeIndex;
            _m_lItemChgStateShowSerialId = ALSerializeOpMgr.next();

            refresh();
        }

        public void refresh()
        {
            //未解锁为未达成状态
            if (!GCommon.isSimpleUnlock(_m_rChapterRefObj.forward_simple_unlock_id))
            {
                _m_eCurChapterState = EChapterMapNodeState.UN_REACH;
            }
            else if (_m_nodeIndex < NPPlayer.instance.chapterComp.curNodeIndex)
            {
                _m_eCurChapterState = EChapterMapNodeState.COMPLETED;
            }
            else if (_m_nodeIndex == NPPlayer.instance.chapterComp.curNodeIndex)
            {
                _m_eCurChapterState = EChapterMapNodeState.UNDERWAY;
            }
            else
            {
                _m_eCurChapterState = EChapterMapNodeState.UN_REACH;
            }
            
            _refresh();
        }
        
        private void _refresh()
        {
            if(wnd == null || _m_rChapterRefObj == null)
                return;
            
            ALUGUICommon.setLabelTxt(wnd.txtChapterTag, $"{_m_lChapterId} - {_m_nodeIndex + 1}");

            if (_m_chapterImg != null)
            {
                _m_chapterImg.showWnd();
                if(null != _m_rNodeStyleRefObj)
                    _m_chapterImg.setTexture(_m_rNodeStyleRefObj.nodeMiniImg);
            }
            
            if (_m_chapterImg2 != null)
            {
                _m_chapterImg2.showWnd();
                if(null != _m_rNodeStyleRefObj)
                    _m_chapterImg2.setTexture(_m_rNodeStyleRefObj.nodeMiniImg);
            }

            
            if (wnd.sldMask != null)
            {
                if (_m_nodeIndex < NPPlayer.instance.chapterComp.curNodeIndex)
                {
                    wnd.sldMask.value = 0;
                } 
                else if (_m_nodeIndex > NPPlayer.instance.chapterComp.curNodeIndex)
                {
                    wnd.sldMask.value = 1;
                }
                else // 当前节点    
                {
                    wnd.sldMask.value = 1 - NPPlayer.instance.chapterComp.getCurNodeFade();
                }
            }
            
            if (wnd.maskImg != null)
            {
                if (_m_nodeIndex < NPPlayer.instance.chapterComp.curNodeIndex)
                {
                    wnd.maskImg.fillAmount = 0;
                } 
                else if (_m_nodeIndex > NPPlayer.instance.chapterComp.curNodeIndex)
                {
                    wnd.maskImg.fillAmount = 1;
                }
                else // 当前节点    
                {
                    wnd.maskImg.fillAmount = 1 - NPPlayer.instance.chapterComp.getCurNodeFade();
                }
            }
            
            wnd.refreshState(_m_eCurChapterState);
            _playAnimation(wnd.defaultStateAniName);
        }

        /// <summary>
        /// item切换为完成状态表现
        /// </summary>
        /// <param name="_playDone"></param>
        public void showItemChgToCompletedState(Action _playDone = null)
        {
            if (wnd == null)
            {
                _playDone?.Invoke();
                return;
            }
            
            //未解锁不处理
            if (!GCommon.isSimpleUnlock(_m_rChapterRefObj.forward_simple_unlock_id))
            {
                _playDone?.Invoke();
                return;
            }
            
            long serialId = _m_lItemChgStateShowSerialId = ALSerializeOpMgr.next();
            wnd.refreshState(EChapterMapNodeState.UNDERWAY);//在切换为完成状态前表现为进行中状态
            // 播放转化为完成状态的动画
            _playAnimation(wnd.chgToCompletedStateAniName, () =>
            {
                if(serialId != _m_lItemChgStateShowSerialId)
                    return;
                
                // 动画播放完成后，切换为完成状态
                wnd.refreshState(EChapterMapNodeState.COMPLETED);
                _playDone?.Invoke();
            });
        }
        
        /// <summary>
        /// item切换为进行中状态表现
        /// </summary>
        /// <param name="_playDone"></param>
        public void showItemChgToUnderwayState(Action _playDone = null)
        {
            if (wnd == null)
            {
                _playDone?.Invoke();
                return;
            }

            //未解锁不处理
            if (!GCommon.isSimpleUnlock(_m_rChapterRefObj.forward_simple_unlock_id))
            {
                _playDone?.Invoke();
                return;
            }
            
            
            long serialId = _m_lItemChgStateShowSerialId = ALSerializeOpMgr.next();
            wnd.refreshState(EChapterMapNodeState.UN_REACH);//在切换为进行中状态前表现为未达到状态
            // 播放转化为进行中状态的动画
            _playAnimation(wnd.chgToUnderwayStateAniName, () =>
            {
                if(serialId != _m_lItemChgStateShowSerialId)
                    return;
                
                // 动画播放完成后，切换为进行中状态
                wnd.refreshState(EChapterMapNodeState.UNDERWAY);
                _playDone?.Invoke();
            });
        }
        
        public void showBossAutoBattleState(Action _playDone = null)
        {
            if (wnd == null)
            {
                _playDone?.Invoke();
                return;
            }

            //未解锁不处理
            if (!GCommon.isSimpleUnlock(_m_rChapterRefObj.forward_simple_unlock_id))
            {
                _playDone?.Invoke();
                return;
            }
            
            
            long serialId = _m_lItemChgStateShowSerialId = ALSerializeOpMgr.next();
            // 播放转化为进行中状态的动画
            _playAnimation(wnd.bossAniName, () =>
            {
                if(serialId != _m_lItemChgStateShowSerialId)
                    return;

                _playDone?.Invoke();
            });
        }
        
        /// <summary>
        /// 强制刷新item显示状态
        /// </summary>
        /// <param name="_state"></param>
        public void forceRefreshItemShowState(EChapterMapNodeState _state)
        {
            if (wnd != null) 
                wnd.refreshState(_state);
        }
        
        public void playAutoForwardSfx()
        {
            if (wnd == null || wnd.sfxParent == null)
                return;

            if(wnd.sfxId == 0 || wnd.sfxParent == null)
                return;
            
            CommonUISfxObj commonUISfxObj = PlaySfxMgr.instance.playUISfx(wnd.sfxId, wnd.sfxParent);
            _m_lSfxObjList.Add(commonUISfxObj);        
        }
        
        /// <summary>
        /// 播放动画
        /// </summary>
        private void _playAnimation(string _aniName, Action _playDone = null)
        {
            if (wnd == null || wnd.ani == null || string.IsNullOrEmpty(_aniName))
            {
                _playDone?.Invoke();
                return;
            }

            wnd.ani.Play(_aniName, _playDone);
        }
        
        private void _sampleAnimation(string _aniName, float _normalizedTime)
        {
            if (wnd == null || wnd.ani == null || string.IsNullOrEmpty(_aniName))
            {
                return;
            }

            wnd.ani.Sample(_aniName, _normalizedTime);
        }
        
        /// <summary>
        /// 前往按钮被点击
        /// </summary>
        /// <param name="_go"></param>
        private void _onGotoBtnClick(GameObject _go)
        {
            if(_m_eCurChapterState == EChapterMapNodeState.COMPLETED)
                return;
            
            if(_m_eCurChapterState == EChapterMapNodeState.UN_REACH)
            {
                NPGUIAddSceneCenterTip.instance.showTransTextInfo($"#1_chapter_map_need_complete_back");
                return;
            }
            
            if (QueueMgr.instance.findLastNode(typeof(GNodeChapterForward)) is GNodeChapterForward _chapterNode)
            {
                QueueMgr.instance.QuitUntilCanStop((_node) => _node == _chapterNode);
            }
            else
            {
                QueueMgr.instance.AddNode(new GNodeChapterForward());
            }
        }
    }
}