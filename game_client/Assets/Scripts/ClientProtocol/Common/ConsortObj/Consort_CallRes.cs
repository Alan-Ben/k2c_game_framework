using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ConsortObj
{

/// <summary>
/// 家人邀约结果
/// </summary>
public class Consort_CallRes : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 家人ID
/// </summary>
private long consortId;
/// <summary>
/// 家人剧情ID
/// </summary>
private long consortStoryId;
/// <summary>
/// 增加的加护点数
/// </summary>
private long addCharmPoint;
/// <summary>
/// 是否获取CG
/// </summary>
private bool isGainCg;
/// <summary>
/// 获得子嗣的实例ID，0-未获得
/// </summary>
private long childId;


public Consort_CallRes() {
	consortId = (long)0;
	consortStoryId = (long)0;
	addCharmPoint = (long)0;
	isGainCg = false;
	childId = (long)0;
}

public Consort_CallRes(
	long _consortId
	, long _consortStoryId
	, long _addCharmPoint
	, bool _isGainCg
	, long _childId
) {	consortId = _consortId;
	consortStoryId = _consortStoryId;
	addCharmPoint = _addCharmPoint;
	isGainCg = _isGainCg;
	childId = _childId;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 家人ID
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 家人ID
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 家人剧情ID
/// </summary>
public long getConsortStoryId() { return consortStoryId; }
/// <summary>
/// 家人剧情ID
/// </summary>
public void setConsortStoryId(long _consortStoryId) { consortStoryId = _consortStoryId; }
/// <summary>
/// 增加的加护点数
/// </summary>
public long getAddCharmPoint() { return addCharmPoint; }
/// <summary>
/// 增加的加护点数
/// </summary>
public void setAddCharmPoint(long _addCharmPoint) { addCharmPoint = _addCharmPoint; }
/// <summary>
/// 是否获取CG
/// </summary>
public bool getIsGainCg() { return isGainCg; }
/// <summary>
/// 是否获取CG
/// </summary>
public void setIsGainCg(bool _isGainCg) { isGainCg = _isGainCg; }
/// <summary>
/// 获得子嗣的实例ID，0-未获得
/// </summary>
public long getChildId() { return childId; }
/// <summary>
/// 获得子嗣的实例ID，0-未获得
/// </summary>
public void setChildId(long _childId) { childId = _childId; }


public int GetBufSize() {
	int _size = 33;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 35;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortStoryId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	addCharmPoint = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	isGainCg = (_buf.get() != 0);
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	childId = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putLong(consortStoryId);
	_buf.putLong(addCharmPoint);
	_buf.put(isGainCg?(byte)1:(byte)0);
	_buf.putLong(childId);
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
	builder.Append("consortId").Append(":").Append(consortId.ToString()).Append(", ");
	builder.Append("consortStoryId").Append(":").Append(consortStoryId.ToString()).Append(", ");
	builder.Append("addCharmPoint").Append(":").Append(addCharmPoint.ToString()).Append(", ");
	builder.Append("isGainCg").Append(":").Append(isGainCg.ToString()).Append(", ");
	builder.Append("childId").Append(":").Append(childId.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

