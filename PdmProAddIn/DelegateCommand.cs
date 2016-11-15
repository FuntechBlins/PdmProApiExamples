using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PdmProAddIn
{
    /// <summary>
    /// An <see cref="ICommand"/> whose delegates can be attached for <see cref="Execute"/> and <see cref="CanExecute"/>.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        public DelegateCommand(Action execute) : this(execute, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        public DelegateCommand(Action<object> execute) : this(execute, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        public DelegateCommand(Action execute, Func<bool> canExecute)
            : this(execute != null ? p => execute() : (Action<object>)null, canExecute != null ? p => canExecute() : (Func<object, bool>)null)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="execute">Delegate to execute when Execute is called on the command.</param>
        /// <param name="canExecute">Delegate to execute when CanExecute is called on the command.</param>
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            if (execute == null) { throw new ArgumentNullException("execute"); }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return canExecute != null ? canExecute(parameter) : true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to b e passed, this object can be set to null.</param>
        /// <exception cref="InvalidOperationException">The <see cref="CanExecute"/> method returns <c>false.</c></exception>
        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("The command cannot be executed because the canExecute action returned false.");
            }

            execute(parameter);
        }

        /// <summary>
        /// Raises the <see cref="E:CanExecuteChanged"/> event.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }

    /// <summary>
    /// Generic DelegateCommand
    /// </summary>
    /// <typeparam name="T">The type of the CommandParameter</typeparam>
    public class DelegateCommand<T> : ICommand where T : class
    {
        private readonly Action<T> execute;
        private readonly Func<T, bool> canExecute;

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecute != null ? canExecute(parameter as T) : true;
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to b e passed, this object can be set to null.</param>
        /// <exception cref="InvalidOperationException">The <see cref="CanExecute"/> method returns <c>false.</c></exception>
        public void Execute(object parameter)
        {
            T p = parameter as T;

            if (!CanExecute(p))
            {
                throw new InvalidOperationException("The command cannot be executed because the canExecute action returned false.");
            }

            execute(p);
        }

        /// <summary>
        /// Raises the <see cref="E:CanExecuteChanged"/> event.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
