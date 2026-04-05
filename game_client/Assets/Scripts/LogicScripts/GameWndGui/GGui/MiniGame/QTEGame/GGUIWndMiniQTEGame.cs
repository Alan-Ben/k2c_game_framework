using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndMiniQTEGame : _ANPGGUIBasicWnd<GGUIMonoMiniQTEGame>
    {
        private static ALResObjSingleContainer _m_alResObjList = new ALResObjSingleContainer();

        [NotNull] private QTEGameRefObj _m_rQteGameRefObj;
        private Action _m_aQteDone;
        
        protected int _m_iCurStepIndex = -1;
        //当前教程阶段对象
        protected MiniQTEGameStepObj _m_oCurStepObj;

        private bool _m_bGameEnd;//是否结束
        private long _m_lStepTaskSerialId;
        
        [NotNull] private NextStepTriggerListMgr _m_nextStepTriggerListMgr = new NextStepTriggerListMgr();
        
        public GGUIWndMiniQTEGame([NotNull] QTEGameRefObj _qteGameRefObj, Action _qteDone) : base(EALUIWndLayer.TOP)
        {
            _m_rQteGameRefObj = _qteGameRefObj;
            _m_aQteDone = _qteDone;
        }
        
        protected override string _monoAssetPath { get { return _m_rQteGameRefObj.wnd_path?.asset_path; } }
        protected override string _monoObjName { get { return _m_rQteGameRefObj.wnd_path?.obj_name; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        
        public bool gameEnd{ get { return _m_bGameEnd; } }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            if(null != _m_alResObjList)
                _m_alResObjList.discard();
            
            _m_nextStepTriggerListMgr.reset();
        }
        
        protected override void _onShowWnd()
        {
            _m_bGameEnd = false;
            _nowStepCompleteAndGoNextStep();
        }

        protected override void _onHideWnd()
        {
            _m_nextStepTriggerListMgr.reset();
            
            _m_lStepTaskSerialId = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_nextStepTriggerListMgr.reset();
        }

        /// <summary>
        /// 当前步骤结束, 并且进入下一步骤
        /// </summary>
        private void _nowStepCompleteAndGoNextStep()
        {
            Action onNowStepAllDone = () =>
            {
                // 退出该步骤显隐物体设置
                if (_m_oCurStepObj != null)
                {
                    ALUGUICommon.setGameObjEnable(_m_oCurStepObj.exitEnableGo, true);
                    ALUGUICommon.setGameObjEnable(_m_oCurStepObj.exitDisableGo, false);
                }
                //上一步的音源释放
                if(_m_alResObjList != null)
                    _m_alResObjList.discard();
                
                if (_checkEndOfTutorial())//若引导结束返回
                    return;

                _enterNewStep();
            };
            
            if (_m_oCurStepObj != null)
            {
                // 执行本步骤完成后效果
                NPPlayerEffectSerializeInfo.dealEffect(NPPlayerEffectSerializeInfo.readEffectList(_m_oCurStepObj.stepDoneEffect), null);
                long serializeId = _m_lStepTaskSerialId = ALSerializeOpMgr.next();
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (serializeId != _m_lStepTaskSerialId)
                        return;

                    onNowStepAllDone();
                }, _m_oCurStepObj.toNextStepDelayTime);    
            }
            else
            {
                onNowStepAllDone();
            }
        }
        
        private void _enterNewStep()
        {
            if(wnd == null || wnd.stepObjList == null)
                return;

            _m_iCurStepIndex = ++_m_iCurStepIndex;
            _m_oCurStepObj = wnd.stepObjList[Mathf.Clamp(_m_iCurStepIndex, 0, wnd.stepObjList.Count - 1)];

            //正式进入新阶段
            stepDeal();
        }

        //正式进入新阶段的处理方法
        private void stepDeal()
        {
            if(_m_oCurStepObj == null)
                return;

            ALUGUICommon.setGameObjEnable(_m_oCurStepObj.enableGo, true);
            ALUGUICommon.setGameObjEnable(_m_oCurStepObj.disableGo, false);

            //加载并播放本阶段音源
            if(_m_oCurStepObj.stepVoiceIndex != null)
                LoadQteVoice(_m_oCurStepObj.stepVoiceIndex.mainId, _m_oCurStepObj.stepVoiceIndex.subId, null);
            
            NPPlayerEffectSerializeInfo.dealEffect(NPPlayerEffectSerializeInfo.readEffectList(_m_oCurStepObj.beforeStartStepEffect), null);
            
            _m_nextStepTriggerListMgr.setTriggerList(_m_oCurStepObj.nextStepTriggerList, _nowStepCompleteAndGoNextStep);
        }

        //加载语音
        public void LoadQteVoice(int _mainId, int _subId, Action _loadedCall)
        {
            if(0 == _mainId && 0 == _subId)
                return;

            AGuideVoiceRefCore.instance.loadObj(_mainId, _subId, (resObj) =>
            {
                if(OnQteVoiceLoaded(resObj))
                {
                    if(_loadedCall != null)
                        _loadedCall();
                }
            });
        }

        //创建音源对象
        private bool OnQteVoiceLoaded(_ATALObjResObj<GameObject> _resObj)
        {
            if(null == _resObj)
                return false;

            //long mergeIndex = ALCommon.mergeInt(_resObj.mainId, _resObj.subId);

            //获取资源对象
            _resObj.createObj(_m_alResObjList);

            return true;
        }
        
        protected bool _checkEndOfTutorial()
        {
            if (wnd != null && wnd.stepObjList != null && _m_iCurStepIndex < wnd.stepObjList.Count - 1)
                return false;

            _gameEndDeal();

            return true;
        }

        //本阶段教程结束的事件处理
        private void _gameEndDeal()
        {
            _m_bGameEnd = true;
            if (wnd == null)
            {
                _m_aQteDone?.Invoke();
                return;
            }

            //执行效果
            if(!string.IsNullOrEmpty(wnd.gameEndFunc))
            {
                NPPlayerEffectSerializeInfo.dealEffect(NPPlayerEffectSerializeInfo.readEffectList(wnd.gameEndFunc), null);
            }

            if (wnd.endDelayTime <= 0)//不需要延迟结束, 直接调用结束方法
            {
                _m_aQteDone?.Invoke();
            }
            else//需要延迟结束
            {
                long serializeId = _m_lStepTaskSerialId = ALSerializeOpMgr.next();
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (serializeId != _m_lStepTaskSerialId)
                        return;
                    
                    _m_aQteDone?.Invoke();
                }, wnd.endDelayTime);
            }
        }
    }
}