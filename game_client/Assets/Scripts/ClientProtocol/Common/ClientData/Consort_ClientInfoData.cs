using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.ClientData
{

public class Consort_ClientInfoData : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 情人id
/// </summary>
private long consortId;
private List<long> lookedSkinIdList;
private List<long> lookedExperienceIdList;


public Consort_ClientInfoData() {
	consortId = (long)0;
	lookedSkinIdList = new List<long>();
	lookedExperienceIdList = new List<long>();
}

public Consort_ClientInfoData(
	long _consortId
	, List<long> _lookedSkinIdList
	, List<long> _lookedExperienceIdList
) {	consortId = _consortId;
	lookedSkinIdList = _lookedSkinIdList;
	lookedExperienceIdList = _lookedExperienceIdList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 情人id
/// </summary>
public long getConsortId() { return consortId; }
/// <summary>
/// 情人id
/// </summary>
public void setConsortId(long _consortId) { consortId = _consortId; }
public List<long> getLookedSkinIdList() { return lookedSkinIdList; }
public void addLookedSkinIdList(long _lookedSkinIdList) { lookedSkinIdList.Add(_lookedSkinIdList); }
public List<long> getLookedExperienceIdList() { return lookedExperienceIdList; }
public void addLookedExperienceIdList(long _lookedExperienceIdList) { lookedExperienceIdList.Add(_lookedExperienceIdList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (lookedSkinIdList.Count * 8);
	_size += 2 + (lookedExperienceIdList.Count * 8);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (lookedSkinIdList.Count * 8);
	_size += 2 + (lookedExperienceIdList.Count * 8);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	consortId = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _lookedSkinIdListCount = _buf.getShort();
	for(int _i = 0; _i < _lookedSkinIdListCount; _i++) { 
		long _lookedSkinIdList = (long)0;
		_lookedSkinIdList = _buf.getLong();
		lookedSkinIdList.Add(_lookedSkinIdList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _lookedExperienceIdListCount = _buf.getShort();
	for(int _i = 0; _i < _lookedExperienceIdListCount; _i++) { 
		long _lookedExperienceIdList = (long)0;
		_lookedExperienceIdList = _buf.getLong();
		lookedExperienceIdList.Add(_lookedExperienceIdList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(consortId);
	_buf.putShort((short)lookedSkinIdList.Count);
	for(int _i = 0; _i < lookedSkinIdList.Count; _i++) { 
		_buf.putLong(lookedSkinIdList[_i]);
	}
	_buf.putShort((short)lookedExperienceIdList.Count);
	for(int _i = 0; _i < lookedExperienceIdList.Count; _i++) { 
		_buf.putLong(lookedExperienceIdList[_i]);
	}
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
	builder.Append("lookedSkinIdList").Append(":").Append(lookedSkinIdList.ToString()).Append(", ");
	builder.Append("lookedExperienceIdList").Append(":").Append(lookedExperienceIdList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

