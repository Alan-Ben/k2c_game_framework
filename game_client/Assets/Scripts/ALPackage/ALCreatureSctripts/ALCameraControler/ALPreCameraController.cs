using UnityEngine;
using System.Collections;

#if AL_CREATURE_SYS
namespace ALPackage
{
    /*********************
     * 摄像头控制对象
     * 对摄像头的移动等行为进行控制，暂时实现的是焦点为指定目标的位置移动处理
     **/
    public class ALPreCameraController : MonoBehaviour
    {
        private float _m_fMinCameraDis;
        private float _m_fSqrMinCameraDis;
        private float _m_fMaxCameraDis;
        private float _m_fSqrMaxCameraDis;
        /** 与目标之间的距离 */
        private Vector3 _m_vBetweenTargetDistance;
        private float _m_fRadius;

        /** 摄像头当前位置信息 */
        private Vector3 _m_vCameraCurPosition;
        /** 摄像头目标的位置信息 */
        private Vector3 _m_vTargetPosition;
        /** 摄像头当前的移动状态信息 */
        private Vector3 _m_vCameraTargetPosition;
        /** 最小移动速度 */
        private float _m_fMinSpeed;
        /** 移动行为的延迟时间 */
        private float _m_fDelayTime;
        /** 移动行为的前半部分加速度时间比例 */
        private float _m_fPreProcessTimeScale;
        /** 不需要加速度处理的距离 */
        private float _m_fNoAccSqrDis;

        /** 当前是否在移动 */
        private bool _m_bIsMoving;
        /** 当前摄像头的移动速度 */
        private Vector3 _m_vMoveSpeed;
        /** 移动的起始位置信息 */
        private Vector3 _m_vMovementStartPos;
        /** 移动的总位移信息 */
        private Vector3 _m_vTotalMovement;
        /** 摄像头移动的开始时间 */
        private float _m_fMoveStartTime;
        /** 摄像头移动的结束时间 */
        private float _m_fMoveEndTime;
        /** 前半段的加速时间 */
        private float _m_fPreProcessTime;
        /** 起始状态摄像头的移动速度 */
        private Vector3 _m_vInitMoveSpeed;
        /** 最小速度的向量值 */
        private Vector3 _m_vMinMoveSpeed;
        /** 前半段加速度 */
        private Vector3 _m_vPreHalfAccSpeed;
        /** 后半段加速度 */
        private Vector3 _m_vBckHalfAccSpeed;
        /** 后半段的起始速度 */
        private Vector3 _m_vBckHalfInitSpeed;

        /** 摄像头观测状态信息 */
        private _IALBasicCameraTargetState _m_tsTargetState = null;

        void Awake()
        {
            _m_fMinCameraDis = -1;
            _m_fSqrMinCameraDis = -1;
            _m_fMaxCameraDis = -1;
            _m_fSqrMaxCameraDis = -1;
        }

        // Use this for initialization
        void Start()
        {
            _m_vBetweenTargetDistance = new Vector3(-6f, 8f, -6f);
            _m_fRadius = _m_vBetweenTargetDistance.magnitude;

            _m_vCameraTargetPosition = new Vector3(0, 0, 0);
            _m_fMinSpeed = 6f;
            _m_fDelayTime = 0.3f;
            _m_fPreProcessTimeScale = 0.6f;
            _m_fNoAccSqrDis = _m_fMinSpeed * _m_fDelayTime * _m_fMinSpeed * _m_fDelayTime;

            _m_bIsMoving = false;
            _m_vMoveSpeed = new Vector3(0, 0, 0);
            _m_vMovementStartPos = new Vector3(0, 0, 0);
            _m_vTotalMovement = new Vector3(0, 0, 0);
            _m_fMoveStartTime = 0;
            _m_fMoveEndTime = 0;
            _m_vInitMoveSpeed = new Vector3(0, 0, 0);
            _m_vMinMoveSpeed = new Vector3(0, 0, 0);
            _m_vPreHalfAccSpeed = new Vector3(0, 0, 0);
            _m_vBckHalfAccSpeed = new Vector3(0, 0, 0);
            _m_vBckHalfInitSpeed = new Vector3(0, 0, 0);
        }

        // Update is called once per frame
        void Update()
        {
            //根据位移相关设置当前摄像头的位置
            if (_m_bIsMoving)
            {
                _calculateAndSetCurPosition();
            }

            //更新摄像头状态
            if (null != _m_tsTargetState)
            {
                _m_tsTargetState.updateCameraState(this.GetComponent<Camera>());
            }
        }

