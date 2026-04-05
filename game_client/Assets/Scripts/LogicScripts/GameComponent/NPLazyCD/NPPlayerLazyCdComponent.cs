using System;
using System.Collections.Generic;
using ALPackage;
using GC2GS.p021_PlayerInfo;
using JetBrains.Annotations;
using NPCommon;
using GS2GC.p002_InitOp;
using GS2GC.p021_PlayerInfo;

namespace GOE
{
    //cd管理器
    public class NPPlayerLazyCdComponent : _ANPBasicPlayerComponent
    {
        //CD数据集
        [NotNull]private Dictionary<long, PlayerLazyCDInfo> _m_cdInfoDict = new Dictionary<long, PlayerLazyCDInfo>();

        //构造函数
        public NPPlayerLazyCdComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            
        }

        //属性
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.LAZY_CD; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        
        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            //请求卡牌列表
            _ReqPlayerLazyCDList();
        }

        protected override void _dealInit()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_START, _onCommonActivityStart);
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onCommonActivityClose);
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("NPPlayerLazyCdComponent init Fail!!!");
        }
        //释放资源函数
        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_START, _onCommonActivityStart);
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_CLOSE, _onCommonActivityClose);
            
            _clear();
        }

        //析构函数
        private void _clear()
        {
            _m_cdInfoDict.Clear();
        }

        /// <summary>
        /// 获取cd数据
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public PlayerLazyCDInfo getLazyCDInfo(long _id)
        {
            PlayerLazyCDInfo lazyCdInfo;
            _m_cdInfoDict.TryGetValue(_id, out lazyCdInfo);

            return lazyCdInfo;
        }
        
        /// <summary>
        /// 获取当前计数
        /// </summary>
        /// <returns></returns>
        public int getCount(long _id)
        {
            PlayerLazyCDInfo lazyCdInfo = getLazyCDInfo(_id);
            if (null == lazyCdInfo)
                return 0;

            return lazyCdInfo.getCount();
        }

        /// <summary>
        /// 获取最大计数
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public int getMaxCount(long _id)
        {
            PlayerLazyCDInfo lazyCdInfo = getLazyCDInfo(_id);
            if (null == lazyCdInfo)
                return 0;

            return lazyCdInfo.MaxCount;
        }

        /// <summary>
        /// 获取恢复倒计时
        /// </summary>
        /// <returns></returns>
        public long getRemainMs(long _id)
        {
            PlayerLazyCDInfo lazyCdInfo = getLazyCDInfo(_id);
            if (null == lazyCdInfo)
                return 0;

            return lazyCdInfo.getRemainMs();
        }

        /// <summary>
        /// 获取恢复到满的时间
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public long getMaxRemainMs(long _id)
        {
            PlayerLazyCDInfo lazyCdInfo = getLazyCDInfo(_id);
            if (null == lazyCdInfo)
                return 0;

            return lazyCdInfo.getMaxRemainMs();
        }

        /// <summary>
        /// 获取到达最大值指定百分比数量的剩余时间
        /// </summary>
        /// <param name="_id"></param>
        /// <param name="_rate"></param>
        /// <returns></returns>
        public long getRemainMsByRate(long _id, float _rate)
        {
            PlayerLazyCDInfo lazyCdInfo = getLazyCDInfo(_id);
            if (null == lazyCdInfo)
                return 0;

            return lazyCdInfo.getRemainMsByRate(_rate);
        }

        /// <summary>
        /// 更新lazycd信息
        /// </summary>
        private void _updateLazyCdInfo(NPCommon_PlayerLazyCD _serverLazyCdInfo)
        {
            if(_serverLazyCdInfo == null)
                return;
            
            _m_cdInfoDict.TryGetValue(_serverLazyCdInfo.getCdId(), out PlayerLazyCDInfo lazyCdInfo);

            if (null == lazyCdInfo)
            {
                lazyCdInfo = new PlayerLazyCDInfo(_serverLazyCdInfo);
                if(null == lazyCdInfo.LazyCdRef)
                    return;
                _m_cdInfoDict.Add(lazyCdInfo.CdId, lazyCdInfo);
            }
            else
            {
                lazyCdInfo.updateInfo(_serverLazyCdInfo);
            }
            WinMsg.SendMsg(WinMsgType.ON_LAZY_CD_CHG, lazyCdInfo.CdId);
        }
        
        #region 红点


        #endregion

        #region S2C

        //CD组件初始化
        public void RetPlayerLazyCDList(GS2GC_002_032_RetPlayerLazyCDList _dataList)
        {
            if(null == _dataList)
                return;
            _m_cdInfoDict?.Clear();
            foreach (NPCommon_PlayerLazyCD npCommonPlayerLazyCd in _dataList.getCdList())
            {
                if(null == npCommonPlayerLazyCd)
                    continue;
                
                PlayerLazyCDInfo lazyCdInfo = new PlayerLazyCDInfo(npCommonPlayerLazyCd);
                if(null == lazyCdInfo.LazyCdRef)
                    continue;
                
                _m_cdInfoDict.Add(lazyCdInfo.CdId, lazyCdInfo);
            }

            //设置加载完成
            setInitDone();
        }


        /// <summary>
        /// CD数据改变
        /// </summary>
        /// <param name="_ret"></param>
        public void OnPlayerLazyCdChged(GS2GC_021_050_OnPlayerLazyCdChged _ret)
        {
            if(null == _ret)
                return;
            if (!isInited)
                return;

            _updateLazyCdInfo(_ret.getCdInfo());
        }


        /// <summary>
        /// CD数据新增
        /// </summary>
        /// <param name="_ret"></param>
        public void OnAddNewLazyCd(GS2GC_021_051_OnAddNewLazyCd _ret)
        {
            if(null == _ret)
                return;
            if (!isInited)
                return;

            _updateLazyCdInfo(_ret.getCdInfo());
        }
        
        #endregion

        
        
        #region C2S
        
        //请求初始化数据
        private void _ReqPlayerLazyCDList()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_032_ReqPlayerLazyCDList());
        }
        
        /// <summary>
        /// 请求查看玩家LazyCD
        /// </summary>
        /// <param name="_cdId">CD类型ID</param>
        /// <param name="_dealDone">成功回调</param>
        /// <param name="_dealFail">失败回调</param>
        public void reqViewPlayerLazyCD(int _cdId, Action<GS2GC_021_045_RetViewPlayerLazyCD> _dealDone = null, Action _dealFail = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_021_045_ReqViewPlayerLazyCD(_cdId), 
                new CommonRequestCallbackProtocolDealer<GS2GC_021_045_RetViewPlayerLazyCD>((_info) =>
                {
                    if(_info != null)
                        _updateLazyCdInfo(_info.getCdInfo());
                    
                    _dealDone?.Invoke(_info);
                }, (_error) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_error);
                    _dealFail?.Invoke();
                }));
        }
    
        #endregion

        #region 消息监听

        /// <summary>
        /// 当活动开启时
        /// </summary>
        /// <param name="_params"></param>
        private void _onCommonActivityStart(params object[] _params)
        {
            if(!isInitDone || _params == null || _params.Length < 2 || !( _params[0] is long _activityId) || !( _params[1] is long _activityInstanceId))
                return;

            PlayerLazyCDInfo activityLazyCdInfo = null;
            foreach (var cdInfo in _m_cdInfoDict.Values)
            {
                if (cdInfo != null && cdInfo.LazyCdRef != null && cdInfo.LazyCdRef.relative_activity_id == _activityId)
                {
                    activityLazyCdInfo = cdInfo;
                    break;
                }
            }
            
            // 更新活动相关lazycd数据
            if(activityLazyCdInfo != null)
                reqViewPlayerLazyCD((int)activityLazyCdInfo.CdId);
        }

        /// <summary>
        /// 当活动关闭时
        /// </summary>
        /// <param name="_params"></param>
        private void _onCommonActivityClose(params object[] _params)
        {
            // 活动关闭时就先不处理lazycd了, 因为活动关闭也没地方展示相关数据
            // 后续若有需要再进行处理, 但是不能从_m_cdInfoDict中移除, 因为_m_cdInfoDict中实际上应该时包含了所有lazycd数据的, 若这边移除会导致数据不完整
            
            // if(!isInitDone || _params == null || _params.Length < 2 || !( _params[0] is long _activityId) || !( _params[1] is long _activityInstanceId))
            //     return;
            //
            // PlayerLazyCDInfo activityLazyCdInfo = null;
            // foreach (var cdInfo in _m_cdInfoDict.Values)
            // {
            //     if (cdInfo != null && cdInfo.LazyCdRef != null && cdInfo.LazyCdRef.relative_activity_id == _activityId)
            //     {
            //         activityLazyCdInfo = cdInfo;
            //         break;
            //     }
            // }
            //
            // if (activityLazyCdInfo != null)
            //     _m_cdInfoDict.Remove(activityLazyCdInfo.CdId);
        }
        
        #endregion
        
    }
}

