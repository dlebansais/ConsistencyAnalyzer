namespace ConsistencyAnalyzer
{
    /// <summary>
    /// Represents a state global to a program.
    /// </summary>
    /// <typeparam name="TState">The state type.</typeparam>
    public class GlobalState<TState>
    {
        /// <summary>
        /// Gets the current state.
        /// </summary>
        public TState? CurrentState { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether there are different states.
        /// </summary>
        public bool IsDifferent { get; private set; }

        /// <summary>
        /// Updates the state with a value.
        /// </summary>
        /// <param name="newState">The new state.</param>
        public void Update(TState newState)
        {
            lock (InternalLock)
            {
                if (CurrentState == null)
                    CurrentState = newState;
                else if (!IsDifferent)
                {
                    if (!CurrentState.Equals(newState))
                        IsDifferent = true;
                }
            }
        }

        private int[] InternalLock = new int[0];
    }
}
