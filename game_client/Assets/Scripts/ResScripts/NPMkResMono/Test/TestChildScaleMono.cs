using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/// <summary>
/// 自动缩放父子关系的测试脚本
/// </summary>
public class TestChildScaleMono : MonoBehaviour
{
    protected class TestScaleGo
    {
        public Transform go;
        public float enableTime;
        public float targetScale;
        public ALRealTimeFloatFadeController scaleCalObj;
    }

    [ALHeader("总的缩放时长")]
    public float scaleTotalTime = 1f;
    [ALHeader("缩放前半部分的加速时长")]
    public float preAccTime = 0.3f;

    //执行缩放操作的处理
    private float scaleStartTime;
    private List<TestScaleGo> scaleObj;
    
#if NP_GAME

    //有效和无效的时候分别注册和注销显示对象
    private void OnEnable()
    {
        if (null == transform)
            return;

        if (null == scaleObj)
            scaleObj = new List<TestScaleGo>();

        scaleObj.Clear();
        scaleStartTime = Time.realtimeSinceStartup;

        //遍历获取子对象列表
        Transform tmpT = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            tmpT = transform.GetChild(i);
            if (null == tmpT)
                continue;

            TestScaleGo scaleGo = new TestScaleGo();
            scaleGo.go = tmpT;
            scaleGo.targetScale = tmpT.localScale.x;
            scaleGo.enableTime = Time.realtimeSinceStartup + Random.Range(0, 1f);
            scaleGo.scaleCalObj = null;

            scaleObj.Add(scaleGo);
        }
    }

    private void Update()
    {
        if (null == scaleObj)
            return;

        TestScaleGo tmpSG = null;
        for (int i = 0; i < scaleObj.Count; i++)
        {
            tmpSG = scaleObj[i];
            if (null == tmpSG)
                continue;

            if (Time.realtimeSinceStartup <= tmpSG.enableTime)
            {
                ALUGUICommon.setUIObjScale(tmpSG.go, 0f);
            }
            else
            {
                if (null == tmpSG.scaleCalObj)
                    tmpSG.scaleCalObj = new ALRealTimeFloatFadeController(0, tmpSG.targetScale, 0, scaleTotalTime + Random.Range(0, 0.5f), preAccTime + Random.Range(0, 0.2f));
                ALUGUICommon.setUIObjScale(tmpSG.go, tmpSG.scaleCalObj.curValue);
            }
        }
    }

    private void OnDisable()
    {
        if(null != scaleObj)
            scaleObj.Clear();
    }

    private void OnDestroy()
    {
    }
    
#endif
}