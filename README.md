# Hamsta Kurakku (Hamster Combat Bot)

## Overview

Hamster Combat Bot is a tool designed to automate certain actions in the game Hamster Combat. It leverages the game's API to perform tasks such as sending ciphers, tapping, and buying upgrades, enhancing the gameplay experience through automation.

## Features

- **Automated Cipher Submission**: Periodically sends ciphers found in the specified directory to the game server (one can be found each day on Google).
- **Gameplay Automation**: Automates tapping, upgrading, and completing tasks within the game (upgrades are selected based on the best profit, calculated as cost per coin/hour).
- **Configuration via Environment Variables**: Uses a token for authentication and configuration, making it easy to set up and secure.

## Installation

1. **Clone the Repository**:
   ```sh
   git clone https://github.com/PopovNx/HamstaKurakku.git
   cd HamstaKurakku
   ```

2. **Build**:
  Ensure you have the .NET 8 SDK installed. Then, run the following commands:
    ```sh
    cd HamsterCrack
    dotnet publish -c Release -o out
    ```

3. **Set Up Environment Variables**:
   Ensure you have your `HAMSTER_TOKEN` ready and set it in your environment. This token is essential for authenticating with the Hamster Combat API.
  
4. **Run the Executable**:
   Navigate to the out folder and run the executable. Make sure to create the cipher folder beforehand.


### Running the Application using Docker

You can run the application using Docker. This is the recommended method as it encapsulates all dependencies and configurations.

1. **Run with Docker**:
   ```sh
   docker run --rm -e HAMSTER_TOKEN=your_hamster_token \
   -v ./ciphers:/app/cipher popovnx/bednihomyachok:0.0.2
   ```
   Replace `your_hamster_token` with your actual token and ensure the `ciphers` directory exists and contains the cipher files.

### Configuration

The application is configured via environment variables. The primary configuration required is the `HAMSTER_TOKEN`, which is used to authenticate API requests.

## Services

The project contains the following services:

- **DailyCipherService**: Periodically scans the `cipher` directory for new cipher files and sends them to the game server.
- **HamstaBackgroundService**: Automates various game actions such as tapping, buying upgrades, and completing tasks.

## Contributing

Contributions are welcome! Please fork the repository and submit pull requests. For major changes, please open an issue first to discuss what you would like to change.


## Contact

For any questions or support, please open an issue in the GitHub repository or contact the maintainer at [povanq@gmail.com](mailto:povanq@gmail.com).

---

Enjoy automating your Hamster Combat gameplay! 🐹🚀
