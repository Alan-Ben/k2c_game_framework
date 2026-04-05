using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;

namespace GOE
{
    //获取玩家信息
    public class NPGSSubDealer_002_014_RetMailStatInfo : NPSubDealer<GS2GC.p002_InitOp.GS2GC_002_014_RetMailStatInfo>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p002_InitOp.GS2GC_002_014_RetMailStatInfo _createProtocolObj()
        {
            return new GS2GC.p002_InitOp.GS2GC_002_014_RetMailStatInfo();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p002_InitOp.GS2GC_002_014_RetMailStatInfo _msg)
        {
            //NPGCommon.UnityEditorLog ( ENPLogType.ERROR, "=====GS2GC_002_014_RetMailStatInfo=====获取玩家信息");

            NPPlayer.instance.mailComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.mailComp.retMailStatInfo(_msg);
            });
        }
    }
}
