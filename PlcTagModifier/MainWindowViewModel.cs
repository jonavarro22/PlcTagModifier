using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace PlcTagModifier
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        // Connection parameters
        private string _plcIp;
        public string PlcIp
        {
            get => _plcIp;
            set
            {
                if (_plcIp != value)
                {
                    _plcIp = value;
                    OnPropertyChanged(nameof(PlcIp));
                    ((RelayCommand)ReadCommand)?.RaiseCanExecuteChanged();
                    ((RelayCommand)WriteCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private int _channel;
        public int Channel
        {
            get => _channel;
            set
            {
                if (_channel != value)
                {
                    _channel = value;
                    OnPropertyChanged(nameof(Channel));
                    ((RelayCommand)ReadCommand)?.RaiseCanExecuteChanged();
                    ((RelayCommand)WriteCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private int _slot;
        public int Slot
        {
            get => _slot;
            set
            {
                if (_slot != value)
                {
                    _slot = value;
                    OnPropertyChanged(nameof(Slot));
                    ((RelayCommand)ReadCommand)?.RaiseCanExecuteChanged();
                    ((RelayCommand)WriteCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // Tag Name and Value
        private string _tagName;
        public string TagName
        {
            get => _tagName;
            set
            {
                if (_tagName != value)
                {
                    _tagName = value;
                    OnPropertyChanged(nameof(TagName));
                    ((RelayCommand)ReadCommand)?.RaiseCanExecuteChanged();
                    ((RelayCommand)WriteCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        private string _tagValue;
        public string TagValue
        {
            get => _tagValue;
            set
            {
                if (_tagValue != value)
                {
                    _tagValue = value;
                    OnPropertyChanged(nameof(TagValue));
                    ((RelayCommand)WriteCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        // New: Tag Type selection
        private string _tagType;
        public string TagType
        {
            get => _tagType;
            set
            {
                if (_tagType != value)
                {
                    _tagType = value;
                    OnPropertyChanged(nameof(TagType));
                }
            }
        }

        // Collection of available tag types for the ComboBox
        public ObservableCollection<string> TagTypes { get; }

        // Connection status
        private string _connectionStatus;
        public string ConnectionStatus
        {
            get => _connectionStatus;
            set
            {
                if (_connectionStatus != value)
                {
                    _connectionStatus = value;
                    OnPropertyChanged(nameof(ConnectionStatus));
                }
            }
        }

        // Commands
        public ICommand ReadCommand { get; }
        public ICommand WriteCommand { get; }
        public ICommand ConnectCommand { get; }

        // Configuration instance
        private PlcConfig _plcConfig;

        public MainWindowViewModel()
        {
            // Initialize commands first.
            ReadCommand = new RelayCommand(async _ => await ReadTagAsync(), _ => CanRead());
            WriteCommand = new RelayCommand(async _ => await WriteTagAsync(), _ => CanWrite());
            ConnectCommand = new RelayCommand(async _ => await ConnectAsync());

            // Initialize configuration with default values.
            _plcConfig = new PlcConfig();
            PlcIp = _plcConfig.PlcIpAddress;
            Channel = _plcConfig.Channel;
            Slot = _plcConfig.Slot;
            TagName = _plcConfig.DefaultTag;
            TagType = _plcConfig.TagType; // default tag type from configuration
            TagValue = string.Empty;
            ConnectionStatus = "Not connected";

            // Initialize available tag types.
            TagTypes = new ObservableCollection<string>
            {
                PlcTagType.Bit,
                PlcTagType.Float32,
                PlcTagType.Float64,
                PlcTagType.Int8,
                PlcTagType.Int16,
                PlcTagType.Int32,
                PlcTagType.Int64,
                PlcTagType.UInt8,
                PlcTagType.UInt16,
                PlcTagType.UInt32,
                PlcTagType.UInt64,
                PlcTagType.String
            };

            // Start automatic connection checking.
            StartAutoConnect();
        }

        private bool CanRead() =>
            !string.IsNullOrWhiteSpace(PlcIp) &&
            !string.IsNullOrWhiteSpace(TagName);

        private bool CanWrite() =>
            !string.IsNullOrWhiteSpace(PlcIp) &&
            !string.IsNullOrWhiteSpace(TagName) &&
            !string.IsNullOrWhiteSpace(TagValue);

        private async Task ReadTagAsync()
        {
            try
            {
                // Update configuration with current values.
                _plcConfig.PlcIpAddress = PlcIp;
                _plcConfig.Channel = Channel;
                _plcConfig.Slot = Slot;
                _plcConfig.TagType = TagType;

                string result = await PlcTagCommunicator.ReadTagAsync(_plcConfig, TagName);
                TagValue = result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading tag: " + ex.Message);
            }
        }

        private async Task WriteTagAsync()
        {
            try
            {
                _plcConfig.PlcIpAddress = PlcIp;
                _plcConfig.Channel = Channel;
                _plcConfig.Slot = Slot;
                _plcConfig.TagType = TagType;

                await PlcTagCommunicator.WriteTagAsync(_plcConfig, TagName, TagValue);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing tag: " + ex.Message);
            }
        }

        private async Task ConnectAsync()
        {
            try
            {
                ConnectionStatus = "Connecting...";
                _plcConfig.PlcIpAddress = PlcIp;
                _plcConfig.Channel = Channel;
                _plcConfig.Slot = Slot;
                _plcConfig.TagType = TagType;

                // Use DefaultTag for connection check.
                string result = await PlcTagCommunicator.ReadTagAsync(_plcConfig, _plcConfig.DefaultTag);
                ConnectionStatus = "Connected";
            }
            catch (Exception ex)
            {
                ConnectionStatus = $"Failed: {ex.Message}";
            }
        }

        private void StartAutoConnect()
        {
            var timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += async (s, e) => { await ConnectAsync(); };
            timer.Start();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
