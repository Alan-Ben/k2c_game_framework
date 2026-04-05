package Common;

import java.nio.ByteBuffer;
public class Common_FriendChatInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long fuid;
private int type;
private String content;
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
	, String _content
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

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getFuid() { return fuid; }
public void setFuid(long _fuid) { fuid = _fuid; }
public int getType() { return type; }
public void setType(int _type) { type = _type; }
public String getContent() { return content; }
public void setContent(String _content) { content = _content; }
public int getChatTime() { return chatTime; }
public void setChatTime(int _chatTime) { chatTime = _chatTime; }
public int getMsgType() { return msgType; }
public void setMsgType(int _msgType) { msgType = _msgType; }
public long getId() { return id; }
public void setId(long _id) { id = _id; }


public final int GetBufSize() {
	int _size = 28;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) fuid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) type = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chatTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) msgType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(fuid);
	_buf.putInt(type);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
	_buf.putInt(chatTime);
	_buf.putInt(msgType);
	_buf.putLong(id);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

