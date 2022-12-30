using System;

namespace HellTap.MeshDecimator.Math;

public struct SymmetricMatrix
{
	public double m0;

	public double m1;

	public double m2;

	public double m3;

	public double m4;

	public double m5;

	public double m6;

	public double m7;

	public double m8;

	public double m9;

	public double this[int index] => index switch
	{
		0 => m0, 
		1 => m1, 
		2 => m2, 
		3 => m3, 
		4 => m4, 
		5 => m5, 
		6 => m6, 
		7 => m7, 
		8 => m8, 
		9 => m9, 
		_ => throw new IndexOutOfRangeException(), 
	};

	public SymmetricMatrix(double c)
	{
		m0 = c;
		m1 = c;
		m2 = c;
		m3 = c;
		m4 = c;
		m5 = c;
		m6 = c;
		m7 = c;
		m8 = c;
		m9 = c;
	}

	public SymmetricMatrix(double m0, double m1, double m2, double m3, double m4, double m5, double m6, double m7, double m8, double m9)
	{
		this.m0 = m0;
		this.m1 = m1;
		this.m2 = m2;
		this.m3 = m3;
		this.m4 = m4;
		this.m5 = m5;
		this.m6 = m6;
		this.m7 = m7;
		this.m8 = m8;
		this.m9 = m9;
	}

	public SymmetricMatrix(double a, double b, double c, double d)
	{
		m0 = a * a;
		m1 = a * b;
		m2 = a * c;
		m3 = a * d;
		m4 = b * b;
		m5 = b * c;
		m6 = b * d;
		m7 = c * c;
		m8 = c * d;
		m9 = d * d;
	}

	public static SymmetricMatrix operator +(SymmetricMatrix a, SymmetricMatrix b)
	{
		return new SymmetricMatrix(a.m0 + b.m0, a.m1 + b.m1, a.m2 + b.m2, a.m3 + b.m3, a.m4 + b.m4, a.m5 + b.m5, a.m6 + b.m6, a.m7 + b.m7, a.m8 + b.m8, a.m9 + b.m9);
	}

	internal double Determinant1()
	{
		return m0 * m4 * m7 + m2 * m1 * m5 + m1 * m5 * m2 - m2 * m4 * m2 - m0 * m5 * m5 - m1 * m1 * m7;
	}

	internal double Determinant2()
	{
		return m1 * m5 * m8 + m3 * m4 * m7 + m2 * m6 * m5 - m3 * m5 * m5 - m1 * m6 * m7 - m2 * m4 * m8;
	}

	internal double Determinant3()
	{
		return m0 * m5 * m8 + m3 * m1 * m7 + m2 * m6 * m2 - m3 * m5 * m2 - m0 * m6 * m7 - m2 * m1 * m8;
	}

	internal double Determinant4()
	{
		return m0 * m4 * m8 + m3 * m1 * m5 + m1 * m6 * m2 - m3 * m4 * m2 - m0 * m6 * m5 - m1 * m1 * m8;
	}

	public double Determinant(int a11, int a12, int a13, int a21, int a22, int a23, int a31, int a32, int a33)
	{
		return this[a11] * this[a22] * this[a33] + this[a13] * this[a21] * this[a32] + this[a12] * this[a23] * this[a31] - this[a13] * this[a22] * this[a31] - this[a11] * this[a23] * this[a32] - this[a12] * this[a21] * this[a33];
	}
}
