# coding=utf-8
import os
import io
import shutil
import sys
import re
import copy
if sys.version>'3':
    from importlib import reload
#支持类型
#             "int"
#             "bool"
#             "long"
#             "timestamp"
#             "string"
#             "bytes" (log表不支持)
#             "string()[]"
#             "text"
#             "decimal(precision,scale)" - 例如: decimal(20,2)
typedef = {"bool":{"java":{"type":"boolean","list":"Boolean","default":"false","restore":"Boolean"},"sql":{"type":"tinyint","length":1,"default":"0"}},
"short":{"java":{"type":"short","list":"Short","default":"0","restore":"Short"},"sql":{"type":"smallint","length":"10","default":"0"}},
"int":{"java":{"type":"int","list":"Integer","default":"0","restore":"Int"},"sql":{"type":"int","length":11,"default":"0"}},
"long":{"java":{"type":"long","list":"Long","default":"0L","restore":"Long"},"sql":{"type":"bigint","length":20,"default":"0"}},
"char":{"java":{"type":"String","list":"String","default":'""',"restore":"String"},"sql":{"type":"char","length":50,"default":""}},
"varchar":{"java":{"type":"String","list":"String","default":'""',"restore":"String"},"sql":{"type":"varchar","length":500,"default":""}},
"text":{"java":{"type":"String","list":"String","default":'""',"restore":"String"},"sql":{"type":"text","length":0,"default":""}},
"mtext":{"java":{"type":"String","list":"String","default":'""',"restore":"String"},"sql":{"type":"mediumtext","length":0,"default":""}},
"string":{"java":{"type":"String","list":"String","default":'""',"restore":"String"},"sql":{"type":"varchar","length":500,"default":""}},
"bytes":{"java":{"type":"byte[]","list":"byte[]","default":"null","restore":"Bytes"},"sql":{"type":"blob","length":0,"default":""}},
"varbinary":{"java":{"type":"byte[]","list":"byte[]","default":"null","restore":"Bytes"},"sql":{"type":"varbinary","length":64,"default":""}},
"float":{"java":{"type":"float","list":"Float","default":"0.0f","restore":"Float"},"sql":{"type":"float","length":0,"default":"0.0"}},
"double":{"java":{"type":"double","list":"Double","default":"0.0d","restore":"Double"},"sql":{"type":"double","length":0,"default":"0.0"}},
"decimal":{"java":{"type":"double","list":"Double","default":"0.0d","restore":"Double"},"sql":{"type":"decimal","length":"10,2","default":"0.0"}},
"mblob":{"java":{"type":"byte[]","list":"byte[]","default":"null","restore":"Bytes"},"sql":{"type":"MEDIUMBLOB","length":0,"default":""}}
}

def getOutputDir(lib, template):
    ouputdir = os.path.join(os.getcwd(), "output", lib, "db/entity", template)
    if not os.path.exists(ouputdir):
        os.makedirs(ouputdir)
    return ouputdir

def capitalize(srcstr):
    srcstr = srcstr.strip('_')
    srcIdx = 0
    srcLength = len(srcstr)
    desstr = ''
    while srcIdx < srcLength:
        if srcstr[srcIdx] == '_' and  srcstr[srcIdx + 1] == '_':
            srcIdx+= 1
            continue
        
        if srcIdx == 0:
            desstr += srcstr[srcIdx].upper()
        elif srcstr[srcIdx] == '_' and  srcstr[srcIdx + 1] != '_':
            srcIdx += 1
            desstr += srcstr[srcIdx].upper()
        else:
            desstr += srcstr[srcIdx]
        srcIdx+= 1
    return desstr

