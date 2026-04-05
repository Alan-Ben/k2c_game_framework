using System.Collections.Generic;
using System;
using UnityEngine;

namespace ALPackage
{
    /// <summary>
    /// 使用对应的多个URL在本对象中进行轮询，并根据处理最终返回结果
    /// 轮询基本只针对文本进行
    /// </summary>
    public class ALURLPostResponseLoopDealer
    {
        /// <summary>
        /// 操作序列号，用于屏蔽不同操作
        /// </summary>
        private int _m_iOpSerialzie;
        private float _m_fOpStartTime;
        private float _m_fOpDealTime;
        /// <summary>
        /// 检测的URL，此变量直接设置外面变量，不去重复创建
        /// </summary>
        private List<ALURLDownloaderURLInfo> _m_lCheckURLList;
        //是否已经开始
        private bool _m_bIsStarted;
        //是否已经开始
        //重试次数
        private int _m_iRetryCount;

        /// <summary>
        /// 已经记录成功的URL
        /// </summary>
        private string _m_sRecordURL;
        //检测的文件名
        private string _m_sSubURL;
        //传递参数
        private IDictionary<string, string> _m_dParamers;

        /// <summary>
        /// 成功和失败的调用
        /// </summary>
        /// //成功第一个参数为对应URL，第二个参数为对应文件内容
        private Action<string, string> _m_dSucDelegate;
        private Action _m_dFailDelegate;
        /// <summary>
        /// 检测结果是否有效的检测函数
        /// </summary>
        private Func<string, bool> _m_fCheckResEnable;

        //单次下载的最大等待处理时间
        private float _m_fMaxWaitTimeS;


        /************ 构造函数 **************************************************************************/
        //"/check"
        /// <summary>
        /// 带入加载文件对应的相对路径进行处理
        /// </summary>
        /// <param name="_fileSubPath"></param>
        public ALURLPostResponseLoopDealer(string _subURL, IDictionary<string, string> _parameters, List<ALURLDownloaderURLInfo> _urlList, float _maxWaitTimeS = 20f)
        {
            _m_iOpSerialzie = ALSerializeOpMgr.next();
            _m_fOpStartTime = 0f;
            _m_fOpDealTime = 0f;
            _m_iRetryCount = 0;
            _m_bIsStarted = false;

            _m_lCheckURLList = _urlList;
            _m_sSubURL = _subURL;
            _m_dParamers = _parameters;

            _m_dSucDelegate = null;
            _m_dFailDelegate = null;
            _m_fCheckResEnable = null;

            _m_fMaxWaitTimeS = _maxWaitTimeS;
        }

        public string recordURL { get { return _m_sRecordURL; } }

        /// <summary>
        /// 获取URL
        /// </summary>
        /// <param name="_getFunc"></param>
        public void checkURL(Action<string, string> _sucDelegate, Action _failDelegate)
        {
            checkURL(_sucDelegate, _failDelegate, null);
        }
        public void checkURL(Action<string, string> _sucDelegate, Action _failDelegate, Func<string, bool> _checkEnable)
        {
            //判断是否在处理过程中
            if (_m_bIsStarted)
            {
                if(null != _failDelegate)
                    _failDelegate();
                return;
            }

            _m_bIsStarted = true;

            //设置重试次数为0
            _m_iRetryCount = 0;

            //设置回调
            _m_dSucDelegate = _sucDelegate;
            _m_dFailDelegate = _failDelegate;
            _m_fCheckResEnable = _checkEnable;

            //开始检测cdn
            _startCheckCDN();
        }

        /// <summary>
        /// 设置访问结果
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_fileInfo"></param>
        protected bool _setFinalURL(int _serialize, string _url, string _mes)
        {
            //序列号不一致不处理
            if(_serialize != _m_iOpSerialzie)
                return false;

            //判断结果是否有效，如果无效则不处理
            if (null != _m_fCheckResEnable && !_m_fCheckResEnable(_mes))
            {
                return false;
            }

            //增加操作序列号
            _m_iOpSerialzie = ALSerializeOpMgr.next();
            _m_fOpStartTime = 0f;
            _m_fOpDealTime = 0f;

            _m_sRecordURL = _url;
            _m_dFailDelegate = null;
            _m_fCheckResEnable = null;
            //处理成功回调
            if (null != _m_dSucDelegate)
            {
                Action<string, string> tempSucDelegate = _m_dSucDelegate;
                _m_dSucDelegate = null;
                tempSucDelegate(_m_sRecordURL, _mes);
            }

            return true;
        }

