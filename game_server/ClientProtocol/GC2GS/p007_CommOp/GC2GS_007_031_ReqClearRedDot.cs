using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p007_CommOp
{

/// <summary>
/// 红点-清除红点请求
/// </summary>
public class GC2GS_007_031_ReqClearRedDot : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 要清除的红点类型列表
/// </summary>
private List<CommonEnum.ERedDotType> redDotTypes;


public GC2GS_007_031_ReqClearRedDot() {
	redDotTypes = new List<CommonEnum.ERedDotType>();
}

public GC2GS_007_031_ReqClearRedDot(
	List<CommonEnum.ERedDotType> _redDotTypes
) {	redDotTypes = _redDotTypes;
}

public byte getMainOrder() { return (byte)7; }

public byte getSubOrder() { return (byte)31; }

/// <summary>
/// 要清除的红点类型列表
/// </summary>
public List<CommonEnum.ERedDotType> getRedDotTypes() { return redDotTypes; }
/// <summary>
/// 要清除的红点类型列表
/// </summary>
public void addRedDotTypes(CommonEnum.ERedDotType _redDotTypes) { redDotTypes.Add(_redDotTypes); }


public int GetBufSize() {
	int _size = 0;
	_size += 2 + (redDotTypes.Count * 4);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 2;
	_size += 2 + (redDotTypes.Count * 4);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _redDotTypesCount = _buf.getShort();
	for(int _i = 0; _i < _redDotTypesCount; _i++) { 
		CommonEnum.ERedDotType _redDotTypes = 0;
		_redDotTypes = (CommonEnum.ERedDotType)_buf.getInt();
		redDotTypes.Add(_redDotTypes);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)redDotTypes.Count);
	for(int _i = 0; _i < redDotTypes.Count; _i++) { 
		_buf.putInt((int)redDotTypes[_i]);

	}
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)7);
	_buf.put((byte)31);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)7);
	_recBuf.put((byte)31);
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
	builder.Append("redDotTypes").Append(":").Append(redDotTypes.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

