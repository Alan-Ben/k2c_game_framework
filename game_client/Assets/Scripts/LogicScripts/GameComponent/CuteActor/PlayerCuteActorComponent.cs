using System;
using System.Collections.Generic;
using ALPackage;
using Common.NpPlayerInfoObj;
using GC2GS.p002_InitOp;
using GC2GS.p021_PlayerInfo;
using GS2GC.p002_InitOp;
using GS2GC.p021_PlayerInfo;
using NPCommon;
using NPEnum;

namespace GOE
{
    public class PlayerCuteActorComponent : _ANPBasicPlayerComponent
    {
        private List<CuteActorInfo> _m_lCuteActorList = new List<CuteActorInfo>();
        
        private ALCommonEnableTaskController _m_expireCuteActorTask;
        private CuteActorInfo _m_iEarliestExpireCuteActor;//距离将要过期最近的Q版形象
        
        public PlayerCuteActorComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.CUTE_ACTOR; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 
        /// </summary>
        public override void presendInitProtocol()
        {
            reqCuteActorInit();
        }

        protected override void _dealInit()
        {
        }

        protected override void _onInitDone()
        {
        }

        protected override void _onInitFail()
        {
            ALLog.Error("PlayerCuteActorComponent init Fail!!!");
        }

        protected override void _discard()
        {
            _clear();
        }

        private void _clear()
        {
            _discardExpireCuteActorTask();
            _m_lCuteActorList?.Clear();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_actorId"></param>
        /// <param name="_onExpiredReturnNull">因为过期形象不移除, 所以这里加上过期形象是否返回空判断, 默认不返回null</param>
        /// <returns></returns>
        public CuteActorInfo getCuteActorInfo(long _actorId, bool _onExpiredReturnNull = false)
        {
            if (_m_lCuteActorList == null || _m_lCuteActorList.Count <= 0)
                return null;

            CuteActorInfo actorInfo = _m_lCuteActorList.Find((_cuteActor) =>
            {
                if (_cuteActor == null)
                    return false;
                return _cuteActor.actorId == _actorId;
            });

            return (actorInfo == null || (actorInfo.isExpired && _onExpiredReturnNull)) ? null : actorInfo;
        }

        /// <summary>
        /// 添加或者更新Q版形象信息
        /// </summary>
        /// <param name="_serverCuteActor"></param>
        private void _addOrUpdateCuteActorInfo(PlayerInfo_CuteActor _serverCuteActor)
        {
            if (_serverCuteActor == null)
                return;

            CuteActorInfo localCuteActorInfo = getCuteActorInfo(_serverCuteActor.getId());
            if (localCuteActorInfo == null)//若没有找到, 添加
            {
                localCuteActorInfo = new CuteActorInfo(_serverCuteActor);
                if (_m_lCuteActorList == null)
                    _m_lCuteActorList = new List<CuteActorInfo>();
                _m_lCuteActorList.Add(localCuteActorInfo);
            }
            else//若找到, 更新
            {
                localCuteActorInfo.updateActorInfo(_serverCuteActor);
            }

            // 若新增或更新的Q版形象不是永久的 且 还未过期 且 localCuteActorInfo过期时间 < _m_iEarliestExpireCuteActor过期时间
            if (!localCuteActorInfo.isPermanent && !localCuteActorInfo.isExpired && localCuteActorInfo.compareExpireTime(_m_iEarliestExpireCuteActor) < 0)
            {
                _m_iEarliestExpireCuteActor = localCuteActorInfo;
            }
        }
        
        /// <summary>
        /// 移除Q版形象信息
        /// </summary>
        /// <param name="_actorId"></param>
        /// <returns></returns>
        private bool _removeCuteActorInfo(long _actorId)
        {
            CuteActorInfo cuteActorInfo = getCuteActorInfo(_actorId);
            return _removeCuteActorInfo(cuteActorInfo);
        }
        
        /// <summary>
        /// 移除Q版形象信息
        /// </summary>
        /// <param name="_actorInfo"></param>
        private bool _removeCuteActorInfo(CuteActorInfo _actorInfo)
        {
            if(_actorInfo == null)
                return false;
            
            bool removeRes = _m_lCuteActorList?.Remove(_actorInfo) ?? false;
            
            if(_actorInfo == _m_iEarliestExpireCuteActor)//若是最早过期的Q版形象被移除，更新最早过期的Q版形象
                _updateEarliestExpireCuteActor();

            return false;
        }
        
        #region Q版形象过期处理

        /// <summary>
        /// 销毁
        /// </summary>
        private void _discardExpireCuteActorTask()
        {
            _m_expireCuteActorTask.setDisable();
        }
        
        /// <summary>
        /// 初始化Q版形象过期处理函数
        /// </summary>
        private void _initExpireCuteActorTask()
        {
            _m_iEarliestExpireCuteActor = null;
            _discardExpireCuteActorTask();
            
            if (_m_lCuteActorList == null || _m_lCuteActorList.Count <= 0)
                return;
            
            _updateEarliestExpireCuteActor();
            _m_expireCuteActorTask = CommonTaskController.CommonEnableDurationActionAddMonoTask(expireCuteActorTask, 1f);
        }

        private void expireCuteActorTask()
        {
            // 因为在每次Actor有变化时(过期 或 通过推送协议变化 可能影响到距离当前过期最近的Actor时_m_iEarliestExpireCuteActor都会更新
            // 所以这里_m_iEarliestExpireCuteActor == null的情况, 不再重新更新_m_iEarliestExpireCuteActor) 
            // if (_m_iEarliestExpireCuteActor == null && _m_lCuteActorList != null && _m_lCuteActorList.Count > 0)
            //     _updateEarliestExpireCuteActor();
            
            if(_m_iEarliestExpireCuteActor == null)
                return;

            // 若最早过期的Q版形象已经过期
            if (_m_iEarliestExpireCuteActor.isExpired)
            {
                // 这里不做移除, 只需要发送窗口消息通知Actor过期即可
                // // 移除最早过期的Q版形象
                // _removeCuteActorInfo(_m_iEarliestExpireCuteActor);
                
                // TODO: 发送窗口消息通知Actor过期
                
                _updateEarliestExpireCuteActor();//更新最早过期的Q版形象
            }
        }
        
        /// <summary>
        /// 更新最早过期的Q版形象信息
        /// </summary>
        /// <returns></returns>
        private void _updateEarliestExpireCuteActor()
        {
            _m_iEarliestExpireCuteActor = null;
            if (_m_lCuteActorList == null || _m_lCuteActorList.Count <= 0)
                return;

            foreach (CuteActorInfo cuteActorInfo in _m_lCuteActorList)
            {
                if(cuteActorInfo == null || cuteActorInfo.isPermanent || cuteActorInfo.isExpired)//若是永久的 或者 已经过期, 跳过判断
                    continue;

                if (cuteActorInfo.compareExpireTime(_m_iEarliestExpireCuteActor) < 0)
                    _m_iEarliestExpireCuteActor = cuteActorInfo;
            }
        }

        #endregion
        
        #region C2S

        /// <summary>
        /// 请求初始化Q版形象列表
        /// </summary>
        public void reqCuteActorInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_030_ReqCuteActorInit());
        }

