using System.Collections.Generic;
using LitJson;

namespace GOE
{
    public static class InjectFixCustomConst
    {
        public static bool customBool_1;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static bool customBool_2;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static bool customBool_3;//判断这次切换服务器是不是切换大区的切服
        public static bool customBool_4;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static bool customBool_5;//记录isApplicationFocused，用于检测主线程是否未响应
        public static bool customBool_6;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static bool customBool_7;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static bool customBool_8;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static bool customBool_9;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        
        public static int customInt_1;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static int customInt_2;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static int customInt_3;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static int customInt_4;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static int customInt_5;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static int customInt_6;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static int customInt_7;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static int customInt_8;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static int customInt_9;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        
        public static long customLong_1;//FixUpdate的时候++用于检测主线程是否未响应
        public static long customLong_2;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static long customLong_3;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static long customLong_4;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static long customLong_5;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static long customLong_6;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static long customLong_7;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static long customLong_8;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static long customLong_9;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        
        public static string customString_1;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static string customString_2;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static string customString_3;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static string customString_4;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static string customString_5;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static string customString_6;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static string customString_7;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static string customString_8;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static string customString_9;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
             
        public static float customFloat_1;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static float customFloat_2;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static float customFloat_3;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static float customFloat_4;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static float customFloat_5;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static float customFloat_6;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static float customFloat_7;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static float customFloat_8;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static float customFloat_9;//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        
        
        public static List<bool> customBoolList_1 = new List<bool>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static List<bool> customBoolList_2 = new List<bool>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static List<bool> customBoolList_3 = new List<bool>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        
        public static List<int> customIntList_1 = new List<int>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static List<int> customIntList_2 = new List<int>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static List<int> customIntList_3 = new List<int>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        
        public static List<long> customLongList_1 = new List<long>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static List<long> customLongList_2 = new List<long>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static List<long> customLongList_3 = new List<long>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        
        public static List<string> customStringList_1 = new List<string>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static List<string> customStringList_2 = new List<string>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static List<string> customStringList_3 = new List<string>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
             
        public static List<float> customFloatList_1 = new List<float>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static List<float> customFloatList_2 = new List<float>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static List<float> customFloatList_3 = new List<float>();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        
        public static LitJson.JsonData customJsonData_1 = new JsonData();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static LitJson.JsonData customJsonData_2 = new JsonData();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static LitJson.JsonData customJsonData_3 = new JsonData();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static LitJson.JsonData customJsonData_4 = new JsonData();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
        public static LitJson.JsonData customJsonData_5 = new JsonData();//专门为InjectFix保留的自定义变量，需要用到的时候请修改名称和注释
    }
}