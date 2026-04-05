package NPCommon.CommonMsg;

import NPCommon.Util.Delegate.HandlerTwo;

import java.util.ArrayList;

//todo 待优化 load从mrg抽离出来
public abstract class ComMsgLoaderBase<T extends ComMsgBase>
{
    public abstract ArrayList<T> load(long _index, int _num, HandlerTwo<Boolean, T> _handler);
}
