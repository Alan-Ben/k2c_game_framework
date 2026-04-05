using System.Collections.Generic;

namespace GOE
{
    public class NPGGUIWndFuncUnlockTipContainer : _ATNPGGUIWndShowAnimContainer<NPGGUIMonoFuncUnlockTipItem, NPGGUIMonoFuncUnlockTipContainer, NPGGUIWndFuncUnlockTipItem>
    {
        private List<NPGGUIWndFuncUnlockTipItem> _m_lItemList;


        public NPGGUIWndFuncUnlockTipContainer(NPGGUIMonoFuncUnlockTipContainer _mono) : base(_mono)
        {
            initWnd();
        }

        protected override NPGGUIWndFuncUnlockTipItem _createItemWnd(NPGGUIMonoFuncUnlockTipItem _itemMono)
        {
            return new NPGGUIWndFuncUnlockTipItem(_itemMono);
        }

        protected override void _onShowWnd()
        {

        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_lItemList?.Clear();
        }

        protected override void _onDiscard()
        {
            _m_lItemList?.Clear();
            _m_lItemList = null;
        }

        protected override void _onWndInitDone()
        {
            _m_lItemList = new List<NPGGUIWndFuncUnlockTipItem>();
        }

        /// <summary>
        /// 显示item列表
        /// </summary>
        /// <param name="_accessInfoList"></param>
        public void showItemList(List<FuncUnlockInfo> _accessInfoList)
        {
            if (_accessInfoList == null || _m_lItemList == null)
                return;

            NPGGUIWndFuncUnlockTipItem itemWnd = null;
            int count = 0;
            //遍历玩家数据
            for (int i = 0; i < _accessInfoList.Count; i++)
            {
                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                    itemWnd = _m_lItemList[i];
                itemWnd.showWnd();
                itemWnd.setInfo(_accessInfoList[i]);
                count++;
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= count; j--)
            {
                //移除窗口
                removeItemWnd(_m_lItemList[j]);
                //从队列删除
                _m_lItemList.RemoveAt(j);
            }
        }
    }
}
