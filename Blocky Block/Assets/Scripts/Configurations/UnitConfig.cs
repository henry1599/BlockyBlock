using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockyBlock.Enums;
using RotaryHeart.Lib.SerializableDictionary;

namespace BlockyBlock.Configurations
{
    [CreateAssetMenu(fileName = "Unit3D Config", menuName = "Scriptable Object/Unit3D Config")]
    public class UnitConfig : ScriptableObject
    {
        public float StepDistance;
        public float MoveTime;
        public float EnterWaterTime;
        public float RotateTime;
        public Unit3DRotation Unit3DRotation;
        public DirectionData GetDataByDirection(UnitDirection _direction)
        {
            return this.Unit3DRotation[_direction];
        }
    }
    [System.Serializable]
    public class Unit3DRotation : SerializableDictionaryBase<UnitDirection, DirectionData> {}
    [System.Serializable]
    public class DirectionData
    {
        public Vector3 Rotation;
        public int XIdx;
        public int YIdx;
    }
}