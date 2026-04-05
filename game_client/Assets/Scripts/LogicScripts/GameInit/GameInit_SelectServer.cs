using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using LitJson;

using ALPackage;
using NPEnum;
using NPCommon;
using NPGS2GC.p001_BasicOp;

namespace GOE
{
    /// <summary>
    /// 玩家登录游戏过程的操作数据对象，全局唯一，方便不同操作之间传递信息
    /// 此数据为用户登录完LS，连接到GS之后的相关登录流程控制类
    /// </summary>
    public class GameInit_SelectServer : _AGameInitProcess
    {
        private static GameInit_SelectServer _g_instance = new GameInit_SelectServer();
        public static GameInit_SelectServer instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GameInit_SelectServer();
                return _g_instance;
            }
        }

        //当前登录使用的UId，每个LS登录成功会重置此数据
        private string _m_sUid;

        //作弊登入时候使用的服务器id
        private int _m_cheatLoginServerId;
        //当前选择登录的服务器信息，在设置UID的时候根据当前配置自动刷新此数据
        private int _m_curSelectServerLogicId;
        //更改选中服务器时的消息处理
        private Action _m_dOnSelectServerItemChg;
        //推荐服信息Id
        private int _m_recommendServerLogicId;
        //推荐服务器数据返回回调
        private Action<int> _m_retRecommendServerLogicId;
        
        //当前登录US的操作序列号
        private long _m_lCurLoginUSSerialize;
        //当前登录的US服务器Id
        private int _m_iserverLogicId;
        //当前登录US的队列索引相关信息
        private long _m_lCurLoginUSQueueIndex;
        private long _m_lCurQueueServerEnterIndex;
        
        //是否自动登录 默认false，目前机制是在切换语言适合改成自动登入
        private bool _m_isAutoLogin = false;
        
        //完成本过程的回调处理
        private Action _m_dDoneDelegate;

        
        private static readonly string _m_gProcessName = "GameInit_SelectServer";
        public static string gProcessName { get { return _m_gProcessName; } }
        
        /// <summary>
        /// 当前登录的US服务器Id
        /// </summary>
        public int loginServerLogicId
        {
            get { return _m_iserverLogicId; }
        }

        public int cheatLoginServerId { get { return _m_cheatLoginServerId; } set { _m_cheatLoginServerId = value; } }
        
        /// <summary>
        /// TODO 预留的接口，感觉比较好的是start_game界面开个勾选，是否自动登入，勾选了后面每次等人员都不用去start_game界面
        /// TODO 暂时先给切换语言临时后，看后面策划改不改
        /// </summary>
        public bool isAutoLogin { get { return _m_isAutoLogin; } set { _m_isAutoLogin = value; } }

        protected GameInit_SelectServer()
        {
            _m_sUid = string.Empty;

            _m_curSelectServerLogicId = 0;
            _m_recommendServerLogicId = 0;
            _m_retRecommendServerLogicId = default;
            _m_dOnSelectServerItemChg = default(Action);

            _m_lCurLoginUSSerialize = ALSerializeOpMgr.next();
            _m_iserverLogicId = 0;
            _m_lCurLoginUSQueueIndex = 0;
            _m_lCurQueueServerEnterIndex = 0;
            _m_cheatLoginServerId = 0;

            _m_dDoneDelegate = null;
        }
        
        /// <summary>
        /// 返回子进度队列
        /// </summary>
        /// <returns></returns>
        protected override _IALProgressnterface[] getChildProcess()
        {
            return null;
        }

        /// <summary>
        /// 重置数据部分
        /// </summary>
        protected override void _resetData()
        {
            //重置回调，避免init调用的时候回调被处理
            _m_dDoneDelegate = null;
            _m_recommendServerLogicId = 0;
            _m_curSelectServerLogicId = 0;
            _m_cheatLoginServerId = 0;
            _m_retRecommendServerLogicId = default;

            //整体初始化流程重置，需要断开玩家的相关服务器连接
            //需要退出相关连接对象
            if (NPGSClientListener.instance != null)
                NPGSClientListener.instance.logout();

            //重置uid为0
            setUid(string.Empty);
        }

        /// <summary>
        /// 加载处理函数
        /// </summary>
        /// <returns></returns>
        protected override void _dealInit(Action _doneDelegate)
        {

            //设置完成回调
            _m_dDoneDelegate = _doneDelegate;

            //判断当前用户之前是否有选择服务器，有则直接进入，没有则弹出开始游戏界面等待用户操作
            LoginTokenSetting.instance.init();

            //发送埋点-进入开始游戏界面
            GCommon.sendStepReport(TraceConst.ENTER_START_GAME_WND);

            //自动登入的话直接进游戏，不展示startgame  
            if (_m_isAutoLogin)
            {
                //每次只自动进入一次，后面再重登还是回到startgame界面
                _m_isAutoLogin = false;
                
                //调用请求进入服务器处理
                GameInit_SelectServer.instance.reqEnterUs();   
            }
            else
            {
                //进入一次start界面
                //展示开始游戏界面
                QueueMgr.instance.addNode_Login_MainUIMainWnd(NPGGUIWndStartGame.instance, UINodeTagConst.C_Login_Start_Game, false, false, null, null);
   
            }
        }

        /// <summary>
        /// 是否在队列中
        /// </summary>
        public bool isInQueue
        {
            get
            {
                //数据有效且位置比服务器进入索引大
                if (_m_lCurLoginUSQueueIndex > 0 && _m_lCurLoginUSQueueIndex >= _m_lCurQueueServerEnterIndex)
                    return true;

                return false;
            }
        }
        /// <summary>
        /// 获取当前所在队列位置
        /// </summary>
        public long curQueueSize
        {
            get
            {
                if (!isInQueue)
                    return 0;

                return _m_lCurLoginUSQueueIndex - _m_lCurQueueServerEnterIndex;
            }
        }

        /// <summary>
        /// 设置当前使用的UId
        /// </summary>
        /// <param name="_uid"></param>
        public void setUid(string _uid)
        {
            _m_sUid = _uid;

            //重置当前选择的服务器信息
            _m_curSelectServerLogicId = 0;
            _m_recommendServerLogicId = 0;
            _m_retRecommendServerLogicId = default;

            _m_lCurLoginUSSerialize = ALSerializeOpMgr.next();
            _m_iserverLogicId = 0;
            _m_lCurLoginUSQueueIndex = 0;
            _m_lCurQueueServerEnterIndex = 0;
        }

        /// <summary>
        /// 重置当前登录操作，在被服务器踢出或断线的时候触发
        /// </summary>
        public void resetCurLoginFunc()
        {
            _m_lCurLoginUSSerialize = ALSerializeOpMgr.next();
            _m_iserverLogicId = 0;
            _m_lCurLoginUSQueueIndex = 0;
            _m_lCurQueueServerEnterIndex = 0;

            //进入默认登录方式UI
            QueueMgr.instance.AddNode(new LNodeDefault());
        }

        /// <summary>
        /// 获取推荐服数据
        /// </summary>
        /// <param name="_msg"></param>
        public void reqRecommendedUSInfo(Action<int> _action)
        {
            //有数据直接返回
            if (_m_recommendServerLogicId > 0)
            {
                if (_action != null) 
                    _action(_m_recommendServerLogicId);
                return;
            }

            _m_retRecommendServerLogicId += _action;
            //请求推荐服信息，这边要等GS连上才行
            GameInit_LoginProcess.instance.regDoneDelegate(() =>
            {
                NPGSClientListener.directSendMsg(NPGSWriter_001_BasicOp.make_012_ReqMostRecommendedUSInfo());
            });
        }
        
        /// <summary>
        /// 获取当前选中的服务器ID
        /// </summary>
        /// <returns></returns>
        public void getCurSelectServerItem(Action<int> _action)
        {
            if (_m_curSelectServerLogicId > 0)
            {
                if (_action != null) 
                    _action(_m_curSelectServerLogicId);
                return;
            }
            
            //未使用sdk说明是账号密码登入界面登入,直接使用作弊登入选中的服务器id
            if (!Game.instance.mainCamera.isUseSDK && _m_cheatLoginServerId > 0)
            {
                if (_action != null) 
                    _action(_m_cheatLoginServerId);
                return;
            }
            
            //当数据为空时，如果数据非空表示已经有选择
            //根据最后一次登录记录，或者已有的服务器数据获取当前使用的服务器信息
            int preServerId = LoginTokenSetting.instance.getUidServerId(_m_sUid);
            if (preServerId > 0)
            {
                if (_action != null) 
                    _action(preServerId);
                return;
            }

            //如果还无数据，则获取最新推荐服务器
            reqRecommendedUSInfo(_action);
        }

        /// <summary>
        /// 注册服务器选中更改回调
        /// </summary>
        /// <param name="_delegate"></param>
        public void regServerChgDelegate(Action _delegate)
        {
            _m_dOnSelectServerItemChg += _delegate;
        }
        public void unregServerChgDelegate(Action _delegate)
        {
            _m_dOnSelectServerItemChg -= _delegate;
        }

        /// <summary>
        /// 设置当前选中的服务器Item
        /// </summary>
        /// <param name="_serverItem"></param>
        public void setCurSelectServerItem(ServerDataInfo _serverItem)
        {
            if (null == _serverItem)
                return;

            _m_curSelectServerLogicId = _serverItem.serverId;

            if (null != _m_dOnSelectServerItemChg)
                _m_dOnSelectServerItemChg();
        }

        /// <summary>
        /// 请求进入us，还原GS连接对象
        /// </summary>
        /// <param name="_serverData"></param>
        public void reqResumeUs()
        {
            //获取当前选中的服务器ID登录
            getCurSelectServerItem((_logicId) =>
            {
                if (_logicId < 0)
                    return;

                //刷新序列号
                _m_lCurLoginUSSerialize = ALSerializeOpMgr.next();
                _m_iserverLogicId = _logicId;
                _m_lCurLoginUSQueueIndex = 0;
                _m_lCurQueueServerEnterIndex = 0;

                //发送请求
                NPGSClientListener.directSendMsg(NPGSWriter_001_BasicOp.make_009_ResumeUSConnection(_m_lCurLoginUSSerialize, _logicId));
            });
        }

