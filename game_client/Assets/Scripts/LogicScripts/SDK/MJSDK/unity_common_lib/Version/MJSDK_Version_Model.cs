using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MJSDK_Package
{
    /// <summary>
    /// SDK版本消息结构体[接收平台回执版本信息]
    /// </summary>
    public class MJSDK_Version_Model
    {
        public int majorV;
        public int minorV;
        public int buildV;
        public int fixV;
    }
}
