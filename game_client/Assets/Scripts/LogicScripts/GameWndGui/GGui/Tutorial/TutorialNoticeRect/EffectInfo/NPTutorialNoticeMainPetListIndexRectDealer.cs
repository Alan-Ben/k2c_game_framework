using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    public class NPTutorialNoticeMainPetListIndexRectDealer : _AWCGTutorialNoticeRectDealer
    {
        private int _m_index; //卡牌ID

        public override EWCGTutorialNoticeRectType noticeRectType { get { return EWCGTutorialNoticeRectType.MAIN_PET_LIST_P; } }

        public override bool getRect(out Rect _rect)
        {
            //获取单位的Rect
            // RectTransform rectTransform = GGUIWndHeroMain.instance.getMainPetRectByIndex(_m_index);
            RectTransform rectTransform = null;
            if (null == rectTransform)
            {
                _rect = new Rect();
                return false;
            }
            _rect = rectTransform.rect;
            return true;
        }

        public static NPTutorialNoticeMainPetListIndexRectDealer readVariable(string _str)
        {
            NPTutorialNoticeMainPetListIndexRectDealer effectObj = new NPTutorialNoticeMainPetListIndexRectDealer();

            effectObj._m_index = int.Parse(_str);

            return effectObj;
        }
    }
}
