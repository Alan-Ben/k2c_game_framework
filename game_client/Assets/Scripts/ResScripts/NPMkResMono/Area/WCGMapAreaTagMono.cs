using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

/******************
 * 地图中不同区域的提示信息脚本对象
 **/
public class WCGMapAreaTagMono : MonoBehaviour
{
    /** 对应区域的Id */
    public int areaId;
    /** 对应区域是否有效的状态标记 */
    public bool isAreaEnableTag;

    /** 在技能释放时隐藏的处理 */
    public List<GameObject> onSkillViewDisable;

    // Update is called once per frame
    void Awake()
    {
        WCGAreaAssistOpObj areaOpObj = WCGAreaAssistOpMgr.instance.getAreaAssistOpObj(areaId);
        if (null == areaOpObj)
            return;

        areaOpObj.regGo(this);
    }
    // Update is called once per frame
    void Start()
    {
    }
}
