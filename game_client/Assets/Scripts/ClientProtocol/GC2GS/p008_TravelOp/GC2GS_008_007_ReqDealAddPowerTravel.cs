using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GC2GS.p008_TravelOp
{

/// <summary>
/// 处理大臣加国力游历事件
/// </summary>
public class GC2GS_008_007_ReqDealAddPowerTravel : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 游历事件ID
/// </summary>
private long instanceId;
/// <summary>
/// 大臣ID
/// </summary>
private long heroId;


public GC2GS_008_007_ReqDealAddPowerTravel() {
	instanceId = (long)0;
	heroId = (long)0;
}

public GC2GS_008_007_ReqDealAddPowerTravel(
	long _instanceId
	, long _heroId
) {	instanceId = _instanceId;
	heroId = _heroId;
}

public byte getMainOrder() { return (byte)8; }

public byte getSubOrder() { return (byte)7; }

/// <summary>
/// 游历事件ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 游历事件ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 大臣ID
/// </summary>
public long getHeroId() { return heroId; }
/// <summary>
/// 大臣ID
/// </summary>
public void setHeroId(long _heroId) { heroId = _heroId; }


public int GetBufSize() {
	int _size = 16;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	heroId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(heroId);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)8);
	_buf.put((byte)7);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)8);
	_recBuf.put((byte)7);
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
	builder.Append("heroId").Append(":").Append(heroId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