def genAll(lib, coderoot,package,boTemplate):
    print('生成%s数据库信息到%s'%(lib, coderoot))

    # 清理之前的生成
    shutil.rmtree(getOutputDir(lib,"BO"), True)
    #shutil.rmtree(getOutputDir(lib,"BM"), True)

    # 解析每个生成源
    srcPath = os.path.join(os.getcwd(), "source_db", lib)

    tables = []
    sys.path.append(srcPath)
    for root, dirs, files in os.walk(srcPath):
        for name in files:
            if not str(name).endswith(".py"):
                continue
            name = name.split(".")[0]
            tbl = __import__(name)
            tbl = reload(tbl)
            tables.append(tbl)
    sys.path.remove(srcPath)

    # 初始化数据库表信息，方便后面使用
    typeparttern = re.compile(r'(?P<name>\w+)(?:\((?P<dblength>[\d,]+)\))?(?:\[(?P<size>\d+)\])?')
    for tbl in tables:
        newField = []
        for dbtype, name, comment in tbl.field:
            match = re.match(typeparttern, dbtype)
            if match is None:
                print('db define invalid in table:[%s] filed:[%s]'%(tbl, dbtype))
                exit()
            typename = match.groupdict().get("name")
            typeinfo = typedef.get(typename)
            if typeinfo is None:
                print('db type:[%s] is invalid. table:[%s] filed:[%s]'%(typename, tbl, dbtype))
                exit()
            size = match.groupdict().get("size")
            size = 1 if size == None else int(size)
            length = match.groupdict().get("dblength")

            sql = copy.copy(typeinfo.get("sql"))
            sql["length"] = sql["length"] if length == None else length
            sql["sql"] = getSql(sql["type"], sql["length"], sql["default"], comment)
            field = (name.strip(), size, comment, typeinfo["java"], sql)

            newField.append(field)
        tbl.field = newField

    if sys.version>'3':
       boTemp = open(os.path.join(os.getcwd(), "template", boTemplate), "r",encoding ="utf-8").read()
       sqlTemp = open(os.path.join(os.getcwd(), "template", "SQL.txt"), "r" ,encoding ="utf-8").read()
    else:
       boTemp = open(os.path.join(os.getcwd(), "template", boTemplate), "r").read()
       sqlTemp = open(os.path.join(os.getcwd(), "template", "SQL.txt"), "r").read()
    # 遍历列表生成SQL,BO,BM
    cnt = 0
    for tbl in tables:
        print("deal %s: %s"%(lib, tbl.__name__))
        sql = genSql(lib, tbl, sqlTemp)
        genBO(lib, tbl, boTemp, sql,package)
        cnt += 1

    # 拷贝
    lib_codeRoot_bo = coderoot
    if os.path.exists(lib_codeRoot_bo):
        shutil.rmtree(lib_codeRoot_bo, True)

    srcCodePath = getOutputDir(lib,"BO")
    shutil.copytree(srcCodePath, lib_codeRoot_bo) #拷贝目录，拷贝之前dst必须不存在
    
    #lib_codeRoot_bm = os.path.join(coderoot, lib, "bm")
    #if os.path.exists(lib_codeRoot_bm):
     #   shutil.rmtree(lib_codeRoot_bm, True)

    #srcCodePath = getOutputDir(lib,"BM")
    #shutil.copytree(srcCodePath, lib_codeRoot_bm) #拷贝目录，拷贝之前dst必须不存在
    
    print("total gen %s cnt: %s\n\n"%(lib, cnt))

