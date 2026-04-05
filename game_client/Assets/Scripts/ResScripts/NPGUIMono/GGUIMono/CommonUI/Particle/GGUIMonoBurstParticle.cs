using UnityEngine;
using System;
using System.Collections.Generic;
using ALPackage;
using GOE;

/// <summary>
/// 粒子炸裂窗口
/// </summary>
public class GGUIMonoBurstParticle : _AALBasicUIWndMono
{
    [ALHeader("粒子特效父节点")]
    public RectTransform sfxParent;

    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(UIResPathConst.WIN_BURST_PARTICLE); } }
    public static string objName { get { return UIResPathAssistant.getObjName(UIResPathConst.WIN_BURST_PARTICLE); } }
}
