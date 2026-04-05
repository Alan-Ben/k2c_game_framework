using System;
using System.Collections.Generic;
#if NP_GAME
using ALPackage;
using DG.Tweening;
#endif
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOE
{
    /// <summary>
    /// 用于从起点到终点，是从曲线列表中随机一条路线来移动
    /// </summary>
    public class GSfxMovePathMono : MonoBehaviour
    {
        [ALHeader("起点")]
        public Transform startTrans;
        [ALHeader("终点")]
        public Transform targetTrans;
        [ALHeader("曲线列表")]
        public List<AnimationCurve> curveList;
        [ALHeader("曲线最大偏移值")]
        public float curveOffSetMax;
        [ALHeader("移动时长")]
        public float moveTime = 1f;
        [ALHeader("延迟移动的最小时间")]
        public float delayMinTime = 0f;
        [ALHeader("延迟移动的最大时间")]
        public float delayMaxTime = 0f;
#if NP_GAME
        private Tweener _m_moveTweener;
        private Vector3 _m_startPos;
        private Vector3 _m_targetPos;
        public event Action onMoveComplete;

        /// <summary>
        /// 开始移动
        /// </summary>
        /// <param name="_startTrans"></param>
        /// <param name="_targetTrans"></param>
        /// <param name="_moveDone"></param>
        public void startMove(Transform _startTrans, Transform _targetTrans, Action _moveDone = null)
        {
            this.gameObject.SetActive(false);
            startTrans = _startTrans;
            targetTrans = _targetTrans;
            startMove(_moveDone);
        }
        
        /// <summary>
        /// 开始移动
        /// </summary>
        /// <param name="_startTrans"></param>
        /// <param name="_targetTrans"></param>
        /// <param name="_moveDone"></param>
        public void startMove(Action _moveDone = null)
        {
            this.gameObject.SetActive(false);
            onMoveComplete = _moveDone;
            float delayTime = delayMinTime + (delayMaxTime - delayMinTime) * Random.value;
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                this.gameObject.SetActive(true);
            }, delayTime);
        }

        private void OnEnable()
        {
            _m_moveTweener?.Kill();
            if (null != startTrans && null != targetTrans)
            {
                _m_startPos = startTrans.position;
                _m_targetPos = targetTrans.position;
                Vector3 moveDir = _m_targetPos - _m_startPos;
                float distance = moveDir.magnitude;
                Vector3 offSetDir = Vector3.zero;
                Vector3 offSetDir2 = Vector3.zero;
                Vector3 offSetVector = Vector3.zero;
                Vector3.OrthoNormalize(ref moveDir, ref offSetDir,ref offSetDir2);
                // Debug.DrawLine(_m_startPos,_m_targetPos,Color.red, 10f);
                // Debug.DrawLine(_m_startPos,_m_startPos + offSetDir,Color.blue, 10f);
                // Debug.DrawLine(_m_startPos,_m_startPos + offSetDir2,Color.green, 10f);
                float startRate = 0;
                float curRate = 0;
                float endRate = 1;
                transform.position = _m_startPos;
                AnimationCurve curve = curveList.GetRandomItem();
                _m_moveTweener = DOTween.To(_value => curRate = _value, startRate, endRate, moveTime)
                    .OnUpdate(() =>
                    {
                        transform.position = _m_startPos + moveDir * distance * curRate;
                        if (null != curve)
                        {
                            float offSet = curve.Evaluate(curRate);
                            offSetVector = offSetDir * offSet * curveOffSetMax;
                            // Debug.DrawLine(transform.position,transform.position + offSetVector,Color.blue, 1f);
                        }
                        transform.position += offSetVector;
                    })
                    .OnComplete(() =>
                    {
                        onMoveComplete?.Invoke();
                        onMoveComplete = null;
                    })
                    .SetAutoKill(true);
            }
        }

        // private void OnDisable()
        // {
        //     _m_moveTweener?.Kill();
        // }
        
#endif
    }
}