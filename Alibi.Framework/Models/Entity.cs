using System;

namespace Alibi.Framework.Models
{
    public class Entity<T> : IDisposable
    {
        public virtual T Id { get; set; }

        private bool IsDisposed { get; set; } = false;

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            IsDisposed = true;
        }
    }


    public class Entity : Entity<int>
    {
    }
}