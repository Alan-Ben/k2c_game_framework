import io
import os
import string
from io import StringIO
import ply.lex as lex
import ply.yacc as yacc
from DataType import *
from mako.template import Template

g_root_path = ''
# 定义词法分析器规则
tokens = (
    'INTEGER',
    'LPAREN',
    'RPAREN',
    'ARRAY_TAG',
    'COMMENT',
    'LBRACKET',
    'RBRACKET',
    'LCURLY_BRACE',
    'RCURLY_BRACE',
    'SEMICOLON',
    'MODULE',
    'ACTION_TAG',
    'REQ',
    'RET',
    'PUSH_TAG',

    'CUSTOM_TYPE_NAME',
    'ID',
    'IGNORE_COMMENT',
    'SECTION_COMMENT',
    'AL_COMMENT',
)
# 保留关键字
reserved = {

    "long": "LONG",
    "double": "DOUBLE",
    "int": "INT",

    "float": "FLOAT",
    "short": "SHORT",
    "bool": "BOOL",
    "byte": "BYTE",
    "bytes": "BYTES",
    "string": "STRING",

}
tokens += tuple(reserved.values())


def t_ARRAY_TAG(t):
    r"""\[\]"""
    t.value = t.value
    return t


t_LPAREN = r'\('
t_RPAREN = r'\)'
t_LBRACKET = r'\['
t_RBRACKET = r'\]'
t_LCURLY_BRACE = r'\{'
t_RCURLY_BRACE = r'\}'
t_SEMICOLON = r'\;'
t_ignore = ' \t\r'

t_MODULE = r'(?i)\$module'
t_ACTION_TAG = r'(?i)\$action'
t_REQ = r'(?i)\$req'
t_RET = r'(?i)\$ret'
t_PUSH_TAG = r'(?i)\$push'


def t_newline(t):
    r"""\n+"""
    t.lexer.lineno += len(t.value)


def t_error(t):
    print(f'Illegal character: {t.value[0]}')
    t.lexer.skip(1)


def t_INTEGER(t):
    r"""\d+"""
    t.value = int(t.value)
    return t


def t_CUSTOM_TYPE_NAME(t):
    r"""\b[a-zA-Z_][a-zA-Z_0-9]*(?:\.[a-zA-Z_][a-zA-Z_0-9]*)+\b"""
    t.value = t.value
    return t


def t_COMMENT(t):
    r"""//.*"""
    t.value = t.value[2:].strip()
    return t


def t_IGNORE_COMMENT(t):
    r"""---.*"""
    pass


def t_SECTION_COMMENT(t):
    r"""\/\*[\s\S]*?\*\/"""
    pass


def t_ID(t):
    r"""[a-zA-Z_][a-zA-Z_0-9]*"""

    t.type = reserved.get(t.value.lower(), 'ID')
    return t

def t_AL_COMMENT(t):
    r"""\[.*\]"""
    t.value = t.value[1:-1].strip()
    return t
lexer = lex.lex()


# 定义语法分析器规则


def p_module(p):
    """
    module : MODULE  INTEGER ID mix_comment struct_list
    """
    module = Module(p[2], p[3])
    struct_list = []
    if len(p) == 5:
        struct_list = p[4]
    else:
        module.comment = p[4]
        struct_list = p[5]

    for struct in struct_list:
        if struct.__class__ == Action:
            module.action_list.append(struct)
        if struct.__class__ == PushProto:
            module.push_list.append(struct)
    p[0] = module


def p_struct_list(p):
    """
    struct_list : action
                | push
                | struct_list action
                | struct_list push
    """
    if len(p) == 2:
        p[0] = [p[1]]
    else:
        p[1].append(p[2])
        p[0] = p[1]


def p_action(p):
    """
       action : ACTION_TAG INTEGER ID mix_comment LPAREN req_body ret_body RPAREN
    """
    action = Action(p[2], p[3])
    if len(p) == 8:
        action.req_field_list.extend(p[5])
        action.ret_field_list.extend(p[6])
    else:
        action.req_field_list.extend(p[6])
        action.ret_field_list.extend(p[7])
        action.comment = p[4]
    p[0] = action


def p_empty(p):
    """
    empty :
    """
    pass




def p_req_body(p):
    """
        req_body : REQ LCURLY_BRACE field_list RCURLY_BRACE
    """
    p[0] = p[3]


def p_ret_body(p):
    """
        ret_body : RET LCURLY_BRACE field_list RCURLY_BRACE
    """
    p[0] = p[3]


