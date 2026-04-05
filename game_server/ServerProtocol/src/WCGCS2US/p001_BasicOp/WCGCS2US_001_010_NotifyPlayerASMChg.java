package WCGCS2US.p001_BasicOp;

import java.nio.ByteBuffer;
public class WCGCS2US_001_010_NotifyPlayerASMChg implements ALBasicProtocolPack._IALProtocolStructure {
private long uid;
private int asmState;
private long asmStartTimeMS;


public WCGCS2US_001_010_NotifyPlayerASMChg() {
	uid = (long)0;
	asmState = 0;
	asmStartTimeMS = (long)0;
}

public WCGCS2US_001_010_NotifyPlayerASMChg(
	 long _uid
	, int _asmState
	, long _asmStartTimeMS
) {	uid = _uid;
	asmState = _asmState;
	asmStartTimeMS = _asmStartTimeMS;
}

public final byte getMainOrder() { return (byte)1; }

public final byte getSubOrder() { return (byte)10; }

public long getUid() { return uid; }
public void setUid(long _uid) { uid = _uid; }
public int getAsmState() { return asmState; }
public void setAsmState(int _asmState) { asmState = _asmState; }
public long getAsmStartTimeMS() { return asmStartTimeMS; }
public void setAsmStartTimeMS(long _asmStartTimeMS) { asmStartTimeMS = _asmStartTimeMS; }


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
	if(_buf.remaining() > 0) asmState = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) asmStartTimeMS = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(uid);
	_buf.putInt(asmState);
	_buf.putLong(asmStartTimeMS);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)1);
	_buf.put((byte)10);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)1);
	_recBuf.put((byte)10);
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

