using System.Collections.Generic;

using UnityEngine;
using ALPackage;

/// <summary>
/// 自动缩放父子关系的测试脚本
/// </summary>
public class CustomMonoKeepScaleShowItemMono : MonoBehaviour
{
    /// <summary>
    /// 自动缩放的单个对象的信息存储结构体
    /// </summary>
    protected class KeepScaleGo
    {
        public Transform go;
        //缩放的目标比例值
        public float targetScale;
        //原始比例
        public float srcLocalScale;
    }

    //对应根节点的常规全局比例，用此比例进行反向计算，保证所有子对象比例一致
    [ALHeader("对应根节点的常规全局比例，用此比例进行反向计算，保证所有子对象比例一致")]
    public float rootIdentityScale = 1f;

    //记录开始执行操作的相关变量
    private List<KeepScaleGo> _m_lScaleObjList;
    //检查处理的根节点比例，避免反复遍历设置
    private float _m_fCheckRootScale;

    //有效和无效的时候分别注册和注销显示对象
    private void OnEnable()
    {
        if (null == transform)
            return;

        //获取根节点比例
        float rootScale = transform.lossyScale.x;
        //计算需要缩放调整的比例
        float deltaScale = (0 == rootScale ? 0f : (rootIdentityScale / rootScale));

        //记录当前比例
        _m_fCheckRootScale = rootIdentityScale;

        //先释放原先数据
        _discard();

        if (null == _m_lScaleObjList)
            _m_lScaleObjList = new List<KeepScaleGo>();

        _m_lScaleObjList.Clear();

        //遍历获取子对象列表，创建对应进行缩放处理的数据
        Transform tmpT = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            tmpT = transform.GetChild(i);
            if (null == tmpT)
                continue;

            KeepScaleGo scaleGo = new KeepScaleGo();
            scaleGo.go = tmpT;
            scaleGo.srcLocalScale = tmpT.localScale.x;
            scaleGo.targetScale = tmpT.lossyScale.x * deltaScale;

            _m_lScaleObjList.Add(scaleGo);
        }

        //遍历一次更新
        Update();
    }

    private void Update()
    {
        if (null == _m_lScaleObjList)
            return;

        //判断比例是否有变化，未变化则不遍历处理
        if(Mathf.Abs(transform.lossyScale.x - _m_fCheckRootScale) < 0.0001f)
            return;

        _m_fCheckRootScale = transform.lossyScale.x;

        //遍历所有对象进行缩放处理
        KeepScaleGo tmpSG = null;
        for (int i = 0; i < _m_lScaleObjList.Count; i++)
        {
            tmpSG = _m_lScaleObjList[i];
            if (null == tmpSG)
                continue;
            
            ALUGUICommon.setUIObjScale(tmpSG.go, tmpSG.go.localScale.x * (tmpSG.targetScale / tmpSG.go.lossyScale.x));
        }
    }

    private void OnDisable()
    {
        _discard();
    }

    /// <summary>
    /// 释放所有数据
    /// </summary>
    private void _discard()
    {
        if (null != _m_lScaleObjList)
        {
            //还原比例
            //遍历所有对象进行缩放处理
            KeepScaleGo tmpSG = null;
            for (int i = 0; i < _m_lScaleObjList.Count; i++)
            {
                tmpSG = _m_lScaleObjList[i];
                if (null == tmpSG)
                    continue;

                ALUGUICommon.setUIObjScale(tmpSG.go, tmpSG.srcLocalScale);
            }

            _m_lScaleObjList.Clear();
            _m_lScaleObjList = null;
        }
    }
}