using System;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public interface _ISfxObj
    {
        /// <summary>
        /// 设置层级
        /// </summary>
        void setLayer(int _layer);
        
        /// <summary>
        /// 强制销毁特效
        /// </summary>
        void forceDiscard();
        
        /// <summary>
        /// 注册加载完成的回调
        /// </summary>
        void regLoadDoneDelegate(Action _delegate);
        
        /// <summary>
        /// 注册特效完全播放完成的回调，根据配表的duration来执行，小于0则不执行
        /// </summary>
        void regPlayCompleteDelegate(Action _delegate);
        
        /// <summary>
        /// 特效开始播放后，根据特效表配置trigger_event_time延迟时间触发的回调，trigger_event_time < 0 直接回调
        /// </summary>
        void regEventTriggerDelegate(Action _delegate);
    }
    
    //特效对象的基类
    public abstract class _BaseSfxObj<T> : _ISfxObj where T : NPSfxMono
    {
        private long _m_lSfxSerialize;
        private long _m_lSfxIndex;
        protected NPSfxRefObj _m_srSfxRef;

        //是否初始化
        private bool _m_isInit;
        
        /// <summary>
        /// 特效根节点对象，每个特效都会创建一个这个对象用于控制
        /// </summary>
        [NotNull]private GameObject _m_gSfxRootGo;
        /** 特效对象的控制脚本 */
        protected T _m_sfxMono;
        
        //展示的层级
        private int _m_iLayer;
        //父节点
        private Transform _m_tParent;

        //资源加载完成之后的回调管理器
        private ALCommonStateDelegate _m_loadDoneDelegate;
        //播放完成的回调
        private Action _m_playCompleteDelegate;
        //触发事件的回调
        private Action _m_eventTriggerDelegate;

        public void init([NotNull]NPSfxRefObj _refObj)
        {
            if(_m_isInit)
                return;
            _m_isInit = true;
            
            _m_lSfxSerialize = ALSerializeOpMgr.next();
            
            _m_srSfxRef = _refObj;
            _m_lSfxIndex = ALCommon.mergeInt(_m_srSfxRef.sfx_index.mainId, _m_srSfxRef.sfx_index.subId);

#if UNITY_EDITOR
            _m_gSfxRootGo = new GameObject($"sfx_{_m_srSfxRef.sfx_index.mainId}_{_m_srSfxRef.sfx_index.subId}|{_m_lSfxSerialize}");
#else
            _m_gSfxRootGo = new GameObject(_m_lSfxSerialize.ToString());
#endif
            _m_sfxMono = null;
            _m_tParent = null;

            _m_iLayer = -1;

            _m_loadDoneDelegate = new ALCommonStateDelegate();

            _onInit();
        }

        public long sfxSerialize { get { return _m_lSfxSerialize; } }
        public NPSfxRefObj sfxRef { get { return _m_srSfxRef; } }

        public GameObject sfxGo { get { return null == _m_gSfxRootGo? null : _m_gSfxRootGo.gameObject; } }
        public Transform sfxTrans { get { return null == _m_gSfxRootGo? null : _m_gSfxRootGo.transform; } }
        public T sfxMono { get { return _m_sfxMono; } }

        /// <summary>
        /// 设置特效父节点
        /// </summary>
        public void setParent(Transform _parent)
        {
            if (null == _parent || null == _m_gSfxRootGo || null == _m_gSfxRootGo.transform)
                return;

            _m_tParent = _parent;
            _m_gSfxRootGo.transform.SetParent(_parent);
        }

        /// <summary>
        /// 初始化本特效对应的特效具体对象
        /// </summary>
        public void playSfxObj()
        {
            if(!_m_isInit)
                return;
            
            if(null == _m_srSfxRef)
            {
                UnityEngine.Debug.LogError($"播放特效时特效配置为空，请检查");
                return;
            }
            
            //记录临时序列号
            long curSerialize = _m_lSfxSerialize;
            SfxCache.instance.popCacheGo(_m_srSfxRef
                , (_sfxMono) =>
                {
                    if (null == _sfxMono)
                        return;

                    //判断序列号，不一致则直接回收
                    if (curSerialize != _m_lSfxSerialize)
                    {
                        //回收
                        SfxCache.instance.AddToCache(_m_lSfxIndex, _sfxMono);
                        return;
                    }

                    _m_sfxMono = _sfxMono as T;
                    if(null == _m_sfxMono || null == _m_sfxMono.transform)
                        return;
                    
                    //设置父对象以及跟随情况
                    _m_sfxMono.setParent(_m_gSfxRootGo.transform);
                    _m_sfxMono.transform.localPosition = Vector3.zero;
                    _m_sfxMono.transform.localScale = Vector3.one;
                    _m_sfxMono.transform.localRotation = Quaternion.identity;

                    //设置有效
                    _m_sfxMono.setEnable();
                    
                    //再设置一次层级，在层级有效的情况下才设置
                    if (_m_iLayer >= 0)
                        setLayer(_m_iLayer);
                    
                    //再设置一次父节点，在有设置的情况下才设置
                    if(null != _m_tParent)
                        setParent(_m_tParent);
                    
                    _onLoadDonePlay();
                    
                    //调用回调
                    if(null != _m_loadDoneDelegate)
                    {
                        _m_loadDoneDelegate.setInitDone();
                    }
                });
            
            //根据是否有指定周期
            if(null != _m_srSfxRef && _m_srSfxRef.duration >= 0)
            {
                //延迟时间删除特效
                ALMonoTaskMgr.instance.addMonoTask(new SfxMonitorTask(this, () =>
                {
                    _m_playCompleteDelegate?.Invoke();
                }), _m_srSfxRef.duration / 1000f);
            }
            if(null != _m_srSfxRef && _m_srSfxRef.trigger_event_time >= 0)
            {
                //延迟时间触发回调
                ALCommonTaskController.CommonActionAddMonoTask(_m_eventTriggerDelegate, _m_srSfxRef.trigger_event_time / 1000f);
            }
            else 
            {
                //小于0直接触发
                if (null != _m_eventTriggerDelegate)
                {
                    _m_eventTriggerDelegate();
                }
            }
            
            //播放音效
            if (_m_srSfxRef != null && null != sfxTrans)
                PlayAudioMgr.instance.playOneShotClip(_m_srSfxRef.audio_id, null, sfxTrans.position);
        }
        

        //释放非定时类特效
        public void discardNoTiming()
        {
            //非定时的不做释放处理
            if (null == _m_srSfxRef || _m_srSfxRef.duration <= 0)
                return;

            //使用回收处理
            forceDiscard();
        }

        public void forceDiscard()
        {
            //修改序列号
            _m_lSfxSerialize = ALSerializeOpMgr.next();

            //放回缓存
            if (null != _m_sfxMono)
                SfxCache.instance.AddToCache(_m_lSfxIndex, _m_sfxMono);
            _m_sfxMono = null;
            
            //回收根对象
            ALUnityCommon.releaseGameObj(_m_gSfxRootGo);
            _m_gSfxRootGo = null;
            
            //释放回调
            if (null != _m_loadDoneDelegate)
                _m_loadDoneDelegate.reset();
            _m_loadDoneDelegate = null;
            
            _m_playCompleteDelegate = null;
            _m_eventTriggerDelegate = null;
            
            _onDiscard();
        }
        
        /**************
         * 设置层级
         **/
        public void setLayer(int _layer)
        {
            _m_iLayer = _layer;

            if (null == _m_sfxMono)
                return;

            _m_sfxMono.setLayer(_m_iLayer);
        }

        /*********************
         * 设置缩放倍率
         **/
        public void setLocalScale(Vector3 _scale)
        {
            if (null == _m_gSfxRootGo)
                return;

            _m_gSfxRootGo.transform.localScale = _scale;
        }

        /// <summary>
        /// 设置本地坐标
        /// </summary>
        /// <param name="_localPos"></param>
        public void setLocalPos(Vector3 _localPos)
        {
            if (null == _m_gSfxRootGo)
                return;

            _m_gSfxRootGo.transform.localPosition = _localPos;
        }

        /// <summary>
        /// 设置世界坐标
        /// </summary>
        /// <param name="_worldPos"></param>
        public void setWorldPos(Vector3 _worldPos)
        {
            if (null == _m_gSfxRootGo)
                return;

            _m_gSfxRootGo.transform.position = _worldPos;
        }
        
        public void setLocalRotation(Quaternion _pos)
        {
            if (null == _m_gSfxRootGo)
                return;

            _m_gSfxRootGo.transform.localRotation = _pos;
        }
		
        /// <summary>
        /// 检测UI的层级管理mono是否有添加，如没添加则根据规则自动添加
        /// </summary>
        public void checkUILayerMono()
        {
            if (null == _m_sfxMono)
                return;

            _m_sfxMono.checkUILayerMono();
        }
        
        
        /// <summary>
        /// 注册一个加载完成之后的回调
        /// </summary>
        /// <param name="_delegate"></param>
        public void regLoadDoneDelegate(Action _delegate)
        {
            if (_m_loadDoneDelegate != null) 
                _m_loadDoneDelegate.regDelegate(_delegate);
        }

        /// <summary>
        /// 注册特效完全播放完成的回调，根据配表的duration来执行，小于0则不执行
        /// </summary>
        public void regPlayCompleteDelegate(Action _delegate)
        {
            _m_playCompleteDelegate += _delegate;
        }

        /// <summary>
        /// 特效开始播放后，根据特效表配置trigger_event_time延迟时间触发的回调，trigger_event_time < 0 直接执行
        /// </summary>
        public void regEventTriggerDelegate(Action _delegate)
        {
            _m_eventTriggerDelegate += _delegate;
        }

        //初始化
        protected abstract void _onInit();

        //当加载完开始播放的时候
        protected abstract void _onLoadDonePlay();

        //当销毁的时候
        protected abstract void _onDiscard();
    }
}