using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p007_CommOp
{

/// <summary>
/// 红点-检查红点状态请求
/// </summary>
public class GC2GS_007_032_ReqCheckRedDot : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 要检查的红点类型
/// </summary>
private CommonEnum.ERedDotType redDotType;


public GC2GS_007_032_ReqCheckRedDot() {
	redDotType = 0;
}

public GC2GS_007_032_ReqCheckRedDot(
	CommonEnum.ERedDotType _redDotType
) {	redDotType = _redDotType;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)32; }

/// <summary>
/// 要检查的红点类型
/// </summary>
public CommonEnum.ERedDotType getRedDotType() { return redDotType; }
/// <summary>
/// 要检查的红点类型
/// </summary>
public void setRedDotType(CommonEnum.ERedDotType _redDotType) { redDotType = _redDotType; }


public int GetBufSize() {
	int _size = 4;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	redDotType = (CommonEnum.ERedDotType)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)redDotType);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)32);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)32);
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
	builder.Append("redDotType").Append(":").Append(redDotType.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

