using System;
using System.Collections.Generic;
using ALPackage;
using DG.Tweening;
using UnityEngine;

namespace GOE
{
    public enum EConsortRandomInviteShowProcessStage
    {
        NONE,
        PREPARE,//准备阶段
        CONSORT_HEAD_MOVE, //妃子头像移动
        SHOW_DIALOG, //展示对话
        SHOW_DONE //展示完成
    }
    
    public class GGUIWndConsortRandomInviteProcess : _ATALBasicUIWnd<GGUIMonoConsortRandomInviteProcess>
    {
        private static GGUIWndConsortRandomInviteProcess _g_instance;
        public static GGUIWndConsortRandomInviteProcess instance { get { return _g_instance ??= new GGUIWndConsortRandomInviteProcess(); } }

        private Common.ConsortObj.Consort_CallRes _m_iCallRes;//邀约结果
        private long _m_lDialogId;//显示对话id
        private _IConsortShowInfo _m_iConsortShowInfo;
        private ConsortStoryRefObj _m_iConsortStoryRefObj;//妃子故事配表数据
        private Func<Vector3> _m_fGetConsortHeadStartPosFunc;
        private NPGGoIndex _m_iBgGoIndex;
        private Action _m_aOnShowDone;//展示完成回调
        
        private NPGGuiWndTexture _m_wConsortHead;//妃子头像
        private NPGGuiWndTexture _m_wConsortCard;//妃子卡牌
        private GGUIWndConsortCardBg _m_wConsortCardBg;//妃子卡牌背景
        private NPGGUIWndCommonShowCase _m_wBgShowCase;//展示背景showcase
        private NPGGUIWndSubDialogue _m_wDialogWnd;//对话窗口
        
        private EConsortRandomInviteShowProcessStage _m_eShowStage = EConsortRandomInviteShowProcessStage.NONE;
        private long _m_lShowProcessSerialize = 0;
        private Tweener _m_tweener;
        
        public GGUIWndConsortRandomInviteProcess() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoConsortRandomInviteProcess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortRandomInviteProcess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

            if (wnd.consortHeadIcon != null)
                _m_wConsortHead = new NPGGuiWndTexture(wnd.consortHeadIcon);

            if (wnd.consortCardIcon != null)
                _m_wConsortCard = new NPGGuiWndTexture(wnd.consortCardIcon);

            if (wnd.consortCardBg != null)
                _m_wConsortCardBg = new GGUIWndConsortCardBg(wnd.consortCardBg);

            if (wnd.monoBgShowcase != null)
                _m_wBgShowCase = new NPGGUIWndCommonShowCase(wnd.monoBgShowcase);

            if (wnd.monoSubDialogue != null)
                _m_wDialogWnd = new NPGGUIWndSubDialogue(wnd.monoSubDialogue);
        }
        
        protected override void _onDiscard()
        {
            _m_wConsortHead?.discard();
            _m_wConsortHead = null;
            
            _m_wConsortCard?.discard();
            _m_wConsortCard = null;
            
            _m_wConsortCardBg?.discard();
            _m_wConsortCardBg = null;
            
            _m_wBgShowCase?.discard();
            _m_wBgShowCase = null;
            
            _m_wDialogWnd?.discard();
            _m_wDialogWnd = null;
            
            _m_tweener?.Kill();
            
            _m_aOnShowDone = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_lShowProcessSerialize = ALSerializeOpMgr.next();
            _m_eShowStage = EConsortRandomInviteShowProcessStage.NONE;

            _m_wConsortHead?.hideWnd();
            _m_wConsortCard?.hideWnd();
            _m_wConsortCardBg?.hideWnd();
            _m_wBgShowCase?.hideWnd();
            _m_wDialogWnd?.hideWnd();
            
            _m_tweener?.Kill();

            _m_aOnShowDone = null;
        }

