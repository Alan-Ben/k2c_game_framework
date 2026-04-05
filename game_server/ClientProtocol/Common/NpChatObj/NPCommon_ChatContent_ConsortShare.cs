using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.NpChatObj
{

/// <summary>
/// 妃子分享
/// </summary>
public class NPCommon_ChatContent_ConsortShare : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 妃子id
/// </summary>
private long consortId;
/// <summary>
/// 皮肤id
/// </summary>
private long skinId;
/// <summary>
/// 魅力
/// </summary>
private long charm;
/// <summary>
/// 亲密度
/// </summary>
private long intimacy;
/// <summary>
/// 羁绊
/// </summary>
private Common.ConsortObj.Consort_Fetters consortFetters;


public NPCommon_ChatContent_ConsortShare() {
	consortId = (long)0;
	skinId = (long)0;
	charm = (long)0;
	intimacy = (long)0;
	consortFetters = new Common.ConsortObj.Consort_Fetters();
}

public NPCommon_ChatContent_ConsortShare(
	long _consortId
	, long _skinId
	, long _charm
	, long _intimacy
	, Common.ConsortObj.Consort_Fetters _consortFetters
) {	consortId = _consortId;
	skinId = _skinId;
	charm = _charm;
	intimacy = _intimacy;
	consortFetters = _consortFetters;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 妃子id
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 妃子id
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
/// <summary>
/// 皮肤id
/// </summary>
public long getSkinId() { return skinId; }
/// <summary>
/// 皮肤id
/// </summary>
public void setSkinId(long _skinId) { skinId = _skinId; }
/// <summary>
/// 魅力
/// </summary>
public long getCharm() { return charm; }
/// <summary>
/// 魅力
/// </summary>
public void setCharm(long _charm) { charm = _charm; }
/// <summary>
/// 亲密度
/// </summary>
public long getIntimacy() { return intimacy; }
/// <summary>
/// 亲密度
/// </summary>
public void setIntimacy(long _intimacy) { intimacy = _intimacy; }
/// <summary>
/// 羁绊
/// </summary>
public Common.ConsortObj.Consort_Fetters getConsortFetters() { return consortFetters; }
/// <summary>
/// 羁绊
/// </summary>
public void setConsortFetters(Common.ConsortObj.Consort_Fetters _consortFetters) { consortFetters = _consortFetters; }


public int GetBufSize() {
	int _size = 40;

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 42;

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	skinId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	charm = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	intimacy = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	int _consortFettersCustLen = _buf.getInt();
	int _consortFettersCurPos = _buf.getCurPos();
	consortFetters.ReadUnzipBuf(_buf, _consortFettersCurPos + _consortFettersCustLen);
	_buf.setPosition(_consortFettersCurPos + _consortFettersCustLen);

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putLong(skinId);
	_buf.putLong(charm);
	_buf.putLong(intimacy);
	_buf.putInt(consortFetters.GetBufSize());
	consortFetters.PutUnzipBuf(_buf);
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
	builder.Append("skinId").Append(":").Append(skinId.ToString()).Append(", ");
	builder.Append("charm").Append(":").Append(charm.ToString()).Append(", ");
	builder.Append("intimacy").Append(":").Append(intimacy.ToString()).Append(", ");
	builder.Append("consortFetters").Append(":").Append(consortFetters == null ? "null" : consortFetters.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

