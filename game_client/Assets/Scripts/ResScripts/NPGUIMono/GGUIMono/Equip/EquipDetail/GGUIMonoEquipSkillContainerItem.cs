using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 藏品技能列表item
    /// </summary>
    public class GGUIMonoEquipSkillContainerItem : _ANPGGUIMonoSingleChoiceItem
    {
        [ALHeader("加成值进度")]
        public Slider sldValue;
        [ALHeader("加成值")]
        public Text txtAddValue;
        [ALHeader("没有数据时需要显示的GO列表")]
        public List<GameObject> goNoDataShowList;
        [ALHeader("没有数据时需要隐藏的GO列表")]
        public List<GameObject> goNoDataHideList;
        [ALHeader("加成值进度条初始值")]
        public float sldBaseValue;
        [ALHeader("没有加成值时进度条默认值")]
        public float noAddValueDefaultSld = 0.5f;
        [ALHeader("失败特效id")]
        public long failSfxId;
        [ALHeader("替换特效id")]
        public long replaceSfxId;
        [ALHeader("替换成功特效父节点")]
        public Transform replaceSfxParent;
    }
}