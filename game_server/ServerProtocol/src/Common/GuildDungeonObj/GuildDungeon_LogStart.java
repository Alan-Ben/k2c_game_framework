package Common.GuildDungeonObj;

import java.nio.ByteBuffer;
/*********
 * 公会副本日志-开启
 **/
public class GuildDungeon_LogStart implements ALBasicProtocolPack._IALProtocolStructure {
private long cid;
/** 副本ID */
private long dungeonId;
/** 开启方式 */
private Common.GuildDungeonEnum.EGuildDungeon_StartType startType;
/** 开启消耗数值 */
private long startCost;


public GuildDungeon_LogStart() {
	cid = (long)0;
	dungeonId = (long)0;
	startType = Common.GuildDungeonEnum.EGuildDungeon_StartType.values()[0];
	startCost = (long)0;
}

public GuildDungeon_LogStart(
	 long _cid
	, long _dungeonId
	, Common.GuildDungeonEnum.EGuildDungeon_StartType _startType
	, long _startCost
) {	cid = _cid;
	dungeonId = _dungeonId;
	startType = _startType;
	startCost = _startCost;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/** 副本ID */
public long getDungeonId() { return dungeonId; }
/** 副本ID */
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
/** 开启方式 */
public Common.GuildDungeonEnum.EGuildDungeon_StartType getStartType() { return startType; }
/** 开启方式 */
public void setStartType(Common.GuildDungeonEnum.EGuildDungeon_StartType _startType) { startType = _startType; }
/** 开启消耗数值 */
public long getStartCost() { return startCost; }
/** 开启消耗数值 */
public void setStartCost(long _startCost) { startCost = _startCost; }


public final int GetBufSize() {
	int _size = 28;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dungeonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startType = Common.GuildDungeonEnum.EGuildDungeon_StartType.EGuildDungeon_StartType_FromInt(_buf.getInt());
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startCost = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(cid);
	_buf.putLong(dungeonId);
	_buf.putInt(startType.ordinal());

	_buf.putLong(startCost);
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

