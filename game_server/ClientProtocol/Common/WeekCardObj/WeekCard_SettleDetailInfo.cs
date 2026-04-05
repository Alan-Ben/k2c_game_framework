using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;


namespace Common.WeekCardObj
{

/// <summary>
/// 周卡-详细结算信息
/// </summary>
public class WeekCard_SettleDetailInfo : ALBasicProtocolPack._IALProtocolStructure {
private CommonEnum.EWeekCardSettleType type;
private byte[] detailInfo;


public WeekCard_SettleDetailInfo() {
	type = 0;
	detailInfo = null;
}

public WeekCard_SettleDetailInfo(
	CommonEnum.EWeekCardSettleType _type
	, byte[] _detailInfo
) {	type = _type;
	detailInfo = _detailInfo;
}

public byte getMainOrder() { return (byte)0; }

public byte getSubOrder() { return (byte)0; }

public CommonEnum.EWeekCardSettleType getType() { return type; }
public void setType(CommonEnum.EWeekCardSettleType _type) { type = _type; }
public byte[] getDetailInfo() { return detailInfo; }

public void setDetailInfo(byte[] _detailInfo) { detailInfo = _detailInfo; }



public int GetBufSize() {
	int _size = 4;
	_size += 4 + (detailInfo == null ? 0 : detailInfo.Length);

	return _size;
}

public int GetFullPackBufSize() {
	int _size = 6;
	_size += 4 + (detailInfo == null ? 0 : detailInfo.Length);

	return _size;
}



public void ReadUnzipBuf(ALProtocolBuf _buf, int _finalPos) {
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	type = (CommonEnum.EWeekCardSettleType)_buf.getInt();
	 if(_finalPos > 0 && _buf.getCurPos() >= _finalPos) return ;
	detailInfo = _buf.getByteBuffer();

}

public void PutUnzipBuf(ALProtocolBuf _buf) {
	_buf.putInt((int)type);

	_buf.putByteBuffer(detailInfo);

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
	builder.Append("type").Append(":").Append(type.ToString()).Append(", ");
	builder.Append("detailInfo").Append(":").Append(detailInfo == null ? "null" : detailInfo.ToString()).Append(", ");
	builder.Append("}");
	return builder.ToString();
}

}

}

