
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ALPackage;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

namespace GOE
{
    public static class UnityExtensions
    {
        #region Text

        public static float getHeight(this Text _text, string _value)
        {
            if (_text == null)
                return 0;

            TextGenerator tg = _text.cachedTextGeneratorForLayout;
            TextGenerationSettings ts = _text.GetGenerationSettings(_text.rectTransform.rect.size);
            return tg.GetPreferredHeight(_value, ts);
        }

        public static float getWidth(this Text _text, string _value)
        {
            if (_text == null)
                return 0;

            TextGenerator tg = _text.cachedTextGeneratorForLayout;
            TextGenerationSettings ts = _text.GetGenerationSettings(_text.rectTransform.rect.size);
            return tg.GetPreferredWidth(_value, ts);
        }

        #endregion

        #region Vector3

        public static string toDetailString(this Vector3 _v)
        {
            return $"({_v.x}, {_v.y}, {_v.z})";
        }

        public static bool IsNaN(this Vector3 _v)
        {
            return float.IsNaN(_v.x) || float.IsNaN(_v.y) || float.IsNaN(_v.z);
        }
        
        /// <summary>
        /// 简单的辅助方法，返回一个新的Vector，无副作用，用来减少代码行数
        /// </summary>
        /// <example>
        /// 例如
        /// Vector3 vec = transform.localPosition;
        /// vec.y = height;
        /// transform.localPosition = vec;
        /// 可以写成
        /// transform.localPosition = transform.localPosition.SetY(height);
        /// 
        /// 例如
        /// var dis = mob.transform.position - pos;
        /// dis.y = 0;
        /// float sqrDis = dis.sqrMagnitude;
        /// 可以写成
        /// float sqrDis = (mob.transform.position - pos).SetY(0).sqrMagnitude;
        /// </example>
        public static Vector3 SetX(this Vector3 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        public static Vector3 SetY(this Vector3 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        public static Vector3 SetZ(this Vector3 vector, float z)
        {
            vector.z = z;
            return vector;
        }
        
        
        
        #endregion

        #region Vector2

        public static string toDetailString(this Vector2 _v)
        {
            return $"({_v.x}, {_v.y})";
        }

        public static Vector2 SetX(this Vector2 vector, float x)
        {
            vector.x = x;
            return vector;
        }

        public static Vector2 SetY(this Vector2 vector, float y)
        {
            vector.y = y;
            return vector;
        }

        #endregion

        #region Transform

        /// <summary>
        /// 从一个 Transform 的所有子对象重找一个 Transform
        /// </summary>
        public static Transform findTransform(this Transform _root, Predicate<Transform> _predicate,
            bool ignoreSelf = true)
        {
            if (_root == null || _predicate == null)
                return null;

            if (!ignoreSelf && _predicate(_root))
                return _root;

            Queue<Transform> recursiveInsteadList = new Queue<Transform>(20);
            recursiveInsteadList.Enqueue(_root);
            while (recursiveInsteadList.Count > 0)
            {
                Transform parent = recursiveInsteadList.Dequeue();
                for (int i = 0; i < parent.childCount; i++)
                {
                    Transform childTrans = parent.GetChild(i);
                    if (_predicate(childTrans))
                        return childTrans;
                    recursiveInsteadList.Enqueue(childTrans);
                }
            }

            return null;
        }

        public static void doSomethingForAllChildren(this Transform _root, Action<Transform> _action,
            bool ignoreSelf = true)
        {
            if (_root == null || _action == null)
                return;

            if (!ignoreSelf) _action(_root);

            Queue<Transform> recursiveInsteadList = new Queue<Transform>(20);
            recursiveInsteadList.Enqueue(_root);
            while (recursiveInsteadList.Count > 0)
            {
                Transform parent = recursiveInsteadList.Dequeue();
                for (int i = 0; i < parent.childCount; i++)
                {
                    Transform childTrans = parent.GetChild(i);
                    _action(childTrans);
                    recursiveInsteadList.Enqueue(childTrans);
                }
            }
        }

        /// <summary>
        /// 把本地坐标转换为另外一个 Transform 的本地坐标
        /// </summary>
        public static Vector3 transformPointToAnotherTransform(this Transform _transform, Transform _targetTransform,
            Vector3 _localPosition)
        {
            if (_transform == null || _targetTransform == null)
                return _localPosition;

            Vector4 homoPos = _targetTransform.worldToLocalMatrix * _transform.localToWorldMatrix *
                              new Vector4(_localPosition.x, _localPosition.y, _localPosition.z, 1);
            return homoPos / homoPos.w;
        }

        #endregion

        #region RectTransform

        /// <summary>
        /// 把本地的 rectPosition 转换成本地坐标
        /// </summary>
        /// <remarks>
        /// 这里的 rectPosition 指的是 RectTransform 圈出的一个方形范围，左下角为 0，0，右上角为 1，1
        /// </remarks>
        public static Vector3 rectPositionToLocalPosition(this RectTransform _transform, Vector2 _rectPosition)
        {
            if (_transform == null)
                return Vector3.zero;

            Vector3 localScale = _transform.localScale;
            Rect localRect = _transform.rect;
            Vector2 size = new Vector2(
                localScale.x == 0 ? 0 : localRect.size.x / localScale.x,
                localScale.y == 0 ? 0 : localRect.size.y / localScale.y);

            Vector2 positionToCorner = _rectPosition * size;
            Vector2 pivotToCorner = _transform.pivot * size;
            return positionToCorner - pivotToCorner;
        }

        public static Rect getRectInParent(this RectTransform _rectTransform)
        {
            if (_rectTransform == null)
                return Rect.zero;

            if (_rectTransform.parent == null)
            {
                Rect worldRect = _rectTransform.rect;
                worldRect.center += (Vector2)_rectTransform.position;
                return worldRect;
            }

            Rect localRect = _rectTransform.rect;
            localRect.center += (Vector2)_rectTransform.localPosition;
            localRect.size = Vector2.Scale((Vector2)_rectTransform.localScale, localRect.size);
            return localRect;
        }

        /// <summary>
        /// 把改位置挪到目标位置下一个
        /// </summary>
        /// <param name="_rectTransform"></param>
        /// <param name="_targetRectTransform"></param>
        public static void SetSiblingNext(this RectTransform _rectTransform, RectTransform _targetRectTransform)
        {
            if (null == _targetRectTransform)
                return;

            _rectTransform.SetSiblingIndex(_targetRectTransform.GetSiblingIndex() + 1);
        }

        /// <summary>
        /// 获取_rectTransform在世界坐标中的Rect
        /// </summary>
        /// <param name="_rectTransform"></param>
        /// <returns></returns>
        public static Rect getWroldRect(this RectTransform _rectTransform)
        {
            if(_rectTransform == null)
                return Rect.zero;
            
            Vector3[] corners = new Vector3[4];
            _rectTransform.GetWorldCorners(corners);
            
            Vector3 bottomLeft = corners[0];
            Vector3 topRight = corners[2];
            
            return new Rect(bottomLeft.x, bottomLeft.y, topRight.x - bottomLeft.x, topRight.y - bottomLeft.y);
        }
        
        /// <summary>
        /// 判断两个RectTransform是否相交或重合
        /// </summary>
        /// <param name="_rectTransform"></param>
        /// <param name="_targetRectTransform"></param>
        /// <returns></returns>
        public static bool Overlaps(this RectTransform _rectTransform, RectTransform _targetRectTransform)
        {
            if (_rectTransform == null || _targetRectTransform == null)
                return false;

            Rect rect1 = _rectTransform.getWroldRect();
            Rect rect2 = _targetRectTransform.getWroldRect();
            return rect1.Overlaps(rect2);
        }
        
        #endregion

        #region ScrollRect

        /// <summary>
        /// 聚焦到 Content 里的某个本地坐标
        /// </summary>
        public static void focusToContentLocalPos(this ScrollRect _scrollRect, Vector3 _contentPos)
        {
            if (_scrollRect == null || _scrollRect.viewport == null || _scrollRect.content == null)
                return;

            Vector3 targetPos = _scrollRect.content.transformPointToAnotherTransform(_scrollRect.viewport, _contentPos);
            Vector3 currentPos = _scrollRect.viewport.rectPositionToLocalPosition(new Vector2(0.5f, 0.5f));
            _scrollRect.content.localPosition += currentPos - targetPos;
        }

        #endregion

        #region Quaternion

        public static Quaternion plus(this Quaternion _a, Quaternion _b)
        {
            return new Quaternion(_a.x + _b.x, _a.y + _b.y, _a.z + _b.z, _a.w + _b.w);
        }

        public static Quaternion minus(this Quaternion _a, Quaternion _b)
        {
            return new Quaternion(_a.x - _b.x, _a.y - _b.y, _a.z - _b.z, _a.w - _b.w);
        }

        #endregion

        #region Matrix4x4

                public static Matrix4x4 plus(this Matrix4x4 _a, Matrix4x4 _b)
        {
            Matrix4x4 result = Matrix4x4.zero;
            result[0, 0] = _a[0, 0] + _b[0, 0];
            result[0, 1] = _a[0, 1] + _b[0, 1];
            result[0, 2] = _a[0, 2] + _b[0, 2];
            result[0, 3] = _a[0, 3] + _b[0, 3];
            result[1, 0] = _a[1, 0] + _b[1, 0];
            result[1, 1] = _a[1, 1] + _b[1, 1];
            result[1, 2] = _a[1, 2] + _b[1, 2];
            result[1, 3] = _a[1, 3] + _b[1, 3];
            result[2, 0] = _a[2, 0] + _b[2, 0];
            result[2, 1] = _a[2, 1] + _b[2, 1];
            result[2, 2] = _a[2, 2] + _b[2, 2];
            result[2, 3] = _a[2, 3] + _b[2, 3];
            result[3, 0] = _a[3, 0] + _b[3, 0];
            result[3, 1] = _a[3, 1] + _b[3, 1];
            result[3, 2] = _a[3, 2] + _b[3, 2];
            result[3, 3] = _a[3, 3] + _b[3, 3];
            return result;
        }

        public static Matrix4x4 minus(this Matrix4x4 _a, Matrix4x4 _b)
        {
            Matrix4x4 result = Matrix4x4.zero;
            result[0, 0] = _a[0, 0] - _b[0, 0];
            result[0, 1] = _a[0, 1] - _b[0, 1];
            result[0, 2] = _a[0, 2] - _b[0, 2];
            result[0, 3] = _a[0, 3] - _b[0, 3];
            result[1, 0] = _a[1, 0] - _b[1, 0];
            result[1, 1] = _a[1, 1] - _b[1, 1];
            result[1, 2] = _a[1, 2] - _b[1, 2];
            result[1, 3] = _a[1, 3] - _b[1, 3];
            result[2, 0] = _a[2, 0] - _b[2, 0];
            result[2, 1] = _a[2, 1] - _b[2, 1];
            result[2, 2] = _a[2, 2] - _b[2, 2];
            result[2, 3] = _a[2, 3] - _b[2, 3];
            result[3, 0] = _a[3, 0] - _b[3, 0];
            result[3, 1] = _a[3, 1] - _b[3, 1];
            result[3, 2] = _a[3, 2] - _b[3, 2];
            result[3, 3] = _a[3, 3] - _b[3, 3];
            return result;
        }

        public static Matrix4x4 multiply(this Matrix4x4 _a, float _value)
        {
            Matrix4x4 result = Matrix4x4.zero;
            result[0, 0] = _a[0, 0] * _value;
            result[0, 1] = _a[0, 1] * _value;
            result[0, 2] = _a[0, 2] * _value;
            result[0, 3] = _a[0, 3] * _value;
            result[1, 0] = _a[1, 0] * _value;
            result[1, 1] = _a[1, 1] * _value;
            result[1, 2] = _a[1, 2] * _value;
            result[1, 3] = _a[1, 3] * _value;
            result[2, 0] = _a[2, 0] * _value;
            result[2, 1] = _a[2, 1] * _value;
            result[2, 2] = _a[2, 2] * _value;
            result[2, 3] = _a[2, 3] * _value;
            result[3, 0] = _a[3, 0] * _value;
            result[3, 1] = _a[3, 1] * _value;
            result[3, 2] = _a[3, 2] * _value;
            result[3, 3] = _a[3, 3] * _value;
            return result;
        }

        #endregion
        
        #region Animation

        //采样
        public static void Sample(this Animation _anim, string _animName, float _normalizeTime)
        {
            if (_anim == null)
                return;

            AnimationState state = _anim[_animName];
            if (state == null)
                return;

            state.enabled = true;
            state.weight = 1f;
            state.normalizedTime = _normalizeTime;
            _anim.Sample();
            state.enabled = false;
            state.weight = 0f;
        }

        //强制重头播放
        public static void ForcePlay(this Animation _anim, string _animName, float _normalizedTime = 0f, Action _onCompleteTimeout = null)
        {
            if (_anim == null)
            {
                _onCompleteTimeout?.Invoke();
                return;
            }

            //解决重置动画以后播放不了的bug
            _anim.enabled = false;
            _anim.enabled = true;

            AnimationState state = _anim[_animName];
            if (state == null)
            {
                _onCompleteTimeout?.Invoke();
                return;
            }

            state.normalizedTime = _normalizedTime;

            _anim.Play(_animName, PlayMode.StopAll);
            if (_onCompleteTimeout != null)
            {
                if(null != state)
                {
                    ALCommonTaskController.CommonActionAddMonoTask(_onCompleteTimeout, state.clip.length);
                }
                else
                {
                    _onCompleteTimeout?.Invoke();
                }
            }
        }

        //动画播放
        public static void Play(this Animation _anim, string _animationName, Action _onCompleteTimeout)
        {
            if (_anim == null)
            {
                _onCompleteTimeout?.Invoke();
                return;
            }

            //解决重置动画以后播放不了的bug
            _anim.enabled = false;
            _anim.enabled = true;

            AnimationClip clip = _anim.GetClip(_animationName);
            if (clip == null)
            {
                _onCompleteTimeout?.Invoke();
                return;
            }

            _anim.Play(_animationName);
            if (_onCompleteTimeout != null)
                ALCommonTaskController.CommonActionAddMonoTask(_onCompleteTimeout, clip.length);
        }

        //获取动画时常
        public static float GetAnimationLength(this Animation _anim, string _animName)
        {
            if (_anim == null)
                return 0;

            AnimationClip clip = _anim.GetClip(_animName);
            if (clip == null)
                return 0;
            else
                return clip.length;
        }

        //设置动画播放速度
        public static void SetAnimSpeed(this Animation _anim, string _animName, float _speed)
        {
            if (_anim == null)
                return;

            if (!string.IsNullOrEmpty(_animName))
            {
                AnimationState state = _anim[_animName];
                if (state != null)
                    state.speed = _speed;
            }
        }

        //重制动画到0
        public static void ResetAnim(this Animation _anim, string _animName)
        {
            if (_anim == null)
                return;

            if (!string.IsNullOrEmpty(_animName))
            {
                AnimationState state = _anim[_animName];
                if (state == null)
                    return;

                _anim.Play(_animName);
                state.time = 0;
                state.enabled = true;
                _anim.Sample(_animName, 0);
                state.enabled = false;
            }
        }

        /// <summary>
        /// 主要用于播放不受timeScale影响的animation
        /// </summary>
        /// <param name="animation"></param>
        /// <param name="clipName"></param>
        /// <param name="useTimeScale"></param>
        /// <param name="onComplete"></param>
        /// <returns></returns>
        public static IEnumerator Play(this Animation animation, string clipName, bool useTimeScale, Action onComplete)
        {
            //We Don't want to use timeScale, so we have to animate by frame..
            if (!useTimeScale)
            {
                AnimationState _currState = animation[clipName];
                bool isPlaying = true;
                float _startTime = 0F;
                float _progressTime = 0F;
                float _timeAtLastFrame = 0F;
                float _timeAtCurrentFrame = 0F;
                float deltaTime = 0F;


                animation.Play(clipName);

                _timeAtLastFrame = Time.realtimeSinceStartup;
                while (isPlaying)
                {
                    _timeAtCurrentFrame = Time.realtimeSinceStartup;
                    deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                    _timeAtLastFrame = _timeAtCurrentFrame;

                    _progressTime += deltaTime;
                    if (_currState.length > 0)
                        _currState.normalizedTime = _progressTime / _currState.length;
                    animation.Sample();

                    //Debug.Log(_progressTime);

                    if (_progressTime >= _currState.length)
                    {
                        //Debug.Log(&quot;Bam! Done animating&quot;);
                        if (_currState.wrapMode != WrapMode.Loop)
                        {
                            //Debug.Log(&quot;Animation is not a loop anim, kill it.&quot;);
                            //_currState.enabled = false;
                            isPlaying = false;
                        }
                        else
                        {
                            //Debug.Log(&quot;Loop anim, continue.&quot;);
                            _progressTime = 0.0f;
                        }
                    }

                    yield return new WaitForEndOfFrame();
                }

                yield return null;
                if (onComplete != null)
                {
                    onComplete();
                }
            }
            else
            {
                animation.Play(clipName);
            }
        }

        #endregion


        #region Animator
        public static void Play(this Animator _anim, string _aniName, Action _onPlayDone)
        {
            if (_anim == null || _anim.runtimeAnimatorController == null || string.IsNullOrEmpty(_aniName))
            {
                _onPlayDone?.Invoke();
                return;
            }

            AnimationClip[] animationClips = _anim.runtimeAnimatorController.animationClips;
            if (animationClips == null)
            {
                _onPlayDone?.Invoke();
                return;
            }

            AnimationClip targetClip = null;
            foreach (AnimationClip clip in animationClips)
            {
                if (clip != null && !string.IsNullOrEmpty(clip.name) && clip.name.Equals(_aniName))
                {
                    targetClip = clip;
                    break;
                }
            }

            if (targetClip == null)
            {
                _onPlayDone?.Invoke();
                return;
            }

            _anim.Play(_aniName);

            if (_onPlayDone != null)
                ALCommonTaskController.CommonActionAddMonoTask(_onPlayDone, targetClip.length);
        }
        #endregion
        
        #region Bounds

        /// <summary>
        /// 获取8个顶点坐标
        /// </summary>
        /// <param name="_bounds"></param>
        /// <returns></returns>
        public static Vector3[] getBoundsCorners(this Bounds _bounds)
        {
            Vector3 center = _bounds.center;
            Vector3 extents = _bounds.extents;

            Vector3[] corners = new Vector3[8];

            corners[0] = center + new Vector3(-extents.x, -extents.y, -extents.z);
            corners[1] = center + new Vector3(extents.x, -extents.y, -extents.z);
            corners[2] = center + new Vector3(-extents.x, extents.y, -extents.z);
            corners[3] = center + new Vector3(extents.x, extents.y, -extents.z);
            corners[4] = center + new Vector3(-extents.x, -extents.y, extents.z);
            corners[5] = center + new Vector3(extents.x, -extents.y, extents.z);
            corners[6] = center + new Vector3(-extents.x, extents.y, extents.z);
            corners[7] = center + new Vector3(extents.x, extents.y, extents.z);

            return corners;
        }

        #endregion

        #region Material

        private static Dictionary<Material, Material> _m_dMaterials = new Dictionary<Material, Material>();
        /// <summary>
        /// 直接使用此材质，如果是编辑器下为了避免影响到本地资源，会实例一个唯一材质。运行时则直接使用
        /// </summary>
        /// <param name="_mat"></param>
        /// <returns></returns>
        public static Material SourceMaterial(this Material _mat)
        {
#if UNITY_EDITOR
            if (_mat == null)
                return null;
            Material mat;
            if(_m_dMaterials == null)
                _m_dMaterials = new Dictionary<Material, Material>();
            if (!_m_dMaterials.TryGetValue(_mat, out mat))
            {
                _m_dMaterials.Add(_mat, mat = UnityEngine.Object.Instantiate(_mat));
            }
            return mat;
#else
            return _mat;
#endif
        }
        #endregion
    }
    
    public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int _capacity) : base(_capacity) {}

        public AnimationClip this[string _name]
        {
            get { return this.Find(_x => _x.Key.name.Equals(_name)).Value; }
            set
            {
                int index = this.FindIndex(_x =>
                {
                    if (_x.Key == null || string.IsNullOrEmpty(_x.Key.name))
                        return false;

                    return _x.Key.name.Equals(_name);
                });
                if (index != -1)
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
            }
        }
    }
}