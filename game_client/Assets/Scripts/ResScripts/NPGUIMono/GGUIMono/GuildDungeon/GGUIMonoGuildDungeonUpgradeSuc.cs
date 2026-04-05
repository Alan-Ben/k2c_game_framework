using ALPackage;
using GOE;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 联盟副本升级成功
/// </summary>
public class GGUIMonoGuildDungeonUpgradeSuc : _AALBasicUIWndMono
{
    [ALHeader("副本名字")]
    public Text txtDungeonName;
    [ALHeader("副本图标")]
    public RawImage dungeonIcon;

    // <AutoGen:MonoDeclaration>
    [ALHeader("旧等级")]
    public Text txtOldLevel;
    [ALHeader("新等级")]
    public Text txtNewLevel;
    [ALHeader("旧Boss血量")]
    public Text txtOldBossBlood;
    [ALHeader("新Boss血量")]
    public Text txtNewBossBlood;
    [ALHeader("旧副本总血量")]
    public Text txtOldTotalBlood;
    [ALHeader("新副本总血量")]
    public Text txtNewTotalBlood;
    // </AutoGen:MonoDeclaration>
    [ALHeader("关闭按钮")]
    public GameObject btnClose;
    
    /************
    * 资源加载路径
    */
    public static string assetPath { get { return UIResPathAssistant.getAssetPath(6904); } }
    public static string objName { get { return UIResPathAssistant.getObjName(6904);} }
}
