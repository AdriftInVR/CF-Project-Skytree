using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 0)]
public class GameData : ScriptableObject
{
    public string playerName;
    public int monedas;
    public float posX;
    public float posY;
    public float posZ;
    public Vector3 playerPosition;
}
