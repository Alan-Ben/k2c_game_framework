
using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GOE
{
    public enum GGUIMonoChildContainerItemShowType
    {
        HaveChild,
        Empty,
        Lock,
    }
    [Serializable]
    public class GGUIMonoChildContainerItemHaveChild
    {
        [ALHeader("显示的列表")]
        public List<GameObject> showList;
        [ALHeader("子嗣的信息")]
        public GGUIMonoChildInfo monoChildInfo;
        [ALHeader("上课获得大臣经验使用的上浮 tip id ")]
        public long oneKeyEducateCollectTipId = 3;
        [ALHeader("一键上课的效果位置")]
        public Transform transOneKeyEffectPoint;
        [ALHeader("一键上课每次上漂粒子数量")]
        public int oneKeyParticleNum;
        [ALHeader("上课特效相关")]
        public Transform educateSfxParent;
        public long educateSfxId;
        public int maxSfxCount = 6;
    }
    [Serializable]
    public class GGUIMonoChildContainerItemEmpty
    {
        public List<GameObject> showList;
    }
    [Serializable]
    public class GGUIMonoChildContainerItemLock
    {
        public List<GameObject> showList;
    }
    public class GGUIMonoChildContainerItem : _AALBasicUIWndMono
    {
        [ALHeader("点击按钮")]
        public GameObject btnClick;
        [ALHeader("有子嗣入驻时的显示")]
        public GGUIMonoChildContainerItemHaveChild haveChildShow;
        [ALHeader("空位时的显示")]
        public GGUIMonoChildContainerItemEmpty emptyShow;
        [ALHeader("未解锁时的显示")]
        public GGUIMonoChildContainerItemLock lockShow;
        [ALHeader("解锁条件描述")]
        public Text txtUnlockDesc;
        [ALHeader("选中和未选中时分别要展示的对象")]
        public List<GameObject> selectShow;
        public List<GameObject> unselectShow;
        [ALHeader("脑力回复的上浮 tip 位置和 tip id ")]
        public Transform energyRecoverEffectPoint;
        public long energyRecoverTipId = 1;
        public long particleId = 0;
        [ALHeader("如果可以毕业展示的对象")]
        public List<GameObject> listCanGraduateShow;
        [ALHeader("有红点时显示的对象")]
        public List<GameObject> listRedShow;


        public void setType(GGUIMonoChildContainerItemShowType _type)
        {
            ALUGUICommon.setGameObjEnable(haveChildShow.showList, false);
            ALUGUICommon.setGameObjEnable(emptyShow.showList, false);
            ALUGUICommon.setGameObjEnable(lockShow.showList, false);

            switch (_type)
            {
                case GGUIMonoChildContainerItemShowType.HaveChild:
                    ALUGUICommon.setGameObjEnable(haveChildShow.showList, true);
                    break;
                case GGUIMonoChildContainerItemShowType.Empty:
                    ALUGUICommon.setGameObjEnable(emptyShow.showList, true);
                    break;
                case GGUIMonoChildContainerItemShowType.Lock:
                    ALUGUICommon.setGameObjEnable(lockShow.showList, true);
                    break;
            }
        }
        public void setSelect(bool _isSelect)
        {
            ALUGUICommon.setGameObjEnable(selectShow, _isSelect);
            ALUGUICommon.setGameObjEnable(unselectShow, !_isSelect);
        }
        public void setCanGraduate(bool _enable)
        {
            ALUGUICommon.setGameObjEnable(listCanGraduateShow, _enable);
        }
        public void setRedTipShow(bool _show)
        {
            ALUGUICommon.setGameObjEnable(listRedShow, _show);
        }
    }
}