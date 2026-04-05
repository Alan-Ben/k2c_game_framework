using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE.MiniGame
{
    public abstract class _ATakeThingsSequentiallyGameThingState : _ASimpleState<ETakeThingsSequentiallyGameThingState>
    {
        [NotNull] protected TakeThingsSequentiallyGameThingUnit _m_thingUnit;
        public _ATakeThingsSequentiallyGameThingState([NotNull] TakeThingsSequentiallyGameThingUnit _thingUnit)
        {
            _m_thingUnit = _thingUnit;
        }

        protected override void _onEnter()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[_ATakeThingsSequentiallyGameThingState] _m_matchItemBgUnit:{_m_thingUnit.thingId} 进入:{state}状态");
            }

            _m_thingUnit.thingShow.setThingState(state);//刷新物体状态
            _onEnterSub();
        }

        protected override void _onExit()
        {
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
            {
                Debug.Log_EditorOnly($"{Time.frameCount}[_ATakeThingsSequentiallyGameThingState] _m_matchItemBgUnit:{_m_thingUnit.thingId} 退出:{state}状态");
            }

            _onExitSub();
        }
        
        protected abstract void _onEnterSub();
        
        protected abstract void _onExitSub();
    }
}