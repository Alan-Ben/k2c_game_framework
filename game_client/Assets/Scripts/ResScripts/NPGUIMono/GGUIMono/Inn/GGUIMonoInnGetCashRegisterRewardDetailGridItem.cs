using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    public class GGUIMonoInnGetCashRegisterRewardDetailGridItem : _TALUGUIMonoGridItem
    {
        [ALHeader("菜品 icon ")]
        public RawImage imgDishIcon;
        [ALHeader("菜品名称")]
        public Text txtDishName;
        [ALHeader("宾客数量")]
        public Text txtGuestNum;
        [ALHeader("熟练度")]
        public Text txtFinesseAdd;
    }
}