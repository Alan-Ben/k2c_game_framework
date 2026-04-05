using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ALPackage;

namespace GOE
{
    //GameSetting序列化数据
    [System.Serializable]
    public class GameSettingData
    {
        //当前语言
        public ENPLanguage eLang = ENPLanguage.EN_US;
        public ENPLanguage eVoiceLang = ENPLanguage.EN_US;//配音语言
        //是否是第一次进入游戏
        public bool bIsFirstEnterGame = true;
        // 是否使用单位高清资源
        public bool bUsingLowActorClarity = true;
        
        //是否使用音效
        public bool bUsingAudio = true;
        //是否高帧率
        public bool bUsingHigtFrame = false;
        //是否使用背景音乐
        public bool bUsingBgAudio = true;
        //是否使用配音
        public bool bUsingVoice = true;
        //背景音乐音量比例
        public float fBgAudioValue = 1f;
        //游戏音效音量比例
        public float fAudioValue = 1f;
        //游戏配音音量比例
        public float fVoiceValue = 1f;
        // 初始音效组分贝
        public Dictionary<string, float> dInitAudioMixerDB;
        //是否展示卡牌等级
        public bool bShowCardLevel = false;
        //大区Id
        public string iAreaId = "-1";
        //平台id
        public int iPlat = -1;
        
        public Dictionary<string, int> dServerIdDic = new Dictionary<string, int>();//上次登陆的ip对应id，<服务器IP,服务器ID>
        /// <summary>
        /// 最新客户端版本
        /// </summary>
        public string sLastClientVersion;
        //服务器时区
        public int serverTimeZone = 8;
        //夏令时时差
        public int dstOffset = 8;
        
        //是否勾选多次注能
        public bool isSelectedMulitPour;
        
        //上次InjectFix的Tag标记
        public string lastInjectFixVersion;
        //上次cdn的客户端版本号的Tag标记，新版本的后台，version只是同个版本号内唯一
        public string lastCdnClientVersion;
        
        //上次ilruntime的tag标记
        public string lastIlRuntimeVersion;
        //是否同意隐私策略协议
        public bool isAgreePolicy;
        //当前账号是否是白名单
        public bool isWhite;
        //是否使用屏幕点击效果
        public bool usingScreenClickEffect = true;
        //是否使用战斗大招子弹效果
        public bool usingBattlePetSkill = true;
        //游戏画质
        public ENPGameQuality gameQuality = ENPGameQuality.NORMAL;
        //是否降低分辨率
        public bool bIsLowResolution = false;
        //是否已发送激活埋点
        public bool isSendActivate;
        //本地推送总开关
        public bool localPushMainSwitch = true;
        //本地推送开关设置，默认全开
        public long localPushSwitch = long.MaxValue;
        //记录游戏运行时间（分钟）
        public long recordGameRunningTimeMin = 0;
        //游戏运行时间埋点是否已经发送
        public bool IsSendGameRunningTime;
        //视频校验上限
        public long videoMaxLoadPixels;
        //视频校验次数
        public int videoMaxLoadPixelsJudgeCount;
        //商店好评CD开始时间（毫秒）
        public long storeReviewsCDStartTimeMs;
        //是否完成商店好评
        public bool isFinishStoreReviews;
    }
    
