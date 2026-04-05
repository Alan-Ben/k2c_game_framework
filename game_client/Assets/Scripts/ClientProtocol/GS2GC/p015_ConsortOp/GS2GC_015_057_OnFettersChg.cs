using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人羁绊数据变更
/// </summary>
public class GS2GC_015_057_OnFettersChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private long consortId;
/// <summary>
/// 家人羁绊数据
/// </summary>
private Common.ConsortObj.Consort_Fetters fetters;


public GS2GC_015_057_OnFettersChg() {
	consortId = (long)0;
	fetters = new Common.ConsortObj.Consort_Fetters();
}

public GS2GC_015_057_OnFettersChg(
	long _consortId
	, Common.ConsortObj.Consort_Fetters _fetters
) {	consortId = _consortId;
	fetters = _fetters;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)57; }

/// <summary>
/// 空
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 空
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 家人羁绊数据
/// </summary>
public Common.ConsortObj.Consort_Fetters getFetters() { return fetters; }
/// <summary>
/// 家人羁绊数据
/// </summary>
public void setFetters(Common.ConsortObj.Consort_Fetters _fetters) { fetters = _fetters; }


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
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _fettersCustLen = _buf.getInt();
	int _fettersCurPos = _buf.getCurPos();
	fetters.ReadUnzipBuf(_buf, _fettersCurPos + _fettersCustLen);
	_buf.setPosition(_fettersCurPos + _fettersCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putInt(fetters.GetBufSize());
	fetters.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)57);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)57);
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
	builder.Append("fetters").Append(":").Append(fetters == null ? "null" : fetters.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