        /*****************
         * 设置摄像头目标对象
         **/
        public void setTargetState(_IALBasicCameraTargetState _targetState)
        {
            _m_tsTargetState = _targetState;
        }

        /************************
         * 设置与目标之间的间隔距离范围
         **/
        public void setMinBetweenTargetDistance(float _distance)
        {
            _m_fMinCameraDis = _distance;

            if (_m_fMinCameraDis < 0)
                _m_fSqrMinCameraDis = -1;
            else
                _m_fSqrMinCameraDis = _m_fMinCameraDis * _m_fMinCameraDis;
        }
        public void setMaxBetweenTargetDistance(float _distance)
        {
            _m_fMaxCameraDis = _distance;

            if (_m_fMaxCameraDis < 0)
                _m_fSqrMaxCameraDis = -1;
            else
                _m_fSqrMaxCameraDis = _m_fMaxCameraDis * _m_fMaxCameraDis;
        }

        /***************************
         * 摄像头旋转
         **/
        public void rotate(float _rotateX, float _rotateY)
        {
            //进行水平旋转计算，计算角度
            float angleY = _rotateX / 180;
            //绕Y轴旋转
            float x1 = (_m_vBetweenTargetDistance.x * Mathf.Cos(angleY)) + (_m_vBetweenTargetDistance.z * Mathf.Sin(angleY));
            float z1 = (-_m_vBetweenTargetDistance.x * Mathf.Sin(angleY)) + (_m_vBetweenTargetDistance.z * Mathf.Cos(angleY));

            //根据摄像头远近调整不同程度的高度
            float y1 = _m_vBetweenTargetDistance.y + (_rotateY * _m_fRadius / 100f);
            if (y1 > 20)
                y1 = 20f;
            else if (y1 < 1)
                y1 = 1;

            Vector3 newVector = new Vector3(x1, y1, z1);
            newVector = newVector.normalized * _m_fRadius;

            _m_vBetweenTargetDistance = newVector;

            //重新计算摄像头位置
            _refreshCameraPos();
        }

        /************************
         * 设置与目标之间的间隔距离
         **/
        public void setBetweenTargetDistanceScale(float _scale)
        {
            Vector3 targetDistance = _m_vBetweenTargetDistance * _scale;

            if (_m_fSqrMinCameraDis > 0 && targetDistance.sqrMagnitude < _m_fSqrMinCameraDis)
            {
                //设置为最短距离
                targetDistance = targetDistance.normalized * _m_fMinCameraDis;
            }
            else if (_m_fSqrMaxCameraDis > 0 && targetDistance.sqrMagnitude > _m_fSqrMaxCameraDis)
            {
                //设置为最长距离
                targetDistance = targetDistance.normalized * _m_fMaxCameraDis;
            }

            _m_vBetweenTargetDistance = targetDistance;
            _m_fRadius = _m_vBetweenTargetDistance.magnitude;

            //重新计算坐标相关参数
            setTargetPosition(_m_vTargetPosition);
        }

