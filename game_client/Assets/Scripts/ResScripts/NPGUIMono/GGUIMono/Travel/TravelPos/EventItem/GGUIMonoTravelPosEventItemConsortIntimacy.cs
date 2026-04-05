using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 游历地点事件item - 妃子亲密度类型Mono
    /// </summary>
    public class GGUIMonoTravelPosEventItemConsortIntimacy : _AGGUIMonoTravelPosEventItem
    {
        [ALHeader("妃子信息")]
        public GGUIMonoConsortCardItem monoConsortCard;
        [ALHeader("妃子详情按钮")]
        public GameObject btnConsortInfo;
        
        [ALHeader("妃子亲密度增加值文本")]
        public TextEx txtAddIntimacyValue;
        [ALHeader("妃子亲密度增加值文本key(一个参数, 增加的亲密度值)")]
        public string txtAddIntimacyValueKey;
    }
}
