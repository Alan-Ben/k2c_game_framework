package GC2GS.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 攻击副本怪物-恢复指定大臣出战次数
 **/
public class GC2GS_037_006_ReqRecoverHeroFight implements ALBasicProtocolPack._IALProtocolStructure {
private long heroId;


public GC2GS_037_006_ReqRecoverHeroFight() {
	heroId = (long)0;
}

public GC2GS_037_006_ReqRecoverHeroFight(
	 long _heroId
) {	heroId = _heroId;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)6; }

public long getHeroId() { return heroId; }
public void setHeroId(long _heroId) { heroId = _heroId; }


public final int GetBufSize() {
	int _size = 8;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)6);
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

