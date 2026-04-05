using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p013_HeroOp
{

/// <summary>
/// 藏品移除
/// </summary>
public class GS2GC_013_068_OnEquipRemove : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 藏品数据id列表
/// </summary>
private List<long> dbIdList;


public GS2GC_013_068_OnEquipRemove() {
	dbIdList = new List<long>();
}

public GS2GC_013_068_OnEquipRemove(
	List<long> _dbIdList
) {	dbIdList = _dbIdList;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)68; }

/// <summary>
/// 藏品数据id列表
/// </summary>
public List<long> getDbIdList() { return dbIdList; }
/// <summary>
/// 藏品数据id列表
/// </summary>
public void addDbIdList(long _dbIdList) { dbIdList.Add(_dbIdList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (dbIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (dbIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _dbIdListCount = _buf.getShort();
	for(int _i = 0; _i < _dbIdListCount; _i++) { 
		long _dbIdList = (long)0;
		_dbIdList = _buf.getLong();
		dbIdList.Add(_dbIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)dbIdList.Count);
	for(int _i = 0; _i < dbIdList.Count; _i++) { 
		_buf.putLong(dbIdList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)68);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)68);
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
	builder.Append("dbIdList").Append(":").Append(dbIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