        /*******************
         * 设置摄像头的目标位置
         **/
        public void setTargetPosition(Vector3 _position)
        {
            _m_vCameraCurPosition = transform.position;
            _m_vTargetPosition = _position;
            //根据距离计算摄像头目标位置
            _m_vCameraTargetPosition = _m_vTargetPosition + _m_vBetweenTargetDistance;

            //根据最小速度计算距离最小平方值
            float minDis = _m_fMinSpeed * Time.deltaTime;
            float sqrDis = (_m_vCameraCurPosition - _m_vCameraTargetPosition).sqrMagnitude;
            //计算距离的平方值
            if (sqrDis > (minDis * minDis))
            {
                _m_bIsMoving = true;

                if (sqrDis < _m_fNoAccSqrDis)
                {
                    //不需要加速度的处理
                    float dis = Mathf.Sqrt(sqrDis);
                    //设置位移信息
                    _m_vMovementStartPos = _m_vCameraCurPosition;
                    _m_vTotalMovement = _m_vCameraTargetPosition - _m_vCameraCurPosition;
                    //设置移动的开始时间
                    _m_fMoveStartTime = Time.time - Time.deltaTime;
                    //设置移动的结束时间
                    _m_fMoveEndTime = Time.time - Time.deltaTime + (dis / _m_fMinSpeed);
                    //前半段加速时间长度
                    _m_fPreProcessTime = _m_fDelayTime * _m_fPreProcessTimeScale;
                    //设置开始移动时的移动速度
                    _m_vInitMoveSpeed = _m_vTotalMovement;
                    //计算起始速度
                    _m_vInitMoveSpeed.Normalize();
                    _m_vInitMoveSpeed = _m_vInitMoveSpeed * _m_fMinSpeed;
                    //最小移动速度
                    _m_vMinMoveSpeed = _m_vInitMoveSpeed;

                    //计算前后的加速度
                    _m_vPreHalfAccSpeed = new Vector3(0, 0, 0);

                    _m_vBckHalfAccSpeed = new Vector3(0, 0, 0);

                    //计算后半段的起始速度
                    _m_vBckHalfInitSpeed = _m_vInitMoveSpeed;
                }
                else
                {
                    //需要进行移动
                    //设置位移信息
                    _m_vMovementStartPos = _m_vCameraCurPosition;
                    _m_vTotalMovement = _m_vCameraTargetPosition - _m_vCameraCurPosition;
                    //设置移动的开始时间
                    _m_fMoveStartTime = Time.time - Time.deltaTime;
                    //设置移动的结束时间
                    _m_fMoveEndTime = Time.time - Time.deltaTime + _m_fDelayTime;
                    //前半段加速时间长度
                    _m_fPreProcessTime = _m_fDelayTime * _m_fPreProcessTimeScale;
                    //设置开始移动时的移动速度
                    _m_vInitMoveSpeed = _m_vMoveSpeed;

                    //设置最小速度对象
                    _m_vMinMoveSpeed = _m_vTotalMovement;
                    //计算起始速度
                    _m_vMinMoveSpeed.Normalize();
                    _m_vMinMoveSpeed = _m_vMinMoveSpeed * _m_fMinSpeed;

                    //计算前后的加速度
                    _m_vPreHalfAccSpeed =
                        new Vector3(
                            _calculatePreAccSpeed(_m_vTotalMovement.x, _m_fDelayTime, _m_fPreProcessTimeScale, _m_vInitMoveSpeed.x, _m_vMinMoveSpeed.x)
                            , _calculatePreAccSpeed(_m_vTotalMovement.y, _m_fDelayTime, _m_fPreProcessTimeScale, _m_vInitMoveSpeed.y, _m_vMinMoveSpeed.y)
                            , _calculatePreAccSpeed(_m_vTotalMovement.z, _m_fDelayTime, _m_fPreProcessTimeScale, _m_vInitMoveSpeed.z, _m_vMinMoveSpeed.z)
                            );

                    _m_vBckHalfAccSpeed =
                        new Vector3(
                            _calculateBckAccSpeed(_m_vPreHalfAccSpeed.x, _m_fDelayTime, _m_fPreProcessTimeScale, _m_vInitMoveSpeed.x, _m_vMinMoveSpeed.x)
                            , _calculateBckAccSpeed(_m_vPreHalfAccSpeed.y, _m_fDelayTime, _m_fPreProcessTimeScale, _m_vInitMoveSpeed.y, _m_vMinMoveSpeed.y)
                            , _calculateBckAccSpeed(_m_vPreHalfAccSpeed.z, _m_fDelayTime, _m_fPreProcessTimeScale, _m_vInitMoveSpeed.z, _m_vMinMoveSpeed.z)
                            );

                    //计算后半段的起始速度
                    _m_vBckHalfInitSpeed =
                        new Vector3(
                            _m_vInitMoveSpeed.x + (_m_vPreHalfAccSpeed.x * _m_fPreProcessTime)
                            , _m_vInitMoveSpeed.y + (_m_vPreHalfAccSpeed.y * _m_fPreProcessTime)
                            , _m_vInitMoveSpeed.z + (_m_vPreHalfAccSpeed.z * _m_fPreProcessTime)
                            );
                }

                //计算当前位置
                _calculateAndSetCurPosition();
            }
            else
            {
                transform.position = _m_vCameraTargetPosition;
            }
        }

        /****************
         * 计算前半段加速度
         **/
        protected float _calculatePreAccSpeed(float _distance, float _totalProcessTime, float _preProcessTimeScale, float _initSpeed, float _minSpeed)
        {
            //a0 = ((1 - d) * (v0 - vm) * t + (2 * s) - (2 * v0 * t)) / (d * t * t)
            float acc = (((1 - _preProcessTimeScale) * (_initSpeed - _minSpeed) * _totalProcessTime) + _distance + _distance - (2 * _initSpeed * _totalProcessTime))
                            / (_preProcessTimeScale * _totalProcessTime * _totalProcessTime);

            return acc;
        }
        /****************
         * 计算后半段加速度
         **/
        protected float _calculateBckAccSpeed(float _preAccSpeed, float _totalProcessTime, float _preProcessTimeScale, float _initSpeed, float _minSpeed)
        {
            //a1 = (vm - v0 - a0*d*t) / (1 - d) * t
            float acc = (_minSpeed - _initSpeed - (_preAccSpeed * _preProcessTimeScale * _totalProcessTime)) / ((1 - _preProcessTimeScale) * _totalProcessTime);
            //a1 = (2s - (2 * v0 * t) - (a0 * d * t * t * (2 - d)) / (t * t * (1 - d) * (1 - d))
            //float acc = (_distance + _distance - (2 * _initSpeed * _totalProcessTime) - (_preAccSpeed * _preProcessTimeScale * _totalProcessTime * _totalProcessTime * (2 - _preProcessTimeScale)))
            //                / (_totalProcessTime * _totalProcessTime * (1 - _preProcessTimeScale) * (1 - _preProcessTimeScale));

            return acc;
        }

