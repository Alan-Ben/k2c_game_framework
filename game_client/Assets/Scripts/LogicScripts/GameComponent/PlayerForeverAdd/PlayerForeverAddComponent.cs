using ALPackage;
using Common.InnObj;
using Common.PrivilegeCardEnum;
using Common.PrivilegeCardObj;
using CommonEnum;
using GC2GS.p021_PlayerInfo;
using GS2GC.p002_InitOp;
using GS2GC.p021_PlayerInfo;
using ILRuntime.CLR.TypeSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using GC2GS.p002_InitOp;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 玩家永久加成
    /// </summary>
    public class PlayerForeverAddComponent : _ANPBasicPlayerComponent
    {
        //玩家属性容器
        [NotNull] private NPPlayerPropertyContainer _m_playerPropertyContainer = new NPPlayerPropertyContainer("PlayerForeverAddComponent");
        //对应数据
        [NotNull] private Dictionary<long, int> _m_valueDict = new Dictionary<long, int>();


        //构造函数
        public PlayerForeverAddComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        protected static ENPPlayerCompType[] _g_DependComp = { };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.PLAYER_FOREVER_ADD; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }
        /// <summary>
        /// 永久加成改变事件
        /// </summary>
        public event Action<long, int> onForeverAddChanged;

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }
        /// <summary>
        /// 玩家属性容器
        /// </summary>
        public NPPlayerPropertyContainer playerPropertyContainer { get { return _m_playerPropertyContainer; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            _reqInit();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
          
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerForeverAddComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _clear();
        }

        public override void onAllCompInited()
        {
        }

        //析构函数
        private void _clear()
        {
            _m_valueDict.Clear();
            _m_playerPropertyContainer.clear();
        }
        
        /// <summary>
        /// 请求权益卡初始化
        /// </summary>
        private void _reqInit()
        {
            //清空一次
            _clear();
            
            NPGSClientListener.sendRequestByLog(new GC2GS_002_069_ReqForeverAddInit(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_002_069_RetForeverAddInit>((_info) =>
                {
                    foreach (NPCommon_ForeverAddInfo npCommonForeverAddInfo in _info.getAddList())
                    {
                        if(null == npCommonForeverAddInfo)
                            continue;
                        
                        PlayerForeverAddRefObj foreverAddRefObj = GRefdataCoreMgr.instance.playerForeverAddRefCore.getRef(npCommonForeverAddInfo.getId());
                        if(null == foreverAddRefObj)
                        {
                            Debug.LogError($"PlayerForeverAddComponent 服务端给的id找不到对应配表，id是：{npCommonForeverAddInfo.getId()}");
                            continue;
                        }
                        
                        _m_valueDict.Add(npCommonForeverAddInfo.getId(), npCommonForeverAddInfo.getCount());
                        _m_playerPropertyContainer.addModifier(foreverAddRefObj.getProperty(npCommonForeverAddInfo.getCount()));
                    }

                    setInitDone();
                }));
        }

        public void onForeverAddChg(GS2GC_021_075_OnForeverAddChg _msg)
        {
            if(null == _msg)
                return;
            PlayerForeverAddRefObj foreverAddRefObj = GRefdataCoreMgr.instance.playerForeverAddRefCore.getRef(_msg.getAdd().getId());
            if(null == foreverAddRefObj)
            {
                Debug.LogError($"PlayerForeverAddComponent 服务端给的id找不到对应配表，id是：{_msg.getAdd().getId()}");
                return;
            }

            int count = _msg.getAdd().getCount();

            //有旧的先删除旧的
            if(_m_valueDict.TryGetValue(foreverAddRefObj.id, out int oldValue))
            {
                _m_playerPropertyContainer.removeModifier(foreverAddRefObj.getProperty(oldValue));
                _m_valueDict[foreverAddRefObj.id] = count;
            }
            else
            {
                _m_valueDict.Add(foreverAddRefObj.id, count);
            }

            //再加入新的
            _m_playerPropertyContainer.addModifier(foreverAddRefObj.getProperty(count));

            // 触发永久加成改变事件
            onForeverAddChanged?.Invoke(foreverAddRefObj.id, count);
        }
    }
}