        /// <summary>
        /// 设置访问失败
        /// </summary>
        protected void _setFail(int _serialize)
        {
            //序列号不一致不处理
            if(_serialize != _m_iOpSerialzie)
                return;

            //判断是否超过重试次数，未超过则重试
            //如果在以检测失败为目的的处理下，失败直接跳过
            if(_m_iRetryCount < 3)
            {
                _m_iRetryCount++;
                //延迟0.5秒
                ALCommonActionMonoTask.addMonoTask(_startCheckCDN, 0.3f);

                return;
            }

            //增加操作序列号
            _m_iOpSerialzie = ALSerializeOpMgr.next();
            _m_fOpStartTime = 0f;
            _m_fOpDealTime = 0f;

            _m_dSucDelegate = null;
            _m_fCheckResEnable = null;
            if (null != _m_dFailDelegate)
            {
                Action tempFailDelegate = _m_dFailDelegate;
                _m_dFailDelegate = null;
                tempFailDelegate();
            }
        }


        /// <summary>
        /// 创建获取第一个CDN地址任务数组，第一个失败不当做全失败
        /// </summary>
        /// <returns></returns>
        protected void _startCheckCDN()
        {
            if(_m_lCheckURLList.Count <= 0)
            {
                _setFail(_m_iOpSerialzie);
                return;
            }

            //增加操作序列号
            _m_iOpSerialzie = ALSerializeOpMgr.next();
            int serialize = _m_iOpSerialzie;
            _m_fOpStartTime = Time.realtimeSinceStartup;
            _m_fOpDealTime = -1f;

            //初始化步骤
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_m_lCheckURLList.Count);
            stepCounter.regAllDoneDelegate(
                () =>
                {
                    _setFail(serialize);
                });

            //处理剩余cdn
            _checkRestCDN(serialize, stepCounter);
        }

        /// <summary>
        /// 检测剩余的cdn，获取最快的一个cdn数据
        /// </summary>
        /// <returns></returns>
        protected void _checkRestCDN(int _serialize, ALStepCounter _stepCounter)
        {
            //判断序列号是否一致，不一致则不进行处理
            if(_serialize != _m_iOpSerialzie)
                return;

            //检测距离当前时间小于0.01秒的所有操作都需要执行
            float deltaTime = Time.realtimeSinceStartup + 0.01f - _m_fOpStartTime;
            //10000秒很大了，当做无效值
            float nearestTime = 10000f;
            //是否有数据在处理
            for (int i = 0; i < _m_lCheckURLList.Count; i++)
            {
                //时间小于deltaTime，但是大于已经处理的时间的则进行处理
                if (_m_lCheckURLList[i].delayTimeS <= deltaTime)
                {
                    //超过已经处理时间节点的为已经处理的
                    if (_m_lCheckURLList[i].delayTimeS > _m_fOpDealTime)
                    {
                        //此处为还未处理的
                        string tmpCheckURL = _m_lCheckURLList[i].url;
                        //开始处理下载操作
                        _checkURL(
                                tmpCheckURL
                                , (string _mes) =>
                                {
                                    //设置最终值，返回设置是否成功
                                    if (_setFinalURL(_serialize, tmpCheckURL, _mes))
                                    {
                                        //重置step
                                        _stepCounter.resetAll();
                                    }
                                    else
                                    {
                                        //设置失败需要执行增加步骤处理
                                        _stepCounter.addDoneStepCount();
                                    }
                                }
                                , (string _error, int _errStatus) =>
                                {
                                    //重置step
                                    _stepCounter.addDoneStepCount();
                                });
                    }
                    else
                    {
                        //已经处理的在else里，不进行处理
                    }
                }
                else
                {
                    //还未处理的，尝试设置最近时间
                    if (_m_lCheckURLList[i].delayTimeS < nearestTime)
                        nearestTime = _m_lCheckURLList[i].delayTimeS;
                }
            }

            //设置已经处理的时间标记
            _m_fOpDealTime = deltaTime;

            //增加一个超时处理,如果1秒无法确认，此时提前开始检测后续CDN，避免等待太久
            //如果是以检测失败为目的则直接开始其他cdn检测
            //判断延时是否有效，无效则需要按照最大超时设置失败
            if (nearestTime < 9999f)
                ALCommonTaskController.CommonActionAddMonoTask(() => { _checkRestCDN(_serialize, _stepCounter); }, nearestTime - (Time.realtimeSinceStartup - _m_fOpStartTime));//使用最接近的时间进行延迟处理
            else
                //增加一个超时处理
                ALCommonTaskController.CommonActionAddMonoTask(() => { _setFail(_serialize); }, _m_fMaxWaitTimeS);
        }



        /// <summary>
        /// 根据URL访问检查对应的URL是否有效
        /// </summary>
        /// <param name="_url"></param>
        /// <param name="_onGetURLDone"></param>
        /// <returns></returns>
        protected void _checkURL(string _url, Action<string> _onSucceed, Action<string, int> _onFailed)
        {
            if (!string.IsNullOrEmpty(_m_sSubURL) && _m_sSubURL[0] == '/')
                _m_sSubURL = _m_sSubURL.Substring(1);

            //构建临时文件路径，执行下载
            ALURLPostDealer.reqPostMsg(ALCommon.urlEndInsure(_url) + _m_sSubURL, _m_dParamers, _onSucceed, _onFailed);
        }
    }
}
