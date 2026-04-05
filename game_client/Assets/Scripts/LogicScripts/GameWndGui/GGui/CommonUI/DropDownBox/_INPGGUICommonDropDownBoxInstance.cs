using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 通用下拉框的数据接口
    /// </summary>
    public interface _INPGGUICommonDropDownBoxInstance
    {
        /// <summary>
        /// 对象的Id
        /// </summary>
        int instanceId { get; }

        /// <summary>
        /// 对象内容
        /// </summary>
        string content { get; }
        
        /// <summary>
        /// 图片
        /// </summary>
        NPGTextureIndex textureIndex{ get; }
    }
}