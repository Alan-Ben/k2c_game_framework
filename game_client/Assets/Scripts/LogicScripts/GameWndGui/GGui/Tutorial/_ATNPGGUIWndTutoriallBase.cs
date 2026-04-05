using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace GOE
{
    /*******************
     * 新手引导窗口
     **/


    public abstract class _ATNPGGUIWndTutoriallBase<T> : _ANPGGUIBasicWnd<T> where T : NPGGUIMonoTutorialMainWnd
    {
        private static ALResObjSingleContainer _m_alResObjList = new ALResObjSingleContainer();

        protected int _m_iCurStepIndex = -1;

        protected string _m_sAssetPath;
        protected string _m_sAssetName;
        //点击可响应的时间
        private float _m_fNextClickCanResponseTime;
        /** 监听教程触发的类型 */
        private ENPTutorialTriggerType _m_eListenTutorialTriggerType;
        /** 监听教程触发的类型参数 */
        private string _m_eListenTutorialTriggerTypeArgs;
        
        private Vector2 _m_vDragPos;
        public Vector2 DragPos { get { return _m_vDragPos; } }

        //当前教程阶段对象
        protected NPGGUIMonoTutorialWndStepObj _m_oCurStepObj;
        //点击下一步按钮后执行的操作
        protected List<NPPlayerEffectSerializeInfo> _m_lClickDealFuncInfo;
        //本阶段执行的操作：使用tutorialFunc字符串方式解析
        protected List<NPPlayerEffectSerializeInfo> _m_lStepDealFuncInfo;
        protected List<NPPlayerEffectSerializeInfo> _m_lDragEndFuncInfo;//正确的拖拽结束后的操作
        protected List<NPPlayerEffectSerializeInfo> _m_lDragBeginFuncInfo;//开始拖拽时的操作
        protected List<NPPlayerEffectSerializeInfo> _m_lDragFuncInfo;    //拖拽时的操作
        protected List<NPPlayerEffectSerializeInfo> _m_lDragFailureFuncInfo; //拖拽失败的操作

        public _ATNPGGUIWndTutoriallBase(string _assetPath, string _assetName)
            : base(EALUIWndLayer.TOP)
        {
            _m_sAssetPath = _assetPath;
            if (!_m_sAssetPath.EndsWith(".unity3d"))
                _m_sAssetPath += ".unity3d";
            _m_sAssetName = _assetName;

            _m_eListenTutorialTriggerType = ENPTutorialTriggerType.NONE;
            _m_eListenTutorialTriggerTypeArgs = String.Empty;
        }

        /********************
         * 获取资源所在资源加载文件名称
         **/
        protected override string _monoAssetPath { get { return _m_sAssetPath; } }
        protected override string _monoObjName { get { return _m_sAssetName; } }

        /**************
         * 获取用于加载资源的管理对象
         **/
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        /******************
         * 显示窗口的事件函数
         **/
        protected override void _onShowWnd()
        {
            //每次显示引导窗口时都默认进入下一个阶段
            _stepCrossFade();

            //引导过程不允许使用窗口回滚功能
            QueueMgr.instance.CloseRollBack();

            //开启引导状态
            Game.instance.openIsInTutorial();
            
            //发送埋点-展示引导窗口
            GCommon.sendStepReport(TraceConst.SHOW_TUTORIAL_WND.setMarkParam(_monoObjName));
        }
        /******************
         * 隐藏窗口的事件函数
         **/
        protected override void _onHideWnd()
        {
            //退出引导，打开窗口回滚功能
            QueueMgr.instance.OpenRollBack();

            //开启引导状态
            Game.instance.closeIsInTutorial();
        }
        /******************
         * 重置窗口数据的事件函数
         **/
        protected override void _onReset()
        {
        }
        /******************
         * 释放资源时触发的事件
         **/
        protected override void _onDiscard()
        {
            if(null != _m_alResObjList)
                _m_alResObjList.discard();
            
            _resetData();

            WinMsg.UnregisterMsg(WinMsgType.TUTORIAL_DEAL_NEXT_STEP_BUTTON, _onDealNextStepButtonClick);
        }

        /*************
         * 窗口初始化完成调用的函数
         * */
        protected override void _onWndInitDone()
        {
            if(wnd == null || wnd.stepList == null)
                return;

            RectTransform temp;
            for(int i = 0; i < wnd.stepList.Count; i++)
            {
                temp = wnd.stepList[i].nextStepBtn;

                if(temp != null)
                    ALUGUICommon.combineBtnClick(temp.gameObject, _OnBtnClick);
            }

            ALUGUICommon.combineBtnClick(wnd.skipBtn, _onSkipBtnClick);

            WinMsg.RegisterMsg(WinMsgType.TUTORIAL_DEAL_NEXT_STEP_BUTTON, _onDealNextStepButtonClick);
        }

        protected void _resetData()
        {
            if(wnd == null)
                return;

            //如果当前阶段有遮罩窗口的话要退出
            _discardMovementMask();
            //清空教程监听
            _clearTutorialTrigger();

            //如果当前阶段不为空
            if(_m_oCurStepObj != null)
            {
                //退出当前阶段
                _ExitCurStep();

            }
            _m_oCurStepObj = null;
            _m_iCurStepIndex = -1;
        }

        /******************
         * 在触发教程事件的时候进行的处理
         **/
        protected void _onTutorialTrigger(params object[] _objs)
        {
            if(null == _objs || _objs.Length <= 0)
                return;
            
            //此时设定不允许esc
            QueueMgr.instance.CloseRollBack();
            //Debug.LogError("###   在触发教程事件的时候进行的处理  #####");
            ENPTutorialTriggerType triggerType = (ENPTutorialTriggerType)_objs[0];

            //操作失败
            if(triggerType == ENPTutorialTriggerType.FAIL)
            {
                _m_iCurStepIndex--;
                //进入下一阶段，即重新进入本阶段
                _stepCrossFade();
                return;
            }

            if(triggerType == _m_eListenTutorialTriggerType)
            {
                //如果没有监听参数，枚举一致就下一步
                if (string.IsNullOrEmpty(_m_eListenTutorialTriggerTypeArgs))
                {
                    //（消息触发执行下一步，不考虑点击响应时间），执行下一步，并注销本函数
                    _dealBtnClick(0);
                }
                else
                {
                    //如果有配置监听参数，并且参数值跟发的值一致才触发下一步
                    if (_objs.Length > 1 && null != _objs[1] && _objs[1].ToString() == _m_eListenTutorialTriggerTypeArgs)
                    {
                        //（消息触发执行下一步，不考虑点击响应时间），执行下一步，并注销本函数
                        _dealBtnClick(0);
                    }
                }
            }
        }

        //监听执行到下一步的消息
        protected void _onDealNextStepButtonClick(params object[] _objs)
        {

            if(!isShow)
                return;

            int stepIndex = (int)_objs[0];
            if(stepIndex != _m_iCurStepIndex)
                return;
            //（消息触发执行下一步，不考虑点击响应时间），执行下一步，并注销本函数
            _dealBtnClick(0);
        }

        /***************
         * 清理监听操作
         **/
        protected void _clearTutorialTrigger()
        {
            _m_eListenTutorialTriggerTypeArgs = String.Empty;

            if(_m_eListenTutorialTriggerType == ENPTutorialTriggerType.NONE)
                return;

            //清空监听
            WinMsg.UnregisterMsg(WinMsgType.CONTROL_TUROTIAL_SETP, _onTutorialTrigger);
            _m_eListenTutorialTriggerType = ENPTutorialTriggerType.NONE;
        }

        //按钮点击事件 点击后进入下一步
        private void _OnBtnClick(GameObject _go)
        {
            //这一步有拖next按钮，并且next按钮是当前点击的按钮，才进行下一步
            if (null != _m_oCurStepObj && _m_oCurStepObj.nextStepBtn != null && _go == _m_oCurStepObj.nextStepBtn.gameObject)
            {
                _dealBtnClick(_m_fNextClickCanResponseTime);
            }
        }

        /// <summary>
        /// 按钮点击事件 点击后进入下一步
        /// </summary>
        /// <param name="_nextClickCanResponseTime">点击可响应的时间</param>
        private void _dealBtnClick(float _nextClickCanResponseTime)
        {
            if(wnd == null || wnd.stepList == null || wnd.stepList.Count - 1 < _m_iCurStepIndex || Time.realtimeSinceStartup < _nextClickCanResponseTime)
                return;

            //先判断是否有需要前置处理的东西
            bool dealDone = false;
            if(null != _m_oCurStepObj.clickPreDealFunc && _m_oCurStepObj.clickPreDealFunc.Count > 0)
            {
                NPGGUIMonoTutorialStepDealCondFunc tmpFunc = null;
                for(int i = 0; i < _m_oCurStepObj.clickPreDealFunc.Count; i++)
                {
                    tmpFunc = _m_oCurStepObj.clickPreDealFunc[i];
                    if(null == tmpFunc)
                        continue;

                    NPPlayerConditionGroupObj condition = NPPlayerConditionGroupObj.readConditionGroupList(tmpFunc.tutorialCondition, "stepC");
                    if(NPPlayerConditionGroupObj.IsEnable(condition, null))
                    {
                        //执行效果
                        _dealStepStr(tmpFunc.tutorialDealFunc);
                        //设置完成
                        dealDone = true;

                        //跳出循环
                        break;
                    }
                }
            }

            //如没进行前置处理则处理默认按钮点击事务
            if(!dealDone)
                _dealStepStr(_m_lClickDealFuncInfo);
            //阶段过渡
            _stepCrossFade();
        }

        //如果有跳过按钮的话，跳过此阶段
        private void _onSkipBtnClick(GameObject _go)
        {
            if(Time.realtimeSinceStartup < _m_fNextClickCanResponseTime)
                return;

            _tutorialSkipEndDeal();
        }

        //阶段过渡方法
        protected virtual void _stepCrossFade()
        {
            if(wnd == null)
                return;

            //如果当前阶段有遮罩窗口的话要退出
            _discardMovementMask();
            //清空教程监听
            _clearTutorialTrigger();

            //如果当前阶段不为空
            if(_m_oCurStepObj != null)
            {
                //退出当前阶段
                _ExitCurStep();

                //当前步骤已经是最后一步，隐藏窗口等待关卡结束
                if(_checkEndofTutorial())
                {
                    discard();
                    return;
                }
            }

            //上一步的引导音源释放
            if(_m_alResObjList != null)
                _m_alResObjList.discard();

            //进入新阶段
            _enterNewStep(true);
        }

        //供外部调用的阶段过渡方法
        public void stepCrossFade()
        {
            _stepCrossFade();
        }

        //进入新阶段的Go显隐控制
        protected void _enterNewStep(bool _next)
        {
            if(wnd == null || wnd.stepList == null)
                return;

            _m_iCurStepIndex = _next ? ++_m_iCurStepIndex : --_m_iCurStepIndex;
            _m_oCurStepObj = wnd.stepList[Mathf.Clamp(_m_iCurStepIndex, 0, wnd.stepList.Count - 1)];

            //进入新阶段前的判断及处理
            if(!beforeEnterNewStep())
                return;

            //正式进入新阶段
            stepDeal();

        }

        //正式进入新阶段的处理方法
        protected void stepDeal()
        {
            if(_m_oCurStepObj == null)
                return;

            //读取本阶段相关操作信息
            //点击下一步按钮后执行的操作
            _m_lClickDealFuncInfo = NPPlayerEffectSerializeInfo.readEffectList(_m_oCurStepObj.clickDealFunc);
            //本阶段执行的操作：使用tutorialFunc字符串方式解析
            _m_lStepDealFuncInfo = NPPlayerEffectSerializeInfo.readEffectList(_m_oCurStepObj.stepDealFunc);
            //拖拽相关操作数据
            if(null != _m_oCurStepObj.dragInfo)
            {
                _m_lDragEndFuncInfo = NPPlayerEffectSerializeInfo.readEffectList(_m_oCurStepObj.dragInfo.dragEndFunc);//正确的拖拽结束后的操作
                _m_lDragBeginFuncInfo = NPPlayerEffectSerializeInfo.readEffectList(_m_oCurStepObj.dragInfo.dragBeginFunc);//开始拖拽时的操作
                _m_lDragFuncInfo = NPPlayerEffectSerializeInfo.readEffectList(_m_oCurStepObj.dragInfo.dragFunc);    //拖拽时的操作
                _m_lDragFailureFuncInfo = NPPlayerEffectSerializeInfo.readEffectList(_m_oCurStepObj.dragInfo.dragFailureFunc); //拖拽失败的操作
            }
            else
            {
                _m_lDragEndFuncInfo = null;
                _m_lDragBeginFuncInfo = null;
                _m_lDragFuncInfo = null;
                _m_lDragFailureFuncInfo = null;
            }

            //进入新阶段，获取触发配置
            _m_eListenTutorialTriggerType = _m_oCurStepObj.triggerType;
            _m_eListenTutorialTriggerTypeArgs = _m_oCurStepObj.triggerTypeArgs;

#if UNITY_EDITOR
            if(Game.instance.mainCamera.gameSetting.forceTutorialDelay)
                _m_fNextClickCanResponseTime = Time.realtimeSinceStartup + _m_oCurStepObj.delayClickResponseTime;
            else
                _m_fNextClickCanResponseTime = Time.realtimeSinceStartup;
#else
        _m_fNextClickCanResponseTime = Time.realtimeSinceStartup + _m_oCurStepObj.delayClickResponseTime;
#endif
            //加载并播放本阶段音源
            LoadGuideVoice(_m_oCurStepObj.gudieVoiceIndex.mainId, _m_oCurStepObj.gudieVoiceIndex.subId, null);

            //判断是否需要监听消息
            if(_m_oCurStepObj.triggerType != ENPTutorialTriggerType.NONE)
            {
                //此时设定允许esc
                QueueMgr.instance.OpenRollBack();
                //Debug.LogError("进入新阶段，获取触发配置:  " + _m_oCurStepObj.triggerType );
                WinMsg.RegisterMsg(WinMsgType.CONTROL_TUROTIAL_SETP, _onTutorialTrigger);
            }

            //埋点数据合法 发送埋点数据
            //if(_m_oCurStepObj.buriedPointID > 0)
            //    WCGGSClientListener.sendMsgByLog(WCGGSWriter_001_BasicOp.make_004_ReqTutorialDataBuriedPoint(GameResCore.instance.remoteVersionNum, _m_oCurStepObj.buriedPointID));

            _setHighLightGoPosition();
            
            if(_m_oCurStepObj != null)
                ALCommonTaskController.CommonActionAddMonoTask(_initMovementMask, _m_oCurStepObj.delayInitMoveMaskTime);

            //判断是否需要延时自动跳过本阶段，如时间有效则开始延时处理
            if(_m_oCurStepObj.autoDelayToNextStep > 0.01f)
            {
                //记录当前步骤
                NPGGUIMonoTutorialWndStepObj dealStep = _m_oCurStepObj;
                ALCommonTaskController.CommonActionAddMonoTask(
                    () =>
                    {
                        //判断阶段是否相同
                        if(_m_oCurStepObj != dealStep)
                            return;

                        //自动去下一步
                        _stepCrossFade();
                    }
                    , _m_oCurStepObj.autoDelayToNextStep);
            }

            _setGameObjectEnable(_m_oCurStepObj.enableGo, true);
            _setGameObjectEnable(_m_oCurStepObj.disableGo, false);

            //绑定拖拽操作
            if(_m_oCurStepObj.dragInfo != null)
            {
                ALUGUICommon.combineBeginDrag(_m_oCurStepObj.dragInfo.dragGo, OnBeginDarg);
                ALUGUICommon.combineDrag(_m_oCurStepObj.dragInfo.dragGo, OnDarg);
                ALUGUICommon.combineEndDrag(_m_oCurStepObj.dragInfo.dragGo, OnEndDarg);
            }
            
            //开始解析新阶段操作字符串
            _dealStepStr(_m_lStepDealFuncInfo);

            //发送埋点
            if (_m_oCurStepObj != null && _m_oCurStepObj.traceId > 0)
                GCommon.sendStepReport(new TraceStepData(_m_oCurStepObj.traceId, 1, _m_oCurStepObj.name));
            if (_AALMonoMain.instance.showDebugOutput && _m_oCurStepObj != null)
                Debug.Log($"Trace>>>当前引导步骤：{_m_oCurStepObj.name}，埋点ID：{_m_oCurStepObj.traceId}");

            afterEnterNewStep();
            
            //判断是否要自动进入下一步
            if (null != _m_oCurStepObj && !string.IsNullOrEmpty(_m_oCurStepObj.autoNextStepCondition))
            {
                NPPlayerConditionGroupObj condition = NPPlayerConditionGroupObj.readConditionGroupList(_m_oCurStepObj.autoNextStepCondition, "tutorial:autoNextStepCondition ");
                if (condition != null && condition.IsEnable(null))
                {
                    _dealBtnClick(0);
                    return;
                }   
            }
        }

        //加载语音
        public void LoadGuideVoice(int _mainId, int _subId, Action _loadedCall)
        {
            if(0 == _mainId && 0 == _subId)
                return;

            AGuideVoiceRefCore.instance.loadObj(_mainId, _subId, (resObj) =>
            {
                if(OnGuideVoiceLoaded(resObj))
                {
                    if(_loadedCall != null)
                        _loadedCall();
                }
            });
        }

        //创建音源对象
        private bool OnGuideVoiceLoaded(_ATALObjResObj<GameObject> _resObj)
        {
            if(null == _resObj)
                return false;

            //long mergeIndex = ALCommon.mergeInt(_resObj.mainId, _resObj.subId);

            //获取资源对象
            _resObj.createObj(_m_alResObjList);

            return true;
        }

        //本教程阶段拖拽操作开始的时候的响应事务
        private void OnBeginDarg(PointerEventData _eventData)
        {
            _m_vDragPos = _eventData.position;
            //开始拖拽的事务流(进入XXX状态)
            //_dealStepStr(_m_oCurStepObj.dragInfo.dragBeginFunc);
            _dealStepStr(_m_lDragBeginFuncInfo);
        }

        //本教程阶段拖拽执行的响应事务
        private void OnDarg(PointerEventData _eventData)
        {
            _m_vDragPos = _eventData.position;

            //_dealStepStr(_m_oCurStepObj.dragInfo.dragFunc);
            _dealStepStr(_m_lDragFuncInfo);
        }

        //拖拽结束后的处理
        private void OnEndDarg(PointerEventData _eventData)
        {
            if(_m_oCurStepObj == null || _m_oCurStepObj.dragInfo == null)
                return;

            _m_vDragPos = _eventData.position;

            if(RectTransformUtility.RectangleContainsScreenPoint(_m_oCurStepObj.dragInfo.dragEndRect, _m_vDragPos, Game.instance.mainCamera.fullCanvas.worldCamera))
            {
                //如果拖拽到了指定区域，就处理拖拽成功的操作流
                //_dealStepStr(_m_oCurStepObj.dragInfo.dragEndFunc);
                _dealStepStr(_m_lDragEndFuncInfo);
                //进入下一步
                _stepCrossFade();
            }
            else
            {
                //未拖拽到指定区域,重置拖拽对象坐标
                if(_m_oCurStepObj == null || _m_oCurStepObj.dragInfo == null)
                    return;

                ALUGUICommon.setUIPos(_m_oCurStepObj.dragInfo.dragGo, Vector2.zero);
                _m_oCurStepObj.dragInfo.dragGo.transform.localScale = go.transform.localScale;
                
                //执行拖拽失败的操作
                //_dealStepStr(_m_oCurStepObj.dragInfo.dragFailureFunc);
                _dealStepStr(_m_lDragFailureFuncInfo);

                //退回到上一步
                //_rollStepBack();
            }
        }

        //回退到上一步
        private void _rollStepBack()
        {
            if(wnd == null)
                return;

            //如果当前阶段不为空,退出当前阶段
            if(_m_oCurStepObj != null)
                _ExitCurStep();

            //如果当前阶段有遮罩窗口的话要退出
            _discardMovementMask();

            //进入新阶段
            _enterNewStep(false);

            if(wnd.stepList == null || _m_oCurStepObj == null || _m_oCurStepObj.stepDealFunc == null)
            {
                Debug.LogError("can not find StepStr,check Monownd's stepList.");
                return;
            }

            //开始解析新阶段操作字符串
            //_dealStepStr(wnd.stepList[_m_iCurStepIndex].stepDealFunc);
            _dealStepStr(_m_lStepDealFuncInfo);
        }

        //退出当前阶段的Go显隐控制
        protected void _ExitCurStep()
        {
            if(_m_oCurStepObj == null)
                return;

            _setGameObjectEnable(_m_oCurStepObj.exitEnableGo, true);
            _setGameObjectEnable(_m_oCurStepObj.exitDisableGo, false);
        }

        //设置List内Go显隐
        private void _setGameObjectEnable(List<GameObject> _GoList, bool _enable)
        {
            if(_GoList == null)
                return;

            for(int i = 0; i < _GoList.Count; i++)
                ALUGUICommon.setGameObjEnable(_GoList[i], _enable);
        }

        //解析stepDealFunc字符串
        protected void _dealStepStr(string _str)
        {
            if(_str == null || _str == "")
                return;

            NPPlayerEffectSerializeInfo.dealEffect(NPPlayerEffectSerializeInfo.readEffectList(_str), null);
        }
        protected void _dealStepStr(List<NPPlayerEffectSerializeInfo> _effectInfo)
        {
            if(_effectInfo == null)
                return;

            NPPlayerEffectSerializeInfo.dealEffect(_effectInfo, null);
        }

        /// <summary>
        /// 设置高亮物体位置
        /// </summary>
        private void _setHighLightGoPosition()
        {
            if(_m_oCurStepObj == null || _m_oCurStepObj.highlightGoRectTransform == null || string.IsNullOrEmpty(_m_oCurStepObj.highlightGoLocationStr))
                return;
            
            _ATutorialHighlightLocationDealer locationDealer = _ATutorialHighlightLocationDealer.readHighlightLocationDealer(_m_oCurStepObj.highlightGoLocationStr);
            if (locationDealer != null && locationDealer.getCenterUIRootPosition(out Vector2 _uiRootVec))
            {
                Vector3 uiPos = _m_oCurStepObj.highlightGoRectTransform.position;
                Vector2 highLightGoCenterUIRootVec = GCommon.getUIRootPos(uiPos, true) + 
                                               new Vector2(_m_oCurStepObj.highlightGoRectTransform.rect.width * (0.5f - _m_oCurStepObj.highlightGoRectTransform.pivot.x), _m_oCurStepObj.highlightGoRectTransform.rect.height * (0.5f - _m_oCurStepObj.highlightGoRectTransform.pivot.y));
                Vector2 positionOffset = _uiRootVec - highLightGoCenterUIRootVec;

                _m_oCurStepObj.highlightGoRectTransform.anchoredPosition = _m_oCurStepObj.highlightGoRectTransform.anchoredPosition + (positionOffset);
            }
        }
        
        //生成运动遮罩并执行
        private void _initMovementMask()
        {
            if(_m_oCurStepObj == null || _m_oCurStepObj.MoveMaskRect == null)
                return;

            //判断是否需要动态获取聚焦区域
            if(null != _m_oCurStepObj.moveMaskRectStr && _m_oCurStepObj.moveMaskRectStr.Length > 0)
            {
                //获取区域
                _AWCGTutorialNoticeRectDealer rectDealer = _AWCGTutorialNoticeRectDealer.readRectDealer(_m_oCurStepObj.moveMaskRectStr);
                if(null != rectDealer)
                {
                    //获取区域
                    Rect rect;
                    if(rectDealer.getRect(out rect))
                    {
                        //计算区域中心坐标
                        Vector2 rectCenter = rect.center;
                        _m_oCurStepObj.MoveMaskRect.localScale = Vector3.one;

                        //根据区域设置当前蒙版区域
                        Vector2 pos = GCommon.getUIRootPos(_m_oCurStepObj.MoveMaskRect);
                        //计算移动差值
                        _m_oCurStepObj.MoveMaskRect.anchoredPosition = _m_oCurStepObj.MoveMaskRect.anchoredPosition + (rectCenter - pos);
                    }
                }
            }

            //传入RectTransfrom来置顶遮罩窗口并进行运动
            NPGTutorialGUIAddSceneMovementMask.instance.enterScene(_m_oCurStepObj);
            NPGTutorialGUIAddSceneMovementMask.instance.regInitDelegate(() =>
            {
                //是否需要移动动画
                if (_m_oCurStepObj.isMoveMaskEnable)
                {
                    NPGGUIWndMovementMask.instance.foucsTarget(_m_oCurStepObj.MoveMaskRect);
                }
                else
                {
                    NPGGUIWndMovementMask.instance.setFoucsTarget(_m_oCurStepObj.MoveMaskRect);
                }
            });
        }

        //销毁运动遮罩
        protected void _discardMovementMask()
        {
            NPGTutorialGUIAddSceneMovementMask.instance.forceQuitAddScene();
        }

        //判断是否是最后一步
        protected bool _checkEndofTutorial()
        {
            if(_m_iCurStepIndex < wnd.stepList.Count - 1)
                return false;

            _tutorialEndDeal();

            return true;
        }

        //本阶段教程结束的事件处理
        private void _tutorialEndDeal()
        {
            if(wnd == null)
                return;

            //执行效果
            if(wnd.turotialEndFunc != null && wnd.turotialEndFunc != "")
            {
                NPPlayerEffectSerializeInfo.dealEffect(NPPlayerEffectSerializeInfo.readEffectList(wnd.turotialEndFunc), null);
            }
            

            //重置数据
            _resetData();

            //退出遮罩
            _discardMovementMask();
            //隐藏窗口
            hideWnd();

            //发送引导完成的消息
            WinMsg.SendMsg(WinMsgType.CUR_TUTORIAL_DONE);
        }

        //本阶段教程跳过方式结束的事件处理
        private void _tutorialSkipEndDeal()
        {
            if(wnd == null)
                return;

            //执行效果
            if(wnd.skipTurotialEndFunc != null && wnd.skipTurotialEndFunc != "")
            {
                NPPlayerEffectSerializeInfo.dealEffect(NPPlayerEffectSerializeInfo.readEffectList(wnd.skipTurotialEndFunc), null);
            }
            
            //重置数据
            _resetData();

            //退出遮罩
            _discardMovementMask();
            //隐藏窗口
            hideWnd();
        }


        //进入新阶段前的判定
        protected abstract bool beforeEnterNewStep();
        //进入新阶段后的事务
        protected abstract void afterEnterNewStep();
    }
}




