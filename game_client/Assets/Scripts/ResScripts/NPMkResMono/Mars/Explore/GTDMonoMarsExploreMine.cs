using System.Collections.Generic;
using ALPackage;
using UnityEngine;

namespace GOE
{
    public class GTDMonoMarsExploreMine : MonoBehaviour
    {
        [ALHeader("跟随目标")]
        public Transform followTarget;
        [ALHeader("点击对象")]
        public GTDCommonPosClickMono monoClick;
        [ALHeader("各种对象下显示的内容")]
        public List<GameObject> listEmptyShow;
        public List<GameObject> listOccupiedByMeShow;
        public List<GameObject> listOccupiedByOthersShow;


        public void setState(bool _isEmpty, bool _isOccupiedByMe)
        {
            ALUGUICommon.setGameObjEnable(listEmptyShow, false);
            ALUGUICommon.setGameObjEnable(listOccupiedByMeShow, false);
            ALUGUICommon.setGameObjEnable(listOccupiedByOthersShow, false);
            
            if (_isEmpty)
                ALUGUICommon.setGameObjEnable(listEmptyShow, true);
            else
                ALUGUICommon.setGameObjEnable(_isOccupiedByMe ? listOccupiedByMeShow : listOccupiedByOthersShow, true);
        }
    }
}
