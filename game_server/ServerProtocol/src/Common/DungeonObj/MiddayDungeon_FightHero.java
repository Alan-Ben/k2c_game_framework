package Common.DungeonObj;

import java.nio.ByteBuffer;
/*********
 * 午间副本_出战大臣信息
 **/
public class MiddayDungeon_FightHero implements ALBasicProtocolPack._IALProtocolStructure {
/** 大臣ID */
private long heroId;
/** 出战次数 */
private short num;


public MiddayDungeon_FightHero() {
	heroId = (long)0;
	num = (short)0;
}

public MiddayDungeon_FightHero(
	 long _heroId
	, short _num
) {	heroId = _heroId;
	num = _num;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 大臣ID */
public long getHeroId() { return heroId; }
/** 大臣ID */
public void setHeroId(long _heroId) { heroId = _heroId; }
/** 出战次数 */
public short getNum() { return num; }
/** 出战次数 */
public void setNum(short _num) { num = _num; }


public final int GetBufSize() {
	int _size = 10;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 12;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) heroId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) num = _buf.getShort();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(heroId);
	_buf.putShort(num);
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

