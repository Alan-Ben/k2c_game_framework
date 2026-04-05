using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_071_RetGuildDungeonInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 设置数据列表
/// </summary>
private List<Common.GuildDungeonObj.GuildDungeon_SetInfo> setList;
/// <summary>
/// 实力数据列表
/// </summary>
private List<Common.GuildDungeonObj.GuildDungeon_InstanceInfo> instanceList;
/// <summary>
/// 已领取奖励的怪物数据列表
/// </summary>
private List<Common.GuildDungeonObj.GuildDungeon_DungeonMonster> gainedRewardMonsterIdList;
/// <summary>
/// 出战大臣数据列表
/// </summary>
private List<Common.GuildDungeonObj.GuildDungeon_FightHero> fightHeroList;


public GS2GC_002_071_RetGuildDungeonInit() {
	setList = new List<Common.GuildDungeonObj.GuildDungeon_SetInfo>();
	instanceList = new List<Common.GuildDungeonObj.GuildDungeon_InstanceInfo>();
	gainedRewardMonsterIdList = new List<Common.GuildDungeonObj.GuildDungeon_DungeonMonster>();
	fightHeroList = new List<Common.GuildDungeonObj.GuildDungeon_FightHero>();
}

public GS2GC_002_071_RetGuildDungeonInit(
	List<Common.GuildDungeonObj.GuildDungeon_SetInfo> _setList
	, List<Common.GuildDungeonObj.GuildDungeon_InstanceInfo> _instanceList
	, List<Common.GuildDungeonObj.GuildDungeon_DungeonMonster> _gainedRewardMonsterIdList
	, List<Common.GuildDungeonObj.GuildDungeon_FightHero> _fightHeroList
) {	setList = _setList;
	instanceList = _instanceList;
	gainedRewardMonsterIdList = _gainedRewardMonsterIdList;
	fightHeroList = _fightHeroList;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)71; }

