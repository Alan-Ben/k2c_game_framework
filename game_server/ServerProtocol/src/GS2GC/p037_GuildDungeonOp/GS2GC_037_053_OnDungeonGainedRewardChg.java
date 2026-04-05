package GS2GC.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 已领取奖励的副本怪物数据列表
 **/
public class GS2GC_037_053_OnDungeonGainedRewardChg implements ALBasicProtocolPack._IALProtocolStructure {
/** 已领取奖励的怪物数据列表 */
private java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_DungeonMonster> gainedRewardMonsterIdList;


public GS2GC_037_053_OnDungeonGainedRewardChg() {
	gainedRewardMonsterIdList = new java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_DungeonMonster>();
}

public GS2GC_037_053_OnDungeonGainedRewardChg(
	 java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_DungeonMonster> _gainedRewardMonsterIdList
) {	gainedRewardMonsterIdList = _gainedRewardMonsterIdList;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)53; }

/** 已领取奖励的怪物数据列表 */
public java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_DungeonMonster> getGainedRewardMonsterIdList() { return gainedRewardMonsterIdList; }
/** 已领取奖励的怪物数据列表 */
public void addGainedRewardMonsterIdList(Common.GuildDungeonObj.GuildDungeon_DungeonMonster _gainedRewardMonsterIdList) { gainedRewardMonsterIdList.add(_gainedRewardMonsterIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (gainedRewardMonsterIdList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (gainedRewardMonsterIdList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _gainedRewardMonsterIdListCount = _buf.getShort();
	for(int _i = 0; _i < _gainedRewardMonsterIdListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_DungeonMonster _gainedRewardMonsterIdList = new Common.GuildDungeonObj.GuildDungeon_DungeonMonster();
		if(_buf.remaining() <= 0) return;
	int __gainedRewardMonsterIdListCustLen = _buf.getInt();
	int __gainedRewardMonsterIdListCurPos = _buf.position();
	_gainedRewardMonsterIdList.ReadUnzipBuf(_buf, __gainedRewardMonsterIdListCurPos + __gainedRewardMonsterIdListCustLen);
	_buf.position(__gainedRewardMonsterIdListCurPos + __gainedRewardMonsterIdListCustLen);

		gainedRewardMonsterIdList.add(_gainedRewardMonsterIdList);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)gainedRewardMonsterIdList.size());
	for(int _i = 0; _i < gainedRewardMonsterIdList.size(); _i++) { 
		_buf.putInt(gainedRewardMonsterIdList.get(_i).GetBufSize());
	gainedRewardMonsterIdList.get(_i).PutUnzipBuf(_buf);
	}
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)53);
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

