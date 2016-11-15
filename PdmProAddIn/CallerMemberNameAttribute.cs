namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// This type is baked-in to .NET 4.5 and therefore this file should simply be removed if/when the project target is chnaged to .NET 4.5 or later.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public sealed class CallerMemberNameAttribute : Attribute
    {
    }
}
