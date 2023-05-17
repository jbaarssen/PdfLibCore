// ReSharper disable all
namespace PdfLibCore.Types;

#pragma warning disable S101, S2342
public readonly struct FPDF_ERR
#pragma warning restore
{
    private readonly uint _value;

    public static readonly FPDF_ERR SUCCESS = new(0);
    public static readonly FPDF_ERR UNKNOWN = new(1);
    public static readonly FPDF_ERR FILE = new(2);
    public static readonly FPDF_ERR FORMAT = new(3);
    public static readonly FPDF_ERR PASSWORD = new(4);
    public static readonly FPDF_ERR SECURITY = new(5);
    public static readonly FPDF_ERR PAGE = new(6);
    public static readonly FPDF_ERR XFALOAD = new(7);
    public static readonly FPDF_ERR XFALAYOUT = new(8);

    private FPDF_ERR(uint value)
    {
        _value = value;
    }

    public static implicit operator uint(FPDF_ERR value) => value._value;
    public static implicit operator FPDF_ERR(uint value) => new(value);
    public static bool operator ==(FPDF_ERR lhs, FPDF_ERR rhs) => lhs._value == rhs._value;
    public static bool operator !=(FPDF_ERR lhs, FPDF_ERR rhs) => lhs._value != rhs._value;

    public override bool Equals(object obj) => obj is FPDF_ERR err && err == this;

    public override string ToString() => this;

    public override int GetHashCode() => _value.GetHashCode();

    public static implicit operator string(FPDF_ERR value) => value switch
    {
        {} when value == SUCCESS => "No error.",
        {} when value == UNKNOWN => "Unkown error.",
        {} when value == FILE => "File not found or could not be opened.",
        {} when value == FORMAT => "File not in PDF format or corrupted.",
        {} when value == PASSWORD => "Password required or incorrect password.",
        {} when value == SECURITY => "Unsupported security scheme.",
        {} when value == PAGE => "Page not found or content error.",
        {} when value == XFALOAD => "Load XFA error.",
        {} when value == XFALAYOUT => "Layout XFA error.",
        _ => $"{value} (No description available)."
    };
}