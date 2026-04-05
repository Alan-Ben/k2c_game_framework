using JetBrains.Annotations;

namespace GOE
{
    public class MarsPowerPropertyShow : _IPropertyShow
    {
        private static MarsPowerPropertyShow _g_instance;
        [NotNull] public static MarsPowerPropertyShow instance { get { return _g_instance ??= new MarsPowerPropertyShow(); } }

        private MarsPowerPropertyShow()
        {
        }
        
        public string simpleName { get { return TextTranslate.instance.getLanguage(TransKeyConst.mars_powerSimpleName_none); } }
        public bool isAddPer { get { return false; } }
    }
}