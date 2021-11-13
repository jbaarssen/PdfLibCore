namespace PdfLibCore.Types
{
    public interface IHandle<out T>
    {
        bool IsNull { get; }

        T SetToNull();
    }
}