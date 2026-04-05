using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p013_HeroOp
{

/// <summary>
/// 藏品技能重塑
/// </summary>
public class GC2GS_013_022_ReqEquipSkillRebuild : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 数据id
/// </summary>
private long dbId;
/// <summary>
/// 技能索引
/// </summary>
private int index;
/// <summary>
/// 是否是高级
/// </summary>
private bool isAdvance;


public GC2GS_013_022_ReqEquipSkillRebuild() {
	dbId = (long)0;
	index = 0;
	isAdvance = false;
}

public GC2GS_013_022_ReqEquipSkillRebuild(
	long _dbId
	, int _index
	, bool _isAdvance
) {	dbId = _dbId;
	index = _index;
	isAdvance = _isAdvance;
}

public byte getMainOrder() { return (byte)13; }

public byte getSubOrder() { return (byte)22; }

/// <summary>
/// 数据id
/// </summary>
public long getDbId() { return dbId; }
/// <summary>
/// 数据id
/// </summary>
public void setDbId(long _dbId) { dbId = _dbId; }
/// <summary>
/// 技能索引
/// </summary>
public int getIndex() { return index; }
/// <summary>
/// 技能索引
/// </summary>
public void setIndex(int _index) { index = _index; }
/// <summary>
/// 是否是高级
/// </summary>
public bool getIsAdvance() { return isAdvance; }
/// <summary>
/// 是否是高级
/// </summary>
public void setIsAdvance(bool _isAdvance) { isAdvance = _isAdvance; }


public int GetBufSize() {
	int _size = 13;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 15;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dbId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	index = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAdvance = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dbId);
	_buf.putInt(index);
	_buf.put(isAdvance?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)13);
	_buf.put((byte)22);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)13);
	_recBuf.put((byte)22);
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
	builder.Append("dbId").Append(":").Append(dbId.ToString()).Append(", ");
	builder.Append("index").Append(":").Append(index.ToString()).Append(", ");
	builder.Append("isAdvance").Append(":").Append(isAdvance.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

