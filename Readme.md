# 🎮 Vive Controller Binding Setup (SteamVR Plugin Branch)

## ❗ Why this is important

This configuration file is **required for proper movement and interaction in the VR environment** when using the **SteamVR plugin branch** of the project. It defines **custom input bindings** for the HTC Vive controllers, enabling features such as:

- Walking/movement input  
- UI interaction

Without this configuration, the default SteamVR bindings **will not trigger** the necessary in-game actions, and the VR experience may not function correctly.

---

## 📁 Binding File

The binding configuration file is located at:
TFM_VR_Experimentation/Assets/InputActions/Binding/TFM_Final [Testing] configuration for Vive Controller.json


## Follow these steps to apply the correct controller configuration:

1. Connect your **HTC Vive headset and controllers**.
2. Launch **SteamVR** and ensure all devices are detected.
3. Start your Unity application (in **Play Mode** or a **standalone build**).
4. Open the **SteamVR Dashboard** (press the system button on a controller).
5. Navigate to:  
   `Devices → Controller Settings → Manage Controller Bindings`
6. Select your Unity application from the list.
7. Click **"Edit"** or **"Create New Binding"**.
8. In the binding editor:
   - Click **"Browse"** or **"Import"**.
   - Navigate to the file path:
     ```
     TFM_VR_Experimentation/Assets/InputActions/Binding/TFM_Final [Testing] configuration for Vive Controller.json
     ```
   - Select the file and confirm.
9. Click **"Save Personal Binding"** to apply it.
   - *(Optional: Click "Replace Default Binding" to make it the default on your system.)*
10. Close the bindings window.

---

✅ Your HTC Vive controllers are now correctly configured for movement and interaction in the VR environment.
