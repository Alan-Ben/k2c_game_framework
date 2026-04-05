package GC2GS.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 开启副本
 **/
public class GC2GS_037_003_ReqStartDungeon implements ALBasicProtocolPack._IALProtocolStructure {
private long dungeonId;
private Common.GuildDungeonEnum.EGuildDungeon_StartType startType;


public GC2GS_037_003_ReqStartDungeon() {
	dungeonId = (long)0;
	startType = Common.GuildDungeonEnum.EGuildDungeon_StartType.values()[0];
}

public GC2GS_037_003_ReqStartDungeon(
	 long _dungeonId
	, Common.GuildDungeonEnum.EGuildDungeon_StartType _startType
) {	dungeonId = _dungeonId;
	startType = _startType;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)3; }

public long getDungeonId() { return dungeonId; }
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
public Common.GuildDungeonEnum.EGuildDungeon_StartType getStartType() { return startType; }
public void setStartType(Common.GuildDungeonEnum.EGuildDungeon_StartType _startType) { startType = _startType; }


public final int GetBufSize() {
	int _size = 12;

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dungeonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startType = Common.GuildDungeonEnum.EGuildDungeon_StartType.EGuildDungeon_StartType_FromInt(_buf.getInt());
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(dungeonId);
	_buf.putInt(startType.ordinal());

}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)3);
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

