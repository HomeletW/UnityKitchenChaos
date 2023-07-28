using System;

public interface IProgressTracked
{
    public event EventHandler<ProgressChangedArg> OnProgressChanged;
    public interface ProgressChangedArg
    {
        /// <summary>
        /// Return true if the bar should be active
        /// </summary>
        /// <returns></returns>
        public bool IsBarActive();

        /// <summary>
        /// Return true if the warning lable should be active
        /// </summary>
        /// <returns></returns>
        public bool IsWarning();
    }

    /// <summary>
    /// Return the current normalized progress (between 0~1)
    /// </summary>
    /// <returns></returns>
    public float GetNormalizedProgress();
}
