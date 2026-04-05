using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p017_ActivityOp
{

/// <summary>
/// 请求活动排行榜基础信息
/// </summary>
public class GC2GS_017_003_ReqActivityRankBaseList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 活动实例id
/// </summary>
private long instanceId;
/// <summary>
/// 排行榜id
/// </summary>
private long rankId;
private bool isCross;


public GC2GS_017_003_ReqActivityRankBaseList() {
	instanceId = (long)0;
	rankId = (long)0;
	isCross = false;
}

public GC2GS_017_003_ReqActivityRankBaseList(
	long _instanceId
	, long _rankId
	, bool _isCross
) {	instanceId = _instanceId;
	rankId = _rankId;
	isCross = _isCross;
}

public byte getMainOrder() { return (byte)17; }

public byte getSubOrder() { return (byte)3; }

/// <summary>
/// 活动实例id
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 活动实例id
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 排行榜id
/// </summary>
public long getRankId() { return rankId; }
/// <summary>
/// 排行榜id
/// </summary>
public void setRankId(long _rankId) { rankId = _rankId; }
public bool getIsCross() { return isCross; }
public void setIsCross(bool _isCross) { isCross = _isCross; }


public int GetBufSize() {
	int _size = 17;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 19;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rankId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isCross = (_buf.get() != 0);
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(rankId);
	_buf.put(isCross?(byte)1:(byte)0);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)17);
	_buf.put((byte)3);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)17);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("rankId").Append(":").Append(rankId.ToString()).Append(", ");
	builder.Append("isCross").Append(":").Append(isCross.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

