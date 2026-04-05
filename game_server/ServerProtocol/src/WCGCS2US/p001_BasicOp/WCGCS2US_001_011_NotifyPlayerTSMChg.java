package WCGCS2US.p001_BasicOp;

import java.nio.ByteBuffer;
public class WCGCS2US_001_011_NotifyPlayerTSMChg implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private int tsmState;
private long tsmStartTimeMS;


public WCGCS2US_001_011_NotifyPlayerTSMChg() {
	uid = (long)0;
	tsmState = 0;
	tsmStartTimeMS = (long)0;
}

public WCGCS2US_001_011_NotifyPlayerTSMChg(
	 long _uid
	, int _tsmState
	, long _tsmStartTimeMS
) {	uid = _uid;
	tsmState = _tsmState;
	tsmStartTimeMS = _tsmStartTimeMS;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)11; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public int getTsmState() { return tsmState; }
public void setTsmState(int _tsmState) { tsmState = _tsmState; }
public long getTsmStartTimeMS() { return tsmStartTimeMS; }
public void setTsmStartTimeMS(long _tsmStartTimeMS) { tsmStartTimeMS = _tsmStartTimeMS; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) uid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) tsmState = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) tsmStartTimeMS = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putInt(tsmState);
	_buf.putLong(tsmStartTimeMS);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)11);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)11);
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

