package WCGCS2US.p002_MatchOp;

import java.nio.ByteBuffer;
public class BattleEnd_BattileInfo implements ALBasicProtocolPack._IALProtocolStructure {
private long battleStartTimeMs;
private long battleEndTimeMs;


public BattleEnd_BattileInfo() {
	battleStartTimeMs = (long)0;
	battleEndTimeMs = (long)0;
}

public BattleEnd_BattileInfo(
	 long _battleStartTimeMs
	, long _battleEndTimeMs
) {	battleStartTimeMs = _battleStartTimeMs;
	battleEndTimeMs = _battleEndTimeMs;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getBattleStartTimeMs() { return battleStartTimeMs; }
public void setBattleStartTimeMs(long _battleStartTimeMs) { battleStartTimeMs = _battleStartTimeMs; }
public long getBattleEndTimeMs() { return battleEndTimeMs; }
public void setBattleEndTimeMs(long _battleEndTimeMs) { battleEndTimeMs = _battleEndTimeMs; }


public final int GetBufSize() {
	int _size = 16;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) battleStartTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) battleEndTimeMs = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(battleStartTimeMs);
	_buf.putLong(battleEndTimeMs);
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

