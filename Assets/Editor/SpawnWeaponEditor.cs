using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnWeapon))]
public class SpawnWeaponEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SpawnWeapon spawnWeapon= (SpawnWeapon)target;

        if (GUILayout.Button("Spawn Weapon Mesh")) spawnWeapon.SpawnMesh();
    }
}
