package Common.DungeonObj;

import java.nio.ByteBuffer;
/*********
 * 晚间副本信息
 **/
public class EveningDungeon_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 本轮开始时间戳 */
private long roundStartTimeMS;
/** 已战斗过的英雄列表 */
private java.util.ArrayList<Long> hadFightHeroList;


public EveningDungeon_Info() {
	roundStartTimeMS = (long)0;
	hadFightHeroList = new java.util.ArrayList<Long>();
}

public EveningDungeon_Info(
	 long _roundStartTimeMS
	, java.util.ArrayList<Long> _hadFightHeroList
) {	roundStartTimeMS = _roundStartTimeMS;
	hadFightHeroList = _hadFightHeroList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 本轮开始时间戳 */
public long getRoundStartTimeMS() { return roundStartTimeMS; }
/** 本轮开始时间戳 */
public void setRoundStartTimeMS(long _roundStartTimeMS) { roundStartTimeMS = _roundStartTimeMS; }
/** 已战斗过的英雄列表 */
public java.util.ArrayList<Long> getHadFightHeroList() { return hadFightHeroList; }
/** 已战斗过的英雄列表 */
public void addHadFightHeroList(long _hadFightHeroList) { hadFightHeroList.add(_hadFightHeroList); }


public final int GetBufSize() {
	int _size = 8;
	_size += 2 + (hadFightHeroList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (hadFightHeroList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roundStartTimeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadFightHeroListCount = _buf.getShort();
	for(int _i = 0; _i < _hadFightHeroListCount; _i++) { 
		long _hadFightHeroList = (long)0;
		if(_buf.remaining() > 0) _hadFightHeroList = _buf.getLong();
		hadFightHeroList.add(_hadFightHeroList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(roundStartTimeMS);
	_buf.putShort((short)hadFightHeroList.size());
	for(int _i = 0; _i < hadFightHeroList.size(); _i++) { 
		_buf.putLong(hadFightHeroList.get(_i));
	}
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

