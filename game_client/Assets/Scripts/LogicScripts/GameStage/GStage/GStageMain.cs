using UnityEngine;
using System.Collections;
using ALPackage;
using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 游戏主场景对象
    /// </summary>
    public class GStageMain : _ABasicGameStage
    {
        private static GStageMain _g_instance = new GStageMain();
        [NotNull]
        public static GStageMain instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new GStageMain();
                return _g_instance;
            }
        }
        
        protected GStageMain() : base()
        {
        }

        public override ENPUIStageType stageType { get { return ENPUIStageType.MAIN; } }
        /***************
         * 是否需要检查教程
         **/
        public override bool needCheckTutorial { get { return true; } }

        protected override void _onEnterStage()
        {
            //播放背景音乐
            PlayAudioMgr.instance.playBackgroundMusic(1001);

            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(setStageInited);

            //进入主3d场景
            GTDSceneMain.instance.enterScene();
            GTDSceneMain.instance.regInitDelegate(stepCounter.addDoneStepCount);
            //进入主界面
            GUISceneMain.instance.enterScene();
            GUISceneMain.instance.regInitDelegate(stepCounter.addDoneStepCount);
        }

        protected override void _onQuitStage()
        {
            PlayAudioMgr.instance.stopBackgroundMusic();
        }
    }
}
