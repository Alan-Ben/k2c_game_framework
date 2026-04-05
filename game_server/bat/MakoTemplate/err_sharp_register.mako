using ${conf.name_space};

namespace ${conf.name_space}
{
    class ${conf.class_name}
    {
        static ${conf.class_name}()
        {
% for class_name in class_name_list:
            new ${class_name}();
% endfor
        }
    }
}
