package GC2GS.p021_PlayerInfo;

import java.nio.ByteBuffer;
public class GC2GS_021_031_ReqDealFriendApply implements ALBasicProtocolPack._IALProtocolStructure {
/** true-同意，false-拒绝 */
private boolean isAgree;
private long applyCid;


public GC2GS_021_031_ReqDealFriendApply() {
	isAgree = false;
	applyCid = (long)0;
}

public GC2GS_021_031_ReqDealFriendApply(
	 boolean _isAgree
	, long _applyCid
) {	isAgree = _isAgree;
	applyCid = _applyCid;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)31; }

/** true-同意，false-拒绝 */
public boolean getIsAgree() { return isAgree; }
/** true-同意，false-拒绝 */
public void setIsAgree(boolean _isAgree) { isAgree = _isAgree; }
public long getApplyCid() { return applyCid; }
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }


public final int GetBufSize() {
	int _size = 9;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) isAgree = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) applyCid = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.put(isAgree?(byte)1:(byte)0);
	_buf.putLong(applyCid);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)31);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)31);
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

