using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 玩家战斗初始化数据
/// </summary>
public class NPCommon_PlayerFightInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 阵容位置和血量列表
/// </summary>
private NPCommon.NPCommon_LineupSettingInfo lineupList;
/// <summary>
/// 卡牌属性列表
/// </summary>
private List<NPCommon.NPCommon_BattleCardInfo> petList;


public NPCommon_PlayerFightInfo() {
	lineupList = new NPCommon.NPCommon_LineupSettingInfo();
	petList = new List<NPCommon.NPCommon_BattleCardInfo>();
}

public NPCommon_PlayerFightInfo(
	NPCommon.NPCommon_LineupSettingInfo _lineupList
	, List<NPCommon.NPCommon_BattleCardInfo> _petList
) {	lineupList = _lineupList;
	petList = _petList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 阵容位置和血量列表
/// </summary>
public NPCommon.NPCommon_LineupSettingInfo getLineupList() { return lineupList; }
/// <summary>
/// 阵容位置和血量列表
/// </summary>
public void setLineupList(NPCommon.NPCommon_LineupSettingInfo _lineupList) { lineupList = _lineupList; }
/// <summary>
/// 卡牌属性列表
/// </summary>
public List<NPCommon.NPCommon_BattleCardInfo> getPetList() { return petList; }
/// <summary>
/// 卡牌属性列表
/// </summary>
public void addPetList(NPCommon.NPCommon_BattleCardInfo _petList) { petList.Add(_petList); }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + lineupList.GetBufSize();
	_size += 2;
for(int _i = 0; _i < petList.Count; _i++) {
	_size += 4 + petList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + lineupList.GetBufSize();
	_size += 2;
for(int _i = 0; _i < petList.Count; _i++) {
	_size += 4 + petList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _lineupListCustLen = _buf.getInt();
	int _lineupListCurPos = _buf.getCurPos();
	lineupList.ReadUnzipBuf(_buf, _lineupListCurPos + _lineupListCustLen);
	_buf.setPosition(_lineupListCurPos + _lineupListCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _petListCount = _buf.getShort();
	for(int _i = 0; _i < _petListCount; _i++) { 
		NPCommon.NPCommon_BattleCardInfo _petList = new NPCommon.NPCommon_BattleCardInfo();
		int __petListCustLen = _buf.getInt();
	int __petListCurPos = _buf.getCurPos();
	_petList.ReadUnzipBuf(_buf, __petListCurPos + __petListCustLen);
	_buf.setPosition(__petListCurPos + __petListCustLen);

		petList.Add(_petList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(lineupList.GetBufSize());
	lineupList.PutUnzipBuf(_buf);
	_buf.putShort((short)petList.Count);
	for(int _i = 0; _i < petList.Count; _i++) { 
		_buf.putInt(petList[_i].GetBufSize());
	petList[_i].PutUnzipBuf(_buf);
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
	builder.Append("lineupList").Append(":").Append(lineupList == null ? "null" : lineupList.ToString()).Append(", ");
	builder.Append("petList").Append(":").Append(petList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

