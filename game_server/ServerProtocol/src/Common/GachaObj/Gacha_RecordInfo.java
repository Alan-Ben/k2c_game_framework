package Common.GachaObj;

import java.nio.ByteBuffer;
/*********
 * 抽卡记录信息
 **/
public class Gacha_RecordInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 卡池物品id */
private long itemId;
/** 抽卡时间戳 秒 */
private int rollTimeSec;
/** 数据id */
private long dbId;


public Gacha_RecordInfo() {
	itemId = (long)0;
	rollTimeSec = 0;
	dbId = (long)0;
}

public Gacha_RecordInfo(
	 long _itemId
	, int _rollTimeSec
	, long _dbId
) {	itemId = _itemId;
	rollTimeSec = _rollTimeSec;
	dbId = _dbId;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 卡池物品id */
public long getItemId() { return itemId; }
/** 卡池物品id */
public void setItemId(long _itemId) { itemId = _itemId; }
/** 抽卡时间戳 秒 */
public int getRollTimeSec() { return rollTimeSec; }
/** 抽卡时间戳 秒 */
public void setRollTimeSec(int _rollTimeSec) { rollTimeSec = _rollTimeSec; }
/** 数据id */
public long getDbId() { return dbId; }
/** 数据id */
public void setDbId(long _dbId) { dbId = _dbId; }


public final int GetBufSize() {
	int _size = 20;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 22;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) itemId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) rollTimeSec = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dbId = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(itemId);
	_buf.putInt(rollTimeSec);
	_buf.putLong(dbId);
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

