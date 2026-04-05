using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using Spine.Unity;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GOE
{
    /// <summary>
    /// showcase单位对象
    /// </summary>
    public abstract class _AShowCaseUnitInfoObj
    {
        //序列号
        private long _m_serialize;

        //是否使用默认位置0,0,0
        private bool _m_useDefaultPos;

        //获取当前对象
        protected GameObject _m_controlUnit;
        //完成状态回调管理器
        [NotNull]protected ALCommonStateDelegate _m_initDoneDelegate;
        public Vector3 localPosition
        {
            get
            {
                if (null == _m_controlUnit)
                    return Vector3.zero;
                return _m_controlUnit.transform.localPosition;
            }
        }
        
        public Vector3 localEulerAngles
        {
            get
            {
                if (null == _m_controlUnit)
                    return Vector3.zero;
                return _m_controlUnit.transform.localEulerAngles;
            }
        }

        protected _AShowCaseUnitInfoObj()
        {
            _m_initDoneDelegate = new ALCommonStateDelegate();
        }
        
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_parent"></param>
        /// <param name="_initDoneAction">初始化加载完成</param>
        /// <param name="_showPrepareDone">表现准备完成</param>
        public void Init(Transform _parent, bool _useDefaultPos, Action _initDoneAction)
        {
            //已经初始化完了直接执行回调
            if (_m_initDoneDelegate.isInited)
            {
                if (null != _initDoneAction)
                    _initDoneAction();
            }

            //先回收一次
            Discard();

            long curSerialize = _m_serialize;
            _loadObj((loadGo) =>
            {
                //加载出来的对象是空的不处理
                if (null == loadGo)
                {
                    if (null != _initDoneAction)
                        _initDoneAction();
                    return;
                }

                //判断序列号，不一致则直接回收
                if (curSerialize != _m_serialize)
                {
                    //回收
                    Discard();
                    if (null != _initDoneAction)
                        _initDoneAction();
                    return;
                }

                _m_controlUnit = loadGo;
                _m_useDefaultPos = _useDefaultPos;

                _onInit(_parent);

                //直接执行加载完成回调
                if (null != _initDoneAction)
                    _initDoneAction();
                
                _m_initDoneDelegate.setInitDone();
            });
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Discard()
        {
            _m_serialize = ALSerializeOpMgr.next();
            _m_initDoneDelegate.reset();
            _m_useDefaultPos = true;

            // 销毁的时候需要重置材质状态，避免，下次从缓存中加载出来显示是黑的
            maskShowMaterial(false);
            darkMaterial(false);
            addColorAndIntensityMaterial(Color.black, 0);
            setDepthAlphaCutoff(0.97f);
            setAlpha(1);
            setZSpacing(0);

            _discardObj();

            _onDiscard();
        }

        /// <summary>
        /// 注册表现准备完成回调
        /// </summary>
        /// <param name="_showPrepareDone"></param>
        public void regShowPrepareDone(Action _showPrepareDone)
        {
            if(null == _showPrepareDone)
                return;
            
            //加载完成后再注册
            _m_initDoneDelegate.regDelegate(() =>
            {
                _regShowPrepareDone(_showPrepareDone);
            });
        }
        
        //当初始化
        protected virtual void _onInit(Transform _parent)
        {
            if (null == _m_controlUnit)
                return;

            if (_parent != null)
            {
                _m_controlUnit.transform.SetParent(null);
                _m_controlUnit.transform.SetParent(_parent);
                _m_controlUnit.transform.localScale = Vector3.one;

                _setDefaultPos();
            }

            ALPackage.ALUGUICommon.setGameObjEnable(_m_controlUnit, true);
        }

        //设置默认位置
        private void _setDefaultPos()
        {
            if (null == _m_controlUnit)
                return;

            Vector3 localPosition = Vector3.zero;
            Quaternion localRotation = Quaternion.identity;
            //是否需要额外处理角度位置微调
            if (_getShowCaseActorBehaviorKey > 0 && !_m_useDefaultPos)
            {
                NPShowCaseActorBehaviorRefObj showCaseActorBehaviorRefObj =
                    GRefdataCoreMgr.instance.showCaseActorBehaviorRefCore.getRef(_getShowCaseActorBehaviorKey);
                if (null != showCaseActorBehaviorRefObj)
                {
                    localPosition = showCaseActorBehaviorRefObj.local_position;
                    localRotation = Quaternion.Euler(showCaseActorBehaviorRefObj.local_rotation);
                }
            }

            //用默认位置0，0，0
            _m_controlUnit.transform.localPosition = localPosition;
            _m_controlUnit.transform.localRotation = localRotation;
        }

        //当销毁时候
        protected virtual void _onDiscard()
        {

        }

        /// <summary>
        /// 设置坐标
        /// </summary>
        /// <param name="_localPosition"></param>
        public void setLocalPosition(Vector3 _localPosition)
        {
            if (null == _m_controlUnit || null == _m_controlUnit.transform)
                return;

            _m_controlUnit.transform.localPosition = _localPosition;
        }
        
        /// <summary>
        /// 设置角度
        /// </summary>
        /// <param name="_localPosition"></param>
        public void setLocalEulerAngles(Vector3 _localVector3)
        {
            if (null == _m_controlUnit || null == _m_controlUnit.transform)
                return;

            _m_controlUnit.transform.localEulerAngles = _localVector3;
        }

        //开启未解锁效果
        public virtual void maskShowMaterial(bool _isGray)
        {
            if (null == _m_controlUnit)
                return;
            Renderer[] renderers = _m_controlUnit.GetComponentsInChildren<Renderer>();
            if (null == renderers)
                return;

            foreach (Renderer renderer in renderers)
            {
                if (null == renderer)
                    continue;

                SkeletonAnimation skeletonAnimation = renderer.GetComponent<SkeletonAnimation>();
                if (skeletonAnimation != null)
                {
                    foreach (AtlasAssetBase atlas in skeletonAnimation.skeletonDataAsset.atlasAssets)
                    {
                        if (atlas == null || atlas.Materials == null)
                            continue;
                        foreach (Material mat in atlas.Materials)
                        {
                            if (mat == null)
                                continue;
                            Material nMat = null;
                            if (!skeletonAnimation.CustomMaterialOverride.TryGetValue(mat, out nMat))
                            {
                                nMat = Object.Instantiate(mat);
                                skeletonAnimation.CustomMaterialOverride[mat] = nMat;
                            }
                            if (nMat != null)
                                ShaderPropertyMgr.openMaskShow(nMat, _isGray);

                            renderer.sharedMaterial = nMat;
                        }
                    }
                }
                else
                {
                    if (null == renderer.materials)
                        continue;
                    foreach (Material mat in renderer.materials)
                        ShaderPropertyMgr.openMaskShow(mat, _isGray);
                }
            }
        }

        //开启变暗效果
        public virtual void darkMaterial(bool _isDark)
        {
            if (null == _m_controlUnit)
                return;
            Renderer[] renderers = _m_controlUnit.GetComponentsInChildren<Renderer>();
            if (null == renderers)
                return;

            foreach (Renderer renderer in renderers)
            {
                if (null == renderer)
                    continue;

                SkeletonRenderer skeletonAnimation = renderer.GetComponent<SkeletonRenderer>();
                if (skeletonAnimation != null)
                {
                    foreach (AtlasAssetBase atlas in skeletonAnimation.skeletonDataAsset.atlasAssets)
                    {
                        if (atlas == null || atlas.Materials == null)
                            continue;
                        foreach (Material mat in atlas.Materials)
                        {
                            if (mat == null) 
                                continue;
                            Material nMat = null;
                            if(!skeletonAnimation.CustomMaterialOverride.TryGetValue(mat, out nMat))
                            {
                                nMat = Object.Instantiate(mat);
                                skeletonAnimation.CustomMaterialOverride[mat] = nMat;
                            }
                            if(nMat != null)
                                ShaderPropertyMgr.openDark(nMat, _isDark);

                            renderer.sharedMaterial = nMat;
                        }
                    }
                }
                else
                {
                    if (null == renderer.materials)
                        continue;
                    foreach (Material mat in renderer.materials)
                        ShaderPropertyMgr.openDark(mat, _isDark);
                }
            }

        }

        /// <summary>
        /// 设置叠加颜色及强度
        /// </summary>
        /// <param name="_color"></param>
        /// <param name="_value"></param>
        public virtual void addColorAndIntensityMaterial(Color _color, float _value)
        {
            if (null == _m_controlUnit)
                return;
            Renderer[] renderers = _m_controlUnit.GetComponentsInChildren<Renderer>();
            if (null == renderers)
                return;

            foreach (Renderer renderer in renderers)
            {
                if (null == renderer)
                    continue;

                SkeletonRenderer skeletonAnimation = renderer.GetComponent<SkeletonRenderer>();
                if (skeletonAnimation != null)
                {
                    foreach (AtlasAssetBase atlas in skeletonAnimation.skeletonDataAsset.atlasAssets)
                    {
                        if (atlas == null || atlas.Materials == null)
                            continue;
                        foreach (Material mat in atlas.Materials)
                        {
                            if (mat == null)
                                continue;
                            Material nMat = null;
                            if (!skeletonAnimation.CustomMaterialOverride.TryGetValue(mat, out nMat))
                            {
                                nMat = Object.Instantiate(mat);
                                skeletonAnimation.CustomMaterialOverride[mat] = nMat;
                            }
                            if (nMat != null)
                                ShaderPropertyMgr.setAddColorAndIntensity(nMat, _color, _value);

                            renderer.sharedMaterial = nMat;
                        }
                    }
                }
                else
                {
                    if (null == renderer.materials)
                        continue;

                    foreach (Material mat in renderer.materials)
                        ShaderPropertyMgr.setAddColorAndIntensity(mat, _color, _value);
                }
            }
        }

        /// <summary>
        /// 设置DepthAlphaCutoff
        /// </summary>
        /// <param name="_value"></param>
        public virtual void setDepthAlphaCutoff(float _value)
        {
            if (null == _m_controlUnit)
                return;
            Renderer[] renderers = _m_controlUnit.GetComponentsInChildren<Renderer>();
            if (null == renderers)
                return;

            foreach (Renderer renderer in renderers)
            {
                if (null == renderer)
                    continue;

                SkeletonRenderer skeletonAnimation = renderer.GetComponent<SkeletonRenderer>();
                if (skeletonAnimation != null)
                {
                    foreach (AtlasAssetBase atlas in skeletonAnimation.skeletonDataAsset.atlasAssets)
                    {
                        if (atlas == null || atlas.Materials == null)
                            continue;
                        foreach (Material mat in atlas.Materials)
                        {
                            if (mat == null)
                                continue;
                            Material nMat = null;
                            if (!skeletonAnimation.CustomMaterialOverride.TryGetValue(mat, out nMat))
                            {
                                nMat = Object.Instantiate(mat);
                                ShaderPropertyMgr.setDepthAlphaCutoff(nMat, _value);
                                skeletonAnimation.CustomMaterialOverride[mat] = nMat;
                            }
                            if (nMat != null)
                                ShaderPropertyMgr.setDepthAlphaCutoff(nMat, _value);

                            renderer.sharedMaterial = nMat;
                        }
                    }
                }
                else
                {
                    if (null == renderer.materials)
                        continue;

                    foreach (Material mat in renderer.materials)
                        ShaderPropertyMgr.setDepthAlphaCutoff(mat, _value);
                }
            }
        }

        public virtual void enableMagicaCloth(bool _isEnable)
        {
            Debug.LogError("子类未实现enableMagicaCloth");
        }

        //设置ZSpacing
        public virtual void setZSpacing(float _value)
        {
        }

        // 表现准备完成函数,默认都直接完成完成
        protected virtual void _regShowPrepareDone(Action _showPrepareDone)
        {
            if(null == _showPrepareDone)
                return;

            _showPrepareDone();
        }

        //获得表现微调表的唯一key，大于0有效
        protected abstract long _getShowCaseActorBehaviorKey { get; }
        
        //加载单位
        protected abstract void _loadObj(Action<GameObject> _doneAction);
        
        //释放单位
        protected abstract void _discardObj();

        //播放动画
        public abstract void playAnim(string _aniName);

        //设置动画触发器
        public abstract void setAnimTrigger(string _name);
        
        //设置动画速度
        public abstract void setSpeed(float _speed);

        //设置透明的
        public abstract void setAlpha(float _alpha);
        
        //播放特效
        public abstract void playSfx(long _sfxId);
        
        //旋转单位角度
        public abstract void rotateUnit(float _angle);

        //重置旋转角度
        public abstract void resetRotationUnit();
        
        //强制切换动画
        public abstract void forceSetAni(string _aniName);

        //显隐对象
        public abstract void enableRender(bool _isEnable);
        
        /// <summary>
        /// 是否正在播放动画
        /// </summary>
        /// <param name="_aniName"></param>
        /// <returns></returns>
        public abstract bool isPlayingAni(string _aniName);
    }
}