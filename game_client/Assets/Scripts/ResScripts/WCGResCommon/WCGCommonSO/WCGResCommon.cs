using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ALPackage;
using GOE;
using NPEnum;

/************
 * 资源读取通用函数
 **/
public class WCGResCommon
{
    public static int WCG_C_LAYER_UNIT = (int)ENPLayer.GAME_UNIT;
    public static int WCG_C_LAYER_SCENE = (int)ENPLayer.GAME_SCENE;
    public static int WCG_C_LAYER_UNIT_SHADOW = (int)ENPLayer.GAME_UNIT_SHADOW;
    public static int WCG_C_LAYER_IGNORE = (int)ENPLayer.GAME_IGNORE_LAYER;
    public static int WCG_C_LAYER_TRIGGER = (int)ENPLayer.GO_TRIGGER;
    public static int WCG_C_LAYER_SPECIAL_ACTOR = (int)ENPLayer.GAME_SPECIAL_ACTOR;
    public static int WCG_C_LAYER_WATER = (int)ENPLayer.WATER;
    public static int WCG_C_LAYER_OVERLAY_VFX = (int)ENPLayer.GAME_OVERLAY_VFX;
    
    public static int WCG_C_LAYER_SHOWCASE = (int)ENPLayer.SHOWCASE;
    public static int WCG_C_LAYER_SPACE_SCENE_NON_VISION_BLOCK = (int)ENPLayer.GAME_SCENE_ENTIY;
    public static int WCG_C_LAYER_SPACE_SCENE_VISION_BLOCK = (int)ENPLayer.GAME_SCENE_ENTIY_OUTLINE;
    public static int WCG_C_LAYER_SPACE_VISION_BLOCK = (int)ENPLayer.GAME_SCENE_DISEMBODIED_OUTLINE;

    public static int WCG_C_LAYER_MASK_SCENE = (1 << WCG_C_LAYER_SCENE);
    public static int WCG_C_LAYER_MASK_WATER = (1 << WCG_C_LAYER_WATER);
    public static int WCG_C_LAYER_MASK_SPACE_SCENE = (1 << WCG_C_LAYER_SPACE_SCENE_VISION_BLOCK) | (1 << WCG_C_LAYER_SPACE_SCENE_NON_VISION_BLOCK) | (1 << WCG_C_LAYER_SCENE);
    
    /// <summary>
    /// 设置GameObject的层级
    /// </summary>
    /// <param name="rootGO"></param>
    /// <param name="layer">层级</param>
    /// <param name="bIncludeChildren">是否包含子物体</param>
    public static void SetLayer(GameObject rootGO, int layer, bool bIncludeChildren)
    {
        if(rootGO == null)
            return;
        rootGO.layer = layer;
        if (bIncludeChildren)
        {
            foreach (Transform trans in rootGO.transform.GetComponentsInChildren<Transform>(true))
            {
                trans.gameObject.layer = layer;
            }
        }
    }
}
