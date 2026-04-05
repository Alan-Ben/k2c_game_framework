using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TowerObj
{

/// <summary>
/// 爬塔_战报信息
/// </summary>
public class Tower_ReportInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 所在位置
/// </summary>
private Common.TowerObj.Tower_PosInfo posInfo;
/// <summary>
/// 攻击者Cid
/// </summary>
private long attackerCid;
/// <summary>
/// 攻击是否成功
/// </summary>
private bool isSucc;
/// <summary>
/// 下降层数
/// </summary>
private int downLevel;
/// <summary>
/// 时间戳 毫秒
/// </summary>
private long timestamp;


public Tower_ReportInfo() {
	posInfo = new Common.TowerObj.Tower_PosInfo();
	attackerCid = (long)0;
	isSucc = false;
	downLevel = 0;
	timestamp = (long)0;
}

public Tower_ReportInfo(
	Common.TowerObj.Tower_PosInfo _posInfo
	, long _attackerCid
	, bool _isSucc
	, int _downLevel
	, long _timestamp
) {	posInfo = _posInfo;
	attackerCid = _attackerCid;
	isSucc = _isSucc;
	downLevel = _downLevel;
	timestamp = _timestamp;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 所在位置
/// </summary>
public Common.TowerObj.Tower_PosInfo getPosInfo() { return posInfo; }
/// <summary>
/// 所在位置
/// </summary>
public void setPosInfo(Common.TowerObj.Tower_PosInfo _posInfo) { posInfo = _posInfo; }
/// <summary>
/// 攻击者Cid
/// </summary>
public long getAttackerCid() { return attackerCid; }
/// <summary>
/// 攻击者Cid
/// </summary>
public void setAttackerCid(long _attackerCid) { attackerCid = _attackerCid; }
/// <summary>
/// 攻击是否成功
/// </summary>
public bool getIsSucc() { return isSucc; }
/// <summary>
/// 攻击是否成功
/// </summary>
public void setIsSucc(bool _isSucc) { isSucc = _isSucc; }
/// <summary>
/// 下降层数
/// </summary>
public int getDownLevel() { return downLevel; }
/// <summary>
/// 下降层数
/// </summary>
public void setDownLevel(int _downLevel) { downLevel = _downLevel; }
/// <summary>
/// 时间戳 毫秒
/// </summary>
public long getTimestamp() { return timestamp; }
/// <summary>
/// 时间戳 毫秒
/// </summary>
public void setTimestamp(long _timestamp) { timestamp = _timestamp; }


public int GetBufSize() {
	int _size = 37;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 39;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _posInfoCustLen = _buf.getInt();
	int _posInfoCurPos = _buf.getCurPos();
	posInfo.ReadUnzipBuf(_buf, _posInfoCurPos + _posInfoCustLen);
	_buf.setPosition(_posInfoCurPos + _posInfoCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	attackerCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isSucc = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	downLevel = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	timestamp = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(posInfo.GetBufSize());
	posInfo.PutUnzipBuf(_buf);
	_buf.putLong(attackerCid);
	_buf.put(isSucc?(byte)1:(byte)0);
	_buf.putInt(downLevel);
	_buf.putLong(timestamp);
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
	builder.Append("posInfo").Append(":").Append(posInfo == null ? "null" : posInfo.ToString()).Append(", ");
	builder.Append("attackerCid").Append(":").Append(attackerCid.ToString()).Append(", ");
	builder.Append("isSucc").Append(":").Append(isSucc.ToString()).Append(", ");
	builder.Append("downLevel").Append(":").Append(downLevel.ToString()).Append(", ");
	builder.Append("timestamp").Append(":").Append(timestamp.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

