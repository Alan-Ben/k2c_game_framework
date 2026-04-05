package Common;

import java.nio.ByteBuffer;
public class WCGCS2US_PersonalMail implements ALBasicProtocolPack._IALProtocolStructure {
private String senderName;
private String title;
private String content;
private String reward;
private int createTime;
private int existTime;


public WCGCS2US_PersonalMail() {
	senderName = "";
	title = "";
	content = "";
	reward = "";
	createTime = 0;
	existTime = 0;
}

public WCGCS2US_PersonalMail(
	 String _senderName
	, String _title
	, String _content
	, String _reward
	, int _createTime
	, int _existTime
) {	senderName = _senderName;
	title = _title;
	content = _content;
	reward = _reward;
	createTime = _createTime;
	existTime = _existTime;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public String getSenderName() { return senderName; }
public void setSenderName(String _senderName) { senderName = _senderName; }
public String getTitle() { return title; }
public void setTitle(String _title) { title = _title; }
public String getContent() { return content; }
public void setContent(String _content) { content = _content; }
public String getReward() { return reward; }
public void setReward(String _reward) { reward = _reward; }
public int getCreateTime() { return createTime; }
public void setCreateTime(int _createTime) { createTime = _createTime; }
public int getExistTime() { return existTime; }
public void setExistTime(int _existTime) { existTime = _existTime; }


public final int GetBufSize() {
	int _size = 8;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(reward);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(content);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(reward);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) senderName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) title = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) content = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) reward = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createTime = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) existTime = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, senderName);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, title);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, content);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, reward);
	_buf.putInt(createTime);
	_buf.putInt(existTime);
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

