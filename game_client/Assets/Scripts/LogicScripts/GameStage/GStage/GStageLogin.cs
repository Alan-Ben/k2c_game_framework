using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;

namespace GOE
{
    /**********************
     * 游戏初次进入的舞台对象
     **/
    public class GStageLogin : _AALBasicGameStage
    {
        private static GStageLogin _g_instance = new GStageLogin();
        public static GStageLogin instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new GStageLogin();
                return _g_instance;
            }
        }

        protected GStageLogin()
            : base()
        {
        }

        protected override void _onEnterStage()
        {
            //加载3D场景信息
            PTDSceneNormal.instance.enterScene(PLoginCommonInfo.instance.obj.loginSceneIndex, _onEnterLoadingScene);
        }

        /*******************
         * 在进入加载过程视图后进行的处理
         **/
        private ALStepCounter _m_scEnterCounter = new ALStepCounter();
        protected void _onEnterLoadingScene()
        {
            _m_scEnterCounter.resetStepInfo();
            _m_scEnterCounter.chgTotalStepCount(2);
            _m_scEnterCounter.regAllDoneDelegate(_onStageInitStepDone);

            //显示客户端版本号
            PUIAddSceneVersion.instance.enterScene();
            PUIAddSceneVersion.instance.regInitDelegate(_m_scEnterCounter.addDoneStepCount);

            //进入登录默认Scene
            NPGUISceneLogin.instance.enterScene();
            NPGUISceneLogin.instance.regInitDelegate(_m_scEnterCounter.addDoneStepCount);
        }

        protected override void _onQuitStage()
        {
            //关掉LoginStage时候退出 游戏版本窗口
            PUIAddSceneVersion.instance.quitScene();
        }

        /*********************
         * 在所有初始化完成时的调用函数
         **/
        protected void _onStageInitStepDone()
        {
            setStageInited();
        }
    }
}
