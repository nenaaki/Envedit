using System.Windows;

namespace Oniqys.Wpf
{
    /// <summary>
    /// FrameworkElementにアタッチ可能なビヘイビアのインターフェースです。
    /// </summary>
    /// <typeparam name="T">FrameworkElementを継承した型</typeparam>
    public interface IBehavior<T>
        where T : FrameworkElement
    {
        public void Attach(T owner);

        public void Detach();
    }
}
