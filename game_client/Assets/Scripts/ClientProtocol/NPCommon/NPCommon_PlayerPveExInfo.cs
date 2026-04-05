using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// PVE关卡其他信息结构体
/// </summary>
public class NPCommon_PlayerPveExInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 怪物血量列表
/// </summary>
private List<NPCommon.NPCommon_BattleMobHp> mobHpList;


public NPCommon_PlayerPveExInfo() {
	mobHpList = new List<NPCommon.NPCommon_BattleMobHp>();
}

public NPCommon_PlayerPveExInfo(
	List<NPCommon.NPCommon_BattleMobHp> _mobHpList
) {	mobHpList = _mobHpList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 怪物血量列表
/// </summary>
public List<NPCommon.NPCommon_BattleMobHp> getMobHpList() { return mobHpList; }
/// <summary>
/// 怪物血量列表
/// </summary>
public void addMobHpList(NPCommon.NPCommon_BattleMobHp _mobHpList) { mobHpList.Add(_mobHpList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (mobHpList.Count * 18);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (mobHpList.Count * 18);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _mobHpListCount = _buf.getShort();
	for(int _i = 0; _i < _mobHpListCount; _i++) { 
		NPCommon.NPCommon_BattleMobHp _mobHpList = new NPCommon.NPCommon_BattleMobHp();
		int __mobHpListCustLen = _buf.getInt();
	int __mobHpListCurPos = _buf.getCurPos();
	_mobHpList.ReadUnzipBuf(_buf, __mobHpListCurPos + __mobHpListCustLen);
	_buf.setPosition(__mobHpListCurPos + __mobHpListCustLen);

		mobHpList.Add(_mobHpList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)mobHpList.Count);
	for(int _i = 0; _i < mobHpList.Count; _i++) { 
		_buf.putInt(mobHpList[_i].GetBufSize());
	mobHpList[_i].PutUnzipBuf(_buf);
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
	builder.Append("mobHpList").Append(":").Append(mobHpList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

