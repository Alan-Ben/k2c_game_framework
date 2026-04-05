using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.WeekCardObj
{

/// <summary>
/// 周卡-游历结算信息
/// </summary>
public class WeekCard_SettleInfo_Travel : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 次数
/// </summary>
private int num;
/// <summary>
/// 已获得妃子列表
/// </summary>
private List<Common.WeekCardObj.WeekCard_TravelGotConsrtInfo> gotConsrtlist;
/// <summary>
/// 未获得妃子列表
/// </summary>
private List<Common.WeekCardObj.WeekCard_TravelUngetConsortInfo> ungetConsortlist;


public WeekCard_SettleInfo_Travel() {
	num = 0;
	gotConsrtlist = new List<Common.WeekCardObj.WeekCard_TravelGotConsrtInfo>();
	ungetConsortlist = new List<Common.WeekCardObj.WeekCard_TravelUngetConsortInfo>();
}

public WeekCard_SettleInfo_Travel(
	int _num
	, List<Common.WeekCardObj.WeekCard_TravelGotConsrtInfo> _gotConsrtlist
	, List<Common.WeekCardObj.WeekCard_TravelUngetConsortInfo> _ungetConsortlist
) {	num = _num;
	gotConsrtlist = _gotConsrtlist;
	ungetConsortlist = _ungetConsortlist;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 次数
/// </summary>
public int getNum() { return num; }
/// <summary>
/// 次数
/// </summary>
public void setNum(int _num) { num = _num; }
/// <summary>
/// 已获得妃子列表
/// </summary>
public List<Common.WeekCardObj.WeekCard_TravelGotConsrtInfo> getGotConsrtlist() { return gotConsrtlist; }
/// <summary>
/// 已获得妃子列表
/// </summary>
public void addGotConsrtlist(Common.WeekCardObj.WeekCard_TravelGotConsrtInfo _gotConsrtlist) { gotConsrtlist.Add(_gotConsrtlist); }
/// <summary>
/// 未获得妃子列表
/// </summary>
public List<Common.WeekCardObj.WeekCard_TravelUngetConsortInfo> getUngetConsortlist() { return ungetConsortlist; }
/// <summary>
/// 未获得妃子列表
/// </summary>
public void addUngetConsortlist(Common.WeekCardObj.WeekCard_TravelUngetConsortInfo _ungetConsortlist) { ungetConsortlist.Add(_ungetConsortlist); }


public int GetBufSize() {
	int _size = 4;
	_size += 2 + (gotConsrtlist.Count * 20);
	_size += 2 + (ungetConsortlist.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 2 + (gotConsrtlist.Count * 20);
	_size += 2 + (ungetConsortlist.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	num = _buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _gotConsrtlistCount = _buf.getShort();
	for(int _i = 0; _i < _gotConsrtlistCount; _i++) { 
		Common.WeekCardObj.WeekCard_TravelGotConsrtInfo _gotConsrtlist = new Common.WeekCardObj.WeekCard_TravelGotConsrtInfo();
		int __gotConsrtlistCustLen = _buf.getInt();
	int __gotConsrtlistCurPos = _buf.getCurPos();
	_gotConsrtlist.ReadUnzipBuf(_buf, __gotConsrtlistCurPos + __gotConsrtlistCustLen);
	_buf.setPosition(__gotConsrtlistCurPos + __gotConsrtlistCustLen);

		gotConsrtlist.Add(_gotConsrtlist);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _ungetConsortlistCount = _buf.getShort();
	for(int _i = 0; _i < _ungetConsortlistCount; _i++) { 
		Common.WeekCardObj.WeekCard_TravelUngetConsortInfo _ungetConsortlist = new Common.WeekCardObj.WeekCard_TravelUngetConsortInfo();
		int __ungetConsortlistCustLen = _buf.getInt();
	int __ungetConsortlistCurPos = _buf.getCurPos();
	_ungetConsortlist.ReadUnzipBuf(_buf, __ungetConsortlistCurPos + __ungetConsortlistCustLen);
	_buf.setPosition(__ungetConsortlistCurPos + __ungetConsortlistCustLen);

		ungetConsortlist.Add(_ungetConsortlist);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt(num);
	_buf.putShort((short)gotConsrtlist.Count);
	for(int _i = 0; _i < gotConsrtlist.Count; _i++) { 
		_buf.putInt(gotConsrtlist[_i].GetBufSize());
	gotConsrtlist[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)ungetConsortlist.Count);
	for(int _i = 0; _i < ungetConsortlist.Count; _i++) { 
		_buf.putInt(ungetConsortlist[_i].GetBufSize());
	ungetConsortlist[_i].PutUnzipBuf(_buf);
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
	builder.Append("num").Append(":").Append(num.ToString()).Append(", ");
	builder.Append("gotConsrtlist").Append(":").Append(gotConsrtlist.ToString()).Append(", ");
	builder.Append("ungetConsortlist").Append(":").Append(ungetConsortlist.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

