using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildDungeonObj
{

/// <summary>
/// 公会副本实例数据
/// </summary>
public class GuildDungeon_InstanceInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long id;
/// <summary>
/// 公会副本ID
/// </summary>
private long dungeonId;
/// <summary>
/// 公会副本等级
/// </summary>
private int lvl;
/// <summary>
/// 开启时间
/// </summary>
private long startMs;
/// <summary>
/// 怪物列表
/// </summary>
private List<Common.GuildDungeonObj.GuildDungeon_Monster> monsterList;
/// <summary>
/// 已标记的怪物ID列表
/// </summary>
private List<long> tagMonsterIdLiist;


public GuildDungeon_InstanceInfo() {
	id = (long)0;
	dungeonId = (long)0;
	lvl = 0;
	startMs = (long)0;
	monsterList = new List<Common.GuildDungeonObj.GuildDungeon_Monster>();
	tagMonsterIdLiist = new List<long>();
}

public GuildDungeon_InstanceInfo(
	long _id
	, long _dungeonId
	, int _lvl
	, long _startMs
	, List<Common.GuildDungeonObj.GuildDungeon_Monster> _monsterList
	, List<long> _tagMonsterIdLiist
) {	id = _id;
	dungeonId = _dungeonId;
	lvl = _lvl;
	startMs = _startMs;
	monsterList = _monsterList;
	tagMonsterIdLiist = _tagMonsterIdLiist;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 公会副本ID
/// </summary>
public long getDungeonId() { return dungeonId; }
/// <summary>
/// 公会副本ID
/// </summary>
public void setDungeonId(long _dungeonId) { dungeonId = _dungeonId; }
/// <summary>
/// 公会副本等级
/// </summary>
public int getLvl() { return lvl; }
/// <summary>
/// 公会副本等级
/// </summary>
public void setLvl(int _lvl) { lvl = _lvl; }
/// <summary>
/// 开启时间
/// </summary>
public long getStartMs() { return startMs; }
/// <summary>
/// 开启时间
/// </summary>
public void setStartMs(long _startMs) { startMs = _startMs; }
/// <summary>
/// 怪物列表
/// </summary>
public List<Common.GuildDungeonObj.GuildDungeon_Monster> getMonsterList() { return monsterList; }
/// <summary>
/// 怪物列表
/// </summary>
public void addMonsterList(Common.GuildDungeonObj.GuildDungeon_Monster _monsterList) { monsterList.Add(_monsterList); }
/// <summary>
/// 已标记的怪物ID列表
/// </summary>
public List<long> getTagMonsterIdLiist() { return tagMonsterIdLiist; }
/// <summary>
/// 已标记的怪物ID列表
/// </summary>
public void addTagMonsterIdLiist(long _tagMonsterIdLiist) { tagMonsterIdLiist.Add(_tagMonsterIdLiist); }


public int GetBufSize() {
	int _size = 28;
	_size += 2 + (monsterList.Count * 21);
	_size += 2 + (tagMonsterIdLiist.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 30;
	_size += 2 + (monsterList.Count * 21);
	_size += 2 + (tagMonsterIdLiist.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dungeonId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	lvl = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	startMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _monsterListCount = _buf.getShort();
	for(int _i = 0; _i < _monsterListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_Monster _monsterList = new Common.GuildDungeonObj.GuildDungeon_Monster();
		int __monsterListCustLen = _buf.getInt();
	int __monsterListCurPos = _buf.getCurPos();
	_monsterList.ReadUnzipBuf(_buf, __monsterListCurPos + __monsterListCustLen);
	_buf.setPosition(__monsterListCurPos + __monsterListCustLen);

		monsterList.Add(_monsterList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _tagMonsterIdLiistCount = _buf.getShort();
	for(int _i = 0; _i < _tagMonsterIdLiistCount; _i++) { 
		long _tagMonsterIdLiist = (long)0;
		_tagMonsterIdLiist = _buf.getLong();
		tagMonsterIdLiist.Add(_tagMonsterIdLiist);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putLong(dungeonId);
	_buf.putInt(lvl);
	_buf.putLong(startMs);
	_buf.putShort((short)monsterList.Count);
	for(int _i = 0; _i < monsterList.Count; _i++) { 
		_buf.putInt(monsterList[_i].GetBufSize());
	monsterList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)tagMonsterIdLiist.Count);
	for(int _i = 0; _i < tagMonsterIdLiist.Count; _i++) { 
		_buf.putLong(tagMonsterIdLiist[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)0);
	_buf.put((byte)0);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)0);
	_recBuf.put((byte)0);
	PutUnzipBuf(_recBuf);
}
public byte[] makePackage() {
	int _bufSize = GetBufSize();
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void readPackage(byte[] _buf) {
	ALProtocolBuf _bufObj = new ALProtocolBuf(_buf);
	ReadUnzipBuf(_bufObj, -1);
}
public void readPackage(ALProtocolBuf _buf) {
	ReadUnzipBuf(_buf, -1);
}
public override string ToString() {
	System.Text.StringBuilder builder = new System.Text.StringBuilder();

	builder.Append("{");
	builder.Append("id").Append(":").Append(id.ToString()).Append(", ");
	builder.Append("dungeonId").Append(":").Append(dungeonId.ToString()).Append(", ");
	builder.Append("lvl").Append(":").Append(lvl.ToString()).Append(", ");
	builder.Append("startMs").Append(":").Append(startMs.ToString()).Append(", ");
	builder.Append("monsterList").Append(":").Append(monsterList.ToString()).Append(", ");
	builder.Append("tagMonsterIdLiist").Append(":").Append(tagMonsterIdLiist.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

