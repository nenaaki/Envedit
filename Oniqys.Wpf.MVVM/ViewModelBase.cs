using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Oniqys.Wpf.MVVM
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// クラスのフィールドを更新します。インスタンスが変化した場合に<see cref="INotifyPropertyChanged.PropertyChanged"/>を発行します。
        /// </summary>
        /// <returns>更新した場合 true、そうでない場合は false</returns>
        protected bool UpdateClassField<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null, string[] propertyNames = null)
            where T : class => field != newValue && UpdateFieldForce(ref field, newValue, propertyName, propertyNames);

        /// <summary>
        /// 構造体のフィールドを更新します。インスタンスが変化した場合に<see cref="INotifyPropertyChanged.PropertyChanged"/>を発行します。
        /// </summary>
        /// <returns>更新した場合 true、そうでない場合は false</returns>
        protected bool UpdateStructField<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null, string[] propertyNames = null)
            where T : struct, IEquatable<T> => !field.Equals(newValue) && UpdateFieldForce(ref field, newValue, propertyName, propertyNames);

        /// <summary>
        /// フィールドを更新し true を返します。
        /// </summary>
        /// <returns>常にtrue</returns>
        protected bool UpdateFieldForce<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null, string[] propertyNames = null)
        {
            field = newValue;
            OnPropertyChanged(propertyName);
            if (propertyNames != null)
            {
                foreach (string name in propertyNames)
                {
                    OnPropertyChanged(name);
                }
            }
            return true;
        }
    }
}
