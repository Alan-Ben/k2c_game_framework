using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.HeroRecommendObj
{

/// <summary>
/// 大臣推荐事件数据
/// </summary>
public class HeroRecommend_Info : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 实例ID
/// </summary>
private long instanceId;
/// <summary>
/// 配置ID
/// </summary>
private long refId;
/// <summary>
/// 随机种子
/// </summary>
private long rndSeed;


public HeroRecommend_Info() {
	instanceId = (long)0;
	refId = (long)0;
	rndSeed = (long)0;
}

public HeroRecommend_Info(
	long _instanceId
	, long _refId
	, long _rndSeed
) {	instanceId = _instanceId;
	refId = _refId;
	rndSeed = _rndSeed;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 实例ID
/// </summary>
public long getInstanceId() { return instanceId; }
/// <summary>
/// 实例ID
/// </summary>
public void setInstanceId(long _instanceId) { instanceId = _instanceId; }
/// <summary>
/// 配置ID
/// </summary>
public long getRefId() { return refId; }
/// <summary>
/// 配置ID
/// </summary>
public void setRefId(long _refId) { refId = _refId; }
/// <summary>
/// 随机种子
/// </summary>
public long getRndSeed() { return rndSeed; }
/// <summary>
/// 随机种子
/// </summary>
public void setRndSeed(long _rndSeed) { rndSeed = _rndSeed; }


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
	instanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	refId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	rndSeed = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(instanceId);
	_buf.putLong(refId);
	_buf.putLong(rndSeed);
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
	builder.Append("instanceId").Append(":").Append(instanceId.ToString()).Append(", ");
	builder.Append("refId").Append(":").Append(refId.ToString()).Append(", ");
	builder.Append("rndSeed").Append(":").Append(rndSeed.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

