using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class TreasureHuntGroundUnit : _AGameLoadUnit, _IRelativePosUnit
    {
        private readonly NPGGoIndex _m_resIndex;
        private GTDMonoTreasureHuntGround _m_mono;
        private Vector3 _m_relativePosition;
        
        
        public TreasureHuntGroundUnit(NPGGoIndex _resIndex, [NotNull] _AGameLogic _gameLogic)
            : base(_gameLogic)
        {
            _m_resIndex = _resIndex;
        }
        
        
        public override void init()
        {
            _trySetGroundOffset();
        }
        public override void discard()
        {
        }


        #region Relative Position
        Vector3 _IRelativePosUnit.position { get { return Vector3.zero; } }
        Vector3 _IRelativePosUnit.relativePosition { set { _m_relativePosition = value; _trySetGroundOffset(); } }
        #endregion
        
        
        private void _trySetGroundOffset()
        {
            if (_m_mono == null) 
                return;
            
            _m_mono.setOffset(_m_relativePosition.z);
        }
        
        
        protected override void _loadOp(Action _complete)
        {
            Transform groundPos = MainAdditionTreasureHuntGameTDScene.instance.getGroundPos();
            MainAdditionTreasureHuntGameTDScene.instance.createUnit<GTDMonoTreasureHuntGround>(_m_resIndex, Vector3.zero, _mono =>
            {
                _m_mono = _mono;
                if (_m_mono == null)
                {
                    ALLog.Error($"{_m_resIndex} load failed, mono is null.");
                    _complete.Invoke();
                    return;
                }

                if (groundPos != null)
                {
                    _m_mono.transform.localPosition = groundPos.localPosition;
                    _m_mono.transform.localRotation = groundPos.localRotation;
                    _m_mono.transform.localScale = groundPos.localScale;
                }
                _trySetGroundOffset();
                _complete.Invoke();
            });
        }
        protected override void _unloadOp()
        {
            if (_m_mono != null)
            {
                MainAdditionTreasureHuntGameTDScene.instance.discardUnit(_m_resIndex, _m_mono);
                _m_mono = null;
            }
        }
    }
}