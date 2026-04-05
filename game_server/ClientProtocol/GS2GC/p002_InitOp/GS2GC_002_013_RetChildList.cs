using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace GS2GC.p002_InitOp
{

/// <summary>
/// 子嗣初始化回包
/// </summary>
public class GS2GC_002_013_RetChildList : ALBasicProtocolPack._IALProtocolStructure {
/// <summary>
/// 子嗣数据列表
/// </summary>
private List<Common.ChildObj.Child_Info> childList;
/// <summary>
/// 子嗣训练位数据列表
/// </summary>
private List<Common.ChildObj.Child_SeatInfo> seatList;
/// <summary>
/// 子嗣（成年未婚）实例ID列表
/// </summary>
private List<Common.ChildObj.Adult_UnmarriedInfo> unmarriedAdultList;
/// <summary>
/// 子嗣（成年已婚）实例ID列表
/// </summary>
private List<long> marriedAdultIdList;
/// <summary>
/// 向玩家发起指定联姻请求数据列表
/// </summary>
private List<Common.ChildObj.Adult_ToMeApplyBaseInfo> applyToMeBaseList;
/// <summary>
/// 所有子嗣（成年/未成年）总收益
/// </summary>
private long bonus;
/// <summary>
/// 所有成年子嗣总收益
/// </summary>
private long adultBonus;


public GS2GC_002_013_RetChildList() {
	childList = new List<Common.ChildObj.Child_Info>();
	seatList = new List<Common.ChildObj.Child_SeatInfo>();
	unmarriedAdultList = new List<Common.ChildObj.Adult_UnmarriedInfo>();
	marriedAdultIdList = new List<long>();
	applyToMeBaseList = new List<Common.ChildObj.Adult_ToMeApplyBaseInfo>();
	bonus = (long)0;
	adultBonus = (long)0;
}

public GS2GC_002_013_RetChildList(
	List<Common.ChildObj.Child_Info> _childList
	, List<Common.ChildObj.Child_SeatInfo> _seatList
	, List<Common.ChildObj.Adult_UnmarriedInfo> _unmarriedAdultList
	, List<long> _marriedAdultIdList
	, List<Common.ChildObj.Adult_ToMeApplyBaseInfo> _applyToMeBaseList
	, long _bonus
	, long _adultBonus
) {	childList = _childList;
	seatList = _seatList;
	unmarriedAdultList = _unmarriedAdultList;
	marriedAdultIdList = _marriedAdultIdList;
	applyToMeBaseList = _applyToMeBaseList;
	bonus = _bonus;
	adultBonus = _adultBonus;
}

public byte getMainOrder() { return (byte)2; }

public byte getSubOrder() { return (byte)13; }

/// <summary>
/// 子嗣数据列表
/// </summary>
public List<Common.ChildObj.Child_Info> getChildList() { return childList; }
/// <summary>
/// 子嗣数据列表
/// </summary>
public void addChildList(Common.ChildObj.Child_Info _childList) { childList.Add(_childList); }
/// <summary>
/// 子嗣训练位数据列表
/// </summary>
public List<Common.ChildObj.Child_SeatInfo> getSeatList() { return seatList; }
/// <summary>
/// 子嗣训练位数据列表
/// </summary>
public void addSeatList(Common.ChildObj.Child_SeatInfo _seatList) { seatList.Add(_seatList); }
/// <summary>
/// 子嗣（成年未婚）实例ID列表
/// </summary>
public List<Common.ChildObj.Adult_UnmarriedInfo> getUnmarriedAdultList() { return unmarriedAdultList; }
/// <summary>
/// 子嗣（成年未婚）实例ID列表
/// </summary>
public void addUnmarriedAdultList(Common.ChildObj.Adult_UnmarriedInfo _unmarriedAdultList) { unmarriedAdultList.Add(_unmarriedAdultList); }
/// <summary>
/// 子嗣（成年已婚）实例ID列表
/// </summary>
public List<long> getMarriedAdultIdList() { return marriedAdultIdList; }
/// <summary>
/// 子嗣（成年已婚）实例ID列表
/// </summary>
public void addMarriedAdultIdList(long _marriedAdultIdList) { marriedAdultIdList.Add(_marriedAdultIdList); }
/// <summary>
/// 向玩家发起指定联姻请求数据列表
/// </summary>
public List<Common.ChildObj.Adult_ToMeApplyBaseInfo> getApplyToMeBaseList() { return applyToMeBaseList; }
/// <summary>
/// 向玩家发起指定联姻请求数据列表
/// </summary>
public void addApplyToMeBaseList(Common.ChildObj.Adult_ToMeApplyBaseInfo _applyToMeBaseList) { applyToMeBaseList.Add(_applyToMeBaseList); }
/// <summary>
/// 所有子嗣（成年/未成年）总收益
/// </summary>
public long getBonus() { return bonus; }
/// <summary>
/// 所有子嗣（成年/未成年）总收益
/// </summary>
public void setBonus(long _bonus) { bonus = _bonus; }
/// <summary>
/// 所有成年子嗣总收益
/// </summary>
public long getAdultBonus() { return adultBonus; }
/// <summary>
/// 所有成年子嗣总收益
/// </summary>
public void setAdultBonus(long _adultBonus) { adultBonus = _adultBonus; }


public int GetBufSize() {
	int _size = 16;
	_size += 2;
for(int _i = 0; _i < childList.Count; _i++) {
	_size += 4 + childList[_i].GetBufSize();
	}

	_size += 2 + (seatList.Count * 36);
	_size += 2;
for(int _i = 0; _i < unmarriedAdultList.Count; _i++) {
	_size += 4 + unmarriedAdultList[_i].GetBufSize();
	}

	_size += 2 + (marriedAdultIdList.Count * 8);
	_size += 2 + (applyToMeBaseList.Count * 16);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 18;
	_size += 2;
for(int _i = 0; _i < childList.Count; _i++) {
	_size += 4 + childList[_i].GetBufSize();
	}

	_size += 2 + (seatList.Count * 36);
	_size += 2;
for(int _i = 0; _i < unmarriedAdultList.Count; _i++) {
	_size += 4 + unmarriedAdultList[_i].GetBufSize();
	}

	_size += 2 + (marriedAdultIdList.Count * 8);
	_size += 2 + (applyToMeBaseList.Count * 16);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _childListCount = _buf.getShort();
	for(int _i = 0; _i < _childListCount; _i++) { 
		Common.ChildObj.Child_Info _childList = new Common.ChildObj.Child_Info();
		int __childListCustLen = _buf.getInt();
	int __childListCurPos = _buf.getCurPos();
	_childList.ReadUnzipBuf(_buf, __childListCurPos + __childListCustLen);
	_buf.setPosition(__childListCurPos + __childListCustLen);

		childList.Add(_childList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _seatListCount = _buf.getShort();
	for(int _i = 0; _i < _seatListCount; _i++) { 
		Common.ChildObj.Child_SeatInfo _seatList = new Common.ChildObj.Child_SeatInfo();
		int __seatListCustLen = _buf.getInt();
	int __seatListCurPos = _buf.getCurPos();
	_seatList.ReadUnzipBuf(_buf, __seatListCurPos + __seatListCustLen);
	_buf.setPosition(__seatListCurPos + __seatListCustLen);

		seatList.Add(_seatList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _unmarriedAdultListCount = _buf.getShort();
	for(int _i = 0; _i < _unmarriedAdultListCount; _i++) { 
		Common.ChildObj.Adult_UnmarriedInfo _unmarriedAdultList = new Common.ChildObj.Adult_UnmarriedInfo();
		int __unmarriedAdultListCustLen = _buf.getInt();
	int __unmarriedAdultListCurPos = _buf.getCurPos();
	_unmarriedAdultList.ReadUnzipBuf(_buf, __unmarriedAdultListCurPos + __unmarriedAdultListCustLen);
	_buf.setPosition(__unmarriedAdultListCurPos + __unmarriedAdultListCustLen);

		unmarriedAdultList.Add(_unmarriedAdultList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _marriedAdultIdListCount = _buf.getShort();
	for(int _i = 0; _i < _marriedAdultIdListCount; _i++) { 
		long _marriedAdultIdList = (long)0;
		_marriedAdultIdList = _buf.getLong();
		marriedAdultIdList.Add(_marriedAdultIdList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	short _applyToMeBaseListCount = _buf.getShort();
	for(int _i = 0; _i < _applyToMeBaseListCount; _i++) { 
		Common.ChildObj.Adult_ToMeApplyBaseInfo _applyToMeBaseList = new Common.ChildObj.Adult_ToMeApplyBaseInfo();
		int __applyToMeBaseListCustLen = _buf.getInt();
	int __applyToMeBaseListCurPos = _buf.getCurPos();
	_applyToMeBaseList.ReadUnzipBuf(_buf, __applyToMeBaseListCurPos + __applyToMeBaseListCustLen);
	_buf.setPosition(__applyToMeBaseListCurPos + __applyToMeBaseListCustLen);

		applyToMeBaseList.Add(_applyToMeBaseList);
	}
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	bonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	adultBonus = _buf.getLong();
}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putShort((short)childList.Count);
	for(int _i = 0; _i < childList.Count; _i++) { 
		_buf.putInt(childList[_i].GetBufSize());
	childList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)seatList.Count);
	for(int _i = 0; _i < seatList.Count; _i++) { 
		_buf.putInt(seatList[_i].GetBufSize());
	seatList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)unmarriedAdultList.Count);
	for(int _i = 0; _i < unmarriedAdultList.Count; _i++) { 
		_buf.putInt(unmarriedAdultList[_i].GetBufSize());
	unmarriedAdultList[_i].PutUnzipBuf(_buf);
	}
	_buf.putShort((short)marriedAdultIdList.Count);
	for(int _i = 0; _i < marriedAdultIdList.Count; _i++) { 
		_buf.putLong(marriedAdultIdList[_i]);
	}
	_buf.putShort((short)applyToMeBaseList.Count);
	for(int _i = 0; _i < applyToMeBaseList.Count; _i++) { 
		_buf.putInt(applyToMeBaseList[_i].GetBufSize());
	applyToMeBaseList[_i].PutUnzipBuf(_buf);
	}
	_buf.putLong(bonus);
	_buf.putLong(adultBonus);
}

public byte[] makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ALProtocolBuf _buf = ALProtocolBuf.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	return _buf.getBuf();
}
public void makeFullPackage(ALProtocolBuf _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)13);
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
	builder.Append("childList").Append(":").Append(childList.ToString()).Append(", ");
	builder.Append("seatList").Append(":").Append(seatList.ToString()).Append(", ");
	builder.Append("unmarriedAdultList").Append(":").Append(unmarriedAdultList.ToString()).Append(", ");
	builder.Append("marriedAdultIdList").Append(":").Append(marriedAdultIdList.ToString()).Append(", ");
	builder.Append("applyToMeBaseList").Append(":").Append(applyToMeBaseList.ToString()).Append(", ");
	builder.Append("bonus").Append(":").Append(bonus.ToString()).Append(", ");
	builder.Append("adultBonus").Append(":").Append(adultBonus.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

