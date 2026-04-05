namespace GOE
{
    /// <summary>
    /// 条件描述显示接口
    /// </summary>
    public interface _IConditionDescShow
    {
        NPGTextureIndex conditionIcon { get; }

        string conditionDesc { get; }
        
        bool conditionIsEnable { get; }

        void jumpFunc();
    }
}