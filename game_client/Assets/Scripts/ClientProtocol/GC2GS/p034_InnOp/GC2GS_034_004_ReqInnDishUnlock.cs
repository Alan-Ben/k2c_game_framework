using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p034_InnOp
{

/// <summary>
/// 旅店菜品解锁
/// </summary>
public class GC2GS_034_004_ReqInnDishUnlock : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 菜品ID
/// </summary>
private long dishId;


public GC2GS_034_004_ReqInnDishUnlock() {
	dishId = (long)0;
}

public GC2GS_034_004_ReqInnDishUnlock(
	long _dishId
) {	dishId = _dishId;
}

public byte getMainOrder() { return (byte)34; }

public byte getSubOrder() { return (byte)4; }

/// <summary>
/// 菜品ID
/// </summary>
public long getDishId() { return dishId; }
/// <summary>
/// 菜品ID
/// </summary>
public void setDishId(long _dishId) { dishId = _dishId; }


public int GetBufSize() {
	int _size = 8;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dishId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(dishId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)34);
	_buf.put((byte)4);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)34);
	_recBuf.put((byte)4);
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
	builder.Append("dishId").Append(":").Append(dishId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

