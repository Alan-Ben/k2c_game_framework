using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p019_DinnerOp
{

/// <summary>
/// 参加宴会玩家推送
/// </summary>
public class GS2GC_019_053_OnJoinerAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 赴宴玩家CID
/// </summary>
private long cid;
/// <summary>
/// 赴宴配置ID
/// </summary>
private long costId;
/// <summary>
/// 宴会配置ID
/// </summary>
private long dinnerId;
/// <summary>
/// 赴宴玩家名称
/// </summary>
private string cname;


public GS2GC_019_053_OnJoinerAdd() {
	cid = (long)0;
	costId = (long)0;
	dinnerId = (long)0;
	cname = "";
}

public GS2GC_019_053_OnJoinerAdd(
	long _cid
	, long _costId
	, long _dinnerId
	, string _cname
) {	cid = _cid;
	costId = _costId;
	dinnerId = _dinnerId;
	cname = _cname;
}

public byte getMainOrder() { return (byte)19; }

public byte getSubOrder() { return (byte)53; }

/// <summary>
/// 赴宴玩家CID
/// </summary>
public long getCid() { return cid; }
/// <summary>
/// 赴宴玩家CID
/// </summary>
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 赴宴配置ID
/// </summary>
public long getCostId() { return costId; }
/// <summary>
/// 赴宴配置ID
/// </summary>
public void setCostId(long _costId) { costId = _costId; }
/// <summary>
/// 宴会配置ID
/// </summary>
public long getDinnerId() { return dinnerId; }
/// <summary>
/// 宴会配置ID
/// </summary>
public void setDinnerId(long _dinnerId) { dinnerId = _dinnerId; }
/// <summary>
/// 赴宴玩家名称
/// </summary>
public string getCname() { return cname; }
/// <summary>
/// 赴宴玩家名称
/// </summary>
public void setCname(string _cname) { cname = _cname; }


public int GetBufSize() {
	int _size = 24;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;
	_size += ALBasicProtocolPack.ALProtocolCommon.GetStringBufSize(cname);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	costId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	dinnerId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cname = _buf.getString();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putLong(costId);
	_buf.putLong(dinnerId);
	_buf.putString(cname);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)19);
	_buf.put((byte)53);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)19);
	_recBuf.put((byte)53);
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
	builder.Append("costId").Append(":").Append(costId.ToString()).Append(", ");
	builder.Append("dinnerId").Append(":").Append(dinnerId.ToString()).Append(", ");
	builder.Append("cname").Append(":").Append(cname.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

