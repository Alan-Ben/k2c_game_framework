
using GOE;
using UnityEngine;
using UnityEngine.UI;

public class GGUIGuildDungeonMapMono : MonoBehaviour
{
    [Header("怪物实例化配置")]
    public RectTransform instanceParent; // 实例化的物体父节点

    public ScrollRect mapScrollRect;
    
    [Header("起始点物体")]
    public Transform startPointGo; // 

    [Header("地图布局配置")]
    public float width = 1;
    public float height = 1;
    public float xBorder = 4;
    public float yBorder = 4;
    
    public Vector3 mapOffsetPos;
    
    public GuildDungeonMapLineConfigMono lineConfig;
    
    // <AutoGen:MonoDeclaration>
    // </AutoGen:MonoDeclaration>
}
