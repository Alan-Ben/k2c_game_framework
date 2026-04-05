using System;
using System.Collections.Generic;

namespace GOE
{
    public class ChildNameRefExportMenu : NPMultiLangBasicExportMenuItem<ChildNameRefObj, ChildNameRefObj, GSOChildNameRefSet>
    {

        public ChildNameRefExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
            : base(_tag, GSOChildNameRefSet.languages, ENPExportSettingEnum.CHILD_NAME, _judgeCanShowFunc)
        {
        }

        /****************
         * 显示的菜单文字
         **/
        protected override string _menuText { get { return "子嗣起名表(child_name)"; } }

        protected override string getAssetPath(ENPLanguage _language) { return MultiLanguageAsset.getLanguageAssetPath(_language, GSOChildNameRefSet.objName); }

        protected override string getAssetObjName(ENPLanguage _language) { return MultiLanguageAsset.getLanguageAssetObjName(_language, GSOChildNameRefSet.objName); }

        protected override string getTableName(ENPLanguage _language, string _tableName)
        {
            if (_language == ENPLanguage.ZH_TW)
            {
                return $"{_tableName}_tw_tw";
            }
            return $"{_tableName}_{_language.ToString().ToLowerInvariant()}";
        }


        public override List<ChildNameRefObj> _exchangeTemplate(List<ChildNameRefObj> _tempList)
        {
            return _tempList;
        }

        public override void _readRefInfo()
        {
            obj.id = GetLong("id");

            obj.girl_name = GetString("girl_name", false);
            obj.boy_name = GetString("boy_name", false);
        }
    }
}