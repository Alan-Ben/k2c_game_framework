using System;
using System.Text;
using System.Collections.Generic;

using ALPackage;

namespace GOE
{
    /*************
     * 玩家各组件管理对象
     **/
    public class NPPlayerComponentMgr
    {
        //组件队列
        private List<_ANPBasicPlayerComponent> _m_lCompList;

        //用于总控加载的none组件
        private NPNoneComponent _m_ncNoneComp;

        public NPPlayerComponentMgr()
        {
            _m_lCompList = new List<_ANPBasicPlayerComponent>();

            //初始化none组件
            _m_ncNoneComp = new NPNoneComponent(this);
        }

        /// <summary>
        /// 当所有组件初始化完成之后调用的函数
        /// </summary>
        public void onAllCompInited()
        {
            _ANPBasicPlayerComponent tmpComp = null;
            for (int i = 0; i < _m_lCompList.Count; i++)
            {
                tmpComp = _m_lCompList[i];
                if (null == tmpComp)
                    continue;

                //处理每个子组件的操作
                tmpComp.onAllCompInited();
            }
        }

        /** 注册一个玩家组件 */
        public void regComp(_ANPBasicPlayerComponent _comp)
        {
            if(null == _comp)
                return;

            int idx = (int)_comp.compType;
            //添加组件
            while(_m_lCompList.Count <= idx)
            {
                _m_lCompList.Add(null);
            }

            //如果已经有对象则报错
            if(null != _m_lCompList[idx])
            {
                ALLog.Crush("重复注册组件：" + _comp.compType);
                return;
            }

            //设置组件对象
            _m_lCompList[idx] = _comp;
        }

        /** 注销一个玩家组件 */
        public void unregComp(_ANPBasicPlayerComponent _comp)
        {
            if(null == _comp)
                return;

            int idx = (int)_comp.compType;
            //添加组件
            if(_m_lCompList.Count <= idx)
            {
                return;
            }

            //如果已经有对象则报错
            if(null == _m_lCompList[idx])
            {
                return;
            }

            if(_m_lCompList[idx] != _comp)
            {
                ALLog.Crush("注销组件不一致：" + _comp.compType);
                return;
            }

            //设置组件对象
            _m_lCompList[idx] = null;
        }

        //初始化所有必须加载的组件
        public void initMustComp(Action<bool> _doneDelegate)
        {
            //直接进行none的初始化即可
            _m_ncNoneComp.init(_doneDelegate);
        }

        /** 初始化所有组件 */
        public bool __initMustComp(Action _doneAction)
        {
            List<_ANPBasicPlayerComponent> list = WCGSingleton<NPPlayerCompListCache>.instance.popItem();

            try
            {
                //获取队列
                _getMustInitCompList(list);

                //判断是否需要加载
                if(list.Count <= 0)
                {
                    //判断是否全部完成
                    if(!_isAllMustInited())
                    {
                        ALLog.Crush("有必须加载的未加载完成，请检查依赖关系！！！");
                        ALLog.Crush("有必须加载的未加载完成，请检查依赖关系！！！");
                        ALLog.Crush("有必须加载的未加载完成，请检查依赖关系！！！");
                    }
                    return true;
                }

                //开启步骤
                ALStepCounter counter = new ALStepCounter();
                counter.resetAll();
                counter.chgTotalStepCount(list.Count);
                counter.regAllDoneDelegate(_doneAction);

                //逐个开始加载
                for(int i = 0; i < list.Count; i++)
                {
                    list[i].init((bool _res) => { counter.addDoneStepCount(); });
                }

                return false;
            }
            finally
            {
                WCGSingleton<NPPlayerCompListCache>.instance.pushBackCacheItem(list);
            }
        }

        /// <summary>
        /// 提前发送初始化消息协议，回的消息不会立即处理。需要等待init调用后才可处理
        /// </summary>
        public void __presendInitProtocol()
        {
            //逐个开始加载
            for (int i = 0; i < _m_lCompList.Count; i++)
            {
                //判断是否允许preinit，如不允许则不处理，同时如果已经执行了初始化操作，也不会进行类似操作
                _ANPBasicPlayerComponent comp = _m_lCompList[i];
                if (null == comp || !comp.canPreInit || comp.isDealPresend)
                    continue;

                comp.dealPresendProtocol();
            }
        }

