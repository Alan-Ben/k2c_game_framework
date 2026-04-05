package GS2GC.p037_GuildDungeonOp;

import java.nio.ByteBuffer;
/*********
 * 公会副本-推送数据
 **/
public class GS2GC_037_056_OnDungeonPush implements ALBasicProtocolPack._IALProtocolStructure {
/** 设置数据列表 */
private java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_SetInfo> setList;
/** 实力数据列表 */
private java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_InstanceInfo> instanceList;
/** 已领取奖励的怪物数据列表 */
private java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_DungeonMonster> gainedRewardMonsterIdList;


public GS2GC_037_056_OnDungeonPush() {
	setList = new java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_SetInfo>();
	instanceList = new java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_InstanceInfo>();
	gainedRewardMonsterIdList = new java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_DungeonMonster>();
}

public GS2GC_037_056_OnDungeonPush(
	 java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_SetInfo> _setList
	, java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_InstanceInfo> _instanceList
	, java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_DungeonMonster> _gainedRewardMonsterIdList
) {	setList = _setList;
	instanceList = _instanceList;
	gainedRewardMonsterIdList = _gainedRewardMonsterIdList;
}

public final byte getMainOrder() { return (byte)37; }

public final byte getSubOrder() { return (byte)56; }

/** 设置数据列表 */
public java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_SetInfo> getSetList() { return setList; }
/** 设置数据列表 */
public void addSetList(Common.GuildDungeonObj.GuildDungeon_SetInfo _setList) { setList.add(_setList); }
/** 实力数据列表 */
public java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_InstanceInfo> getInstanceList() { return instanceList; }
/** 实力数据列表 */
public void addInstanceList(Common.GuildDungeonObj.GuildDungeon_InstanceInfo _instanceList) { instanceList.add(_instanceList); }
/** 已领取奖励的怪物数据列表 */
public java.util.ArrayList<Common.GuildDungeonObj.GuildDungeon_DungeonMonster> getGainedRewardMonsterIdList() { return gainedRewardMonsterIdList; }
/** 已领取奖励的怪物数据列表 */
public void addGainedRewardMonsterIdList(Common.GuildDungeonObj.GuildDungeon_DungeonMonster _gainedRewardMonsterIdList) { gainedRewardMonsterIdList.add(_gainedRewardMonsterIdList); }


public final int GetBufSize() {
	int _size = 0;
	_size += 2 + (setList.size() * 16);
	_size += 2;
	for(int _i = 0; _i < instanceList.size(); _i++) {
	_size += 4 + instanceList.get(_i).GetBufSize();
	}

	_size += 2 + (gainedRewardMonsterIdList.size() * 20);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (setList.size() * 16);
	_size += 2;
	for(int _i = 0; _i < instanceList.size(); _i++) {
	_size += 4 + instanceList.get(_i).GetBufSize();
	}

	_size += 2 + (gainedRewardMonsterIdList.size() * 20);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _setListCount = _buf.getShort();
	for(int _i = 0; _i < _setListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_SetInfo _setList = new Common.GuildDungeonObj.GuildDungeon_SetInfo();
		if(_buf.remaining() <= 0) return;
	int __setListCustLen = _buf.getInt();
	int __setListCurPos = _buf.position();
	_setList.ReadUnzipBuf(_buf, __setListCurPos + __setListCustLen);
	_buf.position(__setListCurPos + __setListCustLen);

		setList.add(_setList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _instanceListCount = _buf.getShort();
	for(int _i = 0; _i < _instanceListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_InstanceInfo _instanceList = new Common.GuildDungeonObj.GuildDungeon_InstanceInfo();
		if(_buf.remaining() <= 0) return;
	int __instanceListCustLen = _buf.getInt();
	int __instanceListCurPos = _buf.position();
	_instanceList.ReadUnzipBuf(_buf, __instanceListCurPos + __instanceListCustLen);
	_buf.position(__instanceListCurPos + __instanceListCustLen);

		instanceList.add(_instanceList);
	}
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
	_buf.putShort((short)setList.size());
	for(int _i = 0; _i < setList.size(); _i++) { 
		_buf.putInt(setList.get(_i).GetBufSize());
	setList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)instanceList.size());
	for(int _i = 0; _i < instanceList.size(); _i++) { 
		_buf.putInt(instanceList.get(_i).GetBufSize());
	instanceList.get(_i).PutUnzipBuf(_buf);
	}
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
	_buf.put((byte)56);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)56);
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

