using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class TreasureHuntRewardUnit : _AGameLoadUnit, _IRelativePosUnit, _IGameTickUnit
    {
        private readonly NPGGoIndex _m_goIndex;
        [NotNull] private readonly Action<TreasureHuntRewardUnit> _m_destroyFunc;
        private readonly Vector3 _m_spawnRelativePosition;
        private readonly Vector3 _m_spawnLogicPosition;
        
        private GTDMonoTreasureHuntReward _m_mono;

        private Vector3 _m_position;
        private bool _m_isCollected;
        
        
        public TreasureHuntRewardUnit([NotNull] _AGameLogic _gameLogic, NPGGoIndex _goIndex, [NotNull] Action<TreasureHuntRewardUnit> _destroyFunc, Vector3 _spawnRelativePosition, Vector3 _spawnLogicPosition) 
            : base(_gameLogic)
        {
            _m_goIndex = _goIndex;
            _m_destroyFunc = _destroyFunc;
            _m_spawnRelativePosition = _spawnRelativePosition;
            _m_spawnLogicPosition = _spawnLogicPosition;
        }

        
        public bool isCollected { get { return _m_isCollected; } }
        public Vector3 position { get { return _m_position; } }
        

        public override void init()
        {
        }
        public override void discard()
        {
        }
        public void tick(float _deltaTime)
        {
            if (_m_mono == null || _m_isCollected)
                return;
            
            // Check if reward is out of back stage bound
            if (_isOutOfBackBound())
            {
                _m_destroyFunc.Invoke(this);
                return;
            }
        }


        #region Relative Position
        Vector3 _IRelativePosUnit.position { get { return _m_position; } }
        Vector3 _IRelativePosUnit.relativePosition { set { if (_m_mono == null) return; _m_mono.transform.localPosition = value; } }
        #endregion

        
        protected override void _loadOp(Action _complete)
        {
            // Create reward mono using scene's createUnit function with relative position
            MainAdditionTreasureHuntGameTDScene.instance.createUnit<GTDMonoTreasureHuntReward>(_m_goIndex, _m_spawnRelativePosition, _mono =>
            {
                if (_mono == null)
                {
                    ALLog.Error($"TreasureHuntRewardUnit load failed, could not create mono with index {_m_goIndex}");
                    _complete.Invoke();
                    return;
                }

                _m_mono = _mono;

                // Initialize position and properties using logic position
                _m_position = _m_spawnLogicPosition;
                _m_isCollected = false;
                
                _complete.Invoke();
            });
        }
        protected override void _unloadOp()
        {
            if (_m_mono != null)
            {
                MainAdditionTreasureHuntGameTDScene.instance.discardUnit(_m_goIndex, _m_mono);
                _m_mono = null;
            }
        }

        
        private bool _isOutOfBackBound()
        {
            if (_m_mono == null)
                return true;
            
            GTDMonoTreasureHuntGame sceneMono = MainAdditionTreasureHuntGameTDScene.instance.getSceneMono();
            if (sceneMono == null)
                return false;

            if (sceneMono.transGameStageRangeBack == null)
                return false;

            float backZ = sceneMono.transGameStageRangeBack.localPosition.z;
            float rewardZ = _m_mono.transform.localPosition.z;

            // Check if reward is beyond back boundary (further backward)  
            return rewardZ < backZ;
        }
    }
}