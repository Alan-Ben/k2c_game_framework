package GS2GC.p023_ArenaOp;

import java.nio.ByteBuffer;
public class GS2GC_023_051_OnArenaBattleInfoChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 战斗数据 */
private Common.ArenaObj.Arena_BattleInfo battleInfo;


public GS2GC_023_051_OnArenaBattleInfoChg() {
	battleInfo = new Common.ArenaObj.Arena_BattleInfo();
}

public GS2GC_023_051_OnArenaBattleInfoChg(
	 Common.ArenaObj.Arena_BattleInfo _battleInfo
) {	battleInfo = _battleInfo;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)51; }

/** 战斗数据 */
public Common.ArenaObj.Arena_BattleInfo getBattleInfo() { return battleInfo; }
/** 战斗数据 */
public void setBattleInfo(Common.ArenaObj.Arena_BattleInfo _battleInfo) { battleInfo = _battleInfo; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + battleInfo.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + battleInfo.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _battleInfoCustLen = _buf.getInt();
	int _battleInfoCurPos = _buf.position();
	battleInfo.ReadUnzipBuf(_buf, _battleInfoCurPos + _battleInfoCustLen);
	_buf.position(_battleInfoCurPos + _battleInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(battleInfo.GetBufSize());
	battleInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)51);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)51);
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

