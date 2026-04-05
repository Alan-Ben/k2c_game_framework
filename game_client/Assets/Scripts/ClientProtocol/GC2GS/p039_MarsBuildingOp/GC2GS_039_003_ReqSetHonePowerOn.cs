using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p039_MarsBuildingOp
{

/// <summary>
/// 火星建筑-主建筑开关
/// </summary>
public class GC2GS_039_003_ReqSetHonePowerOn : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 普通功率开启
/// </summary>
private bool isNormalOn;
/// <summary>
/// 最高功率开启
/// </summary>
private bool isOverdriveOn;


public GC2GS_039_003_ReqSetHonePowerOn() {
	isNormalOn = false;
	isOverdriveOn = false;
}

public GC2GS_039_003_ReqSetHonePowerOn(
	bool _isNormalOn
	, bool _isOverdriveOn
) {	isNormalOn = _isNormalOn;
	isOverdriveOn = _isOverdriveOn;
}

public byte getMainOrder() { return (byte)39; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 普通功率开启
/// </summary>
public bool getIsNormalOn() { return isNormalOn; }
/// <summary>
/// 普通功率开启
/// </summary>
public void setIsNormalOn(bool _isNormalOn) { isNormalOn = _isNormalOn; }
/// <summary>
/// 最高功率开启
/// </summary>
public bool getIsOverdriveOn() { return isOverdriveOn; }
/// <summary>
/// 最高功率开启
/// </summary>
public void setIsOverdriveOn(bool _isOverdriveOn) { isOverdriveOn = _isOverdriveOn; }


public int GetBufSize() {
	int _size = 2;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 4;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isNormalOn = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isOverdriveOn = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(isNormalOn?(byte)1:(byte)0);
	_buf.put(isOverdriveOn?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)3);
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
	builder.Append("isNormalOn").Append(":").Append(isNormalOn.ToString()).Append(", ");
	builder.Append("isOverdriveOn").Append(":").Append(isOverdriveOn.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

