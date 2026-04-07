namespace SaaSUniversity.Client.Service
{
    public class UserState
    {
        public string? Email { get; private set; }
        public bool IsLoggedIn => !string.IsNullOrEmpty(Email);

        public event Action? OnChange;

        public void SetEmail(string email)
        {
            Email = email;
            NotifyStateChanged();
        }

        public void Clear()
        {
            Email = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }


}
