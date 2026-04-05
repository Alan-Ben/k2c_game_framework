using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人星辉数据变更
/// </summary>
public class GS2GC_015_060_OnHaloChg : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private long consortId;
/// <summary>
/// 家人星辉数据
/// </summary>
private Common.ConsortObj.Consort_Halo halo;


public GS2GC_015_060_OnHaloChg() {
	consortId = (long)0;
	halo = new Common.ConsortObj.Consort_Halo();
}

public GS2GC_015_060_OnHaloChg(
	long _consortId
	, Common.ConsortObj.Consort_Halo _halo
) {	consortId = _consortId;
	halo = _halo;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)60; }

/// <summary>
/// 空
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 空
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 家人星辉数据
/// </summary>
public Common.ConsortObj.Consort_Halo getHalo() { return halo; }
/// <summary>
/// 家人星辉数据
/// </summary>
public void setHalo(Common.ConsortObj.Consort_Halo _halo) { halo = _halo; }


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
	int _haloCustLen = _buf.getInt();
	int _haloCurPos = _buf.getCurPos();
	halo.ReadUnzipBuf(_buf, _haloCurPos + _haloCustLen);
	_buf.setPosition(_haloCurPos + _haloCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putInt(halo.GetBufSize());
	halo.PutUnzipBuf(_buf);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)60);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)60);
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
	builder.Append("halo").Append(":").Append(halo == null ? "null" : halo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

