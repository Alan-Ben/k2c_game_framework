using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟协作奖励据点可领取奖励位置
    /// </summary>
    public class TutorialHighlightLocationDealer_GUILD_COOPERATE_REWARD_POS_CAN_GET : _ATutorialHighlightLocationDealer
    {
        public override ETutorialHighlightLocationType highlightLocationType { get { return ETutorialHighlightLocationType.GUILD_COOPERATE_REWARD_POS_CAN_GET; } }

        public override bool getCenterUIRootPosition(out Vector2 centerUIRootPosition)
        {
            centerUIRootPosition = Vector2.zero;

            if (GGUIWndGuildCooperateMain.instance == null || !GGUIWndGuildCooperateMain.instance.isLoaded || !GGUIWndGuildCooperateMain.instance.isShow)
            {
                Debug.LogError("[TutorialHighlightLocationDealer_GUILD_COOPERATE_REWARD_POS_CAN_GET] 联盟协作主界面未加载或未显示");
                return false;
            }

            RectTransform rectTransform = GGUIWndGuildCooperateMain.instance.getCanGetRewardPosRectTransform();
            if (rectTransform == null)
            {
                Debug.LogError($"[TutorialHighlightLocationDealer_GUILD_COOPERATE_REWARD_POS_CAN_GET] 无法获取联盟协作主界面奖励据点可领取奖励的RectTransform");
                return false;
            }

            Vector3 uiPos = rectTransform.position;

            centerUIRootPosition = GCommon.getUIRootPos(uiPos, true) + new Vector2(rectTransform.rect.width * (0.5f - rectTransform.pivot.x), rectTransform.rect.height * (0.5f - rectTransform.pivot.y));
            return true;
        }

        public static TutorialHighlightLocationDealer_GUILD_COOPERATE_REWARD_POS_CAN_GET readVariable(string _str)
        {
            TutorialHighlightLocationDealer_GUILD_COOPERATE_REWARD_POS_CAN_GET dealer = new TutorialHighlightLocationDealer_GUILD_COOPERATE_REWARD_POS_CAN_GET();
            return dealer;
        }
    }
}
