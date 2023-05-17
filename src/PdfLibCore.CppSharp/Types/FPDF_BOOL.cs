// ReSharper disable all
namespace PdfLibCore.Types;

#pragma warning disable S101, S2342
public readonly struct FPDF_BOOL
#pragma warning restore
{
    private readonly bool _value;

    public static FPDF_BOOL True => 1;
    public static FPDF_BOOL False => 0;

    private FPDF_BOOL(int value)
    {
        _value = value == 1;
    }

    public bool Equals(FPDF_BOOL other) => _value == other._value;

    public override bool Equals(object obj) => obj is FPDF_BOOL other && Equals(other);

    public override int GetHashCode() => _value.GetHashCode();

    public override string ToString() => ((bool)this).ToString();

    public static implicit operator bool(FPDF_BOOL input) => input._value;
    public static implicit operator int(FPDF_BOOL input) => input._value ? 1 : 0;
    public static implicit operator FPDF_BOOL(int input) => new (input);
    public static implicit operator FPDF_BOOL(bool input) => new (input ? 1 : 0);

    public static bool operator ==(FPDF_BOOL lhs, FPDF_BOOL rhs) => lhs._value == rhs._value;
    public static bool operator !=(FPDF_BOOL lhs, FPDF_BOOL rhs) => lhs._value != rhs._value;
}