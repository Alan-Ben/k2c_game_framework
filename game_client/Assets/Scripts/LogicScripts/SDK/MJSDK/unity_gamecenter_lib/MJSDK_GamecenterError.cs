using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MJSDK_Package
{
    /// <summary>
    /// 游戏圈组件异常状态码
    /// </summary>
    public class MJSDK_GamecenterError : MJSDK_Error
    {
        //iOS系统低，未支持gamecenter功能
        public const int C_Unity_Gamecenter_Error_Get_UserInfo_No_Support = 40001;
        //获取gamecenter玩家信息失败
        public const int C_Unity_Gamecenter_Error_Get_UserInfo_Failr = 40002;
    }
}
