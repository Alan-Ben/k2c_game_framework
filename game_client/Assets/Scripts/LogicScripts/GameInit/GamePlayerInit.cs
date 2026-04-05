using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using LitJson;

using ALPackage;
using NPEnum;


namespace GOE
{
    /// <summary>
    /// 游戏初始化过程处理函数
    /// </summary>
    public class GamePlayerInit
    {
        #region GS服务器初始化处理
        //gs 登陆完成后处理的事情
        public static void onUSEntered()
        {
            //发送埋点-开始初始化玩家数据组件
            GCommon.sendStepReport(TraceConst.START_INIT_PLAYER_COMP);

#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning("Server Version: " + GameResCore.instance.serverVersion);
#endif

            //初始化所有gs登入后且需要在comp之前需要完成的初始化数据
            _initAllPerCompData();
            
            //在现有流程里，只有当连接到US成功之后，才适合重新确认原先的消息链
            //发送消息核对处理协议
            NPGSClientListener.directSendMsg(NPGSWriter_001_BasicOp.make_020_ReconnectInfo(NPGSMsgDealer.instance.getReceivedMsgCount()));

            //开始提前发送玩家component初始化消息
            if (null != NPPlayer.instance.compMgr)
                NPPlayer.instance.compMgr.__presendInitProtocol();

            //进入游戏初始化前先check一下refdata是否加载，已经加载直接进去，还没加载开始加载
            GameLoginResLoadController.instance.tryDealDoneDelegate(() =>
            {
                ALCommonTaskController.CommonActionAddNextFrameTask(() =>
                {
                    //初始化组件前设置不能弹出奖励
                    RewardQueueMgr.instance.setPaused(true);
                    
                    //进行玩家数据初始化
                    NPPlayer.instance.compMgr.initMustComp(onAllCompInitedData);
                });

#if UNITY_EDITOR || UNITY_STANDALONE
                long npplayerSerialize = NPPlayer.instance.npplayerSerialize;
                ALCommonActionMonoTask.addMonoTask(() =>
                {
                    if (npplayerSerialize != NPPlayer.instance.npplayerSerialize)
                        return;

                    string mes = NPPlayer.instance.compMgr.getAllUnInitComp();

                    if (!string.IsNullOrEmpty(mes))
                        NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.init_comp_not_inited_str, NPPlayer.instance.compMgr.getAllUnInitComp()), 
                            TextTranslate.instance.getLanguage(TransKeyConst.confirm), NPPlayer.instance.compMgr.forceInited);
                }, 10f);
#endif
            });
        }

        //gs 连接恢复的处理
        public static void onGSResume()
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning("Server Version: " + GameResCore.instance.serverVersion);
#endif

            //在现有流程里，只有当连接到US成功之后，才适合重新确认原先的消息链
            //发送消息核对处理协议
            NPGSClientListener.directSendMsg(NPGSWriter_001_BasicOp.make_020_ReconnectInfo(NPGSMsgDealer.instance.getReceivedMsgCount()));
            // NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_025_ReqPlayerChatRoom());
        }

        /// <summary>
        /// 初始化所有gs登入后且需要在comp之前需要完成的初始化数据
        /// </summary>
        private static void _initAllPerCompData()
        {
            //初始化红点系统
            RedTipMgr.instance.init();
        }
        
        /// <summary>
        /// 在所有玩家组件初始化完成后调用的函数
        /// </summary>
        /// <param name="_res"></param>
        public static void onAllCompInitedData(bool _res)
        {
            if(!_res)
            {
                //发送埋点-初始化玩家数据组件失败
                GCommon.sendStepReport(TraceConst.INIT_PLAYER_COMP_FAIL);

                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_playerinfo_init_fail_none) , 
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm) , null);
                return;
            }

            //发送埋点-初始化玩家数据组件成功
            GCommon.sendStepReport(TraceConst.INIT_PLAYER_COMP_SUC);

#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning("Init Step all done");
#endif

            //调用所有组件完成处理
            NPPlayer.instance.compMgr.onAllCompInited();

            // 进行热更组件初始化
            long npplayerSerialize = NPPlayer.instance.npplayerSerialize;//保证重登情况下不会执行上一次的完成回调
            bool isHotfixComponentInitDone = false;//保证完成回调只会被执行一次
            HotfixStaticFunc.initHotfixDataComponent((_isSucc)=>
            {
                //若已经进行了重登, 或热更组件已经初始化完成，则时间到了也不再执行
                if(npplayerSerialize != NPPlayer.instance.npplayerSerialize || isHotfixComponentInitDone)
                {
                    return;
                }

                isHotfixComponentInitDone = true;
                
                onHotfixDataComponentInitDone(_isSucc);
            });
            
            //10秒后还没初始化完，应该是热更组件哪里有问题，但需要能进入游戏
            ALCommonTaskController.CommonActionAddMonoTask(() =>
            {
                //如果这10秒过程中，服务器断线了, 或热更组件已经初始化完成，则时间到了也不再执行
                if(npplayerSerialize != NPPlayer.instance.npplayerSerialize || isHotfixComponentInitDone)
                {
                    return;
                }

                isHotfixComponentInitDone = true;

                onHotfixDataComponentInitDone(false);
            }, 10f);

            //数据初始化完后设置AIHelp相关信息
            GCommon.setAIHelpLanguage();
            GCommon.setAIHelpUserInfo();
        }

        /// <summary>
        /// 初始化hotfix数据组件结束调用
        /// </summary>
        public static void onHotfixDataComponentInitDone(bool _initSucc)
        {
            if(!_initSucc)
            {
                // GOK热更组件初始化失败也让进游戏 所以这里只谈一个弹窗报错提示, 不返回, 还是继续
                Debug.LogError("初始化热更数据Component失败");
                NPMesMgr.instance.showOneBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.login_init_hotfix_component_failed) , 
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm) , _realDealAftHotfixComponentInitDone);
                
                return;
            }

            _realDealAftHotfixComponentInitDone();
        }

        private static void _realDealAftHotfixComponentInitDone()
        {
            //打开loading
            Loading.showLoading(_complete =>
            {
                //进入游戏界面
                Game.instance.loginGameInitUINode(() =>
                {
                    //开始允许弹出奖励
                    RewardQueueMgr.instance.setPaused(false);
                    
                    //刷新一下显示
                    WinMsg.SendMsg(WinMsgType.CUSTOM_RELOAD);
                    
                    if (_complete != null) 
                        _complete();
                    
                    //发送埋点-游戏界面进入完成
                    GCommon.sendStepReport(TraceConst.SUC_ENTER_PER_CREATE_NODE);

                    //第一次进入游戏界面检查一次冲榜排名，有变化则弹tip
                    NPPlayer.instance.commonActivityComp.initCheckRankingIsChange();
                });
            });
        }
        
        #endregion
    }
}
