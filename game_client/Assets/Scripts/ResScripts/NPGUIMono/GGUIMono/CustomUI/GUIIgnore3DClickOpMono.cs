using UnityEngine;
using System.Collections;
using ALPackage;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace GOE
{
    /// <summary>
    /// 自定义条件显隐脚本
    /// </summary>
    public class GUIIgnore3DClickOpMono : MonoBehaviour
    {
        [ALHeader("在不屏蔽3D操作的情况下，将raycaster拖到这里可以屏蔽3D触发的点击行为")]
        public GraphicRaycaster racastObj;

        //有效和无效的时候分别注册和注销显示对象
        private void OnEnable()
        {
#if NP_GAME
            InputListener.instance.regIgnoreClickUIGraphics(racastObj);
#endif
        }

        private void OnDisable()
        {
#if NP_GAME
            InputListener.instance.unregIgnoreClickUIGraphics(racastObj);
#endif
        }
    }
}

