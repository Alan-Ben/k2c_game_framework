using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p037_GuildDungeonOp
{

/// <summary>
/// 已领取奖励的副本怪物数据列表
/// </summary>
public class GS2GC_037_053_OnDungeonGainedRewardChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 已领取奖励的怪物数据列表
/// </summary>
private List<Common.GuildDungeonObj.GuildDungeon_DungeonMonster> gainedRewardMonsterIdList;


public GS2GC_037_053_OnDungeonGainedRewardChg() {
	gainedRewardMonsterIdList = new List<Common.GuildDungeonObj.GuildDungeon_DungeonMonster>();
}

public GS2GC_037_053_OnDungeonGainedRewardChg(
	List<Common.GuildDungeonObj.GuildDungeon_DungeonMonster> _gainedRewardMonsterIdList
) {	gainedRewardMonsterIdList = _gainedRewardMonsterIdList;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)53; }

/// <summary>
/// 已领取奖励的怪物数据列表
/// </summary>
public List<Common.GuildDungeonObj.GuildDungeon_DungeonMonster> getGainedRewardMonsterIdList() { return gainedRewardMonsterIdList; }
/// <summary>
/// 已领取奖励的怪物数据列表
/// </summary>
public void addGainedRewardMonsterIdList(Common.GuildDungeonObj.GuildDungeon_DungeonMonster _gainedRewardMonsterIdList) { gainedRewardMonsterIdList.Add(_gainedRewardMonsterIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (gainedRewardMonsterIdList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (gainedRewardMonsterIdList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
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
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)gainedRewardMonsterIdList.Count);
	for(int _i = 0; _i < gainedRewardMonsterIdList.Count; _i++) { 
		_buf.putInt(gainedRewardMonsterIdList[_i].GetBufSize());
	gainedRewardMonsterIdList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)53);
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
	builder.Append("gainedRewardMonsterIdList").Append(":").Append(gainedRewardMonsterIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

