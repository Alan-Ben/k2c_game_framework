using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p032_GuildOp
{

/// <summary>
/// 返回联盟协作攻击日志列表
/// </summary>
public class GS2GC_032_041_RetGuildCooperateAttackLogList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 攻击日志列表
/// </summary>
private List<Common.GuildCooperateObj.GuildCooperate_AttackLog> attackLogList;


public GS2GC_032_041_RetGuildCooperateAttackLogList() {
	attackLogList = new List<Common.GuildCooperateObj.GuildCooperate_AttackLog>();
}

public GS2GC_032_041_RetGuildCooperateAttackLogList(
	List<Common.GuildCooperateObj.GuildCooperate_AttackLog> _attackLogList
) {	attackLogList = _attackLogList;
}

public byte getMainOrder() { return (byte)32; }

public byte getSubOrder() { return (byte)41; }

/// <summary>
/// 攻击日志列表
/// </summary>
public List<Common.GuildCooperateObj.GuildCooperate_AttackLog> getAttackLogList() { return attackLogList; }
/// <summary>
/// 攻击日志列表
/// </summary>
public void addAttackLogList(Common.GuildCooperateObj.GuildCooperate_AttackLog _attackLogList) { attackLogList.Add(_attackLogList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2;
for(int _i = 0; _i < attackLogList.Count; _i++) {
	_size += 4 + attackLogList[_i].GetBufSize();
	}


	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2;
for(int _i = 0; _i < attackLogList.Count; _i++) {
	_size += 4 + attackLogList[_i].GetBufSize();
	}


	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _attackLogListCount = _buf.getShort();
	for(int _i = 0; _i < _attackLogListCount; _i++) { 
		Common.GuildCooperateObj.GuildCooperate_AttackLog _attackLogList = new Common.GuildCooperateObj.GuildCooperate_AttackLog();
		int __attackLogListCustLen = _buf.getInt();
	int __attackLogListCurPos = _buf.getCurPos();
	_attackLogList.ReadUnzipBuf(_buf, __attackLogListCurPos + __attackLogListCustLen);
	_buf.setPosition(__attackLogListCurPos + __attackLogListCustLen);

		attackLogList.Add(_attackLogList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)attackLogList.Count);
	for(int _i = 0; _i < attackLogList.Count; _i++) { 
		_buf.putInt(attackLogList[_i].GetBufSize());
	attackLogList[_i].PutUnzipBuf(_buf);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)32);
	_buf.put((byte)41);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)32);
	_recBuf.put((byte)41);
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
	builder.Append("attackLogList").Append(":").Append(attackLogList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