def p_push(p):
    """
    push :  PUSH_TAG INTEGER ID mix_comment  LCURLY_BRACE field_list RCURLY_BRACE
    """
    push = PushProto(p[2], p[3])

    if len(p) == 7:
        push.field_list.extend(p[5])
    else:
        push.field_list.extend(p[6])
        push.comment = p[4]
    p[0] = push


def p_field_list(p):
    """
    field_list : field
                  | field_list field
                  | empty
        """
    if len(p) == 2:
        if p[1] is None:
            p[0] = []
        else:
            p[0] = [p[1]]
    else:
        p[1].append(p[2])
        p[0] = p[1]
        
def p_array_tag(p):
    """
    array_tag : ARRAY_TAG
              | empty
    """
    p[0] = p[1] is not None

def p_filed(p):
    """
    field : type array_tag ID  al_comment SEMICOLON comment
    """
    comment = p[4]
    if p[6] is not None and p[6] != '空' and len(p[6]) > 0:
        comment = p[6]
    p[0] = Field(p[3], p[1], p[2], comment)


def p_type(p):
    """
    type : raw_type
        | custom_type
    """
    p[0] = p[1]


def p_raw_type(p):
    """
    raw_type : LONG
        | DOUBLE
        | INT
        | FLOAT
        | SHORT
        | BOOL
        | BYTE
        | BYTES
        | STRING
    """
    p[0] = p[1]


def p_custom_type(p):
    """custom_type : CUSTOM_TYPE_NAME
    """
    p[0] = p[1]


def p_al_comment(p):
    """
    al_comment : AL_COMMENT
            | empty
    """
    if len(p) == 2:
        if p[1] is None:
            p[0] = '空'
        else:
            p[0] = p[1]
    else:
        p[0] = '空'


def p_comment(p):
    """
    comment : COMMENT
            | empty
    """
    if len(p) == 2:
        if p[1] is None:
            p[0] = '空'
        else:
            p[0] = p[1]
    else:
        p[0] = '空'


def p_mix_comment(p):
    """
    mix_comment : comment
                | al_comment
    """
    p[0] = p[1]

def p_error(p):
    if p:
        print("Syntax error at token", p)
    else:
        print("Syntax error at EOF")


def load_module(module_full_path) -> Module:
    fileHandler = open(module_full_path, "r", encoding='utf-8', newline='\n')
    lines = fileHandler.readlines()
    srcCode = ''.join(lines)
    lexer.input(srcCode)
    # while True:
    #     tok = lexer.token()
    #     if not tok:
    #         break
    #     print(tok)
    # pass
    parser = yacc.yacc()
    module = parser.parse(srcCode, lexer=lexer, tracking=True)
    return module


def gen_proto(path_dir, main_order, sub_order, proto_name, java_package, sharp_package, field_list, comment):
    fullFileName = path_dir + "/" + proto_name + ".alpro"
    if os.path.isfile(fullFileName):
        os.remove(fullFileName)
    import_list = set()

    for field in field_list:
        if '.' in field.data_type:
            import_list.add(field.data_type[0: field.data_type.rindex('.')])

    handlerFile = open(fullFileName, "wt", encoding="utf-8", newline='\n')
    template = Template(filename='MakoTemplate/proto.mako')
    code = template.render(proto_name=proto_name
                           , java_package=java_package
                           , sharp_package=sharp_package
                           , main_order=main_order
                           , sub_order=sub_order
                           , field_list=field_list
                           , comment=comment
                           , import_list=import_list
                           )
    handlerFile.write(code)
    handlerFile.close()


def gen_handler(handlerDir, handlerFileName, reqPackage, reqProtoName, retPackage, retProtoName, handlerPackage):
    fullFileName = handlerDir + "/" + handlerFileName + ".java"
    if os.path.isfile(fullFileName):
        # print("already exists req handler:", fullFileName)
        return
    handlerFile = open(fullFileName, "wt", encoding="utf-8", newline='\n')
    template = Template(filename='MakoTemplate/handler.mako')
    code = template.render(handlerFileName=handlerFileName
                           , reqPackage=reqPackage
                           , reqProtoName=reqProtoName
                           , retPackage=retPackage
                           , retProtoName=retProtoName
                           , handlerPackage=handlerPackage)
    handlerFile.write(code)
    handlerFile.close()


def getJavaType(data_type: str):
    if data_type == "bool":
        return "boolean"
    if data_type == "string":
        return "String"
    else:
        if '.' not in data_type:
            return data_type

        parts = data_type.rsplit(".", 1)
        if len(parts) > 1:
            return parseProtoJavaType(parts[0], parts[1])
        else:
            return data_type


java_proto_package_map = {}


