using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 联盟成员任命界面item
    /// </summary>
    public class GGUIMonoGuildAppointItem : _AALBasicUIWndMono
    {
        [ALHeader("任命职位类型")]
        public EGuildPositionType positionType;
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("按钮描述")]
        public Text txtDesc;
        [ALHeader("是现职位时需要显示的GO列表")]
        public List<GameObject> goCurPositionShowList;
    }
}
