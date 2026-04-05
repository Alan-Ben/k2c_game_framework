using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using LitJson;

using ALPackage;
using NPEnum;


namespace GOE
{
    /// <summary>
    /// 游戏初始化过程处理函数
    /// </summary>
    public class GameInit_PlatResInit : _AGameInitProcess
    {
        private static GameInit_PlatResInit _g_instance = new GameInit_PlatResInit();
        public static GameInit_PlatResInit instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GameInit_PlatResInit();

                return _g_instance;
            }
        }

        //全体资源加载的进度记录对象
        private ALProcessSubNode _m_pAllResProcess;

        private static readonly string _m_gProcessName = "plat_res_init";
        public static string gProcessName { get { return _m_gProcessName; } }
        
        protected GameInit_PlatResInit()
        {
            _m_pAllResProcess = new ALProcessSubNode();
        }

        /// <summary>
        /// 返回子进度队列
        /// </summary>
        /// <returns></returns>
        protected override _IALProgressnterface[] getChildProcess()
        {
            return new _IALProgressnterface[] {
                _m_pAllResProcess };
        }

        /// <summary>
        /// 是否允许重置，默认允许。
        /// 子类可以重载
        /// 
        /// 平台资源加载不允许重置状态
        /// </summary>
        protected override bool canReset { get { return false; } }

        /// <summary>
        /// 重置数据部分
        /// </summary>
        protected override void _resetData()
        {

        }

        /// <summary>
        /// 加载处理函数
        /// </summary>
        /// <returns></returns>
        protected override void _dealInit(Action _doneDelegate)
        {
            //发送埋点-开始初始化平台资源
            GCommon.sendStepReport(TraceConst.START_INIT_PLAT_RES);

            //使用底层Process进行初始化流程
            ALProcess basicProcess = ALProcess.CreateProcess(_m_gProcessName);

            //首先进行初始化步骤处理
            basicProcess
                //初始化平台部分resCore
                .addDelegateProcess(PlatResCore.instance.init)
                .addDelegateProcess(_initLoginCommonInfo, "common_info_init")
                //初始化语言
                .addProcess(_initGameLanguage)

                .addDelegateProcess(_onPlatResInited, "plat_init")

                //PlatCommonInfo初始化完成后开始初始化音效管理器,否则登陆前音效不会播放，因为监听没有注册
                .addProcess(()=>
                {
                    //发送埋点-初始化音效
                    GCommon.sendStepReport(TraceConst.INIT_PLAT_RES_AUDIO);
                    PlayAudioMgr.instance.init();
                })

                //初始化透明背景缓存
                .addProcess(()=>
                {
                    //发送埋点-初始化音效
                    GCommon.sendStepReport(TraceConst.INIT_PLAT_RES_TRANSPARENTBK);
                    NPUICacheTransparentBk.g_init();
                })

                //进入正常视图
                .addProcess(_finalInitOp)
                .addProcess(()=>
                {
                    //发送埋点-初始化平台资源完成
                    GCommon.sendStepReport(TraceConst.INIT_PLAT_RES_DONE);
                    _doneDelegate?.Invoke();
                });

            //开始执行
            basicProcess.dealProcess(new GameInitMonitor(_m_gProcessName, 1000));
        }


        /// <summary>
        /// 初始化设置游戏语言
        /// </summary>
        /// <param name="_doneDelegate"></param>
        protected static void _initGameLanguage()
        {
            try
            {
                //如果是第一次进入游戏，设置游戏语言为系统语言
                if (GameSetting.instance.IsFirstEnterGame())
                {
                    GameSetting.instance.setIsFirstEnterGame(false);
                    // 第一次进入游戏，自动根据机型设置Quality等级
                    GameSetting.instance.autoSetQuality();
                    //发送埋点-初始化设置游戏画质
                    GCommon.sendStepReport(TraceConst.INIT_SET_QUALITY.setMarkParam(GameSetting.instance.gameQuality.ToString()));
                    
                    //TODO SDK包使用SDK的接口来获取设备语言
                    _setLanguageByUnity();

                    //校验是否支持该语言，不支持设置默认英语
                    if(PLoginCommonInfo.instance.obj == null || 
                       PLoginCommonInfo.instance.obj.supportLanguageList == null || 
                       !PLoginCommonInfo.instance.obj.supportLanguageList.Contains(GameSetting.instance.getCurrentLanguage()))
                        GameSetting.instance.setCurrentLanguage(ENPLanguage.EN_US);
                    
                    if(PLoginCommonInfo.instance.obj == null || 
                       PLoginCommonInfo.instance.obj.supportVoiceLanguageList == null || 
                       !PLoginCommonInfo.instance.obj.supportVoiceLanguageList.Contains(GameSetting.instance.getCurrentVoiceLanguage()))
                        GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.EN_US);
                        
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"无法初始化语言{e}");
            }
            finally
            {
                //语言如果为NONE设置成英语
                if (GameSetting.instance.getCurrentLanguage() == ENPLanguage.NONE)
                {
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.EN_US);
                }

                if (GameSetting.instance.getCurrentVoiceLanguage() == ENPLanguage.NONE)
                {
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.EN_US);
                }

                //发送埋点-初始化设置游戏语言
                GCommon.sendStepReport(TraceConst.INIT_PLAT_RES_LANGUAGE.setMarkParam(GameSetting.instance.getCurrentLanguage()));
            }
        }

        /// <summary>
        /// 根据系统语言设置当前语言,临时用
        /// </summary>
        /// <exception cref="Exception"></exception>
        protected static void _setLanguageByUnity()
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Arabic:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.AR_AR);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.AR_AR);
                    break;
                case SystemLanguage.Chinese:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.ZH_CN);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.ZH_CN);
                    break;
                case SystemLanguage.English:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.EN_US);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.EN_US);
                    break;
                case SystemLanguage.French:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.FR_FR);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.FR_FR);
                    break;
                case SystemLanguage.German:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.DE_DE);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.DE_DE);
                    break;
                case SystemLanguage.Italian:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.IT_IT);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.IT_IT);
                    break;
                case SystemLanguage.Japanese:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.JA_JP);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.JA_JP);
                    break;
                case SystemLanguage.Korean:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.KO_KR);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.KO_KR);
                    break;
                case SystemLanguage.Russian:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.RU_RU);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.RU_RU);
                    break;
                case SystemLanguage.Spanish:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.ES_ES);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.ES_ES);
                    break;
                case SystemLanguage.Turkish:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.TR_TR);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.TR_TR);
                    break;
                case SystemLanguage.ChineseSimplified:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.ZH_CN);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.ZH_CN);
                    break;
                case SystemLanguage.ChineseTraditional:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.ZH_TW);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.ZH_TW);
                    break;
                case SystemLanguage.Unknown:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.EN_US);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.EN_US);
                    break;
                default:
                    GameSetting.instance.setCurrentLanguage(ENPLanguage.EN_US);
                    GameSetting.instance.setCurrentVoiceLanguage(ENPLanguage.EN_US);
                    Debug.LogError($"我们没有支持玩家所在地区语言，默认使用英语，玩家语言是：{Application.systemLanguage.ToString()}");
                    break;
            }
        }

        /******************
         * 平台资源初始化完毕的处理函数
         **/
        protected static void _onPlatResInited(Action _doneDelegate)
        {
            //发送埋点-初始化平台翻译表数据库
            GCommon.sendStepReport(TraceConst.INIT_PLAT_RES_LANGUAGE_SQL);
            //初始化平台翻译表数据库，然后再初始化平台相关信息
            NPPGUILanguageSqlite.instance.initDB(_doneDelegate, GameSetting.instance.getCurrentLanguage());
        }

        /// <summary>
        /// 初始化登录部分的通用资源
        /// </summary>
        /// <param name="_doneDelegate"></param>
        protected static void _initLoginCommonInfo(Action _doneDelegate)
        {
            //发送埋点-初始化登录部分的通用资源
            GCommon.sendStepReport(TraceConst.INIT_PLAT_RES_COMMON_INFO);

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(_doneDelegate);
            //初始化Login的通用数据信息
            PLoginCommonInfo.instance.init(stepCounter.addDoneStepCount, _onPlatInitFail);
            //初始化国家大区数据信息
            PCountryAreaInfo.instance.init(stepCounter.addDoneStepCount, ()=>
            {
                stepCounter.addDoneStepCount();
                _onPlatCountryAreaInitFail();
            });
        }

        protected static void _onPlatInitFail()
        {
            //初始化失败，则调用本函数
            Debug.LogError("Plat Init Fail!");
        }

        protected static void _onPlatCountryAreaInitFail()
        {
            //初始化失败，则调用本函数
            Debug.LogError("Country Area Init Fail!");
        }

        /// <summary>
        /// 根据当前用户情况展示登录界面
        /// 最后登录操作的地方
        /// </summary>
        protected void _finalInitOp()
        {
            //设置进度
            _m_pAllResProcess.setProcess(1f);
        }

        /// <summary>
        /// 设置当前进度
        /// </summary>
        /// <param name="_process"></param>
        protected void _setProcess(float _process)
        {
            _m_pAllResProcess.setProcess(_process);
        }
    }
}