    public class GameSetting : _AALBasicSettingInfo
    {
        private static GameSetting _g_instance;
        public static GameSetting instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new GameSetting(_m_sPath);
                return _g_instance;
            }
        }

        private static string _m_sPath = "cachesetting_070927";
        
        private GameSettingData _m_settingData;

        private bool _m_bIsInitResolution = false;
        private Vector2Int _m_vScreenSize;
        private float _m_fLowResolutionScale;

        public GameSetting(string _savePath)
           : base(_savePath)
        {
            _m_settingData = new GameSettingData();
        }
        
        /// <summary>
        /// 服务端时区
        /// </summary>
        public int serverTimeZone { get { return _m_settingData.serverTimeZone; } }
        /// <summary>
        /// 夏令时时差
        /// </summary>
        public int dstOffset { get { return _m_settingData.dstOffset; } }
        /// <summary>
        /// 是否是白名单
        /// </summary>
        public bool isWhite { get { return _m_settingData.isWhite; } }

        /*************
        * 构建需要保存的字符串
         * 
        **/
        protected override string _makeSettingStr()
        {
            return JsonUtility.ToJson(_m_settingData);
        }

        /**************
       * 读取保存的字符串
       **/
        protected override void _initSettingStr(string _infoStr)
        {
            if(string.IsNullOrEmpty(_infoStr))
                return;

            _m_settingData = JsonUtility.FromJson<GameSettingData>(_infoStr);
            
            //一些设置
            QualityMgr.instance.setQualityLevel(_m_settingData.gameQuality);
            _updateIsLowResolution();
            //设置文本语言
            TextEx.g_Lang = _m_settingData.eLang;
        }

        /// <summary>
        /// 是否使用高帧率
        /// </summary>
        public bool usingHighFrame
        {
            get
            {
                if (null == _m_settingData)
                    return false;
                return _m_settingData.bUsingHigtFrame;
            }
            set
            {
                if (null == _m_settingData)
                    return;

                _m_settingData.bUsingHigtFrame = value;
                saveSetting();
            }
        }

        /// <summary>
        /// 是否使用屏幕点击效果
        /// </summary>
        public bool usingScreenClickEffect
        {
            get
            {
                if (null == _m_settingData)
                    return false;
                return _m_settingData.usingScreenClickEffect;
            }
            set
            {
                if (null == _m_settingData)
                    return;

                _m_settingData.usingScreenClickEffect = value;
                saveSetting();
            }
        }

        /// <summary>
        /// 是否使用战斗大招子弹效果
        /// </summary>
        public bool usingBattlePetSkill
        {
            get
            {
                if (null == _m_settingData)
                    return false;
                return _m_settingData.usingBattlePetSkill;
            }
            set
            {
                if (null == _m_settingData)
                    return;

                _m_settingData.usingBattlePetSkill = value;
                saveSetting();
            }
        }

        /// <summary>
        /// 游戏画质
        /// </summary>
        public ENPGameQuality gameQuality
        {
            get
            {
                if (null == _m_settingData)
                    return ENPGameQuality.NORMAL;
                return _m_settingData.gameQuality;
            }
            set
            {
                if (null == _m_settingData)
                    return;

                _m_settingData.gameQuality = value;
                
                _checkLowResolutionForQuality(_m_settingData.gameQuality);
                _updateIsLowResolution();
                QualityMgr.instance.setQualityLevel(_m_settingData.gameQuality);
                saveSetting();
            }
        }

        #region LowResolution

        /// <summary>
        /// 修改分辨率前要先存下屏幕分辨率
        /// </summary>
        private void _checkIsInitResolution()
        {
            if(!_m_bIsInitResolution)
            {
                _m_vScreenSize = new Vector2Int(Screen.width, Screen.height);
                _m_fLowResolutionScale = 0.7778f;
                _m_bIsInitResolution = true;
            }
        }
        
        /// <summary>
        /// 根据当前Quality和分辨率设置是否低分辨率
        /// </summary>
        /// <param name="_quality"></param>
        private void _checkLowResolutionForQuality(ENPGameQuality _quality)
        {
            _checkIsInitResolution();

            if (_m_settingData != null && _quality == ENPGameQuality.VERY_LOW)
            {
                // 如果分辨率高于1700*1080，开启降分辨率
                if(_m_vScreenSize.x * _m_vScreenSize.y > 1700 * 1080)
                    _m_settingData.bIsLowResolution = true;
                else
                    _m_settingData.bIsLowResolution = false;
            }
        }
        
        /// <summary>
        /// 根据GameSetting设置低分辨率
        /// </summary>
        private void _updateIsLowResolution()
        {
            _checkIsInitResolution();
#if UNITY_STANDALONE && !UNITY_EDITOR
#else
            if(lowResolution)
            {
                Screen.SetResolution((int)(_m_vScreenSize.x * _m_fLowResolutionScale), (int)(_m_vScreenSize.y*_m_fLowResolutionScale), Screen.fullScreen);
            }
            else
            {
                Screen.SetResolution(_m_vScreenSize.x, _m_vScreenSize.y, Screen.fullScreen);
            }
#endif
        }
        

        #endregion
        
        /// <summary>
        /// 是否降低分辨率
        /// </summary>
        public bool lowResolution
        {
            get
            {
                if (null == _m_settingData)
                    return false;
                return _m_settingData.bIsLowResolution;
            }
            set
            {
                if (null == _m_settingData)
                    return;

                _m_settingData.bIsLowResolution = value;
                _updateIsLowResolution();
                saveSetting();
            }
        }

        /// <summary>
        /// 自动根据机器信息，设置Quality，仅在第一次进入的时候使用
        /// </summary>
        public void autoSetQuality()
        {
#if UNITY_IOS
            string modelStr = SystemInfo.deviceModel;
            string modelType = modelStr.ToLowerInvariant().Trim().Substring(0, 3);
            if (modelType == "iph")
            {
                // iPhone:	                    iPhone1,1
                // iPhone 3G:	                iPhone1,2
                // iPhone 3GS:	                iPhone2,1
                // iPhone 4:	                iPhone3,1
                //                              iPhone3,2
                //                              iPhone3,3
                // iPhone 4S:	                iPhone4,1
                // iPhone 5:	                iPhone5,1
                //                              iPhone5,2
                // iPhone 5c:	                iPhone5,3
                //                              iPhone5,4
                // iPhone 5s:	                iPhone6,1
                //                              iPhone6,2
                // iPhone 6:	                iPhone7,2
                // iPhone 6 Plus:	            iPhone7,1
                // iPhone 6s:	                iPhone8,1
                // iPhone 6s Plus:	            iPhone8,2
                // iPhone SE (1st generation):	iPhone8,4
                // iPhone 7:	                iPhone9,1
                //                              iPhone9,3
                // iPhone 7 Plus:	            iPhone9,2
                //                              iPhone9,4
                // iPhone 8:	                iPhone10,1
                //                              iPhone10,4
                // iPhone 8 Plus:	            iPhone10,2
                //                              iPhone10,5
                // iPhone X:	                iPhone10,3
                //                              iPhone10,6
                // iPhone XR:	                iPhone11,8
                // iPhone XS:	                iPhone11,2
                // iPhone XS Max:	            iPhone11,6
                //                              iPhone11,4
                // iPhone 11:	                iPhone12,1
                // iPhone 11 Pro:	            iPhone12,3
                // iPhone 11 Pro Max:	        iPhone12,5
                // iPhone SE (2nd generation):	iPhone12,8
                // iPhone 12 mini:	            iPhone13,1
                // iPhone 12:	                iPhone13,2
                // iPhone 12 Pro:	            iPhone13,3
                // iPhone 12 Pro Max:	        iPhone13,4
                // iPhone 13 mini:	            iPhone14,4
                // iPhone 13:	                iPhone14,5
                // iPhone 13 Pro:	            iPhone14,2
                // iPhone 13 Pro Max:	        iPhone14,3
                // iPhone SE (3rd generation):	iPhone14,6
                // iPhone 14:	                iPhone14,7
                // iPhone 14 Plus:	            iPhone14,8
                // iPhone 14 Pro:	            iPhone15,2
                // iPhone 14 Pro Max:	        iPhone15,3

                if (modelStr.Equals("iPhone1,1") 
                    || modelStr.Equals("iPhone1,2")
                    || modelStr.Equals("iPhone2,1")
                    || modelStr.Equals("iPhone3,1")
                    || modelStr.Equals("iPhone3,2")
                    || modelStr.Equals("iPhone3,3")
                    || modelStr.Equals("iPhone4,1")
                    || modelStr.Equals("iPhone5,1")
                    || modelStr.Equals("iPhone5,2")
                    || modelStr.Equals("iPhone5,3")
                    || modelStr.Equals("iPhone5,4")
                    || modelStr.Equals("iPhone6,1")
                    || modelStr.Equals("iPhone6,2")
                    || modelStr.Equals("iPhone7,2")
                    || modelStr.Equals("iPhone7,1")
                    || modelStr.Equals("iPhone8,1")
                    || modelStr.Equals("iPhone8,2")
                    || modelStr.Equals("iPhone8,4")
                    || modelStr.Equals("iPhone9,1")
                    || modelStr.Equals("iPhone9,3")
                    || modelStr.Equals("iPhone9,2")
                    || modelStr.Equals("iPhone9,4")
                ) // iphone7 及以下都设置为Low
                    gameQuality = ENPGameQuality.VERY_LOW;
                // iphone8 、8Plus设置中
                else if (modelStr.Equals("iPhone10,1")// iPhone 8:	                iPhone10,1
                    || modelStr.Equals("iPhone10,4")//                              iPhone10,4
                    || modelStr.Equals("iPhone10,2")// iPhone 8 Plus:	            iPhone10,2
                    || modelStr.Equals("iPhone10,5"))//                              iPhone10,5
                    gameQuality = ENPGameQuality.LOW;
                else if(modelStr.Equals("iPhone10,3") // iPhone X:	                iPhone10,3
                        || modelStr.Equals("iPhone10,6") //                              iPhone10,6
                        || modelStr.Equals("iPhone11,8") // iPhone XR:	                iPhone11,8
                        || modelStr.Equals("iPhone11,2") // iPhone XS:	                iPhone11,2
                        || modelStr.Equals("iPhone11,6")) // iPhone XS Max:	            iPhone11,6
                    gameQuality = ENPGameQuality.NORMAL;
                else if(modelStr.Equals("iPhone12,1") // iPhone 11:	                iPhone12,1
                        || modelStr.Equals("iPhone12,3") // iPhone 11 Pro:	            iPhone12,3
                        || modelStr.Equals("iPhone12,5")// iPhone 11 Pro Max:	        iPhone12,5
                        || modelStr.Equals("iPhone12,8")// iPhone SE (2nd generation):	iPhone12,8
                        || modelStr.Equals("iPhone13,1"))// iPhone 12 mini:	            iPhone13,1
                    gameQuality = ENPGameQuality.HIGH;
                //其他都设置为高
                else 
                    gameQuality = ENPGameQuality.ULTRA; 
            }
            else if (modelType == "ipa")
            {
                //iPad机型
                gameQuality = ENPGameQuality.HIGH;
            }
#elif UNITY_ANDROID
            int processorFrequency = SystemInfo.processorFrequency;
            int systemMemorySize = SystemInfo.systemMemorySize;

            if (processorFrequency > 0)
            {
                float score = 40f;
                if (SystemInfo.graphicsShaderLevel <= 35 || processorFrequency < 1750)
                    score = 0;
                else if (processorFrequency < 2000)
                    score = 20;
                else if (processorFrequency < 2300)
                    score = 40;
                else if (processorFrequency < 3000)
                    score = 60;
                else
                    score = 80;
                
                // 如果内存为2G及以下，则降低两档
                if(systemMemorySize < 2048)
                    score = score - 35;
                
                score = Mathf.Max(score, 0);
                gameQuality = (ENPGameQuality)((int)(5 * score/100f));
            }
            else if(systemMemorySize > 0)
            {
                if (SystemInfo.graphicsShaderLevel <= 35 || systemMemorySize < 2048)
                    gameQuality = ENPGameQuality.VERY_LOW;
                else if (systemMemorySize < 4096)
                    gameQuality = ENPGameQuality.LOW;
                else if (systemMemorySize < 6144)
                    gameQuality = ENPGameQuality.NORMAL;
                else if (systemMemorySize < 8192)
                    gameQuality = ENPGameQuality.HIGH;
                else
                    gameQuality = ENPGameQuality.ULTRA;
            }
            else
            {
                gameQuality = ENPGameQuality.NORMAL;
            }
#else
            gameQuality = ENPGameQuality.ULTRA;
#endif
        }

        /// <summary>
        /// 获取当前游戏语言
        /// </summary>
        /// <returns></returns>
        public ENPLanguage getCurrentLanguage()
        {
            if (null == _m_settingData)
                return ENPLanguage.EN_US;
            
            if(_m_settingData.eLang != ENPLanguage.NONE)
                return _m_settingData.eLang;
            return ENPLanguage.EN_US;
        }
        
        //设置当前语言
        public void setCurrentLanguage(ENPLanguage _lang)
        {
            if(null == _m_settingData || _m_settingData.eLang == _lang)
                return;

            _m_settingData.eLang = _lang;
            TextEx.g_Lang = _lang;

            saveSetting();
        }
        
        /// <summary>
        /// 获取当前配音的语言
        /// </summary>
        /// <returns></returns>
        public ENPLanguage getCurrentVoiceLanguage()
        {
            if (null == _m_settingData)
                return ENPLanguage.EN_US;
            
            if(_m_settingData.eVoiceLang != ENPLanguage.NONE)
                return _m_settingData.eVoiceLang;
            return ENPLanguage.EN_US;//若使用配音语言为ENPLanguage.NONE, 默认使用英语
        }
        /// <summary>
        /// 设置当前配音的语言
        /// </summary>
        /// <param name="_lang"></param>
        public void setCurrentVoiceLanguage(ENPLanguage _lang)
        {
            if(null == _m_settingData || _m_settingData.eVoiceLang == _lang)
                return;

            _m_settingData.eVoiceLang = _lang;

            saveSetting();
        }
        
        /// <summary>
        /// 根据服务器ip获取上次登陆的服务器id
        /// </summary>
        /// <param name="_ip"></param>
        public int getServerIdByIp(string _ip)
        {
            int serverId = 0;
            if (_m_settingData == null ||  _m_settingData.dServerIdDic == null || string.IsNullOrEmpty(_ip))
            {
                if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                    Debug.Log($"【{Time.frameCount}】[Setting] getServerIdByIp: 根据服务器ip获取上次登陆的服务器id，_m_settingData == null ||  _m_settingData.dServerIdDic == null || string.IsNullOrEmpty(_ip)，IP：{_ip}  ServerId:{0}");
                return 0;
            }

            if (_m_settingData.dServerIdDic.ContainsKey(_ip))
                serverId = _m_settingData.dServerIdDic[_ip];
            else
                serverId = 0;
            
            if (_AALMonoMain.instance != null && _AALMonoMain.instance.showDebugOutput)
                Debug.Log($"【{Time.frameCount}】[Setting] getServerIdByIp: 根据服务器ip获取上次登陆的服务器id，IP：{_ip}  ServerId:{serverId}");
            return serverId;
        }
        
        /// <summary>
        /// 设置服务器ID
        /// </summary>
        /// <param name="_ip"></param>
        /// <param name="_serverId"></param>
        public void setServerId(string _ip,int _serverId)
        {
            if(string.IsNullOrEmpty(_ip))
                return;
            if(null == _m_settingData)
                return;
        
            if(_m_settingData.dServerIdDic == null)
                _m_settingData.dServerIdDic = new Dictionary<string, int>();

            if(_m_settingData.dServerIdDic.ContainsKey(_ip))
                _m_settingData.dServerIdDic[_ip] = _serverId;
            else
                _m_settingData.dServerIdDic.Add(_ip, _serverId);

        }

        /// <summary>
        /// 设置是否使用单位高清资源
        /// </summary>
        /// <param name="_isFirstEnterGame"></param>
        public void setIsUsingLowActorClarity(bool _isUsingLowActorClarity)
        {
            if(null == _m_settingData || _m_settingData.bUsingLowActorClarity == _isUsingLowActorClarity)
                return;

            _m_settingData.bUsingLowActorClarity = _isUsingLowActorClarity;

            saveSetting();
        }
        
        /// <summary>
        /// 获取是否使用单位高清资源
        /// </summary>
        /// <returns></returns>
        public bool usingLowActorClarity
        {
            get
            {
                if (null == _m_settingData)
                    return false;
            
                return _m_settingData.bUsingLowActorClarity;
            }
        }
        
        /// <summary>
        /// 设置是否第一次进入游戏
        /// </summary>
        /// <param name="_isFirstEnterGame"></param>
        public void setIsFirstEnterGame(bool _isFirstEnterGame)
        {
            if(null == _m_settingData || _m_settingData.bIsFirstEnterGame == _isFirstEnterGame)
                return;

            _m_settingData.bIsFirstEnterGame = _isFirstEnterGame;

            saveSetting();
        }
        
        /// <summary>
        /// 取是否第一次进入游戏
        /// </summary>
        /// <returns></returns>
        public bool IsFirstEnterGame()
        {
            if (null == _m_settingData)
                return false;
            
            return _m_settingData.bIsFirstEnterGame;
        }

        /// <summary>
        /// 是否使用背景音乐
        /// </summary>
        /// <returns></returns>
        public bool usingBgAudio
        {
            get
            {
                if (null == _m_settingData)
                    return false;
            
                return _m_settingData.bUsingBgAudio;
            }
        }
        
        /// <summary>
        /// 是否使用音效
        /// </summary>
        /// <returns></returns>
        public bool usingAudio
        {
            get
            {
                if (null == _m_settingData)
                    return false;
            
                return _m_settingData.bUsingAudio;
            }
        }
        
        /// <summary>
        /// 是否使用配音
        /// </summary>
        /// <returns></returns>
        public bool usingVoice
        {
            get
            {
                if (null == _m_settingData)
                    return false;
            
                return _m_settingData.bUsingVoice;
            }
        }
        
        /// <summary>
        /// 背景音乐音量比例
        /// </summary>
        /// <returns></returns>
        public float bgAudioValue
        {
            get
            {
                if (null == _m_settingData)
                    return 0f;
            
                return _m_settingData.fBgAudioValue;
            }
        }
        
        /// <summary>
        /// 游戏音效音量比例
        /// </summary>
        /// <returns></returns>
        public float audioValue
        {
            get
            {
                if (null == _m_settingData)
                    return 0f;
            
                return _m_settingData.fAudioValue;
            }
        }
        
        /// <summary>
        /// 游戏配音音量比例
        /// </summary>
        /// <returns></returns>
        public float voiceValue
        {
            get
            {
                if (null == _m_settingData)
                    return 0f;
            
                return _m_settingData.fVoiceValue;
            }
        }
        
        /// <summary>
        /// 是否展示卡牌等级
        /// </summary>
        /// <returns></returns>
        public bool showCardLevel
        {
            get
            {
                if (null == _m_settingData)
                    return false;
            
                return _m_settingData.bShowCardLevel;
            }
        }
        
        /// <summary>
        /// 设置音效相关参数
        /// </summary>
        public void setAudioSettingAfterLoadData()
        {
            if(null == _m_settingData)
                return;
            
            PlayAudioMgr.instance.chgAudioValue(_m_settingData.bUsingAudio, _m_settingData.fAudioValue);
            PlayAudioMgr.instance.chgBgAudioValue(_m_settingData.bUsingBgAudio, _m_settingData.fBgAudioValue);
            PlayAudioMgr.instance.chgVoiceValue(_m_settingData.bUsingVoice, _m_settingData.fVoiceValue);
        }

        /// <summary>
        /// 设置是否使用音效
        /// </summary>
        /// <param name="_isUsing"></param>
        public void setIsUsingAudio(bool _isUsing)
        {
            if (null == _m_settingData)
                return;
            _m_settingData.bUsingAudio = _isUsing;
            setAudioSettingAfterLoadData();
            saveSetting();
        }

        /// <summary>
        /// 设置是否使用配音
        /// </summary>
        /// <param name="_isUsing"></param>
        public void setIsUsingVoice(bool _isUsing)
        {
            if (null == _m_settingData)
                return;
            _m_settingData.bUsingVoice = _isUsing;
            setAudioSettingAfterLoadData();
            saveSetting();
        }

        /// <summary>
        /// 设置音效音量
        /// </summary>
        /// <param name="_value"></param>
        public void setAudioValue(float _value)
        {
            if (null == _m_settingData)
                return;
            _m_settingData.fAudioValue = _value;
            setAudioSettingAfterLoadData();
            saveSetting();
        }

        /// <summary>
        /// 设置配音音量
        /// </summary>
        /// <param name="_value"></param>
        public void setVoiceValue(float _value)
        {
            if (null == _m_settingData)
                return;
            _m_settingData.fVoiceValue = _value;
            setAudioSettingAfterLoadData();
            saveSetting();
        }

        /// <summary>
        /// 获取初始音效组分贝
        /// </summary>
        /// <param name="_audioGroupName"></param>
        /// <param name="_voiceDB"></param>
        /// <returns></returns>
        public bool getInitAudioMixerDB(string _audioGroupName, out float _voiceDB)
        {
            _voiceDB = AudioMixerMgr.InvalidVoiceAudioMixerVolume;
            if (null == _m_settingData || string.IsNullOrEmpty(_audioGroupName))
                return false;

            if (_m_settingData.dInitAudioMixerDB == null)
                _m_settingData.dInitAudioMixerDB = new Dictionary<string, float>();
            
            return _m_settingData.dInitAudioMixerDB.TryGetValue(_audioGroupName, out _voiceDB);
        }
        
        /// <summary>
        /// 设置初始化配音音量
        /// </summary>
        /// <param name="_audioGroupName"></param>
        /// <param name="_voiceDB">音量值</param>
        /// <param name="_voiceRatioValue">音量值百分比</param>
        public void setInitVoiceValueDB(string _audioGroupName, float _voiceDB)
        {
            if(null == _m_settingData || string.IsNullOrEmpty(_audioGroupName))
                return;

            if (_m_settingData.dInitAudioMixerDB == null)
                _m_settingData.dInitAudioMixerDB = new Dictionary<string, float>();
            _m_settingData.dInitAudioMixerDB[_audioGroupName] = _voiceDB;
            
            saveSetting();
        }
        
        /// <summary>
        /// 设置是否使用背景音乐
        /// </summary>
        /// <param name="_isUsing"></param>
        public void setIsUsingBgAudio(bool _isUsing)
        {
            if (null == _m_settingData)
                return;
            _m_settingData.bUsingBgAudio = _isUsing;
            setAudioSettingAfterLoadData();
            saveSetting();
        }

        /// <summary>
        /// 设置背景音乐音量
        /// </summary>
        /// <param name="_value"></param>
        public void setBgAudioValue(float _value)
        {
            if (null == _m_settingData)
                return;
            _m_settingData.fBgAudioValue = _value;
            setAudioSettingAfterLoadData();
            saveSetting();
        }
        
        public void setAreaId(string _areaId, int _plat)
        {
            if(null == _m_settingData)
                return;
            _m_settingData.iAreaId = _areaId;
            _m_settingData.iPlat = _plat;
        }
        
        /// <summary>
        /// 大区Id
        /// </summary>
        /// <returns></returns>
        public string getAreaId()
        {
            if (null == _m_settingData)
                return "-1";
            return _m_settingData.iAreaId;
        }

        public void setLastClientVersion(string _lastClientVersion)
        {
            if (null == _m_settingData)
                return ;
            _m_settingData.sLastClientVersion = _lastClientVersion;
        }

        /// <summary>
        /// 设置是否勾选多次注能
        /// </summary>
        /// <param name="_isSelectedMulitPour"></param>
        public void setMulitPour(bool _isSelectedMulitPour)
        {
            if(null == _m_settingData)
                return;
            _m_settingData.isSelectedMulitPour = _isSelectedMulitPour;
            saveSetting();
        }
        
        /// <summary>
        /// 是否勾选多次注能
        /// </summary>
        /// <returns></returns>
        public bool getMulitPour()
        {
            if (null == _m_settingData)
                return false;
            return _m_settingData.isSelectedMulitPour;
        }
        
        /// <summary>
        /// 设置上次InjectFix的Tag标记
        /// </summary>
        /// <param name="_isSelectedMulitPour"></param>
        public void setLastInjectFixVersion(string _lastInjectFixVersion)
        {
            if(null == _m_settingData)
                return;
            _m_settingData.lastInjectFixVersion = _lastInjectFixVersion;
            saveSetting();
        }
        
        /// <summary>
        /// 上次InjectFix的Tag标记
        /// </summary>
        /// <returns></returns>
        public string getLastInjectFixVersion()
        {
            if (null == _m_settingData)
                return string.Empty;
            return _m_settingData.lastInjectFixVersion;
        }
        
        /// <summary>
        /// 设置上次IlRuntime的Tag标记
        /// </summary>
        /// <param name="_isSelectedMulitPour"></param>
        public void setLastIlRuntimeVersion(string _lastIlRuntimeVersion)
        {
            if(null == _m_settingData)
                return;
            _m_settingData.lastIlRuntimeVersion = _lastIlRuntimeVersion;
            saveSetting();
        }
        
        /// <summary>
        /// 上次IlRuntime的Tag标记
        /// </summary>
        /// <returns></returns>
        public string getLastIlRuntimeVersion()
        {
            if (null == _m_settingData)
                return string.Empty;
            return _m_settingData.lastIlRuntimeVersion;
        }

        /// <summary>
        /// 设置是否同意隐私协议
        /// </summary>
        /// <param name="_isAgreePolicy"></param>
        public void setIsAgreePolicy(bool _isAgreePolicy)
        {
            if (null == _m_settingData)
                return;
            _m_settingData.isAgreePolicy = _isAgreePolicy;
            saveSetting();
        }

        /// <summary>
        /// 是否同意隐私协议
        /// </summary>
        /// <returns></returns>
        public bool isAgreePolicy()
        {
            if (null == _m_settingData)
                return false;
            return _m_settingData.isAgreePolicy;
        }

        /// <summary>
        /// 设置是否是白名单
        /// </summary>
        /// <param name="_isWhite"></param>
        public void setIsWhite(bool _isWhite)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isWhite = _isWhite;
            saveSetting();
        }

        /// <summary>
        /// 设置服务器时区
        /// </summary>
        /// <param name="_serverTimeZone"></param>
        public void setServerTimeZone(int _serverTimeZone)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.serverTimeZone = _serverTimeZone;
            saveSetting();
        }

        /// <summary>
        /// 设置夏令时时差
        /// </summary>
        /// <param name="_dstOffset"></param>
        public void setDstOffset(int _dstOffset)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.dstOffset = _dstOffset;
            saveSetting();
        }
        
        /// <summary>
        /// 设置上次cdn的客户端版本号的Tag标记，新版本的后台，version只是同个版本号内唯一
        /// </summary>
        /// <param name="_isSelectedMulitPour"></param>
        public void setLastCdnClientVersion(string _lastCdnClientVersion)
        {
            if(null == _m_settingData)
                return;
            _m_settingData.lastCdnClientVersion = _lastCdnClientVersion;
            saveSetting();
        }
        
        /// <summary>
        /// 上次cdn的客户端版本号的Tag标记，新版本的后台，version只是同个版本号内唯一
        /// </summary>
        /// <returns></returns>
        public string getLastCdnClientVersion()
        {
            if (null == _m_settingData)
                return string.Empty;
            return _m_settingData.lastCdnClientVersion;
        }

        /// <summary>
        /// 设置是否发送激活埋点
        /// </summary>
        /// <param name="_isSend"></param>
        public void setIsSendActivate(bool _isSend)
        {
            if(null == _m_settingData)
                return;
            _m_settingData.isSendActivate = _isSend;
            saveSetting();
        }

        /// <summary>
        /// 是否发送激活埋点
        /// </summary>
        /// <returns></returns>
        public bool getIsSendActivate()
        {
            if (null == _m_settingData)
                return false;

            return _m_settingData.isSendActivate;
        }

        /// <summary>
        /// 获取本地推送是否打开
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public bool getLocalPushSwitchIsOpen(ELocalPushType _type)
        {
            if (null == _m_settingData)
                return true;

            return (_m_settingData.localPushSwitch & (1 << (int)_type)) != 0;
        }

        /// <summary>
        /// 设置本地推送开关
        /// </summary>
        /// <param name="_type"></param>
        public void setLocalPushSwitch(ELocalPushType _type, bool _isOpen)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.localPushSwitch = _isOpen
                ? _m_settingData.localPushSwitch | (1 << (int) _type)
                : _m_settingData.localPushSwitch & ~(1 << (int) _type);
            saveSetting();
        }

        /// <summary>
        /// 获取本地推送总开关
        /// </summary>
        /// <returns></returns>
        public bool getlocalPushMainSwitchIsOpen()
        {
            if (null == _m_settingData)
                return true;

            return _m_settingData.localPushMainSwitch;
        }

        /// <summary>
        /// 设置本地推送总开关
        /// </summary>
        public void setlocalPushMainSwitch(bool _isOpen)
        {
            if (null == _m_settingData)
                return;

            if (_isOpen)
                _m_settingData.localPushSwitch = long.MaxValue;
            else
                _m_settingData.localPushSwitch = 0;

            _m_settingData.localPushMainSwitch = _isOpen;
            saveSetting();
        }

        /// <summary>
        /// 获取游戏运行时间（分钟）
        /// </summary>
        /// <returns></returns>
        public long getGameRunningTimeMin()
        {
            if (null == _m_settingData)
                return 0;

            return _m_settingData.recordGameRunningTimeMin;
        }

        /// <summary>
        /// 设置增加游戏运行时间
        /// </summary>
        public void addGameRunningTimeMin(long _addMin)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.recordGameRunningTimeMin += _addMin;
            saveSetting();
        }

        /// <summary>
        /// 获取游戏运行时间埋点是否已经发送
        /// </summary>
        /// <returns></returns>
        public bool getIsSendGameRunningTime()
        {
            if (null == _m_settingData)
                return false;

            return _m_settingData.IsSendGameRunningTime;
        }

        /// <summary>
        /// 设置游戏运行时间埋点是否已经发送
        /// </summary>
        public void setIsSendGameRunningTime(bool _isSend)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.IsSendGameRunningTime = _isSend;
            saveSetting();
        }
        
        /// <summary>
        /// 获取视频校验上限
        /// </summary>
        /// <returns></returns>
        public long getVideoMaxLoadPixels()
        {
            if (null == _m_settingData)
                return 0;

            return _m_settingData.videoMaxLoadPixels;
        }

        /// <summary>
        /// 设置视频校验上限
        /// </summary>
        public void setVideoMaxLoadPixels(long _videoMaxLoadPixels)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.videoMaxLoadPixels = _videoMaxLoadPixels;
            saveSetting();
        }
        
        /// <summary>
        /// 视频校验次数
        /// </summary>
        /// <returns></returns>
        public int getVideoMaxLoadPixelsJudgeCount()
        {
            if (null == _m_settingData)
                return 0;

            return _m_settingData.videoMaxLoadPixelsJudgeCount;
        }

        /// <summary>
        /// 增加视频校验次数
        /// </summary>
        public void addVideoMaxLoadPixelsJudgeCount(int _count)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.videoMaxLoadPixelsJudgeCount += _count;
            saveSetting();
        }

        /// <summary>
        /// 获取商店好评CD开始时间（毫秒）
        /// </summary>
        /// <returns></returns>
        public long getStoreReviewsCdStartTimeMs()
        {
            if (null == _m_settingData)
                return 0;

            return _m_settingData.storeReviewsCDStartTimeMs;
        }

        /// <summary>
        /// 设置商店好评CD开始时间（毫秒）
        /// </summary>
        public void setStoreReviewsCdStartTimeMs(long _startTimeMs)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.storeReviewsCDStartTimeMs = _startTimeMs;
            saveSetting();
        }

        /// <summary>
        /// 获取商店好评是否已完成
        /// </summary>
        /// <returns></returns>
        public bool getIsFinishStoreReviews()
        {
            if (null == _m_settingData)
                return false;

            return _m_settingData.isFinishStoreReviews;
        }

        /// <summary>
        /// 设置商店好评已完成
        /// </summary>
        public void setIsFinishStoreReviews(bool _isFinish)
        {
            if (null == _m_settingData)
                return;

            _m_settingData.isFinishStoreReviews = _isFinish;
            saveSetting();
        }
    }
}