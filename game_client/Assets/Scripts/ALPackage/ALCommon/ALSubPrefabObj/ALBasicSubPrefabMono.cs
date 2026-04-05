using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/******************
 * 基本的窗口脚本对象
 **/
namespace ALPackage
{
    public class ALBasicSubPrefabMono : MonoBehaviour
    {
        public Animation objAnimation;
        public string showAniName = "show";
        public string hideAniName = "hide";
        public float showAniTime;
        public float hideAniTime;
    }
}
