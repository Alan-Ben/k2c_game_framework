package Common.GuildDungeonObj;

import java.nio.ByteBuffer;
/*********
 * 公会副本日志-自动开启
 **/
public class GuildDungeon_LogAutoStart implements ALBasicProtocolPack._IALProtocolStructure {
/** 副本ID */
private long dungeonId;
/** 联盟财富消耗数值 */
private long costValue;


public GuildDungeon_LogAutoStart() {
	dungeonId = (long)0;
	costValue = (long)0;
}

public GuildDungeon_LogAutoStart(
	 long _dungeonId
	, long _costValue
) {	dungeonId = _dungeonId;
	costValue = _costValue;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 副本ID */
public long getDungeonId() { return dungeonId; }
/** 副本ID */
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
/** 联盟财富消耗数值 */
public long getCostValue() { return costValue; }
/** 联盟财富消耗数值 */
public void setCostValue(long _costValue) { costValue = _costValue; }


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
	if(_buf.remaining() > 0) dungeonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) costValue = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dungeonId);
	_buf.putLong(costValue);
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

