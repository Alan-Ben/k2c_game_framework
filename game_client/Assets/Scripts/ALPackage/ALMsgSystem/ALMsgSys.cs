using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
    //消息的通用参数接收处理对象
    public delegate void MsgRecAction(params object[] _objs);

    /// <summary>
    /// 窗口消息
    /// </summary>
    public class ALMsgSys
    {
        private static ALMsgSys _g_instance = new ALMsgSys();
        public static ALMsgSys instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new ALMsgSys();
                return _g_instance;
            }
        }

        private Dictionary<int, List<MsgRecAction>> _m_dBroadcastTable;
        private Dictionary<int, List<Action>> _m_dBroadcastActTable;

        protected ALMsgSys()
        {
            _m_dBroadcastTable = new Dictionary<int, List<MsgRecAction>>();
            _m_dBroadcastActTable = new Dictionary<int, List<Action>>();
        }

        /** 发送消息 */
        public static void SendMsg(int _msg)
        {
            try
            {
                instance._SendMsg(_msg);
            }
            catch(Exception _ex)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("Send Msg: " + _msg + " Err: " + _ex.Message + "\n" + _ex.StackTrace);
#endif
            }
        }
        public static void SendMsg(int _msg, params object[] _objs)
        {
            try
            {
                instance._SendMsg(_msg, _objs);
            }
            catch(Exception _ex)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("Send Msg: " + _msg + " Err: " + _ex.Message + "\n" + _ex.StackTrace);
#endif
            }
        }
        
        public static void RegisterMsg(int _msg, MsgRecAction _callback)
        {
            instance._RegisterMsg(_msg, _callback);
        }

        public static void RegisterMsgAct(int _msg, Action _callback)
        {
            instance._RegisterMsg(_msg, _callback);
        }
        
        public static void UnregisterMsg(int _msg, MsgRecAction _callback)
        {
            instance._UnregisterMsg(_msg, _callback);
        }
        public static void UnregisterMsgAct(int _msg, Action _callback)
        {
            instance._UnregisterMsg(_msg, _callback);
        }
        
        /*****************
         * 广播处理
         **/
        protected void _RegisterMsg(int _msg, MsgRecAction _callback)
        {
            List<MsgRecAction> broadcast;
            if(!_m_dBroadcastTable.TryGetValue(_msg, out broadcast))
            {
                broadcast = new List<MsgRecAction>();
                _m_dBroadcastTable[_msg] = broadcast;
            }

            if(!broadcast.Contains(_callback))
            {
                broadcast.Add(_callback);
            }

#if UNITY_EDITOR
            //editor增加数量报警，有助于检查问题
            if(broadcast.Count % 30 == 0)
            {
                ALLog.Error($"Msg: {_msg} count: {broadcast.Count}");
            }
#endif
        }

        protected void _RegisterMsg(int _msg, Action _callback)
        {
            List<Action> broadcast;
            if(!_m_dBroadcastActTable.TryGetValue(_msg, out broadcast))
            {
                broadcast = new List<Action>();
                _m_dBroadcastActTable[_msg] = broadcast;
            }

            if(!broadcast.Contains(_callback))
            {
                broadcast.Add(_callback);
            }

#if UNITY_EDITOR
            //editor增加数量报警，有助于检查问题
            if (broadcast.Count % 30 == 0)
            {
                ALLog.Error($"Msg: {_msg} count: {broadcast.Count}");
            }
#endif
        }

        /// <summary>
        /// 反注册广播事件
        /// </summary>
        /// <param name="_type">反注册类型</param>
        /// <param name="_callback">反注册广播事件</param>
        protected void _UnregisterMsg(int _msg, MsgRecAction _callback)
        {
            List<MsgRecAction> broadcast;
            if(!_m_dBroadcastTable.TryGetValue(_msg, out broadcast))
            {
                return;
            }

            broadcast.Remove(_callback);
        }
        protected void _UnregisterMsg(int _msg, Action _callback)
        {
            List<Action> broadcast;
            if(!_m_dBroadcastActTable.TryGetValue(_msg, out broadcast))
            {
                return;
            }

            broadcast.Remove(_callback);
        }

        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="_type">广播类型</param>
        /// <param name="_objs">广播不定参</param>
        protected void _SendMsg(int _msg)
        {
            List<MsgRecAction> srcList;
            if(_m_dBroadcastTable.TryGetValue(_msg, out srcList) && srcList.Count > 0)
            {
                //取出一个缓存列表
                List<MsgRecAction> broadcast = null;
                broadcast = ALMsgRecActionListCache.instance.popItem();

                try
                {
                    broadcast.AddRange(srcList);
                    for(int i = 0; i < broadcast.Count; ++i)
                    {
                        broadcast[i]();
                    }
                }
                finally
                {
                    ALMsgRecActionListCache.instance.pushBackCacheItem(broadcast);
                }
            }

            List<Action> srcActList;
            if(_m_dBroadcastActTable.TryGetValue(_msg, out srcActList) && srcActList.Count > 0)
            {
                //取出一个缓存列表
                List<Action> broadcast = null;
                broadcast = ALMsgActionListCache.instance.popItem();

                try
                {
                    broadcast.AddRange(srcActList);
                    for(int i = 0; i < broadcast.Count; ++i)
                    {
                        broadcast[i]();
                    }
                }
                finally
                {
                    ALMsgActionListCache.instance.pushBackCacheItem(broadcast);
                }
            }
        }
        protected void _SendMsg(int _msg, params object[] _objs)
        {
            if(_m_dBroadcastTable.ContainsKey(_msg))
            {
                //取出一个缓存列表
                List<MsgRecAction> broadcast = null;
                lock(ALMsgRecActionListCache.instance)
                {
                    broadcast = ALMsgRecActionListCache.instance.popItem();
                }

                try
                {
                    broadcast.AddRange(_m_dBroadcastTable[_msg]);
                    for(int i = 0; i < broadcast.Count; ++i)
                    {
                        broadcast[i](_objs);
                    }
                }
                finally
                {
                    lock(ALMsgRecActionListCache.instance)
                    {
                        ALMsgRecActionListCache.instance.pushBackCacheItem(broadcast);
                    }
                }
            }

            List<Action> srcActList;
            if(_m_dBroadcastActTable.TryGetValue(_msg, out srcActList) && srcActList.Count > 0)
            {
                //取出一个缓存列表
                List<Action> broadcast = null;
                broadcast = ALMsgActionListCache.instance.popItem();

                try
                {
                    broadcast.AddRange(srcActList);
                    for(int i = 0; i < broadcast.Count; ++i)
                    {
                        broadcast[i]();
                    }
                }
                finally
                {
                    ALMsgActionListCache.instance.pushBackCacheItem(broadcast);
                }
            }
        }
    }


    /// <summary>
    /// 消息广播管理基类
    /// </summary>
    public abstract class BaseSend<T>
    {
    }
}