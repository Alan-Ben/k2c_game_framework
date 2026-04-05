using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace NPCommon
{

/// <summary>
/// 设置阵型时客户端发给服务端的数据
/// </summary>
public class NPCommon_layoutInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 宠物实例id
/// </summary>
private long petInstanceId;
/// <summary>
/// 阵型里的坐标
/// </summary>
private long layoutIndex;


public NPCommon_layoutInfo() {
	petInstanceId = (long)0;
	layoutIndex = (long)0;
}

public NPCommon_layoutInfo(
	long _petInstanceId
	, long _layoutIndex
) {	petInstanceId = _petInstanceId;
	layoutIndex = _layoutIndex;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 宠物实例id
/// </summary>
public long getPetInstanceId() { return petInstanceId; }
/// <summary>
/// 宠物实例id
/// </summary>
public void setPetInstanceId(long _petInstanceId) { petInstanceId = _petInstanceId; }
/// <summary>
/// 阵型里的坐标
/// </summary>
public long getLayoutIndex() { return layoutIndex; }
/// <summary>
/// 阵型里的坐标
/// </summary>
public void setLayoutIndex(long _layoutIndex) { layoutIndex = _layoutIndex; }


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
	petInstanceId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	layoutIndex = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(petInstanceId);
	_buf.putLong(layoutIndex);
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
	builder.Append("petInstanceId").Append(":").Append(petInstanceId.ToString()).Append(", ");
	builder.Append("layoutIndex").Append(":").Append(layoutIndex.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

