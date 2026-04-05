using UnityEngine;
using System.Collections;
using ALPackage;
using System;

namespace GOE
{
    /// <summary>
    /// UIStage 的泛型模板类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class _ABasicGameStage : _AALBasicGameStage
    {
        /**************
         * 获取当前所在的场景类型
         **/
        public static ENPUIStageType curWCGGameStageType
        {
            get
            {
                if(!(ALGameStageCore.instance.curGameStage is _ABasicGameStage))
                    return ENPUIStageType.NONE;

                return ((_ABasicGameStage)ALGameStageCore.instance.curGameStage).stageType;
            }
        }

        protected _ABasicGameStage()
            : base()
        {
        }

        /****************
         * 当前stage类型
        **/
        public abstract ENPUIStageType stageType { get; }
        /***************
         * 是否需要检查教程
         **/
        public abstract bool needCheckTutorial { get; }
    }
}
