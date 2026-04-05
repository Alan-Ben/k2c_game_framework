using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIWndConsortAppointCallProcess : _ATALBasicUIWnd<GGUIMonoConsortAppointCallProcess>
    {
        private static GGUIWndConsortAppointCallProcess _g_instance;
        public static GGUIWndConsortAppointCallProcess instance { get { return _g_instance ??= new GGUIWndConsortAppointCallProcess(); } }
        
        private GS2GC.p015_ConsortOp.GS2GC_015_006_RetCallAppoint _m_appointInfoRetInfo;
        private GGottenConsortInfo _m_rGottenConsortInfo;//出游妃子信息
        private ConsortTravelRefObj _m_rTravelRefObj;//出游配表数据
        private Action _m_aOnShowDone;//展示完成回调

        private NPGGUIWndCommonShowCase _m_wBgShowCase;//展示背景showcase
        private NPGGUIWndSubDialogue _m_wDialogWnd;//对话窗口
        
        private long _m_lShowSerialize = 0;

        public GGUIWndConsortAppointCallProcess() : base(EALUIWndLayer.ADDITION)
        {
        }
        
        protected override string _monoAssetPath { get { return GGUIMonoConsortAppointCallProcess.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoConsortAppointCallProcess.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoBgShowcase != null)
            {
                _m_wBgShowCase = new NPGGUIWndCommonShowCase(wnd.monoBgShowcase);
            }

            if (wnd.monoSubDialogue != null)
            {
                _m_wDialogWnd = new NPGGUIWndSubDialogue(wnd.monoSubDialogue);
            }
        }
        
        protected override void _onDiscard()
        {
            _m_wBgShowCase?.discard();
            _m_wBgShowCase = null;
            
            _m_wDialogWnd?.discard();
            _m_wDialogWnd = null;
        }
        
        protected override void _onShowWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            _m_lShowSerialize = ALSerializeOpMgr.next();

            _m_wBgShowCase?.hideWnd();
            _m_wDialogWnd?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wBgShowCase?.resetWnd();
            _m_wDialogWnd?.resetWnd();
        }

        public void setData(GS2GC.p015_ConsortOp.GS2GC_015_006_RetCallAppoint _callAppointRetInfo, GGottenConsortInfo _consortInfo, ConsortTravelRefObj _travelRefObj, Action _onShowDone = null)
        {
            _m_appointInfoRetInfo = _callAppointRetInfo;
            _m_rGottenConsortInfo = _consortInfo;
            _m_rTravelRefObj = _travelRefObj;
            _m_aOnShowDone = _onShowDone;

            _refreshWnd();
            _startShowProcess();
        }

        private void _refreshWnd()
        {
            if (null == wnd)
                return;

            int bgShowIndex = wnd.bgShowcaseIndex < 0 ? 0 : wnd.bgShowcaseIndex;
            _AShowCaseUnitInfoObj[] unitInfoObjs = new _AShowCaseUnitInfoObj[bgShowIndex + 1];
            ShowCaseCommonResUnitInfoObj resUnitInfoObj = new ShowCaseCommonResUnitInfoObj(_m_rTravelRefObj?.bg_go);

            unitInfoObjs[bgShowIndex] = resUnitInfoObj;
            
            _m_wBgShowCase?.showWnd(unitInfoObjs);
            _m_wBgShowCase?.regInitDoneDelegate(() =>
            {
                if (null != resUnitInfoObj && null != wnd.defaultMaterial)
                {
                    SpriteRenderer spriteRenderer = resUnitInfoObj.unit.GetComponentInChildren<SpriteRenderer>();
                    if (null != spriteRenderer)
                    {
                        spriteRenderer.material = Material.Instantiate(wnd.defaultMaterial);
                    }
                    resUnitInfoObj.playAnim(wnd.defaultAniName);
                }
            });
        }

        /// <summary>
        /// 开始表现流程
        /// </summary>
        private void _startShowProcess()
        {
            long serialize = _m_lShowSerialize;
            ALProcess process = ALProcess.CreateProcess();
            
            process
                .addDelegateProcess((_onComplete) =>
                {
                    _showBg(serialize, _onComplete);
                })
                .addDelegateProcess((_onComplete) =>
                {
                    _showDialog(serialize, _onComplete);
                })
                .addProcess(() =>
                {
                    _m_aOnShowDone?.Invoke();
                });
            
            process.deal();
        }

        /// <summary>
        /// 展示背景过程
        /// </summary>
        /// <param name="_serialize"></param>
        /// <param name="_onShowDone"></param>
        private void _showBg(long _serialize, Action _onShowDone)
        {
            if (wnd == null || _m_lShowSerialize != _serialize)
            {
                _onShowDone?.Invoke();
                return;
            }
            
            if (wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.showBgAniName))
            {
                wnd.wndAnimation.Play(wnd.showBgAniName, () =>
                {
                    if (_m_lShowSerialize != _serialize)
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
        /// 展示对话
        /// </summary>
        /// <param name="_serialize"></param>
        /// <param name="_onShowDone"></param>
        private void _showDialog(long _serialize, Action _onShowDone)
        {
            if (wnd == null || _serialize != _m_lShowSerialize || _m_rGottenConsortInfo == null || _m_rGottenConsortInfo.consortRefObj == null || _m_rTravelRefObj == null)
            {
                _onShowDone?.Invoke();
                return;
            }

            if (_m_wDialogWnd != null)
            {
                _m_wDialogWnd.showWnd();
                _m_wDialogWnd.setInfo(_m_rGottenConsortInfo.consortRefObj.getTravelDialogId(_m_rTravelRefObj.id), () =>
                {
                    if (_m_lShowSerialize != _serialize)
                        return;
                    
                    _onShowDone?.Invoke();
                });
            }
            else
            {
                _onShowDone?.Invoke();
            }

            if (wnd.wndAnimation != null && !string.IsNullOrEmpty(wnd.showDialogAniName))
            {
                wnd.wndAnimation.ForcePlay(wnd.showDialogAniName);
            }
        }
    }
}