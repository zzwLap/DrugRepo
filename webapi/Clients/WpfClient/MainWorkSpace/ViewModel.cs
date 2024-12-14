using System;
using System.Text.RegularExpressions;
using System.Windows.Media;

public abstract class ViewModel : IDisposable
{
    public virtual string DisplayName { get; protected set; }
    public virtual string Glyph { get; set; }
    #region IDisposable Members
    public void Dispose()
    {
        OnDispose();
    }
    protected virtual void OnDispose() { }
#if DEBUG
    ~ViewModel()
    {
        string msg = string.Format("{0} ({1}) ({2}) Finalized", GetType().Name, DisplayName, GetHashCode());
        System.Diagnostics.Debug.WriteLine(msg);
    }
#endif
    #endregion 
}
