using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

/// <summary>
/// AI聊天消息
/// </summary>
public class Common_AiChatMessage : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 角色类型
/// </summary>
private CommonEnum.EAiChatRoleType roleType;
/// <summary>
/// 聊天内容
/// </summary>
private string msg;


public Common_AiChatMessage() {
	roleType = 0;
	msg = "";
}

public Common_AiChatMessage(
	CommonEnum.EAiChatRoleType _roleType
	, string _msg
) {	roleType = _roleType;
	msg = _msg;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 角色类型
/// </summary>
public CommonEnum.EAiChatRoleType getRoleType() { return roleType; }
/// <summary>
/// 角色类型
/// </summary>
public void setRoleType(CommonEnum.EAiChatRoleType _roleType) { roleType = _roleType; }
/// <summary>
/// 聊天内容
/// </summary>
public string getMsg() { return msg; }
/// <summary>
/// 聊天内容
/// </summary>
public void setMsg(string _msg) { msg = _msg; }


public int GetBufSize() {
	int _size = 4;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(msg);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(msg);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	roleType = (CommonEnum.EAiChatRoleType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	msg = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)roleType);

	_buf.putString(msg);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("roleType").Append(":").Append(roleType.ToString()).Append(", ");
	builder.Append("msg").Append(":").Append(msg.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

