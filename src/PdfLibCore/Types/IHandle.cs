namespace PdfLibCore.Types
{
    public interface IHandle<T>
    {
        bool IsNull { get; }

        T SetToNull();
    }
}