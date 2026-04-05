using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-转换矿石结果
/// </summary>
public class TreasureHunt_TransOreResult : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 矿石ID
/// </summary>
private long oreId;
/// <summary>
/// 是否普通矿石
/// </summary>
private bool isNormal;
/// <summary>
/// 转换数量
/// </summary>
private int num;


public TreasureHunt_TransOreResult() {
	oreId = (long)0;
	isNormal = false;
	num = 0;
}

public TreasureHunt_TransOreResult(
	long _oreId
	, bool _isNormal
	, int _num
) {	oreId = _oreId;
	isNormal = _isNormal;
	num = _num;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 矿石ID
/// </summary>
public long getOreId() { return oreId; }
/// <summary>
/// 矿石ID
/// </summary>
public void setOreId(long _oreId) { oreId = _oreId; }
/// <summary>
/// 是否普通矿石
/// </summary>
public bool getIsNormal() { return isNormal; }
/// <summary>
/// 是否普通矿石
/// </summary>
public void setIsNormal(bool _isNormal) { isNormal = _isNormal; }
/// <summary>
/// 转换数量
/// </summary>
public int getNum() { return num; }
/// <summary>
/// 转换数量
/// </summary>
public void setNum(int _num) { num = _num; }


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
	oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isNormal = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	num = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(oreId);
	_buf.put(isNormal?(byte)1:(byte)0);
	_buf.putInt(num);
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
	builder.Append("oreId").Append(":").Append(oreId.ToString()).Append(", ");
	builder.Append("isNormal").Append(":").Append(isNormal.ToString()).Append(", ");
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

