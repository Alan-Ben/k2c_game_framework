using System;
using System.Text;

namespace ALPackage
{
    /**************
     * PHP交互工具类-Base64编码
     **/
    public class ALPHPAccessToolByBase64
    {

        public static string encode(string _value)
        {
            byte[] temp1 = Encoding.UTF8.GetBytes(_value);
            string temp2 = Convert.ToBase64String(temp1);
            return temp2;
        }
    }
}
