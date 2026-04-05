using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人-指定邀约
/// </summary>
public class GS2GC_015_006_RetCallAppoint : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 增加的加护点数
/// </summary>
private long addCharmPoint;
/// <summary>
/// 获得子嗣的实例ID，0-未获得
/// </summary>
private long childId;


public GS2GC_015_006_RetCallAppoint() {
	addCharmPoint = (long)0;
	childId = (long)0;
}

public GS2GC_015_006_RetCallAppoint(
	long _addCharmPoint
	, long _childId
) {	addCharmPoint = _addCharmPoint;
	childId = _childId;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)6; }

/// <summary>
/// 增加的加护点数
/// </summary>
public long getAddCharmPoint() { return addCharmPoint; }
/// <summary>
/// 增加的加护点数
/// </summary>
public void setAddCharmPoint(long _addCharmPoint) { addCharmPoint = _addCharmPoint; }
/// <summary>
/// 获得子嗣的实例ID，0-未获得
/// </summary>
public long getChildId() { return childId; }
/// <summary>
/// 获得子嗣的实例ID，0-未获得
/// </summary>
public void setChildId(long _childId) { childId = _childId; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addCharmPoint = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	childId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(addCharmPoint);
	_buf.putLong(childId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)6);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)6);
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
	builder.Append("addCharmPoint").Append(":").Append(addCharmPoint.ToString()).Append(", ");
	builder.Append("childId").Append(":").Append(childId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