        /// <summary>
        /// 处理所有组件的PreInit操作，发送消息进行相关处理
        /// </summary>
        /// <param name="_doneAction"></param>
        /// <param name="_progressAction"></param>
        /// <returns></returns>
        public void __preInitAllComp()
        {
            //逐个开始加载
            for(int i = 0; i < _m_lCompList.Count; i++)
            {
                //判断是否允许preinit，如不允许则不处理，同时如果已经执行了初始化操作，也不会进行类似操作
                if(null == _m_lCompList[i] || !_m_lCompList[i].canPreInit || _m_lCompList[i].isInited)
                    continue;

                _m_lCompList[i].init(null);
            }
        }

        /** 释放所有数据 */
        public void discard()
        {
            //逐个开始加载
            for(int i = 0; i < _m_lCompList.Count; i++)
            {
                if(null == _m_lCompList[i])
                    continue;

                _m_lCompList[i].discard();
            }

            _m_lCompList.Clear();

            //初始化none组件
            _m_ncNoneComp.discard();
            _m_ncNoneComp = null;
        }

#if UNITY_EDITOR || UNITY_STANDALONE
        /** 获取必须初始化的组件队列 */
        public string getAllUnInitComp()
        {
            StringBuilder builder = new StringBuilder();

            _ANPBasicPlayerComponent tmpObj = null;
            //逐个筛选
            for(int i = 0; i < _m_lCompList.Count; i++)
            {
                tmpObj = _m_lCompList[i];
                if(null == tmpObj)
                    continue;

                //判断是否必须加载
                if(!tmpObj.isInitDone && tmpObj.compType != ENPPlayerCompType.NONE)
                {
                    if(builder.Length > 0)
                        builder.Append(" & ");
                    builder.Append(tmpObj.compType.ToString());
                }
            }

            return builder.ToString();
        }
        /** 获取必须初始化的组件队列 */
        public void forceInited()
        {
            _ANPBasicPlayerComponent tmpObj = null;
            //逐个筛选
            for(int i = 0; i < _m_lCompList.Count; i++)
            {
                tmpObj = _m_lCompList[i];
                if(null == tmpObj)
                    continue;

                //判断是否必须加载
                if(!tmpObj.isInitDone && tmpObj.compType != ENPPlayerCompType.NONE)
                {
                    tmpObj.setInitDone();
                }
            }
        }
#endif

        /** 获取组件对象 */
        public _ANPBasicPlayerComponent getComp(ENPPlayerCompType _compType)
        {
            return _getComp(_compType);
        }
        protected _ANPBasicPlayerComponent _getComp(ENPPlayerCompType _compType)
        {
            int idx = (int)_compType;

            if(idx >= _m_lCompList.Count)
                return null;

            return _m_lCompList[idx];
        }

        /** 获取必须初始化的组件队列 */
        protected void _getMustInitCompList(List<_ANPBasicPlayerComponent> _recList)
        {
            if(null == _recList)
                return;

            _ANPBasicPlayerComponent tmpObj = null;
            //逐个筛选
            for(int i = 0; i < _m_lCompList.Count; i++)
            {
                tmpObj = _m_lCompList[i];
                if(null == tmpObj)
                    continue;

                //判断是否必须加载
                if(!tmpObj.isMustInit || tmpObj.isInitDone)
                    continue;

                //是否有依赖需要加载
                bool hasDepend = false;

                //需要加载则进行加载处理，需要对需要加载的队列进行处理
                ENPPlayerCompType[] dependList = tmpObj.dependCompList;
                if(null != dependList)
                {
                    //进行加载处理
                    _ANPBasicPlayerComponent dependTmp = null;
                    for(int n = 0; n < dependList.Length; n++)
                    {
                        dependTmp = _getComp(dependList[n]);
                        if(null == dependTmp)
                            continue;

                        //开始进行加载处理
                        if(!dependTmp.isMustInit)
                        {
                            //依赖一个非必须加载项则报错
                            ALLog.Crush("一个必须加载模块：" + tmpObj.compType + " 依赖了一个非必须加载模块：" + dependTmp.compType);
                            continue;
                        }

                        //如此项未加载完成则设置
                        if(!dependTmp.isInitDone)
                            hasDepend = true;
                    }
                }

                //判断是否有依赖项
                if(hasDepend)
                    continue;

                //加入队列
                _recList.Add(tmpObj);
            }
        }

        /** 判断是否所有必须加载项都加载完成了 */
        protected bool _isAllMustInited()
        {
            _ANPBasicPlayerComponent tmpObj = null;
            //逐个筛选
            for(int i = 0; i < _m_lCompList.Count; i++)
            {
                tmpObj = _m_lCompList[i];
                if(null == tmpObj)
                    continue;

                //判断是否必须加载
                if(!tmpObj.isMustInit)
                    continue;
                if(!tmpObj.isInitDone)
                    return false;
            }

            return true;
        }
    }
}
