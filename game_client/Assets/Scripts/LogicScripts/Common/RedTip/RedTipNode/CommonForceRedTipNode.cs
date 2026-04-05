namespace GOE
{
    /// <summary>
    /// 强制红点节点
    /// </summary>
    public class CommonForceRedTipNode : _ARedTipNode
    {
        public CommonForceRedTipNode(string _saveKey) : base(_saveKey)
        {
        }
        /// <summary>
        /// 强制红点当count>0是显示
        /// </summary>
        /// <returns></returns>
        protected override bool _needShow()
        {
            return getCount() > 0;
        }
    }
}