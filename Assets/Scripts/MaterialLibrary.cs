using UnityEngine;

[System.Serializable]
public class BusColorMaterial
{
    public Material main;
    public Material dull;
}

[CreateAssetMenu(menuName = "Bus/Material Library")]
public class MaterialLibrary : ScriptableObject
{
    public BusColorMaterial Red;
    public BusColorMaterial Blue;
    public BusColorMaterial Green;
    public BusColorMaterial Orange;
    public BusColorMaterial Purple;
}