using System;
using System.Collections.Generic;

using ALPackage;
using UnityEngine;

namespace GOE
{
    /******************
     * 玩家的基础组件对象
     **/
    public abstract class _ANPBasicPlayerComponent
    {
        private NPPlayerComponentMgr _m_pmPlayerCompMgr;

        //是否发送了presend的消息
        private bool _m_bIsDealPresend;
        //是否初始化了
        private bool _m_bIsInited;
        //是否初始化过程完结了
        private bool _m_bIsInitDone;
        //初始化结果
        private bool _m_bIsInitSuc;

        //回调数据
        private Action<bool> _m_aInitDelegate;

        //如果使用PreInit方式，对应存储初始化函数的对象，注意这里只能有一个函数注册
        private Action _m_dPreInitDealAction;

        public _ANPBasicPlayerComponent(NPPlayerComponentMgr _compMgr)
        {
            _m_pmPlayerCompMgr = _compMgr;

            _m_bIsDealPresend = false;
            _m_bIsInited = false;
            _m_bIsInitDone = false;
            _m_bIsInitSuc = false;

            _m_aInitDelegate = default(Action<bool>);

            _m_dPreInitDealAction = null;

            //初始化时则向管理器注册本对象
            if(_m_pmPlayerCompMgr != null)
                _m_pmPlayerCompMgr.regComp(this);
        }

        public NPPlayerComponentMgr compMgr { get { return _m_pmPlayerCompMgr; } }

        public bool isDealPresend { get { return _m_bIsDealPresend; } }
        public bool isInited { get { return _m_bIsInited; } }
        public bool isInitDone { get { return _m_bIsInitDone; } }
        public bool isInitSuc { get { return _m_bIsInitSuc; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public virtual bool canPreInit { get { return false; } }

        /// <summary>
        /// 处理提前发送消息的处理函数
        /// </summary>
        public void dealPresendProtocol()
        {
            if (_m_bIsDealPresend)
                return;

            _m_bIsDealPresend = true;

            //处理发送
            presendInitProtocol();
        }

        /** 初始化 */
        public void init(Action<bool> _doneDelegate)
        {
            if(_m_bIsInited)
            {
                //注册相关回调
                if(_m_bIsInitDone)
                {
                    if(null != _doneDelegate)
                        _doneDelegate(_m_bIsInitSuc);
                }
                else
                {
                    if(null != _doneDelegate)
                        _m_aInitDelegate += _doneDelegate;
                }

                return;
            }

            _m_bIsInited = true;
            //注册回调
            if(null != _doneDelegate)
                _m_aInitDelegate += _doneDelegate;

            //处理初始化操作
            _tryDealInit();
        }

        /// <summary>
        /// 进行PreInit初始化的操作。这个操作会注册一个回调，在依赖项加载完毕之后才进行初始化行为_initAction
        /// </summary>
        /// <param name="_initAction"></param>
        public void dealPreInitFunc(Action _initAction)
        {
            //数据还未初始化只允许注册回调，其他不可以处理
            //这里增加_m_bIsInited的判断，确保先调用了dealInit才能执行实际的初始化行为，如果没有调用dealInit则会先记录回调，等待调用
            if (!GameLoginResLoadController.instance.isResLoadDone || !_m_bIsInited)
            {
                if (null == _m_dPreInitDealAction)
                    _m_dPreInitDealAction = _initAction;
                else
                    _m_dPreInitDealAction += _initAction;

                return;
            }

            //判断是否允许preinit如果不允许则直接调用
            if (!canPreInit)
            {
#if UNITY_EDITOR
                ALLog.Error("在不允许PreInit的模块中进行了dealPreInitFunc操作");
#endif
                try
                {

                    if (null != _initAction)
                    _initAction();
                }
                catch (Exception _ex)
                {
                    UnityEngine.Debug.LogError($"{GetType().ToString()} Init Component Err: {_ex.Message} \n Stack:{_ex.StackTrace}");
                }

                return;
            }

            List<_ANPBasicPlayerComponent> list = WCGSingleton<NPPlayerCompListCache>.instance.popItem();
            try
            {
                //获取需要加载的队列，当队列为空时表示可以直接开始初始化
                _getMustInitCompList(list);

                //判断是否需要加载
                if (list.Count <= 0)
                {
                    //判断是否全部完成
                    //处理初始化操作
                    if (_AALMonoMain.instance.showDebugOutput)
                    {
                        UnityEngine.Debug.Log($"【{Time.frameCount}】[Component] _dealInitAction   {this.compType}");
                    }

                    try
                    {
                        if (null != _initAction)
                            _initAction();
                    }
                    catch (Exception _ex)
                    {
                        UnityEngine.Debug.LogError($"{GetType().ToString()} Init Component Err: {_ex.Message} \n Stack:{_ex.StackTrace}");
                    }

                    return;
                }

                //还有需要初始化的处理，这里直接注册，暂时不输出报错，部分情况下一个组件可能需要多个初始化
//                if(null != _m_dPreInitDealAction)
//                {
//#if UNITY_EDITOR
//                    ALLog.Error("再已经注册了PreInitDealAction的情况下再次注册");
//#endif
//                }

                if (null == _m_dPreInitDealAction)
                    _m_dPreInitDealAction = _initAction;
                else
                    _m_dPreInitDealAction += _initAction;
            }
            finally
            {
                WCGSingleton<NPPlayerCompListCache>.instance.pushBackCacheItem(list);//这里有坑么？只要有一个组件初始化挂了，剩下组件就都不初始化了？
            }
        }

        //释放资源函数
        public void discard()
        {
            //调用子类释放函数
            _discard();

            _m_bIsDealPresend = false;
            _m_bIsInited = false;
            _m_bIsInitDone = false;
            _m_bIsInitSuc = false;

            _m_aInitDelegate = default(Action<bool>);
            _m_dPreInitDealAction = null;

            if(_m_pmPlayerCompMgr != null)
                _m_pmPlayerCompMgr.unregComp(this);

            _m_pmPlayerCompMgr = null;
        }

        public void setInitDone()
        {
            //已经初始化成功则不处理
            if(_m_bIsInitDone && _m_bIsInitSuc)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("初始成功后还调用初始化setInitDone函数");
#endif
                return;
            }

            //设置结束
            _m_bIsInitDone = true;
            _m_bIsInitSuc = true;

            //调用事件函数
            _onInitDone();

            //调用回调
            if(null != _m_aInitDelegate)
                _m_aInitDelegate(_m_bIsInitSuc);
            _m_aInitDelegate = default(Action<bool>);
            _m_dPreInitDealAction = null;
        }

        public void setInitFail()
        {
            //已经初始化成功则不处理
            if(_m_bIsInitDone)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError("初始后还调用初始化setInitFail函数");
#endif
                return;
            }

            //设置结束
            _m_bIsInitDone = true;
            _m_bIsInitSuc = false;

            //调用事件函数
            _onInitFail();

            //调用回调
            if(null != _m_aInitDelegate)
                _m_aInitDelegate(_m_bIsInitSuc);
            _m_aInitDelegate = default(Action<bool>);
            _m_dPreInitDealAction = null;
        }