def genBO(lib, src, temp, sql, package):
    output = temp
    output = output.replace("<%CLASS_NAME%>", "%sBO"%capitalize(src.__name__))
    output = output.replace("<%TABLE_NAME%>", src.__name__)
    #output = output.replace("<%BM_NAME%>", "%sBM"%capitalize(src.__name__))
    output = output.replace("<%PACKAGE%>", package)
    output = output.replace("<%CREATE_SQL%>", sql)

	#数据库标签
    dbTag = "main"
    if hasattr(src,"dbTag") and src.dbTag!="" :
        dbTag=src.dbTag
    output = output.replace("<%DB_TAG%>", dbTag)

	#类注解
    if hasattr(src,"classAnnotation"):
        output = output.replace("<%CLASS_ANNOTATION%>", "%s"%src.classAnnotation)
    else:
        output = output.replace("<%CLASS_ANNOTATION%>", "isIdAuto= true")

    #添加记录有效期，用于定时删除
    recordExpiredSec = 0
    if hasattr(src,"recordExpiredSec"):
        recordExpiredSec = src.recordExpiredSec
    output = output.replace("<%EXPIRED_SECOND%>", "%s"%recordExpiredSec)
    

    field_define = "" # private String user = ""  // 用户名
    field_restore = "" # bo.m_id = rs.getInt(1);
    field_list = ""
    field_store = "" # strBuf.append("'" + m_id + "',");
    field_store_bytes = "" # ret.add(xxx);
    field_store_marked_bytes=""# if(IsFiledmarked())   ret.add(xxx);
    field_update = "" # sBuilder.append("  `user` = ").append(m_user);
    field_marked_update = "" # if(IsFiledmarked())   sBuilder.append("  `user` = ").append(m_user);field_update = "" # sBuilder.append("  `user` = ").append(m_user);
    field_interface = ""
    field_default = ""
    field_getByteSize ="" #写入二进制流需要的长度
    field_writeByteBuffer =""  #写入二进制流
    field_readByteBuffer =""  #读取二进制流
    index = 2 # 1 是id
    
    fieldIndex=0; #字段索引
    for name, size, comment, java, sql in src.field:
        field_define+="\n    public static final int FIELD_%s =%d;\n"%(name,fieldIndex)
        length_str = str(sql["length"])
        if length_str != "0" and length_str:
            field_define += "    @DataBaseField(type = \"%s(%s)\",<size> fieldname = \"%s\", comment = \"%s\")\n"%(sql["type"], sql["length"], name, comment)
        else:
            field_define += "    @DataBaseField(type = \"%s\",<size> fieldname = \"%s\", comment = \"%s\")\n"%(sql["type"], name, comment)

        if size == 1:
            field_define = field_define.replace("<size>", '')
            field_default += "        %s = %s;\n"%(name, java["default"])
            if java["type"] == "byte[]":
                field_store_bytes += "        ret.add(%s); \n"%(name)
                field_store_marked_bytes += "        if(isFieldMarked(FIELD_%s)) ret.add(%s); \n"%(name,name)
                field_store += "        strBuf.append(\"?, \");\n"
                field_update += "        sBuilder.append(\" `%s` = ?,\");\n"%(name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s)) sBuilder.append(\" `%s` = ?,\");\n"%(name,name)
                field_getByteSize+="        _size+=2;_size+=%s.length;//%s\n"%(name,name)
                field_writeByteBuffer+="        buff.putShort((short)(%s == null ? 0 : %s.length));if(null != %s){buff.put(%s);}\n"%(name,name,name,name)
                field_readByteBuffer+="        int %s_count = buff.getShort();if(%s_count>0){%s = new byte[%s_count];buff.get(%s);}\n"%(name,name,name,name,name)
                
            elif java["type"] == "boolean":
                field_store += "        strBuf.append(\"'\").append(%s ? 1 : 0).append(\"', \");\n"%(name)
                field_update += "        sBuilder.append(\" `%s` = '\").append(%s ? 1 : 0).append(\"',\");\n"%(name, name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s)) sBuilder.append(\" `%s` = '\").append(%s ? 1 : 0).append(\"',\");\n"%(name,name, name)
                field_getByteSize+="        _size+=1;//%s\n"%(name)
                field_writeByteBuffer+="        buff.put((byte)(%s?1:0));\n"%(name)
                field_readByteBuffer+="        %s=(buff.get()==1);\n"%(name)
            elif java["type"] == "String":
                field_store += "        strBuf.append(\"'\").append(%s == null ? null : %s.replace(\"'\",\"''\").replace(\"\\\\\",\"\\\\\\\\\")).append(\"', \");\n"%(name, name)
                field_update += "        sBuilder.append(\" `%s` = '\").append(%s == null ? null : %s.replace(\"'\",\"''\").replace(\"\\\\\",\"\\\\\\\\\")).append(\"',\");\n"%(name, name, name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s)) sBuilder.append(\" `%s` = '\").append(%s == null ? null : %s.replace(\"'\",\"''\").replace(\"\\\\\",\"\\\\\\\\\")).append(\"',\");\n"%(name,name, name, name)
                field_getByteSize+="        _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(%s);//%s\n"%(name,name)
                field_writeByteBuffer+="        ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, %s);\n"%(name)
                field_readByteBuffer+="        %s=ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff);\n"%(name)
            elif java["type"] == "long":
                field_store += "        strBuf.append(\"'\").append(%s).append(\"', \");\n"%(name)
                field_update += "        sBuilder.append(\" `%s` = '\").append(%s).append(\"',\");\n"%(name, name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s)) sBuilder.append(\" `%s` = '\").append(%s).append(\"',\");\n"%(name,name, name)
                field_getByteSize+="        _size+=8;//%s\n"%(name)
                field_writeByteBuffer+="        buff.putLong(%s);\n"%(name)
                field_readByteBuffer+="        %s=buff.getLong();\n"%(name)
            elif java["type"] == "double":
                field_store += "        strBuf.append(\"'\").append(%s).append(\"', \");\n"%(name)
                field_update += "        sBuilder.append(\" `%s` = '\").append(%s).append(\"',\");\n"%(name, name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s)) sBuilder.append(\" `%s` = '\").append(%s).append(\"',\");\n"%(name,name, name)
                field_getByteSize+="        _size+=8;//%s\n"%(name)
                field_writeByteBuffer+="        buff.putDouble(%s);\n"%(name)
                field_readByteBuffer+="        %s=buff.getDouble();\n"%(name)
            elif java["type"] == "int" :
                field_store += "        strBuf.append(\"'\").append(%s).append(\"', \");\n"%(name)
                field_update += "        sBuilder.append(\" `%s` = '\").append(%s).append(\"',\");\n"%(name, name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s)) sBuilder.append(\" `%s` = '\").append(%s).append(\"',\");\n"%(name,name, name)
                field_getByteSize+="        _size+=4;//%s\n"%(name)
                field_writeByteBuffer+="        buff.putInt(%s);\n"%(name)
                field_readByteBuffer+="        %s=buff.getInt();\n"%(name)
            elif java["type"] == "short" :
                field_store += "        strBuf.append(\"'\").append(%s).append(\"', \");\n"%(name)
                field_update += "        sBuilder.append(\" `%s` = '\").append(%s).append(\"',\");\n"%(name, name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s)) sBuilder.append(\" `%s` = '\").append(%s).append(\"',\");\n"%(name,name, name)
                field_getByteSize+="        _size+=4;//%s\n"%(name)
                field_writeByteBuffer+="        buff.putShort(%s);\n"%(name)
                field_readByteBuffer+="        %s=buff.getShort();\n"%(name)
            elif java["type"] == "float":
                field_store += "        strBuf.append(\"'\").append(%s).append(\"', \");\n"%(name)
                field_update += "        sBuilder.append(\" `%s` = '\").append(%s).append(\"',\");\n"%(name, name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s)) sBuilder.append(\" `%s` = '\").append(%s).append(\"',\");\n"%(name,name, name)
                field_getByteSize+="        _size+=4;//%s\n"%(name)
                field_writeByteBuffer+="        buff.putFloat(%s);\n"%(name)
                field_readByteBuffer+="        %s=buff.getFloat();\n"%(name)

            field_define += "    private %s %s;\n"%(java["type"], name)
            field_list += ", `%s`"%(name)
            field_restore += "        %s = rs.get%s(%s);\n"%(name, java["restore"], index)

            template  = "    // %s\n"%comment
            template += "    public <%type%> get<%capitalize%>() { return this.<%name%>; }\n"
            template += "    public void set<%capitalize%>(BM _bm, <%type%> <%name%>) {\n"
            template += "        if(<%%name%%>%s) \n"%(".equals(this.<%name%>)" if java["type"] == "String" else "==this.<%name%>")
            template += "            return;\n"
            template += "        this.<%name%> = <%name%>; \n"
            template += "        markField(_bm, FIELD_%s); \n"%(name)
            template += "    }\n"
            template += "    public void save<%capitalize%>(BM _bm, <%type%> <%name%>) {\n"
            template += "        if(<%%name%%>%s) \n"%(".equals(this.<%name%>)" if java["type"] == "String" else "==this.<%name%>")
            template += "            return;\n"
            template += "        this.<%name%> = <%name%>;\n"
            template += "        <%func%>(_bm, \"<%name%>\", <%name%><%valuepatch%>);\n"
            template += "    }\n"
            template += "\n"
        else:
            field_define = field_define.replace("<size>", ' size = %s,'%size)
            field_default += "        %s = new ArrayList<>(%s);\n"%(name,size)
            field_default += "        for (int i = 0; i < %s; ++i) {\n"%(size)
            field_default += "            %s.add(%s);\n"%(name, java["default"])
            field_default += "        }\n"
            field_store += "        for (int i = 0; i < %s.size(); ++i) {\n"%(name)
            field_update += "        for (int i = 0; i < %s.size(); ++i) {\n"%(name)
            field_getByteSize+="        _size+=2;//%s\n"%(name)
            field_getByteSize+="        for (int i = 0; i < %s.size(); ++i){\n"%(name)
            field_writeByteBuffer+="        buff.putShort((short)(%s.size()));\n"%(name)
            field_writeByteBuffer+="        for (int i = 0; i < %s.size(); ++i){\n"%(name)
            field_readByteBuffer+="        %s.clear();\n"%(name)
            field_readByteBuffer+="        int %s_count=buff.getShort();\n"%(name)
            field_readByteBuffer+="        for(int i=0;i<%s_count;i++){\n"%(name)
            if java["type"] == "byte[]":
                field_store_bytes += "             ret.add(%s); \n"%(name)
                field_store_marked_bytes+= "             if(isFieldMarked(FIELD_%s)) ret.add(%s); \n"%(name,name)
                field_store += "             strBuf.append(\"?, \");\n"
                field_update += "             sBuilder.append(\" `%s_\").append(i).append(\"` = ?,\");\n"%(name)
                field_getByteSize+="             _size+=2;_size+=%s.get(i).length;\n"%(name)
                field_writeByteBuffer+="             buff.putShort((short)(%s.get(i) == null ? 0 : %s.get(i).length));if(null != %s.get(i)){buff.put(%s.get(i));}\n"%(name,name,name,name)
                field_readByteBuffer+="             int count = buff.getShort();if(count>0){byte[] tmp = new byte[count];buff.get(tmp);%s.add(tmp);}\n"%(name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s))  for (int i = 0; i < %s.size(); ++i) {sBuilder.append(\" `%s_\").append(i).append(\"` = ?,\");}\n"%(name,name,name)
            elif java["type"] == "boolean":
                field_store += "             strBuf.append(\"'\").append(%s.get(i) ? 1 : 0).append(\"', \");\n"%(name)
                field_update += "             sBuilder.append(\" `%s_\").append(i).append(\"` = '\").append(%s.get(i) ? 1 : 0).append(\"',\");\n"%(name, name)
                field_getByteSize+="             _size+=1;\n"
                field_writeByteBuffer+="            buff.put((byte)(%s.get(i)?1:0));\n"%(name)
                field_readByteBuffer+="             %s.add((buff.get()==1));\n"%(name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s))   for (int i = 0; i < %s.size(); ++i) {sBuilder.append(\" `%s_\").append(i).append(\"` = '\").append(%s.get(i) ? 1 : 0).append(\"',\");}\n"%(name,name,name, name)
            elif java["type"] == "String":
                field_store += "             strBuf.append(\"'\").append(%s.get(i) == null ? null : %s.get(i).replace(\"'\",\"''\")).append(\"', \");\n"%(name, name)
                field_update += "             sBuilder.append(\" `%s_\").append(i).append(\"` = '\").append(%s == null ? null : %s.get(i).replace(\"'\",\"''\")).append(\"',\");\n"%(name, name, name)
                field_getByteSize+="             _size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(%s.get(i));\n"%(name)
                field_writeByteBuffer+="            ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(buff, %s.get(i));\n"%(name)
                field_readByteBuffer+="             %s.add(ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(buff));\n"%(name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s))  for (int i = 0; i < %s.size(); ++i) { sBuilder.append(\" `%s_\").append(i).append(\"` = '\").append(%s.get(i) == null ? null : %s.get(i).replace(\"'\",\"''\").replace(\"\\\\\",\"\\\\\\\\\")).append(\"',\");}\n"%(name,name,name, name, name)
            elif java["type"] == "long" :
                field_store += "            strBuf.append(\"'\").append(%s.get(i)).append(\"', \");\n"%(name)
                field_update += "            sBuilder.append(\" `%s_\").append(i).append(\"` = '\").append(%s.get(i)).append(\"',\");\n"%(name, name)
                field_getByteSize+="            _size+=8;\n"
                field_writeByteBuffer+="             buff.putLong(%s.get(i));\n"%(name)
                field_readByteBuffer+="             %s.add(buff.getLong());\n"%(name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s))  for (int i = 0; i < %s.size(); ++i) { sBuilder.append(\" `%s_\").append(i).append(\"` = '\").append(%s.get(i)).append(\"',\");}\n"%(name,name,name, name)
            elif java["type"] == "double":
                field_store += "            strBuf.append(\"'\").append(%s.get(i)).append(\"', \");\n"%(name)
                field_update += "            sBuilder.append(\" `%s_\").append(i).append(\"` = '\").append(%s.get(i)).append(\"',\");\n"%(name, name)
                field_getByteSize+="            _size+=8;\n"
                field_writeByteBuffer+="             buff.putDouble(%s.get(i));\n"%(name)
                field_readByteBuffer+="             %s.add(buff.getDouble());\n"%(name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s)) for (int i = 0; i < %s.size(); ++i) {sBuilder.append(\" `%s_\").append(i).append(\"` = '\").append(%s.get(i)).append(\"',\");}\n"%(name,name,name, name)
            elif java["type"] == "int" :
                field_store += "             strBuf.append(\"'\").append(%s.get(i)).append(\"', \");\n"%(name)
                field_update += "             sBuilder.append(\" `%s_\").append(i).append(\"` = '\").append(%s.get(i)).append(\"',\");\n"%(name, name)
                field_getByteSize+="             _size+=4;\n"
                field_writeByteBuffer+="             buff.putInt(%s.get(i));\n"%(name)
                field_readByteBuffer+="             %s.add(buff.getInt());\n"%(name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s)) for (int i = 0; i < %s.size(); ++i) {sBuilder.append(\" `%s_\").append(i).append(\"` = '\").append(%s.get(i)).append(\"',\");}\n"%(name,name,name, name)
            elif java["type"] == "float":
                field_store += "             strBuf.append(\"'\").append(%s.get(i)).append(\"', \");\n"%(name)
                field_update += "             sBuilder.append(\" `%s_\").append(i).append(\"` = '\").append(%s.get(i)).append(\"',\");\n"%(name, name)
                field_getByteSize+="             _size+=4;\n"
                field_writeByteBuffer+="             buff.putFloat(%s.get(i));\n"%(name)
                field_readByteBuffer+="             %s.add(buff.getFloat());\n"%(name)
                field_marked_update += "        if(isFieldMarked(FIELD_%s)) for (int i = 0; i < %s.size(); ++i) {sBuilder.append(\" `%s_\").append(i).append(\"` = '\").append(%s.get(i)).append(\"',\");}\n"%(name,name,name, name)
            field_store += "        }\n"
            field_update += "        }\n"
            field_getByteSize+="        }\n"
            field_writeByteBuffer+="        }\n"
            field_readByteBuffer+="        }\n"

            field_define += "    private List<%s> %s;\n"%(java["list"], name)
            for i in range(size):
                field_list += ", `%s_%s`"%(name, i)
            field_restore += "        %s = new ArrayList<>(%s);\n"%(name, size)
            field_restore += "        for (int i = 0; i < %s; ++i) {\n"%(size)
            field_restore += "            %s.add(rs.get%s(i + %s));\n"%(name, java["restore"], index)
            field_restore += "        }\n"

            template  = "    // %s\n"%comment
            template += "    public int get<%capitalize%>Size() { return this.<%name%>.size(); }\n"
            template += "    public List<<%list>> get<%capitalize%>All() { return new ArrayList<>(<%name%>); }\n"
            template += "    public void set<%capitalize%>All(BM _bm, <%type%> value) { for (int i = 0; i < this.<%name%>.size(); ++i) this.<%name%>.set(i, value);  markField(_bm, FIELD_<%name%>); }\n"
            template += "    public void save<%capitalize%>All(BM _bm, <%type%> value) { set<%capitalize%>All(_bm, value); saveAll(_bm); }\n"
            template += "    public <%type%> get<%capitalize%>(int index) { return this.<%name%>.get(index); }\n"
            template += "    public void set<%capitalize%>(BM _bm, int index, <%type%> value) {\n"
            template += "        if(value%s)\n"%(".equals(this.<%name%>.get(index))" if java["type"] == "String" else "==this.<%name%>.get(index)")
            template += "            return;\n"
            template += "        this.<%name%>.set(index, value);\n"
            template += "        markField(_bm, FIELD_%s); \n"%(name)
            template += "    }\n"
            template += "    public void save<%capitalize%>(BM _bm, int index, <%type%> value) {\n"
            template += "        if(value%s)\n"%(".equals(this.<%name%>.get(index))" if java["type"] == "String" else "==this.<%name%>.get(index)")
            template += "            return;\n"
            template += "        this.<%name%>.set(index, value);\n"
            template += "        <%func%>(_bm, \"<%name%>_\" + index, this.<%name%>.get(index));\n"
            template += "    }\n"
            template += "\n"
        template = template.replace("<%type%>", java["type"]).replace("<%capitalize%>", capitalize(name)).replace("<%name%>", name).replace("<%list>", java["list"])
        template = template.replace("<%valuepatch%>", " ? 1 : 0" if java["type"] == "boolean" else "")
        template = template.replace("<%func%>", "saveFieldBytes" if java["type"] == "byte[]" else "saveField")
        field_interface += template
        index += size
        fieldIndex+=1; 

    output = output.replace("<%FIELD_DEFAULT%>", field_default[:-1])
    output = output.replace("<%FIELD_DEFINE%>", field_define[:-1])
    output = output.replace("<%FIELD_RESTORE%>", field_restore[:-1])
    output = output.replace("<%FIELD_LIST%>", field_list)
    output = output.replace("<%FIELD_STORE%>", field_store[:-1])
    output = output.replace("<%FIELD_STORE_BYTES%>", field_store_bytes[:-1])
    output = output.replace("<%FIELD_STORE_MARKED_BYTES%>", field_store_marked_bytes[:-1])
    output = output.replace("<%FIELD_UPDATE%>", field_update[:-1])
    output = output.replace("<%FIELD_MARK_UPDATE%>", field_marked_update[:-1])
    output = output.replace("<%FIELD_INTERFACE%>", field_interface[:-1])
    output = output.replace("<%FIELD_BUFFERSIZE%>", field_getByteSize[:-1])
    output = output.replace("<%FIELD_WRITEBUFFER%>", field_writeByteBuffer[:-1])
    output = output.replace("<%FIELD_READBUFFER%>", field_readByteBuffer[:-1])
    
    outFile =open(os.path.join(getOutputDir(lib, "BO"), "%sBO.java"%capitalize(src.__name__)),"wb")
    if sys.version >'3':
        outFile.write(output.encode())
    else:
        outFile.write(output)

def getSql(sqlType, sqlLength, defValue, comment):
    if sqlType == "blob":
        return " %s NULL COMMENT '%s',\n"%(sqlType, comment)
    elif sqlType == "MEDIUMBLOB":
        return " %s NULL COMMENT '%s',\n"%(sqlType, comment)
    elif sqlType == "timestamp":
        return " %s NULL DEFAULT NULL COMMENT '%s',\n"%(sqlType, comment)
    elif sqlType == "text":
        return " %s NULL COMMENT '%s',\n"%(sqlType, comment)
    elif sqlType == "mediumtext":
        return " %s NULL COMMENT '%s',\n"%(sqlType, comment)
    elif sqlType == "bool":
        return " %s NOT NULL DEFAULT '%s' COMMENT '%s',\n"%(sqlType, defValue, comment)
    elif sqlType == "float":
        return " %s NOT NULL DEFAULT '%s' COMMENT '%s',\n"%(sqlType, defValue, comment)
    elif sqlType == "double":
        return " %s NOT NULL DEFAULT '%s' COMMENT '%s',\n"%(sqlType, defValue, comment)
    elif sqlType == "decimal":
        return " %s(%s) NOT NULL DEFAULT '%s' COMMENT '%s',\n"%(sqlType, sqlLength, defValue, comment)
    else:
        return " %s(%s) NOT NULL DEFAULT '%s' COMMENT '%s',\n"%(sqlType, sqlLength, defValue, comment)

def genSql(lib, src, temp):
    output = temp
    output = output.replace("<%TABLE_NAME%>", "%s"%src.__name__)
    output = output.replace("<%COMMENT%>", "%s"%src.tableComment)

    # <%FIELD_INFO%>
    field_info = ""  # int(11); varchar(30)
    for name, size, comment, java, sql in src.field:
        if size == 1:
            field_info += '`%s`%s'%(name, sql['sql'])
        else:
            for index in range(size):
                field_info += '`%s_%s`%s'%(name, index, sql['sql']) 
    output = output.replace("<%FIELD_INFO%>", field_info)

    # <%KEY%> # KEY `user` (`user`),
    key_info = ""
    if len(src.key) != 0:
         for key in src.key:
             key_info += "  KEY `%s` (`%s`),\n"%(key, key)
    output = output.replace("<%KEY%>", key_info)

    # <%UNIQUE_KEY%> # UNIQUE INDEX `psw` (`psw`),
    key_info = ""
    if len(src.ukey) != 0:
         for key in src.ukey:
             key_info += "  UNIQUE INDEX `%s` (`%s`),\n"%(key, key)
    output = output.replace("<%UNIQUE_KEY%>", key_info)

    if len(set(src.key) & (set(src.ukey))):
        print('duplicate declared key or unique_key in table [%s]: %s'%(src.__name__,set(src.key) & (set(src.ukey))))
        exit()

    # 返回分行的建表sql
    ret = ""
    lines = output.split("\n")
    for line in lines:
        line = str(line).strip()
        if "" == line:
            continue
        ret += '                + "%s"\n'%line
    return ret [18:-1]+";"

