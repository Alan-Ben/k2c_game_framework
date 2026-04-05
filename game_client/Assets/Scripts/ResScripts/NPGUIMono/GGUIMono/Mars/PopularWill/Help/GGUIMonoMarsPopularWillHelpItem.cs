using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoMarsPopularWillHelpItem : _TALUGUIMonoGridItem
    {
        [ALHeader("求助名")]
        public TextEx txtHelpName;
        [ALHeader("求助名key, 一个参数:求助npc名")]
        public string txtHelpNameKey;

        [ALHeader("npc头像")]
        public RawImage npcHeadIcon;
        
        [ALHeader("状态列表")]
        public List<NPCommonEnumStatMutexShowInfo<EMarsPopularWillHelpState>> stateShowList;

        [ALHeader("处理按钮")]
        public GameObject btnDeal;
    }
}