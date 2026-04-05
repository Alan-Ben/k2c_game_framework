using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p014_ChildOp
{

/// <summary>
/// 获取联姻池待匹配子嗣数据
/// </summary>
public class GS2GC_014_018_RetGetPoolAdult : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 匹配池子嗣数据
/// </summary>
private Common.ChildObj.Adult_PoolInfo poolAdult;


public GS2GC_014_018_RetGetPoolAdult() {
	poolAdult = new Common.ChildObj.Adult_PoolInfo();
}

public GS2GC_014_018_RetGetPoolAdult(
	Common.ChildObj.Adult_PoolInfo _poolAdult
) {	poolAdult = _poolAdult;
}

public byte getMainOrder() { return (byte)14; }

public byte getSubOrder() { return (byte)18; }

/// <summary>
/// 匹配池子嗣数据
/// </summary>
public Common.ChildObj.Adult_PoolInfo getPoolAdult() { return poolAdult; }
/// <summary>
/// 匹配池子嗣数据
/// </summary>
public void setPoolAdult(Common.ChildObj.Adult_PoolInfo _poolAdult) { poolAdult = _poolAdult; }


public int GetBufSize() {
	int _size = 0;
	_size += 4 + poolAdult.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 4 + poolAdult.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _poolAdultCustLen = _buf.getInt();
	int _poolAdultCurPos = _buf.getCurPos();
	poolAdult.ReadUnzipBuf(_buf, _poolAdultCurPos + _poolAdultCustLen);
	_buf.setPosition(_poolAdultCurPos + _poolAdultCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(poolAdult.GetBufSize());
	poolAdult.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)14);
	_buf.put((byte)18);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)14);
	_recBuf.put((byte)18);
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
	builder.Append("poolAdult").Append(":").Append(poolAdult == null ? "null" : poolAdult.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