        /// <summary>
        /// 请求设置玩家Q版形象(玩家Q版形象通过玩家参数ENPPlayerParam.CUTE_ACTOR获取, 在请求变化时也会变化玩家参数)
        /// </summary>
        /// <param name="_cuteActorId"></param>
        public void ReqSetCuteActor(long _cuteActorId)
        {
            NPGSClientListener.sendMsgByLog(NPGSWriter_004_PlayerOp.make_004_022_ReqSetCuteActor(_cuteActorId));
        }

        #endregion

        #region S2C

        /// <summary>
        /// 收到请求初始化Q版形象列表回包
        /// </summary>
        public void RetCuteActorInit(GS2GC_002_030_RetCuteActorInit _msg)
        {
            if(_msg == null)
                return;
            
            _clear();
            if (_m_lCuteActorList == null)
                _m_lCuteActorList = new List<CuteActorInfo>();
            CuteActorInfo cuteActorInfo = null;
            List<PlayerInfo_CuteActor> cuteActorList = _msg.getCuteActorList();
            if (cuteActorList != null && cuteActorList.Count > 0)
            {
                foreach (PlayerInfo_CuteActor item in cuteActorList)
                {
                    _addOrUpdateCuteActorInfo(item);
                }
            }

            _initExpireCuteActorTask();
            
            setInitDone();
        }

        /// <summary>
        /// Q版形象删除推送
        /// </summary>
        public void OnCuteActorDel(GS2GC_021_088_OnCuteActorDel _msg)
        {
            if(_msg == null)
                return;
            
            _removeCuteActorInfo(_msg.getRefId());//移除Q版形象信息
        }

        /// <summary>
        /// Q版形象数据更新
        /// </summary>
        /// <param name="_msg"></param>
        public void OnCuteActorChg(GS2GC_021_089_OnCuteActorChg _msg)
        {
            if(_msg == null)
                return;

            _addOrUpdateCuteActorInfo(_msg.getInfo());
        }

        /// <summary>
        /// 添加Q版形象数据
        /// </summary>
        /// <param name="_msg"></param>
        public void OnCuteActorAdd(GS2GC_021_090_OnCuteActorAdd _msg)
        {
            if(_msg == null)
                return;
            
            _addOrUpdateCuteActorInfo(_msg.getInfo());
        }
        
        #endregion
    }
}