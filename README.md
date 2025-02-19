# PlcTagModifier

PlcTagModifier is a WPF application designed to read and write tags on a PLC (Programmable Logic Controller) using the libplctag library. The application demonstrates how to configure connection parameters, select a tag type, and perform read/write operations using an MVVM architecture.

## Features

- **PLC Connection**: Configure the PLC IP address, channel, slot, and port.
- **Tag Read/Write**: Read and write tag values using the libplctag managed API.
- **Tag Type Selection**: Supports multiple data types including:
  - Bit
  - Float32
  - Float64
  - Int8
  - Int16
  - Int32
  - Int64
  - UInt8
  - UInt16
  - UInt32
  - UInt64
  - String
- **Automatic Connection Monitoring**: A timer refreshes the connection status every few seconds.
- **MVVM Pattern**: Clean separation between UI and business logic.

## Prerequisites

- **.NET Framework / .NET Core**: This project is built as a WPF application. Ensure you have a compatible version installed.
- **libplctag**: Include the managed version of the libplctag library in your project. Make sure its DLL is available in the output folder.

## Installation and Build

1. **Clone the Repository**

   ```bash
   git clone https://github.com/jonavarro22/PlcTagModifier
   cd PlcTagModifier
   ```

2. **Open the Solution in Visual Studio**

   Open `PlcTagModifier.sln` in Visual Studio.

3. **Restore NuGet Packages**

   Restore any NuGet packages if required (including the managed libplctag library).

4. **Build the Project**

   Build the solution using Visual Studio or via command line:
   
   ```bash
   msbuild PlcTagModifier.sln
   ```

5. **Run the Application**

   Start debugging (F5) or run the built executable.

## Usage

1. **Configure PLC Connection**

   - Enter the **PLC IP**, **Channel**, and **Slot**.
   - The default tag name is loaded from the configuration but can be edited.

2. **Select Tag Type**

   - Use the provided dropdown (ComboBox) to select the tag type (e.g., `Int32`, `Float32`, etc.). This determines which getter/setter methods are used when reading or writing the tag.

3. **Connect to the PLC**

   - Click the **Connect** button to test connectivity. The connection status will update automatically (every 5 seconds) based on the connection check.

4. **Read/Write Tag Value**

   - Click **Read** to retrieve the current tag value.
   - Modify the tag value in the input field and click **Write** to update it on the PLC.

## Code Structure

- **MainWindow.xaml**:  
  Defines the UI layout including connection parameters, tag name, tag type selection, tag value, and action buttons.

- **MainWindowViewModel.cs**:  
  Contains the MVVM view model that exposes properties for connection settings, tag name, tag value, selected tag type, and the available tag types. It also implements the commands for reading, writing, and connecting to the PLC.

- **PlcTagCommunicator.cs**:  
  Provides static methods (`ReadTagAsync` and `WriteTagAsync`) for communicating with the PLC using the managed libplctag API. It supports multiple data types based on the tag type selected by the user.

- **PlcConfig.cs**:  
  Contains the configuration for the PLC connection, including default values for IP, port, channel, slot, default tag, timeout, and the default tag type.

## Troubleshooting

- **DLL Issues**:  
  Make sure the libplctag DLL is in your application's output directory.

- **Connection Failures**:  
  Verify that the PLC is reachable, and the network settings (IP, port, channel, slot) are correct.

- **Data Type Mismatches**:  
  Ensure the selected tag type matches the actual type of the tag on the PLC to avoid errors during read/write operations.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Acknowledgements

- **libplctag**: For providing the API to communicate with PLCs.

