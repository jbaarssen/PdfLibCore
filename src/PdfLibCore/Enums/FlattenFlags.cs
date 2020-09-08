namespace PdfLibCore.Enums
{
    public enum FlattenFlags
    {
        /// <summary>
        /// Flatten for normal display.
        /// </summary>
        NormalDisplay = 0,

        /// <summary>
        /// Flatten for print.
        /// </summary>
        Print = 1
    }

    public enum FlattenResults
    {
        /// <summary>
        /// Flatten operation failed.
        /// </summary>
        Fail = 0,

        /// <summary>
        /// Flatten operation succeed.
        /// </summary>
        Success = 1,

        /// <summary>
        /// Nothing to be flattened.
        /// </summary>
        NothingToDo = 2
    }
}