        protected override void _onReset()
        {
            _m_wConsortHead?.discardTexture();
            _m_wConsortCard?.discardTexture();
            _m_wConsortCardBg?.resetWnd();
            _m_wBgShowCase?.resetWnd();
            _m_wDialogWnd?.resetWnd();
            
            _m_tweener?.Kill();
            
            _m_aOnShowDone = null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void setInfo(Common.ConsortObj.Consort_CallRes _callRes, long _dialogId, Func<Vector3> _getConsortHeadStartPosFunc, NPGGoIndex _bgGoIndex, Action _onShowDone)
        {
            _m_eShowStage = EConsortRandomInviteShowProcessStage.PREPARE;
            
            _m_iCallRes = _callRes;
            _m_lDialogId = _dialogId;
            _m_iConsortShowInfo = NPPlayer.instance.consortComp.getConsortInfo(_m_iCallRes?.getConsortId() ?? 0);
            _m_iConsortStoryRefObj = _m_iConsortShowInfo?.consortRefObj?.getConsortStory(_m_iCallRes?.getConsortStoryId() ?? 0);
            _m_fGetConsortHeadStartPosFunc = _getConsortHeadStartPosFunc;
            _m_iBgGoIndex = _bgGoIndex;
            _m_aOnShowDone = _onShowDone;

            if (_m_iBgGoIndex == null)
            {
                List<ConsortStoryBgRefObj> storyBgRefList = GRefdataCoreMgr.instance.getConsortStoryBgRefObjList(_m_iConsortStoryRefObj?.id ?? 0);
                List<NPGGoIndex> bgGoIndexList = new List<NPGGoIndex>();
                if (storyBgRefList != null)
                {
                    foreach (var storyBgRefObj in storyBgRefList)
                    {
                        if(storyBgRefObj != null && storyBgRefObj.bg_go_index_list != null)
                            bgGoIndexList.AddRange(storyBgRefObj.bg_go_index_list);
                    }    
                }
                _m_iBgGoIndex = bgGoIndexList.GetRandomItem();
            }
            
            _refreshWnd();
            _startShowProcess();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;

            int bgShowIndex = wnd.bgShowcaseIndex < 0 ? 0 : wnd.bgShowcaseIndex;
            _AShowCaseUnitInfoObj[] unitInfoObjs = new _AShowCaseUnitInfoObj[bgShowIndex + 1];

            ShowCaseCommonResUnitInfoObj bgUnitInfoObj = null;
            if (_m_iBgGoIndex != null)
            {
                bgUnitInfoObj = new ShowCaseCommonResUnitInfoObj(_m_iBgGoIndex);
                unitInfoObjs[bgShowIndex] = bgUnitInfoObj;    
            }
            
            _m_wBgShowCase?.showWnd(unitInfoObjs);
            _m_wBgShowCase?.regInitDoneDelegate(() =>
            {
                if (null != bgUnitInfoObj && bgUnitInfoObj.unit != null && null != wnd.defaultMaterial)
                {
                    SpriteRenderer spriteRenderer = bgUnitInfoObj.unit.GetComponentInChildren<SpriteRenderer>();
                    if (null != spriteRenderer)
                    {
                        spriteRenderer.material = Material.Instantiate(wnd.defaultMaterial);
                    }
                    bgUnitInfoObj.playAnim(wnd.defaultAniName);
                }
            });
        }
        
        /// <summary>
        /// 开始展示过程
        /// </summary>
        private void _startShowProcess()
        {
            long serialize = _m_lShowProcessSerialize = ALSerializeOpMgr.next();
            ALProcess process = ALProcess.CreateProcess();

            process
                .addDelegateProcess((_onComplete) =>
                {
                    _m_eShowStage = EConsortRandomInviteShowProcessStage.CONSORT_HEAD_MOVE;
                    _showConsortHeadAndMove(serialize, _onComplete);
                })
                .addDelegateProcess((_onComplete) =>
                {
                    _onConsortMoveComplete(serialize, _onComplete);
                })
                .addDelegateProcess((_onComplete) =>
                {
                    _showDialog(serialize, _onComplete);
                })
                .addProcess(() =>
                {
                    _processShowDone(serialize);
                });
            
            process.deal();
        }
        
        /// <summary>
        /// 展示妃子头像并移动
        /// </summary>
        /// <param name="_onShowDone"></param>
        private void _showConsortHeadAndMove(long _serialize, Action _onShowDone)
        {
            if (wnd == null || _m_iConsortShowInfo == null || _m_lShowProcessSerialize != _serialize)
            {
                _onShowDone?.Invoke();
                return;
            }
            
            ALUGUICommon.setLabelTxt(wnd.txtConsortName, _m_iConsortShowInfo.consortTransName);
            if(_m_wConsortHead != null)
            {
                _m_wConsortHead.showWnd();
                _m_wConsortHead.setTexture(_m_iConsortShowInfo.consortSkinShowInfo?.consortHeadIcon);
            }

            if (_m_wConsortCard != null)
            {
                _m_wConsortCard.showWnd();
                _m_wConsortCard.setTexture(_m_iConsortShowInfo.consortSkinShowInfo?.consortCardImage);
            }

            if (_m_wConsortCardBg != null)
            {
                _m_wConsortCardBg.showWnd();
                _m_wConsortCardBg.setQuality(_m_iConsortShowInfo.consortQuality);
            }

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                if(_m_lShowProcessSerialize != _serialize)
                    return;
                
                _onShowDone?.Invoke();
            });
            
