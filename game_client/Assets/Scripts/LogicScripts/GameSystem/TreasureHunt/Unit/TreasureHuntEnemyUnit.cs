using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class TreasureHuntEnemyUnit : _AGameLoadUnit, _IRelativePosUnit, _ISphereCollisionUnit, _IGameTickUnit
    {
        private readonly NPGGoIndex _m_goIndex;
        [NotNull] private readonly Action<TreasureHuntEnemyUnit> _m_destroyFunc;
        private readonly Vector3 _m_spawnRelativePosition;
        private readonly Vector3 _m_spawnLogicPosition;
        
        private GTDMonoTreasureHuntEnemy _m_mono;
        private float _m_radius;

        private Vector3 _m_position;
        private Vector3 _m_moveDirection;
        private float _m_moveSpeed;
        
        
        public TreasureHuntEnemyUnit([NotNull] _AGameLogic _gameLogic, NPGGoIndex _goIndex, [NotNull] Action<TreasureHuntEnemyUnit> _destroyFunc, Vector3 _spawnRelativePosition, Vector3 _spawnLogicPosition) 
            : base(_gameLogic)
        {
            _m_goIndex = _goIndex;
            _m_destroyFunc = _destroyFunc;
            _m_spawnRelativePosition = _spawnRelativePosition;
            _m_spawnLogicPosition = _spawnLogicPosition;
        }

        
        public override void init()
        {
        }
        public override void discard()
        {
        }
        public void tick(float _deltaTime)
        {
            if (_m_mono == null)
                return;

            // Move in the chosen direction
            _m_position += _m_moveDirection * _m_moveSpeed * _deltaTime;
            
            // Check if enemy is out of stage bounds
            if (_isOutOfStageBounds())
            {
                _m_destroyFunc.Invoke(this);
                return;
            }
            
            // Update transform position (handled by relative position system)
            // _m_mono.transform.localPosition = _m_position;
        }



        #region Relative Position
        Vector3 _IRelativePosUnit.position { get { return _m_position; } }
        Vector3 _IRelativePosUnit.relativePosition { set { if (_m_mono == null) return; _m_mono.transform.localPosition = value; } }
        #endregion

        #region Collision
        float _ISphereCollisionUnit.radius { get { return _m_radius; } }
        Vector3 _ICollisionUnit.position { get { return _m_position; } }
        bool _ICollisionUnit.isEnabled { get { return true; } }
        void _ICollisionUnit.onCollisionStart(_ICollisionUnit _other)
        {
            if (_other is TreasureHuntPlayerUnit)
            {
                if (_m_mono != null && _m_mono.sfxTransform != null)
                    PlaySfxMgr.instance.playSfxByPos(_m_mono.hitSfxId, _m_mono.sfxTransform.position);
                _m_destroyFunc.Invoke(this);
            }
        }
        void _ICollisionUnit.onCollision(_ICollisionUnit _other)
        {
        }
        void _ICollisionUnit.onCollisionEnd(_ICollisionUnit _other)
        {
        }
        #endregion

        
        protected override void _loadOp(Action _complete)
        {
            // Create enemy mono using scene's createUnit function with relative position
            MainAdditionTreasureHuntGameTDScene.instance.createUnit<GTDMonoTreasureHuntEnemy>(_m_goIndex, _m_spawnRelativePosition, _mono =>
            {
                if (_mono == null)
                {
                    ALLog.Error($"TreasureHuntEnemyUnit load failed, could not create mono with index {_m_goIndex}");
                    _complete.Invoke();
                    return;
                }

                _m_mono = _mono;

                // Initialize position and properties using logic position
                _m_position = _m_spawnLogicPosition;
                _m_radius = _m_mono.radius;
                
                // Choose random speed from range
                _m_moveSpeed = _m_mono.randomSpeedRange.getRandomValue();
                
                // Choose random direction from list
                _chooseRandomDirection();
                
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

        
        private void _chooseRandomDirection()
        {
            if (_m_mono == null || _m_mono.randomMoveDirectionList == null || _m_mono.randomMoveDirectionList.Count == 0)
            {
                // Default to forward movement if no directions specified
                _m_moveDirection = Vector3.forward;
                return;
            }

            // Choose random target from list
            int randomIndex = UnityEngine.Random.Range(0, _m_mono.randomMoveDirectionList.Count);
            Transform targetTransform = _m_mono.randomMoveDirectionList[randomIndex];
            
            if (targetTransform == null)
            {
                _m_moveDirection = Vector3.forward;
                return;
            }

            // Calculate direction to target
            Vector3 targetDirection = (targetTransform.position - _m_mono.transform.position).normalized;
            _m_moveDirection = targetDirection;
        }

        private bool _isOutOfStageBounds()
        {
            if (_m_mono == null)
                return true;
            
            GTDMonoTreasureHuntGame sceneMono = MainAdditionTreasureHuntGameTDScene.instance.getSceneMono();
            if (sceneMono == null)
                return false;

            if (sceneMono.transGameStageRangeFront == null || sceneMono.transGameStageRangeBack == null)
                return false;

            float frontZ = sceneMono.transGameStageRangeFront.localPosition.z;
            float backZ = sceneMono.transGameStageRangeBack.localPosition.z;
            float enemyZ = _m_mono.transform.localPosition.z;

            // Check if enemy is beyond front boundary (further forward)
            if (enemyZ > frontZ)
                return true;

            // Check if enemy is beyond back boundary (further backward)  
            if (enemyZ < backZ)
                return true;

            return false;
        }
    }
}