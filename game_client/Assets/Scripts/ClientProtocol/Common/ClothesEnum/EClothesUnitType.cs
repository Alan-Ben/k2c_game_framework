using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ALBasicProtocolPack;
using UnityEngine;


namespace Common.ClothesEnum
{

/// <summary>
/// 时装类型
/// </summary>
public enum EClothesUnitType {
	NONE, //0 ==== 
	[InspectorName("HAIRSTYLE - idx[1] - 发型")]
	HAIRSTYLE, //1 ==== 发型
	[InspectorName("DRESS - idx[2] - 连衣裙")]
	DRESS, //2 ==== 连衣裙
	[InspectorName("COAT - idx[3] - 外套")]
	COAT, //3 ==== 外套
	[InspectorName("TOP - idx[4] - 上衣")]
	TOP, //4 ==== 上衣
	[InspectorName("BOTTOMS - idx[5] - 下装")]
	BOTTOMS, //5 ==== 下装
	[InspectorName("SOCKS - idx[6] - 袜子")]
	SOCKS, //6 ==== 袜子
	[InspectorName("SHOES - idx[7] - 鞋子")]
	SHOES, //7 ==== 鞋子
	[InspectorName("ACCESSORIES - idx[8] - 饰品")]
	ACCESSORIES, //8 ==== 饰品
	[InspectorName("EYEBROW_MAKEUP - idx[9] - 眉妆")]
	EYEBROW_MAKEUP, //9 ==== 眉妆
	[InspectorName("COSMETIC_CONTACT_LENSES - idx[10] - 美瞳")]
	COSMETIC_CONTACT_LENSES, //10 ==== 美瞳
	[InspectorName("EYE_SHADOW - idx[11] - 眼影")]
	EYE_SHADOW, //11 ==== 眼影
	[InspectorName("EYE_LIINER - idx[12] - 眼线")]
	EYE_LIINER, //12 ==== 眼线
	[InspectorName("EYELASH - idx[13] - 睫毛")]
	EYELASH, //13 ==== 睫毛
	[InspectorName("ROUGE - idx[14] - 腮红")]
	ROUGE, //14 ==== 腮红
	[InspectorName("LIP_MAKEUP - idx[15] - 唇妆")]
	LIP_MAKEUP, //15 ==== 唇妆
	[InspectorName("FACE_TATTOOS - idx[16] - 纹面")]
	FACE_TATTOOS, //16 ==== 纹面
}

public class EClothesUnitTypeComparer : IEqualityComparer<EClothesUnitType>{
	public bool Equals(EClothesUnitType x, EClothesUnitType y) { return x == y; }
	public int GetHashCode(EClothesUnitType obj) { return obj.GetHashCode(); }
	public static int g_iEnumCount = 17;
}
}

