using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using GOE;

/// <summary>
/// 用于监听Camera视野变化来改变UI缩放比例的Mono
/// </summary>
public class NPGGUIMonoCommonCameraScaleListen : MonoBehaviour
{
    [ALHeader("最小缩放比例")]
    public float minScale = 0.5f;
    [ALHeader("最大缩放比例")]
    public float maxScale = 1.5f;

    [ALHeader("对应标准缩放的摄像机org参数，")]
    public float normalizeCameraOrgSize = 12;
    [ALHeader("视野缩放倍率调整对应实际ui缩放的比例系数，用于让两边缩放倍率有一定容错性")]
    public float deltaScale = 1f;
    [ALHeader("视角缩放的监听控制的Go列表")]
    public List<GameObject> listenControlObjs;
#if NP_GAME
    
    private void OnEnable()
    {
        _onCameraScale();
        WinMsg.RegisterMsgAct(WinMsgType.ON_CAMERA_ORTHOGRAPH_SIZE_CHANGE, _onCameraScale);
    }

    private void OnDisable()
    {
        WinMsg.UnregisterMsgAct(WinMsgType.ON_CAMERA_ORTHOGRAPH_SIZE_CHANGE, _onCameraScale);
    }
    
    /// <summary>
    /// 设置缩放比例
    /// </summary>
    private void _onCameraScale()
    {
        if (null == listenControlObjs)
            return;

        float realScale = Mathf.Clamp((normalizeCameraOrgSize / CameraController.instance.defaultOrthographicSize * CameraController.instance.inverseProportion) * deltaScale
                            , minScale, maxScale);

        //设置比例
        ALUGUICommon.setUIObjScale(listenControlObjs, realScale);
    }
#endif
}
