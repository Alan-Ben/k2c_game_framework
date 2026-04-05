package Common.GuildDungeonObj;

import java.nio.ByteBuffer;
/*********
 * 公会副本伤害排行
 **/
public class GuildDungeon_DamageRankItem implements ALBasicProtocolPack._IALProtocolStructure {
/** 玩家CID */
private long cid;
/** 分数 */
private long value;


public GuildDungeon_DamageRankItem() {
	cid = (long)0;
	value = (long)0;
}

public GuildDungeon_DamageRankItem(
	 long _cid
	, long _value
) {	cid = _cid;
	value = _value;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 玩家CID */
public long getCid() { return cid; }
/** 玩家CID */
public void setCid(long _cid) { cid = _cid; }
/** 分数 */
public long getValue() { return value; }
/** 分数 */
public void setValue(long _value) { value = _value; }


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
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) value = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(value);
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

