JavaPackage ${java_package};
csharpspace ${sharp_package};
% for ss in import_list:
import ${ss}.alpro;
% endfor
ALProtocol ${proto_name} [${comment}] <${main_order}, ${sub_order}>
{
    % for field in field_list:
        %if field.is_array:
    ${field.data_type}[] ${field.name}[${field.comment}];
        %else:
    ${field.data_type} ${field.name}[${field.comment}];
        %endif
    % endfor
}
