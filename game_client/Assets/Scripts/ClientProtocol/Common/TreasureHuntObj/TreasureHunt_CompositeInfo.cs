using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-组合信息
/// </summary>
public class TreasureHunt_CompositeInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 组合ID
/// </summary>
private long refId;
/// <summary>
/// 收集时间 ms
/// </summary>
private long collectTimeMs;
/// <summary>
/// 是否激活普通组合
/// </summary>
private bool isNormalActive;
/// <summary>
/// 是否激活高级组合
/// </summary>
private bool isAdvancedActive;


public TreasureHunt_CompositeInfo() {
	refId = (long)0;
	collectTimeMs = (long)0;
	isNormalActive = false;
	isAdvancedActive = false;
}

public TreasureHunt_CompositeInfo(
	long _refId
	, long _collectTimeMs
	, bool _isNormalActive
	, bool _isAdvancedActive
) {	refId = _refId;
	collectTimeMs = _collectTimeMs;
	isNormalActive = _isNormalActive;
	isAdvancedActive = _isAdvancedActive;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 组合ID
/// </summary>
public long getRefId() { return refId; }
/// <summary>
/// 组合ID
/// </summary>
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// 收集时间 ms
/// </summary>
public long getCollectTimeMs() { return collectTimeMs; }
/// <summary>
/// 收集时间 ms
/// </summary>
public void setCollectTimeMs(long _collectTimeMs) { collectTimeMs = _collectTimeMs; }
/// <summary>
/// 是否激活普通组合
/// </summary>
public bool getIsNormalActive() { return isNormalActive; }
/// <summary>
/// 是否激活普通组合
/// </summary>
public void setIsNormalActive(bool _isNormalActive) { isNormalActive = _isNormalActive; }
/// <summary>
/// 是否激活高级组合
/// </summary>
public bool getIsAdvancedActive() { return isAdvancedActive; }
/// <summary>
/// 是否激活高级组合
/// </summary>
public void setIsAdvancedActive(bool _isAdvancedActive) { isAdvancedActive = _isAdvancedActive; }


public int GetBufSize() {
	int _size = 18;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 20;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	collectTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isNormalActive = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isAdvancedActive = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(refId);
	_buf.putLong(collectTimeMs);
	_buf.put(isNormalActive?(byte)1:(byte)0);
	_buf.put(isAdvancedActive?(byte)1:(byte)0);
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
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("collectTimeMs").Append(":").Append(collectTimeMs.ToString()).Append(", ");
	builder.Append("isNormalActive").Append(":").Append(isNormalActive.ToString()).Append(", ");
	builder.Append("isAdvancedActive").Append(":").Append(isAdvancedActive.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

