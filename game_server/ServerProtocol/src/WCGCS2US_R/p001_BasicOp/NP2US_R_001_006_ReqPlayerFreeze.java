package WCGCS2US_R.p001_BasicOp;

import java.nio.ByteBuffer;
public class NP2US_R_001_006_ReqPlayerFreeze implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家cid */
private long cid;
/** 是否冻结处理 */
private boolean isFreeze;
/** 冻结时间 秒 */
private long timeSec;


public NP2US_R_001_006_ReqPlayerFreeze() {
	cid = (long)0;
	isFreeze = false;
	timeSec = (long)0;
}

public NP2US_R_001_006_ReqPlayerFreeze(
	 long _cid
	, boolean _isFreeze
	, long _timeSec
) {	cid = _cid;
	isFreeze = _isFreeze;
	timeSec = _timeSec;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)6; }

/** 玩家cid */
public long getCid() { return cid; }
/** 玩家cid */
public void setCid(long _cid) { cid = _cid; }
/** 是否冻结处理 */
public boolean getIsFreeze() { return isFreeze; }
/** 是否冻结处理 */
public void setIsFreeze(boolean _isFreeze) { isFreeze = _isFreeze; }
/** 冻结时间 秒 */
public long getTimeSec() { return timeSec; }
/** 冻结时间 秒 */
public void setTimeSec(long _timeSec) { timeSec = _timeSec; }


public final int GetBufSize() {
	int _size = 17;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isFreeze = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) timeSec = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.put(isFreeze?(byte)1:(byte)0);
	_buf.putLong(timeSec);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)6);
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

