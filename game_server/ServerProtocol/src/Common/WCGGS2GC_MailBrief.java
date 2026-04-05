package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_MailBrief implements ALBasicProtocolPack._IALProtocolStructure {
private long sId;
private String title;
private long createTime;
private long readTime;
private long pickUpTime;
private String senderName;
private boolean isReward;
private long expireTime;


public WCGGS2GC_MailBrief() {
	sId = (long)0;
	title = "";
	createTime = (long)0;
	readTime = (long)0;
	pickUpTime = (long)0;
	senderName = "";
	isReward = false;
	expireTime = (long)0;
}

public WCGGS2GC_MailBrief(
	 long _sId
	, String _title
	, long _createTime
	, long _readTime
	, long _pickUpTime
	, String _senderName
	, boolean _isReward
	, long _expireTime
) {	sId = _sId;
	title = _title;
	createTime = _createTime;
	readTime = _readTime;
	pickUpTime = _pickUpTime;
	senderName = _senderName;
	isReward = _isReward;
	expireTime = _expireTime;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getSId() { return sId; }
public void setSId(long _sId) { sId = _sId; }
public String getTitle() { return title; }
public void setTitle(String _title) { title = _title; }
public long getCreateTime() { return createTime; }
public void setCreateTime(long _createTime) { createTime = _createTime; }
public long getReadTime() { return readTime; }
public void setReadTime(long _readTime) { readTime = _readTime; }
public long getPickUpTime() { return pickUpTime; }
public void setPickUpTime(long _pickUpTime) { pickUpTime = _pickUpTime; }
public String getSenderName() { return senderName; }
public void setSenderName(String _senderName) { senderName = _senderName; }
public boolean getIsReward() { return isReward; }
public void setIsReward(boolean _isReward) { isReward = _isReward; }
public long getExpireTime() { return expireTime; }
public void setExpireTime(long _expireTime) { expireTime = _expireTime; }


public final int GetBufSize() {
	int _size = 41;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 43;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(senderName);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) sId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) title = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createTime = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) readTime = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pickUpTime = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) senderName = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) expireTime = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(sId);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, title);
	_buf.putLong(createTime);
	_buf.putLong(readTime);
	_buf.putLong(pickUpTime);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, senderName);
	_buf.put(isReward?(byte)1:(byte)0);
	_buf.putLong(expireTime);
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