        /*******************
         * 计算并设置当前本对象的位置
         **/
        protected void _calculateAndSetCurPosition()
        {
            if (Time.time >= _m_fMoveEndTime)
            {
                //到达结束时间，直接设置位置
                transform.position = _m_vCameraTargetPosition;
                _m_vMoveSpeed.Set(0, 0, 0);

                _m_bIsMoving = false;
            }
            else
            {
                //在移动过程，根据各参数计算当前位置
                float processTime = Time.time - _m_fMoveStartTime;

                Vector3 moveDis = new Vector3(0, 0, 0);
                float preProcessTime = processTime;
                if (preProcessTime >= _m_fPreProcessTime)
                {
                    preProcessTime = _m_fPreProcessTime;
                }

                //根据前半段时间计算当前位移
                moveDis.x += (_m_vInitMoveSpeed.x * preProcessTime + (0.5f * _m_vPreHalfAccSpeed.x * preProcessTime * preProcessTime));
                moveDis.y += (_m_vInitMoveSpeed.y * preProcessTime + (0.5f * _m_vPreHalfAccSpeed.y * preProcessTime * preProcessTime));
                moveDis.z += (_m_vInitMoveSpeed.z * preProcessTime + (0.5f * _m_vPreHalfAccSpeed.z * preProcessTime * preProcessTime));

                //计算后半程经过的时间
                float bakProcessTime = processTime - _m_fPreProcessTime;
                if (bakProcessTime > 0)
                {
                    //时间区域在后半程计算后半程位移
                    moveDis.x += _m_vBckHalfInitSpeed.x * bakProcessTime + (0.5f * _m_vBckHalfAccSpeed.x * bakProcessTime * bakProcessTime);
                    moveDis.y += _m_vBckHalfInitSpeed.y * bakProcessTime + (0.5f * _m_vBckHalfAccSpeed.y * bakProcessTime * bakProcessTime);
                    moveDis.z += _m_vBckHalfInitSpeed.z * bakProcessTime + (0.5f * _m_vBckHalfAccSpeed.z * bakProcessTime * bakProcessTime);

                    //计算当前速度
                    _m_vMoveSpeed.x = _m_vBckHalfInitSpeed.x + (_m_vBckHalfAccSpeed.x * preProcessTime);
                    _m_vMoveSpeed.y = _m_vBckHalfInitSpeed.y + (_m_vBckHalfAccSpeed.y * preProcessTime);
                    _m_vMoveSpeed.z = _m_vBckHalfInitSpeed.z + (_m_vBckHalfAccSpeed.z * preProcessTime);
                }
                else
                {
                    //没经过后半程直接用前半程速度计算
                    //计算当前速度
                    _m_vMoveSpeed.x = _m_vInitMoveSpeed.x + (_m_vPreHalfAccSpeed.x * preProcessTime);
                    _m_vMoveSpeed.y = _m_vInitMoveSpeed.y + (_m_vPreHalfAccSpeed.y * preProcessTime);
                    _m_vMoveSpeed.z = _m_vInitMoveSpeed.z + (_m_vPreHalfAccSpeed.z * preProcessTime);
                }

                //设置当前摄像头位置
                transform.position = _m_vMovementStartPos + moveDis;
            }
        }

