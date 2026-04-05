using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GOE
{
    /// <summary>
    /// 粒子组信息
    /// </summary>
    public class BurstParticleGroupInfo
    {
        private bool _m_bIsEnable;//该粒子组是否有效
        private Particle_Cache _m_cParticleCache;//管理该粒子组的缓存池

        private int _m_iTotalCount;//总的粒子数量

        //父节点Go对象，创建Group的时候创建对象
        private GameObject _m_gParentGo;
        private RectTransform _m_tParentTrans;

        private Vector2 _m_vStartPos;//起始位置
        private Vector2 _m_vEndPos;//终点位置
        private NPParticleRefObj _m_particleRefObj;//配置信息
        private Action<ParticleCacheItem> _m_aOnItemLoadDone;//粒子加载完成回调
        private Action _m_aOnBurstDone;//每个粒子炸裂结束事件
        private Action _m_aOnFlyDone;//每个粒子上飘结束事件
        private Action<BurstParticleGroupInfo> _m_aOnGroupDone;//粒子组表现结束事件
        private Action _m_aDoneAction;//完成事件

        private ALCommonEnableTaskController _m_tParicleTickTask;//粒子运动任务

        private List<BurstParticleObj> _m_lActiveParticleList;//所有本集合的运动粒子对象
        private float _m_fPraticleStartTimeS;//粒子集合开始处理的时间戳，用于对每个粒子进行tick处理

        private bool _m_bIsPlayBurstAudio;//是否播放炸裂音效
        private bool _m_bIsPlayFlyAudio;//是否播放飞行音效

        /// <summary> 粒子组父节点 </summary>
        protected internal RectTransform _groupRootTrans { get { return _m_tParentTrans; } }

        /// <summary>
        /// 粒子组信息
        /// </summary>
        /// <param name="_parentTrans">父节点</param>
        /// <param name="_particleRefObj">粒子配置</param>
        /// <param name="_particleCache">粒子缓存池</param>
        /// <param name="_particleNum">粒子数量</param>
        /// <param name="_startPos">开始位置</param>
        /// <param name="_endPos">目标位置</param>
        /// <param name="_onItemLoadDone">粒子加载完成回调</param>
        /// <param name="_doneAction">表现完成回调</param>
        /// <param name="_onBurstDone">每个粒子炸裂完成回调</param>
        /// <param name="_onFlyDone">每个粒子飞行完成回调</param>
        /// <param name="_onGroupDone">该粒子组表现结束回调</param>
        public BurstParticleGroupInfo(
            Transform _parentTrans,
            NPParticleRefObj _particleRefObj,
            Particle_Cache _particleCache,
            int _particleNum,
            Vector2 _startPos,
            Vector2 _endPos,
            Action<ParticleCacheItem> _onItemLoadDone,
            Action _doneAction,
            Action _onBurstDone,
            Action _onFlyDone,
            Action<BurstParticleGroupInfo> _onGroupDone)
        {
            //创建父节点Go
            _m_gParentGo = new GameObject();
            if (null != _m_gParentGo.transform)
            {
                _m_gParentGo.name = $"part_{_particleRefObj.id}";
                //设置父节点
                _m_gParentGo.transform.SetParent(_parentTrans);
                _m_tParentTrans = _m_gParentGo.AddMissingComponent<RectTransform>();

                //重置属性
                _m_tParentTrans.localPosition = Vector3.zero;
                _m_tParentTrans.localScale = Vector3.one;
            }

            _m_bIsEnable = true;
            _m_particleRefObj = _particleRefObj;
            _m_cParticleCache = _particleCache;
            _m_vStartPos = _startPos;
            _m_vEndPos = _endPos;
            _m_iTotalCount = _particleNum;
            _m_aOnItemLoadDone = _onItemLoadDone;
            _m_aDoneAction = _doneAction;
            _m_aOnBurstDone = _onBurstDone;
            _m_aOnFlyDone = _onFlyDone;
            _m_aOnGroupDone = _onGroupDone;
            _m_bIsPlayBurstAudio = false;
            _m_bIsPlayFlyAudio = false;

            _m_lActiveParticleList = new List<BurstParticleObj>();

            //初始化粒子
            _initParticle();

            //开启粒子运动tick
            _initParitcleTickTask();
        }

        


        #region 管理

        /// <summary>
        /// 销毁
        /// </summary>
        public void discard()
        {
            //回收所有粒子
            _pushBackAllItem();

            //执行结束事件
            _onGroupDone();
            _m_bIsEnable = false;
        }

        #endregion


        #region 表现事件

        /// <summary>
        /// 单个粒子开始炸裂
        /// </summary>
        protected internal void _onStartBurst()
        {
            //播放粒子开始炸开音效
            if (!_m_bIsPlayBurstAudio)
            {
                _m_bIsPlayBurstAudio = true;
                if (_m_particleRefObj != null && _m_particleRefObj.burst_audio_id > 0)
                    PlayAudioMgr.instance.playClip(_m_particleRefObj.burst_audio_id);
            }
        }

        /// <summary>
        /// 单个粒子炸裂完成
        /// </summary>
        /// <param name="_particleMono"></param>
        protected internal void _onBurstDone()
        {
            _m_aOnBurstDone?.Invoke();

            //播放粒子开始飞行音效
            if (!_m_bIsPlayFlyAudio)
            {
                _m_bIsPlayFlyAudio = true;
                if (_m_particleRefObj != null && _m_particleRefObj.fly_audio_id > 0)
                    PlayAudioMgr.instance.playClip(_m_particleRefObj.fly_audio_id);
            }
        }

        /// <summary>
        /// 单个粒子飞行完成
        /// </summary>
        /// <param name="_particleMono"></param>
        protected internal void _onFlyDone()
        {
            _m_aOnFlyDone?.Invoke();

            //播放每个粒子完成收集音效
            if (_m_particleRefObj != null && _m_particleRefObj.each_done_audio_id > 0)
                PlayAudioMgr.instance.playClip(_m_particleRefObj.each_done_audio_id);
        }

        /// <summary>
        /// 粒子组表现完成
        /// </summary>
        private void _onGroupDone()
        {
            _m_aDoneAction?.Invoke();
            _m_aOnGroupDone?.Invoke(this);

            _m_aOnBurstDone = null;
            _m_aOnFlyDone = null;
            _m_aDoneAction = null;
            _m_aOnGroupDone = null;
            _m_bIsEnable = false;
            _m_bIsPlayBurstAudio = false;
            _m_bIsPlayFlyAudio = false;

            //停止粒子运动任务
            _discardParticleTickTask();
            _m_lActiveParticleList?.Clear();

            ALUnityCommon.releaseGameObj(_m_gParentGo);
        }

        #endregion


        #region 粒子管理

        /// <summary>
        /// 取出一个粒子item
        /// </summary>
        /// <returns></returns>
        protected internal ParticleCacheItem _popParticleItem()
        {
            if (null == _m_cParticleCache)
                return null;

            ParticleCacheItem item = _m_cParticleCache.popItem();
            item?.initItem(_groupRootTrans);
            item?.setDefaultIcon(_m_particleRefObj.icon_index);

            if (_m_aOnItemLoadDone != null)
                _m_aOnItemLoadDone(item);

            return item;
        }

        /// <summary>
        /// 回收粒子item
        /// </summary>
        /// <param name="_item"></param>
        protected internal void _pushBackCacheItem(ParticleCacheItem _item)
        {
            if (null == _m_cParticleCache)
                return;

            _m_cParticleCache.pushBackCacheItem(_item);
        }

        /// <summary>
        /// 初始化粒子
        /// </summary>
        private void _initParticle()
        {
            //生成粒子
            for (int i = 0; i < _m_iTotalCount; i++)
            {
                //随机生成粒子坐标参数
                float angle = Random.Range(0.0f, 360.0f);//位置为0 - 360度的随机一个角度  
                float rad = angle / 180 * Mathf.PI;//角度变换成弧度  
                float midR = (_m_particleRefObj.max_radius + _m_particleRefObj.min_radius) / 2;
                float rate1 = Random.Range(1.0f, midR / _m_particleRefObj.min_radius);
                float rate2 = Random.Range(midR / _m_particleRefObj.max_radius, 1.0f);
                float r = Random.Range(_m_particleRefObj.max_radius * rate1, _m_particleRefObj.max_radius * rate2);

                //计算炸裂散开的坐标
                Vector2 burstPos = new Vector2(_m_vStartPos.x + r * Mathf.Cos(rad), _m_vStartPos.y + r * Mathf.Sin(rad));

                //创建粒子对象
                BurstParticleObj particleObj = BurstParticleObjCache.instance.popItem();
                if (null == particleObj)
                    continue;

                //加入队列
                _m_lActiveParticleList?.Add(particleObj);

                particleObj.setValue(i * _m_particleRefObj.create_interval, this
                    , _m_vStartPos, burstPos, _m_vEndPos, _m_particleRefObj.burst_time + (i * _m_particleRefObj.single_particle_fly_interval)
                    , _m_particleRefObj.fly_time, _m_particleRefObj.first_burst_acc_time_scale, _m_particleRefObj.first_fly_acc_time_scale,
                    _m_particleRefObj.alpha_start_time, _m_particleRefObj.alpha_end_value);
            }
        }

        /// <summary>
        /// 回收所有还未回收的粒子，已经表现完成的粒子在飞行结束事件中回收
        /// </summary>
        private void _pushBackAllItem()
        {
            if (_m_lActiveParticleList == null)
                return;

            //回收所有Item
            BurstParticleObj tmpObj = null;
            for (int i = 0; i < _m_lActiveParticleList.Count; i++)
            {
                tmpObj = _m_lActiveParticleList[i];

                //调用一次onflydone
                _onFlyDone();

                if (null == tmpObj)
                    continue;

                //将对象放回缓存
                BurstParticleObjCache.instance.pushBackCacheItem(tmpObj);
            }
            //清空队列
            _m_lActiveParticleList.Clear();
        }

        #endregion


        #region 粒子任务

        /// <summary>
        /// 初始化粒子运动任务
        /// </summary>
        private void _initParitcleTickTask()
        {
            _discardParticleTickTask();

            //获取开始时间戳
            _m_fPraticleStartTimeS = Time.realtimeSinceStartup;

            //开启每帧任务进行处理
            _m_tParicleTickTask = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_tick);
        }

        /// <summary>
        /// 销毁粒子运动任务
        /// </summary>
        private void _discardParticleTickTask()
        {
            _m_tParicleTickTask.setDisable();
        }

        /// <summary>
        /// 执行每一个粒子的运动任务
        /// </summary>
        private void _tick()
        {
            //计算当前运行时间
            float processTimeS = Time.realtimeSinceStartup - _m_fPraticleStartTimeS;

            BurstParticleObj tmpObj = null;
            for (int i = 0; i < _m_lActiveParticleList.Count; )
            {
                tmpObj = _m_lActiveParticleList[i];
                if (null == tmpObj)
                    continue;

                //根据tick结果进行不同处理
                if(!tmpObj.tick(processTimeS))
                {
                    //如果数据无效，从队列删除
                    _m_lActiveParticleList.RemoveAt(i);

                    //放回缓存
                    BurstParticleObjCache.instance.pushBackCacheItem(tmpObj);
                }
                else
                {
                    i++;
                }
            }

            //如果全部完成了，执行全部结束事件
            if (_m_bIsEnable && _m_lActiveParticleList.Count <= 0)
            {
                _onGroupDone();
            }
        }

        #endregion
    }
}