using NPEnum;

namespace GOE
{
    /// <summary>
    /// 一个可以储存 common item 的容器
    /// </summary>
    public interface _ICommonItemContainer
    {
        int getItemNum(ENPItemType _type, long _subId);
    }
}