using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpPlayerInfoObj
{

/// <summary>
/// 玩家通用展示信息
/// </summary>
public class PlayerInfo_CommonShow : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 玩家id
/// </summary>
private long cid;
/// <summary>
/// 小头像组合信息
/// </summary>
private NPCommon.PlayerInfo_IconShow iconShow;
/// <summary>
/// 属性列表
/// </summary>
private Common.Common_LongList attrList;
/// <summary>
/// Q版形象id
/// </summary>
private long cuteActorId;
/// <summary>
/// 被点赞次数
/// </summary>
private long beLikeCount;
/// <summary>
/// 经验值
/// </summary>
private long exp;
/// <summary>
/// vip经验
/// </summary>
private long vipExp;
/// <summary>
/// 历史最大战力
/// </summary>
private long maxPower;


public PlayerInfo_CommonShow() {
	cid = (long)0;
	iconShow = new NPCommon.PlayerInfo_IconShow();
	attrList = new Common.Common_LongList();
	cuteActorId = (long)0;
	beLikeCount = (long)0;
	exp = (long)0;
	vipExp = (long)0;
	maxPower = (long)0;
}

public PlayerInfo_CommonShow(
	long _cid
	, NPCommon.PlayerInfo_IconShow _iconShow
	, Common.Common_LongList _attrList
	, long _cuteActorId
	, long _beLikeCount
	, long _exp
	, long _vipExp
	, long _maxPower
) {	cid = _cid;
	iconShow = _iconShow;
	attrList = _attrList;
	cuteActorId = _cuteActorId;
	beLikeCount = _beLikeCount;
	exp = _exp;
	vipExp = _vipExp;
	maxPower = _maxPower;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 玩家id
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 玩家id
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 小头像组合信息
/// </summary>
public NPCommon.PlayerInfo_IconShow getIconShow() { return iconShow; }
/// <summary>
/// 小头像组合信息
/// </summary>
public void setIconShow(NPCommon.PlayerInfo_IconShow _iconShow) { iconShow = _iconShow; }
/// <summary>
/// 属性列表
/// </summary>
public Common.Common_LongList getAttrList() { return attrList; }
/// <summary>
/// 属性列表
/// </summary>
public void setAttrList(Common.Common_LongList _attrList) { attrList = _attrList; }
/// <summary>
/// Q版形象id
/// </summary>
public long getCuteActorId() { return cuteActorId; }
/// <summary>
/// Q版形象id
/// </summary>
public void setCuteActorId(long _cuteActorId) { cuteActorId = _cuteActorId; }
/// <summary>
/// 被点赞次数
/// </summary>
public long getBeLikeCount() { return beLikeCount; }
/// <summary>
/// 被点赞次数
/// </summary>
public void setBeLikeCount(long _beLikeCount) { beLikeCount = _beLikeCount; }
/// <summary>
/// 经验值
/// </summary>
public long getExp() { return exp; }
/// <summary>
/// 经验值
/// </summary>
public void setExp(long _exp) { exp = _exp; }
/// <summary>
/// vip经验
/// </summary>
public long getVipExp() { return vipExp; }
/// <summary>
/// vip经验
/// </summary>
public void setVipExp(long _vipExp) { vipExp = _vipExp; }
/// <summary>
/// 历史最大战力
/// </summary>
public long getMaxPower() { return maxPower; }
/// <summary>
/// 历史最大战力
/// </summary>
public void setMaxPower(long _maxPower) { maxPower = _maxPower; }


public int GetBufSize() {
	int _size = 48;
	_size += 4 + iconShow.GetBufSize();
	_size += 4 + attrList.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 50;
	_size += 4 + iconShow.GetBufSize();
	_size += 4 + attrList.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _iconShowCustLen = _buf.getInt();
	int _iconShowCurPos = _buf.getCurPos();
	iconShow.ReadUnzipBuf(_buf, _iconShowCurPos + _iconShowCustLen);
	_buf.setPosition(_iconShowCurPos + _iconShowCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _attrListCustLen = _buf.getInt();
	int _attrListCurPos = _buf.getCurPos();
	attrList.ReadUnzipBuf(_buf, _attrListCurPos + _attrListCustLen);
	_buf.setPosition(_attrListCurPos + _attrListCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cuteActorId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	beLikeCount = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	exp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	vipExp = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	maxPower = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putInt(iconShow.GetBufSize());
	iconShow.PutUnzipBuf(_buf);
	_buf.putInt(attrList.GetBufSize());
	attrList.PutUnzipBuf(_buf);
	_buf.putLong(cuteActorId);
	_buf.putLong(beLikeCount);
	_buf.putLong(exp);
	_buf.putLong(vipExp);
	_buf.putLong(maxPower);
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
	builder.Append("cid").Append(":").Append(cid.ToString()).Append(", ");
	builder.Append("iconShow").Append(":").Append(iconShow == null ? "null" : iconShow.ToString()).Append(", ");
	builder.Append("attrList").Append(":").Append(attrList == null ? "null" : attrList.ToString()).Append(", ");
	builder.Append("cuteActorId").Append(":").Append(cuteActorId.ToString()).Append(", ");
	builder.Append("beLikeCount").Append(":").Append(beLikeCount.ToString()).Append(", ");
	builder.Append("exp").Append(":").Append(exp.ToString()).Append(", ");
	builder.Append("vipExp").Append(":").Append(vipExp.ToString()).Append(", ");
	builder.Append("maxPower").Append(":").Append(maxPower.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

