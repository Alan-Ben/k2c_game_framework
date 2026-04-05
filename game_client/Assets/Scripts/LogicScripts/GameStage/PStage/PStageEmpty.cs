using System;
using System.Collections.Generic;

using UnityEngine;
using ALPackage;


/**********************
 * 游戏初次进入的舞台对象
 **/
public class PStageEmpty : _AALBasicGameStage
{
    private static PStageEmpty _g_instance = new PStageEmpty();
    public static PStageEmpty instance
    {
        get
        {
            if (null == _g_instance)
                _g_instance = new PStageEmpty();
            return _g_instance;
        }
    }

    protected PStageEmpty()
        : base()
    {
    }

    protected override void _onEnterStage()
    {
        ALCommonActionMonoTask.addNextFrameTask(setStageInited);
    }

    protected override void _onQuitStage()
    {
    }
}
