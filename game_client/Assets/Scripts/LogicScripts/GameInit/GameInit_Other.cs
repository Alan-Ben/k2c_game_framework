using ALPackage;
using System;
using UnityEngine.Video;

namespace GOE
{
    /// <summary>
    /// 游戏初始化过程处理函数
    /// </summary>
    public class GameInit_Other
    {
        private static GameInit_Other _g_instance = new GameInit_Other();
        public static GameInit_Other instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GameInit_Other();

                return _g_instance;
            }
        }

        //是否开始
        private bool _m_bIsStarted;
        //完成状态回调管理器
        private ALCommonStateDelegate _m_dDelegate;
        //游戏前公告展示下标
        private int _m_iGameBeforeNoticeIndex = 0;

        private static readonly string _m_gProcessName = "GameInit_Other";
        public static string gProcessName { get { return _m_gProcessName; } }
        
        protected GameInit_Other()
        {
            _m_dDelegate = new ALCommonStateDelegate();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void init()
        {
            init(null);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void init(Action _doneDelegate)
        {
            //注册回调
            _m_dDelegate.regDelegate(_doneDelegate);

            //已经开始则不处理
            if (_m_bIsStarted)
                return;

            _m_bIsStarted = true;

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(1);
            stepCounter.regAllDoneDelegate(() =>
            {
                //设置完成
                _m_dDelegate.setInitDone();
            });

            ALGlobalControl.instance.onVideoErrorReceived += _onVideoErrorReceived;
            
            //展示游戏前公告
            _showGameBeforeNotice(stepCounter.addDoneStepCount);
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void reset()
        {
            _m_bIsStarted = false;
            _m_iGameBeforeNoticeIndex = 0;
            _m_dDelegate.reset();
        }

        //有视频播放错误都发送埋点
        private void _onVideoErrorReceived(_IALVideoResource _resource, _AALVideoPlayerDealer _vp, string _msg)
        {
            if(null == _vp)
                return;
            
            Debug.LogError_EditorOnly($"视频播放错误：视频资源：{_resource?.resourcePath}，错误消息：{_msg}");
            
            //发送埋点
            GCommon.sendStepReport(TraceConst.VIDEO_PLAY_ERROR.setMarkParam(_resource?.resourcePath, _msg));
        }
        
        /// <summary>
        /// 展示游戏前公告
        /// </summary>
        /// <param name="_doneDelegate"></param>
        private void _showGameBeforeNotice(Action _doneDelegate)
        {
            //没走cdn直接处理
            if (!Game.instance.isUseCdn)
            {
                if (_doneDelegate != null)
                    _doneDelegate();
                return;
            }

            //游戏前公告需要先等待cdn特别是大区初始化完
            ALProcess processObj = ALProcess.CreateProcess("showGameBeforeNotice");
            processObj
                .addDelegateProcess(GameInit_CDN.instance.init)
                .addProcess(() =>
                {
                    CDNSetting_GameBeforeNoticeInfo.instance.requestData(_noticeInfo =>
                    {
                        //判断数据是否有效
                        if (null == _noticeInfo || _noticeInfo.Count <= 0)
                        {
                            if (_doneDelegate != null)
                                _doneDelegate();
                            return;
                        }

                        //显示索引
                        if (_m_iGameBeforeNoticeIndex < _noticeInfo.Count)
                        {
                            GameBeforeNoticeInfo tmpInfo = _noticeInfo[_m_iGameBeforeNoticeIndex];

                            //是否有效，并且是否自动打开
                            if (null != tmpInfo && tmpInfo.inValid() && tmpInfo.is_auto_open != 0)
                            {
                                //获取语言公告
                                GameNoticeContent content = tmpInfo.getLanguage(GameSetting.instance.getCurrentLanguage());
                                //展示公告
                                if (null != content)
                                {
                                    NPGUIAddSceneSingleWndScene oneBtnScene = new NPGUIAddSceneSingleWndScene(NPPGUIWndGameBeforeNotice.instance, false);
                                    oneBtnScene.regInitDelegate(() =>
                                    {
                                        NPPGUIWndGameBeforeNotice.instance.setInfo(content.title, content.content, TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                                            () =>
                                            {
                                        //增加索引
                                        _m_iGameBeforeNoticeIndex++;
                                                _showGameBeforeNotice(_doneDelegate);

                                                oneBtnScene.quitScene();
                                            }, () =>
                                            {
                                                oneBtnScene.quitScene();
                                                if (null != _doneDelegate)
                                                    _doneDelegate();
                                            });
                                    });
                                    oneBtnScene.enterScene();
                                    return;
                                }
                            }
                            //增加索引
                            _m_iGameBeforeNoticeIndex++;
                            _showGameBeforeNotice(_doneDelegate);
                        }
                        else
                        {
                            if (null != _doneDelegate)
                                _doneDelegate();
                        }
                    });
                })
                .deal();

            
        }
    }
}
