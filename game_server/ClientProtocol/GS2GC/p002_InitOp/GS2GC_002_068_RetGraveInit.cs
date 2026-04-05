using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_068_RetGraveInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 是否可领奖新晋杰出者
/// </summary>
private bool hasGraveNewReward;
/// <summary>
/// 是否有杰出者记录
/// </summary>
private bool hasGraveRecord;


public GS2GC_002_068_RetGraveInit() {
	hasGraveNewReward = false;
	hasGraveRecord = false;
}

public GS2GC_002_068_RetGraveInit(
	bool _hasGraveNewReward
	, bool _hasGraveRecord
) {	hasGraveNewReward = _hasGraveNewReward;
	hasGraveRecord = _hasGraveRecord;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)68; }

/// <summary>
/// 是否可领奖新晋杰出者
/// </summary>
public bool getHasGraveNewReward() { return hasGraveNewReward; }
/// <summary>
/// 是否可领奖新晋杰出者
/// </summary>
public void setHasGraveNewReward(bool _hasGraveNewReward) { hasGraveNewReward = _hasGraveNewReward; }
/// <summary>
/// 是否有杰出者记录
/// </summary>
public bool getHasGraveRecord() { return hasGraveRecord; }
/// <summary>
/// 是否有杰出者记录
/// </summary>
public void setHasGraveRecord(bool _hasGraveRecord) { hasGraveRecord = _hasGraveRecord; }


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
	hasGraveNewReward = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	hasGraveRecord = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.put(hasGraveNewReward?(byte)1:(byte)0);
	_buf.put(hasGraveRecord?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)68);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
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
	builder.Append("hasGraveNewReward").Append(":").Append(hasGraveNewReward.ToString()).Append(", ");
	builder.Append("hasGraveRecord").Append(":").Append(hasGraveRecord.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

