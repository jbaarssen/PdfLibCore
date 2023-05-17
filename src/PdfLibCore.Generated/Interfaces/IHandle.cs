// ReSharper disable once CheckNamespace
namespace PdfLibCore.Generated;

public interface IHandle<out T>
{
    bool IsNull { get; }

    T SetToNull();
}