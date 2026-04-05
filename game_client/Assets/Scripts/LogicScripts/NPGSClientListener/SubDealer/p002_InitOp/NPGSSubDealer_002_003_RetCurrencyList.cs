using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;

namespace GOE
{
    //获取玩家资源信息
    public class NPGSSubDealer_002_003_RetCurrencyList : NPSubDealer<GS2GC.p002_InitOp.GS2GC_002_003_RetCurrencyList>
    {
        /// <summary>
        /// 构造协议对象结构体，默认让子类重载，这样的性能会比createInstance高，特别在协议处理初始化的时候
        /// </summary>
        /// <returns></returns>
        protected override GS2GC.p002_InitOp.GS2GC_002_003_RetCurrencyList _createProtocolObj()
        {
            return new GS2GC.p002_InitOp.GS2GC_002_003_RetCurrencyList();
        }

        protected override void _dealProtocolByLog(_IALProtocolDealer _dealer, GS2GC.p002_InitOp.GS2GC_002_003_RetCurrencyList _msg)
        {
            //NPGCommon.UnityEditorLog ( ENPLogType.ERROR, "=====GS2GC_002__003_RetCurrencyList=====获取玩家资源信息");

            NPPlayer.instance.rescourceComp.dealPreInitFunc(() =>
            {
                NPPlayer.instance.rescourceComp.setData(_msg.getCurrencyList());
            });
        }
    }
}
