using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ChildObj
{

/// <summary>
/// 联姻池待匹配子嗣（成年未婚）基础数据
/// </summary>
public class Adult_PoolBaseInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 发起请求的玩家CID
/// </summary>
private long applyCid;
/// <summary>
/// 发起请求的玩家子嗣ID
/// </summary>
private long applyAdultId;
/// <summary>
/// 子嗣收益
/// </summary>
private long bonus;
/// <summary>
/// 对方子嗣允许联姻的最低收益
/// </summary>
private long minBonus;


public Adult_PoolBaseInfo() {
	applyCid = (long)0;
	applyAdultId = (long)0;
	bonus = (long)0;
	minBonus = (long)0;
}

public Adult_PoolBaseInfo(
	long _applyCid
	, long _applyAdultId
	, long _bonus
	, long _minBonus
) {	applyCid = _applyCid;
	applyAdultId = _applyAdultId;
	bonus = _bonus;
	minBonus = _minBonus;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 发起请求的玩家CID
/// </summary>
public long getApplyCid() { return applyCid; }
/// <summary>
/// 发起请求的玩家CID
/// </summary>
public void setApplyCid(long _applyCid) { applyCid = _applyCid; }
/// <summary>
/// 发起请求的玩家子嗣ID
/// </summary>
public long getApplyAdultId() { return applyAdultId; }
/// <summary>
/// 发起请求的玩家子嗣ID
/// </summary>
public void setApplyAdultId(long _applyAdultId) { applyAdultId = _applyAdultId; }
/// <summary>
/// 子嗣收益
/// </summary>
public long getBonus() { return bonus; }
/// <summary>
/// 子嗣收益
/// </summary>
public void setBonus(long _bonus) { bonus = _bonus; }
/// <summary>
/// 对方子嗣允许联姻的最低收益
/// </summary>
public long getMinBonus() { return minBonus; }
/// <summary>
/// 对方子嗣允许联姻的最低收益
/// </summary>
public void setMinBonus(long _minBonus) { minBonus = _minBonus; }


public int GetBufSize() {
	int _size = 32;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 34;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	applyCid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	applyAdultId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	bonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	minBonus = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(applyCid);
	_buf.putLong(applyAdultId);
	_buf.putLong(bonus);
	_buf.putLong(minBonus);
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
	builder.Append("applyCid").Append(":").Append(applyCid.ToString()).Append(", ");
	builder.Append("applyAdultId").Append(":").Append(applyAdultId.ToString()).Append(", ");
	builder.Append("bonus").Append(":").Append(bonus.ToString()).Append(", ");
	builder.Append("minBonus").Append(":").Append(minBonus.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

