using System;
using System.Collections.Generic;

namespace GOE
{
    public class NPPlayerNameRefExportMenu : NPMultiLangBasicExportMenuItem<PlayerNameRefObj, PlayerNameRefObj, GSOPlayerNameRefSet>
    {

        public NPPlayerNameRefExportMenu(string _tag, Func<string, string, bool> _judgeCanShowFunc)
            : base(_tag, GSOPlayerNameRefSet.languages, ENPExportSettingEnum.PLAYER_NAME, _judgeCanShowFunc)
        {
        }

        /****************
         * 显示的菜单文字
         **/
        protected override string _menuText { get { return "玩家起名表(player_name)"; } }

        protected override string getAssetPath(ENPLanguage _language) { return MultiLanguageAsset.getLanguageAssetPath(_language, GSOPlayerNameRefSet.objName); }

        protected override string getAssetObjName(ENPLanguage _language) { return MultiLanguageAsset.getLanguageAssetObjName(_language, GSOPlayerNameRefSet.objName); }

        protected override string getTableName(ENPLanguage _language, string _tableName)
        {
            if (_language == ENPLanguage.ZH_TW)
            {
                return $"{_tableName}_tw_tw";
            }
            return $"{_tableName}_{_language.ToString().ToLowerInvariant()}";
        }


        public override List<PlayerNameRefObj> _exchangeTemplate(List<PlayerNameRefObj> _tempList)
        {
            return _tempList;
        }

        public override void _readRefInfo()
        {
            obj.id = GetLong("id");

            obj.suffix_name = GetString("suffix_name", false);//名称

            obj.prefix_name = GetString("prefix_name", false);//称谓
        }
    }
}