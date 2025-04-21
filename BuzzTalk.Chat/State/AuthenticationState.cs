using System.ComponentModel;
using BuzzTalk.Chat.Models;

namespace BuzzTalk.Chat.State
{
    public class AuthenticationState : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public const string BuzzKey = "BuzzTalk";

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Token { get; set; }
        private bool _isAuthenticated;
        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            set
            {
                if (_isAuthenticated != value)
                {
                    _isAuthenticated = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAuthenticated)));
                }
            }
        }
        public void OnPropertyChanged(UserModel userModel)
        {
            Id = userModel.Id;
            Name = userModel.Name;
            Email = userModel.Email;
            Token = userModel.Token;
            Username = userModel.Username;
            IsAuthenticated = true;
        }
        public void OnPropertyChanged()
        {
            Name = string.Empty;
            Email = string.Empty;
            Token = string.Empty;
            IsAuthenticated = false;

        }
    }
}
