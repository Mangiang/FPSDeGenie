using UnityEngine;
using System.Collections.Generic;


    public enum FontEnum{
        ARIAL,
    };
    public enum BasesEnum{
        BASE,
        ENNEMYBASE,
    };
    public enum FxEnum{
        BULLETHOLE,
        SMOKE,
    };
    public enum PlayerEnum{
        PLAYER,
    };
    public enum ResourcesEnum{
        RESOURCE,
    };
    public enum RobotsEnum{
        ALLYROBOT,
        ENNEMYROBOT,
    };
    public enum SpawnsEnum{
        ALLYBASESPAWN,
        ENNEMYBASESPAWN,
    };


public static class ResourcesData
{
    public static string[] _font;
    public static string[] _bases;
    public static string[] _fx;
    public static string[] _player;
    public static string[] _resources;
    public static string[] _robots;
    public static string[] _spawns;


    public static void Init()
    {
        _font = new string[]{
                                  "Font/ARIAL"};

        _bases = new string[]{
                                  "Objects/Bases/Base",
                                  "Objects/Bases/EnnemyBase"};

        _fx = new string[]{
                                  "Objects/FX/BulletHole",
                                  "Objects/FX/smoke"};

        _player = new string[]{
                                  "Objects/Player/Player"};

        _resources = new string[]{
                                  "Resource"};

        _robots = new string[]{
                                  "Objects/Robots/AllyRobot",
                                  "Objects/Robots/EnnemyRobot"};

        _spawns = new string[]{
                                  "Objects/Spawns/AllyBaseSpawn",
                                  "Objects/Spawns/EnnemyBaseSpawn"};

    }
    public static Object GetObject<T>(string[] arr, T enumValue)
    {
        string resourceName = arr[System.Convert.ToInt32(enumValue)];
        return Resources.Load(resourceName);
    }
}
