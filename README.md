# VR-in-construction-workplace

## How to Set Up the Project

### 1. Clone the Project
Clone the repository and open it in **Unity (editor version 6000.0.36f1)**

### 2. Import Required Packages
Make sure to import the following packages:
- [SimpliCity Construction Yard](https://assetstore.unity.com/packages/3d/environments/industrial/simplicity-construction-yard-72569)
- [Big Warehouse Pack](https://assetstore.unity.com/packages/3d/environments/industrial/big-warehouse-pack-96082)
- [Terrain Sample Asset Pack](https://assetstore.unity.com/packages/3d/environments/landscapes/terrain-sample-asset-pack-145808)
- [Obi Rope](https://assetstore.unity.com/packages/tools/physics/obi-rope-55579)

### 3. Fix Pink Prefabs caused by Missing Materials
If you did not have the packages downloaded beforehand, all prefabs will appear **pink** due to missing materials.

For both **SimpliCity Construction Yard** and **Big Warehouse** packages:
1. Navigate to the `Materials` folder
    - `Assets/SimpliCity_Construction_Yard/- Materials`    
OR
    - `Assets/IGBlocks/IG_Warehouse/Models/Materials`
2. Select all materials
3. Navigate to: **Edit → Rendering → Materials → Convert Selected Built-in Materials to URP**
    

> The prefabs should now display correctly in the scenes!! 🤯

### 4. Generate Lightmap UVs for SimpliCity Package
To correctly generate shadows for the **SimpliCity Construction Yard** package:
1. Navigate to the `Meshes` folder: 
    - `Assets/SimpliCity_Construction_Yard/- Meshes`
2. Select all meshes
3. Click on **Generate Lightmap UVs** in the inspector tab
4. Click **Apply**

> The project should now be fully set up and ready to use! 🚀
