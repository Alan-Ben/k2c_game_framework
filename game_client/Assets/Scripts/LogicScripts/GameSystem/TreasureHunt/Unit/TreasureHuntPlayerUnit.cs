using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public enum TreasureHuntPlayerSpeedType
    {
        ZERO,
        IDLE,
        BOOST,
        SEARCHING,
    }
    
    public class TreasureHuntPlayerUnit : _AGameLoadUnit, _ISphereCollisionUnit, _IRelativePosUnit, _IGameTickUnit
    {
        private const int MAX_REWARD_SFX_COUNT = 5;
        private static readonly int ANIMATOR_PARAM_BOOST_REMAIN_TIME = Animator.StringToHash("BoostRemainTime");
        private static readonly int ANIMATOR_PARAM_IS_BOOST = Animator.StringToHash("IsBoost");
        private static readonly int ANIMATOR_PARAM_IS_GAMING = Animator.StringToHash("IsGaming");
        private static readonly int ANIMATOR_TRIGGER_IDLE_START = Animator.StringToHash("IdleStart");
        private static readonly int ANIMATOR_TRIGGER_GAMING_START = Animator.StringToHash("GamingStart");
        private static readonly int ANIMATOR_TRIGGER_GAME_LOSE = Animator.StringToHash("GameLose");
        private static readonly int ANIMATOR_TRIGGER_GAME_WIN = Animator.StringToHash("GameWin");
        private static readonly int ANIMATOR_TRIGGER_DAMAGE_HIT = Animator.StringToHash("DamageHit");
        private static readonly int ANIMATOR_TRIGGER_REWARD_GET = Animator.StringToHash("RewardGet");
        
        [ItemNotNull, NotNull] private readonly List<CommonTDSfxObj> _m_rewardSfxList;
        
        private GTDMonoTreasureHuntPlayer _m_mono;
        private float _m_radius;
        private float _m_initPositionZ;
        
        private Vector3 _m_position;
        private float _m_targetPositionX;
        private bool _m_isHorizontalMoveComplete;
        
        private float _m_currentForwardSpeed;
        private float _m_targetForwardSpeed;
        private TreasureHuntPlayerSpeedType _m_speedType;
        private float _m_additionalSpeed;
        private float _m_currentLeanAngle;
        
        private float _m_collisionShakeTimer;
        private float _m_collisionShakeIntensity;
        private float _m_nextShakeDirection;
        
        private int _m_currentHealth;
        private int _m_maxHealth;
        private bool _m_bLastHaveShield;
        
        
        public TreasureHuntPlayerUnit([NotNull] _AGameLogic _gameLogic) 
            : base(_gameLogic)
        {
            _m_rewardSfxList = new List<CommonTDSfxObj>();
        }
        
        
        public float forwardOffset { get { return _m_position.z - _m_initPositionZ; } }
        public float currentForwardSpeed { get { return _m_currentForwardSpeed; } }
        public float targetForwardSpeed { get { return _m_targetForwardSpeed; } }
        public TreasureHuntPlayerSpeedType speedType { get { return _m_speedType; } }
        public float additionalSpeed { get { return _m_additionalSpeed; } }
        public float currentLeanAngle { get { return _m_currentLeanAngle; } }
        public bool isCollisionShaking { get { return _m_collisionShakeTimer > 0f; } }
        public int currentHealth { get { return _m_currentHealth; } }
        public int maxHealth { get { return _m_maxHealth; } }
        public bool isDead { get { return _m_currentHealth <= 0; } }
        public Vector3 position { get { return _m_position; } }
        public float worldPositionY
        {
            get
            {
                if (_m_mono == null) return 0;

                return _m_mono.transform.position.y;
            }
        }


        public override void init()
        {
        }
        public override void discard()
        {
            _clearAllRewardSfx();
        }
        public void tick(float _deltaTime)
        {
            if (_m_mono == null)
                return;

#if UNITY_EDITOR || PLATFORM_STANDALONE
            float xMovement = 0;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                xMovement -= _m_mono.keyboardHorizontalMovement;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                xMovement += _m_mono.keyboardHorizontalMovement;
            if (xMovement != 0)
            {
                _m_targetPositionX += xMovement * _deltaTime;
                _m_isHorizontalMoveComplete = false;
            }
#endif
            
            _updateForwardMovement(_deltaTime);
            _updateLeanRotation(_deltaTime);
            _updateHorizontalMovement(_deltaTime);
            _updateTransform(_deltaTime);
        }
        
        
        public void setMoveTargetHorizontal(float _horizontalValue)
        {
            if (_m_mono == null)
                return;
            
            _m_targetPositionX = _horizontalValue;
            _m_isHorizontalMoveComplete = false;
        }
        public void resetHorizontalToDefault(bool _immediately = false)
        {
            if (_m_mono == null)
                return;
                
            _m_targetPositionX = MainAdditionTreasureHuntGameTDScene.instance.getDefaultPlayerPositionX();
            _m_isHorizontalMoveComplete = false;
            
            if (_immediately)
            {
                _m_position.x = _m_targetPositionX;
                _m_isHorizontalMoveComplete = true;
            }
        }
        public void setToZeroSpeed(bool _immediately = false)
        {
            if (_m_mono == null)
                return;

            _m_speedType = TreasureHuntPlayerSpeedType.ZERO;
            _updateTargetSpeed();

            if (_immediately)
                _m_currentForwardSpeed = _m_targetForwardSpeed;
        }
        public void setToIdleSpeed(bool _immediately = false)
        {
            if (_m_mono == null)
                return;
                
            _m_speedType = TreasureHuntPlayerSpeedType.IDLE;
            _updateTargetSpeed();
            
            if (_immediately)
                _m_currentForwardSpeed = _m_targetForwardSpeed;
        }
        public void setToBoostSpeed(bool _immediately = false)
        {
            if (_m_mono == null)
                return;
                
            _m_speedType = TreasureHuntPlayerSpeedType.BOOST;
            _updateTargetSpeed();
            
            if (_immediately)
                _m_currentForwardSpeed = _m_targetForwardSpeed;
        }
        public void setToSearchingSpeed(bool _immediately = false)
        {
            if (_m_mono == null)
                return;
                
            _m_speedType = TreasureHuntPlayerSpeedType.SEARCHING;
            _updateTargetSpeed();
            
            if (_immediately)
                _m_currentForwardSpeed = _m_targetForwardSpeed;
        }
        public void setAdditionalSpeed(float _additionalSpeed, bool _immediately = false)
        {
            _m_additionalSpeed = _additionalSpeed;
            _updateTargetSpeed();
            
            if (_immediately)
                _m_currentForwardSpeed = _m_targetForwardSpeed;
        }
        public void addAdditionalSpeed(float _speedDelta, bool _immediately = false)
        {
            _m_additionalSpeed += _speedDelta;
            _updateTargetSpeed();
            
            if (_immediately)
                _m_currentForwardSpeed = _m_targetForwardSpeed;
        }
        public void setHealthCount(int _healthCount)
        {
            _m_maxHealth = _healthCount;
            _m_currentHealth = _healthCount;
            _m_bLastHaveShield = _m_currentHealth > 1;
            refreshShieldState();
        }
        public void takeDamage(int _damage = 1)
        {
            _m_currentHealth = Mathf.Max(0, _m_currentHealth - _damage);
            refreshShieldState();
        }
        public void heal(int _healAmount)
        {
            _m_currentHealth = Mathf.Min(_m_maxHealth, _m_currentHealth + _healAmount);
            refreshShieldState();
        }
        public void refreshShieldState()
        {
            if (_m_mono == null)
                return;

            bool hasShield = _m_currentHealth > 1;

            //护盾消失，播放音效
            if (_m_bLastHaveShield && !hasShield)
            {
                if (_m_mono.shieldDamagedAudioId > 0)
                    PlayAudioMgr.instance.playClip(_m_mono.shieldDamagedAudioId);
            }
            _m_bLastHaveShield = hasShield;
            _m_mono.setHaveShield(hasShield);
        }
        public void showGetRewardEffect()
        {
            if (_m_mono == null)
                return;
            
            _playSfx(_m_mono.rewardGetSfxId);
            animTriggerRewardGet();
        }
        public void animSetBoostRemainTime(float _remainingTime)
        {
            if (_m_mono == null || _m_mono.animator == null)
                return;
                
            _m_mono.animator.SetFloat(ANIMATOR_PARAM_BOOST_REMAIN_TIME, _remainingTime);
        }
        public void animTriggerIdleStart()
        {
            if (_m_mono == null || _m_mono.animator == null)
                return;

            _m_mono.animator.SetTrigger(ANIMATOR_TRIGGER_IDLE_START);
        }
        public void animSetBoostState(bool _boost)
        {
            if (_m_mono == null || _m_mono.animator == null)
                return;

            _m_mono.animator.SetBool(ANIMATOR_PARAM_IS_BOOST, _boost);
        }
        public void animTriggerGamingStart()
        {
            if (_m_mono == null || _m_mono.animator == null)
                return;

            _m_mono.animator.SetTrigger(ANIMATOR_TRIGGER_GAMING_START);
        }
        public void animSetGamingState(bool _gaming)
        {
            if (_m_mono == null || _m_mono.animator == null)
                return;
            
            _m_mono.animator.SetBool(ANIMATOR_PARAM_IS_GAMING, _gaming);
        }
        public void animTriggerGameLose()
        {
            if (_m_mono == null || _m_mono.animator == null)
                return;

            _m_mono.animator.SetTrigger(ANIMATOR_TRIGGER_GAME_LOSE);
        }
        public void animTriggerGameWin()
        {
            if (_m_mono == null || _m_mono.animator == null)
                return;

            _m_mono.animator.SetTrigger(ANIMATOR_TRIGGER_GAME_WIN);
        }
        public void animTriggerDamageHit()
        {
            if (_m_mono == null || _m_mono.animator == null)
                return;

            _m_mono.animator.SetTrigger(ANIMATOR_TRIGGER_DAMAGE_HIT);
        }
        public void animTriggerRewardGet()
        {
            if (_m_mono == null || _m_mono.animator == null)
                return;

            _m_mono.animator.SetTrigger(ANIMATOR_TRIGGER_REWARD_GET);
        }
        public void resetAllTrigger()
        {
            if (_m_mono == null || _m_mono.animator == null)
                return;
            
            _m_mono.animator.ResetTrigger(ANIMATOR_TRIGGER_REWARD_GET);
            _m_mono.animator.ResetTrigger(ANIMATOR_TRIGGER_DAMAGE_HIT);
            _m_mono.animator.ResetTrigger(ANIMATOR_TRIGGER_GAME_WIN);
            _m_mono.animator.ResetTrigger(ANIMATOR_TRIGGER_GAME_LOSE);
            _m_mono.animator.ResetTrigger(ANIMATOR_TRIGGER_GAMING_START);
            _m_mono.animator.ResetTrigger(ANIMATOR_TRIGGER_IDLE_START);
        }
        public void playBoostSound()
        {
            if (_m_mono == null || _m_mono.boostAudioId <= 0)
                return;

            PlayAudioMgr.instance.playClip(_m_mono.boostAudioId);
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
            if (_m_mono == null)
                return;
            
            _playSfx(_m_mono.damageHitSfxId);
            _triggerCollisionShake();
            takeDamage(1);
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
            _m_mono = MainAdditionTreasureHuntGameTDScene.instance.getPlayerMono();
            if (_m_mono == null)
            {
                _complete.Invoke();
                return;
            }

            _m_position = _m_mono.transform.localPosition;
            _m_targetPositionX = _m_position.x;
            _m_initPositionZ = _m_position.z;
            _m_radius = _m_mono.radius;
            _m_isHorizontalMoveComplete = true;
            
            _m_currentForwardSpeed = 0f;
            _m_speedType = TreasureHuntPlayerSpeedType.IDLE;
            _m_additionalSpeed = 0f;
            _updateTargetSpeed();
            _m_currentLeanAngle = 0f;
            _m_collisionShakeTimer = 0f;
            _m_collisionShakeIntensity = 0f;
            _m_nextShakeDirection = 0f;
            _m_currentHealth = 0;
            _m_maxHealth = 0;
            _complete.Invoke();
        }
        protected override void _unloadOp()
        {
            _m_mono = null;
        }
        
        
        private void _updateTargetSpeed()
        {
            if (_m_mono == null)
                return;
                
            float baseSpeed = 0f;
            switch (_m_speedType)
            {
                case TreasureHuntPlayerSpeedType.ZERO:
                    baseSpeed = 0f;
                    break;
                case TreasureHuntPlayerSpeedType.IDLE:
                    baseSpeed = _m_mono.idleMoveSpeedForward;
                    break;
                case TreasureHuntPlayerSpeedType.BOOST:
                    baseSpeed = _m_mono.boostMoveSpeedForward;
                    break;
                case TreasureHuntPlayerSpeedType.SEARCHING:
                    baseSpeed = _m_mono.searchingMoveSpeedForward;
                    break;
            }
            
            _m_targetForwardSpeed = baseSpeed + _m_additionalSpeed;
        }
        
        private void _updateForwardMovement(float _deltaTime)
        {
            // Accelerate forward speed towards target speed
            _m_currentForwardSpeed = Mathf.MoveTowards(
                _m_currentForwardSpeed, 
                _m_targetForwardSpeed, 
                _m_mono.accelerationForward * _deltaTime
            );

            // Apply forward movement
            _m_position += Vector3.forward * _m_currentForwardSpeed * _deltaTime;
        }
        private void _updateHorizontalMovement(float _deltaTime)
        {
            if (_m_isHorizontalMoveComplete)
                return;

            float currentX = _m_position.x;
            float deltaX = _m_targetPositionX - currentX;
            float moveX = Mathf.Sign(deltaX) * _m_mono.moveSpeedHorizontal * _deltaTime;
            
            // Check if we've reached or passed the target
            if (Mathf.Abs(moveX) >= Mathf.Abs(deltaX))
            {
                _m_position.x = _m_targetPositionX;
                _m_isHorizontalMoveComplete = true;
            }
            else
            {
                _m_position.x += moveX;
            }
        }
        private void _updateLeanRotation(float _deltaTime)
        {
            // Calculate target lean angle based on horizontal movement direction
            float targetLeanAngle = 0f;
            if (!_m_isHorizontalMoveComplete)
            {
                float deltaX = _m_targetPositionX - _m_position.x;
                if (deltaX != 0)
                {
                    float normalizedDirection = Mathf.Sign(deltaX);
                    targetLeanAngle = normalizedDirection * -_m_mono.moveLeanAngle;
                }
            }
            
            // Smoothly interpolate to target lean angle using dedicated lean speed
            _m_currentLeanAngle = Mathf.MoveTowards(_m_currentLeanAngle, targetLeanAngle, _m_mono.leanSpeed * _deltaTime);
        }
        private float _updateCollisionShake(float _deltaTime)
        {
            if (_m_collisionShakeTimer <= 0f)
                return 0f;

            // Update shake timer
            _m_collisionShakeTimer -= _deltaTime;
            
            // Calculate shake intensity fade over time
            float normalizedTime = 1f - (_m_collisionShakeTimer / _m_mono.collisionLeanTime);
            float fadeIntensity = Mathf.Lerp(_m_collisionShakeIntensity, 0f, normalizedTime);
            
            // Generate shake angle using random oscillation
            // Change direction multiple times per second based on frequency
            float timeSinceStart = _m_mono.collisionLeanTime - _m_collisionShakeTimer;
            float directionChangeInterval = 1f / _m_mono.collisionShakeFrequency;
            
            // Check if we should change direction
            if (Mathf.FloorToInt(timeSinceStart / directionChangeInterval) != Mathf.FloorToInt((timeSinceStart - _deltaTime) / directionChangeInterval))
                _m_nextShakeDirection = UnityEngine.Random.Range(-1f, 1f);
            
            float shakeAngle = _m_nextShakeDirection * fadeIntensity;
            
            return shakeAngle;
        }
        private void _triggerCollisionShake()
        {
            if (_m_mono == null)
                return;

            _m_collisionShakeTimer = _m_mono.collisionLeanTime;
            _m_collisionShakeIntensity = _m_mono.collisionLeanIntensity;
            _m_nextShakeDirection = UnityEngine.Random.Range(-1f, 1f); // Start with random direction
            animTriggerDamageHit();
        }
        private void _updateTransform(float _deltaTime)
        {
            if (_m_mono == null)
                return;

            // The position will handle by relative position.
            // _m_mono.transform.localPosition = _m_position;
            
            // Update rotation with lean
            Vector3 currentEuler = _m_mono.transform.localEulerAngles;
            _m_mono.transform.localRotation = Quaternion.Euler(currentEuler.x, currentEuler.y, _m_currentLeanAngle + _updateCollisionShake(_deltaTime));
        }
        private void _playSfx(long _sfxId)
        {
            if (_m_mono == null || _m_mono.sfxParent == null || _sfxId <= 0)
                return;

            CommonTDSfxObj sfxObj = PlaySfxMgr.instance.playTDSfx(_sfxId, _m_mono.sfxParent);
            if (sfxObj != null)
            {
                // Add to list and manage count
                _m_rewardSfxList.Add(sfxObj);

                // If exceeded maximum count, remove and discard the oldest one
                if (_m_rewardSfxList.Count > MAX_REWARD_SFX_COUNT)
                {
                    CommonTDSfxObj oldestSfx = _m_rewardSfxList[0];
                    _m_rewardSfxList.RemoveAt(0);
                    oldestSfx.forceDiscard();
                }
            }
        }
        private void _clearAllRewardSfx()
        {
            foreach (CommonTDSfxObj sfxObj in _m_rewardSfxList)
            {
                sfxObj?.forceDiscard();
            }
            _m_rewardSfxList.Clear();
        }
    }
}