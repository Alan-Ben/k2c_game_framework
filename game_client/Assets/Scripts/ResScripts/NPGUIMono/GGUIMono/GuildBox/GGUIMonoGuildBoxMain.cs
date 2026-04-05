using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 页签类型
/// </summary>
public enum EGuildBoxPage
{
    FreeBox,//免费宝箱
    GiftBox,//付费宝箱
}
/// <summary>
/// 联盟宝箱
/// </summary>
public class GGUIMonoGuildBoxMain : _AALBasicUIWndMono
{
    
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    [ALHeader("页签父节点")]
    public Transform pageParent;
    [ALHeader("免费宝箱页签")]
    public NPGGUIMonoCommonTab freeBoxTab;
    [ALHeader("免费宝箱页签显示go")]
    public List<GameObject> freeBoxTabShowList;
    
    [ALHeader("赠礼宝箱页签")]
    public NPGGUIMonoCommonTab girtBoxTab;
    [ALHeader("赠礼宝箱页签显示go")]
    public List<GameObject> giftBoxTabShowList;

    [ALHeader("活跃宝箱进度条")]
    public NPGGUIMonoProgress activeBoxProgress;
    [ALHeader("领取活跃宝箱奖励按钮")]
    public GameObject btnGetActiveBox;
    [ALHeader("活跃宝箱奖励详情")]
    public GameObject btnActiveBoxDetail;
    [ALHeader("活跃宝箱积分获取详情")]
    public GameObject btnActiveBoxScoreDetail;
    [ALHeader("活跃宝箱可领取时显示Go列表")]
    public List<GameObject> activeBoxCanGetShowList;
    [ALHeader("活跃宝箱可领取时隐藏Go列表")]
    public List<GameObject> activeBoxCanGetHideList;

    // <AutoGen:MonoDeclaration>

    // </AutoGen:MonoDeclaration>
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(8702); } }
    public static string objName { get { return UIResPathAssistant.getObjName(8702);} }
}