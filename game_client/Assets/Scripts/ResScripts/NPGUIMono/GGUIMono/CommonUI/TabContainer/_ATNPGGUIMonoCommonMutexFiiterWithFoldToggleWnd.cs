using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 带有折叠开关的互斥过滤窗口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class _ATNPGGUIMonoCommonMutexFiiterWithFoldToggleWnd<T> : _ATNPGGUIMonoCommonMutexFiiterWnd<T>
    {
        [ALHeader("当前选择的类型文本")]
        public List<Text> curTypeTxtList;
    
        [ALHeader("展开收起toggle")]
        public NPGGUIMonoCommonToggleEx toggleFold;

        [ALHeader("收起按钮")]
        public GameObject foldBtn;
    }
}