package GC2GS.p021_PlayerInfo;

import java.nio.ByteBuffer;
public class GC2GS_021_025_ReqDoneAchieveStep implements ALBasicProtocolPack._IALProtocolStructure {
private long achieveId;
private int step;


public GC2GS_021_025_ReqDoneAchieveStep() {
	achieveId = (long)0;
	step = 0;
}

public GC2GS_021_025_ReqDoneAchieveStep(
	 long _achieveId
	, int _step
) {	achieveId = _achieveId;
	step = _step;
}

public final byte getMainOrder() { return (byte)21; }

public final byte getSubOrder() { return (byte)25; }

public long getAchieveId() { return achieveId; }
public void setAchieveId(long _achieveId) { achieveId = _achieveId; }
public int getStep() { return step; }
public void setStep(int _step) { step = _step; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) achieveId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) step = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(achieveId);
	_buf.putInt(step);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)21);
	_buf.put((byte)25);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)21);
	_recBuf.put((byte)25);
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

