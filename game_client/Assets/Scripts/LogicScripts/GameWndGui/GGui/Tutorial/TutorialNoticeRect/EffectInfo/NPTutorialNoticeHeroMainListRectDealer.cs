using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 骑士主页面列表位置
    /// </summary>
    public class NPTutorialNoticeHeroMainListRectDealer : _AWCGTutorialNoticeRectDealer
    {
        private EHeroMainTargetHeroType _m_targetHeroType;

        public override EWCGTutorialNoticeRectType noticeRectType { get { return EWCGTutorialNoticeRectType.HERO_MAIN_LIST_P; } }

        public override bool getRect(out Rect _rect)
        {
            RectTransform rectTransform = GGUIWndHeroMain.instance?.getTargetHeroRectTransform(_m_targetHeroType);
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

        public static NPTutorialNoticeHeroMainListRectDealer readVariable(string _str)
        {
            NPTutorialNoticeHeroMainListRectDealer effectObj = new NPTutorialNoticeHeroMainListRectDealer();

            // 解析枚举类型
            if (!string.IsNullOrEmpty(_str))
            {
                if (ALCommon.TryEnumParse(typeof(EHeroMainTargetHeroType), _str, out EHeroMainTargetHeroType _targetType))
                {
                    effectObj._m_targetHeroType = _targetType;
                }
            }

            return effectObj;
        }
    }
}

