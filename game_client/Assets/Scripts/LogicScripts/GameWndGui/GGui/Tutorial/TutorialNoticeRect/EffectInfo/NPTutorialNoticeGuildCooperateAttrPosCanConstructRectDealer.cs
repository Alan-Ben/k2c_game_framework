using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟协作属性据点可建造位置
    /// </summary>
    public class NPTutorialNoticeGuildCooperateAttrPosCanConstructRectDealer : _AWCGTutorialNoticeRectDealer
    {
        public override EWCGTutorialNoticeRectType noticeRectType { get { return EWCGTutorialNoticeRectType.GUILD_COOPERATE_ATTR_POS_CAN_CONSTRUCT; } }

        public override bool getRect(out Rect _rect)
        {
            RectTransform rectTransform = GGUIWndGuildCooperateMain.instance?.getCanConstructAttrPosRectTransform();
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

        public static NPTutorialNoticeGuildCooperateAttrPosCanConstructRectDealer readVariable(string _str)
        {
            NPTutorialNoticeGuildCooperateAttrPosCanConstructRectDealer effectObj = new NPTutorialNoticeGuildCooperateAttrPosCanConstructRectDealer();
            return effectObj;
        }
    }
}

