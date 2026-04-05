
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

public class GGUIMonoRushExchangeFollower : ALGGUIMonoCommonFollowItem
{
    [ALHeader("状态列表")]
    public List<RushExchangeStateInfo> stateInfoList = new List<RushExchangeStateInfo>();
    [ALHeader("倒计时")]
    public Text txtTime;
    public TextMeshProUGUIEx txtMeshProTime;
    [ALHeader("可兑换 颜色")] 
    public Color stateReadyCol;
    [ALHeader("兑换中 颜色")]
    public Color stateExchangeCol;
    [ALHeader("兑换完成可领奖 颜色")]
    public Color stateRewardCol;
    [ALHeader("等待中 颜色")]
    public Color stateWaitCol;
   

}
