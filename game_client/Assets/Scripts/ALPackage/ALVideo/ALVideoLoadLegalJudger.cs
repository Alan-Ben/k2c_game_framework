using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

namespace ALPackage
{
    /// <summary>
    /// 针对AVPro2加载视频的特殊判断处理对象
    /// 由于在部分机型上，预加载超过一定分辨率的视频会导致加载卡死
    /// 因此在本对象会进行预加载分析和处理，而后续在加载视频的时候都会针对可能的瓶颈进行判断和处理
    /// 
    /// 本处理器的判断逻辑是不断加载视频，当达到上限或者加载耗时过长会停止加载，并记录最大可用加载数量
    /// </summary>
    public class ALVideoLoadLegalJudger
    {
        private static ALVideoLoadLegalJudger _g_instance = new ALVideoLoadLegalJudger();
        public static ALVideoLoadLegalJudger instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new ALVideoLoadLegalJudger();
                return _g_instance;
            }
        }

        //最低加载耗时，一般后续加载耗时不能超过最低加载耗时的3倍，如超过3倍则视为已经到达瓶颈
        private float _m_fMinLoadDurationS;
        //已经加载的队列，用于后续释放
        private List<ALVideoPlayerClipObj> _m_lTmpLoadList;
        //是否在测试加载，这样避免因为既有参数影响测试结果
        private bool _m_bIsTesting;
        //检测操作序列号
        private long _m_lTestSerialize;
        //检测结束的回调
        private Action<long> _m_dTestFinalDelegate;

        //最大可加载像素的安全加载比例，建议是按照0.7比例走
        private float _m_fMaxLoadSafeRate;
        //最大视频的宽度和高度，一般需要冗余3个视频作为安全缓冲。避免交叉加载的时候出现问题
        private int _m_iMaxVCWidth;
        private int _m_iMaxVCHeight;
        //最大可加载像素数量，一般取极限值的70%作为安全值，超出安全值得prepare不会进行处理，而超出安全值得视频停止播放后都会释放
        private long _m_lMaxLoadPixels;

        //当前已经加载的像素数量
        private long _m_lCurLoadPixels;

        protected ALVideoLoadLegalJudger()
        {
            _m_fMinLoadDurationS = -1f;
            _m_lTmpLoadList = new List<ALVideoPlayerClipObj>();
            _m_bIsTesting = false;
            _m_lTestSerialize = ALSerializeOpMgr.next();
            _m_dTestFinalDelegate = default(Action<long>);

            _m_lMaxLoadPixels = 0;

            _m_fMaxLoadSafeRate = 0f;
            _m_iMaxVCWidth = 0;
            _m_iMaxVCHeight = 0;
            _m_lMaxLoadPixels = 0;

            _m_lCurLoadPixels = 0;
        }

        /// <summary>
        /// 直接初始化最大加载像素数量，一般用于外部存储了最大加载像素数量的情况下直接使用
        /// </summary>
        /// <param name="_maxLoadPixels"></param>
        public void initMaxLoadPixels(long _maxLoadPixels)
        {
            _m_lMaxLoadPixels = _maxLoadPixels;
        }

        /// <summary>
        /// 开始尝试加载测试承压情况，一般最多加载到16个左右就足够了
        /// </summary>
        /// <param name="_finalAct">最终返回结算的回调处理，回调带入-1表示被取消</param>
        /// <param name="_maxTryCount">最多尝试加载的数量，一般结合实际视频分辨率来做决定</param>
        public void tryLoadCount(_IALVideoResource _vcRes, Action<long> _finalAct, float _safeRate = 0.7f, int _maxVCWidth = 0, int _maxVCHeight = 0, int _maxTryCount = 32)
        {
            if(_m_bIsTesting)
            {
                ALLog.Sys($"Try start Test Load Count when test is already running!");
                if(null != _finalAct)
                    _m_dTestFinalDelegate += _finalAct;
                return;
            }

            //清理资源
            _resetVariables();

            //设置相关变量
            _m_fMaxLoadSafeRate = _safeRate;
            _m_iMaxVCWidth = _maxVCWidth;
            _m_iMaxVCHeight = _maxVCHeight;

            //设置加载中
            _m_bIsTesting = true;
            _m_lTestSerialize = ALSerializeOpMgr.next();

            //注册回调
            if(null != _finalAct)
                _m_dTestFinalDelegate += _finalAct;

            //调用尝试加载的处理函数
            _testRepeatLoad(_m_lTestSerialize, _vcRes, 1, _maxTryCount);
        }

        /// <summary>
        /// 判断当前是否需要释放视频资源
        /// 当已加载超出最大值时需要释放
        /// </summary>
        /// <returns></returns>
        public bool judgeNeedDisposeVideo()
        {
            //测试期间都不需要释放
            if (_m_bIsTesting)
                return false;

            //最大值无效不需要释放
            if (_m_lMaxLoadPixels <= 0)
                return false;

            //如果当前加载值未超过最大值，则不需要释放
            if (_m_lCurLoadPixels <= _m_lMaxLoadPixels)
                return false;

            //此时需要释放
            return true;
        }

        /// <summary>
        /// 判断当前是否允许进行预加载，如果不允许则所有非forcePrepare的操作都会直接返回
        /// </summary>
        /// <returns></returns>
        public bool judgeCanPrepareVideo()
        {
            //测试期间都不需要释放
            if (_m_bIsTesting)
                return true;

            //最大值无效不需要释放
            if (_m_lMaxLoadPixels <= 0)
                return true;

            //如果当前加载值未超过最大值，则不需要释放
            if(_m_lCurLoadPixels <= _m_lMaxLoadPixels)
                return true;

            //此时不允许进行预加载
            return false;
        }


        /// <summary>
        /// 取消处理
        /// </summary>
        /// <param name="_finalAct"></param>
        public void cancelTest()
        {
            //最后释放的时候可能会在回调调用本函数，此时不应处理
            if (!_m_bIsTesting)
                return;

            //设置加载中
            _m_bIsTesting = false;
            //修改序列号
            _m_lTestSerialize = ALSerializeOpMgr.next();

            //清理内存，这里必须后清理确保数据没有被更改
            _resetVariables();

            //调用回调
            Action<long> tmpDelegate = _m_dTestFinalDelegate;
            _m_dTestFinalDelegate = default(Action<long>);
            if (null != tmpDelegate)
                tmpDelegate(-1);
        }

        /// <summary>
        /// 在视频准备完成后调用的处理函数
        /// </summary>
        protected internal void _onVideoPrepared(_AALVideoPlayerDealer _clipObj)
        {
            if (null == _clipObj)
                return;

            //累加已加载总数
            _m_lCurLoadPixels += _clipObj.videoWidth * _clipObj.videoHeight;
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning($"on video prepared use [{_m_lCurLoadPixels}]s");
#endif
        }

        /// <summary>
        /// 在视频重置后调用的处理函数
        /// </summary>
        protected internal void _onVideoReset(_AALVideoPlayerDealer _clipObj)
        {
            if (null == _clipObj)
                return;

            //累加已加载总数
            _m_lCurLoadPixels -= _clipObj.videoWidth * _clipObj.videoHeight;
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning($"on video reset use [{_m_lCurLoadPixels}]s");
#endif
        }

        /// <summary>
        /// 重置相关资源及参数，确保资源不泄露
        /// </summary>
        private void _resetVariables()
        {
            _m_fMinLoadDurationS = -1f;
            for (int i = 0; i < _m_lTmpLoadList.Count; i++)
            {
                _m_lTmpLoadList[i]?.discard();
            }
            _m_lTmpLoadList.Clear();
        }

        /// <summary>
        /// 尝试预加载单个视频资源
        /// </summary>
        /// <param name="_vcRes"></param>
        /// <param name="_onPrepareDone"></param>
        private void _testPrepareVideo(_IALVideoResource _vcRes, Action<ALVideoPlayerClipObj, bool, string> _onPrepareDone)
        {
            if(null == _vcRes)
            {
                if(null != _onPrepareDone)
                    _onPrepareDone(null, false, "VCResourece is null!");
                return;
            }

            ALVideoPlayerClipObj clipObj = new ALVideoPlayerClipObj(_vcRes, VideoAudioOutputMode.None, null);
            //放入临时队列
            _m_lTmpLoadList.Add(clipObj);
            //用准备函数
            clipObj.prepare((bool _isSuc, string _err)=>_onPrepareDone(clipObj, _isSuc, _err));
        }

        /// <summary>
        /// 尝试重复加载，不断尝试总负载上限
        /// </summary>
        /// <param name="_idx"></param>
        /// <param name="_maxIdx"></param>
        /// <param name="_finalAct"></param>
        private void _testRepeatLoad(long _testSerialize, _IALVideoResource _vcRes, int _idx, int _maxIdx)
        {
            if (_idx > _maxIdx)
            {
                //进行结果结算
                _onTestResult();
                return;
            }

            //检测序列号不一致则不继续处理，回调会在取消的时候直接处理
            if (_testSerialize != _m_lTestSerialize)
                return;

            float startTime = Time.realtimeSinceStartup;
            _testPrepareVideo(_vcRes
               , (ALVideoPlayerClipObj _vcObj, bool _isSuc, string _err) =>
               {
                   //检测序列号不一致则不继续处理，回调会在取消的时候直接处理
                   if (_testSerialize != _m_lTestSerialize)
                       return;

                   //如果直接失败，则直接返回
                   if (!_isSuc)
                   {
                       //直接结算
                       _onTestResult();
                       return;
                   }

                   //UnityEngine.Debug.LogError($"load idx[{_idx}] use [{Time.realtimeSinceStartup - startTime}]s");
                   float curDuration = Time.realtimeSinceStartup - startTime;
                   if (_m_fMinLoadDurationS < 0)
                   {
                       //如时间还未设置，以当次时间为准
                       _m_fMinLoadDurationS = curDuration;
                   }
                   else
                   {
                       if(curDuration < _m_fMinLoadDurationS)
                       {
                           //如果加载时间更短，以最短计算
                           _m_fMinLoadDurationS = curDuration;
                       }
                       else if (curDuration > _m_fMinLoadDurationS * 2f)
                       {
                           //超出阈值，直接停止测试，计算结果
                           //直接结算
                           _onTestResult();
                           return;
                       }
                       else
                       {
                           if(null == _vcObj._texture)
                           {
                               //直接结算
                               _onTestResult();
                               return;
                           }
                       }
                   }

                   _testRepeatLoad(_testSerialize, _vcRes, _idx + 1, _maxIdx);
               });
        }

        /// <summary>
        /// 统计出结果时调用的结果处理函数
        /// </summary>
        /// <param name="_finalAct"></param>
        private void _onTestResult()
        {
            //最后释放的时候可能会在回调调用本函数，此时不应处理
            if (!_m_bIsTesting)
                return;

            //设置加载中
            _m_bIsTesting = false;
            //重置序列号
            _m_lTestSerialize = ALSerializeOpMgr.next();

            //UnityEngine.Debug.LogError($"test max Load [{_m_lCurLoadPixels}]s");
            //注意使用double计算，避免溢出
            _m_lMaxLoadPixels = (long)((double)_m_lCurLoadPixels * (double)_m_fMaxLoadSafeRate);

            //计算扣除3个最大视频尺寸后的安全加载值
            long tmpSafePixels = _m_lCurLoadPixels - (3 * (long)((long)_m_iMaxVCWidth * (long)_m_iMaxVCHeight));
            if (tmpSafePixels <= 0)
                tmpSafePixels = _m_lMaxLoadPixels;

            //两者取最小值
            _m_lMaxLoadPixels = Math.Min(_m_lMaxLoadPixels, tmpSafePixels);

            //清理内存，这里必须后清理确保数据没有被更改
            _resetVariables();

            //调用回调
            Action<long> tmpDelegate = _m_dTestFinalDelegate;
            _m_dTestFinalDelegate = default(Action<long>);
            if (null != tmpDelegate)
                tmpDelegate(_m_lMaxLoadPixels);
        }
    }
}