        /// <summary>
        /// 当所有组件完成之后会调用此函数
        /// 如果组件有特殊刷新需要，如高级公式计算需要。可以在这里进行统一刷新计算处理
        /// </summary>
        public virtual void onAllCompInited()
        {

        }

        /** 初始化所有组件 */
        protected void _tryDealInit()
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
                    //处理初始化操作
                    _dealInit();

                    //判断是否已经注册了preinit处理，已经注册则直接调用
                    if (null != _m_dPreInitDealAction)
                        _m_dPreInitDealAction();
                    _m_dPreInitDealAction = null;
                    return;
                }

                //开启步骤
                ALStepCounter counter = new ALStepCounter();
                counter.resetAll();
                counter.chgTotalStepCount(list.Count);
                counter.regAllDoneDelegate(_tryDealInit);

                //逐个开始加载
                for(int i = 0; i < list.Count; i++)
                {
                    list[i].init((bool _res) => { counter.addDoneStepCount(); });
                }
            }
            finally
            {
                WCGSingleton<NPPlayerCompListCache>.instance.pushBackCacheItem(list);
            }
        }

        /** 获取必须初始化的组件队列 */
        protected void _getMustInitCompList(List<_ANPBasicPlayerComponent> _recList)
        {
            if(null == _recList)
                return;

            //需要加载则进行加载处理，需要对需要加载的队列进行处理
            ENPPlayerCompType[] dependList = dependCompList;
            if(null != dependList)
            {
                //进行加载处理
                _ANPBasicPlayerComponent dependTmp = null;
                for(int n = 0; n < dependList.Length; n++)
                {
                    dependTmp = compMgr.getComp(dependList[n]);
                    if(null == dependTmp)
                        continue;

                    //如此项未加载完成则设置
                    if(!dependTmp.isInitDone)
                        _recList.Add(dependTmp);
                }
            }
        }

        //是否必要型初始化组件
        public abstract bool isMustInit { get; }
        //获取组件类型
        public abstract ENPPlayerCompType compType { get; }
        //依赖的组件队列
        public abstract ENPPlayerCompType[] dependCompList { get; }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public abstract void presendInitProtocol();

        //处理初始化操作
        protected abstract void _dealInit();
        //初始化结果操作
        protected abstract void _onInitDone();
        protected abstract void _onInitFail();
        //释放资源函数
        protected abstract void _discard();
    }
}