def parseProtoJavaType(proto_file_name: str, type_name):
    if proto_file_name in java_proto_package_map:
        return java_proto_package_map.get(proto_file_name, "") + "." + type_name

    full_proto_file = "../ServerProtocol/ProtocolScripts/" + proto_file_name.replace('.', '/') + ".alpro"
    if not os.path.exists(full_proto_file):
        return type_name
    fileHandler = open(full_proto_file, "r", encoding="utf-8")
    for line in fileHandler.readlines():
        if 'JavaPackage' in line:
            package = line.split(' ', 1)[1].strip()
            java_proto_package_map[proto_file_name] = package.replace(';', '')
            break
    fileHandler.close()

    return java_proto_package_map.get(proto_file_name, "") + "." + type_name



def getJavaObjType(data_type:str):
    if data_type == "bool":
        return "Boolean"
    elif data_type == "int":
        return "Integer"
    elif data_type == "long":
        return "Long"
    elif data_type == "short":
        return "Short"
    elif data_type == "byte":
        return "Byte"
    elif data_type == "float":
        return "Float"
    elif data_type == "double":
        return "Double"
    else:
        if '.' not in data_type:
            return data_type

        parts = data_type.rsplit(".", 1)
        if len(parts) > 1:
            return parseProtoJavaType(parts[0], parts[1])
        else:
            return data_type


def gen_cmd(_action: Action, req_package, req_proto_name):
    """
    生成客户端命令
    """
    cmd = ClientCmd()
    cmd.name = _action.name

    param_list = ''
    body = '%s msg = new %s();\n' % (req_proto_name, req_proto_name)
    comment = _action.comment + '('

    # 生成注释列表 (种族id，技能位)
    for i in range(len(_action.req_field_list)):
        field = _action.req_field_list[i]
        comment += "%s," % field.comment
    if len(_action.req_field_list) > 0:
        comment = comment[0:-1]
    comment += ')'
    cmd.comment = comment

    # 生成参数列表 long raceId,int slot
    for i in range(len(_action.req_field_list)):
        field = _action.req_field_list[i]
        if field.is_array:
            param_list += "List<%s> _param%d," % (getJavaObjType(field.data_type), i)
        else:
            param_list += "%s _param%d," % (getJavaType(field.data_type), i)
    if len(param_list) > 0:
        param_list = param_list[0:-1]
    cmd.param_list = param_list

    # 生成body
    for i in range(len(_action.req_field_list)):
        field = _action.req_field_list[i]
        if field.is_array:
            body += "\t\tmsg.get%s().addAll(_param%d);\n" % (upper_first(field.name), i)
        else:
            body += "\t\tmsg.set%s(_param%d);\n" % (upper_first(field.name), i)
    body += "\t\tgetOwner().sendGameMsg(msg);\n"
    cmd.body = body

    cmd.req_package = req_package
    cmd.req_proto_name = req_proto_name
    return cmd


def gen_client_commander(module, cmd_List):
    class_name = 'Cmd%s' % module.module_name
    fullFileName = "../Client/src/MGClient/Cmd/Cmds/" + class_name + ".java"
    if os.path.isfile(fullFileName):
        os.remove(fullFileName)
    handlerFile = open(fullFileName, "wt", encoding="utf-8", newline='\n')
    template = Template(filename='MakoTemplate/client_cmd.mako')

    importList = ''
    for cmd in cmd_List:
        importList += 'import %s.%s;\n' % (cmd.req_package, cmd.req_proto_name)

    code = template.render(class_name=class_name
                           , importList=importList
                           , cmd_List=cmd_List
                           , name=module.module_name
                           , comment=module.comment
                           )
    handlerFile.write(code)
    handlerFile.close()


def gen_client_register(module, retPackage, proto_List):
    class_name = 'GSMsgRegister_%s' % module.module_name
    fullFileName = "../Client/src/MGClient/GSListener/" + class_name + ".java"
    if os.path.isfile(fullFileName):
        os.remove(fullFileName)
    handlerFile = open(fullFileName, "wt", encoding="utf-8", newline='\n')
    template = Template(filename='MakoTemplate/client_register.mako')

    importList = ''
    for proto_name in proto_List:
        importList += 'import %s.%s;\r\n' % (retPackage, proto_name)

    code = template.render(class_name=class_name
                           , importList=importList
                           , proto_List=proto_List
                           )
    handlerFile.write(code)
    handlerFile.close()


def gen_auto_regist_class(handler_dir, class_name, handler_package):
    fullFileName = handler_dir + "/" + class_name + ".java"
    if os.path.isfile(fullFileName):
        return
    handlerFile = open(fullFileName, "wt", encoding="utf-8", newline='\n')
    template = Template(filename='MakoTemplate/auto_regist.mako')
    code = template.render(handler_package=handler_package
                           , class_name=class_name
                           )
    handlerFile.write(code)
    handlerFile.close()


