using CommonEnum;

namespace GOE
{
    public interface _IWeekCardAssignItemShowInfo
    {
        /// <summary>
        /// 委派item的资源id
        /// </summary>
        /// <returns></returns>
        long getUIPathId();
        /// <summary>
        /// 委派类型
        /// </summary>
        /// <returns></returns>
        EWeekCardSettleType getAssignType();
        /// <summary>
        /// 委派名字
        /// </summary>
        /// <returns></returns>
        string getAssignName();

        /// <summary>
        /// 委派描述
        /// </summary>
        /// <returns></returns>
        string getAssignDesc();
        /// <summary>
        /// 解锁状态
        /// </summary>
        /// <returns></returns>
        EGameCommonUnlockType getUnlockType();
    }
}