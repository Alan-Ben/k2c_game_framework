using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Hotfix.Common.HotSimpleActivityObj
{

/// <summary>
/// 万能活动商店信息
/// </summary>
public class RegularActivity_ShopInfo : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 下次刷新时间
/// </summary>
private long nextRefreshTimeMs;
/// <summary>
/// 购买记录列表
/// </summary>
private List<Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord> buyRecordList;


public RegularActivity_ShopInfo() {
	nextRefreshTimeMs = (long)0;
	buyRecordList = new List<Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord>();
}

public RegularActivity_ShopInfo(
	long _nextRefreshTimeMs
	, List<Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord> _buyRecordList
) {	nextRefreshTimeMs = _nextRefreshTimeMs;
	buyRecordList = _buyRecordList;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

/// <summary>
/// 下次刷新时间
/// </summary>
public long getNextRefreshTimeMs() { return nextRefreshTimeMs; }
/// <summary>
/// 下次刷新时间
/// </summary>
public void setNextRefreshTimeMs(long _nextRefreshTimeMs) { nextRefreshTimeMs = _nextRefreshTimeMs; }
/// <summary>
/// 购买记录列表
/// </summary>
public List<Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord> getBuyRecordList() { return buyRecordList; }
/// <summary>
/// 购买记录列表
/// </summary>
public void addBuyRecordList(Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord _buyRecordList) { buyRecordList.Add(_buyRecordList); }


public int GetBufSize() {
	int _size = 8;
	_size += 2 + (buyRecordList.Count * 20);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 10;
	_size += 2 + (buyRecordList.Count * 20);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	nextRefreshTimeMs = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _buyRecordListCount = _buf.getShort();
	for(int _i = 0; _i < _buyRecordListCount; _i++) { 
		Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord _buyRecordList = new Hotfix.Common.HotSimpleActivityObj.RegularActivity_ShopBuyRecord();
		int __buyRecordListCustLen = _buf.getInt();
	int __buyRecordListCurPos = _buf.getCurPos();
	_buyRecordList.ReadUnzipBuf(_buf, __buyRecordListCurPos + __buyRecordListCustLen);
	_buf.setPosition(__buyRecordListCurPos + __buyRecordListCustLen);

		buyRecordList.Add(_buyRecordList);
	}
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putLong(nextRefreshTimeMs);
	_buf.putShort((short)buyRecordList.Count);
	for(int _i = 0; _i < buyRecordList.Count; _i++) { 
		_buf.putInt(buyRecordList[_i].GetBufSize());
	buyRecordList[_i].PutUnzipBuf(_buf);
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
	builder.Append("nextRefreshTimeMs").Append(":").Append(nextRefreshTimeMs.ToString()).Append(", ");
	builder.Append("buyRecordList").Append(":").Append(buyRecordList.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

