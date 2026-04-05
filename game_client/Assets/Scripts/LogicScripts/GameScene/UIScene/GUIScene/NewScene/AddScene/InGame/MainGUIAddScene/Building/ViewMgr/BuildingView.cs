
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class BuildingView : _AALBasicLoadObj
    {
        [NotNull] private readonly FunctionBuildingViewMgr _m_funcBuildingViewMgr;
        [NotNull] private readonly BuildingInfo _m_buildingInfo;
        private readonly NPGGoIndex _m_resIndex;
        private readonly Vector3 _m_position;

        private GTDMonoBuilding _m_mono;
        private GGUICommonFollowTarget _m_followTarget;
        private GGUIWndBuildingBuildFollowBtnFollowItemController _m_buildBtnFollower;
        private _AGTDMonoBuildingFunction[] _m_functionArray;
        private _IGTDHoneEntryPointView[] _m_entryPointView;

        private bool _m_isNextOrderedBuildTipShow;


        public BuildingView([NotNull]FunctionBuildingViewMgr _funcBuildingViewMgr, [NotNull] BuildingInfo _buildingInfo)
        {
            _m_funcBuildingViewMgr = _funcBuildingViewMgr;
            _m_buildingInfo = _buildingInfo;
            _m_resIndex = _m_buildingInfo.isBuilt ? _m_buildingInfo.baseRef.built_res_index : _m_buildingInfo.baseRef.unbuilt_res_index;
            _m_position = MainAdditionBuildingTDScene.instance.getBuildingPos(_buildingInfo.id);
            _m_buildBtnFollower = new GGUIWndBuildingBuildFollowBtnFollowItemController();
            _m_isNextOrderedBuildTipShow = false;
        }
        

        public Vector3 position { get { return _m_position; } }
        public NPGGoIndex resIndex { get { return _m_resIndex; } }
        public _IGTDHoneEntryPointView[] entryPointViewList { get { return _m_entryPointView; } }


        protected override void _loadOp()
        {
            MainAdditionBuildingTDScene.instance.createBuilding<GTDMonoBuilding>(_m_resIndex, _m_position, _mono =>
            {
                if (_mono == null)
                {
                    _setLoadDone();
                    return;
                }
                
                _m_mono = _mono;

                if (_m_mono.hudTarget != null)
                {
                    _m_followTarget = new GGUICommonFollowTarget(_m_mono.hudTarget, Vector3.zero);
                    GGUIWndBuildingFollow.instance.regInstance(_m_followTarget);
                    if (_m_isNextOrderedBuildTipShow)
                    {
                        _m_buildBtnFollower ??= new GGUIWndBuildingBuildFollowBtnFollowItemController();
                        _m_buildBtnFollower.setBuildingInfo(_m_buildingInfo);
                        _m_followTarget.addController(_m_buildBtnFollower);
                    }
                }
                if (_m_mono.clickMono != null)
                    _m_mono.clickMono.onClick += _onBuildingClick;
                
                _AGTDHomeEntryPointMono_Base[] entryPointMonoArray = _m_mono.GetComponents<_AGTDHomeEntryPointMono_Base>();
                if (entryPointMonoArray != null)
                {
                    _m_entryPointView = new _IGTDHoneEntryPointView[entryPointMonoArray.Length];
                    for (int i = 0; i < entryPointMonoArray.Length; i++)
                    {
                        _AGTDHomeEntryPointMono_Base entryMono = entryPointMonoArray[i];
                        _IGTDHoneEntryPointView pointView = EntryPointViewFactory.instance.createEntryPointView(entryMono);
                        if (pointView == null)
                            continue;
                        
                        pointView.init();
                        pointView.refreshShow();
                        _m_entryPointView[i] = pointView;
                    }
                }

                _m_functionArray = _m_mono.GetComponents<_AGTDMonoBuildingFunction>();
                if (_m_functionArray == null)
                {
                    _setLoadDone();
                    return;
                }
                
                ALStepCounter stepCounter = new ALStepCounter();
                stepCounter.chgTotalStepCount(_m_functionArray.Length + 1);
                stepCounter.regAllDoneDelegate(_setLoadDone);
                foreach (_AGTDMonoBuildingFunction function in _m_functionArray)
                {
                    // todo: 目前这个 buildingId 不用策划去配置，潜规则上和 buildingInfo 强制绑定，但是客户端框架其实可以拆开。
                    // todo: 如果需要拆开，就删除这行，把 buildingId 让策划。
                    function.buildingId = _m_buildingInfo.id;
                    _m_funcBuildingViewMgr.addFunctionBuilding(function, stepCounter.addDoneStepCount);
                }
                
                stepCounter.addDoneStepCount();
            });
        }
        protected override void _discard()
        {
            if (_m_mono == null)
                return;

            if (_m_functionArray != null)
            {
                foreach (_AGTDMonoBuildingFunction function in _m_functionArray)
                    _m_funcBuildingViewMgr.removeFunctionBuilding(function);
                _m_functionArray = null;
            }

            if (_m_entryPointView != null)
            {
                foreach (_IGTDHoneEntryPointView pointView in _m_entryPointView)
                    pointView?.discard();
                _m_entryPointView = null;
            }

            _m_followTarget?.discard();
            _m_followTarget = null;
            _m_buildBtnFollower = null;
            
            if (_m_mono.clickMono != null)
                _m_mono.clickMono.onClick -= _onBuildingClick;

            MainAdditionBuildingTDScene.instance.discardBuilding<GTDMonoBuilding>(_m_resIndex, _m_mono);
            _m_mono = null;
        }

        public void setNextOrderedBuildTip(bool _isShow)
        {
            if (_m_isNextOrderedBuildTipShow == _isShow)
                return;
            
            _m_isNextOrderedBuildTipShow = _isShow;
            if (_m_followTarget != null)
            {
                if (_m_isNextOrderedBuildTipShow)
                {
                    _m_buildBtnFollower ??= new GGUIWndBuildingBuildFollowBtnFollowItemController();
                    _m_buildBtnFollower.setBuildingInfo(_m_buildingInfo);
                    _m_followTarget.addController(_m_buildBtnFollower);
                }
                else
                {
                    _m_buildBtnFollower?.discard();
                    _m_buildBtnFollower = null;
                }
            }
        }


        private void _onBuildingClick()
        {
            if (_m_buildingInfo.isBuilt)
                return;

            if (_m_followTarget == null)
                return;

            GGUIWndBuildingUnbuiltNonsenseFollowItemController nonsense = new GGUIWndBuildingUnbuiltNonsenseFollowItemController();
            nonsense.setNonsenseTranslated(TextTranslate.instance.getLanguage(_m_buildingInfo.baseRef.unbuilt_click_tip));
            _m_followTarget.addController(nonsense);
        }
    }
}