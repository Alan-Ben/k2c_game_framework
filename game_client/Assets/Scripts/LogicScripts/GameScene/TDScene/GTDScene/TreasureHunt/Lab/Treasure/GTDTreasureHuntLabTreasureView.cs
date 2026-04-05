using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GTDTreasureHuntLabTreasureView : _AALBasicLoadObj
    {
        private NPGGoIndex _m_goIndex;
        private Transform _m_parent;
        private GameObject _m_go;
        private GTDMonoTreasureHuntLabTreasure _m_mono;
        
        private _ITreasureHuntTreasureInfo _m_iTreasureInfo;
        
        public GTDTreasureHuntLabTreasureView(NPGGoIndex goIndex, Transform _parent)
        {
            _m_goIndex = goIndex;
            _m_parent = _parent;
        }
        
        protected override void _loadOp()
        {
            if (null == _m_goIndex || !_m_goIndex.isValid())
            {
                Debug.LogError($"NPGGoIndex加载错误: _m_goIndex:{_m_goIndex} 无效");
                _setLoadDone();
                return;
            }

            GGoResCore.instance.loadObj(_m_goIndex, (_assetHandle) =>
            {
                if (_assetHandle == null || _assetHandle.loadedInfo == null)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _m_goIndex.mainId + " - " + _m_goIndex.subId);
#endif
                    
                    _setLoadDone();
                    return;
                }
                //获取资源对象
                GameObject assetGo = _assetHandle.loadedInfo.obj;
                if (null == assetGo)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGGoIndex加载错误: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    _setLoadDone();
                    return;
                }
                _m_go = GameObject.Instantiate(assetGo);
                _m_mono = _m_go.GetComponent<GTDMonoTreasureHuntLabTreasure>();
                if (_m_mono == null)
                {
#if UNITY_EDITOR
                    UnityEngine.Debug.LogError("NPGGoIndex加载错误, 资源上缺少GTDMonoTreasureHuntLabTreasure脚本: " + _assetHandle.mainId + " - " + _assetHandle.subId);
#endif
                    ALUnityCommon.releaseGameObj(_m_go);
                    _m_go = null;
                    _setLoadDone();
                    return;
                }

                if(_m_mono.btnDraw != null)
                    _m_mono.btnDraw.onClick += _onClick;
                
                _m_go.transform.SetParent(_m_parent);
                _m_go.transform.localPosition = Vector3.zero;
                _m_go.transform.localScale = Vector3.one;
                _m_go.transform.rotation = Quaternion.identity;
                _setLoadDone();
            });
        }

        protected override void _discard()
        {
            _m_iTreasureInfo = null;
            
            if (_m_mono != null)
            {
                if(_m_mono.btnDraw != null)
                    _m_mono.btnDraw.onClick -= _onClick;
                
                _m_mono = null;    
            }
            
            if (null != _m_go)
                ALUnityCommon.releaseGameObj(_m_go);
            _m_go = null;
        }

        public void show()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_TREASURE_HUNT_TREASURE_OUTPUT_CHG, _onTreasureOutputInfoChg);
            
            ALUGUICommon.setGameObjEnable(_m_go, true);
            
            _refresh();
        }
        
        public void hide()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_TREASURE_HUNT_TREASURE_OUTPUT_CHG, _onTreasureOutputInfoChg);
            
            ALUGUICommon.setGameObjEnable(_m_go, false);
        }

        public void setData(_ITreasureHuntTreasureInfo _treasureInfo)
        {
            _m_iTreasureInfo = _treasureInfo;

            _refresh();
        }
        
        private void _refresh()
        {
            if(_m_mono == null || _m_iTreasureInfo == null)
                return;
            TreasureHuntTreasureRefObj treasureRefObj = _m_iTreasureInfo.treasureRefObj;
            if(treasureRefObj == null)
                return;

            _refreshOutputShow();
        }
        
        private void _refreshOutputShow()
        {
            if(_m_mono == null || _m_iTreasureInfo == null)
                return;
            
            if (_m_iTreasureInfo.treasureOutputRefObj != null)
            {
                ALUGUICommon.setGameObjEnable(_m_mono.isOutputTreasureShow, true);
                ALUGUICommon.setGameObjEnable(_m_mono.isOutputTreasureHide, false);
                
                TreasureHuntTreasureOutputInfo treasureOutputInfo = NPPlayer.instance.treasureHuntComponent.getTreasureOutputInfo(_m_iTreasureInfo.treasureId);
                if (treasureOutputInfo != null && treasureOutputInfo.canDraw)
                {
                    ALUGUICommon.setGameObjEnable(_m_mono.hasOutputCanDrawShow, true);
                    ALUGUICommon.setGameObjEnable(_m_mono.noOutputCanDrawShow, false);
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(_m_mono.hasOutputCanDrawShow, false);
                    ALUGUICommon.setGameObjEnable(_m_mono.noOutputCanDrawShow, true);    
                }
            }
            else
            {
                ALUGUICommon.setGameObjEnable(_m_mono.isOutputTreasureShow, false);
                ALUGUICommon.setGameObjEnable(_m_mono.isOutputTreasureHide, true);
                
                ALUGUICommon.setGameObjEnable(_m_mono.hasOutputCanDrawShow, false);
                ALUGUICommon.setGameObjEnable(_m_mono.noOutputCanDrawShow, false);
            }
        }
        
        private void _refreshOutputShow(TreasureHuntTreasureOutputInfo _treasureOutputInfo)
        {
            if(_m_mono == null)
                return;
            
            if (_treasureOutputInfo != null)
            {
                ALUGUICommon.setGameObjEnable(_m_mono.isOutputTreasureShow, true);
                ALUGUICommon.setGameObjEnable(_m_mono.isOutputTreasureHide, false);
                
                if (_treasureOutputInfo.canDraw)
                {
                    ALUGUICommon.setGameObjEnable(_m_mono.hasOutputCanDrawShow, true);
                    ALUGUICommon.setGameObjEnable(_m_mono.noOutputCanDrawShow, false);
                }
                else
                {
                    ALUGUICommon.setGameObjEnable(_m_mono.hasOutputCanDrawShow, false);
                    ALUGUICommon.setGameObjEnable(_m_mono.noOutputCanDrawShow, true);    
                }
            }
            else
            {
                ALUGUICommon.setGameObjEnable(_m_mono.isOutputTreasureShow, false);
                ALUGUICommon.setGameObjEnable(_m_mono.isOutputTreasureHide, true);
                
                ALUGUICommon.setGameObjEnable(_m_mono.hasOutputCanDrawShow, false);
                ALUGUICommon.setGameObjEnable(_m_mono.noOutputCanDrawShow, false);
            }
        }
        
        private void _onClick()
        {
            if(_m_iTreasureInfo == null)
                return;

            TreasureHuntTreasureOutputInfo treasureOutputInfo = NPPlayer.instance.treasureHuntComponent.getTreasureOutputInfo(_m_iTreasureInfo.treasureId);
            if(treasureOutputInfo == null || !treasureOutputInfo.canDraw)
                return;
            
            NPPlayer.instance.treasureHuntComponent.reqTreasureHuntDrawTreasureOutput(_m_iTreasureInfo.treasureId, null);
        }

        private void _onTreasureOutputInfoChg(params object[] _args)
        {
            if(_args == null || _args.Length < 1 || !(_args[0] is TreasureHuntTreasureOutputInfo outputInfo) 
               || _m_iTreasureInfo == null || _m_iTreasureInfo.treasureId != outputInfo.treasureId)
                return;
            
            _refreshOutputShow(outputInfo);
        }
    }
    
    // 旧版本, GTDMonoTreasureHuntLabTreasure无需加载, 直接在GTDMonoTreasureHuntLab中配置
    // public class GTDTreasureHuntLabTreasureView
    // {
    //     [NotNull] private GTDMonoTreasureHuntLabTreasure _m_treasureMono;
    //     
    //     private GGUICommonFollowTarget _m_followTarget;
    //     private GGUITreasureHuntLabTreasureFollowItemController _m_followItemController;
    //
    //     private bool _m_bIsInit;
    //     private bool _m_bInitDone;
    //     private Action _m_aOnInitDone;
    //     
    //     public GTDTreasureHuntLabTreasureView([NotNull] GTDMonoTreasureHuntLabTreasure _mono)
    //     {
    //         _m_treasureMono = _mono;
    //
    //         _m_bIsInit = false;
    //         _m_bInitDone = false;
    //         _m_aOnInitDone = null;
    //     }
    //
    //     public void init(Action _onInitDoneAction = null)
    //     {
    //         if (_m_bInitDone)//先判断是否初始化完成, 若已经完成, 直接调用回调
    //         {
    //             _onInitDoneAction?.Invoke();
    //             return;
    //         }
    //
    //         regOnInitDoneDelegate(_onInitDoneAction);//注册回调
    //         if (_m_bIsInit)//判断是否已经调用过init, 若已经调用过, 直接返回
    //             return;
    //
    //         _m_bIsInit = true;
    //         
    //         if (_m_treasureMono.uiFollowParent == null || _m_treasureMono.uiFollowResPathId < 0)
    //         {
    //             _onInitDone();
    //         }
    //         else
    //         {
    //             _m_followTarget = new GGUICommonFollowTarget(_m_treasureMono.uiFollowParent, Vector3.zero);
    //             GGUIWndTreasureHuntLabFollowItemRoot.instance.regInstance(_m_followTarget);
    //
    //             _m_followItemController = new GGUITreasureHuntLabTreasureFollowItemController(_m_treasureMono.uiFollowResPathId);
    //             _m_followTarget.addController(_m_followItemController);
    //             
    //             _m_followItemController.regItemWndLoadDoneDelegate(_onInitDone);
    //         }
    //     }
    //     
    //     public void discard()
    //     {
    //         _m_aOnInitDone = null;
    //         _m_bIsInit = false;
    //         _m_bInitDone = false;
    //         
    //         _m_followItemController?.discard();
    //         _m_followItemController = null;
    //
    //         if (_m_followTarget != null)
    //         {
    //             GGUIWndTreasureHuntLabFollowItemRoot.instance.removeInstance(_m_followTarget);
    //             _m_followTarget.discard();
    //             _m_followTarget = null;
    //         }
    //     }
    //
    //     public void regOnInitDoneDelegate(Action _action)
    //     {
    //         if(_action == null)
    //             return;
    //
    //         if (_m_bInitDone)
    //         {
    //             _action();
    //             return;                
    //         }
    //
    //         _m_aOnInitDone += _action;
    //     }
    //
    //     private void _onInitDone()
    //     {
    //         if(!_m_bIsInit)//初始化完成后, 先判断_m_bIsInit是否为false, 为false则表示还未调用过init方法 或 已经调用了discard方法进行了销毁, 这两种情况都不需要后续操作
    //             return;
    //
    //         _m_bInitDone = true;
    //         
    //         Action action = _m_aOnInitDone;
    //         _m_aOnInitDone = null;
    //         
    //         action?.Invoke();
    //     }
    //
    //     public void show()
    //     {
    //         ALUGUICommon.setGameObjEnable(_m_treasureMono, true);
    //         
    //         _m_followItemController?.dealItemWnd((_wnd) =>
    //         {
    //             _wnd?.showWnd();
    //         });
    //     }
    //
    //     public void hide()
    //     {
    //         ALUGUICommon.setGameObjEnable(_m_treasureMono, false);
    //         
    //         _m_followItemController?.dealItemWnd((_wnd) =>
    //         {
    //             _wnd?.hideWnd();
    //         });
    //     }
    // }
}