using GOE;
namespace ${name_space}
{
/****
 ${err_file.comment}
 ****/
    class ${err_file.class_name}
    {
        static ${err_file.class_name}()
        {
% for line in err_file.lines:
            ProtocolErrorCodeResult.instance.RegistResult(new RESULT(${line.code}, "${line.msg}"));
% endfor
        }
    }
}
