using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p015_ConsortOp
{

/// <summary>
/// 家人新增推送
/// </summary>
public class GS2GC_015_050_OnConsortAdd : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 空
/// </summary>
private Common.ConsortObj.Consort_Info consort;
/// <summary>
/// 来源类型
/// </summary>
private Common.ConsortEnum.EConsortSourceType sourceType;


public GS2GC_015_050_OnConsortAdd() {
	consort = new Common.ConsortObj.Consort_Info();
	sourceType = 0;
}

public GS2GC_015_050_OnConsortAdd(
	Common.ConsortObj.Consort_Info _consort
	, Common.ConsortEnum.EConsortSourceType _sourceType
) {	consort = _consort;
	sourceType = _sourceType;
}

public byte getMainOrder() { return (byte)15; }

public byte getSubOrder() { return (byte)50; }

/// <summary>
/// 空
/// </summary>
public Common.ConsortObj.Consort_Info getConsort() { return consort; }
/// <summary>
/// 空
/// </summary>
public void setConsort(Common.ConsortObj.Consort_Info _consort) { consort = _consort; }
/// <summary>
/// 来源类型
/// </summary>
public Common.ConsortEnum.EConsortSourceType getSourceType() { return sourceType; }
/// <summary>
/// 来源类型
/// </summary>
public void setSourceType(Common.ConsortEnum.EConsortSourceType _sourceType) { sourceType = _sourceType; }


public int GetBufSize() {
	int _size = 4;
	_size += 4 + consort.GetBufSize();

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + consort.GetBufSize();

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _consortCustLen = _buf.getInt();
	int _consortCurPos = _buf.getCurPos();
	consort.ReadUnzipBuf(_buf, _consortCurPos + _consortCustLen);
	_buf.setPosition(_consortCurPos + _consortCustLen);

	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	sourceType = (Common.ConsortEnum.EConsortSourceType)_buf.getInt();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(consort.GetBufSize());
	consort.PutUnzipBuf(_buf);
	_buf.putInt((int)sourceType);

}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)15);
	_buf.put((byte)50);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)15);
	_recBuf.put((byte)50);
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
	builder.Append("consort").Append(":").Append(consort == null ? "null" : consort.ToString()).Append(", ");
	builder.Append("sourceType").Append(":").Append(sourceType.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

