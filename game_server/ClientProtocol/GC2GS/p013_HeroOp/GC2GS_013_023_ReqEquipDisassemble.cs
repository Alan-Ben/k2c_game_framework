using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p013_HeroOp
{

/// <summary>
/// 藏品分解
/// </summary>
public class GC2GS_013_023_ReqEquipDisassemble : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据id列表
/// </summary>
private List<long> dbList;


public GC2GS_013_023_ReqEquipDisassemble() {
	dbList = new List<long>();
}

public GC2GS_013_023_ReqEquipDisassemble(
	List<long> _dbList
) {	dbList = _dbList;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)23; }

/// <summary>
/// 数据id列表
/// </summary>
public List<long> getDbList() { return dbList; }
/// <summary>
/// 数据id列表
/// </summary>
public void addDbList(long _dbList) { dbList.Add(_dbList); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (dbList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (dbList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _dbListCount = _buf.getShort();
	for(int _i = 0; _i < _dbListCount; _i++) { 
		long _dbList = (long)0;
		_dbList = _buf.getLong();
		dbList.Add(_dbList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)dbList.Count);
	for(int _i = 0; _i < dbList.Count; _i++) { 
		_buf.putLong(dbList[_i]);
	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)23);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)23);
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
	builder.Append("dbList").Append(":").Append(dbList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

