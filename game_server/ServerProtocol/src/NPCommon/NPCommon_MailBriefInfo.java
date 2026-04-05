package NPCommon;

import java.nio.ByteBuffer;
public class NPCommon_MailBriefInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long id;
private String title;
private long createTimeMs;
private long readTimeMs;
private long gainTimeMs;


public NPCommon_MailBriefInfo() {
	id = (long)0;
	title = "";
	createTimeMs = (long)0;
	readTimeMs = (long)0;
	gainTimeMs = (long)0;
}

public NPCommon_MailBriefInfo(
	 long _id
	, String _title
	, long _createTimeMs
	, long _readTimeMs
	, long _gainTimeMs
) {	id = _id;
	title = _title;
	createTimeMs = _createTimeMs;
	readTimeMs = _readTimeMs;
	gainTimeMs = _gainTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getId() { return id; }
public void setId(long _id) { id = _id; }
public String getTitle() { return title; }
public void setTitle(String _title) { title = _title; }
public long getCreateTimeMs() { return createTimeMs; }
public void setCreateTimeMs(long _createTimeMs) { createTimeMs = _createTimeMs; }
public long getReadTimeMs() { return readTimeMs; }
public void setReadTimeMs(long _readTimeMs) { readTimeMs = _readTimeMs; }
public long getGainTimeMs() { return gainTimeMs; }
public void setGainTimeMs(long _gainTimeMs) { gainTimeMs = _gainTimeMs; }


public final int GetBufSize() {
	int _size = 32;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 34;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(title);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) title = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) createTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) readTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) gainTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, title);
	_buf.putLong(createTimeMs);
	_buf.putLong(readTimeMs);
	_buf.putLong(gainTimeMs);
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