# 清除指定目录下所有指定类型的文件
def clear_files_by_type(dir_path, file_type):
    for filename in os.listdir(dir_path):
        if filename.endswith(file_type):
            file_path = os.path.join(dir_path, filename)
            os.remove(file_path)


def gen_module(module: Module) -> str:
    sb = StringIO()
    # 创建协议脚本目录
    protoScriptPathDir = "../ServerProtocol/ProtocolScripts/GC2GS/p%03d_%sOp" % (
        module.main_order, module.module_name)
    if not os.path.exists(protoScriptPathDir):
        os.makedirs(protoScriptPathDir)  # 循环创建目录

    clear_files_by_type(protoScriptPathDir, '.alpro')

    # 协议处理函数包名
    handlerPackage = " NPUSServer.NPUserMsgDispather.p%03d_%sOp" % (module.main_order, module.module_name)
    # 协议处理函数目录
    handlerDir = "../UserServer/src/NPUSServer/NPUserMsgDispather/p%03d_%sOp" % (
        module.main_order, module.module_name)
    if len(module.action_list) > 0:
        if not os.path.exists(handlerDir):
            os.makedirs(handlerDir)  # 循环创建目录

    client_cmd_list = []  # 测试命令列表
    retProtoList = []  # 回包列表

    # 请求协议的包名
    reqPackage = "GC2GS.p%03d_%sOp" % (module.main_order, module.module_name)
    # 返回协议的包名
    retPackage = "GS2GC.p%03d_%sOp" % (module.main_order, module.module_name)

    # 处理请求列表
    for action in module.action_list:
        # 生成请求协议
        reqProtoName = "GC2GS_%03d_%03d_Req%s" % (module.main_order, action.sub_order, action.name)
        # 生成协议脚本
        gen_proto(protoScriptPathDir
                  , module.main_order
                  , action.sub_order
                  , reqProtoName
                  , reqPackage
                  , reqPackage
                  , action.req_field_list
                  , action.comment)

        # 生成返回协议
        retProtoName = "GS2GC_%03d_%03d_Ret%s" % (module.main_order, action.sub_order, action.name)
        gen_proto(protoScriptPathDir
                  , module.main_order
                  , action.sub_order
                  , retProtoName
                  , retPackage
                  , retPackage
                  , action.ret_field_list
                  , action.comment)

        # 生成请求处理函数
        handlerFileName = "MsgDealer_GC2GS_%03d_%03d_Req%s" % (module.main_order, action.sub_order, action.name)
        gen_handler(handlerDir, handlerFileName, reqPackage, reqProtoName, retPackage, retProtoName, handlerPackage)

        # 生成客户端cmd
        client_cmd_list.append(gen_cmd(action, reqPackage, reqProtoName))

        # 加入返回协议列表
        retProtoList.append(retProtoName)

        sb.write(reqProtoName + "\n")
        sb.write(retProtoName + "\n\n")

    # 处理推送列表
    for push in module.push_list:
        # 生成协议文件
        pushProtoName = "GS2GC_%03d_%03d_%s" % (module.main_order, push.sub_order, push.name)
        gen_proto(protoScriptPathDir
                  , module.main_order
                  , push.sub_order
                  , pushProtoName
                  , retPackage
                  , retPackage
                  , push.field_list
                  , push.comment)
        # 加入返回协议列表
        retProtoList.append(pushProtoName)
        sb.write(pushProtoName + "\n")

    # 生成自动注册协议类
    # gen_auto_regist_class(handlerDir, "zzz_%s_AutoRegistPlaceHolder" % module.module_name, handlerPackage)

    # 生成客户端测试命令
    gen_client_commander(module, client_cmd_list)
    # 生成协议注册命令
    gen_client_register(module, retPackage, retProtoList)

    return sb.getvalue()


# 生成所有模块
def gen_all(root_path):
    for root, dirs, files in os.walk(os.getcwd()):
        for f in files:
            if f.endswith(".module"):
                fullPath = os.path.join(root, f)
                print("---------------------start load module:" + fullPath + '---------------------')
                module = load_module(fullPath)
                print("---------------------start gen module:" + module.module_name + '---------------------')
                protoList = gen_module(module)
                # 生成协议列表文件
                proto_name_file = os.path.join(root, f[0:-7] + ".proto_list")
                txtFile = open(proto_name_file, "wt", encoding="utf-8", newline='\n')
                txtFile.write(protoList)
                txtFile.close()


gen_all("src")
os.chdir("../ServerProtocol/ProtocolScripts")  # 切换到协议脚本目录
os.popen("ALProtocolMaker.exe")  # 执行协议脚本生成器
