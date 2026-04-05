using System.Collections.Generic;
using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 收获的全局控制对象
    /// 根据类型可以注册和注销汇总焦点
    /// 每个汇总的效果都将根据具体类型获取对应的对象并进行处理
    /// </summary>
    public class GGUIHarvestCore
    {
        private static GGUIHarvestCore _g_instance = new GGUIHarvestCore();
        public static GGUIHarvestCore instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIHarvestCore();

                return _g_instance;
            }
        }

        //所有汇总目标的队列
        private List<_IHarvestTarget> _m_lAllTargetList;

        protected GGUIHarvestCore()
        {
            _m_lAllTargetList = new List<_IHarvestTarget>();
        }

        /// <summary>
        /// 注册和注销收获目标对象
        /// </summary>
        /// <param name="_target"></param>
        public void regHarvestTarget(_IHarvestTarget _target)
        {
            if (null == _target)
                return;

            _m_lAllTargetList.Add(_target);
        }
        public void unregHarvestTarget(_IHarvestTarget _target)
        {
            if (null == _target)
                return;

            _m_lAllTargetList.Remove(_target);
        }

        /// <summary>
        /// 获取注册目标对象
        /// </summary>
        /// <param name="_resType"></param>
        /// <returns></returns>
        public _IHarvestTarget getHarvestTarget(EHarvestType _resType)
        {
            _IHarvestTarget target = null;
            //倒序查找，确保优先找到最新的对象
            for(int i = _m_lAllTargetList.Count - 1; i >= 0; i--)
            {
                target = _m_lAllTargetList[i];
                if (null == target)
                    continue;

                if (target.harvestType == _resType)
                    return target;
            }

            return null;
        }

        /// <summary>
        /// 开启一个资源收集流程处理
        /// </summary>
        /// <param name="_resType">资源类型，用于与目标接收对象关联</param>
        /// <param name="_3dPos">对应场景中的3D位置，如果是UI则带入_srcUIObj</param>
        /// <param name="_particleCount">拆分粒子数量</param>
        /// <param name="_particleId">粒子样式Id</param>
        /// <param name="_singleParticleCount">单个粒子对应的数量，一般来说可以总值少于目标值，不要大于目标值</param>
        /// <param name="_onItemLoaded">粒子加载完成后的回调</param>
        /// <param name="_onAllParticleDone">当所有粒子完成后的回调</param>
        public void startHarvestCollection(EHarvestType _resType, Vector3 _3dPos, int _particleCount, long _particleId, long _singleParticleCount, Action<ParticleCacheItem> _onItemLoaded = null, Action _onAllParticleDone = null)
        {
            //将3D场景坐标转换为UI体系坐标
            Vector2 srcUIPos = GCommon.worldPos2UIPos(_3dPos);

            //使用内部函数执行具体操作
            _startHarvestCollection(_resType, srcUIPos, _particleCount, _particleId, _singleParticleCount, _onItemLoaded, _onAllParticleDone);
        }
        public void startHarvestCollection(EHarvestType _resType, Vector2 _uiPos, int _particleCount, long _particleId, long _singleParticleCount, Action<ParticleCacheItem> _onItemLoaded = null, Action _onAllParticleDone = null)
        {
            //使用内部函数执行具体操作
            _startHarvestCollection(_resType, _uiPos, _particleCount, _particleId, _singleParticleCount, _onItemLoaded, _onAllParticleDone);
        }
        public void startHarvestCollection(EHarvestType _resType, RectTransform _srcUIObj, int _particleCount, long _particleId, long _singleParticleCount, Action<ParticleCacheItem> _onItemLoaded = null, Action _onAllParticleDone = null)
        {
            //源头有问题则不触发
            if(null == _srcUIObj)
            {
                ALLog.Error("Harvest collection SrcUIObj is null!!");
                return;
            }

            //使用内部函数执行具体操作
            _startHarvestCollection(_resType, GCommon.getUIRootPos(_srcUIObj), _particleCount, _particleId, _singleParticleCount, _onItemLoaded, _onAllParticleDone);
        }
        protected void _startHarvestCollection(EHarvestType _resType, Vector2 _srcIUPos, int _particleCount, long _particleId, long _singleParticleCount, Action<ParticleCacheItem> _onItemLoaded = null, Action _onAllParticleDone = null)
        {
            //获取对应的收获目标对象
            _IHarvestTarget target = getHarvestTarget(_resType);
            Vector2 targetUIPos = Vector2.zero;
            if (null != target && null != target.targetUIObj)
            {
                targetUIPos = GCommon.getUIRootPos(target.targetUIObj);
            }

            if(null == target)
            {
#if UNITY_EDITOR
                ALLog.Sys($"can not find res type {_resType}，会默认找default");
#endif
                target = getHarvestTarget(EHarvestType.DEFAULT);

                if (null != target && null != target.targetUIObj)
                {
                    targetUIPos = GCommon.getUIRootPos(target.targetUIObj);
                }
            }

            //找不到任何目标则不处理
            if (null == target)
            {
                _onAllParticleDone?.Invoke();
                return;
            }

            //创建新操作序列号
            long newOpSerialize = ALSerializeOpMgr.next();
            //开启新的效果
            target.particleStart(newOpSerialize);

            //执行粒子效果
            UIParticleMgr.playParticleByUIPos(_particleId, _particleCount, _srcIUPos, targetUIPos,
                       null
                       , _onItemLoaded,
                       () =>
                       {
                           //完整结束的处理
                           if (null == target)
                               return;

                           //调用结束处理
                           target.particleComplete(newOpSerialize);

                           //执行所有完成之后的处理
                           if (null != _onAllParticleDone)
                               _onAllParticleDone();
                       }
                       , null
                       , () =>
                       {
                           if (null == target)
                               return;

                           //增加单个进度情况
                           target.particleItemDone(newOpSerialize, _singleParticleCount);
                       }
                );
        }
    }
}