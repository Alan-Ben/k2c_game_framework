package Common.GuildDungeonObj;

import java.nio.ByteBuffer;
/*********
 * 公会副本实例数据
 **/
public class GuildDungeon_InstanceInfo implements ALBasicProtocolPack._IALProtocolStructure {
/** 实例ID */
private long id;
/** 公会副本ID */
private long dungeonId;
/** 公会副本等级 */
private int lvl;
/** 开启时间 */
private long startMs;
/** 怪物列表 */
private java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_Monster> monsterList;
/** 已标记的怪物ID列表 */
private java.util.ArrayList<Long> tagMonsterIdLiist;


public GuildDungeon_InstanceInfo() {
	id = (long)0;
	dungeonId = (long)0;
	lvl = 0;
	startMs = (long)0;
	monsterList = new java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_Monster>();
	tagMonsterIdLiist = new java.util.ArrayList<Long>();
}

public GuildDungeon_InstanceInfo(
	 long _id
	, long _dungeonId
	, int _lvl
	, long _startMs
	, java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_Monster> _monsterList
	, java.util.ArrayList<Long> _tagMonsterIdLiist
) {	id = _id;
	dungeonId = _dungeonId;
	lvl = _lvl;
	startMs = _startMs;
	monsterList = _monsterList;
	tagMonsterIdLiist = _tagMonsterIdLiist;
}

public final byte getMainOrder() { return (byte)0; }

public final byte getSubOrder() { return (byte)0; }

/** 实例ID */
public long getId() { return id; }
/** 实例ID */
public void setId(long _id) { id = _id; }
/** 公会副本ID */
public long getDungeonId() { return dungeonId; }
/** 公会副本ID */
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
/** 公会副本等级 */
public int getLvl() { return lvl; }
/** 公会副本等级 */
public void setLvl(int _lvl) { lvl = _lvl; }
/** 开启时间 */
public long getStartMs() { return startMs; }
/** 开启时间 */
public void setStartMs(long _startMs) { startMs = _startMs; }
/** 怪物列表 */
public java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_Monster> getMonsterList() { return monsterList; }
/** 怪物列表 */
public void addMonsterList(Common.GuildDungeonObj.GuildDungeon_Monster _monsterList) { monsterList.add(_monsterList); }
/** 已标记的怪物ID列表 */
public java.util.ArrayList<Long> getTagMonsterIdLiist() { return tagMonsterIdLiist; }
/** 已标记的怪物ID列表 */
public void addTagMonsterIdLiist(long _tagMonsterIdLiist) { tagMonsterIdLiist.add(_tagMonsterIdLiist); }


public final int GetBufSize() {
	int _size = 28;
	_size += 2 + (monsterList.size() * 21);
	_size += 2 + (tagMonsterIdLiist.size() * 8);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 30;
	_size += 2 + (monsterList.size() * 21);
	_size += 2 + (tagMonsterIdLiist.size() * 8);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) id = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) dungeonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) startMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _monsterListCount = _buf.getShort();
	for(int _i = 0; _i < _monsterListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_Monster _monsterList = new Common.GuildDungeonObj.GuildDungeon_Monster();
		if(_buf.remaining() <= 0) return;
	int __monsterListCustLen = _buf.getInt();
	int __monsterListCurPos = _buf.position();
	_monsterList.ReadUnzipBuf(_buf, __monsterListCurPos + __monsterListCustLen);
	_buf.position(__monsterListCurPos + __monsterListCustLen);

		monsterList.add(_monsterList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _tagMonsterIdLiistCount = _buf.getShort();
	for(int _i = 0; _i < _tagMonsterIdLiistCount; _i++) { 
		long _tagMonsterIdLiist = (long)0;
		if(_buf.remaining() > 0) _tagMonsterIdLiist = _buf.getLong();
		tagMonsterIdLiist.add(_tagMonsterIdLiist);
	}
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putLong(id);
	_buf.putLong(dungeonId);
	_buf.putInt(lvl);
	_buf.putLong(startMs);
	_buf.putShort((short)monsterList.size());
	for(int _i = 0; _i < monsterList.size(); _i++) { 
		_buf.putInt(monsterList.get(_i).GetBufSize());
	monsterList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)tagMonsterIdLiist.size());
	for(int _i = 0; _i < tagMonsterIdLiist.size(); _i++) { 
		_buf.putLong(tagMonsterIdLiist.get(_i));
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

