namespace PdfLibCore.Enums
{
    /// <summary>
    /// PDF object types
    /// </summary>
    public enum ObjectTypes
    {
        Unknown = 0,
        Boolean = 1,
        Number = 2,
        String = 3,
        Name = 4,
        Array = 5,
        Dictionary = 6,
        Stream = 7,
        Nullobj = 8,
        Reference = 9
    }
}