#if UNITY_EDITOR
        //是否收到了001_004协议，这个协议代表了是否真的成功进入US，
        public bool isReceive001_004;
#endif
        
        /// <summary>
        /// 请求进入us，返回请求的客户端序列号
        /// </summary>
        /// <param name="_serverData"></param>
        public void reqEnterUs()
        {
            //获取当前选中的服务器ID登录
            getCurSelectServerItem((_logicId) =>
            {
                //发送埋点-开始请求进入US
                GCommon.sendStepReport(TraceConst.START_REQ_ENTER_US.setMarkParam(_logicId));

                if (_logicId < 0)
                    return;

                //刷新序列号
                _m_lCurLoginUSSerialize = ALSerializeOpMgr.next();
                _m_iserverLogicId = _logicId;
                _m_lCurLoginUSQueueIndex = 0;
                _m_lCurQueueServerEnterIndex = 0;

#if UNITY_EDITOR
                isReceive001_004 = false;
                ALCommonTaskController.CommonActionAddMonoTask(() =>
                {
                    if (!isReceive001_004)
                    {
                        NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_us_protocol_timeout_none), 
                            TextTranslate.instance.getLanguage(TransKeyConst.confirm), null);
                        UnityEngine.Debug.LogError($"发生进入US协议后，10秒了还没收到GS的001_004协议，找服务端排查！！！");
                    }
                }, 10f);
#endif
                
                //发送请求
                NPGSClientListener.directSendMsg(NPGSWriter_001_BasicOp.make_005_RequestEnterUS(_m_lCurLoginUSSerialize, _logicId));
            });
        }

        /// <summary>
        /// 推荐服务器返回
        /// </summary>
        public void retMostRecommendedUSInfo(NPGS2GC.p001_BasicOp.NPGS2GC_001_012_RetMostRecommendedUSInfo _msg)
        {
            if(null == _msg)
                return;
            
            _m_recommendServerLogicId = _msg.getServerItem().getServerLogicId();
            
            //执行回调
            if (_m_retRecommendServerLogicId != null) 
                _m_retRecommendServerLogicId(_m_recommendServerLogicId);
            _m_retRecommendServerLogicId = null;
        }
        
        /// <summary>
        /// 请求进入服务器之后的回调消息处理
        /// </summary>
        /// <param name="_queueIndex"></param>
        public void retEnterUSRes(NPGS2GC_001_005_EnterUSRes _msg)
        {
            //发送埋点-US协议回包
            GCommon.sendStepReport(TraceConst.START_REQ_ENTER_US_RET);

            //判断序列号是否一致，不一致直接不处理i
            if (_msg.getClientSerialize() != _m_lCurLoginUSSerialize)
            {
                ALLog.Error($"retEnterUSRes client init serialize[{_msg.getClientSerialize()}] not equals to select server serialize[{_m_lCurLoginUSSerialize}]!");
                return;
            }

            //发送获取基本信息处理，刷新相关基本信息
            NPGSClientListener.directSendMsg(NPGSWriter_001_BasicOp.make_001_ReqBasicInfo());
            
            //根据结果信息进行不同处理
            if(_msg.getQueueIndex() != 0)
            {
                //设置队列信息
                _m_lCurLoginUSQueueIndex = _msg.getQueueIndex();
                _m_lCurQueueServerEnterIndex = 0;

                //发送消息请求队列信息
                NPGSClientListener.directSendMsg(NPGSWriter_001_BasicOp.make_007_ReqQueueInfo(_m_lCurLoginUSSerialize));

                //直接进入玩家排队过程信息
                QueueMgr.instance.addNode_Login_MainUIMainWnd(NPGGUIWndQueue.instance, UINodeTagConst.C_Login_Queue, false, null
                    , () => { NPGGUIWndQueue.instance.showInfo(TextTranslate.instance.getLanguage(TransKeyConst.login_reqEnterServer), _m_iserverLogicId); });
            }
        }

        /// <summary>
        /// 进入服务器冻结
        /// </summary>
        /// <param name="_msg"></param>
        public void enterUSButFreeze(NPGS2GC_001_013_EnterUSButFreeze _msg)
        {
            //发送埋点-进入US账号被冻结
            GCommon.sendStepReport(TraceConst.ENTER_US_FREEZE);

            //判断序列号是否一致，不一致直接不处理i
            if (_msg.getClientSerialize() != _m_lCurLoginUSSerialize)
            {
                ALLog.Error($"EnterUSButFreeze client init serialize[{_msg.getClientSerialize()}] not equals to select server serialize[{_m_lCurLoginUSSerialize}]!");
                return;
            }
            
            //打开默认startgame页面
            QueueMgr.instance.addNode_Login_MainUIMainWnd(NPGGUIWndStartGame.instance, UINodeTagConst.C_Login_Start_Game, false, false, null, null);

            //冻结弹窗
            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_serverFreeze, TimeUtil.millisecondsToTime_DayHourMin(_msg.getFreezeTimeMs()))
                , TextTranslate.instance.getLanguage(TransKeyConst.cancel)
                , () =>
                {
                    //刷新一下登录界面
                    NPGGUIWndStartGame.instance.refreshCurServer();
                }
                , TextTranslate.instance.getLanguage(TransKeyConst.login_chgServer)
                , () =>
                {
                    //刷新一下登录界面
                    NPGGUIWndStartGame.instance.refreshCurServer();
                    //打开服务器选择界面
                    QueueMgr.instance.addNode_Login_MainUIAddWnd(NPGGUIWndServerList.instance, UINodeTagConst.C_ADD_Login_Server_List);
                });
        }

        /// <summary>
        /// 在进入游戏服务器完成的时候调用的事件函数，会触发本流程完毕
        /// </summary>
        public void onUSEnterDone(long _clientInitSerialize)
        {
            //判断序列号是否一致，不一致则表示不是最新处理
            if (_clientInitSerialize != _m_lCurLoginUSSerialize)
            {
                ALLog.Error($"onUSEnterDone client init serialize[{_clientInitSerialize}] not equals to select server serialize[{_m_lCurLoginUSSerialize}]!");
                return;
            }

            WinMsg.SendMsg(WinMsgType.ON_US_ENTER_DONE);

            
            //记录选择的服务器信息
            LoginTokenSetting.instance.setUidServerId(_m_sUid, _m_iserverLogicId);

            //调用回调
            Action doneDelegate = _m_dDoneDelegate;
            _m_dDoneDelegate = null;
            if (null != doneDelegate)
                doneDelegate();

            //发送埋点-进入US完成
            GCommon.sendStepReport(TraceConst.ENTER_US_DONE);
        }

        /// <summary>
        /// 刷新当前排队窗口信息
        /// </summary>
        public void onUSQueueInfoChg(long _clientSerialize, long _curEnterIndex)
        {
            //判断序列号是否一致
            if (_clientSerialize != _m_lCurLoginUSSerialize)
                return;

            //设置当前索引
            _m_lCurQueueServerEnterIndex = _curEnterIndex;

            //当还需要排队时延迟1秒继续发送
            if (_m_lCurQueueServerEnterIndex < _m_lCurLoginUSQueueIndex)
            {
                //还需要排队
                //此时进入队列状态，显示对应队列节点
                NPGGUIWndQueue.instance.showInfo(TextTranslate.instance.getLanguage(TransKeyConst.login_queue_ing, _m_lCurLoginUSQueueIndex - _m_lCurQueueServerEnterIndex, GCommon.getLoginQueueTime(_m_lCurLoginUSQueueIndex - _m_lCurQueueServerEnterIndex)));

                //延迟1秒发送队列请求
                ALCommonTaskController.CommonActionAddMonoTask(() => { NPGSClientListener.directSendMsg(NPGSWriter_001_BasicOp.make_007_ReqQueueInfo(_m_lCurLoginUSSerialize)); }, 1f);
            }
            else
            {
                //此时进入队列状态，显示对应队列节点
                NPGGUIWndQueue.instance.showInfo(TextTranslate.instance.getLanguage(TransKeyConst.login_queue_end));

                //后续等待 1-4 消息返回将开始相关用户初始化操作
            }
        }

        /// <summary>
        /// 退出当前所在服务器的队列
        /// </summary>
        public void quitQueue()
        {
            //如果已经过了排队位置则不发送退出队列消息
            if (_m_lCurQueueServerEnterIndex < _m_lCurLoginUSQueueIndex)
                return;

            //发送消息请求队列信息
            NPGSClientListener.directSendMsg(NPGSWriter_001_BasicOp.make_008_ReqQuitQueue(_m_lCurLoginUSSerialize));
        }
    }
}
