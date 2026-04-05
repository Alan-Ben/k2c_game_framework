package Common;

import java.nio.ByteBuffer;
public class WCGGS2GC_SummonInfo implements ALBasicProtocolPack._IALProtocolStructure {
private int pos;
private long summonId;
private int pauseLeftSec;
private int beginUnlockTimeS;


public WCGGS2GC_SummonInfo() {
	pos = 0;
	summonId = (long)0;
	pauseLeftSec = 0;
	beginUnlockTimeS = 0;
}

public WCGGS2GC_SummonInfo(
	 int _pos
	, long _summonId
	, int _pauseLeftSec
	, int _beginUnlockTimeS
) {	pos = _pos;
	summonId = _summonId;
	pauseLeftSec = _pauseLeftSec;
	beginUnlockTimeS = _beginUnlockTimeS;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public int getPos() { return pos; }
public void setPos(int _pos) { pos = _pos; }
public long getSummonId() { return summonId; }
public void setSummonId(long _summonId) { summonId = _summonId; }
public int getPauseLeftSec() { return pauseLeftSec; }
public void setPauseLeftSec(int _pauseLeftSec) { pauseLeftSec = _pauseLeftSec; }
public int getBeginUnlockTimeS() { return beginUnlockTimeS; }
public void setBeginUnlockTimeS(int _beginUnlockTimeS) { beginUnlockTimeS = _beginUnlockTimeS; }


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
	if(_buf.remaining() > 0) pos = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) summonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) pauseLeftSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) beginUnlockTimeS = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(pos);
	_buf.putLong(summonId);
	_buf.putInt(pauseLeftSec);
	_buf.putInt(beginUnlockTimeS);
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