        /*******************
         * 根据位置差值，重新计算摄像头位置
         **/
        protected void _refreshCameraPos()
        {
            //根据距离计算摄像头目标位置
            _m_vCameraTargetPosition = _m_vTargetPosition + _m_vBetweenTargetDistance;

            //根据最小速度计算距离最小平方值
            float minDis = _m_fMinSpeed * (Time.time - _m_fMoveStartTime);
            float sqrDis = (_m_vCameraCurPosition - _m_vCameraTargetPosition).sqrMagnitude;
            //计算距离的平方值
            if (sqrDis > (minDis * minDis))
            {
                _m_bIsMoving = true;

                if (sqrDis < _m_fNoAccSqrDis)
                {
                    //不需要加速度的处理
                    float dis = Mathf.Sqrt(sqrDis);
                    //设置位移信息
                    _m_vMovementStartPos = _m_vCameraCurPosition;
                    _m_vTotalMovement = _m_vCameraTargetPosition - _m_vCameraCurPosition;
                    //设置移动的结束时间
                    _m_fMoveEndTime = _m_fMoveStartTime + (dis / _m_fMinSpeed);
                    //前半段加速时间长度
                    _m_fPreProcessTime = _m_fDelayTime * _m_fPreProcessTimeScale;
                    //设置开始移动时的移动速度
                    _m_vInitMoveSpeed = _m_vTotalMovement;
                    //计算起始速度
                    _m_vInitMoveSpeed.Normalize();
                    _m_vInitMoveSpeed = _m_vInitMoveSpeed * _m_fMinSpeed;
                    //最小移动速度
                    _m_vMinMoveSpeed = _m_vInitMoveSpeed;

                    //计算前后的加速度
                    _m_vPreHalfAccSpeed = new Vector3(0, 0, 0);

                    _m_vBckHalfAccSpeed = new Vector3(0, 0, 0);

                    //计算后半段的起始速度
                    _m_vBckHalfInitSpeed = _m_vInitMoveSpeed;
                }
                else
                {
                    //需要进行移动
                    //设置位移信息
                    _m_vMovementStartPos = _m_vCameraCurPosition;
                    _m_vTotalMovement = _m_vCameraTargetPosition - _m_vCameraCurPosition;
                    //设置移动的结束时间
                    _m_fMoveEndTime = _m_fMoveStartTime + _m_fDelayTime;
                    //前半段加速时间长度
                    _m_fPreProcessTime = _m_fDelayTime * _m_fPreProcessTimeScale;
                    //设置开始移动时的移动速度
                    _m_vInitMoveSpeed = _m_vMoveSpeed;

                    //设置最小速度对象
                    _m_vMinMoveSpeed = _m_vTotalMovement;
                    //计算起始速度
                    _m_vMinMoveSpeed.Normalize();
                    _m_vMinMoveSpeed = _m_vMinMoveSpeed * _m_fMinSpeed;

                    //计算前后的加速度
                    _m_vPreHalfAccSpeed =
                        new Vector3(
                            _calculatePreAccSpeed(_m_vTotalMovement.x, _m_fDelayTime, _m_fPreProcessTimeScale, _m_vInitMoveSpeed.x, _m_vMinMoveSpeed.x)
                            , _calculatePreAccSpeed(_m_vTotalMovement.y, _m_fDelayTime, _m_fPreProcessTimeScale, _m_vInitMoveSpeed.y, _m_vMinMoveSpeed.y)
                            , _calculatePreAccSpeed(_m_vTotalMovement.z, _m_fDelayTime, _m_fPreProcessTimeScale, _m_vInitMoveSpeed.z, _m_vMinMoveSpeed.z)
                            );

                    _m_vBckHalfAccSpeed =
                        new Vector3(
                            _calculateBckAccSpeed(_m_vPreHalfAccSpeed.x, _m_fDelayTime, _m_fPreProcessTimeScale, _m_vInitMoveSpeed.x, _m_vMinMoveSpeed.x)
                            , _calculateBckAccSpeed(_m_vPreHalfAccSpeed.y, _m_fDelayTime, _m_fPreProcessTimeScale, _m_vInitMoveSpeed.y, _m_vMinMoveSpeed.y)
                            , _calculateBckAccSpeed(_m_vPreHalfAccSpeed.z, _m_fDelayTime, _m_fPreProcessTimeScale, _m_vInitMoveSpeed.z, _m_vMinMoveSpeed.z)
                            );

                    //计算后半段的起始速度
                    _m_vBckHalfInitSpeed =
                        new Vector3(
                            _m_vInitMoveSpeed.x + (_m_vPreHalfAccSpeed.x * _m_fPreProcessTime)
                            , _m_vInitMoveSpeed.y + (_m_vPreHalfAccSpeed.y * _m_fPreProcessTime)
                            , _m_vInitMoveSpeed.z + (_m_vPreHalfAccSpeed.z * _m_fPreProcessTime)
                            );
                }

                //计算当前位置
                _calculateAndSetCurPosition();
            }
            else
            {
                transform.position = _m_vCameraTargetPosition;
            }
        }
    }
}
#endif
