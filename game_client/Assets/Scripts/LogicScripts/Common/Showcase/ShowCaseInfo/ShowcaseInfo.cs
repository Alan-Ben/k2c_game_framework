using System;
using System.Collections.Generic;
using System.Linq;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 单个showcase的数据存储及管理对象
    /// </summary>
    public partial class ShowcaseInfo
    {
        //当前序列号
        private long _m_lOpSerialize;
        //这个showcase对象的父节点
        private Transform _m_parent;

        //是否所有步骤完成
        private bool _m_isAllDone;
        //所有加载都完成的回调
        private Action _m_OnAllDone;
        private Action _m_showPrepareDone;
        
        //当前模板信息
        private NPGShowcaseIndex _m_templateIndex;
        //加载出来的模板信息
        private NPShowcaseTemplateMono _m_templateMono;
        //单位index信息列表
        private ShowCaseIndexObj[] _m_unitIndexList;
        //摄像头状态
        private EShowcaseCameraType _m_eShowcaseCameraType;
        //摄像头信息
        private _IShowcaseCameraController _m_showcaseCameraController;
        //RT的大小信息
        private RenderTextureDescriptor _m_RTdescriptor;
        /// <summary>
        /// 当前设定的UI视觉中心
        /// </summary>
        private RectTransform _m_vViewRect;
        
        private long _m_lightSerialize;
        
        public ShowcaseInfo(Transform _parent)
        {
            _m_parent = _parent;
            _m_lOpSerialize = ALSerializeOpMgr.next();
            _m_lightSerialize = 0;
        }

        public NPGShowcaseIndex TemplateIndex { get { return _m_templateIndex; } }
        public NPShowcaseTemplateMono TemplateMono { get { return _m_templateMono; } }
        public int unitIndexCount { get { return null == _m_unitIndexList ? 0 : _m_unitIndexList.Count(); } }
        public _IShowcaseCameraController showcaseCameraController { get { return _m_showcaseCameraController; } }

        /// <summary>
        /// 开始显示
        /// </summary>
        public void enableShowCase(NPGShowcaseIndex _templateIndex, _AShowCaseUnitInfoObj[] _unitList, EShowcaseCameraType _cameraType, RenderTextureDescriptor _descriptor)
        {
            if(null == _templateIndex)
                return;

            _m_lOpSerialize = ALSerializeOpMgr.next();
            long serialize = _m_lOpSerialize;

            _m_isAllDone = false;
            
            ALProcess basicProcess = ALProcess.CreateProcess("showcase");
            basicProcess
                //舞台加载
                .addDelegateProcess((delegateAction) =>
                {
                    if(serialize != _m_lOpSerialize)
                        return;
                    _loadTemplate(_templateIndex, delegateAction, serialize);
                }) 
                //单位加载
                .addDelegateProcess((delegateAction) =>
                {
                    if(serialize != _m_lOpSerialize)
                        return;
                    
                    _setUnitData(_unitList, delegateAction);
                }) 
                //摄像头定位
                .addProcess(() =>
                {
                    if(serialize != _m_lOpSerialize)
                        return;
                    _putCameraAt(_cameraType, _descriptor);
                })
                //所有步骤完成回调
                .addProcess(() =>
                {
                    if(serialize != _m_lOpSerialize)
                        return;
                    _onAllDone();
                });

            basicProcess.dealProcess();
        }

        /// <summary>
        /// 关闭显示
        /// </summary>
        public void disableShowCase()
        {
            _clearAll();
            
            _m_OnAllDone = null;
            _m_showPrepareDone = null;
            _m_lOpSerialize = ALSerializeOpMgr.next();
        }
        
        /// <summary>
        /// 单独加载单位，用户舞台加载后动态切换加载单位
        /// </summary>
        public void additionLoadUnitAutoRemove(_AShowCaseUnitInfoObj _unitInfo, int _index, float _discardTime, Action _discardAction)
        {
            //有单位要加载重置标记位，说明还在加载中
            _m_isAllDone = false;
            //设置单位数据
            _setUnitData(_unitInfo, _index, null);
            //设置完成
            _onAllDone();
            
            long serialize = _m_lOpSerialize;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                if(serialize != _m_lOpSerialize)
                    return;
                removeLoadUnit(_index);

                if (_discardAction != null)
                    _discardAction();
            }, _discardTime);
        }

        /// <summary>
        /// 单独加载单位，用户舞台加载后动态切换加载单位
        /// </summary>
        public void additionLoadUnit(_AShowCaseUnitInfoObj _unitInfo, int _index, Action _doneAction)
        {
            //有单位要加载重置标记位，说明还在加载中
            _m_isAllDone = false;
            
            //设置单位数据
            _setUnitData(_unitInfo, _index, _doneAction);
            
            //设置完成
            _onAllDone();
        }
        
        /// <summary>
        /// 移除加载的单位
        /// </summary>
        /// <param name="_index"></param>
        public void removeLoadUnit(int _index)
        {
            if (_m_unitIndexList == null || _index < 0 || _index >= _m_unitIndexList.Length)
                return;

            _m_unitIndexList[_index]?.discardUnitObj();
        }
        
        /// <summary>
        /// 设置UI视觉中心
        /// </summary>
        /// <param name="_viewRect">UI中心</param>
        public void setUIViewCenter(RectTransform _viewRect)
        {
            _m_vViewRect = _viewRect;
        }
        
        /// <summary>
        /// 加载模板
        /// </summary>
        private void _loadTemplate(NPGShowcaseIndex _templateIndex, Action _doneAction, long _serialize)
        {
            if (null == _templateIndex || !_templateIndex.isValid())
            {
                Debug.LogError($"[showcase]模板加载失败:{_templateIndex}");
                return;
            }

            ShowCaseResMgr.instance.popTemplate(_templateIndex, (_index, _mono) =>
            {
                //序列号不一样不处理，直接扔回去
                if(_serialize != _m_lOpSerialize)
                {
                    ShowCaseResMgr.instance.pushBackTemplate(_index, _mono);
                    return;
                }
                
                if (null == _mono)
                {
                    Debug.LogError($"[showcase]模板加载失败:{_templateIndex}");
                    return;
                }
                
                //每次重新加载舞台都清理一次单位对象
                _clearLoadUnit();
                //模板信息清除
                _clearTemplate();

                //复制新信息
                _m_templateIndex = _templateIndex;
                //舞台赋值
                _m_templateMono = _mono;
                _m_templateMono.transform.SetParent(_m_parent);
                _m_templateMono.transform.localPosition = Vector3.zero;
                ALPackage.ALUGUICommon.setGameObjEnable(_m_templateMono.gameObject, true);
                
                //开启showcase模板上的独立灯光
                _m_lightSerialize = LightsMgr.instance.openAdditionLight(_m_templateMono.lightGoIndex, null);
                
                //有几个位置就创建单位数组多少个
                if (null != _m_templateMono.unitList)
                {
                    _m_unitIndexList = new ShowCaseIndexObj[_m_templateMono.unitList.Count];
                    for (int i = 0; i < _m_templateMono.unitList.Count; i++)
                    {
                        _m_unitIndexList[i] = new ShowCaseIndexObj(_m_templateMono.unitList[i], _m_templateMono.getAnimatorTagByIndex(i), _m_templateMono.useDefaultPos);
                        _m_unitIndexList[i].init();
                    }
                }
                else
                {
                    _m_unitIndexList = Array.Empty<ShowCaseIndexObj>();
                }
                
                if (null != _doneAction)
                    _doneAction();
            });
        }
        
        //设置单位数据
        private void _setUnitData(_AShowCaseUnitInfoObj _unitInfo, int _index, Action _action)
        {
            if (null == _unitInfo || null == _m_unitIndexList || _m_unitIndexList.Length <= _index)
            {
                if (null != _action)
                    _action();
                return;
            }

            ShowCaseIndexObj itemIndexObj = _m_unitIndexList[_index];
            if(null == itemIndexObj)
            {
                if (null != _action)
                    _action();
                return;
            }
            
            itemIndexObj.addUnitObj(_unitInfo, _action);
        }
        
        //设置单位数据
        private void _setUnitData(_AShowCaseUnitInfoObj[] _unitList, Action _action)
        {
            if(null == _unitList || _unitList.Length == 0)
            {
                if (null != _action)
                    _action();
                return;
            }
            
            if(null == _m_unitIndexList)
                return;
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_unitList.Length);
            stepCounter.regAllDoneDelegate(_action);

            //赋值对应位置加载列表
            for (int i = 0; i < _unitList.Length; i++)
            {
                _setUnitData(_unitList[i], i, stepCounter.addDoneStepCount);
            }
        }

        /// <summary>
        /// 摄像头定位
        /// </summary>
        private void _putCameraAt(EShowcaseCameraType _cameraType, RenderTextureDescriptor _descriptor)
        {
            if(_cameraType == EShowcaseCameraType.None)
                return;

            if (null == _m_templateMono || null == _m_templateMono.cameraPlaceholders)
            {
                Debug.LogError($"[showcase]移动摄像头时舞台还没加载完成");
                return;
            }
            
            //清除旧相机
            _clearCamera();
            
            //赋值新值
            _m_RTdescriptor = _descriptor;
            _m_eShowcaseCameraType = _cameraType;
            
            if (_m_eShowcaseCameraType == EShowcaseCameraType.RTCamera)
            {
                RTCameraController rtCameraController = RTCameraMgr.instance.popAvailableInfo();
                if (null != rtCameraController)
                {
                    //设置rt大小
                    rtCameraController.initRenderTexture(_m_RTdescriptor);
                    
                    _m_showcaseCameraController = rtCameraController;
                    _m_showcaseCameraController.setDataTo(_m_templateMono.cameraPlaceholders.transform.parent, _m_templateMono.cameraPlaceholders.cameraData);
                    _m_showcaseCameraController.enableShowcaseCamera();
                }
            }
            else if(_m_eShowcaseCameraType == EShowcaseCameraType.MainCamera)
            {
                float centerDepthZ = 0;
                if (_m_vViewRect != null)
                {
                    if (_m_templateMono.viewCenter != null)
                    {
                        Vector3 camPos = _m_templateMono.cameraPlaceholders.transform.parent.TransformPoint(_m_templateMono.cameraPlaceholders.cameraData.localPosition);
                        centerDepthZ = (_m_templateMono.viewCenter.transform.position - camPos).magnitude ;
                    }
                    else
                    {
                        if (_m_templateMono.unitList.Count > 0 && _m_templateMono.unitList[0] != null)
                        {
                            Vector3 camPos = _m_templateMono.cameraPlaceholders.transform.parent.TransformPoint(_m_templateMono.cameraPlaceholders.cameraData.localPosition);
                            centerDepthZ = (_m_templateMono.unitList[0].position - camPos).magnitude ;
                        }
                    }
                }
                // ViewRect有无都设置，内部判断有则相机偏移，null则不处理
                RTMainCameraController.instance.setUIViewCenter(_m_vViewRect, centerDepthZ);
                
                _m_showcaseCameraController = RTMainCameraController.instance;
                _m_showcaseCameraController.setDataTo(_m_templateMono.cameraPlaceholders.transform.parent, _m_templateMono.cameraPlaceholders.cameraData);
                _m_showcaseCameraController.enableShowcaseCamera();
            }
            
            //开启阴影裁剪边界
            if(null != _m_showcaseCameraController)
                _m_showcaseCameraController.setShadowClipBounds(_m_templateMono.isOpenPlayerShadowBounds, _m_templateMono.finalPlayerShadowBounds.getBoundsCorners());
        }

        /// <summary>
        /// 清空当前showcase对象所有东西
        /// </summary>
        private void _clearAll()
        {
            _m_isAllDone = false;
            
            //摄像头处理
            _clearCamera();
            
            //单位信息清除
            _clearLoadUnit();
            
            //模板信息清除
            _clearTemplate();
        }

        //模板信息清除
        private void _clearTemplate()
        {
            //关闭当前模板舞台上的附加灯光
            if(_m_lightSerialize != 0)
                LightsMgr.instance.closeAdditionLight(_m_lightSerialize);
            _m_lightSerialize = 0;
            
            if (null != _m_templateMono)
            {
                ShowCaseResMgr.instance.pushBackTemplate(_m_templateIndex, _m_templateMono);
                _m_templateMono = null;
            }
            _m_templateIndex = null;
        }

        //清除摄像机信息
        private void _clearCamera()
        {
            //摄像头处理
            if (null != _m_showcaseCameraController)
            {
                if (_m_eShowcaseCameraType == EShowcaseCameraType.RTCamera)
                {
                    RTCameraMgr.instance.pushBackInfo(_m_showcaseCameraController as RTCameraController);
                }

                _m_showcaseCameraController.disableShowcaseCamera();
                _m_showcaseCameraController = null;
            }
            
            _m_eShowcaseCameraType = EShowcaseCameraType.None;
        }
        
        //单位信息清除
        private void _clearLoadUnit()
        {
            //单位信息清除
            if (null != _m_unitIndexList)
            {
                ShowCaseIndexObj showCaseIndexObj = null;
                for (int i = 0; i < _m_unitIndexList.Length; i++)
                {
                    showCaseIndexObj = _m_unitIndexList[i];
                    if(null == showCaseIndexObj)
                        continue;
                    
                    showCaseIndexObj.discard();
                    _m_unitIndexList[i] = null;
                }
            }
            _m_unitIndexList = null;
        }

        /// <summary>
        /// 当全部完成
        /// </summary>
        private void _onAllDone()
        {
            _m_isAllDone = true;
            
            //赋值回调
            Action doneAction = _m_OnAllDone;
            _m_OnAllDone = null;
            //执行回调
            if (null != doneAction)
                doneAction();
        }

        /// <summary>
        /// 注册显示完成回调
        /// </summary>
        /// <param name="_action"></param>
        public void regAllDoneDelegate(Action _action)
        {
            if(null == _action)
                return;
            
            if (_m_isAllDone)
            {
                if (null != _action)
                    _action();
            }

            _m_OnAllDone += _action;
        }
        
        //注册表现准备完成回调，主要视频用
        public void regShowPrepareDoneDelegate(Action _action)
        {
            //首先要加载完成
            regAllDoneDelegate(() =>
            {
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(_m_unitIndexList.Length);
                stepCounter.regAllDoneDelegate(_action);

                ShowCaseIndexObj item = null;
                for (var i = 0; i < _m_unitIndexList.Length; i++)
                {
                    item = _m_unitIndexList[i];
                    if (null == item)
                    {
                        stepCounter.addDoneStepCount();
                        continue;
                    }
                    
                    item.regShowPrepareDone(() =>
                    {
                        stepCounter.addDoneStepCount();
                    });
                }
            });
        }

        //获取RenderTexture
        public RenderTexture getRenderTexture()
        {
            if (_m_eShowcaseCameraType != EShowcaseCameraType.RTCamera)
                return null;
            RTCameraController rtCameraController = _m_showcaseCameraController as RTCameraController;
            if (null == rtCameraController)
                return null;
            
            return rtCameraController.renderTexture;
        }
    }
}

