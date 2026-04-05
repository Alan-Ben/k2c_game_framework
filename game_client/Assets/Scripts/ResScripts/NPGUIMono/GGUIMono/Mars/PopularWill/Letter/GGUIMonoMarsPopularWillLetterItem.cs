using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 火星基地 - 民意信件
    /// </summary>
    public class GGUIMonoMarsPopularWillLetterItem : _TALUGUIMonoGridItem
    {
        [ALHeader("npc头像")]
        public RawImage npcHead;

        [ALHeader("npc名字")]
        public TextEx txtNpcName;
        
        [ALHeader("信件内容")]
        public TextEx txtContent;

        [ALHeader("信件是吐槽时显示")]
        public List<GameObject> isComplainShowGoList;
        [ALHeader("信件不是吐槽时显示")]
        public List<GameObject> notComplainShowGoList;

        [ALHeader("前往按钮")]
        public GameObject btnGoto;
    }
}