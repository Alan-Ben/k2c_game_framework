using ALPackage;
using System;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class SceneInfoRefObj : _IALBasicRefObj
{
    public long _refId { get { return id; } }

    public long id;
    public string name;//场景名字
    public NPGTextureIndex scene_icon;//场景图标  
    public NPGSceneIndex td_scene_idx;//对应的3D场景
    public WCGCameraSettingInfo camera_setting;//游戏摄像头设置信息对象

    public Vector3 corner_pos;//本场景中0，0坐标对应的三维坐标

    public NPGTerrainIndex terr_idx;    //对应地形数据的Id,如无数据则不填写，用于战斗场景

    //最大以及最小的摄像头视角宽度
    public float min_org_scale;
    public float max_org_scale;

    //这个场景的可视范围（逻辑坐标系下）
    public Rect map_drag_rect;
    //这个场景的柔和边缘范围（逻辑坐标系下）
    public Vector2 map_border_scale_size;
    //拖动时的相机移动平面
    public ESceneMoveType move_type;
    // 是否启用拖拽提示窗口
    public bool use_drag_tip_wnd;
    public Vector2 drag_tip_show_distance;
    // 惯性相关参数
    public float air_friction; // 空气阻力
    public float slide_friction; // 滑动阻力

    public GLightGoIndex dir_lights_so_index;//场景光SO文件资源下标
    public int urp_render_index = -1;//场景URPRender索引设置

#if NP_GAME
    // 把世界坐标相关的东西转换成 cornerPos 下的逻辑坐标的转换器 
    private _ALogicPlane2DPosGetter _m_logicPosGetter;
    // 相机大小变化的范围
    private WCGFloatRange _m_scaleChgRange;

    /// <summary>
    /// 获得这个场景配置的坐标转换器，以便把坐标转换到这个场景的逻辑坐标系下
    /// </summary>
    /// <remarks>
    /// 逻辑坐标系的构成为：以 corner_pos 为原点，以 move_type 决定的一个 2D 平面
    /// </remarks>
    [NotNull] 
    public _ALogicPlane2DPosGetter getLogicPosGetter()
    {
        if (_m_logicPosGetter != null)
            return _m_logicPosGetter;

        switch (move_type)
        {
            case ESceneMoveType.XZ:
                return _m_logicPosGetter = new LogicPlane2DPosGetterXZ(corner_pos);
            case ESceneMoveType.XY:
                return _m_logicPosGetter = new LogicPlane2DPosGetterXY(corner_pos);
            case ESceneMoveType.YZ:
                return _m_logicPosGetter = new LogicPlane2DPosGetterZY(corner_pos);
            default:
                return _m_logicPosGetter = new LogicPlane2DPosGetterXZ(corner_pos);
        }
    }
    [NotNull]
    public WCGFloatRange getScaleChgRange()
    {
        return _m_scaleChgRange ??= new WCGFloatRange(min_org_scale, max_org_scale);
    }

    public Vector3 changePosByMoveType(Vector3 _originPos, Vector3 _newPos)
    {
        return move_type switch
        {
            ESceneMoveType.XZ => new Vector3(_newPos.x, _originPos.y, _newPos.z),
            ESceneMoveType.XY => new Vector3(_newPos.x, _newPos.y, _originPos.z),
            ESceneMoveType.YZ => new Vector3(_originPos.x, _newPos.y, _newPos.z),
            _ => _newPos
        };
    }
#endif
}

public class NPGSOSceneInfoRefSet : _TALSOBasicRefSet<SceneInfoRefObj>
{
    /************
     * 资源加载路径
     **/
    public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
    public static string objName { get { return "scene_info"; } }
}
