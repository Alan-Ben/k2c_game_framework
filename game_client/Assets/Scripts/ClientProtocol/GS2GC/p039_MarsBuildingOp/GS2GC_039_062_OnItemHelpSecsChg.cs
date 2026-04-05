using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p039_MarsBuildingOp
{

/// <summary>
/// 火星加速道具加速时长数值变化
/// </summary>
public class GS2GC_039_062_OnItemHelpSecsChg : ALBasicProtocolPack._IALProtocolStructure {
private Common.MarsEnum.EMarsBagItemUseTimeType objType;
/// <summary>
/// 对象ID
/// </summary>
private long objId;
/// <summary>
/// 加速时长（秒）
/// </summary>
private int secs;


public GS2GC_039_062_OnItemHelpSecsChg() {
	objType = 0;
	objId = (long)0;
	secs = 0;
}

public GS2GC_039_062_OnItemHelpSecsChg(
	Common.MarsEnum.EMarsBagItemUseTimeType _objType
	, long _objId
	, int _secs
) {	objType = _objType;
	objId = _objId;
	secs = _secs;
}

public byte getMainOrder() { return (byte)39; }

public byte getSubOrder() { return (byte)62; }

public Common.MarsEnum.EMarsBagItemUseTimeType getObjType() { return objType; }
public void setObjType(Common.MarsEnum.EMarsBagItemUseTimeType _objType) { objType = _objType; }
/// <summary>
/// 对象ID
/// </summary>
public long getObjId() { return objId; }
/// <summary>
/// 对象ID
/// </summary>
public void setObjId(long _objId) { objId = _objId; }
/// <summary>
/// 加速时长（秒）
/// </summary>
public int getSecs() { return secs; }
/// <summary>
/// 加速时长（秒）
/// </summary>
public void setSecs(int _secs) { secs = _secs; }


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
	objType = (Common.MarsEnum.EMarsBagItemUseTimeType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	objId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	secs = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)objType);

	_buf.putLong(objId);
	_buf.putInt(secs);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)39);
	_buf.put((byte)62);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)39);
	_recBuf.put((byte)62);
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
	builder.Append("objType").Append(":").Append(objType.ToString()).Append(", ");
	builder.Append("objId").Append(":").Append(objId.ToString()).Append(", ");
	builder.Append("secs").Append(":").Append(secs.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

