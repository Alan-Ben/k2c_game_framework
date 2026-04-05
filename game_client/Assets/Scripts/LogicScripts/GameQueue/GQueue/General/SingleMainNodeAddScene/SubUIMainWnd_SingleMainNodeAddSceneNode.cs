using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 通用的进入MainUIScene视图的处理节点对象
    /// </summary>
    public class SubUIMainWnd_SingleMainNodeAddSceneNode : _AUISingleMainNodeAddSceneNode
    {
        private _AALBasicLoadUIWndBasicClass _m_addWnd;
        private _AALBasicSubContainerScene _m_scParentScene;

        /// <summary>
        /// 进入节点前的行为
        /// </summary>
        private Action _m_dPreEnterDone;
        /// <summary>
        /// 进入完成之后调用的函数
        /// </summary>
        private Action _m_dOnEnterDone;

        public SubUIMainWnd_SingleMainNodeAddSceneNode(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicSubContainerScene _parentScene)
            : base()
        {
            _m_addWnd = _addWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public SubUIMainWnd_SingleMainNodeAddSceneNode(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicSubContainerScene _parentScene, string _nodeTag)
            : base(_nodeTag, true)
        {
            _m_addWnd = _addWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = null;
            _m_dOnEnterDone = null;
        }
        public SubUIMainWnd_SingleMainNodeAddSceneNode(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicSubContainerScene _parentScene, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk)
            : base(_nodeTag, _needTransBk)
        {
            _m_addWnd = _addWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = _preEnterDone;
            _m_dOnEnterDone = _onEnterDone;
        }
        public SubUIMainWnd_SingleMainNodeAddSceneNode(_AALBasicLoadUIWndBasicClass _addWnd, _AALBasicSubContainerScene _parentScene, EUIQueueStageType _stageType, string _nodeTag, Action _preEnterDone, Action _onEnterDone, bool _needTransBk)
            : base(_stageType, _nodeTag, _needTransBk)
        {
            _m_addWnd = _addWnd;
            _m_scParentScene = _parentScene;
            _m_dPreEnterDone = _preEnterDone;
            _m_dOnEnterDone = _onEnterDone;
        }

        /// <summary>
        /// 返回模糊背景情况下当前显示的UI对象
        /// </summary>
        protected override _AALBasicLoadUIWndBasicClass blurCurUI { get { return _m_addWnd; } }

        /// <summary>
        /// 单独展示的Scene对象
        /// </summary>
        protected override _AALBasicSubContainerScene_NoChild _getAddScene { get { return _m_scParentScene; } }

        protected override void _preEnterAddScene()
        {
            if (null != _m_dPreEnterDone)
                _m_dPreEnterDone();
            _m_dPreEnterDone = null;
        }

        protected override void _onEnterAddSceneDone()
        {
            //尝试直接展示窗口
            if (null != _m_scParentScene)
                _m_scParentScene.showMainWnd(_m_addWnd, _onWndShowDone);
            else
                _onWndShowDone();
        }

        /// <summary>
        /// 展示窗口后的处理函数
        /// </summary>
        protected void _onWndShowDone()
        {
            //保证顺序在bk后
            if (_m_addWnd != null) 
                GCommon.moveTransformToLastAndRefreshLayer(_m_addWnd.getGameObj());
            
            if (null != _m_dOnEnterDone)
                _m_dOnEnterDone();
            _m_dOnEnterDone = null;
        }
    }
}
