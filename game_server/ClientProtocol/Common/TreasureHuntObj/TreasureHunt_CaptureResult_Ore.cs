using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TreasureHuntObj
{

/// <summary>
/// 太空寻宝-捕捉结果_矿石
/// </summary>
public class TreasureHunt_CaptureResult_Ore : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 矿石ID
/// </summary>
private long oreId;
/// <summary>
/// 重量
/// </summary>
private int weight;
/// <summary>
/// 是否首次捕捉
/// </summary>
private bool isFirstCapture;
/// <summary>
/// 最大矿石归属者cid
/// </summary>
private long serverMaxCid;
/// <summary>
/// 最大矿石重量
/// </summary>
private int serverMaxWeight;
/// <summary>
/// 是否首次捕捉高级
/// </summary>
private bool isFirstDrawAdvanced;


public TreasureHunt_CaptureResult_Ore() {
	oreId = (long)0;
	weight = 0;
	isFirstCapture = false;
	serverMaxCid = (long)0;
	serverMaxWeight = 0;
	isFirstDrawAdvanced = false;
}

public TreasureHunt_CaptureResult_Ore(
	long _oreId
	, int _weight
	, bool _isFirstCapture
	, long _serverMaxCid
	, int _serverMaxWeight
	, bool _isFirstDrawAdvanced
) {	oreId = _oreId;
	weight = _weight;
	isFirstCapture = _isFirstCapture;
	serverMaxCid = _serverMaxCid;
	serverMaxWeight = _serverMaxWeight;
	isFirstDrawAdvanced = _isFirstDrawAdvanced;
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
/// 重量
/// </summary>
public int getWeight() { return weight; }
/// <summary>
/// 重量
/// </summary>
public void setWeight(int _weight) { weight = _weight; }
/// <summary>
/// 是否首次捕捉
/// </summary>
public bool getIsFirstCapture() { return isFirstCapture; }
/// <summary>
/// 是否首次捕捉
/// </summary>
public void setIsFirstCapture(bool _isFirstCapture) { isFirstCapture = _isFirstCapture; }
/// <summary>
/// 最大矿石归属者cid
/// </summary>
public long getServerMaxCid() { return serverMaxCid; }
/// <summary>
/// 最大矿石归属者cid
/// </summary>
public void setServerMaxCid(long _serverMaxCid) { serverMaxCid = _serverMaxCid; }
/// <summary>
/// 最大矿石重量
/// </summary>
public int getServerMaxWeight() { return serverMaxWeight; }
/// <summary>
/// 最大矿石重量
/// </summary>
public void setServerMaxWeight(int _serverMaxWeight) { serverMaxWeight = _serverMaxWeight; }
/// <summary>
/// 是否首次捕捉高级
/// </summary>
public bool getIsFirstDrawAdvanced() { return isFirstDrawAdvanced; }
/// <summary>
/// 是否首次捕捉高级
/// </summary>
public void setIsFirstDrawAdvanced(bool _isFirstDrawAdvanced) { isFirstDrawAdvanced = _isFirstDrawAdvanced; }


public int GetBufSize() {
	int _size = 26;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 28;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	oreId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	weight = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isFirstCapture = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serverMaxCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	serverMaxWeight = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isFirstDrawAdvanced = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(oreId);
	_buf.putInt(weight);
	_buf.put(isFirstCapture?(byte)1:(byte)0);
	_buf.putLong(serverMaxCid);
	_buf.putInt(serverMaxWeight);
	_buf.put(isFirstDrawAdvanced?(byte)1:(byte)0);
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
	builder.Append("weight").Append(":").Append(weight.ToString()).Append(", ");
	builder.Append("isFirstCapture").Append(":").Append(isFirstCapture.ToString()).Append(", ");
	builder.Append("serverMaxCid").Append(":").Append(serverMaxCid.ToString()).Append(", ");
	builder.Append("serverMaxWeight").Append(":").Append(serverMaxWeight.ToString()).Append(", ");
	builder.Append("isFirstDrawAdvanced").Append(":").Append(isFirstDrawAdvanced.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

