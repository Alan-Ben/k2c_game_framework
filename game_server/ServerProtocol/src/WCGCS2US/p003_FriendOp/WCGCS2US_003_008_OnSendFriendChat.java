package WCGCS2US.p003_FriendOp;

import java.nio.ByteBuffer;
public class WCGCS2US_003_008_OnSendFriendChat implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private long fuid;
private String content;
private int chatTime;
private int msgType;
private String voiceUrl;


public WCGCS2US_003_008_OnSendFriendChat() {
	uid = (long)0;
	fuid = (long)0;
	content = "";
	chatTime = 0;
	msgType = 0;
	voiceUrl = "";
}

public WCGCS2US_003_008_OnSendFriendChat(
	 long _uid
	, long _fuid
	, String _content
	, int _chatTime
	, int _msgType
	, String _voiceUrl
) {	uid = _uid;
	fuid = _fuid;
	content = _content;
	chatTime = _chatTime;
	msgType = _msgType;
	voiceUrl = _voiceUrl;
}

public final byte getMainOrder() { return (byte)3; }

public final byte getSubOrder() { return (byte)8; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public long getFuid() { return fuid; }
public void setFuid(long _fuid) { fuid = _fuid; }
public String getContent() { return content; }
public void setContent(String _content) { content = _content; }
public int getChatTime() { return chatTime; }
public void setChatTime(int _chatTime) { chatTime = _chatTime; }
public int getMsgType() { return msgType; }
public void setMsgType(int _msgType) { msgType = _msgType; }
public String getVoiceUrl() { return voiceUrl; }
public void setVoiceUrl(String _voiceUrl) { voiceUrl = _voiceUrl; }


public final int GetBufSize() {
	int _size = 24;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(voiceUrl);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(voiceUrl);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) fuid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) chatTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) msgType = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) voiceUrl = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putLong(fuid);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
	_buf.putInt(chatTime);
	_buf.putInt(msgType);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, voiceUrl);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)3);
	_buf.put((byte)8);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)3);
	_recBuf.put((byte)8);
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

