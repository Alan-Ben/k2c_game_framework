package GS2GC.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
public class GS2GC_037_004_RetUpgradeDungeonLvl implements ALBasicProtocolPack._IALProtocolStructure {
/** 升级后的等级 */
private int upgradeLvl;


public GS2GC_037_004_RetUpgradeDungeonLvl() {
	upgradeLvl = 0;
}

public GS2GC_037_004_RetUpgradeDungeonLvl(
	 int _upgradeLvl
) {	upgradeLvl = _upgradeLvl;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)4; }

/** 升级后的等级 */
public int getUpgradeLvl() { return upgradeLvl; }
/** 升级后的等级 */
public void setUpgradeLvl(int _upgradeLvl) { upgradeLvl = _upgradeLvl; }


public final int GetBufSize() {
	int _size = 4;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) upgradeLvl = _buf.getInt();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putInt(upgradeLvl);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)4);
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

