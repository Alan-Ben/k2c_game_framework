using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 卧室建筑视图
    /// </summary>
    public class RoomBuildingView : _AALBasicLoadObj
    {
        [NotNull] private readonly PlayerRoomSkinRefObj _m_roomSkinRefObj;
        private readonly Vector3 _m_position;
        private readonly NPGGoIndex _m_resIndex;

        private GTDMonoRoomBuilding _m_mono;
        private GGUICommonFollowTarget _m_nameFollowTarget;
        private GGUIWndRoomBuildingEntranceFollowItemController _m_nameFollower;
        
        
        public RoomBuildingView([NotNull] PlayerRoomSkinRefObj _roomSkinRef, Vector3 _position)
        {
            _m_roomSkinRefObj = _roomSkinRef;
            _m_position = _position;
            _m_resIndex = _roomSkinRef.building_index;
        }
        

        public Vector3 position { get { return _m_position; } }
        public NPGGoIndex resIndex { get { return _m_resIndex; } }
        
       
        protected override void _loadOp()
        {
            MainAdditionBuildingTDScene.instance.createBuilding<GTDMonoRoomBuilding>(_m_resIndex, _m_position,
                _mono =>
                {
                    if (_mono == null)
                    {
                        _setLoadDone();
                        return;
                    }

                    _m_mono = _mono;

                    // 注册名字跟随目标
                    if (_m_mono.nameHudTarget != null)
                    {
                        _m_nameFollowTarget = new GGUICommonFollowTarget(_m_mono.nameHudTarget, Vector3.zero);
                        GGUIWndBuildingFollow.instance.regInstance(_m_nameFollowTarget);
                    }

                    // 绑定点击事件
                    if (_m_mono.clickMono != null)
                        _m_mono.clickMono.onClick += _onBuildingClick;

                    // 添加名字跟随器
                    if (_m_nameFollowTarget != null)
                    {
                        _m_nameFollower = new GGUIWndRoomBuildingEntranceFollowItemController();
                        _m_nameFollower.setRoomSkinRef(_m_roomSkinRefObj);
                        _m_nameFollowTarget.addController(_m_nameFollower);
                    }

                    _setLoadDone();
                });
        }
        protected override void _discard()
        {
            if (_m_mono == null)
                return;
            
            _m_nameFollowTarget?.discard();
            _m_nameFollowTarget = null;
            _m_nameFollower = null;
            
            if (_m_mono.clickMono != null)
                _m_mono.clickMono.onClick -= _onBuildingClick;
            
            MainAdditionBuildingTDScene.instance.discardBuilding<GTDMonoRoomBuilding>(_m_resIndex, _m_mono);
            _m_mono = null;
        }
        
        
        /// <summary>
        /// 点击建筑时的处理
        /// </summary>
        private void _onBuildingClick()
        {
            QueueMgr.instance.AddNode(new GNodeRoom());
        }
    }
}
