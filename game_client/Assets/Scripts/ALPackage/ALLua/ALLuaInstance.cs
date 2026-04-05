using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace ALPackage
{
#if AL_XLUA
    /// <summary>
    /// 单个Lua执行实例对象，包含相关信息的缓存处理
    /// </summary>
    public class ALLuaInstance
    {
        /// <summary>
        /// 本lua运行环境的标记信息
        /// </summary>
        private string _m_sTag;
        /// <summary>
        /// 父对象
        /// </summary>
        private ALLuaManager _m_lmLuaMgr;
        /// <summary>
        /// 本实例中的全局Lua环境
        /// </summary>
        private XLua.LuaEnv _m_leGeneralLuaEnv;

        /// <summary>
        /// 基础构造函数
        /// </summary>
        public ALLuaInstance(string _tag, ALLuaManager _luaMgr)
        {
            _m_sTag = _tag;
            _m_lmLuaMgr = _luaMgr;
            _m_leGeneralLuaEnv = new XLua.LuaEnv();
        }

        public string tag { get { return _m_sTag; } }

        /// <summary>
        /// 创建一个对应env的table
        /// </summary>
        /// <returns></returns>
        public XLua.LuaTable createEnvTable()
        {
            return _m_leGeneralLuaEnv.NewTable();
        }
        
        /// <summary>
        /// 执行全局Lua对象
        /// </summary>
        /// <param name="_generalLua"></param>
        public object[] doLua(string _generalLua)
        {
            if(null == _m_leGeneralLuaEnv)
            {
                UnityEngine.Debug.LogError("Do Lua When Instance is discarded!");
                return null;
            }

            //执行lua脚本
            return _m_leGeneralLuaEnv.DoString(_generalLua);
        }
        public object[] doLua(string _generalLua, string _chunkName = "chunk", XLua.LuaTable _env = null)
        {
            if(null == _m_leGeneralLuaEnv)
            {
                UnityEngine.Debug.LogError("Do Lua When Instance is discarded!");
                return null;
            }

            //执行lua脚本
            return _m_leGeneralLuaEnv.DoString(_generalLua, _chunkName, _env);
        }

        /// <summary>
        /// 每帧执行的释放函数
        /// </summary>
        public void tick()
        {
            if(null == _m_leGeneralLuaEnv)
                return;

            _m_leGeneralLuaEnv.Tick();
        }

        /// <summary>
        /// 释放相关资源
        /// </summary>
        public void discard()
        {
            if(null != _m_leGeneralLuaEnv)
            {
                _m_leGeneralLuaEnv.Dispose();
                _m_leGeneralLuaEnv = null;
            }
        }
    }
#endif
}
