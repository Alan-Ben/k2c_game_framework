using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public abstract class _AMarsBuildingView : _AALBasicLoadObj, _IMarsIntelligentControlEffectPlayer
    {
        private readonly MarsBuildingInfo _m_buildingInfo;
        private readonly NPGGoIndex _m_resIndex;
        private readonly Vector3 _m_position;
        
        
        protected _AMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo)
        {
            _m_buildingInfo = _buildingInfo;
            _m_resIndex = _m_buildingInfo.getCurrentResIndex();
            _m_position = MainAdditionMarsTDScene.instance.getBuildingPos(_m_buildingInfo.refObj.id);
        }
        

        [NotNull] public MarsBuildingInfo buildingInfo { get { return _m_buildingInfo; } }
        public NPGGoIndex resIndex { get { return _m_resIndex; } }
        public Vector3 position { get { return _m_position; } }
        

        public abstract void tick();
        public abstract void playIntelligentControlEffect(long _intelligentControlId, Action _complete = null);
    }
    public abstract class _AMarsBuildingView<T> : _AMarsBuildingView where T : MonoBehaviour
    {
        private T _m_mono;
        
        
        protected _AMarsBuildingView([NotNull] MarsBuildingInfo _buildingInfo) 
            : base(_buildingInfo)
        {
        }
        
        
        public T mono { get { return _m_mono; } }
        
        
        protected override void _loadOp()
        {
            MainAdditionMarsTDScene.instance.createUnit<T>(resIndex, position, _mono =>
            {
                if (_mono == null)
                {
                    _onInitDone();
                    _setLoadDone();
                    return;
                }

                _m_mono = _mono;
                _onInitDone();
                _setLoadDone();
            });
        }
        protected override void _discard()
        {
            _onDiscard();
            MainAdditionMarsTDScene.instance.discardUnit(resIndex, _m_mono);
        }

        protected abstract void _onInitDone();
        protected abstract void _onDiscard();
    }
}