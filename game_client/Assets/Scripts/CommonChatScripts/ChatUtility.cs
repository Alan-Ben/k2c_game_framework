
using System.Collections.Generic;
using System.Diagnostics;
using ChatPackage.Internal;
using Common.CommObj;
using Debug = UnityEngine.Debug;

namespace ChatPackage
{
    /// <summary>
    /// 聊天包一些通用的方法
    /// </summary>
    public static class ChatUtility
    {
        /// <summary>
        /// 组装聊天uid
        /// </summary>
        /// <param name="_systemId"></param>
        /// <param name="_systemTag"></param>
        /// <param name="_uid"></param>
        /// <returns></returns>
        public static string makeChatUid(long _systemId,string _systemTag, string _uid)
        {
            return $"{_systemId}.{_systemTag}.{_uid}";
        }
        
        /// <summary>
        /// 拆解聊天uid,返回对应参数【_systemId，_systemTag，_uid】
        /// 其中_uid为客户端传入参数，用于丢出给客户端游戏内使用
        /// </summary>
        /// <param name="_chatUid"></param>
        /// <returns>【_systemId，_systemTag，_uid】</returns>
        public static string[] splitChatUid(string _chatUid)
        {
            string[] strList = _chatUid.Split('.');
            return strList;
        }
        
        /// <summary>
        /// 用room相关的消息数据，制作一个用于服务器传输的消息类型
        /// </summary>
        /// <param name="_uid">服务器给的客户端唯一id</param>
        /// <param name="_roomId">聊天室id</param>
        /// <param name="_msgInfo">对应的消息</param>
        /// <returns>用于服务器传输的聊天室消息</returns>
        public static RoomMsg makeRoomMsg(string _uid, long _roomId, _AMsgDetailInfo _msgInfo)
        {
            if (_msgInfo == null)
                return null;

            Common_Msg content = new Common_Msg(_msgInfo.msgType, _msgInfo.getSenderBytesData(), _msgInfo.getContentBytesData());
            return new RoomMsg(_uid, _roomId, 0, 0, content);
        }

        /// <summary>
        /// 用private相关的消息数据，制作一个用于服务器传输的消息类型
        /// </summary>
        /// <param name="_senderUserToken">发送对象的标识</param>
        /// <param name="_receiverUserToken">接受对象的标识</param>
        /// <param name="_msgInfo">对应的消息</param>
        /// <param name="_timeMs">发送消息的时间</param>
        /// <returns>用于服务器传输的私聊消息类型</returns>
        public static PrivateChatMsg makePrivateChatMsg(string _senderUserToken,string _receiverUserToken, _AMsgDetailInfo _msgInfo,long _timeMs)
        {
            if (_msgInfo == null)
                return null;
            
            Common_Msg content = new Common_Msg(_msgInfo.msgType, _msgInfo.getSenderBytesData(), _msgInfo.getContentBytesData());

            return new PrivateChatMsg(_senderUserToken, _receiverUserToken, 0, content, _timeMs);
        }
        
        /// <summary>
        /// DebugOnly的错误日志
        /// </summary>
        [Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
        public static void logError_DebugOnly(string _content)
        {
            UnityEngine.Debug.LogError("【ChatPackage Error】" + _content);
        }

        /// <summary>
        /// DebugOnly的警告日志
        /// </summary>
        [Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
        public static void logWarning_DebugOnly(string _content)
        {
            UnityEngine.Debug.LogWarning("【ChatPackage Warning】" + _content);
        }
        
        /// <summary>
        /// DebugOnly打印日志
        /// </summary>
        [Conditional("DEBUG_EDITOR"), Conditional("UNITY_EDITOR")]
        public static void log_DebugOnly(string _content)
        {
            UnityEngine.Debug.Log("【ChatPackage】" + _content);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        public static void logError(string _content)
        {
            UnityEngine.Debug.LogError("【ChatPackage Error】" + _content);
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        public static void logWarning(string _content)
        {
            UnityEngine.Debug.LogError("【ChatPackage Warning】" + _content);
        }

        /// <summary>
        /// 打印日志
        /// </summary>
        public static void log(string _content)
        {
            UnityEngine.Debug.Log("【ChatPackage】" + _content);
        }

        /// <summary>
        /// 构建一个唯一的key
        /// </summary>
        public static int makeMsgItemDataKey(_IMsgItemData _data)
        {
            if (_data == null)
                return 0;
            
            // 构建一个唯一key
            int msgType = _data.msgType;
            bool isMyMsg = _data.isMyMsg;
            return makeMsgItemDataKey(msgType, isMyMsg);
        }

        /// <summary>
        /// 构建一个唯一的key
        /// </summary>
        public static int makeMsgItemDataKey(int _msgType, bool _isMyMsg)
        {
            // 简单的根据myMsg返回负数的msgType或者正数的msgType
            return _msgType * (_isMyMsg ? 1 : -1);
        }
    }
}