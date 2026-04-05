package GS2GC.p002_InitOp;

import java.nio.ByteBuffer;
/*********
 * 子嗣初始化回包
 **/
public class GS2GC_002_013_RetChildList implements ALBasicProtocolPack._IALProtocolStructure {
/** 子嗣数据列表 */
private java.util.ArrayList<Common.ChildObj.Child_Info> childList;
/** 子嗣训练位数据列表 */
private java.util.ArrayList<Common.ChildObj.Child_SeatInfo> seatList;
/** 子嗣（成年未婚）实例ID列表 */
private java.util.ArrayList<Common.ChildObj.Adult_UnmarriedInfo> unmarriedAdultList;
/** 子嗣（成年已婚）实例ID列表 */
private java.util.ArrayList<Long> marriedAdultIdList;
/** 向玩家发起指定联姻请求数据列表 */
private java.util.ArrayList<Common.ChildObj.Adult_ToMeApplyBaseInfo> applyToMeBaseList;
/** 所有子嗣（成年/未成年）总收益 */
private long bonus;
/** 所有成年子嗣总收益 */
private long adultBonus;


public GS2GC_002_013_RetChildList() {
	childList = new java.util.ArrayList<Common.ChildObj.Child_Info>();
	seatList = new java.util.ArrayList<Common.ChildObj.Child_SeatInfo>();
	unmarriedAdultList = new java.util.ArrayList<Common.ChildObj.Adult_UnmarriedInfo>();
	marriedAdultIdList = new java.util.ArrayList<Long>();
	applyToMeBaseList = new java.util.ArrayList<Common.ChildObj.Adult_ToMeApplyBaseInfo>();
	bonus = (long)0;
	adultBonus = (long)0;
}

public GS2GC_002_013_RetChildList(
	 java.util.ArrayList<Common.ChildObj.Child_Info> _childList
	, java.util.ArrayList<Common.ChildObj.Child_SeatInfo> _seatList
	, java.util.ArrayList<Common.ChildObj.Adult_UnmarriedInfo> _unmarriedAdultList
	, java.util.ArrayList<Long> _marriedAdultIdList
	, java.util.ArrayList<Common.ChildObj.Adult_ToMeApplyBaseInfo> _applyToMeBaseList
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

public final byte getMainOrder() { return (byte)2; }

public final byte getSubOrder() { return (byte)13; }

/** 子嗣数据列表 */
public java.util.ArrayList<Common.ChildObj.Child_Info> getChildList() { return childList; }
/** 子嗣数据列表 */
public void addChildList(Common.ChildObj.Child_Info _childList) { childList.add(_childList); }
/** 子嗣训练位数据列表 */
public java.util.ArrayList<Common.ChildObj.Child_SeatInfo> getSeatList() { return seatList; }
/** 子嗣训练位数据列表 */
public void addSeatList(Common.ChildObj.Child_SeatInfo _seatList) { seatList.add(_seatList); }
/** 子嗣（成年未婚）实例ID列表 */
public java.util.ArrayList<Common.ChildObj.Adult_UnmarriedInfo> getUnmarriedAdultList() { return unmarriedAdultList; }
/** 子嗣（成年未婚）实例ID列表 */
public void addUnmarriedAdultList(Common.ChildObj.Adult_UnmarriedInfo _unmarriedAdultList) { unmarriedAdultList.add(_unmarriedAdultList); }
/** 子嗣（成年已婚）实例ID列表 */
public java.util.ArrayList<Long> getMarriedAdultIdList() { return marriedAdultIdList; }
/** 子嗣（成年已婚）实例ID列表 */
public void addMarriedAdultIdList(long _marriedAdultIdList) { marriedAdultIdList.add(_marriedAdultIdList); }
/** 向玩家发起指定联姻请求数据列表 */
public java.util.ArrayList<Common.ChildObj.Adult_ToMeApplyBaseInfo> getApplyToMeBaseList() { return applyToMeBaseList; }
/** 向玩家发起指定联姻请求数据列表 */
public void addApplyToMeBaseList(Common.ChildObj.Adult_ToMeApplyBaseInfo _applyToMeBaseList) { applyToMeBaseList.add(_applyToMeBaseList); }
/** 所有子嗣（成年/未成年）总收益 */
public long getBonus() { return bonus; }
/** 所有子嗣（成年/未成年）总收益 */
public void setBonus(long _bonus) { bonus = _bonus; }
/** 所有成年子嗣总收益 */
public long getAdultBonus() { return adultBonus; }
/** 所有成年子嗣总收益 */
public void setAdultBonus(long _adultBonus) { adultBonus = _adultBonus; }


public final int GetBufSize() {
	int _size = 16;
	_size += 2;
	for(int _i = 0; _i < childList.size(); _i++) {
	_size += 4 + childList.get(_i).GetBufSize();
	}

	_size += 2 + (seatList.size() * 36);
	_size += 2;
	for(int _i = 0; _i < unmarriedAdultList.size(); _i++) {
	_size += 4 + unmarriedAdultList.get(_i).GetBufSize();
	}

	_size += 2 + (marriedAdultIdList.size() * 8);
	_size += 2 + (applyToMeBaseList.size() * 16);

	return _size;
}

public final int GetFullPackBufSize() {
	int _size = 18;
	_size += 2;
	for(int _i = 0; _i < childList.size(); _i++) {
	_size += 4 + childList.get(_i).GetBufSize();
	}

	_size += 2 + (seatList.size() * 36);
	_size += 2;
	for(int _i = 0; _i < unmarriedAdultList.size(); _i++) {
	_size += 4 + unmarriedAdultList.get(_i).GetBufSize();
	}

	_size += 2 + (marriedAdultIdList.size() * 8);
	_size += 2 + (applyToMeBaseList.size() * 16);

	return _size;
}



public final void ReadUnzipBuf(ByteBuffer _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _childListCount = _buf.getShort();
	for(int _i = 0; _i < _childListCount; _i++) { 
		Common.ChildObj.Child_Info _childList = new Common.ChildObj.Child_Info();
		if(_buf.remaining() <= 0) return;
	int __childListCustLen = _buf.getInt();
	int __childListCurPos = _buf.position();
	_childList.ReadUnzipBuf(_buf, __childListCurPos + __childListCustLen);
	_buf.position(__childListCurPos + __childListCustLen);

		childList.add(_childList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _seatListCount = _buf.getShort();
	for(int _i = 0; _i < _seatListCount; _i++) { 
		Common.ChildObj.Child_SeatInfo _seatList = new Common.ChildObj.Child_SeatInfo();
		if(_buf.remaining() <= 0) return;
	int __seatListCustLen = _buf.getInt();
	int __seatListCurPos = _buf.position();
	_seatList.ReadUnzipBuf(_buf, __seatListCurPos + __seatListCustLen);
	_buf.position(__seatListCurPos + __seatListCustLen);

		seatList.add(_seatList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _unmarriedAdultListCount = _buf.getShort();
	for(int _i = 0; _i < _unmarriedAdultListCount; _i++) { 
		Common.ChildObj.Adult_UnmarriedInfo _unmarriedAdultList = new Common.ChildObj.Adult_UnmarriedInfo();
		if(_buf.remaining() <= 0) return;
	int __unmarriedAdultListCustLen = _buf.getInt();
	int __unmarriedAdultListCurPos = _buf.position();
	_unmarriedAdultList.ReadUnzipBuf(_buf, __unmarriedAdultListCurPos + __unmarriedAdultListCustLen);
	_buf.position(__unmarriedAdultListCurPos + __unmarriedAdultListCustLen);

		unmarriedAdultList.add(_unmarriedAdultList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _marriedAdultIdListCount = _buf.getShort();
	for(int _i = 0; _i < _marriedAdultIdListCount; _i++) { 
		long _marriedAdultIdList = (long)0;
		if(_buf.remaining() > 0) _marriedAdultIdList = _buf.getLong();
		marriedAdultIdList.add(_marriedAdultIdList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() <= 0) return;
	short _applyToMeBaseListCount = _buf.getShort();
	for(int _i = 0; _i < _applyToMeBaseListCount; _i++) { 
		Common.ChildObj.Adult_ToMeApplyBaseInfo _applyToMeBaseList = new Common.ChildObj.Adult_ToMeApplyBaseInfo();
		if(_buf.remaining() <= 0) return;
	int __applyToMeBaseListCustLen = _buf.getInt();
	int __applyToMeBaseListCurPos = _buf.position();
	_applyToMeBaseList.ReadUnzipBuf(_buf, __applyToMeBaseListCurPos + __applyToMeBaseListCustLen);
	_buf.position(__applyToMeBaseListCurPos + __applyToMeBaseListCustLen);

		applyToMeBaseList.add(_applyToMeBaseList);
	}
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) bonus = _buf.getLong();
	 if(_finalPos > 0 && _buf.position() >= _finalPos) return ;
	if(_buf.remaining() > 0) adultBonus = _buf.getLong();
}

public final void PutUnzipBuf(ByteBuffer _buf) {
	_buf.putShort((short)childList.size());
	for(int _i = 0; _i < childList.size(); _i++) { 
		_buf.putInt(childList.get(_i).GetBufSize());
	childList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)seatList.size());
	for(int _i = 0; _i < seatList.size(); _i++) { 
		_buf.putInt(seatList.get(_i).GetBufSize());
	seatList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)unmarriedAdultList.size());
	for(int _i = 0; _i < unmarriedAdultList.size(); _i++) { 
		_buf.putInt(unmarriedAdultList.get(_i).GetBufSize());
	unmarriedAdultList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putShort((short)marriedAdultIdList.size());
	for(int _i = 0; _i < marriedAdultIdList.size(); _i++) { 
		_buf.putLong(marriedAdultIdList.get(_i));
	}
	_buf.putShort((short)applyToMeBaseList.size());
	for(int _i = 0; _i < applyToMeBaseList.size(); _i++) { 
		_buf.putInt(applyToMeBaseList.get(_i).GetBufSize());
	applyToMeBaseList.get(_i).PutUnzipBuf(_buf);
	}
	_buf.putLong(bonus);
	_buf.putLong(adultBonus);
}

public final ByteBuffer makeFullPackage() {
	int _bufSize = GetBufSize() + 2;
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	_buf.put((byte)2);
	_buf.put((byte)13);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void makeFullPackage(ByteBuffer _recBuf) {
	if(null == _recBuf)
		return ;
	_recBuf.put((byte)2);
	_recBuf.put((byte)13);
	PutUnzipBuf(_recBuf);
}
public final ByteBuffer makePackage() {
	int _bufSize = GetBufSize();
	ByteBuffer _buf = ByteBuffer.allocate(_bufSize);
	PutUnzipBuf(_buf);
	_buf.flip();
	return _buf;
}
public final void readPackage(ByteBuffer _buf) {
	ReadUnzipBuf(_buf, -1);
}
}

