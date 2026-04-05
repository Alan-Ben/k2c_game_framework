using System;
using System.Collections.Generic;

using ALPackage;

/******************
 * 翻译参数存储cache对象，避免创建过多对象
 **/
public class LanguageArgListCache : _AALUnsafeCacheController<List<object>, List<object>>
{
    public LanguageArgListCache() : base(1, 5)
    {
        init(new List<object>());
    }

    //警告信息文字
    protected override string _warningTxt { get { return "NPLanguageArgListCache"; } }

    protected override List<object> _createItem(List<object> _template)
    {
        return new List<object>();
    }

    protected override void _discardItem(List<object> _item)
    {
        if (_item != null) 
            _item.Clear();
        return;
    }

    protected override void _onInit(List<object> _template)
    {
    }

    protected override void _resetItem(List<object> _item)
    {
        if (_item != null) 
            _item.Clear();
    }
}
