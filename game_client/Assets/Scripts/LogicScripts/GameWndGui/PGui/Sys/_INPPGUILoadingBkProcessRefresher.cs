using UnityEngine;
using System.Collections.Generic;
using System;
using ALPackage;
using ALBasicProtocolPack;

namespace GOE
{
    /// <summary>
    /// 登录流程中，下面进度条的刷新处理接口对象
    /// </summary>
    public interface _INPPGUILoadingBkProcessRefresher
    {
        /// <summary>
        /// 获取当前进度
        /// </summary>
        float curProcess { get; }

        /// <summary>
        /// 获取当前操作的文本
        /// </summary>
        string curOPTxt { get; }

        /// <summary>
        /// 刷新间隔
        /// </summary>
        float refreshDuration { get; }
    }
}