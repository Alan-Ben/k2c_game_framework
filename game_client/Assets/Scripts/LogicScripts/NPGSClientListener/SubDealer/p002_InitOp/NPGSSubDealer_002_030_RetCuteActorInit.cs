using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;

namespace GOE
{
    //Q版形象组件初始化
    public class NPGSSubDealer_002_030_RetCuteActorInit : NPSubDealer<GS2GC.p002_InitOp.GS2GC_002_030_RetCuteActorInit>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p002_InitOp.GS2GC_002_030_RetCuteActorInit _createProtocolObj()
        {
            return new GS2GC.p002_InitOp.GS2GC_002_030_RetCuteActorInit();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p002_InitOp.GS2GC_002_030_RetCuteActorInit _msg)
        {
            NPPlayer.instance.cuteActorComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.cuteActorComp.RetCuteActorInit(_msg);
            });
        }
    }
}
