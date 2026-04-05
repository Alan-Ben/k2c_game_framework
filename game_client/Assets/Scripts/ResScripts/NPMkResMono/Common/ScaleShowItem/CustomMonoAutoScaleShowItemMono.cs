using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/// <summary>
/// 自动缩放父子关系的测试脚本
/// </summary>
public class CustomMonoAutoScaleShowItemMono : MonoBehaviour
{
    /// <summary>
    /// 自动缩放的单个对象的信息存储结构体
    /// </summary>
    protected class AutoScaleGo
    {
        public Transform go;
        //开始计算缩放的开始时间
        public float enableTime;
        //缩放的目标比例值
        public float targetScale;
        //原始比例
        public float srcLocalScale;
        //缩放计算对象，本对象在需要使用的时候才创建
        public ALRealTimeFloatFadeController scaleCalObj;
    }

    //对应根节点的常规全局比例，用此比例进行反向计算，保证所有子对象比例一致
    [ALHeader("对应根节点的常规全局比例，用此比例进行反向计算，保证所有子对象比例一致")]
    public float rootIdentityScale = 1f;

    [ALHeader("总的缩放时长")]
    public float scaleTotalTime = 0.8f;
    [ALHeader("总的缩放时长随机范围")]
    public float randomTotalTime = 0.5f;
    [ALHeader("缩放前半部分的加速时长")]
    public float preAccTime = 0.2f;
    [ALHeader("缩放前半部分的加速时长随机范围")]
    public float randomPreAccTime = 0.2f;

    [ALHeader("每个缩放对象开始缩放的随机时间范围")]
    public float randomEnableTime = 1f;

    [ALHeader("是展示子对象还是隐藏子对象")]
    public bool isShowItem = true;

    //记录开始执行操作的相关变量
    //执行缩放操作的处理
    private float _m_fScaleStartTime;
    private List<AutoScaleGo> _m_lScaleObjList;

    //有效和无效的时候分别注册和注销显示对象
    private void OnEnable()
    {
        if (null == transform)
            return;

        //将keep脚本先无效掉
        if(null != gameObject)
        {
            CustomMonoKeepScaleShowItemMono keepMono = gameObject.GetComponent<CustomMonoKeepScaleShowItemMono>();
            if (null != keepMono)
                keepMono.enabled = false;
        }

        //获取根节点比例
        float rootScale = transform.lossyScale.x;
        //计算需要缩放调整的比例
        float deltaScale = (0 == rootScale ? 0f : (rootIdentityScale / rootScale));

        //先释放原先数据
        _clearScaleList();

        if (null == _m_lScaleObjList)
            _m_lScaleObjList = new List<AutoScaleGo>();

        _m_lScaleObjList.Clear();
        _m_fScaleStartTime = Time.realtimeSinceStartup;

        //遍历获取子对象列表，创建对应进行缩放处理的数据
        Transform tmpT = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            tmpT = transform.GetChild(i);
            if (null == tmpT)
                continue;

            AutoScaleGo scaleGo = new AutoScaleGo();
            scaleGo.go = tmpT;
            scaleGo.targetScale = tmpT.localScale.x * deltaScale;
            scaleGo.srcLocalScale = tmpT.localScale.x;
            scaleGo.enableTime = Time.realtimeSinceStartup + Random.Range(0, randomEnableTime);
            scaleGo.scaleCalObj = null;

            //设置所有子对象有效
            ALUGUICommon.setGameObjEnable(scaleGo.go.gameObject);

            //如果是显示处理，需要先缩小所有对象
            if (isShowItem)
                ALUGUICommon.setUIObjScale(scaleGo.go, 0f);

            _m_lScaleObjList.Add(scaleGo);
        }
    }

    private void Update()
    {
        if (null == _m_lScaleObjList)
            return;

        //是否还有对象在进行缩放
        bool isFinish = true;
        //遍历所有对象进行缩放处理
        AutoScaleGo tmpSG = null;
        for (int i = 0; i < _m_lScaleObjList.Count; i++)
        {
            tmpSG = _m_lScaleObjList[i];
            if (null == tmpSG)
                continue;

            if (Time.realtimeSinceStartup <= tmpSG.enableTime)
            {
                //设置未开始，此时不对缩放做任何操作
                isFinish = false;
            }
            else
            {
                if (null == tmpSG.scaleCalObj)
                {
                    if(isShowItem)
                        tmpSG.scaleCalObj = new ALRealTimeFloatFadeController(0, tmpSG.targetScale, 0, scaleTotalTime + Random.Range(0, randomTotalTime), preAccTime + Random.Range(0, randomPreAccTime));
                    else
                        tmpSG.scaleCalObj = new ALRealTimeFloatFadeController(tmpSG.targetScale, 0f, 0, scaleTotalTime + Random.Range(0, randomTotalTime), preAccTime + Random.Range(0, randomPreAccTime));
                }
                ALUGUICommon.setUIObjScale(tmpSG.go, tmpSG.scaleCalObj.curValue);

                //如果有对象未结束需要设置结束标记
                if (!tmpSG.scaleCalObj.isDone)
                    isFinish = false;
            }
        }

        //如果所有单位都已经结束，则清空数据
        if(isFinish)
        {
            _discard();
        }
    }

    private void OnDisable()
    {
        _discard();
    }

    /// <summary>
    /// 释放所有数据
    /// </summary>
    private void _clearScaleList()
    {
        if (null != _m_lScaleObjList)
        {
            //需要还原比例
            AutoScaleGo tmpSG = null;
            for (int i = 0; i < _m_lScaleObjList.Count; i++)
            {
                tmpSG = _m_lScaleObjList[i];
                if (null == tmpSG)
                    continue;

                ALUGUICommon.setUIObjScale(tmpSG.go, tmpSG.srcLocalScale);

                //如果是隐藏的，需要将所有子对象无效
                if (!isShowItem)
                    ALUGUICommon.setGameObjDisable(tmpSG.go.gameObject);
            }

            _m_lScaleObjList.Clear();
            _m_lScaleObjList = null;
        }
    }

    /// <summary>
    /// 释放所有数据
    /// </summary>
    private void _discard()
    {
        _clearScaleList();

        //获取本对象的脚本
        if (null != gameObject)
        {
            CustomMonoKeepScaleShowItemMono keepMono = gameObject.GetComponent<CustomMonoKeepScaleShowItemMono>();
            if (null == keepMono)
                keepMono = gameObject.AddComponent<CustomMonoKeepScaleShowItemMono>();

            //设置比例
            keepMono.rootIdentityScale = rootIdentityScale;

            //设置脚本有效
            keepMono.enabled = true;
        }
    }
}