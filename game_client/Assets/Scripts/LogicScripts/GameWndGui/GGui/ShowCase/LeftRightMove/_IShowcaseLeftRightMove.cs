namespace GOE
{
    /// <summary>
    /// 左右滑动showcase的数据接口
    /// </summary>
    public interface _IShowcaseLeftRightMove
    {
        //唯一id
        public long id { get; }
        //单位模型
        public NPGGoIndex unitIndex { get; }
        //背景模型
        public NPGGoIndex bgIndex { get; }
    }
}