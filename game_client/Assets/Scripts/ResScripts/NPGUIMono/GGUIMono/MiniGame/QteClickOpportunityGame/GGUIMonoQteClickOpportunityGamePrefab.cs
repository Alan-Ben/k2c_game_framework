using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    public class GGUIMonoQteClickOpportunityGamePrefab : _AALBasicUIWndMono
    {
        [ALHeader("点击item列表")]
        public List<GGUIMonoQteClickOpportunityItem> itemList;
        
        [ALHeader("不同状态显示")]
        public MultiStateShow<EQteClickOpportunityGameState> stateShow;
    }
}