using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GGUIMonoMarsLvlPropertyShowItemGrid : _ATNPGGUIMonoShowAnimGrid<GGUIMonoMarsLvlPropertyShowItem>
    {
        [ALHeader("属性名称Item加载父物体")]
        public Transform propertyNameItemParent;
        
        [ALHeader("属性名称预制体")]
        public NPGGUIMonoCommonTextItem monoPropertyNameItemPrefab;
    }
}