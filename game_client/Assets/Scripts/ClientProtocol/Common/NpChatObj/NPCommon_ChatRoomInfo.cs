using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpChatObj
{

/// <summary>
/// NP聊天房间信息
/// </summary>
public class NPCommon_ChatRoomInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 聊天服务器房间的唯一id
/// </summary>
private long chatSdkRoomId;
/// <summary>
/// 房间类型,ENPChatRoomType
/// </summary>
private int chatRoomType;
/// <summary>
/// 在指定类型下的房间唯一id
/// </summary>
private long chatRoomSubTypeId;


public NPCommon_ChatRoomInfo() {
	chatSdkRoomId = (long)0;
	chatRoomType = 0;
	chatRoomSubTypeId = (long)0;
}

public NPCommon_ChatRoomInfo(
	long _chatSdkRoomId
	, int _chatRoomType
	, long _chatRoomSubTypeId
) {	chatSdkRoomId = _chatSdkRoomId;
	chatRoomType = _chatRoomType;
	chatRoomSubTypeId = _chatRoomSubTypeId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 聊天服务器房间的唯一id
/// </summary>
public long getChatSdkRoomId() { return chatSdkRoomId; }
/// <summary>
/// 聊天服务器房间的唯一id
/// </summary>
public void setChatSdkRoomId(long _chatSdkRoomId) { chatSdkRoomId = _chatSdkRoomId; }
/// <summary>
/// 房间类型,ENPChatRoomType
/// </summary>
public int getChatRoomType() { return chatRoomType; }
/// <summary>
/// 房间类型,ENPChatRoomType
/// </summary>
public void setChatRoomType(int _chatRoomType) { chatRoomType = _chatRoomType; }
/// <summary>
/// 在指定类型下的房间唯一id
/// </summary>
public long getChatRoomSubTypeId() { return chatRoomSubTypeId; }
/// <summary>
/// 在指定类型下的房间唯一id
/// </summary>
public void setChatRoomSubTypeId(long _chatRoomSubTypeId) { chatRoomSubTypeId = _chatRoomSubTypeId; }


public int GetBufSize() {
	int _size = 20;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chatSdkRoomId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chatRoomType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chatRoomSubTypeId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(chatSdkRoomId);
	_buf.putInt(chatRoomType);
	_buf.putLong(chatRoomSubTypeId);
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
	builder.Append("chatSdkRoomId").Append(":").Append(chatSdkRoomId.ToString()).Append(", ");
	builder.Append("chatRoomType").Append(":").Append(chatRoomType.ToString()).Append(", ");
	builder.Append("chatRoomSubTypeId").Append(":").Append(chatRoomSubTypeId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

