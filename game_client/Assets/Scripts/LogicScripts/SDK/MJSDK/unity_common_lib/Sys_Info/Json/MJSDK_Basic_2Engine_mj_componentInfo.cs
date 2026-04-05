using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// mj-componentInfo：获取项目所集成组件信息
    /// </summary>
    public class MJSDK_Basic_2Engine_mj_componentInfo 
    {
        public List<MJSDK_Basic_ComponentInfo> component;
    }


    /// <summary>
    /// 组件信息
    /// </summary>
    [Serializable]
    public class MJSDK_Basic_ComponentInfo
    {
        //组件初始化状态  0为失败，1为成功
        public string status;
        //组件版本信息
        public string version;
        //组件标识符
        public string tag;
        //扩展字段1
        public string mark;
        //扩展字段1
        public string mark2;
        //扩展字段1
        public string mark3;
    }
}