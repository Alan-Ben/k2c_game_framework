using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p037_GuildDungeonOp
{

/// <summary>
/// 公会副本-设置怪物标签
/// </summary>
public class GC2GS_037_012_ReqSetTagMonsterList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 公会实例ID
/// </summary>
private long id;
/// <summary>
/// 怪物ID列表
/// </summary>
private List<long> monsterIdList;


public GC2GS_037_012_ReqSetTagMonsterList() {
	id = (long)0;
	monsterIdList = new List<long>();
}

public GC2GS_037_012_ReqSetTagMonsterList(
	long _id
	, List<long> _monsterIdList
) {	id = _id;
	monsterIdList = _monsterIdList;
}

public byte getMainOrder() { return (byte)37; }

public byte getSubOrder() { return (byte)12; }

/// <summary>
/// 公会实例ID
/// </summary>
public long getId() { return id; }
/// <summary>
/// 公会实例ID
/// </summary>
public void setId(long _id) { id = _id; }
/// <summary>
/// 怪物ID列表
/// </summary>
public List<long> getMonsterIdList() { return monsterIdList; }
/// <summary>
/// 怪物ID列表
/// </summary>
public void addMonsterIdList(long _monsterIdList) { monsterIdList.Add(_monsterIdList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (monsterIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (monsterIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	id = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _monsterIdListCount = _buf.getShort();
	for(int _i = 0; _i < _monsterIdListCount; _i++) { 
		long _monsterIdList = (long)0;
		_monsterIdList = _buf.getLong();
		monsterIdList.Add(_monsterIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(id);
	_buf.putShort((short)monsterIdList.Count);
	for(int _i = 0; _i < monsterIdList.Count; _i++) { 
		_buf.putLong(monsterIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)37);
	_buf.put((byte)12);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)37);
	_recBuf.put((byte)12);
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
	builder.Append("monsterIdList").Append(":").Append(monsterIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

