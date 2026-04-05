using System.Collections.Generic;
using ALPackage;
using NPEnum;

namespace GOE
{
    [System.Serializable]
    public class FunctionUnlockRefObj : _IALBasicRefObj
    {
        public long _refId
        {
            get { return (long)type; }
        }

        public ENPFunctionType type; //功能类型
        public string name;//名字
        public string desc;//描述(预告界面描述)
        public List<string> desc_args;//描述参数(预告界面描述)
        public NPGTextureIndex tex_icon;//图标
        public NPGTextureIndex tex_banner;//系统图片
        public long simple_unlock_id;//对应simple_unlock_ref表的id
        public bool ignore_pop_unlock_tip;//是否忽略解锁提示及解锁表现
        public bool ignore_function_list_show;//是否在系统功能列表里不展示，true不展示
        public long sort_id;//排序id
        public _NPPlayerEffectSerializeInfo go_to;//跳转
        public EValueFormatType process_num_format;//进度值格式化显示方式
        public _NPPlayerVariableSerializeInfo process_cur_num;//进度当前值高级公式
        public long process_max_num;//进度值最大值
        public EFuncBelongType func_belong_type;//功能隶属于哪个主界面（ROOM:卧室  CITY:主城  BOTH:两者都行）
        public long entry_point_id;//对应入口点ID
        public _NPPlayerEffectSerializeInfo end_deal_effect;//弹窗结束时候执行的effect
        public long tutorial_id;//解锁表现后需要触发的引导id
        public List<NPCommonCostItem> gain_item_list;//解锁奖励
        public float focus_camera_view_value;//需要拉近时摄像机视野值（如果是正交相机就是OrthographicSize的值，如果是透视相机就是FieldOfView）
        public long unlock_order_id;//解锁顺序id，数值小优先展示
    }


    public class GSOFunctionUnlockRefSet : _TALSOBasicRefSet<FunctionUnlockRefObj>
    {
        public static string assetPath { get { return "refdata/game_refdata.unity3d"; } }
        public static string objName { get { return "func_unlock"; } }
    }
}