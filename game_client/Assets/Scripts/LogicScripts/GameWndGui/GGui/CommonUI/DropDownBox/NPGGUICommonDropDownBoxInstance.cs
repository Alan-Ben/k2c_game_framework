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
    public class NPGGUICommonDropDownBoxInstance : _INPGGUICommonDropDownBoxInstance
    {
        public NPGGUICommonDropDownBoxInstance(int _instanceId, string _content, NPGTextureIndex _textureIndex = null)
        {
            instanceId = _instanceId;
            content = _content;
            textureIndex = _textureIndex;
        }

        public int instanceId { get; }
        public string content { get; }
        public NPGTextureIndex textureIndex { get; }
    }
}