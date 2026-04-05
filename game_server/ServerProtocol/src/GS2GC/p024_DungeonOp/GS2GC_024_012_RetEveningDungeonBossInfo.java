package GS2GC.p024_DungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_024_012_RetEveningDungeonBossInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** Boss信息 */
private Common.DungeonObj.EveningDungeon_BossInfo bossInfo;


public GS2GC_024_012_RetEveningDungeonBossInfo() {
	bossInfo = new Common.DungeonObj.EveningDungeon_BossInfo();
}

public GS2GC_024_012_RetEveningDungeonBossInfo(
	 Common.DungeonObj.EveningDungeon_BossInfo _bossInfo
) {	bossInfo = _bossInfo;
}

public final byte getMainOrder() { return (byte)24; }

public final byte getSubOrder() { return (byte)12; }

/** Boss信息 */
public Common.DungeonObj.EveningDungeon_BossInfo getBossInfo() { return bossInfo; }
/** Boss信息 */
public void setBossInfo(Common.DungeonObj.EveningDungeon_BossInfo _bossInfo) { bossInfo = _bossInfo; }


public final int GetBufSize() {
	int _size = 40;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _bossInfoCustLen = _buf.getInt();
	int _bossInfoCurPos = _buf.position();
	bossInfo.ReadUnzipBuf(_buf, _bossInfoCurPos + _bossInfoCustLen);
	_buf.position(_bossInfoCurPos + _bossInfoCustLen);

}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(bossInfo.GetBufSize());
	bossInfo.PutUnzipBuf(_buf);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)24);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)24);
	_recBuf.put((byte)12);
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

