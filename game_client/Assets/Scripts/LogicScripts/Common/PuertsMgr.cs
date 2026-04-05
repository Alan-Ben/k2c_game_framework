using UnityEngine;
using System;
using ALPackage;

namespace GOE
{
#if AL_PUERTS
    public class NPPuertsMgr : ALPuertsManager
    {
        private static NPPuertsMgr _g_instance = null;
        public static NPPuertsMgr instance
        {
            get
            {
                if(null == _g_instance)
                {
                    ALLog.Sys("Get Puerts when it is not Init!");
                }

                return _g_instance;
            }
        }

        /// <summary>
        /// 初始化全局变量，并在完成初始化的时候调用回调
        /// </summary>
        /// <param name="_doneDelegate"></param>
        public static void init(Action _doneDelegate)
        {
            //创建对象
            _g_instance = new NPPuertsMgr(NPRefdataResCore.instance);

            ALStepCounter stepC = new ALStepCounter();
            stepC.chgTotalStepCount(2);
            stepC.regAllDoneDelegate(_doneDelegate);

            //进行初始化处理
            _g_instance.addPuertsScriptAsset("refdata/np_puerts.unity3d", "", stepC.addDoneStepCount
                , () => {
                    ALLog.Error("Init refdata/np_puerts.unity3d fail!!");
                    stepC.addDoneStepCount();
                });
            _g_instance.addPuertsScriptAsset("refdata/np_puerts/ui/main.unity3d", "ui/main", stepC.addDoneStepCount
                , () => {
                    ALLog.Error("Init refdata/np_puerts/ui/main.unity3d fail!!");
                    stepC.addDoneStepCount();
                });
        }

        protected NPPuertsMgr(_AALResourceCore _resCore)
            : base(_resCore, "puerts")
        {

        }
    }
#endif
}
