package Common.DungeonObj;

import java.nio.ByteBuffer;
/*********
 * 午间副本信息
 **/
public class MiddayDungeon_Info implements ALBasicProtocolPack._IALProtocolStructure {
/** 本轮开始时间戳 */
private long roundStartTimeMS;
/** Boss信息 */
private Common.DungeonObj.MiddayDungeon_BossInfo bossInfo;
/** 已战斗过的英雄列表 */
private java.util.ArrayList<Common.DungeonObj.MiddayDungeon_FightHero> hadFightHeroList;
/** 借用过大臣的玩家列表 */
private java.util.ArrayList<Long> borrowCidList;


public MiddayDungeon_Info() {
	roundStartTimeMS = (long)0;
	bossInfo = new Common.DungeonObj.MiddayDungeon_BossInfo();
	hadFightHeroList = new java.util.ArrayList<Common.DungeonObj.MiddayDungeon_FightHero>();
	borrowCidList = new java.util.ArrayList<Long>();
}

public MiddayDungeon_Info(
	 long _roundStartTimeMS
	, Common.DungeonObj.MiddayDungeon_BossInfo _bossInfo
	, java.util.ArrayList<Common.DungeonObj.MiddayDungeon_FightHero> _hadFightHeroList
	, java.util.ArrayList<Long> _borrowCidList
) {	roundStartTimeMS = _roundStartTimeMS;
	bossInfo = _bossInfo;
	hadFightHeroList = _hadFightHeroList;
	borrowCidList = _borrowCidList;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 本轮开始时间戳 */
public long getRoundStartTimeMS() { return roundStartTimeMS; }
/** 本轮开始时间戳 */
public void setRoundStartTimeMS(long _roundStartTimeMS) { roundStartTimeMS = _roundStartTimeMS; }
/** Boss信息 */
public Common.DungeonObj.MiddayDungeon_BossInfo getBossInfo() { return bossInfo; }
/** Boss信息 */
public void setBossInfo(Common.DungeonObj.MiddayDungeon_BossInfo _bossInfo) { bossInfo = _bossInfo; }
/** 已战斗过的英雄列表 */
public java.util.ArrayList<Common.DungeonObj.MiddayDungeon_FightHero> getHadFightHeroList() { return hadFightHeroList; }
/** 已战斗过的英雄列表 */
public void addHadFightHeroList(Common.DungeonObj.MiddayDungeon_FightHero _hadFightHeroList) { hadFightHeroList.add(_hadFightHeroList); }
/** 借用过大臣的玩家列表 */
public java.util.ArrayList<Long> getBorrowCidList() { return borrowCidList; }
/** 借用过大臣的玩家列表 */
public void addBorrowCidList(long _borrowCidList) { borrowCidList.add(_borrowCidList); }


public final int GetBufSize() {
	int _size = 24;
	_size += 2 + (hadFightHeroList.size() * 14);
	_size += 2 + (borrowCidList.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 26;
	_size += 2 + (hadFightHeroList.size() * 14);
	_size += 2 + (borrowCidList.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) roundStartTimeMS = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	int _bossInfoCustLen = _buf.getInt();
	int _bossInfoCurPos = _buf.position();
	bossInfo.ReadUnzipBuf(_buf, _bossInfoCurPos + _bossInfoCustLen);
	_buf.position(_bossInfoCurPos + _bossInfoCustLen);

	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _hadFightHeroListCount = _buf.getShort();
	for(int _i = 0; _i < _hadFightHeroListCount; _i++) { 
		Common.DungeonObj.MiddayDungeon_FightHero _hadFightHeroList = new Common.DungeonObj.MiddayDungeon_FightHero();
		if(_buf.remaining() <= 0) return;
	int __hadFightHeroListCustLen = _buf.getInt();
	int __hadFightHeroListCurPos = _buf.position();
	_hadFightHeroList.ReadUnzipBuf(_buf, __hadFightHeroListCurPos + __hadFightHeroListCustLen);
	_buf.position(__hadFightHeroListCurPos + __hadFightHeroListCustLen);

		hadFightHeroList.add(_hadFightHeroList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _borrowCidListCount = _buf.getShort();
	for(int _i = 0; _i < _borrowCidListCount; _i++) { 
		long _borrowCidList = (long)0;
		if(_buf.remaining() > 0) _borrowCidList = _buf.getLong();
		borrowCidList.add(_borrowCidList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(roundStartTimeMS);
	_buf.putInt(bossInfo.GetBufSize());
	bossInfo.PutUnzipBuf(_buf);
	_buf.putShort((short)hadFightHeroList.size());
	for(int _i = 0; _i < hadFightHeroList.size(); _i++) { 
		_buf.putInt(hadFightHeroList.get(_i).GetBufSize());
	hadFightHeroList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)borrowCidList.size());
	for(int _i = 0; _i < borrowCidList.size(); _i++) { 
		_buf.putLong(borrowCidList.get(_i));
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

