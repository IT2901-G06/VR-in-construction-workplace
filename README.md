# VR-in-construction-workplace

## How to Set Up the Project

### 1. Make sure Android Build Support is installed

How to install Android Build Support
1. Opening Unity Hub
2. Click "Installs"
3. Click the cogwheel on your unity installation
4. Click "Add modules"
5. Under platforms, check "Android Build Support", "OpenJDK" and "Android SDK & NDK Tools"
6. Click "Install"
7. Reopen the Unity editor if its already running

### 2. Clone the Project
Clone the repository and open it in **Unity (editor version 6000.0.36f1)**

### 3. Import Required Packages
Make sure to import the following packages:
- [SimpliCity Construction Yard](https://assetstore.unity.com/packages/3d/environments/industrial/simplicity-construction-yard-72569)
- [Big Warehouse Pack](https://assetstore.unity.com/packages/3d/environments/industrial/big-warehouse-pack-96082)
- [Terrain Sample Asset Pack](https://assetstore.unity.com/packages/3d/environments/landscapes/terrain-sample-asset-pack-145808)
- [Obi Rope](https://assetstore.unity.com/packages/tools/physics/obi-rope-55579)
- [VR Interaction Framework](https://assetstore.unity.com/packages/templates/systems/vr-interaction-framework-161066)

### 4. Fix Pink Prefabs caused by Missing Materials
If you did not have the packages downloaded beforehand, prefabs will appear **pink** due to missing materials.

For the **SimpliCity Construction Yard**, **Big Warehouse**, and **VR Interaction Framework** packages:
1. Navigate to the `Materials` folder
    - `Assets/SimpliCity_Construction_Yard/- Materials`    
OR
    - `Assets/IGBlocks/IG_Warehouse/Models/Materials`   
OR
    - `Assets\BNG Framework\Materials`
OR
    - `Assets\BNG Framework\Models\Hands`
2. Select all materials
3. Navigate to: **Edit → Rendering → Materials → Convert Selected Built-in Materials to URP**
    
The prefabs should now display correctly in the scenes!! 🤯

### 5. Generate Lightmap UVs for SimpliCity Package
To correctly generate shadows for the **SimpliCity Construction Yard** package:
1. Navigate to the `Meshes` folder: 
    - `Assets/SimpliCity_Construction_Yard/- Meshes`
2. Select all meshes
3. Click on **Generate Lightmap UVs** in the inspector tab
4. Click **Apply**

### 6. (Optional) Connect bHaptics developer account and API key

In order to feel haptic feedback through bHaptics equipment when using Unity, you must link your bHaptics developer portal account through an API key.

1. Create an account for the "bHaptics Developer Portal" [here](https://auth.bhaptics.com/login?success-url=https://developer.bhaptics.com/applications)
2. Follow the "Create the App" section from [this](https://docs.bhaptics.com/portal/app-and-event) guide.
3. Follow [this](https://docs.bhaptics.com/portal/deploy-the-app#create-api-key) guide in order to link your app. Follow the guide up to and including this section "Link to the Game".

After finishing these steps, the bHaptics equipment should work. To confirm this, add a new event in the developer portal, create a new deploy, then click bHaptics in the Unity application toolbar, then play the event from the popup window.

---

The project should now be fully set up and ready to use! 🚀

## How to run

> ⚠️ **Warning:** In order to use the bHaptics equipment, the running operating system MUST be Windows. The project however works fine without the equipment connected.

Although you are free to run any scenario directly, you should run the scene `MainMenu` to experience the full application.

### Using Meta Quest headset

> ⚠️ **Warning:** To use the headset you need to be running the Windows operating system.

1. Make sure you have Meta Quest Link downloaded.
2. Make sure your Quest 2/3 headset is connected to the link app, either through cable or via airlink. Quest 3 is preferred for hand tracking.
3. (Optional, but must for haptic feedback) Make sure you have bHaptics Player downloaded, and that the equipment is connected.

### Using Meta XR Simulator

1. Set the `Tracking Origin Type` according to the explanation in [Simulator crashing / showing black screen](#simulator-crashing--showing-black-screen).
2. Toggle the Meta XR Simulator button just to the left of the Unity play button.

## Common pitfalls

### Meta XR Simulator

The project requires the meta XR simulator in order to simulate the usage of the meta quest headsets. Throughout development we have found some issues with this simulator. This subsection highlights them.

#### Simulator crashing / showing black screen

This is caused by the OVRManager's tracking type being set to the wrong type based on the operating system used. This value can be changed by clicking the `OVRCameraRig` prefab, and adjusting `Tracking Origin Type` under the `OVRManager` script.  

If using:  
**Windows** - `Floor Level`  
**MacOS** - `Stage`

#### Simulator moving / clicking things uncontrollably

For some reason the simulator never fully closes between simulation runs. And for some mystical reason it decides to track all keyboard inputs even when the simulator window is not focused. These inputs are replayed when the simulator is opened automatically when pressing "Play" in Unity. 

The solution to this issue is to just let the simulator play out keyboard events. Then just exit the simulator and start the scene by clicking "Play" again. Alternatively you could also completely reopen Unity to force a complete restart of the simulator.