using ALPackage;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 炸裂粒子窗口
    /// </summary>
    public class GGUIWndBurstParticle : _ANPGGUIBasicWnd<GGUIMonoBurstParticle>
    {
        private static GGUIWndBurstParticle _g_instance = new GGUIWndBurstParticle();
        public static GGUIWndBurstParticle instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndBurstParticle();
                return _g_instance;
            }
        }

        private List<BurstParticleGroupInfo> _m_lParticleGroupList;//粒子组列表
        private Particle_CacheMgr _m_cParticleCacheMgr;//粒子缓存池管理器

        public GGUIWndBurstParticle() : base(EALUIWndLayer.NOTICE) { }

        protected override string _monoAssetPath { get { return GGUIMonoBurstParticle.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBurstParticle.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onDiscard()
        {
            if (_m_lParticleGroupList != null)
            {
                foreach (BurstParticleGroupInfo burstParticleInfo in _m_lParticleGroupList)
                {
                    if (burstParticleInfo != null)
                        burstParticleInfo.discard();
                }
                _m_lParticleGroupList.Clear();
            }
            _m_lParticleGroupList = null;

            if (_m_cParticleCacheMgr != null)
                _m_cParticleCacheMgr.discardAll();
            _m_cParticleCacheMgr = null;
        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {

        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onWndInitDone()
        {
            if (wnd == null || wnd.sfxParent == null)
                return;

            //粒子组列表
            _m_lParticleGroupList = new List<BurstParticleGroupInfo>();

            //初始化粒子缓存池管理器
            _m_cParticleCacheMgr = new Particle_CacheMgr(wnd.sfxParent.gameObject, 1, 100);
        }

        /// <summary>
        /// 显示炸裂粒子
        /// </summary>
        /// <param name="_particleId">粒子ID</param>
        /// <param name="_particleNum">粒子数量</param>
        /// <param name="_start3DWorldPos">起点</param>
        /// <param name="_end3DWorldPos">终点</param>
        /// <param name="_onGroupLoaded">粒子组加载完成回调</param>
        /// <param name="_onItemLoaded">粒子加载完成回调</param>
        /// <param name="_doneAction">表现完成回调</param>
        /// <param name="_onBurstDone">每个粒子炸裂完成回调</param>
        /// <param name="_onFlyDone">每个粒子飞行完成回调</param>
        public void showBurstParticle(
            long _particleId,
            int _particleNum,
            Vector3 _start3DWorldPos,
            Vector3 _end3DWorldPos,
            Action<BurstParticleGroupInfo> _onGroupLoaded,
            Action<ParticleCacheItem> _onItemLoaded,
            Action _doneAction,
            Action _onBurstDone,
            Action _onFlyDone)
        {
            if (_particleNum <= 0)
            {
                _doneAction?.Invoke();
                return;
            }

            //计算坐标
            Vector2 startPos = GCommon.worldPos2UIPos(_start3DWorldPos);
            Vector2 endPos = GCommon.worldPos2UIPos(_end3DWorldPos);

            _showBurstParticleByUIPos(_particleId, _particleNum, startPos, endPos, _onGroupLoaded, _onItemLoaded, _doneAction, _onBurstDone, _onFlyDone);
        }

        /// <summary>
        /// 显示炸裂粒子
        /// </summary>
        /// <param name="_particleId">粒子ID</param>
        /// <param name="_particleNum">粒子数量</param>
        /// <param name="_startUIPos">起点</param>
        /// <param name="_endUIPos">终点</param>
        /// <param name="_onGroupLoaded">粒子组加载完成回调</param>
        /// <param name="_onItemLoaded">粒子加载完成回调</param>
        /// <param name="_doneAction">表现完成回调</param>
        /// <param name="_onBurstDone">每个粒子炸裂完成回调</param>
        /// <param name="_onFlyDone">每个粒子飞行完成回调</param>
        public void showBurstParticleByUIPos(
            long _particleId,
            int _particleNum,
            Vector2 _startUIPos,
            Vector2 _endUIPos,
            Action<BurstParticleGroupInfo> _onGroupLoaded,
            Action<ParticleCacheItem> _onItemLoaded,
            Action _doneAction,
            Action _onBurstDone,
            Action _onFlyDone)
        {
            if (_particleNum <= 0)
            {
                _doneAction?.Invoke();
                return;
            }

            _showBurstParticleByUIPos(_particleId, _particleNum, _startUIPos, _endUIPos, _onGroupLoaded, _onItemLoaded, _doneAction, _onBurstDone, _onFlyDone);
        }

        /// <summary>
        /// 显示炸裂粒子
        /// </summary>
        /// <param name="_particleId">粒子ID</param>
        /// <param name="_particleNum">粒子数量</param>
        /// <param name="_startRect">起点</param>
        /// <param name="_endRect">终点</param>
        /// <param name="_onGroupLoaded">粒子组加载完成回调</param>
        /// <param name="_onItemLoaded">粒子加载完成回调</param>
        /// <param name="_doneAction">表现完成回调</param>
        /// <param name="_onBurstDone">每个粒子炸裂完成回调</param>
        /// <param name="_onFlyDone">每个粒子飞行完成回调</param>
        public void showBurstParticleByUIPos(
            long _particleId,
            int _particleNum,
            RectTransform _startRect,
            RectTransform _endRect,
            Action<BurstParticleGroupInfo> _onGroupLoaded,
            Action<ParticleCacheItem> _onItemLoaded,
            Action _doneAction,
            Action _onBurstDone,
            Action _onFlyDone)
        {
            if (_startRect == null
                || _endRect == null
                || _particleNum <= 0)
            {
                _doneAction?.Invoke();
                return;
            }

            //计算坐标
            Vector2 startPos = GCommon.getUIRootPos(_startRect);
            Vector2 endPos = GCommon.getUIRootPos(_endRect);

            _showBurstParticleByUIPos(_particleId, _particleNum, startPos, endPos, _onGroupLoaded, _onItemLoaded, _doneAction, _onBurstDone, _onFlyDone);
        }

        /// <summary>
        /// 显示炸裂粒子
        /// </summary>
        /// <param name="_particleId">粒子ID</param>
        /// <param name="_particleNum">粒子数量</param>
        /// <param name="_startPos">起点</param>
        /// <param name="_endPos">终点</param>
        /// <param name="_onGroupLoaded">粒子组加载完成回调</param>
        /// <param name="_onItemLoaded">粒子加载完成回调</param>
        /// <param name="_doneAction">表现完成回调</param>
        /// <param name="_onBurstDone">每个粒子炸裂完成回调</param>
        /// <param name="_onFlyDone">每个粒子飞行完成回调</param>
        private void _showBurstParticleByUIPos(
            long _particleId, 
            int _particleNum, 
            Vector2 _startPos, 
            Vector2 _endPos,
            Action<BurstParticleGroupInfo> _onGroupLoaded,
            Action<ParticleCacheItem> _onItemLoaded,
            Action _doneAction,
            Action _onBurstDone,
            Action _onFlyDone)
        {
            if(_m_cParticleCacheMgr == null)
            {
                _doneAction?.Invoke();
                return;
            }

            //获取粒子配表信息
            NPParticleRefObj particleRef = GRefdataCoreMgr.instance.particleMap.getRef(_particleId);

            if (particleRef == null)
            {
                ALLog.Error($"Can not find particle: {_particleId}");
                _doneAction?.Invoke();
                return;
            }

            _m_cParticleCacheMgr.loadCache(particleRef.particle_index, (_cache) =>
            {
                if (wnd == null)
                    return;

                BurstParticleGroupInfo particleInfo = new BurstParticleGroupInfo(
                    wnd.sfxParent, 
                    particleRef, 
                    _cache, 
                    _particleNum, 
                    _startPos, 
                    _endPos,
                    _onItemLoaded,
                    _doneAction, 
                    _onBurstDone,
                    _onFlyDone, 
                    _onParticleGroupDone);

                if (_m_lParticleGroupList == null)
                    _m_lParticleGroupList = new List<BurstParticleGroupInfo>();
                _m_lParticleGroupList.Add(particleInfo);
                _onGroupLoaded?.Invoke(particleInfo);
            });
        }

        /// <summary>
        /// 在某组粒子表现完成时触发的函数
        /// </summary>
        protected void _onParticleGroupDone(BurstParticleGroupInfo _info)
        {
            _m_lParticleGroupList?.Remove(_info);
        }

        public void testDiscard()
        {
            if (_m_lParticleGroupList == null)
                return;

            if (_m_lParticleGroupList.Count >= 1)
            {
                _m_lParticleGroupList[0]?.discard();
            }
        }
    }
}