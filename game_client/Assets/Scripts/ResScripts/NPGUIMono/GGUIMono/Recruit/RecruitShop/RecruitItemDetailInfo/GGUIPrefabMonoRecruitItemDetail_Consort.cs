using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 妃子详细信息prefab窗口
    /// </summary>
    public class GGUIPrefabMonoRecruitItemDetail_Consort : GGUIPrefabMonoRecruitItemDetail
    {
        [ALHeader("妃子详细信息子窗口")]
        public GGUISubMonoConsortDetailInfo monoConsortDetailInfo;

        [ALHeader("更多信息按钮")]
        public GameObject btnMoreInfo;
        
        [ALHeader("关联大臣图标容器")]
        public GGUIMonoHeroConsortSimpleIconContainer relationHeroIconContainer;
    }
}