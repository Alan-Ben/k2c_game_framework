using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// unicode编码字符长度检查表
    /// </summary>
    public class NPUnicodeLengthCheckExportMenu : NPBasicExportMenuItem<NPUnicodeLengthCheckRefObj, NPUnicodeLengthCheckRefObj, NPGSOUnicodeLengthCheckRefSet>
    {

        public NPUnicodeLengthCheckExportMenu(string _tag, Func<string,string, bool> _judgeCanShowFunc)
            : base("unicode_length_check", ENPExportSettingEnum.UNICODE_LENGTH_CHECK, NPGSOUnicodeLengthCheckRefSet.assetPath, NPGSOUnicodeLengthCheckRefSet.objName, _tag, _judgeCanShowFunc)
        {
        }

        /****************
         * 显示的菜单文字
         **/
        protected override string _menuText { get { return "Unicode编码字符长度表(unicode_length_check)"; } }


        public override List<NPUnicodeLengthCheckRefObj> _exchangeTemplate(List<NPUnicodeLengthCheckRefObj> _tempList)
        {
            return _tempList;
        }

        public override void _readRefInfo()
        {
            obj.min_range_include = Convert.ToUInt16(GetString("min_range_include"), 16);
            obj.max_range_include = Convert.ToUInt16(GetString("max_range_include"), 16);
            obj.length = GetInt("length");
        }
    }
}