using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

public class GS2GC_002_088_RetLoverCollectInit : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 当前选中的情人配置ID，0=未选择
/// </summary>
private long targetLoverId;
/// <summary>
/// 是否已领取当前目标情人
/// </summary>
private bool isClaimed;


public GS2GC_002_088_RetLoverCollectInit() {
	targetLoverId = (long)0;
	isClaimed = false;
}

public GS2GC_002_088_RetLoverCollectInit(
	long _targetLoverId
	, bool _isClaimed
) {	targetLoverId = _targetLoverId;
	isClaimed = _isClaimed;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)88; }

/// <summary>
/// 当前选中的情人配置ID，0=未选择
/// </summary>
public long getTargetLoverId() { return targetLoverId; }
/// <summary>
/// 当前选中的情人配置ID，0=未选择
/// </summary>
public void setTargetLoverId(long _targetLoverId) { targetLoverId = _targetLoverId; }
/// <summary>
/// 是否已领取当前目标情人
/// </summary>
public bool getIsClaimed() { return isClaimed; }
/// <summary>
/// 是否已领取当前目标情人
/// </summary>
public void setIsClaimed(bool _isClaimed) { isClaimed = _isClaimed; }


public int GetBufSize() {
	int _size = 9;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 11;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	targetLoverId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isClaimed = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(targetLoverId);
	_buf.put(isClaimed?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)88);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)88);
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
	builder.Append("targetLoverId").Append(":").Append(targetLoverId.ToString()).Append(", ");
	builder.Append("isClaimed").Append(":").Append(isClaimed.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

