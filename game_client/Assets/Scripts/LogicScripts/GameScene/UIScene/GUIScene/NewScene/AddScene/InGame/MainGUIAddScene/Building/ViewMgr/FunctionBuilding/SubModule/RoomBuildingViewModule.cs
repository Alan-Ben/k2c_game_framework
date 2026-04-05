using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 卧室建筑视图模块
    /// </summary>
    public class RoomBuildingViewModule : _AFunctionBuildingViewMgrSubModule
    {
        [NotNull] private readonly Dictionary<GTDMonoRoomFunction, RoomBuildingView> _m_buildingViewDict;
        private bool _m_isInit;
        private int _m_initSerialize;
        
        
        public RoomBuildingViewModule()
        {
            _m_buildingViewDict = new Dictionary<GTDMonoRoomFunction, RoomBuildingView>();
        }


        public override void init(Action _complete)
        {
            if (_m_isInit)
            {
                _complete?.Invoke();
                return;
            }
            _m_isInit = true;
            
            _complete?.Invoke();
        }
        public override void discard()
        {
            if (!_m_isInit)
                return;
            _m_isInit = false;

            foreach (RoomBuildingView buildingView in _m_buildingViewDict.Values)
            {
                buildingView.discard();
            }
            _m_buildingViewDict.Clear();
            
            _m_initSerialize = ALSerializeOpMgr.next();
        }
        public override void addBuilding(_AGTDMonoBuildingFunction _function, Action _complete)
        {
            if (!_m_isInit)
            {
                _complete?.Invoke();
                return;
            }
            
            if (_function is not GTDMonoRoomFunction roomFunction || roomFunction.parentTrans == null)
            {
                _complete?.Invoke();
                return;
            }
            
            PlayerRoomSkinRefObj roomSkinRefObj = NPPlayer.instance.roomSkinComp.currentRoomSkinRefObj;
            if (roomSkinRefObj == null)
            {
                _complete?.Invoke();
                return;
            }
            
            RoomBuildingView buildingView = new RoomBuildingView(roomSkinRefObj, roomFunction.parentTrans.position);
            _m_buildingViewDict.Add(roomFunction, buildingView);
            buildingView.load(_complete);
        }
        public override void removeBuilding(_AGTDMonoBuildingFunction _function)
        {
            if (!_m_isInit)
                return;
            
            if (_function is not GTDMonoRoomFunction roomFunction)
                return;

            if (!_m_buildingViewDict.TryGetValue(roomFunction, out RoomBuildingView buildingView))
                return;
            
            buildingView.discard();
            _m_buildingViewDict.Remove(roomFunction);
        }
    }
}
