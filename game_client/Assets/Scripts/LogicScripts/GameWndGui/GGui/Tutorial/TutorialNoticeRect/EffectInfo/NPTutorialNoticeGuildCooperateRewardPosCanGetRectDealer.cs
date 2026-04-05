using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟协作奖励据点可领取奖励位置
    /// </summary>
    public class NPTutorialNoticeGuildCooperateRewardPosCanGetRectDealer : _AWCGTutorialNoticeRectDealer
    {
        public override EWCGTutorialNoticeRectType noticeRectType { get { return EWCGTutorialNoticeRectType.GUILD_COOPERATE_REWARD_POS_CAN_GET; } }

        public override bool getRect(out Rect _rect)
        {
            RectTransform rectTransform = GGUIWndGuildCooperateMain.instance?.getCanGetRewardPosRectTransform();
            if (null == rectTransform)
            {
                _rect = new Rect();
                return false;
            }

            Vector2 pos = GCommon.getUIRootPos(rectTransform);
            Vector2 minimumCornerPos = new Vector2(pos.x - rectTransform.pivot.x * rectTransform.rect.width, 
                                                   pos.y - rectTransform.pivot.y * rectTransform.rect.height);
            _rect = new Rect(minimumCornerPos, rectTransform.rect.size);
            return true;
        }

        public static NPTutorialNoticeGuildCooperateRewardPosCanGetRectDealer readVariable(string _str)
        {
            NPTutorialNoticeGuildCooperateRewardPosCanGetRectDealer effectObj = new NPTutorialNoticeGuildCooperateRewardPosCanGetRectDealer();
            return effectObj;
        }
    }
}

