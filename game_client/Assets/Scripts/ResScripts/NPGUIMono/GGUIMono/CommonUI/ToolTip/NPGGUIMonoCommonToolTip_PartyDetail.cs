using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace GOE
{
    public class NPGGUIMonoCommonToolTip_PartyDetail : NPGGUIMonoCommonToolTip
    {
        [ALHeader("举办者玩家信息")]
        public NPGGUIMonoPlayerIcon playerIconMono;

        [ALHeader("聚会名称")]
        public Text txtName;

        [ALHeader("参加者列表")]
        public NPGGUIMonoCommonTextItemGrid joinGridMono;
    }
}
