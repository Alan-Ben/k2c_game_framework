package GS2GC.p023_ArenaOp;

import java.nio.ByteBuffer;
public class GS2GC_023_013_RetArenaAKeyAttack implements ALBasicProtocolPack._IALProtocolStructure {
/** 战斗结果 */
private Common.ArenaObj.Arena_BattleResult battleResult;


public GS2GC_023_013_RetArenaAKeyAttack() {
	battleResult = new Common.ArenaObj.Arena_BattleResult();
}

public GS2GC_023_013_RetArenaAKeyAttack(
	 Common.ArenaObj.Arena_BattleResult _battleResult
) {	battleResult = _battleResult;
}

public final byte getMainOrder() { return (byte)23; }

public final byte getSubOrder() { return (byte)13; }

/** 战斗结果 */
public Common.ArenaObj.Arena_BattleResult getBattleResult() { return battleResult; }
/** 战斗结果 */
public void setBattleResult(Common.ArenaObj.Arena_BattleResult _battleResult) { battleResult = _battleResult; }


public final int GetBufSize() {
	int _size = 0;
	_size += 4 + battleResult.GetBufSize();

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + battleResult.GetBufSize();

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _battleResultCustLen = _buf.getInt();
	int _battleResultCurPos = _buf.position();
	battleResult.ReadUnzipBuf(_buf, _battleResultCurPos + _battleResultCustLen);
	_buf.position(_battleResultCurPos + _battleResultCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(battleResult.GetBufSize());
	battleResult.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)23);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)23);
	_recBuf.put((byte)13);
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

