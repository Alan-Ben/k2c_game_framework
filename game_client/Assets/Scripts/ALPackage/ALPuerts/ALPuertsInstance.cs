using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

#if AL_PUERTS
using Puerts;
#endif

namespace ALPackage
{
#if AL_PUERTS
    /// <summary>
    /// 单个Puerts执行实例对象，包含相关信息的缓存处理
    /// </summary>
    public class ALPuertsInstance
    {
        /// <summary>
        /// 本运行环境的标记信息
        /// </summary>
        private string _m_sTag;
        /// <summary>
        /// 父对象
        /// </summary>
        private ALPuertsManager _m_lmPuertsMgr;
        /// <summary>
        /// 本实例中的全局环境
        /// </summary>
        private JsEnv _m_leGeneralPuertsEnv;

        /// <summary>
        /// 基础构造函数
        /// </summary>
        public ALPuertsInstance(string _tag, ALPuertsManager _puertsMgr)
        {
            _m_sTag = _tag;
            _m_lmPuertsMgr = _puertsMgr;
            _m_leGeneralPuertsEnv = new JsEnv(_puertsMgr);
        }

        public string tag { get { return _m_sTag; } }
        public JsEnv jsEnv { get { return _m_leGeneralPuertsEnv; } }

        /// <summary>
        /// 执行对应的语句
        /// </summary>
        /// <param name="_chunkStr"></param>
        public void doScripts(string _chunkStr, string _chunkName = "alP")
        {
            if(null == _m_leGeneralPuertsEnv)
            {
                UnityEngine.Debug.LogError("Do Puerts When Instance is discarded!");
                return ;
            }

            //执行puerts脚本
            try
            {
                _m_leGeneralPuertsEnv.Eval(_chunkStr, _chunkName);
            }
            catch(Exception _ex)
            {
                ALLog.Error($"Deal Puerts Scripts [{_chunkStr}] Error: {_ex.Message}");
            }
        }
        public TResult doScripts<TResult>(string _chunkStr, string _chunkName = "alP")
        {
            if(null == _m_leGeneralPuertsEnv)
            {
                UnityEngine.Debug.LogError("Do Puerts When Instance is discarded!");
                return default(TResult);
            }

            //执行puerts脚本
            try
            {
                //执行puerts脚本
                return _m_leGeneralPuertsEnv.Eval<TResult>(_chunkStr, _chunkName);
            }
            catch (Exception _ex)
            {
                ALLog.Error($"Deal Puerts Scripts [{_chunkStr}] Error: {_ex.Message}");

                return default(TResult);
            }
        }

        /// <summary>
        /// 每帧执行的释放函数
        /// </summary>
        public void tick()
        {
            if(null == _m_leGeneralPuertsEnv)
                return;

            _m_leGeneralPuertsEnv.Tick();
        }

        /// <summary>
        /// 释放相关资源
        /// </summary>
        public void discard()
        {
            if(null != _m_leGeneralPuertsEnv)
            {
                _m_leGeneralPuertsEnv.Dispose();
                _m_leGeneralPuertsEnv = null;
            }
        }
    }
#endif
}
