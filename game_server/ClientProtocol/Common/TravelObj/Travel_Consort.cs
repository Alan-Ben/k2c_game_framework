using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.TravelObj
{

/// <summary>
/// 游历妃子数据
/// </summary>
public class Travel_Consort : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 妃子ID
/// </summary>
private long consortId;
/// <summary>
/// 好感度
/// </summary>
private int like;


public Travel_Consort() {
	consortId = (long)0;
	like = 0;
}

public Travel_Consort(
	long _consortId
	, int _like
) {	consortId = _consortId;
	like = _like;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 妃子ID
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 妃子ID
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 好感度
/// </summary>
public int getLike() { return like; }
/// <summary>
/// 好感度
/// </summary>
public void setLike(int _like) { like = _like; }


public int GetBufSize() {
	int _size = 12;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 14;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	like = _buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putInt(like);
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
	builder.Append("like").Append(":").Append(like.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

