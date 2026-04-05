package NPCommon.RefData.Ref;

import java.lang.annotation.ElementType;
import java.lang.annotation.Retention;
import java.lang.annotation.RetentionPolicy;
import java.lang.annotation.Target;

@Target(ElementType.TYPE)
@Retention(RetentionPolicy.RUNTIME)
public @interface RefTable
{
    String tableName() default "";

    boolean isSingletonKey() default true;

    boolean canbeEmpty() default true;

    boolean ignore() default false;
}
