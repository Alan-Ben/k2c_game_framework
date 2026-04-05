package NPCommon.RefData.Ref;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Target(ElementType.FIELD)
@Retention(RetentionPolicy.RUNTIME)
public @interface RefField
{
    /**
     * 是否忽略此field
     * @return
     */
    boolean isIgnore() default false;

    /************
     * 当为队列的时候用此字符串划分
     *
     * @author alzq.z
     * @time 2019年4月3日 下午11:04:26
     */
    String arrayToken() default ";";
}
