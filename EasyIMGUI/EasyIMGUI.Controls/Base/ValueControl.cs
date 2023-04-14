using System;

namespace EasyIMGUI.Controls.Base
{
    /// <summary>
    /// Represents a <see cref="Control"/> that contains a <see cref="Value"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/>.</typeparam>
    public abstract class ValueControl<T> : Control
    {
        private T _Value = default;

        private Func<T> BindingValueGetter;

        private Action<T> BindingValueSetter;

        /// <summary>
        /// Is invoked everytime <see cref="Value"/> has been changed.
        /// The value it has been changed to is passed through the event args.
        /// <see cref="OnValueChanged"/> is still invoked even if the <see cref="Value"/> has been bounded.
        /// </summary>
        public event EventHandler<T> OnValueChanged;

        /// <summary>
        /// Determines if <see cref="Value"/> has been bounded to a Getter and Setter.
        /// </summary>
        public bool IsValueBinded => BindingValueSetter != null && BindingValueGetter != null;

        /// <summary>
        /// The value of a <see cref="ValueControl{T}"/>.
        /// Represents different property of a <see cref="Control"/> depending on the inheriter.
        /// </summary>
        public T Value
        {
            get => IsValueBinded ? BindingValueGetter.Invoke() : _Value;
            set
            {
                if (_Value == null || !_Value.Equals(value))
                {
                    if (IsValueBinded)
                    {
                        BindingValueSetter.Invoke(value);
                    }

                    _Value = value;
                    OnValueChanged?.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// Is used to bind a property or field to the <see cref="Value"/>.
        /// </summary>
        /// <param name="getter">The getter for <see cref="Value"/> to use.
        /// <code>() => MyProperty</code> or
        /// <code>() => Person.GetName()</code>
        /// are common use cases.
        /// </param>
        /// <param name="setter">The getter for <see cref="Value"/> to use.
        /// <code>x => MyProperty = x</code> or
        /// <code>x => Person.SetName(x)</code>
        /// are common use cases.
        /// </param>
        public void Bind(Func<T> getter, Action<T> setter)
        {
            BindingValueGetter = getter;
            BindingValueSetter = setter;
        }

        /// <summary>
        /// Unbinds any property or field from <see cref="Value"/> if there was any.
        /// </summary>
        public void Unbind()
        {
            BindingValueGetter = null;
            BindingValueSetter = null;
        }
    }
}
