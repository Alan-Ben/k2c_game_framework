using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// UI队列管理器
    /// </summary>
    public class QueueMgr : _AALQueueMgr<BaseQueueNode>
    {
        private static QueueMgr _g_instance = new QueueMgr();
        public static QueueMgr instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new QueueMgr();
                return _g_instance;
            }
        }

        #region 统一对外的便捷接口处理

        /// <summary>
        /// 进入一个只有操作的节点，这个节点操作之后就失效。
        /// 一般是用于某些操作需要进行引导判断的时候的处理
        /// </summary>
        /// <param name="_delegate"></param>
        public void addNode_OnlyOp(Action _delegate)
        {
            AddNode(new OnlyOpNode(_delegate));
        }
        public void addNode_OnlyOp(string _nodeUITag)
        {
            AddNode(new OnlyOpNode(null, _nodeUITag));
        }
        public void addNode_OnlyOp(Action _delegate, string _nodeUITag)
        {
            AddNode(new OnlyOpNode(_delegate, _nodeUITag));
        }

        /// <summary>
        /// 单独的展示一个窗口的处理
        /// 这个处理中一定有背景蒙版，且点击会关闭窗口
        /// </summary>
        /// <param name="_wndObj"></param>
        /// <param name="_onWndLoadDone"></param>
        public void addNode_InGame_SingleWnd(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone)
        {
            AddNode(new GAddNode_SingleWnd(_wndObj, _onWndLoadDone));
        }
        public void addNode_InGame_SingleWnd(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, string _nodeTag)
        {
            AddNode(new GAddNode_SingleWnd(_wndObj, _onWndLoadDone, _nodeTag));
        }
        public void addNode_InGame_SingleWnd(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, string _nodeTag, bool _needClickBkNoRemove)
        {
            AddNode(new GAddNode_SingleWnd(_wndObj, _onWndLoadDone, _nodeTag, _needClickBkNoRemove));
        }
        public void addNode_InGame_SingleWnd(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, string _nodeTag, bool _needClickBkNoRemove, Action _onClickBkNoRemoveDealAction)
        {
            AddNode(new GAddNode_SingleWnd(_wndObj, _onWndLoadDone, _nodeTag, _needClickBkNoRemove, _onClickBkNoRemoveDealAction));
        }
        public void addNode_InGame_SingleWnd(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, string _nodeTag, bool _needClickBkNoRemove ,bool _needAutoRemove)
        {
            AddNode(new GAddNode_SingleWnd(_wndObj, _onWndLoadDone, _nodeTag, _needClickBkNoRemove, _needAutoRemove));
        }

        public void addNode_InGame_SingleWnd(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, EUIQueueStageType _stageType, string _nodeTag)
        {
            AddNode(new GAddNode_SingleWnd(_wndObj, _onWndLoadDone, _stageType, _nodeTag));
        }
        public void addNode_InGame_SingleWnd(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove)
        {
            AddNode(new GAddNode_SingleWnd(_wndObj, _onWndLoadDone, _stageType, _nodeTag, _needAutoRemove));
        }
        public void addNode_InGame_SingleWnd(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove, bool _needTransBk)
        {
            AddNode(new GAddNode_SingleWnd(_wndObj, _onWndLoadDone, _stageType, _nodeTag, _needAutoRemove, _needTransBk));
        }
        public void addNode_InGame_SingleWnd(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove, bool _needTransBk, bool _needRemovePreAutoRemove)
        {
            AddNode(new GAddNode_SingleWnd(_wndObj, _onWndLoadDone, _stageType, _nodeTag, _needAutoRemove, _needTransBk, _needRemovePreAutoRemove));
        }
        
        /// <summary>
        /// 单独的展示一个窗口的处理
        /// 这个处理中一定有背景蒙版，且点击会关闭窗口
        /// </summary>
        /// <param name="_wndObj"></param>
        /// <param name="_onWndLoadDone"></param>
        public void addNode_InGame_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone)
        {
            AddNode(new GAddNode_SingleWnd_OnlyCloseDiscard(_wndObj, _onWndLoadDone));
        }
        public void addNode_InGame_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, string _nodeTag)
        {
            AddNode(new GAddNode_SingleWnd_OnlyCloseDiscard(_wndObj, _onWndLoadDone, _nodeTag));
        }
        public void addNode_InGame_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, string _nodeTag, bool _needClickBkNoRemove)
        {
            AddNode(new GAddNode_SingleWnd_OnlyCloseDiscard(_wndObj, _onWndLoadDone, _nodeTag, _needClickBkNoRemove));
        }
        public void addNode_InGame_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, string _nodeTag, bool _needClickBkNoRemove, Action _onClickBkNoRemoveDealAction)
        {
            AddNode(new GAddNode_SingleWnd_OnlyCloseDiscard(_wndObj, _onWndLoadDone, _nodeTag, _needClickBkNoRemove, _onClickBkNoRemoveDealAction));
        }
        public void addNode_InGame_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, string _nodeTag, bool _needClickBkNoRemove ,bool _needAutoRemove)
        {
            AddNode(new GAddNode_SingleWnd_OnlyCloseDiscard(_wndObj, _onWndLoadDone, _nodeTag, _needClickBkNoRemove, _needAutoRemove));
        }

        public void addNode_InGame_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, EUIQueueStageType _stageType, string _nodeTag)
        {
            AddNode(new GAddNode_SingleWnd_OnlyCloseDiscard(_wndObj, _onWndLoadDone, _stageType, _nodeTag));
        }
        public void addNode_InGame_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove)
        {
            AddNode(new GAddNode_SingleWnd_OnlyCloseDiscard(_wndObj, _onWndLoadDone, _stageType, _nodeTag, _needAutoRemove));
        }
        public void addNode_InGame_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove, bool _needTransBk)
        {
            AddNode(new GAddNode_SingleWnd_OnlyCloseDiscard(_wndObj, _onWndLoadDone, _stageType, _nodeTag, _needAutoRemove, _needTransBk));
        }
        public void addNode_InGame_SingleWnd_OnlyCloseDiscard(_AALBasicLoadUIWndBasicClass _wndObj, Action _onWndLoadDone, EUIQueueStageType _stageType, string _nodeTag, bool _needAutoRemove, bool _needTransBk, bool _needRemovePreAutoRemove)
        {
            AddNode(new GAddNode_SingleWnd_OnlyCloseDiscard(_wndObj, _onWndLoadDone, _stageType, _nodeTag, _needAutoRemove, _needTransBk, _needRemovePreAutoRemove));
        }

        /// <summary>
        /// 添加一个单独的AddScene的节点
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        public void addNode_InGame_SingleAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene)
        {
            AddNode(new UISingleAddSceneNode(_addContainerUIScene));
        }
        public void addNode_InGame_SingleAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag)
        {
            AddNode(new UISingleAddSceneNode(_addContainerUIScene, _nodeTag));
        }
        public void addNode_InGame_SingleAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new UISingleAddSceneNode(_addContainerUIScene, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_SingleAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag)
        {
            AddNode(new UISingleAddSceneNode(_addContainerUIScene, _stageType, _nodeTag));
        }
        public void addNode_InGame_SingleAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new UISingleAddSceneNode(_addContainerUIScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone));
        }

        /// <summary>
        /// 添加一个单独的AddScene的节点同时作为主节点（MainNode）
        /// 这个处理模式可带有模糊背景
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        public void addNode_SingleMainNodeAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene)
        {
            AddNode(new UISingleMainNodeAddSceneNode(_addContainerUIScene));
        }
        public void addNode_SingleMainNodeAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag)
        {
            AddNode(new UISingleMainNodeAddSceneNode(_addContainerUIScene, _nodeTag));
        }
        public void addNode_SingleMainNodeAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk = true)
        {
            AddNode(new UISingleMainNodeAddSceneNode(_addContainerUIScene, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk));
        }
        public void addNode_SingleMainNodeAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag)
        {
            AddNode(new UISingleMainNodeAddSceneNode(_addContainerUIScene, _stageType, _nodeTag));
        }
        public void addNode_SingleMainNodeAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk = true)
        {
            AddNode(new UISingleMainNodeAddSceneNode(_addContainerUIScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk));
        }


        /// <summary>
        /// 添加一个单独的AddScene的节点同时作为主节点（MainNode）
        /// 在进入后这个单独的AddScene会自动切换到对应的窗口附加视图
        /// 这个处理模式可带有模糊背景
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        public void addNode_SingleMainNodeAddScene_AddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _addContainerUIScene)
        {
            AddNode(new SubUIAddWnd_SingleMainNodeAddSceneNode(_addWnd, _addContainerUIScene));
        }
        public void addNode_SingleMainNodeAddScene_AddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag)
        {
            AddNode(new SubUIAddWnd_SingleMainNodeAddSceneNode(_addWnd, _addContainerUIScene, _nodeTag));
        }
        public void addNode_SingleMainNodeAddScene_AddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk = true)
        {
            AddNode(new SubUIAddWnd_SingleMainNodeAddSceneNode(_addWnd, _addContainerUIScene, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk));
        }
        public void addNode_SingleMainNodeAddScene_AddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag)
        {
            AddNode(new SubUIAddWnd_SingleMainNodeAddSceneNode(_addWnd, _addContainerUIScene, _stageType, _nodeTag, null, null, true));
        }
        public void addNode_SingleMainNodeAddScene_AddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk = true)
        {
            AddNode(new SubUIAddWnd_SingleMainNodeAddSceneNode(_addWnd, _addContainerUIScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk));
        }


        /// <summary>
        /// 添加一个单独的AddScene的节点同时作为主节点（MainNode）
        /// 在进入后这个单独的AddScene会自动切换到对应的窗口主视图
        /// 这个处理模式可带有模糊背景
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        public void addNode_SingleMainNodeAddScene_MainWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _addContainerUIScene)
        {
            AddNode(new SubUIMainWnd_SingleMainNodeAddSceneNode(_addWnd, _addContainerUIScene));
        }
        public void addNode_SingleMainNodeAddScene_MainWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag)
        {
            AddNode(new SubUIMainWnd_SingleMainNodeAddSceneNode(_addWnd, _addContainerUIScene, _nodeTag));
        }
        public void addNode_SingleMainNodeAddScene_MainWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk = true)
        {
            AddNode(new SubUIMainWnd_SingleMainNodeAddSceneNode(_addWnd, _addContainerUIScene, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk));
        }
        public void addNode_SingleMainNodeAddScene_MainWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag)
        {
            AddNode(new SubUIMainWnd_SingleMainNodeAddSceneNode(_addWnd, _addContainerUIScene, _stageType, _nodeTag, null, null, true));
        }
        public void addNode_SingleMainNodeAddScene_MainWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk = true)
        {
            AddNode(new SubUIMainWnd_SingleMainNodeAddSceneNode(_addWnd, _addContainerUIScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk));
        }

        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示子视图的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_InGame_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene)
        {
            AddNode(new MainUIAddSceneNode_Var_InGame(_addContainerUIScene, _parentScene));
        }
        public void addNode_InGame_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene, string _nodeTag)
        {
            AddNode(new MainUIAddSceneNode_Var_InGame(_addContainerUIScene, _parentScene, _nodeTag));
        }
        public void addNode_InGame_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddSceneNode_Var_InGame(_addContainerUIScene, _parentScene, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddSceneNode_Var_InGame(_addContainerUIScene, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone));
        }

        public void addNode_InGame_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene)
        {
            AddNode(new MainUIAddSceneNode_Var_InGame(_addContainerUIScene, GUISceneMain.instance));
        }
        public void addNode_InGame_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag)
        {
            AddNode(new MainUIAddSceneNode_Var_InGame(_addContainerUIScene, GUISceneMain.instance, _nodeTag));
        }
        public void addNode_InGame_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddSceneNode_Var_InGame(_addContainerUIScene, GUISceneMain.instance, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddSceneNode_Var_InGame(_addContainerUIScene, GUISceneMain.instance, _stageType, _nodeTag, _preEnterDone, _onEnterDone));
        }


        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示这个主视图的子主视图的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, string _nodeUITag)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, _nodeUITag));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }

        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, long _backResPathId)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, _backResPathId));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, _nodeUITag));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag, long _backResPathId)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, _backResPathId, _nodeUITag));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag, bool _isOnlyUINode, long _backResPathId)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, _backResPathId, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone, long _backResPathId)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, _backResPathId, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, long _backResPathId)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, _backResPathId, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_InGame(_uiScene, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }


        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示子窗口的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_InGame_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene)
        {
            AddNode(new MainUIAddWndNode_Var_InGame(_addWnd, _parentScene));
        }
        public void addNode_InGame_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, string _nodeTag)
        {
            AddNode(new MainUIAddWndNode_Var_InGame(_addWnd, _parentScene, _nodeTag));
        }
        public void addNode_InGame_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddWndNode_Var_InGame(_addWnd, _parentScene, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddWndNode_Var_InGame(_addWnd, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone, true));
        }
        public void addNode_InGame_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk)
        {
            AddNode(new MainUIAddWndNode_Var_InGame(_addWnd, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk));
        }

        public void addNode_InGame_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd)
        {
            AddNode(new MainUIAddWndNode_Var_InGame(_addWnd, GUISceneMain.instance));
        }
        public void addNode_InGame_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, string _nodeTag)
        {
            AddNode(new MainUIAddWndNode_Var_InGame(_addWnd, GUISceneMain.instance, _nodeTag));
        }
        public void addNode_InGame_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddWndNode_Var_InGame(_addWnd, GUISceneMain.instance, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddWndNode_Var_InGame(_addWnd, GUISceneMain.instance, _stageType, _nodeTag, _preEnterDone, _onEnterDone, true));
        }
        public void addNode_InGame_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk)
        {
            AddNode(new MainUIAddWndNode_Var_InGame(_addWnd, GUISceneMain.instance, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk));
        }
        public void addNode_InGame_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk,bool _needAutoRemove)
        {
            AddNode(new MainUIAddWndNode_Var_InGame(_addWnd, GUISceneMain.instance, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk, _needAutoRemove));
        }
        public void addNode_InGame_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk, bool _needAutoRemove, bool _needClickNoRemove)
        {
            AddNode(new MainUIAddWndNode_Var_InGame(_addWnd, GUISceneMain.instance, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk, _needAutoRemove, _needClickNoRemove));
        }
        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示子窗口的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, string _nodeUITag)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, _nodeUITag));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, long _backResPathId, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, bool _needAutoRemove, bool _isCanRollBackQuit, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, _parentScene, _backResPathId, _stageType, _nodeUITag, _isOnlyUINode, _needAutoRemove, _isCanRollBackQuit, _preEnterDone, _onEnterDone));
        }

        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, _nodeUITag));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag, long _backResPathId)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, GUISceneMain.instance, _backResPathId, _nodeUITag));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag, Action _preEnterDone, Action _onEnterDone, long _backResPathId)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, GUISceneMain.instance, _backResPathId, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone, long _backResPathId)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, GUISceneMain.instance, _backResPathId, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, bool _needAutoRemove, bool _isCanRollBackQuit, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_InGame(_uiWnd, GUISceneMain.instance, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode, _needAutoRemove, _isCanRollBackQuit, _preEnterDone, _onEnterDone));
        }



        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示子视图的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_InGame_SubUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _ANPBasicAddContainerUIScene _parentScene)
        {
            AddNode(new SubUIAddSceneNode_Var_InGame(_addContainerUIScene, _parentScene));
        }
        public void addNode_InGame_SubUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag)
        {
            AddNode(new SubUIAddSceneNode_Var_InGame(_addContainerUIScene, _parentScene, _nodeTag));
        }
        public void addNode_InGame_SubUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIAddSceneNode_Var_InGame(_addContainerUIScene, _parentScene, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_SubUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIAddSceneNode_Var_InGame(_addContainerUIScene, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone));
        }


        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示这个主视图的子视图的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_InGame_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene)
        {
            AddNode(new SubUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK));
        }
        public void addNode_InGame_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag)
        {
            AddNode(new SubUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, _nodeUITag));
        }
        public void addNode_InGame_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new SubUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new SubUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainSceneNode_Var_InGame(_uiScene, _parentScene, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }


        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示子窗口的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_InGame_SubUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene)
        {
            AddNode(new SubUIAddWndNode_Var_InGame(_addWnd, _parentScene));
        }
        public void addNode_InGame_SubUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag)
        {
            AddNode(new SubUIAddWndNode_Var_InGame(_addWnd, _parentScene, _nodeTag));
        }
        public void addNode_InGame_SubUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIAddWndNode_Var_InGame(_addWnd, _parentScene, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_SubUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIAddWndNode_Var_InGame(_addWnd, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone));
        }

        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示子窗口的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_InGame_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene)
        {
            AddNode(new SubUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK));
        }
        public void addNode_InGame_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag)
        {
            AddNode(new SubUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, _nodeUITag));
        }
        public void addNode_InGame_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new SubUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_InGame_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new SubUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_InGame_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainWndNode_Var_InGame(_uiWnd, _parentScene, UIResPathConst.WIN_COMMON_BACK, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }

        ////////////////////  ==== Login 部分开始

        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示子视图的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_Login_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene)
        {
            AddNode(new MainUIAddSceneNode_Var_Login(_addContainerUIScene, _parentScene));
        }
        public void addNode_Login_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene, string _nodeTag)
        {
            AddNode(new MainUIAddSceneNode_Var_Login(_addContainerUIScene, _parentScene, _nodeTag));
        }
        public void addNode_Login_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddSceneNode_Var_Login(_addContainerUIScene, _parentScene, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddSceneNode_Var_Login(_addContainerUIScene, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone));
        }

        public void addNode_Login_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene)
        {
            AddNode(new MainUIAddSceneNode_Var_Login(_addContainerUIScene, NPGUISceneLogin.instance));
        }
        public void addNode_Login_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag)
        {
            AddNode(new MainUIAddSceneNode_Var_Login(_addContainerUIScene, NPGUISceneLogin.instance, _nodeTag));
        }
        public void addNode_Login_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddSceneNode_Var_Login(_addContainerUIScene, NPGUISceneLogin.instance, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddSceneNode_Var_Login(_addContainerUIScene, NPGUISceneLogin.instance, _stageType, _nodeTag, _preEnterDone, _onEnterDone));
        }


        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示这个主视图的子主视图的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, _parentScene));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, string _nodeUITag)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, _parentScene, _nodeUITag));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, _parentScene, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, _parentScene, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, _parentScene, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, _parentScene, _stageType, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, _parentScene, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, _parentScene, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }

        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, NPGUISceneLogin.instance));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, NPGUISceneLogin.instance, _nodeUITag));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, NPGUISceneLogin.instance, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, NPGUISceneLogin.instance, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, NPGUISceneLogin.instance, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, NPGUISceneLogin.instance, _stageType, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, NPGUISceneLogin.instance, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_Login_MainUIMainScene(_ANPBasicAddContainerUIScene _uiScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainSceneNode_Var_Login(_uiScene, NPGUISceneLogin.instance, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }


        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示子窗口的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_Login_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene)
        {
            AddNode(new MainUIAddWndNode_Var_Login(_addWnd, _parentScene));
        }
        public void addNode_Login_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, string _nodeTag)
        {
            AddNode(new MainUIAddWndNode_Var_Login(_addWnd, _parentScene, _nodeTag));
        }
        public void addNode_Login_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddWndNode_Var_Login(_addWnd, _parentScene, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddWndNode_Var_Login(_addWnd, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone, true));
        }
        public void addNode_Login_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk)
        {
            AddNode(new MainUIAddWndNode_Var_Login(_addWnd, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk));
        }

        public void addNode_Login_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd)
        {
            AddNode(new MainUIAddWndNode_Var_Login(_addWnd, NPGUISceneLogin.instance));
        }
        public void addNode_Login_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, string _nodeTag)
        {
            AddNode(new MainUIAddWndNode_Var_Login(_addWnd, NPGUISceneLogin.instance, _nodeTag));
        }
        public void addNode_Login_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddWndNode_Var_Login(_addWnd, NPGUISceneLogin.instance, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIAddWndNode_Var_Login(_addWnd, NPGUISceneLogin.instance, _stageType, _nodeTag, _preEnterDone, _onEnterDone, true));
        }
        public void addNode_Login_MainUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk)
        {
            AddNode(new MainUIAddWndNode_Var_Login(_addWnd, NPGUISceneLogin.instance, _stageType, _nodeTag, _preEnterDone, _onEnterDone, _needTransBk));
        }


        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示子窗口的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, _parentScene));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, string _nodeUITag)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, _parentScene, _nodeUITag));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, _parentScene, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, _parentScene, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, _parentScene, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, _parentScene, _stageType, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, _parentScene, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _AALBasicTopContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, _parentScene, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }

        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, NPGUISceneLogin.instance));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, NPGUISceneLogin.instance, _nodeUITag));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, NPGUISceneLogin.instance, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, NPGUISceneLogin.instance, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, NPGUISceneLogin.instance, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag, bool _isOnlyUINode, bool _isCanRollBackQuit, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, NPGUISceneLogin.instance, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, false, _isCanRollBackQuit, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, string _nodeUITag, bool _isOnlyUINode, bool _needAutoRemove, bool _isCanRollBackQuit, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, NPGUISceneLogin.instance, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _needAutoRemove, _isCanRollBackQuit, _preEnterDone, _onEnterDone));
        }
        
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, NPGUISceneLogin.instance, _stageType, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, NPGUISceneLogin.instance, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_Login_MainUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new MainUIMainWndNode_Var_Login(_uiWnd, NPGUISceneLogin.instance, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }



        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示子视图的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_Login_SubUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _ANPBasicAddContainerUIScene _parentScene)
        {
            AddNode(new SubUIAddSceneNode_Var_Login(_addContainerUIScene, _parentScene));
        }
        public void addNode_Login_SubUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag)
        {
            AddNode(new SubUIAddSceneNode_Var_Login(_addContainerUIScene, _parentScene, _nodeTag));
        }
        public void addNode_Login_SubUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIAddSceneNode_Var_Login(_addContainerUIScene, _parentScene, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_SubUIAddScene(_ANPBasicAddContainerUIScene _addContainerUIScene, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIAddSceneNode_Var_Login(_addContainerUIScene, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone));
        }


        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示这个主视图的子视图的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_Login_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene)
        {
            AddNode(new SubUIMainSceneNode_Var_Login(_uiScene, _parentScene));
        }
        public void addNode_Login_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag)
        {
            AddNode(new SubUIMainSceneNode_Var_Login(_uiScene, _parentScene, _nodeUITag));
        }
        public void addNode_Login_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new SubUIMainSceneNode_Var_Login(_uiScene, _parentScene, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_Login_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainSceneNode_Var_Login(_uiScene, _parentScene, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainSceneNode_Var_Login(_uiScene, _parentScene, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainSceneNode_Var_Login(_uiScene, _parentScene, _stageType, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new SubUIMainSceneNode_Var_Login(_uiScene, _parentScene, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_Login_SubUIMainScene(_ANPBasicAddContainerUIScene _uiScene, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainSceneNode_Var_Login(_uiScene, _parentScene, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }


        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示子窗口的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_Login_SubUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene)
        {
            AddNode(new SubUIAddWndNode_Var_Login(_addWnd, _parentScene));
        }
        public void addNode_Login_SubUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag)
        {
            AddNode(new SubUIAddWndNode_Var_Login(_addWnd, _parentScene, _nodeTag));
        }
        public void addNode_Login_SubUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIAddWndNode_Var_Login(_addWnd, _parentScene, _nodeTag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_SubUIAddWnd(_AALBasicLoadUIWndBasicClass _addWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIAddWndNode_Var_Login(_addWnd, _parentScene, _stageType, _nodeTag, _preEnterDone, _onEnterDone));
        }

        /// <summary>
        /// 在游戏逻辑中，在一个主视图上显示子窗口的各类便捷方法
        /// </summary>
        /// <param name="_addContainerUIScene"></param>
        /// <param name="_parentScene"></param>
        public void addNode_Login_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene)
        {
            AddNode(new SubUIMainWndNode_Var_Login(_uiWnd, _parentScene));
        }
        public void addNode_Login_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag)
        {
            AddNode(new SubUIMainWndNode_Var_Login(_uiWnd, _parentScene, _nodeUITag));
        }
        public void addNode_Login_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new SubUIMainWndNode_Var_Login(_uiWnd, _parentScene, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_Login_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainWndNode_Var_Login(_uiWnd, _parentScene, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainWndNode_Var_Login(_uiWnd, _parentScene, EUIQueueStageType.MAIN, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainWndNode_Var_Login(_uiWnd, _parentScene, _stageType, _nodeUITag, _preEnterDone, _onEnterDone));
        }
        public void addNode_Login_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode)
        {
            AddNode(new SubUIMainWndNode_Var_Login(_uiWnd, _parentScene, _stageType, _nodeUITag, _isOnlyUINode));
        }
        public void addNode_Login_SubUIMainWnd(_AALBasicLoadUIWndBasicClass _uiWnd, _ANPBasicAddContainerUIScene _parentScene, EUIQueueStageType _stageType, string _nodeUITag, bool _isOnlyUINode, Action _preEnterDone, Action _onEnterDone)
        {
            AddNode(new SubUIMainWndNode_Var_Login(_uiWnd, _parentScene, _stageType, _nodeUITag, _isOnlyUINode, _preEnterDone, _onEnterDone));
        }

        #endregion


        /// <summary>
        /// 获取默认根节点
        /// </summary>
        /// <returns></returns>
        protected override BaseQueueNode _getDefaultRootNode()
        {
            return new GNodeBuilding();
        }

        /// <summary>
        /// 寻找最后一个符合类型的节点
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public BaseQueueNode findLastNode(Type _type)
        {
            bool isLastMain;
            BaseQueueNode node = _findLastNode(_type, out isLastMain);
            return node;
        }

        /// <summary>
        /// 寻找最后一个符合类型的节点
        /// </summary>
        /// <param name="_nodeTag"></param>
        /// <returns></returns>
        public BaseQueueNode findLastNode(string _nodeTag)
        {
            bool isLastMain;
            BaseQueueNode node = _findLastNode(_nodeTag, out isLastMain);
            return node;
        }
        
        /// <summary>
        /// 在系统出现基础逻辑错误时触发的事件处理
        /// </summary>
        protected override void _onError()
        {
            //TODo 打开游戏的退出游戏确认框 比如：
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.quit_game)
                , TextTranslate.instance.getLanguage(TransKeyConst.play_more)
                , null
                , TextTranslate.instance.getLanguage(TransKeyConst.quit)
                , () =>
                {
                //处理退出
            }
                );
        }
        /// <summary>
        /// 在没有处理主节点切换时触发的事件函数，或在部分特殊操作时，如没有进行主视图切换都将执行本操作
        /// </summary>
        protected override void _onNotMainNodeChg()
        {
            //触发引导检查效果，只有在notice全部关闭后才触发引导。
            //由于在node chg里统一触发了，这里可以暂不处理
            //NPUINoticeMgr.instance.checkDone(_tryTriggerTutorial);
        }
        /// <summary>
        /// 节点的变更函数，在节点无后续处理情况的时候触发的节点变更函数
        /// </summary>
        private bool __m_bIsRegNoticeCheckDone = false;
        protected override void _onNodeChg()
        {
            //优先展示获取物品信息
            //判断当前视图跟当前场景是否可展示
            if (null != _lastNode && _lastNode.canShowNotice)
            {
                //检查触发功能解锁
                _tryTriggerUnlock();
                
                //先进行展示处理
                if (__m_bIsRegNoticeCheckDone)
                {
                    NPUINoticeMgr.instance.dealTryPopNotice(null);
                }
                else
                {
                    //设置已注册，避免重复注册
                    __m_bIsRegNoticeCheckDone = true;
                    NPUINoticeMgr.instance.dealTryPopNotice(_onNoticeCheckDone);
                }
            }
            else
            {
                //判断是否需要检查教程
                _doNoticeCheckDone(_lastNode, _onAllNodePreDone);
            }
            
            WinMsg.SendMsg(WinMsgType.ON_NODE_CHG);
        }

        /***********
         * 判断是否需要检测教程
         **/
        protected void _onNoticeCheckDone()
        {
            //设置未注册状态，确保下一次可注册
            __m_bIsRegNoticeCheckDone = false;

            //调用具体处理逻辑
            _doNoticeCheckDone(_lastNode, _onAllNodePreDone);
        }

        /// <summary>
        /// 此处处理Notice展示之后的处理
        /// 在所有处理最后需要调用回调，确保回到正常流程
        /// </summary>
        /// <param name="_allNodePreDone"></param>
        protected virtual void _doNoticeCheckDone(BaseQueueNode _lastNode, Action _allNodePreDone)
        {
            if (null == _lastNode)
            {
                if (null != _allNodePreDone)
                    _allNodePreDone();
                return;
            }

            //调用Node的自身提示处理函数，让Node自己决定是否需要处理提示
            _lastNode._doNoticeCheckDone(_allNodePreDone);
        }

        /// <summary>
        /// 所有节点前置操作做完后进行本处理
        /// </summary>
        protected void _onAllNodePreDone()
        {
            ALCommonActionMonoTask.addNextFrameTask(_tryTriggerTutorial);
        }

        /**********************
         * 尝试触发引导
         **/
        private void _tryTriggerTutorial()
        {
            //判断是否需要检查教程
            if(Game.instance.isInTutorial)
                return;

            WinMsg.SendMsg(WinMsgType.TRIGGER_TUTORIAL);
            
            //判断是否需要检查教程,这个是有引导了就不处理了
            if(Game.instance.isInTutorial)
                return;
            
            if(null == _lastNode)
                return;
            
            //尝试触发简易引导
            SimpleTutorialController.instance.checkStartSimpleTutorial(_lastNode.nodeTag);
        }
        
        /**********************
         * 尝试触发功能解锁
         **/
        private void _tryTriggerUnlock()
        {
            WinMsg.SendMsg(WinMsgType.TRIGGER_FUNC_UNLOCK_TIP);
        }
    }

    /// <summary>
    /// 视图节点基类
    /// </summary>
    public abstract class BaseQueueNode : _AALQueueBaseNode
    {
        /// <summary>
        /// 节点类型(预留字段,如果使用不到后面会删掉)
        /// </summary>
        private EUIQueueStageType _m_eStageType = EUIQueueStageType.NONE;

        public BaseQueueNode(EUIQueueStageType _stageType)
            : base((int)_stageType)
        {
            _m_eStageType = _stageType;
        }
        public BaseQueueNode(EUIQueueStageType _stageType, string _nodeUITag)
            : base((int)_stageType, _nodeUITag)
        {
            _m_eStageType = _stageType;
        }


        public EUIQueueStageType npStageType { get { return _m_eStageType; } }
        
        /// <summary>
        /// 允许弹出的提示窗口类型，默认都允许弹
        /// 特殊node需要限制仅限本node可以弹窗指定notice，需要重写这个方法
        /// </summary>
        public virtual ENoticeType enableNoticeType { get { return ENoticeType.NORMAL; } }

        /// <summary>
        /// 进入节点操作
        /// </summary>
        public override void doEnterNode(Action _triggerEnterDone)
        {
            //处理附加操作，当时主界面节点时才对摄像头做处理
            if (IsMainViewNode)
            {
                // 存在一种情况时，提前切换了节点，没有先调用hideblur，所以这里加个判断，当切换节点blur没有被hide的话，先hide一下
                if (ScreenBlurMgr.instance.curIsOpenBlur)
                    ScreenBlurMgr.instance.hideBlur();

                if (isOnlyUINode)
                {
                    //关闭摄像头
                    if (null != CameraController.instance.controlCamera)
                    {
                        CameraController.instance.cameraIsOpenRender = false;
                        WinMsg.SendMsg(WinMsgType.MAIN_CAMERA_DISABLE);
                    }
                }
                else
                {
                    //开启摄像头
                    if (null != CameraController.instance.controlCamera)
                    {
                        CameraController.instance.cameraIsOpenRender = true;
                        WinMsg.SendMsg(WinMsgType.MAIN_CAMERA_ENABLE);
                    }
                }
            }

            //设置展示Notice的样式
            NPUINoticeMgr.instance.addEnableNotice(enableNoticeType);

            //直接调用
            EnterNode();

            //然后调用回调
            if(null != _triggerEnterDone)
                _triggerEnterDone();
        }

        /// <summary>
        /// 节点的退出事件函数
        /// </summary>
        public override void doQuitNode(Action _dealOnQuitDone)
        {
            //移除展示Notice的样式
            NPUINoticeMgr.instance.rmvEnableNotice(enableNoticeType);

            //调用子类
            QuitNode();

            if(null != _dealOnQuitDone)
                _dealOnQuitDone();
        }

        /// <summary>
        /// 在使用回退操作退出的处理
        /// </summary>
        public override void onRollBackQuit()
        {

        }
        /// <summary>
        /// 正常的关闭处理操作
        /// </summary>
        public override void onCloseQuit()
        {
        }

        /// <summary>
        /// 此处处理Notice展示之后的处理
        /// 在所有处理最后需要调用回调，确保回到正常流程
        /// </summary>
        /// <param name="_allNodePreDone"></param>
        protected virtual internal void _doNoticeCheckDone(Action _allNodePreDone)
        {
            if (null != _allNodePreDone)
                _allNodePreDone();
        }

        /// <summary>
        /// 本节点是否一个纯粹的UI节点，如果是则将关闭主摄像头
        /// 本属性只在 isMainUIView的时候生效
        /// </summary>
        public virtual bool isOnlyUINode { get { return false; } }


        /// <summary>
        /// 在进行回退处理的时候，队列系统是否能直接退出本节点
        /// </summary>
        public override bool IsCanRollBackQuit { get { return true; } }

        /** 当前节点是否还有效 */
        public override bool isEnable { get { return true; } }

        /// <summary>
        /// 是否可以展示提示窗口
        /// </summary>
        public virtual bool canShowNotice { get { return true; } }

        /// <summary>
        /// 进入节点操作
        /// </summary>
        public abstract void EnterNode();

        /// <summary>
        /// 退出的处理操作
        /// </summary>
        public abstract void QuitNode();
    }

    /// <summary>
    /// 视图节点基类
    /// </summary>
    public abstract class UIQueueBaseNode : BaseQueueNode
    {
        public UIQueueBaseNode(EUIQueueStageType _stageType)
            : base(_stageType)
        {
        }
        public UIQueueBaseNode(EUIQueueStageType _stageType, string _uiTag)
            : base(_stageType, _uiTag)
        {
        }

        /// <summary>
        /// 在节点进入队列的时候执行的事件函数
        /// </summary>
        public override void onEnterQueue()
        {

        }
        /// <summary>
        /// 不论在任何时候，当节点被退出总队列的时候，都会调用onClose函数
        /// </summary>
        public override void onClose()
        {

        }
    }
}
