using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common
{

public class Common_FriendChatInfo : ALBasicProtocolPack._IALProtocolStructure {
private long fuid;
private int type;
private string content;
private int chatTime;
private int msgType;
private long id;


public Common_FriendChatInfo() {
	fuid = (long)0;
	type = 0;
	content = "";
	chatTime = 0;
	msgType = 0;
	id = (long)0;
}

public Common_FriendChatInfo(
	long _fuid
	, int _type
	, string _content
	, int _chatTime
	, int _msgType
	, long _id
) {	fuid = _fuid;
	type = _type;
	content = _content;
	chatTime = _chatTime;
	msgType = _msgType;
	id = _id;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getFuid() { return fuid; }
public void setFuid(long _fuid) { fuid = _fuid; }
public int getType() { return type; }
public void setType(int _type) { type = _type; }
public string getContent() { return content; }
public void setContent(string _content) { content = _content; }
public int getChatTime() { return chatTime; }
public void setChatTime(int _chatTime) { chatTime = _chatTime; }
public int getMsgType() { return msgType; }
public void setMsgType(int _msgType) { msgType = _msgType; }
public long getId() { return id; }
public void setId(long _id) { id = _id; }


public int GetBufSize() {
	int _size = 28;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	fuid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	content = _buf.getString();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	chatTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	msgType = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(fuid);
	_buf.putInt(type);
	_buf.putString(content);
	_buf.putInt(chatTime);
	_buf.putInt(msgType);
	_buf.putLong(id);
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
	builder.Append("fuid").Append(":").Append(fuid.ToString()).Append(", ");
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("content").Append(":").Append(content.ToString()).Append(", ");
	builder.Append("chatTime").Append(":").Append(chatTime.ToString()).Append(", ");
	builder.Append("msgType").Append(":").Append(msgType.ToString()).Append(", ");
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