            // 妃子头像移动
            if (wnd.consortHeadTrans != null && wnd.headMoveTargetTrans != null)
            {
                wnd.consortHeadTrans.position = _m_fGetConsortHeadStartPosFunc?.Invoke() ?? Vector3.zero;
                Vector3 targetPos = wnd.headMoveTargetTrans.position;
                float moveTime = _m_fGetConsortHeadStartPosFunc == null ? -1 : wnd.consortHeadMoveTime;//若没有传入获取初始坐标位置的方法, 直接将移动时间设置为-1, 代表不移动, 直接设置目标位置

                if (moveTime > 0)
                {
                    DOTween.To(() =>
                    {
                        if (wnd == null || wnd.consortHeadTrans == null)
                            return Vector3.zero;

                        return wnd.consortHeadTrans.position;
                    }, (Vector3 _pos) =>
                    {
                        if (wnd == null || wnd.consortHeadTrans == null)
                            return;

                        wnd.consortHeadTrans.position = _pos;
                    }, targetPos, moveTime)
                    .OnComplete(() =>
                    {
                        stepCounter.addDoneStepCount();
                    });
                }
                else
                {
                    wnd.consortHeadTrans.position = targetPos;
                    stepCounter.addDoneStepCount();
                }
            }
            else
            {
                stepCounter.addDoneStepCount();
            }

            if (wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.onConsortHeadMoveAniName))
            {
                wnd.wndAnimation.Play(wnd.onConsortHeadMoveAniName, () =>
                {
                    stepCounter.addDoneStepCount();
                });
            }
            else
            {
                stepCounter.addDoneStepCount();
            }
        }

        /// <summary>
        /// 当妃子头像移动完成后显示动画
        /// </summary>
        private void _onConsortMoveComplete(long _serialize, Action _onShowDone)
        {
            if (wnd == null || _serialize != _m_lShowProcessSerialize)
            {
                _onShowDone?.Invoke();
                return;
            }

            if (wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.onConsortHeadMoveDoneAniName))
            {
                wnd.wndAnimation.Play(wnd.onConsortHeadMoveDoneAniName, () =>
                {
                    if (_m_lShowProcessSerialize != _serialize)
                        return;

                    _onShowDone?.Invoke();
                });
            }
            else
            {
                _onShowDone?.Invoke();
            }
        }

        /// <summary>
        /// 显示对话
        /// </summary>
        private void _showDialog(long _serialize, Action _onShowDone)
        {
            if (_serialize != _m_lShowProcessSerialize)
                return;

            if (wnd == null || _m_iConsortShowInfo == null || _m_iConsortShowInfo.consortRefObj == null || _m_iCallRes == null)
            {
                _onShowDone?.Invoke();
                return;
            }

            _m_eShowStage = EConsortRandomInviteShowProcessStage.SHOW_DIALOG;
            
            long showDialogId = _m_lDialogId;
            if (showDialogId <= 0)
            {
                showDialogId = _m_iConsortStoryRefObj?.dialogue_id ?? 0;
            }
            if (_m_wDialogWnd != null)
            {
                // 没有获取CG时不显示CG
                ConsortUtil.consortCallDialogNotShowCG = _m_iCallRes != null && !_m_iCallRes.getIsGainCg();
                
                _m_wDialogWnd.showWnd();
                _m_wDialogWnd.setInfo(showDialogId, () =>
                {
                    // 对话显示完成后 对话重置为需要显示CG
                    ConsortUtil.consortCallDialogNotShowCG = false;
                    
                    if (_m_lShowProcessSerialize != _serialize)
                        return;
                    
                    _onShowDone?.Invoke();
                });
            }
            else
            {
                _onShowDone?.Invoke();
            }

            if (wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.dialogShowAniName))
            {
                wnd.wndAnimation.ForcePlay(wnd.dialogShowAniName);
            }
        }

        private void _processShowDone(long _serialize)
        {
            if (_serialize != _m_lShowProcessSerialize)
                return;
            
            _m_eShowStage = EConsortRandomInviteShowProcessStage.SHOW_DONE;
            
            _m_aOnShowDone?.Invoke();
        }
        
        public void doEsc()
        {
            if(wnd == null || !isShow)
                return;
            
            long serialize = _m_lShowProcessSerialize = ALSerializeOpMgr.next();

            if (_m_eShowStage is EConsortRandomInviteShowProcessStage.NONE or EConsortRandomInviteShowProcessStage.SHOW_DONE)
                return;

            if (_m_eShowStage is EConsortRandomInviteShowProcessStage.PREPARE or EConsortRandomInviteShowProcessStage.CONSORT_HEAD_MOVE)
            {
                _showDialog(serialize, () =>
                {
                    _processShowDone(serialize);
                });
                return;
            }

            _processShowDone(serialize);
        }
    }
}