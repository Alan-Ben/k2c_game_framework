package NPGS2GC.p001_BasicOp;

import java.nio.ByteBuffer;
public class NPGS2GC_001_001_RetBasicInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 服务器版本 */
private String version;
private long timeTag;
private int timezone;
/** 夏令时时差 */
private int dstOffset;
/** 资源版本 */
private String resVersion;


public NPGS2GC_001_001_RetBasicInfo() {
	version = "";
	timeTag = (long)0;
	timezone = 0;
	dstOffset = 0;
	resVersion = "";
}

public NPGS2GC_001_001_RetBasicInfo(
	 String _version
	, long _timeTag
	, int _timezone
	, int _dstOffset
	, String _resVersion
) {	version = _version;
	timeTag = _timeTag;
	timezone = _timezone;
	dstOffset = _dstOffset;
	resVersion = _resVersion;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)1; }

/** 服务器版本 */
public String getVersion() { return version; }
/** 服务器版本 */
public void setVersion(String _version) { version = _version; }
public long getTimeTag() { return timeTag; }
public void setTimeTag(long _timeTag) { timeTag = _timeTag; }
public int getTimezone() { return timezone; }
public void setTimezone(int _timezone) { timezone = _timezone; }
/** 夏令时时差 */
public int getDstOffset() { return dstOffset; }
/** 夏令时时差 */
public void setDstOffset(int _dstOffset) { dstOffset = _dstOffset; }
/** 资源版本 */
public String getResVersion() { return resVersion; }
/** 资源版本 */
public void setResVersion(String _resVersion) { resVersion = _resVersion; }


public final int GetBufSize() {
	int _size = 16;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(version);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resVersion);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(version);
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(resVersion);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) version = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timeTag = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timezone = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dstOffset = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) resVersion = ALBasicProtocolPack.ALProtocolCommon.GetStringFromBuf(_buf);
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, version);
	_buf.putLong(timeTag);
	_buf.putInt(timezone);
	_buf.putInt(dstOffset);
	ALBasicProtocolPack.ALProtocolCommon.PutStringIntoBuf(_buf, resVersion);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)1);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)1);
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

