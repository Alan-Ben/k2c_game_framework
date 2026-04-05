using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.GuildObj
{

/// <summary>
/// 联盟成员贡献信息
/// </summary>
public class Guild_MemberContributeInfo : ALBasicProtocolPack._IALProtocolStructure {
private long cid;
/// <summary>
/// 7日贡献度
/// </summary>
private long sevenDaysContribute;
/// <summary>
/// 总贡献度
/// </summary>
private long totalContribute;


public Guild_MemberContributeInfo() {
	cid = (long)0;
	sevenDaysContribute = (long)0;
	totalContribute = (long)0;
}

public Guild_MemberContributeInfo(
	long _cid
	, long _sevenDaysContribute
	, long _totalContribute
) {	cid = _cid;
	sevenDaysContribute = _sevenDaysContribute;
	totalContribute = _totalContribute;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public long getCid() { return cid; }
public void setCid(long _cid) { cid = _cid; }
/// <summary>
/// 7日贡献度
/// </summary>
public long getSevenDaysContribute() { return sevenDaysContribute; }
/// <summary>
/// 7日贡献度
/// </summary>
public void setSevenDaysContribute(long _sevenDaysContribute) { sevenDaysContribute = _sevenDaysContribute; }
/// <summary>
/// 总贡献度
/// </summary>
public long getTotalContribute() { return totalContribute; }
/// <summary>
/// 总贡献度
/// </summary>
public void setTotalContribute(long _totalContribute) { totalContribute = _totalContribute; }


public int GetBufSize() {
	int _size = 24;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 26;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	cid = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sevenDaysContribute = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	totalContribute = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(cid);
	_buf.putLong(sevenDaysContribute);
	_buf.putLong(totalContribute);
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
	builder.Append("sevenDaysContribute").Append(":").Append(sevenDaysContribute.ToString()).Append(", ");
	builder.Append("totalContribute").Append(":").Append(totalContribute.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

