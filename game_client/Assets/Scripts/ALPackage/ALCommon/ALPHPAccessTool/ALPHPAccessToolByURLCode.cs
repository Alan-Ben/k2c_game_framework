

namespace ALPackage
{
    /**************
     * PHP交互工具类-URLCode
     **/
    public class ALPHPAccessToolByURLCode
    {

        public static string encode(string str)
        {
            string strTemp = "";
            int length = str.Length;
            for(int i = 0; i < length; i++)
            {
                if((str[i] >= 'A' && str[i] <= 'Z') ||
                    (str[i] >= 'a' && str[i] <= 'z') ||
                    (str[i] >= '0' && str[i] <= '9') ||
                    (str[i] == '-') ||
                    (str[i] == '_') ||
                    (str[i] == '.') ||
                    (str[i] == '~'))
                    strTemp += str[i];
                else if(str[i] == ' ')
                    strTemp += "+";
                else
                {
                    strTemp += '%';
                    strTemp += ToHex((char)(str[i] >> 4));
                    strTemp += ToHex((char)(str[i] % 16));
                }
            }
            return strTemp;
        }

        public static string decode(string str)
        {
            string strTemp = "";
            int length = str.Length;
            for(int i = 0; i < length; i++)
            {
                if(str[i] == '+')
                    strTemp += ' ';
                else if(str[i] == '%')
                {
                    char high = FromHex(str[++i]);
                    char low = FromHex(str[++i]);
                    strTemp += (char)(high * 16 + low);
                }
                else
                    strTemp += str[i];
            }
            return strTemp;
        }

        /************ 其他函数 **************************************************************************/
        private static char ToHex(char x)
        {
            return x > 9 ? (char)(x + 55) : (char)(x + 48);
        }

        private static char FromHex(char x)
        {
            char y;
            if(x >= 'A' && x <= 'Z')
                y = (char)(x - 'A' + 10);
            else if(x >= 'a' && x <= 'z')
                y = (char)(x - 'a' + 10);
            else if(x >= '0' && x <= '9')
                y = (char)(x - '0');
            else
                y = (char)0;
            return y;
        }
    }
}
