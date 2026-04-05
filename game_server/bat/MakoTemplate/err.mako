package ${java_package};
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result._IErrHolder;

/****
 ${err_file.comment}
 ****/
 
public class ${err_file.class_name} implements _IErrHolder
{
% for line in err_file.lines:
    public static final Result ${line.name} = Result.constInit(${line.code},"${line.msg}");
% endfor
}
