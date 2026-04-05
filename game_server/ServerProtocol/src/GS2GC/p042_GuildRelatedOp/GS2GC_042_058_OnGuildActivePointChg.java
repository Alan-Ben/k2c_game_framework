package GS2GC.p042_GuildRelatedOp;

import java.nio.ByteBuffer;
/*********
 * 联盟活跃点变化数据
 **/
public class GS2GC_042_058_OnGuildActivePointChg implements ALBasicProtocolPack._IALProtocolStructure {
private long activePoint;
private int targetLvl;


public GS2GC_042_058_OnGuildActivePointChg() {
	activePoint = (long)0;
	targetLvl = 0;
}

public GS2GC_042_058_OnGuildActivePointChg(
	 long _activePoint
	, int _targetLvl
) {	activePoint = _activePoint;
	targetLvl = _targetLvl;
}

public final byte getMainOrder() { return (byte)42; }

public final byte getSubOrder() { return (byte)58; }

public long getActivePoint() { return activePoint; }
public void setActivePoint(long _activePoint) { activePoint = _activePoint; }
public int getTargetLvl() { return targetLvl; }
public void setTargetLvl(int _targetLvl) { targetLvl = _targetLvl; }


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
	if(_buf.remaining() > 0) activePoint = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) targetLvl = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(activePoint);
	_buf.putInt(targetLvl);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)42);
	_buf.put((byte)58);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)42);
	_recBuf.put((byte)58);
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