/// <summary>
/// 设置数据列表
/// </summary>
public List<Common.GuildDungeonObj.GuildDungeon_SetInfo> getSetList() { return setList; }
/// <summary>
/// 设置数据列表
/// </summary>
public void addSetList(Common.GuildDungeonObj.GuildDungeon_SetInfo _setList) { setList.Add(_setList); }
/// <summary>
/// 实力数据列表
/// </summary>
public List<Common.GuildDungeonObj.GuildDungeon_InstanceInfo> getInstanceList() { return instanceList; }
/// <summary>
/// 实力数据列表
/// </summary>
public void addInstanceList(Common.GuildDungeonObj.GuildDungeon_InstanceInfo _instanceList) { instanceList.Add(_instanceList); }
/// <summary>
/// 已领取奖励的怪物数据列表
/// </summary>
public List<Common.GuildDungeonObj.GuildDungeon_DungeonMonster> getGainedRewardMonsterIdList() { return gainedRewardMonsterIdList; }
/// <summary>
/// 已领取奖励的怪物数据列表
/// </summary>
public void addGainedRewardMonsterIdList(Common.GuildDungeonObj.GuildDungeon_DungeonMonster _gainedRewardMonsterIdList) { gainedRewardMonsterIdList.Add(_gainedRewardMonsterIdList); }
/// <summary>
/// 出战大臣数据列表
/// </summary>
public List<Common.GuildDungeonObj.GuildDungeon_FightHero> getFightHeroList() { return fightHeroList; }
/// <summary>
/// 出战大臣数据列表
/// </summary>
public void addFightHeroList(Common.GuildDungeonObj.GuildDungeon_FightHero _fightHeroList) { fightHeroList.Add(_fightHeroList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (setList.Count * 16);
	_size += 2;
for(int _i = 0; _i < instanceList.Count; _i++) {
	_size += 4 + instanceList[_i].GetBufSize();
	}

	_size += 2 + (gainedRewardMonsterIdList.Count * 20);
	_size += 2 + (fightHeroList.Count * 28);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (setList.Count * 16);
	_size += 2;
for(int _i = 0; _i < instanceList.Count; _i++) {
	_size += 4 + instanceList[_i].GetBufSize();
	}

	_size += 2 + (gainedRewardMonsterIdList.Count * 20);
	_size += 2 + (fightHeroList.Count * 28);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _setListCount = _buf.getShort();
	for(int _i = 0; _i < _setListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_SetInfo _setList = new Common.GuildDungeonObj.GuildDungeon_SetInfo();
		int __setListCustLen = _buf.getInt();
	int __setListCurPos = _buf.getCurPos();
	_setList.ReadUnzipBuf(_buf, __setListCurPos + __setListCustLen);
	_buf.setPosition(__setListCurPos + __setListCustLen);

		setList.Add(_setList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _instanceListCount = _buf.getShort();
	for(int _i = 0; _i < _instanceListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_InstanceInfo _instanceList = new Common.GuildDungeonObj.GuildDungeon_InstanceInfo();
		int __instanceListCustLen = _buf.getInt();
	int __instanceListCurPos = _buf.getCurPos();
	_instanceList.ReadUnzipBuf(_buf, __instanceListCurPos + __instanceListCustLen);
	_buf.setPosition(__instanceListCurPos + __instanceListCustLen);

		instanceList.Add(_instanceList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _gainedRewardMonsterIdListCount = _buf.getShort();
	for(int _i = 0; _i < _gainedRewardMonsterIdListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_DungeonMonster _gainedRewardMonsterIdList = new Common.GuildDungeonObj.GuildDungeon_DungeonMonster();
		int __gainedRewardMonsterIdListCustLen = _buf.getInt();
	int __gainedRewardMonsterIdListCurPos = _buf.getCurPos();
	_gainedRewardMonsterIdList.ReadUnzipBuf(_buf, __gainedRewardMonsterIdListCurPos + __gainedRewardMonsterIdListCustLen);
	_buf.setPosition(__gainedRewardMonsterIdListCurPos + __gainedRewardMonsterIdListCustLen);

		gainedRewardMonsterIdList.Add(_gainedRewardMonsterIdList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _fightHeroListCount = _buf.getShort();
	for(int _i = 0; _i < _fightHeroListCount; _i++) { 
		Common.GuildDungeonObj.GuildDungeon_FightHero _fightHeroList = new Common.GuildDungeonObj.GuildDungeon_FightHero();
		int __fightHeroListCustLen = _buf.getInt();
	int __fightHeroListCurPos = _buf.getCurPos();
	_fightHeroList.ReadUnzipBuf(_buf, __fightHeroListCurPos + __fightHeroListCustLen);
	_buf.setPosition(__fightHeroListCurPos + __fightHeroListCustLen);

		fightHeroList.Add(_fightHeroList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)setList.Count);
	for(int _i = 0; _i < setList.Count; _i++) { 
		_buf.putInt(setList[_i].GetBufSize());
	setList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)instanceList.Count);
	for(int _i = 0; _i < instanceList.Count; _i++) { 
		_buf.putInt(instanceList[_i].GetBufSize());
	instanceList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)gainedRewardMonsterIdList.Count);
	for(int _i = 0; _i < gainedRewardMonsterIdList.Count; _i++) { 
		_buf.putInt(gainedRewardMonsterIdList[_i].GetBufSize());
	gainedRewardMonsterIdList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)fightHeroList.Count);
	for(int _i = 0; _i < fightHeroList.Count; _i++) { 
		_buf.putInt(fightHeroList[_i].GetBufSize());
	fightHeroList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)71);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)71);
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
	builder.Append("setList").Append(":").Append(setList.ToString()).Append(", ");
	builder.Append("instanceList").Append(":").Append(instanceList.ToString()).Append(", ");
	builder.Append("gainedRewardMonsterIdList").Append(":").Append(gainedRewardMonsterIdList.ToString()).Append(", ");
	builder.Append("fightHeroList").Append(":").Append(fightHeroList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

